using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 套利单任务组。
    /// </summary>
    public class ArbitrageTaskGroup
    {
        #region member
        private List<ArbitrageTask> m_taskList = new List<ArbitrageTask>();
        #endregion

        #region property
        public OpenCloseType OpenCloseType { get; set; }

        /// <summary>
        /// 买入合约。
        /// </summary>
        public USeInstrument BuyInstrument { get; set; }

        /// <summary>
        /// 卖出。
        /// </summary>
        public USeInstrument SellInstrument { get; set; }

        /// <summary>
        /// 买入合约下单类型。
        /// </summary>
        public ArbitrageOrderPriceType BuyInstrumentOrderPriceType { get; set; }

        /// <summary>
        /// 卖出合约下单类型。
        /// </summary>
        public ArbitrageOrderPriceType SellInstrumentOrderPriceType { get; set; }

        /// <summary>
        /// 操作方向。
        /// </summary>
        public ArbitrageOperationSide OperationSide { get; set; }

        /// <summary>
        /// 优先买入方向。
        /// </summary>
        public USeOrderSide PreferentialSide { get; set; }

        /// <summary>
        /// 任务列表。
        /// </summary>
        public List<ArbitrageTask> TaskList
        {
            get { return m_taskList; }
            set { m_taskList = value; }
        }
        #endregion

        #region 只读属性
        /// <summary>
        /// 优先合约。
        /// </summary>
        [XmlIgnoreAttribute]
        public USeInstrument FirstInstrument
        {
            get
            {
                if (this.PreferentialSide == USeOrderSide.Buy)
                {
                    return this.BuyInstrument;
                }
                else
                {
                    Debug.Assert(this.PreferentialSide == USeOrderSide.Sell);
                    return this.SellInstrument;
                }
            }
        }

        /// <summary>
        /// 卖出。
        /// </summary>
        [XmlIgnoreAttribute]
        public USeInstrument SecondInstrument
        {
            get
            {
                if (this.PreferentialSide == USeOrderSide.Buy)
                {
                    return this.SellInstrument;
                }
                else
                {
                    Debug.Assert(this.PreferentialSide == USeOrderSide.Sell);
                    return this.BuyInstrument;
                }
            }
        }

        /// <summary>
        /// 任务数量。
        /// </summary>
        [XmlIgnoreAttribute]
        public int TaskCount
        {
            get { return m_taskList.Count; }
        }

        /// <summary>
        /// 已完成任务数量。
        /// </summary>
        [XmlIgnoreAttribute]
        public int FinishTaskCount
        {
            get
            {
                return m_taskList.Count(t => (t.TaskState == ArbitrageTaskState.SecondTradeFinish || t.TaskState == ArbitrageTaskState.ForceFinish));
            }
        }

        /// <summary>
        /// 执行中的任务数量。
        /// </summary>
        [XmlIgnoreAttribute]
        public int InExecutionTaskCount
        {
            get
            {
                return m_taskList.Count(t =>  (t.TaskState != ArbitrageTaskState.None && 
                                               t.TaskState != ArbitrageTaskState.SecondTradeFinish && 
                                               t.TaskState == ArbitrageTaskState.ForceFinish));
            }
        }

        /// <summary>
        /// 未执行的任务数量。
        /// </summary>
        [XmlIgnoreAttribute]
        public int NoExecuteTaskCount
        {
            get
            {
                return m_taskList.Count(t => t.TaskState == ArbitrageTaskState.None);
            }
        }

        /// <summary>
        /// 未完成任务数量。
        /// </summary>
        [XmlIgnoreAttribute]
        public int UnfinishTaskCount
        {
            get
            {
                return (this.TaskCount - this.FinishTaskCount);
            }
        }

        /// <summary>
        /// 是否有未完成的委托单。
        /// </summary>
        [XmlIgnoreAttribute]
        public bool HasUnFinishOrderBook
        {
            get
            {
                foreach (ArbitrageTask task in m_taskList)
                {
                    if (task.HasUnFinishOrderBook)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// 优先合约成交手数。
        /// </summary>
        [XmlIgnore]
        public int FirstSubTaskTradeQty
        {
            get
            {
                return m_taskList.Sum(t => t.FirstSubTask.TradeQty);
            }
        }

        /// <summary>
        /// 优先合约计划下单手数。
        /// </summary>
        [XmlIgnore]
        public int FirstSubTaskPlanOrderQty
        {
            get
            {
                return m_taskList.Sum(t => t.FirstSubTask.PlanOrderQty);
            }
        }

        /// <summary>
        /// 优先合约平均成交价。
        /// </summary>
        [XmlIgnore]
        public decimal FirstSubTaskAvgTradePrice
        {
            get
            {
                int tradeQty = this.FirstSubTaskTradeQty;
                if(tradeQty >0)
                {
                    decimal amount = m_taskList.Sum(t => t.FirstSubTask.TradeAvgPrice * t.FirstSubTask.TradeQty);
                    return amount / tradeQty;
                }
                else
                {
                    return 0m;
                }
            }
        }


        /// <summary>
        /// 反手合约成交手数。
        /// </summary>
        [XmlIgnore]
        public int SecondSubTaskTradeQty
        {
            get
            {
                return m_taskList.Sum(t => t.SecondSubTask.TradeQty);
            }
        }

        /// <summary>
        /// 反手合约计划下单手数。
        /// </summary>
        [XmlIgnore]
        public int SecondSubTaskPlanOrderQty
        {
            get
            {
                return m_taskList.Sum(t => t.SecondSubTask.PlanOrderQty);
            }
        }

        /// <summary>
        /// 反手合约平均成交价。
        /// </summary>
        [XmlIgnore]
        public decimal SecondSubTaskAvgTradePrice
        {
            get
            {
                int tradeQty = this.SecondSubTaskTradeQty;
                if (tradeQty > 0)
                {
                    decimal amount = m_taskList.Sum(t => t.SecondSubTask.TradeAvgPrice * t.SecondSubTask.TradeQty);
                    return amount / tradeQty;
                }
                else
                {
                    return 0m;
                }
            }
        }



        /// <summary>
        /// 买入合约成交手数。
        /// </summary>
        [XmlIgnore]
        public int BuySubTaskTradeQty
        {
            get
            {
                if(this.PreferentialSide == USeOrderSide.Buy)
                {
                    return this.FirstSubTaskTradeQty;
                }
                else
                {
                    Debug.Assert(this.PreferentialSide == USeOrderSide.Sell);
                    return this.SecondSubTaskTradeQty;
                }
            }
        }

        /// <summary>
        /// 优先合约计划下单手数。
        /// </summary>
        [XmlIgnore]
        public int BuySubTaskPlanOrderQty
        {
            get
            {
                if (this.PreferentialSide == USeOrderSide.Buy)
                {
                    return this.FirstSubTaskPlanOrderQty;
                }
                else
                {
                    Debug.Assert(this.PreferentialSide == USeOrderSide.Sell);
                    return this.SecondSubTaskPlanOrderQty;
                }
            }
        }

        /// <summary>
        /// 优先合约平均成交价。
        /// </summary>
        [XmlIgnore]
        public decimal BuySubTaskAvgTradePrice
        {
            get
            {
                if (this.PreferentialSide == USeOrderSide.Buy)
                {
                    return this.FirstSubTaskAvgTradePrice;
                }
                else
                {
                    Debug.Assert(this.PreferentialSide == USeOrderSide.Sell);
                    return this.SecondSubTaskAvgTradePrice;
                }
            }
        }


        /// <summary>
        /// 反手合约成交手数。
        /// </summary>
        [XmlIgnore]
        public int SellSubTaskTradeQty
        {
            get
            {
                if (this.PreferentialSide == USeOrderSide.Buy)
                {
                    return this.SecondSubTaskTradeQty;
                }
                else
                {
                    Debug.Assert(this.PreferentialSide == USeOrderSide.Sell);
                    return this.FirstSubTaskTradeQty;
                }
            }
        }

        /// <summary>
        /// 反手合约计划下单手数。
        /// </summary>
        [XmlIgnore]
        public int SellSubTaskPlanOrderQty
        {
            get
            {
                if (this.PreferentialSide == USeOrderSide.Buy)
                {
                    return this.SecondSubTaskPlanOrderQty;;
                }
                else
                {
                    Debug.Assert(this.PreferentialSide == USeOrderSide.Sell);
                    return this.FirstSubTaskPlanOrderQty;
                }
            }
        }

        /// <summary>
        /// 反手合约平均成交价。
        /// </summary>
        [XmlIgnore]
        public decimal SellSubTaskAvgTradePrice
        {
            get
            {
                if (this.PreferentialSide == USeOrderSide.Buy)
                {
                    return this.SecondSubTaskAvgTradePrice;
                }
                else
                {
                    Debug.Assert(this.PreferentialSide == USeOrderSide.Sell);
                    return this.FirstSubTaskAvgTradePrice;
                }
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// 获取所有未完成委托回报。
        /// </summary>
        /// <returns></returns>
        public List<USeOrderBook> GetAllUnFinishOrderBooks()
        {
            List<USeOrderBook> list = new List<USeOrderBook>();
            foreach (ArbitrageTask task in m_taskList)
            {
                list.AddRange(task.UnFinishOrderBooks);
            }
            return list;
        }

        /// <summary>
        /// 获取所有未完成委托回报。
        /// </summary>
        /// <returns></returns>
        public List<USeOrderBook> GetAllOrderBooks()
        {
            List<USeOrderBook> list = new List<USeOrderBook>();
            foreach (ArbitrageTask task in m_taskList)
            {
                list.AddRange(task.OrderBooks);
            }
            return list;
        }

        /// <summary>
        /// 委托单是否属于该任务。
        /// </summary>
        /// <param name="orderNum"></param>
        /// <returns></returns>
        public bool ContainsOrderBook(USeOrderNum orderNum)
        {
            foreach (ArbitrageTask task in m_taskList)
            {
                if(task.ContainsOrderBook(orderNum))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 更新委托回报。
        /// </summary>
        /// <param name="orderBook">委托回报。</param>
        public OrderBookUpdateResult UpdateOrderBook(USeOrderBook orderBook)
        {
            foreach (ArbitrageTask task in m_taskList)
            {
                if (task.ContainsOrderBook(orderBook.OrderNum))
                {
                    OrderBookUpdateResult result = task.UpdateOrderBook(orderBook);
                    Debug.Assert(result != null);

                    if (result != null)
                    {
                        result.Task = task;
                        return result;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获取下一个优先合约待开仓任务索引。
        /// </summary>
        public int GetFirstTaskUnExecutIndex()
        {
            for (int i = 0; i < m_taskList.Count; i++)
            {
                if (m_taskList[i].TaskState == ArbitrageTaskState.None)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 检查前置任务是否完成反手合约委托。
        /// </summary>
        /// <param name="taskIndex">任务索引。</param>
        /// <returns></returns>
        public bool CheckSecondTaskIsPlaceOrder(int taskIndex)
        {
            if (taskIndex < 0) return true;

            for(int i=0;i<m_taskList.Count && i< taskIndex;i++)
            {
                ArbitrageTask task = m_taskList[i];
                if(task.TaskState < ArbitrageTaskState.SecondPlaceOrderFinish)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取指定任务前的前置任务 反手合约未完全成交的任务数。
        /// <param name="taskIndex">任务索引。</param>
        /// <returns></returns>
        public int GetUnTradeSecondTaskCount(int taskIndex)
        {
            if (taskIndex < 0) return 0;

            Debug.Assert(taskIndex < m_taskList.Count);

            int count = 0;
            for (int i = 0; i < m_taskList.Count && i < taskIndex; i++)
            {
                ArbitrageTask task = m_taskList[i];
                if (task.TaskState < ArbitrageTaskState.SecondTradeFinish)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// 重置失败下单次数。
        /// </summary>
        public void ResetTryOrderErrorCount()
        {
            foreach(ArbitrageTask task in m_taskList)
            {
                task.ResetTryOrderCount();
            }
        }
        #endregion

        #region clone
        /// <summary>
        /// 克隆方法。
        /// </summary>
        /// <returns></returns>
        public ArbitrageTaskGroup Clone()
        {
            ArbitrageTaskGroup entity = new ArbitrageTaskGroup();
            entity.OpenCloseType = this.OpenCloseType;
            entity.BuyInstrument = this.BuyInstrument == null ? null : this.BuyInstrument.Clone();
            entity.SellInstrument = this.SellInstrument == null ? null : this.SellInstrument.Clone();
            entity.BuyInstrumentOrderPriceType = this.BuyInstrumentOrderPriceType;
            entity.SellInstrumentOrderPriceType = this.SellInstrumentOrderPriceType;

            entity.OperationSide = this.OperationSide;
            entity.PreferentialSide = this.PreferentialSide;

            List<ArbitrageTask> taskList = new List<ArbitrageTask>();
            foreach(ArbitrageTask taskItem in m_taskList)
            {
                taskList.Add(taskItem.Clone());
            }
            entity.m_taskList = taskList;

            return entity;
        }
        #endregion
    }
}
