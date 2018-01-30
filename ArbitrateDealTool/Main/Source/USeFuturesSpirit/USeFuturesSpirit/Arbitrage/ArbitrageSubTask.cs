using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;

using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 套利单子任务。
    /// </summary>
    public class ArbitrageSubTask
    {
        #region member
        private List<USeOrderBook> m_orderBookList = null;  // 合约委托列表
        #endregion

        #region construction
        public ArbitrageSubTask()
        {
            m_orderBookList = new List<USeOrderBook>();
        }
        #endregion

        #region property
        /// <summary>
        /// 合约。
        /// </summary>
        public USeInstrument Instrument { get; set; }

        /// <summary>
        /// 下单价格类型。
        /// </summary>
        public ArbitrageOrderPriceType OrderPriceType { get; set; }

        /// <summary>
        /// 买卖方向。
        /// </summary>
        public USeOrderSide OrderSide { get; set; }

        /// <summary>
        /// 开平方向。
        /// </summary>
        public USeOffsetType OffsetType { get; set; }

        /// <summary>
        /// 计划委托数量。
        /// </summary>
        public int PlanOrderQty { get; set; }

        /// <summary>
        /// 委托列表。
        /// </summary>
        public List<USeOrderBook> OrderBooks
        {
            get { return m_orderBookList; }
            set { m_orderBookList = value; }
        }
        #endregion

        /// <summary>
        /// 尝试下单次数(累计次数)。
        /// </summary>
        [XmlIgnore]
        public int TryOrderCount { get; set; }

        /// <summary>
        /// 上次下单时间。
        /// </summary>
        [XmlIgnore]
        public DateTime? LastOrderTime { get; set; }

        #region 只读属性
        /// <summary>
        /// 正向已委托数量。
        /// </summary>
        [XmlIgnore]
        public int PositiveOrderQty
        {
            get
            {
                USeOrderSide orderSide = this.OrderSide;
                return m_orderBookList.Where(o=>o.OrderSide == orderSide).Sum(o => (o.OrderQty - o.BlankQty - o.CancelQty));
            }
        }

        /// <summary>
        /// 正向成交数量。
        /// </summary>
        [XmlIgnoreAttribute]
        public int PositiveTradeQty
        {
            get
            {
                USeOrderSide orderSide = this.OrderSide;
                return m_orderBookList.Where(o => o.OrderSide == orderSide).Sum(o => o.TradeQty);
            }
        }

        /// <summary>
        /// 反向已委托数量。
        /// </summary>
        [XmlIgnore]
        public int NegativeOrderQty
        {
            get
            {
                USeOrderSide orderSide = this.OrderSide.GetOppositeOrderSide();
                return m_orderBookList.Where(o => o.OrderSide == orderSide).Sum(o => (o.OrderQty - o.BlankQty - o.CancelQty));
            }
        }

        /// <summary>
        /// 反向已成交数量。
        /// </summary>
        [XmlIgnoreAttribute]
        public int NegativeTradeQty
        {
            get
            {
                USeOrderSide orderSide = this.OrderSide.GetOppositeOrderSide();
                return m_orderBookList.Where(o => o.OrderSide == orderSide).Sum(o => o.TradeQty);
            }
        }

        /// <summary>
        /// 已委托数量
        /// (正向委托数量-反向委托数量)。
        /// </summary>
        [XmlIgnoreAttribute]
        public int OrderQty
        {
            get
            {
                return (this.PositiveOrderQty - this.NegativeOrderQty);
            }
        }

        /// <summary>
        /// 未委托数量。
        /// </summary>
        [XmlIgnoreAttribute]
        public int UnOrderQty
        {
            get { return (this.PlanOrderQty - this.OrderQty); }
        }

        /// <summary>
        /// 成交数量。
        /// </summary>
        [XmlIgnoreAttribute]
        public int TradeQty
        {
            get { return (this.PositiveTradeQty - this.NegativeTradeQty);}
        }

        /// <summary>
        /// 平均成交价格。
        /// </summary>
        [XmlIgnoreAttribute]
        public decimal TradeAvgPrice
        {
            get
            {
                //(正手成交量*正手均价 - 反手成交量*反手均价) / 正手持仓量
                int tradeQty = this.TradeQty;
                if (tradeQty > 0)
                {
                    //不用合约乘数TradeAmount
                    decimal positiveAmount = m_orderBookList.Where(o => o.OrderSide == this.OrderSide).Sum(o => (o.TradePrice* o.TradeQty));
                    decimal negativeAmount = m_orderBookList.Where(o => o.OrderSide == this.OrderSide.GetOppositeOrderSide()).Sum(o => (o.TradePrice * o.TradeQty));
                    return (positiveAmount - negativeAmount) / tradeQty;
                }
                else
                {
                    return 0m;
                }
            }
        }

        /// <summary>
        /// 是否有未完成委托单。
        /// </summary>
        [XmlIgnoreAttribute]
        public bool HasUnFinishOrderBook
        {
            get
            {
                return (m_orderBookList.Count(o => o.IsFinish == false) > 0);
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
                List<USeOrderBook> unFinishList = (from o in m_orderBookList
                                                   where o.IsFinish == false
                                                   select o).ToList();
                Debug.Assert(unFinishList != null);
                return unFinishList;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// 委托单是否属于该任务。
        /// </summary>
        /// <param name="orderNum"></param>
        /// <returns></returns>
        public bool ContainsOrderBook(USeOrderNum orderNum)
        {
            return (m_orderBookList.Exists(o => o.OrderNum.Equals(orderNum)));
        }

        /// <summary>
        /// 新增正向委托。
        /// </summary>
        /// <param name="orderBook">正向委托回报。</param>
        public void AddPositiveOrderBook(USeOrderBook orderBook)
        {
            Debug.Assert(ContainsOrderBook(orderBook.OrderNum) == false);
            Debug.Assert(orderBook.Instrument == this.Instrument);
            Debug.Assert(orderBook.OrderSide == this.OrderSide);
            if (this.OffsetType == USeOffsetType.Open)
            {
                Debug.Assert(orderBook.OffsetType == this.OffsetType);
            }
            else
            {
                Debug.Assert((orderBook.OffsetType == USeOffsetType.Close) ||
                    (orderBook.OffsetType == USeOffsetType.CloseHistory) ||
                    (orderBook.OffsetType == USeOffsetType.CloseToday));
            }

            m_orderBookList.Add(orderBook);
        }

        /// <summary>
        /// 新增反向委托。
        /// </summary>
        /// <param name="orderBook">反向委托回报。</param>
        public void AddNegativeOrderBook(USeOrderBook orderBook)
        {
            Debug.Assert(ContainsOrderBook(orderBook.OrderNum) == false);
            Debug.Assert(orderBook.Instrument == this.Instrument);
            Debug.Assert(orderBook.OrderSide == this.OrderSide.GetOppositeOrderSide());
            if(this.OffsetType == USeOffsetType.Open)
            {
                Debug.Assert(orderBook.OffsetType == USeOffsetType.Close ||
                    orderBook.OffsetType == USeOffsetType.CloseHistory ||
                    orderBook.OffsetType == USeOffsetType.CloseToday);
            }
            else
            {
                Debug.Assert(orderBook.OffsetType == USeOffsetType.Open);
            }

            m_orderBookList.Add(orderBook);
        }

        /// <summary>
        /// 更新委托回报。
        /// </summary>
        /// <param name="orderBook">委托回报。</param>
        /// <returns>更新委托回报条数。</returns>
        public OrderBookUpdateResult UpdateOrderBook(USeOrderBook orderBook)
        {
            for (int i = 0; i < m_orderBookList.Count; i++)
            {
                USeOrderBook orderBookItem = m_orderBookList[i];
                if (orderBookItem.OrderNum.Equals(orderBook.OrderNum))
                {
                    OrderBookUpdateResult result = GetOrderBookUpdateResult(m_orderBookList[i], orderBook);
                    m_orderBookList[i] = orderBook.Clone();
                    return result;
                }
            }

            return null;
        }

        private OrderBookUpdateResult GetOrderBookUpdateResult(USeOrderBook oldOrderBook,USeOrderBook newOrderBook)
        {
            OrderBookUpdateResult result = new OrderBookUpdateResult() {
                TradeQty = newOrderBook.TradeQty - oldOrderBook.TradeQty,
                CancelQty = newOrderBook.CancelQty - oldOrderBook.CancelQty,
                BlankQty = newOrderBook.BlankQty - oldOrderBook.BlankQty
            };
            return result;
        }

        /// <summary>
        /// 重置失败下单次数。
        /// </summary>
        public void ResetTryOrderCount()
        {
            this.TryOrderCount = 0;
            this.LastOrderTime = null;
        }

        /// <summary>
        /// 下单次数累加1。
        /// </summary>
        public void AddTryOrderCount()
        {
            this.TryOrderCount = this.TryOrderCount + 1;
            this.LastOrderTime = DateTime.Now;
        }

        /// <summary>
        /// 是否可以继续下单。
        /// </summary>
        /// <param name="orderInterval"></param>
        public bool CanPlaceNextOrder(TimeSpan orderInterval)
        {
            if(this.LastOrderTime.HasValue == false)
            {
                return true;
            }
            else
            {
                return (this.LastOrderTime.Value.Add(orderInterval) <= DateTime.Now);
            }
        }
        #endregion

        #region Clone
        public ArbitrageSubTask Clone()
        {
            ArbitrageSubTask subTask = new ArbitrageSubTask();
            subTask.Instrument = this.Instrument == null ? null : this.Instrument.Clone();
            subTask.OrderPriceType = this.OrderPriceType;
            subTask.OrderSide = this.OrderSide;
            subTask.OffsetType = this.OffsetType;
            subTask.PlanOrderQty = this.PlanOrderQty;

            List<USeOrderBook> orderBookList = new List<USeOrderBook>();
            foreach (USeOrderBook orderBookItem in m_orderBookList)
            {
                orderBookList.Add(orderBookItem.Clone());
            }
            subTask.m_orderBookList = orderBookList;

            return subTask;
        }
        #endregion
    }
}
