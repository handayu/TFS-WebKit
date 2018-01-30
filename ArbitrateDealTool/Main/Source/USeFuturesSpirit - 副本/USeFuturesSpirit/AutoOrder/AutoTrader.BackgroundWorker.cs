using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Diagnostics;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 自动下单机--后台工作任务。
    /// </summary>
    public partial class AutoTrader
    {
        #region member
        private BackgroundWorker m_worker = null;  //工作线程
        #endregion

        #region 开平仓
        /// <summary>
        /// 开仓/平仓监控下单。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerDoWorkForOpenAndClose(object sender, DoWorkEventArgs e)
        {
            string text = string.Format("启动跟单流程,当前状态为{0}", this.ArbitrageOrderState.ToDescription());
            AutoTraderNotice notice = CreateTraderNotice(text);
            SafeFireAutoTraderNotice(notice);
            WriteTraderNoticeLog(notice);

            SafeFireAutoTraderStateChanged(AutoTraderWorkType.OpenOrClose, AutoTraderState.Enable);

            lock (m_syncObj)
            {
                m_arbitrageOrder.ResetTryOrderErrorCount();
            }
            while (true)
            {
                ArbitrageOrderState arbitrageOrderState = ArbitrageOrderState.None;

                lock (m_syncObj)
                {
                    if (m_backgroundRunFlag == false) break;  // 退出跟单
                    arbitrageOrderState = m_arbitrageOrder.State;
                }

                switch (arbitrageOrderState)
                {
                    case ArbitrageOrderState.Opening:
                        //建仓中
                        ProcessArbitrageOrderOpenTask();
                        break;
                    case ArbitrageOrderState.Closeing:
                        ProcessArbitrageOrderCloseTask();
                        //平仓中
                        break;
                    case ArbitrageOrderState.None:
                        //无下一步指示,等待
                        m_operatorEvent.WaitOne(EVENT_WAIT_Time);
                        break;
                    case ArbitrageOrderState.Opened:
                        BeginClose();
                        return;
                    case ArbitrageOrderState.Closed:
                        //已结束，流程退出
                        return;
                    default:
                        Debug.Assert(false, "未知类型的套利单状态");
                        return;
                }
            }
        }

        /// <summary>
        /// 开平仓流程结束。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerForOpenAndCloseCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (m_worker != null)
            {
                m_worker.ProgressChanged -= WorkerForOpenAndCloseProgressChanged;
                m_worker.RunWorkerCompleted -= WorkerForOpenAndCloseCompleted;
                m_worker.DoWork -= WorkerDoWorkForOpenAndClose;

                m_worker = null;
            }

            //流程结束，置位运行标识
            m_backgroundRunFlag = false;
            m_backgroundWorkerType = AutoTraderWorkType.None;

            string text = "下单机已停止";
            AutoTraderNotice notice = CreateTraderNotice(text);
            SafeFireAutoTraderNotice(notice);
            WriteTraderNoticeLog(notice);

            SafeFireAutoTraderStateChanged(AutoTraderWorkType.None, AutoTraderState.Disable);
        }

        /// <summary>
        /// 开平仓进度报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerForOpenAndCloseProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        #endregion

        #region 开仓对齐
        /// <summary>
        /// 开仓对齐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerDoWorkForOpenAlignment(object sender, DoWorkEventArgs e)
        {
            {
                AutoTraderNotice notice = CreateTraderNotice("开仓对齐操作");
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }

            //开仓对齐
            //1.撤销掉当前所有未成交委托单
            //2.检查不对齐过剩仓位，对过剩仓位进行反向平仓处理
            //3.方向平仓以市价委托，尽量保证成交
            Debug.Assert(m_arbitrageOrder.State == ArbitrageOrderState.Opening);

            if (m_arbitrageOrder.HasUnFinishOrderBook)
            {
                string text = "开仓对齐有未完成委托单正在撤单";
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);

                List<USeOrderBook> unFinishOrderBook = m_arbitrageOrder.GetAllUnfinishOrderBooks();
                foreach (USeOrderBook orderBook in unFinishOrderBook)
                {
                    string errorMessage = string.Empty;
                    bool cancelResult = m_orderDriver.CancelOrder(orderBook.OrderNum, orderBook.Instrument, out errorMessage);
                    if (cancelResult == false)
                    {
                        text = string.Format("开仓对齐{0} 撤单失败", orderBook.OrderNum);
                        notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                        SafeFireAutoTraderNotice(notice);
                        WriteTraderNoticeLog(notice);

                        return;   // 退出流程，触发预警
                    }
                }

                text = "开仓对齐撤单操作完成，等待撤单成功";
                notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);

                while (true)
                {
                    //等待撤单完成
                    m_operatorEvent.WaitOne(EVENT_WAIT_Time);

                    if (m_backgroundRunFlag == false)
                    {
                        text = string.Format("开仓对齐流程退出");
                        notice = CreateTraderNotice(AutoTraderNoticeType.Infomation, text);
                        SafeFireAutoTraderNotice(notice);
                        WriteTraderNoticeLog(notice);
                        return;
                    }

                    if (m_arbitrageOrder.HasUnFinishOrderBook == false)
                    {
                        text = string.Format("开仓对齐已完成撤单");
                        notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                        SafeFireAutoTraderNotice(notice);
                        WriteTraderNoticeLog(notice);
                        break;
                    }
                }
            }


            List<OrderCommand> commandList = new List<OrderCommand>();
            // 对过剩合约反向平仓，逐个任务平仓，避免仓位计算错误
            foreach (ArbitrageTask task in m_arbitrageOrder.OpenTaskGroup.TaskList)
            {
                if (task.FirstSubTask.TradeQty == task.SecondSubTask.TradeQty) continue;

                Debug.Assert(task.FirstSubTask.TradeQty > task.SecondSubTask.TradeQty);

                //反向平仓
                USeOrderSide orderSide = task.FirstSubTask.OrderSide.GetOppositeOrderSide();
                decimal orderPrice = GetFirstInstrumentOrderPrice(task.FirstSubTask.Instrument, ArbitrageOrderPriceType.OpponentPrice, orderSide);
                Debug.Assert(orderPrice > 0);
                int orderQty = task.FirstSubTask.TradeQty - task.SecondSubTask.TradeQty;
                Debug.Assert(orderQty > 0);
                USeInstrument instrument = task.FirstSubTask.Instrument;
                USeOffsetType offsetType = USeOffsetType.Close;  //[yangming]暂时定为平仓

                List<OrderCommand> subCommandList = CreateOrderCommands(task.TaskId, instrument, orderSide, offsetType, orderQty, orderPrice, "开仓对齐");
                foreach (OrderCommand command in subCommandList)
                {
                    bool orderResult = PlaceOrderForOrderCommand(command);
                    if (orderResult)
                    {
                        task.FirstSubTask.AddNegativeOrderBook(command.CreateOrignalOrderBook());
                        task.UpdateTaskState();
                        //task.TaskState = ArbitrageTaskState.FirstPlaceOrder;
                    }
                }
                commandList.AddRange(subCommandList);
            }

            #region 通知
            foreach (OrderCommand command in commandList)
            {
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, command.ToDescription());
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
            #endregion

            {
                string text = "开仓对齐已完成下单，等待成交";
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
        }

        /// <summary>
        /// 开仓对齐流程结束。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerForOpenAlignmentCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (m_worker != null)
            {
                m_worker.ProgressChanged -= WorkerForOpenAlignmentProgressChanged;
                m_worker.RunWorkerCompleted -= WorkerForOpenAlignmentCompleted;
                m_worker.DoWork -= WorkerDoWorkForOpenAlignment;

                m_worker = null;
            }

            //流程结束，置位运行标识
            m_backgroundRunFlag = false;
            m_backgroundWorkerType = AutoTraderWorkType.None;

            string text = "开仓对齐流程结束";
            AutoTraderNotice notice = CreateTraderNotice(text);
            SafeFireAutoTraderNotice(notice);
            WriteTraderNoticeLog(notice);

            SafeFireAutoTraderStateChanged(AutoTraderWorkType.None, AutoTraderState.Disable);
        }

        /// <summary>
        /// 开仓对齐进度报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerForOpenAlignmentProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        #endregion

        #region 开仓追单
        /// <summary>
        /// 开仓追单。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerDoWorkForOpenChaseOrder(object sender, DoWorkEventArgs e)
        {
            {
                AutoTraderNotice notice = CreateTraderNotice("开仓追单操作");
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }

            //开仓追单
            //1.撤销掉当前所有未成交委托单
            //2.检查不对齐欠缺仓位，对欠缺仓位进行开仓追单
            //3.开仓以市价委托，尽量保证成交
            Debug.Assert(m_arbitrageOrder.State == ArbitrageOrderState.Opening);

            if (m_arbitrageOrder.HasUnFinishOrderBook)
            {
                string text = "开仓追单有未完成委托单正在撤单";
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);

                List<USeOrderBook> unFinishOrderBook = m_arbitrageOrder.GetAllUnfinishOrderBooks();
                foreach (USeOrderBook orderBook in unFinishOrderBook)
                {
                    string errorMessage = string.Empty;
                    bool cancelResult = m_orderDriver.CancelOrder(orderBook.OrderNum, orderBook.Instrument, out errorMessage);
                    if (cancelResult == false)
                    {
                        text = string.Format("开仓追单{0} 撤单失败", orderBook.OrderNum);
                        notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                        SafeFireAutoTraderNotice(notice);
                        WriteTraderNoticeLog(notice);

                        return;   // 退出流程，触发预警
                    }
                }

                text = "开仓追单操作完成，等待撤单成功";
                notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);

                while (true)
                {
                    //等待撤单完成
                    m_operatorEvent.WaitOne(EVENT_WAIT_Time);

                    if (m_backgroundRunFlag == false)
                    {
                        text = string.Format("开仓追单流程退出");
                        notice = CreateTraderNotice(AutoTraderNoticeType.Infomation, text);
                        SafeFireAutoTraderNotice(notice);
                        WriteTraderNoticeLog(notice);
                        return;
                    }

                    if (m_arbitrageOrder.HasUnFinishOrderBook == false)
                    {
                        text = string.Format("开仓追单已完成撤单");
                        notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                        SafeFireAutoTraderNotice(notice);
                        WriteTraderNoticeLog(notice);
                        break;
                    }
                }
            }

            List<OrderCommand> commandList = new List<OrderCommand>();
            // 对欠缺仓位进行补单
            foreach (ArbitrageTask task in m_arbitrageOrder.OpenTaskGroup.TaskList)
            {
                if (task.FirstSubTask.TradeQty == task.SecondSubTask.TradeQty) continue;

                Debug.Assert(task.FirstSubTask.TradeQty > task.SecondSubTask.TradeQty);

                //追单
                USeInstrument instrument = task.SecondSubTask.Instrument;
                USeOrderSide orderSide = task.SecondSubTask.OrderSide;
                int orderQty = task.FirstSubTask.TradeQty - task.SecondSubTask.TradeQty;
                Debug.Assert(orderQty > 0);
                decimal orderPrice = GetFirstInstrumentOrderPrice(instrument, ArbitrageOrderPriceType.OpponentPrice, orderSide);
                Debug.Assert(orderPrice > 0);
                USeOffsetType offsetType = USeOffsetType.Open;

                List<OrderCommand> subCommandList = CreateOrderCommands(task.TaskId, instrument, orderSide, offsetType, orderQty, orderPrice, "开仓追单");
                foreach (OrderCommand command in subCommandList)
                {
                    bool orderResult = PlaceOrderForOrderCommand(command);
                    if (orderResult)
                    {
                        task.SecondSubTask.AddPositiveOrderBook(command.CreateOrignalOrderBook());
                        task.UpdateTaskState();
                        //task.TaskState = ArbitrageTaskState.FirstPlaceOrder;
                    }
                }

                commandList.AddRange(subCommandList);
            }

            #region 通知
            foreach (OrderCommand command in commandList)
            {
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, command.ToDescription());
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
            #endregion

            {
                string text = "开仓追单已完成追单，等待成交";
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
        }

        /// <summary>
        /// 开仓追单完成。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerForOpenChaseOrderCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (m_worker != null)
            {
                m_worker.ProgressChanged -= WorkerForOpenChaseOrderProgressChanged;
                m_worker.RunWorkerCompleted -= WorkerForOpenChaseOrderCompleted;
                m_worker.DoWork -= WorkerDoWorkForOpenChaseOrder;

                m_worker = null;
            }

            //流程结束，置位运行标识
            m_backgroundRunFlag = false;
            m_backgroundWorkerType = AutoTraderWorkType.None;

            string text = "开仓追单已停止";
            AutoTraderNotice notice = CreateTraderNotice(text);
            SafeFireAutoTraderNotice(notice);
            WriteTraderNoticeLog(notice);

            SafeFireAutoTraderStateChanged(AutoTraderWorkType.None, AutoTraderState.Disable);
        }

        /// <summary>
        /// 开仓追单进度报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerForOpenChaseOrderProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        #endregion

        #region 平仓追单
        /// <summary>
        /// 平仓追单。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerDoWorkForCloseChaseOrder(object sender, DoWorkEventArgs e)
        {

            {
                AutoTraderNotice notice = CreateTraderNotice("平仓追单对齐操作");
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }

            //开仓追单
            //1.撤销掉当前所有未成交委托单
            //2.检查不对齐欠缺仓位，对欠缺仓位进行开仓追单
            //3.开仓以市价委托，尽量保证成交
            Debug.Assert(m_arbitrageOrder.State == ArbitrageOrderState.Closeing);

            if (m_arbitrageOrder.HasUnFinishOrderBook)
            {
                string text = "平仓追单有未完成委托单正在撤单";
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);

                List<USeOrderBook> unFinishOrderBook = m_arbitrageOrder.GetAllUnfinishOrderBooks();
                foreach (USeOrderBook orderBook in unFinishOrderBook)
                {
                    string errorMessage = string.Empty;
                    bool cancelResult = m_orderDriver.CancelOrder(orderBook.OrderNum, orderBook.Instrument, out errorMessage);
                    if (cancelResult == false)
                    {
                        text = string.Format("平仓追单{0} 撤单失败", orderBook.OrderNum);
                        notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                        SafeFireAutoTraderNotice(notice);
                        WriteTraderNoticeLog(notice);

                        return;   // 退出流程，触发预警
                    }
                }

                text = "平仓追单操作完成，等待撤单成功";
                notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);

                while (true)
                {
                    //等待撤单完成
                    m_operatorEvent.WaitOne(EVENT_WAIT_Time);

                    if (m_backgroundRunFlag == false)
                    {
                        text = string.Format("平仓追单流程退出");
                        notice = CreateTraderNotice(AutoTraderNoticeType.Infomation, text);
                        SafeFireAutoTraderNotice(notice);
                        WriteTraderNoticeLog(notice);
                        return;
                    }

                    if (m_arbitrageOrder.HasUnFinishOrderBook == false)
                    {
                        text = string.Format("平仓追单已完成撤单");
                        notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                        SafeFireAutoTraderNotice(notice);
                        WriteTraderNoticeLog(notice);
                        break;
                    }
                }
            }

            List<OrderCommand> commandList = new List<OrderCommand>();
            // 对欠缺仓位进行补单
            foreach (ArbitrageTask task in m_arbitrageOrder.CloseTaskGroup.TaskList)
            {
                if (task.FirstSubTask.TradeQty == task.SecondSubTask.TradeQty) continue;

                Debug.Assert(task.FirstSubTask.TradeQty > task.SecondSubTask.TradeQty);

                //追单
                USeInstrument instrument = task.SecondSubTask.Instrument;
                USeOrderSide orderSide = task.SecondSubTask.OrderSide;
                int orderQty = task.FirstSubTask.TradeQty - task.SecondSubTask.TradeQty;
                Debug.Assert(orderQty > 0);
                decimal orderPrice = GetFirstInstrumentOrderPrice(instrument, ArbitrageOrderPriceType.OpponentPrice, orderSide);
                Debug.Assert(orderPrice > 0);
                USeOffsetType offsetType = USeOffsetType.Close;

                List<OrderCommand> subCommandList = CreateOrderCommands(task.TaskId, instrument, orderSide, offsetType, orderQty, orderPrice, "平仓追单");
                foreach (OrderCommand command in subCommandList)
                {
                    bool orderResult = PlaceOrderForOrderCommand(command);
                    if (orderResult)
                    {
                        task.SecondSubTask.AddPositiveOrderBook(command.CreateOrignalOrderBook());
                        task.UpdateTaskState();
                        //task.TaskState = ArbitrageTaskState.FirstPlaceOrder;
                    }
                }

                commandList.AddRange(subCommandList);
            }

            #region 通知
            foreach (OrderCommand command in commandList)
            {
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, command.ToDescription());
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
            #endregion

            {
                string text = "平仓追单已完成追单，等待成交";
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
        }

        /// <summary>
        /// 平仓追单完成。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerForCloseChaseOrderCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (m_worker != null)
            {
                m_worker.ProgressChanged -= WorkerForCloseChaseOrderProgressChanged;
                m_worker.RunWorkerCompleted -= WorkerForCloseChaseOrderCompleted;
                m_worker.DoWork -= WorkerDoWorkForCloseChaseOrder;

                m_worker = null;
            }

            //流程结束，置位运行标识
            m_backgroundRunFlag = false;
            m_backgroundWorkerType = AutoTraderWorkType.None;

            string text = "平仓追单已停止";
            AutoTraderNotice notice = CreateTraderNotice(text);
            SafeFireAutoTraderNotice(notice);
            WriteTraderNoticeLog(notice);

            SafeFireAutoTraderStateChanged(AutoTraderWorkType.None, AutoTraderState.Disable);
        }

        /// <summary>
        /// 平仓追单进度报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerForCloseChaseOrderProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        #endregion
    }
}
