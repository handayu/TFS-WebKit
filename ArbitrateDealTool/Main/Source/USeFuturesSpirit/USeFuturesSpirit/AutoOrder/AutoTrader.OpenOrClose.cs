using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using USe.TradeDriver.Common;
using USeFuturesSpirit.Arbitrage;
namespace USeFuturesSpirit
{
    /// <summary>
    /// 自动下单机 -- 开平仓操作。
    /// </summary>
    public partial class AutoTrader
    {
        #region 开平仓任务工具方法
        /// <summary>
        /// 校验开仓运行参数。
        /// </summary>
        /// <param name="errorMessage">错误信息。</param>
        /// <returns></returns>
        private bool VerfiyOpenArgument(ArbitrageOpenArgument argument, out string errorMessage)
        {
            Debug.Assert(argument != null);
            errorMessage = string.Empty;
            if (argument.BuyInstrument == null)
            {
                errorMessage = "买入合约不能为空";
                return false;
            }
            if (argument.SellInstrument == null)
            {
                errorMessage = "卖出合约不能为空";
                return false;
            }

            if (argument.TotalOrderQty <= 0)
            {
                errorMessage = "下单口数不能为空";
                return false;
            }
            if (argument.OrderQtyUint <= 0)
            {
                errorMessage = "下单单位不能为空";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 校验平仓运行参数。
        /// </summary>
        /// <param name="errorMessage">错误信息。</param>
        /// <returns></returns>
        private bool VerfiyArbitrageArgument(ArbitrageArgument argument, out string errorMessage)
        {
            //[yangming]待补充
            Debug.Assert(argument != null);
            errorMessage = string.Empty;
            //if (argument.BuyInstrument == null)
            //{
            //    errorMessage = "买入合约不能为空";
            //    return false;
            //}
            //if (argument.SellInstrument == null)
            //{
            //    errorMessage = "卖出合约不能为空";
            //    return false;
            //}

            //if (argument.OrderQtyUint <= 0)
            //{
            //    errorMessage = "下单单位不能为空";
            //    return false;
            //}
            return true;
        }

        /// <summary>
        /// 创建空的委托回报。
        /// </summary>
        /// <param name="orderNum">委托单号。</param>
        /// <param name="instrument">合约。</param>
        /// <param name="orderQty">委托量。</param>
        /// <param name="orderPrice">委托价格。</param>
        /// <param name="offsetType">开平方向。</param>
        /// <param name="orderSide">买卖方向。</param>
        /// <returns></returns>
        private USeOrderBook CreateOrignalUSeOrderBook(USeOrderNum orderNum, USeInstrument instrument, int orderQty, decimal orderPrice, USeOffsetType offsetType, USeOrderSide orderSide)
        {
            USeOrderBook orderBook = new USeOrderBook();
            orderBook.OrderNum = orderNum;
            orderBook.Account = string.Empty;
            orderBook.Instrument = instrument;
            orderBook.OrderQty = orderQty;
            orderBook.OrderPrice = orderPrice;
            orderBook.TradeQty = 0;
            orderBook.TradeAmount = 0m;
            orderBook.TradePrice = 0m;
            orderBook.TradeFee = 0m;
            orderBook.OrderStatus = USeOrderStatus.Unknown;
            orderBook.CancelQty = 0;
            orderBook.OrderSide = orderSide;
            orderBook.OffsetType = offsetType;
            orderBook.Memo = string.Empty;
            orderBook.OrderTime = DateTime.Now;

            return orderBook;
        }

        /// <summary>
        /// 获取指定价格。
        /// </summary>
        /// <param name="marketData">行情。</param>
        /// <param name="priceType">价格类型。</param>
        /// <returns></returns>
        private decimal GetMarketPrice(USeMarketData marketData, ArbitrageOrderPriceType priceType)
        {
            if (marketData == null) return 0m;

            switch (priceType)
            {
                case ArbitrageOrderPriceType.LastPrice: return marketData.LastPrice;
                case ArbitrageOrderPriceType.OpponentPrice: return marketData.AskPrice;
                case ArbitrageOrderPriceType.QueuePrice: return marketData.BidPrice;
                default: Debug.Assert(false); return 0m;
            }
        }

        /// <summary>
        /// 根据最小变动单位修剪下单价格。
        /// 【朝优先成交方向优化,买入向上,卖出向下修剪】。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <param name="orderPrice">下单价格。</param>
        /// <param name="orderSide">买卖方向。</param>
        /// <returns></returns
        private decimal TrimOrderPrice(USeInstrument instrument, decimal orderPrice, USeOrderSide orderSide)
        {
            USeInstrumentDetail instrumentDetail = m_orderDriver.QueryInstrumentDetail(instrument);

            decimal trimPrice = (int)(orderPrice / instrumentDetail.PriceTick) * instrumentDetail.PriceTick;
            if (orderPrice != trimPrice)
            {
                if (orderSide == USeOrderSide.Buy)
                {
                    Debug.Assert(trimPrice < orderPrice);    // 买入价格高一个最小变动单位
                    trimPrice = trimPrice + instrumentDetail.PriceTick;
                }
                else
                {
                    Debug.Assert(orderSide == USeOrderSide.Sell);
                    trimPrice = trimPrice - instrumentDetail.PriceTick; // 卖出价格低一个最小变动单位
                }
            }

            return trimPrice;
        }

        /// <summary>
        /// 是否满足价差。
        /// </summary>
        /// <param name="spreadSide"></param>
        /// <param name="threshold"></param>
        /// <param name="priceSpread"></param>
        /// <returns></returns>
        private bool IsInPriceSpread(PriceSpreadCondition condition, decimal priceSpread)
        {
            switch (condition.PriceSpreadSide)
            {
                case PriceSpreadSide.GreaterOrEqual:
                    return (priceSpread >= condition.PriceSpreadThreshold);
                case PriceSpreadSide.LessOrEqual:
                    return (priceSpread <= condition.PriceSpreadThreshold);
                default:
                    Debug.Assert(false);
                    return false;
            }
        }

        /// <summary>
        /// 获取优先合约下单价格。
        /// </summary>
        /// <param name="instrument">优先合约。</param>
        /// <param name="orderPriceType">下单价格类型。</param>
        /// <param name="orderSide">买卖方向。</param>
        /// <returns></returns>
        private decimal GetFirstInstrumentOrderPrice(USeInstrument instrument, ArbitrageOrderPriceType orderPriceType, USeOrderSide orderSide)
        {
            USeMarketData marketData = m_quoteDriver.Query(instrument);
            Debug.Assert(marketData != null);

            switch (orderPriceType)
            {
                case ArbitrageOrderPriceType.LastPrice: return marketData.LastPrice;
                case ArbitrageOrderPriceType.OpponentPrice:
                    {
                        if (orderSide == USeOrderSide.Buy)
                        {
                            return marketData.AskPrice;
                        }
                        else if (orderSide == USeOrderSide.Sell)
                        {
                            return marketData.BidPrice;
                        }
                        else
                        {
                            Debug.Assert(false);
                            return 0m;
                        }
                    }
                case ArbitrageOrderPriceType.QueuePrice:
                    {
                        if (orderSide == USeOrderSide.Buy)
                        {
                            return marketData.BidPrice;
                        }
                        else if (orderSide == USeOrderSide.Sell)
                        {
                            return marketData.AskPrice;
                        }
                        else
                        {
                            Debug.Assert(false);
                            return 0m;
                        }
                    }
                default:
                    Debug.Assert(false);
                    return 0m;
            }
        }

        /// <summary>
        /// 检查是否满足任务组监控价差条件。
        /// </summary>
        /// <param name="taskGroup">任务组。</param>
        /// <returns></returns>
        private PriceSpreadCheckResult CheckPriceSpread(OpenCloseType openCloseType,ArbitrageArgument argument)
        {
            USeMarketData nearMarketData = m_quoteDriver.Query(argument.NearInstrument);
            USeMarketData farMarketData = m_quoteDriver.Query(argument.FarInstrument);

            decimal nearPrice = 0m;
            decimal farPrice = 0m;
            if(openCloseType == OpenCloseType.Open)
            {
                nearPrice = GetMarketPrice(nearMarketData, argument.OpenArg.NearOrderPriceType);
                farPrice = GetMarketPrice(farMarketData, argument.OpenArg.FarOrderPriceType);
            }
            else if(openCloseType == OpenCloseType.Close)
            {
                nearPrice = GetMarketPrice(nearMarketData, argument.CloseArg.NearOrderPriceType);
                farPrice = GetMarketPrice(farMarketData, argument.CloseArg.FarOrderPriceType);
            }
            else
            {
                Debug.Assert(false);
                return PriceSpreadCheckResult.CreateNoOrderResult();
            }

            if (nearPrice <= 0 || farPrice <= 0)
            {
                return PriceSpreadCheckResult.CreateNoOrderResult();
            }

            decimal priceSpread = nearPrice - farPrice;
            if (openCloseType == OpenCloseType.Open)
            {
                Debug.Assert(argument.OpenArg != null && argument.OpenArg.OpenCondition != null);
                if (IsInPriceSpread(argument.OpenArg.OpenCondition, priceSpread))
                {
                    PriceSpreadCheckResult result = new PriceSpreadCheckResult();
                    result.OrderReason = TaskOrderReason.Open;
                    result.PriceSpreadThreshold = argument.OpenArg.OpenCondition.PriceSpreadThreshold;
                    return result;
                }
            }
            else if (openCloseType == OpenCloseType.Close)
            {
                Debug.Assert(argument.CloseArg != null && argument.CloseArg.CloseCondition != null);
                if (argument.CloseArg != null && argument.CloseArg.CloseCondition != null)
                {
                    if (IsInPriceSpread(argument.CloseArg.CloseCondition, priceSpread))
                    {
                        PriceSpreadCheckResult result = new PriceSpreadCheckResult();
                        result.OrderReason = TaskOrderReason.Close;
                        result.PriceSpreadThreshold = argument.CloseArg.CloseCondition.PriceSpreadThreshold;
                        return result;
                    }
                }

                if (argument.StopLossArg != null && argument.StopLossArg.StopLossCondition != null)
                {
                    if (IsInPriceSpread(argument.StopLossArg.StopLossCondition, priceSpread))
                    {
                        PriceSpreadCheckResult result = new PriceSpreadCheckResult();
                        result.OrderReason = TaskOrderReason.StopLoss;
                        result.PriceSpreadThreshold = argument.StopLossArg.StopLossCondition.PriceSpreadThreshold;
                        return result;
                    }
                }
            }

            return PriceSpreadCheckResult.CreateNoOrderResult();
        }
        #endregion

        #region 开平仓监控
        /// <summary>
        /// 处理套利单建仓任务。
        /// </summary>
        private void ProcessArbitrageOrderOpenTask()
        {
            while (true)
            {
                //无变更，继续等待
                bool waitResult = m_operatorEvent.WaitOne(EVENT_WAIT_Time);  //[yangming]理论上超时响应，继续等待信号，保险起见现在不等待

                if (m_backgroundRunFlag == false)
                {
                    return;  // 监控流程停止退出。
                }

                if (m_arbitrageOrder.State != ArbitrageOrderState.Opening)
                {
                    return;  // 当前状态已非开仓中,退出开仓检查流程
                }

                // 0.检查是否有反手合约需要挂单
                bool secondTaskResult = ProcessSecondSubTask(m_arbitrageOrder.OpenTaskGroup);

                // 1.检查是否有优先合约需要挂单
                bool firstTaskResult = ProcessFirstSubTask(m_arbitrageOrder.OpenTaskGroup,m_arbitrageOrder.OpenArgument.DifferentialUnit);

                if (secondTaskResult || firstTaskResult)
                {
                    SafeFireArbitrageOrderChanged();
                }
            }
        }

        /// <summary>
        /// 处理套利单平仓任务。
        /// </summary>
        private void ProcessArbitrageOrderCloseTask()
        {
            while (true)
            {
                //无变更，继续等待
                bool waitResult = m_operatorEvent.WaitOne(EVENT_WAIT_Time);  //[yangming]理论上超时响应，继续等待信号，保险起见现在不等待

                if (m_backgroundRunFlag == false)
                {
                    return;  // 监控流程停止退出。
                }

                if (m_arbitrageOrder.State != ArbitrageOrderState.Closeing)
                {
                    return;  // 当前状态已非平仓中,退出平仓检查流程
                }

                //0.检查是否有反手合约需要挂单
                bool secondTaskResult = ProcessSecondSubTask(m_arbitrageOrder.CloseTaskGroup);

                //1.检查是否有优先合约需要挂单
                bool firstTaskResult = ProcessFirstSubTask(m_arbitrageOrder.CloseTaskGroup, m_arbitrageOrder.CloseArgument.DifferentialUnit);

                if (secondTaskResult || firstTaskResult)
                {
                    SafeFireArbitrageOrderChanged();
                }
            }
        }

        /// <summary>
        /// 套利单任务--反手合约下单(优先合约已成交)
        /// </summary>
        /// <param name="taskGroup">开仓/平仓 任务组。</param>
        private bool ProcessSecondSubTask(ArbitrageTaskGroup taskGroup)
        {
            for (int i = 0; i < taskGroup.TaskList.Count; i++)
            {
                #region 校验
                ArbitrageTask task = taskGroup.TaskList[i];

                if (task.TaskState != ArbitrageTaskState.FirstTradeFinish)
                {
                    //优先合约未成交，无需做反手合约检查
                    continue;
                }

                ArbitrageSubTask secondSubTask = task.SecondSubTask;

                if (secondSubTask.TryOrderCount >= m_tryOrderCondition.MaxTryOrderCount)
                {
                    m_backgroundRunFlag = false;

                    string text = string.Format("流程暂停，{2}下单任务[{0}]超出最大尝试下单次数{1}次",
                        task.TaskId, m_tryOrderCondition.MaxTryOrderCount, secondSubTask.Instrument.InstrumentCode);
                    AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                    SafeFireAutoTraderNotice(notice);

                    AlarmNotice alarm = new AlarmNotice(AlarmType.AutoTraderWarning, text);
                    SafeFireAutoTraderAlarm(alarm);

                    WriteTraderNoticeLog(notice);
                    
                    return false;
                }

                if (secondSubTask.CanPlaceNextOrder(m_tryOrderCondition.NextOrderInterval) == false)
                {
                    WriteTraderDebugInfo("反手合约检查，距离上次下单时间过近，暂时不下单");
                    continue;
                }
                #endregion

                #region 下单
                //获取反手合约下单价格
                decimal orderPrice = GetSecondTaskOrderPrice(taskGroup.OperationSide, taskGroup.PreferentialSide, task.FirstSubTaskAvgTradePrice, task.PriceSpread);

                orderPrice = TrimOrderPrice(secondSubTask.Instrument, orderPrice, secondSubTask.OrderSide);
                int orderQty = secondSubTask.UnOrderQty;
                Debug.Assert(orderQty > 0);

                USeInstrument instrument = secondSubTask.Instrument;
                USeOffsetType offsetType = secondSubTask.OffsetType;
                USeOrderSide orderSide = secondSubTask.OrderSide;

                List<OrderCommand> commandList = null;
                lock (ms_orderSyncObj)
                {
                    commandList = CreateOrderCommands(task.TaskId, instrument, orderSide, offsetType, orderQty, orderPrice, "反手合约");
                    Debug.Assert(commandList != null && commandList.Count > 0);

                    foreach (OrderCommand command in commandList)
                    {
                        bool orderResult = PlaceOrderForOrderCommand(command);
                        if (orderResult)
                        {
                            secondSubTask.AddPositiveOrderBook(command.CreateOrignalOrderBook());
                            task.UpdateTaskState();
                            //task.TaskState = ArbitrageTaskState.FirstPlaceOrder;
                        }
                    }
                }
                secondSubTask.AddTryOrderCount();  // 下单累加1次
                #endregion

                #region 通知
                foreach (OrderCommand command in commandList)
                {
                    AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, command.ToDescription());
                    SafeFireAutoTraderNotice(notice);
                    WriteTraderNoticeLog(notice);
                }
                #endregion
            }

            return false;
        }

        /// <summary>
        /// 套利单任务 -- 优先合约下单。
        /// </summary>
        /// <param name="taskGroup">开仓/平仓 任务组。</param>
        /// <param name="differentialUnit">最大仓差。</param>
        /// <returns></returns>
        private bool ProcessFirstSubTask(ArbitrageTaskGroup taskGroup,int differentialUnit)
        {
            #region 校验
            PriceSpreadCheckResult priceSpeadResult = CheckPriceSpread(taskGroup.OpenCloseType, m_arbitrageOrder.Argument);
            if(priceSpeadResult.OrderReason == TaskOrderReason.None)
            {
                WriteTraderDebugInfo("优先合约检查,不满足价差条件");
                return false;
            }

            int firstUnExeTaskIndex = taskGroup.GetFirstTaskUnExecutIndex();
            if (firstUnExeTaskIndex < 0)
            {
                // 优先合约全部完成下单
                return false;
            }

            bool secondTaskIsPlaceOrder = taskGroup.CheckSecondTaskIsPlaceOrder(firstUnExeTaskIndex);
            if (secondTaskIsPlaceOrder == false)
            {
                WriteTraderDebugInfo("反手合约还未下单,不许下单");
                return false;
            }

            //反手仓位未成交任务数
            int secondUnTradeTaskCount = taskGroup.GetUnTradeSecondTaskCount(firstUnExeTaskIndex);
            if (secondUnTradeTaskCount >= differentialUnit)
            {
                WriteTraderDebugInfo("优先合约反手合约任务仓差大于等于允许最大仓差，不许下单");
                return false;
            }

            ArbitrageTask task = taskGroup.TaskList[firstUnExeTaskIndex];
            Debug.Assert(task != null && task.TaskState == ArbitrageTaskState.None);
            ArbitrageSubTask firstSubTask = task.FirstSubTask;

            if (firstSubTask.TryOrderCount >= m_tryOrderCondition.MaxTryOrderCount)
            {
                m_backgroundRunFlag = false;

                string text = string.Format("流程暂停，{2}下单任务[{0}]超出最大尝试下单次数{1}次", 
                    task.TaskId, m_tryOrderCondition.MaxTryOrderCount,firstSubTask.Instrument.InstrumentCode);
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, text);
                SafeFireAutoTraderNotice(notice);

                AlarmNotice alarm = new AlarmNotice(AlarmType.AutoTraderWarning, text);
                SafeFireAutoTraderAlarm(alarm);

                WriteTraderNoticeLog(notice);
                
                return false;
            }

            if (firstSubTask.CanPlaceNextOrder(m_tryOrderCondition.NextOrderInterval) == false)
            {
                WriteTraderDebugInfo("优先合约检查，距离上次下单时间过近，暂时不下单");
                return false;
            }
            #endregion

            #region 下单

            Debug.Assert(priceSpeadResult.OrderReason == TaskOrderReason.Open ||
                         priceSpeadResult.OrderReason == TaskOrderReason.Close ||
                         priceSpeadResult.OrderReason == TaskOrderReason.StopLoss);
            task.OrderReason = priceSpeadResult.OrderReason;
            task.PriceSpread = priceSpeadResult.PriceSpreadThreshold;

            //获取优先合约下单价格
            decimal orderPrice = GetFirstInstrumentOrderPrice(firstSubTask.Instrument, firstSubTask.OrderPriceType, firstSubTask.OrderSide);
            Debug.Assert(orderPrice > 0);
            int orderQty = firstSubTask.UnOrderQty;
            Debug.Assert(orderQty > 0);

            USeInstrument instrument = firstSubTask.Instrument;
            USeOffsetType offsetType = firstSubTask.OffsetType;
            USeOrderSide orderSide = firstSubTask.OrderSide;

            List<OrderCommand> commandList = null;
            lock (ms_orderSyncObj)
            {
                commandList = CreateOrderCommands(task.TaskId, instrument, orderSide, offsetType, orderQty, orderPrice, "优先合约");
                Debug.Assert(commandList != null && commandList.Count > 0);

                foreach (OrderCommand command in commandList)
                {
                    bool orderResult = PlaceOrderForOrderCommand(command);
                    if (orderResult)
                    {
                        firstSubTask.AddPositiveOrderBook(command.CreateOrignalOrderBook());
                        task.UpdateTaskState();
                        //task.TaskState = ArbitrageTaskState.FirstPlaceOrder;
                    }
                }
            }
            firstSubTask.AddTryOrderCount();  // 下单累加1次
            #endregion

            #region 通知
            foreach (OrderCommand command in commandList)
            { 
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, command.ToDescription());
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 创建委托命令集合。
        /// </summary>
        /// <param name="taskId">任务ID。</param>
        /// <param name="instrument">合约。</param>
        /// <param name="orderSide">买卖方向。</param>
        /// <param name="offsetType">开平方向。</param>
        /// <param name="orderQty">委托量。</param>
        /// <param name="orderPrice">委托价格。</param>
        /// <param name="orderReason">委托原因。</param>
        /// <returns></returns>
        private List<OrderCommand> CreateOrderCommands(int taskId,USeInstrument instrument,USeOrderSide orderSide,USeOffsetType offsetType,int orderQty,decimal orderPrice,string orderReason)
        {
            List<OrderCommand> commandList = new List<OrderCommand>();

            //构造指令
            if (offsetType == USeOffsetType.Open)
            {
                OrderCommand command = new OrderCommand() {
                    TaskId = taskId,
                    Instrument = instrument,
                    OrderSide = orderSide,
                    OffsetType = USeOffsetType.Open,
                    OrderQty = orderQty,
                    OrderPrice = orderPrice,
                    OrderReason = orderReason
                };

                commandList.Add(command);
            }
            else
            {
                Debug.Assert(offsetType == USeOffsetType.Close);

                if (instrument.Market == USeMarket.SHFE)
                {
                    #region 上交所平仓
                    USeDirection direction = orderSide == USeOrderSide.Buy ? USeDirection.Short : USeDirection.Long;
                        //上海交易所平仓需区分平今还是平昨
                    USePosition position = m_orderDriver.QueryPositions(instrument, direction);
                    //[yangming]
                    string tmpTestText = string.Format(@"[hanyuClose]Ins:{0} OrderSide:{1} OffsetFlag:{2} OrderQty：{3},NewAvaliablePosition:{4} ,NewFrozonPosition:{5}，NewPosition{6}," +
                        "OldAvaliablePosition:{7}，OldFrozonPosition:{8},OldPosition:{9}",instrument.InstrumentCode,orderSide,offsetType,orderQty,position.NewAvaliablePosition,
                        position.NewFrozonPosition,position.NewPosition,position.OldAvaliablePosition,position.OldFrozonPosition,position.OldPosition);

                    USeManager.Instance.EventLogger.WriteAudit(tmpTestText);
                    //
                    if(position == null || position.AvaliablePosition < orderQty)
                    {
                        //查询不到仓位,或者仓位不足，直接构造平今指令
                        OrderCommand command = new OrderCommand() {
                            TaskId = taskId,
                            Instrument = instrument,
                            OrderSide = orderSide,
                            OffsetType = USeOffsetType.CloseToday,
                            OrderQty = orderQty,
                            OrderPrice = orderPrice,
                            OrderReason = orderReason
                        };

                        commandList.Add(command);
                    }
                    else
                    {
                        //平今
                        int remainQty = orderQty;
                        if(position.NewAvaliablePosition >0)
                        {
                            int closeQty = Math.Min(position.NewAvaliablePosition, remainQty);
                            OrderCommand command = new OrderCommand() {
                                TaskId = taskId,
                                Instrument = instrument,
                                OrderSide = orderSide,
                                OffsetType = USeOffsetType.CloseToday,
                                OrderQty = closeQty,
                                OrderPrice = orderPrice,
                                OrderReason = orderReason
                            };
                            remainQty -= closeQty;
                            commandList.Add(command);
                        }

                        //平昨
                        if(remainQty >0)
                        {
                            Debug.Assert(remainQty <= position.OldAvaliablePosition);
                            int closeQty = remainQty;
                            OrderCommand command = new OrderCommand() {
                                TaskId = taskId,
                                Instrument = instrument,
                                OrderSide = orderSide,
                                OffsetType = USeOffsetType.CloseHistory,
                                OrderQty = closeQty,
                                OrderPrice = orderPrice,
                                OrderReason = orderReason
                            };
                            remainQty -= closeQty;
                            commandList.Add(command);
                        }
                        Debug.Assert(remainQty == 0);
                    }
                    #endregion
                }
                else
                {
                    OrderCommand command = new OrderCommand() {
                        TaskId = taskId,
                        Instrument = instrument,
                        OrderSide = orderSide,
                        OffsetType = USeOffsetType.Close,
                        OrderQty = orderQty,
                        OrderPrice = orderPrice,
                        OrderReason = orderReason
                    };
                }
            }

            return commandList;
        }

        /// <summary>
        /// 根据下单指令下单。
        /// </summary>
        /// <param name="command">下单指令。</param>
        private bool PlaceOrderForOrderCommand(OrderCommand command)
        {
            string errorMessage = string.Empty;
            USeOrderNum orderNum = m_orderDriver.PlaceOrder(command.Instrument,command.OrderQty,command.OrderPrice,command.OffsetType,command.OrderSide, out errorMessage);
            command.OrderNum = orderNum;
            if(orderNum == null)
            {
                Debug.Assert(string.IsNullOrEmpty(errorMessage) == false);
                command.OrderErrorMessage = errorMessage;

                return false; // 下单失败
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 创建任务组
        /// <summary>
        /// 创建开仓任务组。
        /// </summary>
        /// <param name="openArg">开仓参数。</param>
        /// <returns></returns>
        private static ArbitrageTaskGroup CreateOpenTaskGroup(ArbitrageArgument argument)
        {
            ArbitrageOpenArgument openArg = argument.OpenArg;

            USeInstrument firstInstrument = null;
            USeInstrument secondInstrument = null;
            USeOrderSide firstOrderSide = USeOrderSide.Buy;
            USeOrderSide secondOrderSide = USeOrderSide.Sell;
            ArbitrageOrderPriceType firstOrderPriceType = ArbitrageOrderPriceType.Unknown;
            ArbitrageOrderPriceType secondOrderPriceType = ArbitrageOrderPriceType.Unknown;

            if (openArg.PreferentialSide == USeOrderSide.Buy)
            {
                //优先买入
                firstInstrument = openArg.BuyInstrument;
                firstOrderSide = USeOrderSide.Buy;
                firstOrderPriceType = openArg.BuyInstrumentOrderPriceType;

                secondInstrument = openArg.SellInstrument;
                secondOrderSide = USeOrderSide.Sell;
                secondOrderPriceType = openArg.SellInstrumentOrderPriceType;
            }
            else if (openArg.PreferentialSide == USeOrderSide.Sell)
            {
                //优先卖出
                firstInstrument = openArg.SellInstrument;
                firstOrderSide = USeOrderSide.Sell;
                firstOrderPriceType = openArg.SellInstrumentOrderPriceType;

                secondInstrument = openArg.BuyInstrument;
                secondOrderSide = USeOrderSide.Buy;
                secondOrderPriceType = openArg.BuyInstrumentOrderPriceType;
            }
            else
            {
                Debug.Assert(false);
            }

            Debug.Assert(openArg.OrderQtyUint > 0);
            int taskCount = openArg.TotalOrderQty / openArg.OrderQtyUint;
            if ((openArg.TotalOrderQty % openArg.OrderQtyUint) > 0)
            {
                taskCount += 1;
            }

            #region 构造任务组
            ArbitrageTaskGroup taskGroup = new ArbitrageTaskGroup();
            taskGroup.OpenCloseType = OpenCloseType.Open;
            taskGroup.BuyInstrument = openArg.BuyInstrument;
            taskGroup.SellInstrument = openArg.SellInstrument;
            taskGroup.BuyInstrumentOrderPriceType = openArg.BuyInstrumentOrderPriceType;
            taskGroup.SellInstrumentOrderPriceType = openArg.SellInstrumentOrderPriceType;

            taskGroup.OperationSide = argument.OperationSide;
            taskGroup.PreferentialSide = openArg.PreferentialSide;

            List<ArbitrageTask> taskList = new List<ArbitrageTask>();
            int remainPlanQty = openArg.TotalOrderQty;
            for (int i = 1; i <= taskCount; i++)
            {
                int planOrderQty = Math.Min(openArg.OrderQtyUint, remainPlanQty);
                Debug.Assert(planOrderQty > 0 && planOrderQty <= openArg.OrderQtyUint);
                remainPlanQty -= planOrderQty;

                ArbitrageTask task = new ArbitrageTask();
                task.TaskId = i;
                task.TaskState = ArbitrageTaskState.None;
                ArbitrageSubTask firstSubTask = new ArbitrageSubTask() {
                    Instrument = firstInstrument,
                    OrderPriceType = firstOrderPriceType,
                    OrderSide = firstOrderSide,
                    PlanOrderQty = planOrderQty,
                    OffsetType = USeOffsetType.Open,
                };
                ArbitrageSubTask secondSubTask = new ArbitrageSubTask() {
                    Instrument = secondInstrument,
                    OrderPriceType = secondOrderPriceType,
                    OrderSide = secondOrderSide,
                    PlanOrderQty = planOrderQty,
                    OffsetType = USeOffsetType.Open
                };

                task.FirstSubTask = firstSubTask;
                task.SecondSubTask = secondSubTask;

                taskList.Add(task);
            }
            Debug.Assert(remainPlanQty == 0);
            taskGroup.TaskList = taskList;
            #endregion

            return taskGroup;
        }

        /// <summary>
        /// 创建平仓任务组。
        /// </summary>
        /// <param name="openTaskGroup">开仓任务组。</param>
        /// <param name="closeArg">平仓参数。</param>
        /// <returns></returns>
        private ArbitrageTaskGroup CreateCloseTaskGroup(ArbitrageTaskGroup openTaskGroup,ArbitrageArgument argument)
        {
            ArbitrageCloseArgument closeArg = argument.CloseArg;
            Debug.Assert(openTaskGroup.BuyInstrument == closeArg.SellInstrument);
            Debug.Assert(openTaskGroup.SellInstrument == closeArg.BuyInstrument);

            int buyPosition = openTaskGroup.SellSubTaskTradeQty; // 平仓买入量 = 开仓卖出量
            int sellPosition = openTaskGroup.BuySubTaskTradeQty; // 平仓卖出量 = 开仓买入量

            USeInstrument firstInstrument = null;
            USeInstrument secondInstrument = null;
            USeOrderSide firstOrderSide = USeOrderSide.Buy;
            USeOrderSide secondOrderSide = USeOrderSide.Sell;
            ArbitrageOrderPriceType firstOrderPriceType = ArbitrageOrderPriceType.Unknown;
            ArbitrageOrderPriceType secondOrderPriceType = ArbitrageOrderPriceType.Unknown;
            int firstPosition = 0;
            int secondPosition = 0;
            if (closeArg.PreferentialSide == USeOrderSide.Buy)
            {
                //优先买入
                firstInstrument = closeArg.BuyInstrument;
                firstOrderSide = USeOrderSide.Buy;
                firstOrderPriceType = closeArg.BuyInstrumentOrderPriceType;
                secondInstrument = closeArg.SellInstrument;
                secondOrderSide = USeOrderSide.Sell;
                secondOrderPriceType = closeArg.SellInstrumentOrderPriceType;
                firstPosition = buyPosition;
                secondPosition = sellPosition;
            }
            else if (closeArg.PreferentialSide == USeOrderSide.Sell)
            {
                //优先卖出
                firstInstrument = closeArg.SellInstrument;
                firstOrderSide = USeOrderSide.Sell;
                firstOrderPriceType = closeArg.SellInstrumentOrderPriceType;
                secondInstrument = closeArg.BuyInstrument;
                secondOrderSide = USeOrderSide.Buy;
                secondOrderPriceType = closeArg.BuyInstrumentOrderPriceType;
                firstPosition = sellPosition;
                secondPosition = buyPosition;
            }
            else
            {
                Debug.Assert(false);
            }


            Debug.Assert(closeArg.OrderQtyUint > 0);

            int maxPositon = Math.Max(buyPosition, sellPosition);
            int taskCount = maxPositon / closeArg.OrderQtyUint;
            if ((maxPositon % closeArg.OrderQtyUint) > 0)
            {
                taskCount += 1;
            }

            #region 构造任务组
            ArbitrageTaskGroup taskGroup = new ArbitrageTaskGroup();
            taskGroup.OpenCloseType = OpenCloseType.Close;
            taskGroup.BuyInstrument = closeArg.BuyInstrument;
            taskGroup.SellInstrument = closeArg.SellInstrument;
            taskGroup.BuyInstrumentOrderPriceType = closeArg.BuyInstrumentOrderPriceType;
            taskGroup.SellInstrumentOrderPriceType = closeArg.SellInstrumentOrderPriceType;

            taskGroup.OperationSide = argument.OperationSide.GetOppositeSide();
            taskGroup.PreferentialSide = closeArg.PreferentialSide;

            List<ArbitrageTask> taskList = new List<ArbitrageTask>();
            int remainFirstPlanQty = firstPosition;
            int remainSecondPlanQty = secondPosition;
            for (int i = 1; i <= taskCount; i++)
            {
                int firstPlanOrderQty = Math.Min(closeArg.OrderQtyUint, remainFirstPlanQty);
                remainFirstPlanQty -= firstPlanOrderQty;
                int secondPlanOrderQty = Math.Min(closeArg.OrderQtyUint, remainSecondPlanQty);
                remainSecondPlanQty -= secondPlanOrderQty;

                ArbitrageTask task = new ArbitrageTask();
                task.TaskId = i;
                task.TaskState = ArbitrageTaskState.None;
                ArbitrageSubTask firstSubTask = new ArbitrageSubTask()
                {
                    Instrument = firstInstrument,
                    OrderPriceType = firstOrderPriceType,
                    OrderSide = firstOrderSide,
                    PlanOrderQty = firstPlanOrderQty,
                    OffsetType = USeOffsetType.Close,
                };
                ArbitrageSubTask secondSubTask = new ArbitrageSubTask()
                {
                    Instrument = secondInstrument,
                    OrderPriceType = secondOrderPriceType,
                    OrderSide = secondOrderSide,
                    PlanOrderQty = secondPlanOrderQty,
                    OffsetType = USeOffsetType.Close
                };

                task.FirstSubTask = firstSubTask;
                task.SecondSubTask = secondSubTask;

                taskList.Add(task);
            }
            Debug.Assert(remainFirstPlanQty == 0);
            Debug.Assert(remainSecondPlanQty == 0);
            taskGroup.TaskList = taskList;
            #endregion

            return taskGroup;
        }


        #endregion

        private decimal GetSecondTaskOrderPrice(ArbitrageOperationSide operationSide,USeOrderSide preferentialSide, decimal firstSubTaskAvgTradePrice,decimal priceSpread)
        {
            Debug.Assert(firstSubTaskAvgTradePrice > 0);

            if(operationSide == ArbitrageOperationSide.BuyNearSellFar)
            {
                if(preferentialSide == USeOrderSide.Buy)
                {
                    return firstSubTaskAvgTradePrice - priceSpread;
                }
                else if(preferentialSide == USeOrderSide.Sell)
                {
                    return firstSubTaskAvgTradePrice + priceSpread;
                }
                else
                {
                    Debug.Assert(false);
                }
            }
            else if(operationSide == ArbitrageOperationSide.SellNearBuyFar)
            {
                if(preferentialSide == USeOrderSide.Buy)
                {
                    return firstSubTaskAvgTradePrice + priceSpread;
                }
                else if(preferentialSide == USeOrderSide.Sell)
                {
                    return firstSubTaskAvgTradePrice - priceSpread;
                }
                else
                {
                    Debug.Assert(false);
                }
            }

            throw new Exception("无法计算反手子任务下单价格");
        }
        
    }
}
