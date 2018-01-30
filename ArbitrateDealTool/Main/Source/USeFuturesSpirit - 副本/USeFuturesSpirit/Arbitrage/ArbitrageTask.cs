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
    /// 套利下单任务。
    /// </summary>
    public sealed class ArbitrageTask
    {
        #region member
        private ArbitrageSubTask m_firstSubTask = null;   // 优先合约任务
        private ArbitrageSubTask m_secondSubTask = null;  // 反手合约任务
        #endregion

        #region construction
        /// <summary>
        /// 构造方法。
        /// </summary>
        public ArbitrageTask()
        {
            this.TaskState = ArbitrageTaskState.None;
            m_firstSubTask = new ArbitrageSubTask();
            m_secondSubTask = new ArbitrageSubTask();
        }
        #endregion

        #region property
        /// <summary>
        /// 任务ID。
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// 任务执行状态。
        /// </summary>
        public ArbitrageTaskState TaskState { get; set; }

        /// <summary>
        /// 任务下单原因。
        /// </summary>
        public TaskOrderReason OrderReason { get; set; }

        /// <summary>
        /// 优先合约-反手合约下单价差。
        /// </summary>
        public decimal PriceSpread { get; set; }

        /// <summary>
        /// 优先合约下单任务。
        /// </summary>
        public ArbitrageSubTask FirstSubTask
        {
            get { return m_firstSubTask; }
            set { m_firstSubTask = value; }
        }

        /// <summary>
        /// 反手合约下单任务。
        /// </summary>
        public ArbitrageSubTask SecondSubTask
        {
            get { return m_secondSubTask; }
            set { m_secondSubTask = value; }
        }
        #endregion

        #region 只读属性
        /// <summary>
        /// 是否有未完成委托单。
        /// </summary>
        [XmlIgnoreAttribute]
        public bool HasUnFinishOrderBook
        {
            get
            {
                if (m_firstSubTask.HasUnFinishOrderBook)
                {
                    return true;
                }
                if (m_secondSubTask.HasUnFinishOrderBook)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 未完成委托单列表。
        /// </summary>
        [XmlIgnoreAttribute]
        public List<USeOrderBook> UnFinishOrderBooks
        {
            get
            {
                List<USeOrderBook> firstUnFinishList = m_firstSubTask.UnFinishOrderBooks;
                List<USeOrderBook> secondUnFinishList = m_secondSubTask.UnFinishOrderBooks;

                return (firstUnFinishList.Union(secondUnFinishList)).ToList();
            }
        }

        /// <summary>
        /// 委托单列表。
        /// </summary>
        [XmlIgnoreAttribute]
        public List<USeOrderBook> OrderBooks
        {
            get
            {
                List<USeOrderBook> firstList = m_firstSubTask.OrderBooks;
                List<USeOrderBook> secondList = m_secondSubTask.OrderBooks;

                return (firstList.Union(secondList)).ToList();
            }
        }

        /// <summary>
        /// 任务是否完成。
        /// </summary>
        [XmlIgnoreAttribute]
        public bool IsFinish
        {
            get
            {
                return (this.TaskState == ArbitrageTaskState.SecondTradeFinish);
            }
        }

        /// <summary>
        /// 优先子任务平均成交价格。
        /// </summary>
        /// <returns></returns>
        public decimal FirstSubTaskAvgTradePrice
        {
            get
            {
                return m_firstSubTask.TradeAvgPrice;
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// 委托单是否属于该任务。
        /// </summary>
        /// <param name="orderNum">委托单号。</param>
        /// <returns></returns>
        public bool ContainsOrderBook(USeOrderNum orderNum)
        {
            if (m_firstSubTask.ContainsOrderBook(orderNum))
            {
                return true;
            }
            if (m_secondSubTask.ContainsOrderBook(orderNum))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 更新委托回报。
        /// </summary>
        /// <param name="orderBook">委托回报。</param>
        /// <returns>更新委托回报条数。</returns>
        public OrderBookUpdateResult UpdateOrderBook(USeOrderBook orderBook)
        {
            OrderBookUpdateResult result = m_firstSubTask.UpdateOrderBook(orderBook);
            if (result != null)
            {
                return result;
            }
            result = m_secondSubTask.UpdateOrderBook(orderBook);
            return result;
        }

        /// <summary>
        /// 更新任务状态。
        /// </summary>
        public void UpdateTaskState()
        {
            ArbitrageTaskState taskState = ArbitrageTaskState.None;
            //先检查优先任务，优先任务完成后在检查反手任务
            // 以优先任务状态为优先
            if (m_firstSubTask.TradeQty == m_firstSubTask.PlanOrderQty)  // 优先任务已完成
            {
                if (m_secondSubTask.TradeQty == m_secondSubTask.PlanOrderQty)
                {
                    //成交量等于计划下单量，标识为优先合约交易完成
                    taskState = ArbitrageTaskState.SecondTradeFinish;
                }
                else if (m_secondSubTask.OrderQty == m_secondSubTask.PlanOrderQty)
                {
                    //挂单数量等于计划量
                    taskState = ArbitrageTaskState.SecondPlaceOrderFinish;
                }
                else if (m_secondSubTask.OrderQty > 0)
                {
                    taskState = ArbitrageTaskState.SecondPalceOrder;
                }
                else
                {
                    taskState = ArbitrageTaskState.FirstTradeFinish;
                }
            }
            else
            { 
                // 优先任务还未完成
                if (m_firstSubTask.OrderQty == m_firstSubTask.PlanOrderQty)
                {
                    //挂单数量等于计划量,挂单完成
                    taskState = ArbitrageTaskState.FirstPlaceOrderFinish;
                }
                else if (m_firstSubTask.OrderQty > 0)
                {
                    taskState = ArbitrageTaskState.FirstPlaceOrder;
                }
                else
                {
                    taskState = ArbitrageTaskState.None;
                }
            }

            this.TaskState = taskState;
        }

        /// <summary>
        /// 重置失败下单次数。
        /// </summary>
        public void ResetTryOrderCount()
        {
            m_firstSubTask.ResetTryOrderCount();
            m_secondSubTask.ResetTryOrderCount();
        }
        #endregion

        #region Clone
        public ArbitrageTask Clone()
        {
            ArbitrageTask task = new ArbitrageTask();
            task.TaskId = this.TaskId;
            task.TaskState = this.TaskState;
            task.OrderReason = this.OrderReason;
            task.PriceSpread = this.PriceSpread;
            task.FirstSubTask = m_firstSubTask == null ? null : m_firstSubTask.Clone();
            task.SecondSubTask = m_secondSubTask == null ? null : m_secondSubTask.Clone();

            return task;
        }
        #endregion
    }
}
