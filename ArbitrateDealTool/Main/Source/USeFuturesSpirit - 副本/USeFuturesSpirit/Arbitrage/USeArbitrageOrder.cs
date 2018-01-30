using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using USe.TradeDriver.Common;
using USeFuturesSpirit.Arbitrage;
namespace USeFuturesSpirit
{
    /// <summary>
    /// 套利单。
    /// </summary>
    public class USeArbitrageOrder
    {
        #region member
        private ArbitrageOrderState m_state = ArbitrageOrderState.None;
        private ArbitrageArgument m_argument = null;

        private ArbitrageTaskGroup m_openTaskGroup = null;   // 开仓任务组
        private ArbitrageTaskGroup m_closeTaskGroup = null;  // 平仓任务组
        #endregion

        #region 构造方法
        public USeArbitrageOrder()
        {
            this.TraderIdentify = Guid.Empty;
            m_openTaskGroup = new ArbitrageTaskGroup();
            m_closeTaskGroup = new ArbitrageTaskGroup();
        }
        #endregion

        #region property
        /// <summary>
        /// 自动下单机标识。
        /// </summary>
        public Guid TraderIdentify { get; set; }

        /// <summary>
        /// 别名序号。
        /// </summary>
        public int AliasNum { get; set; }

        /// <summary>
        /// 经纪商ID。
        /// </summary>
        public string BrokerId { get; set; }

        /// <summary>
        /// 账号。
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 套利单状态。
        /// </summary>
        public ArbitrageOrderState State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        /// <summary>
        /// 创建日期。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 结束日期。
        /// </summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// 套利参数。
        /// </summary>
        public ArbitrageArgument Argument
        {
            get { return m_argument; }
            set { m_argument = value; }
        }

        /// <summary>
        /// 开仓参数。
        /// </summary>
        [XmlIgnore]
        public ArbitrageOpenArgument OpenArgument
        {
            get
            {
                if (m_argument != null)
                {
                    return m_argument.OpenArg;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 平仓参数。
        /// </summary>
        [XmlIgnore]
        public ArbitrageCloseArgument CloseArgument
        {
            get
            {
                if (m_argument != null)
                {
                    return m_argument.CloseArg;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 平仓止损参数。
        /// </summary>
        [XmlIgnore]
        public ArbitrageStopLossArgument StopLoosArgument
        {
            get
            {
                if (m_argument != null)
                {
                    return m_argument.StopLossArg;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 开仓任务组。
        /// </summary>
        public ArbitrageTaskGroup OpenTaskGroup
        {
            get { return m_openTaskGroup;}
            set { m_openTaskGroup = value; }
        }

        /// <summary>
        /// 平仓任务组。
        /// </summary>
        public ArbitrageTaskGroup CloseTaskGroup
        {
            get { return m_closeTaskGroup;}
            set { m_closeTaskGroup = value; }
        }

        /// <summary>
        /// 结算结果。
        /// </summary>
        public ArbitrageOrderSettlement SettlementResult
        {
            get;
            set;
        }
        #endregion

        #region 只读属性
        /// <summary>
        /// 套利单别名。
        /// </summary>
        [XmlIgnore]
        public string Alias
        {
            get { return string.Format("{0:MMdd}_{1}", this.CreateTime, this.AliasNum.ToString().PadLeft(3, '0')); }
        }

        /// <summary>
        /// 是否有未完成委托单。
        /// </summary>
        [XmlIgnoreAttribute]
        public bool HasUnFinishOrderBook
        {
            get
            {
                if(m_openTaskGroup.HasUnFinishOrderBook)
                {
                    return true;
                }
                if(m_closeTaskGroup.HasUnFinishOrderBook)
                {
                    return true;
                }

                return false;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// 更新委托回报。
        /// </summary>
        /// <param name="orderBook">委托回报。</param>
        /// <returns>影响的任务。</returns>
        public OrderBookUpdateResult UpdateOrderBook(USeOrderBook orderBook)
        {
            OrderBookUpdateResult result = m_openTaskGroup.UpdateOrderBook(orderBook);
            if (result != null) return result;

            result = m_closeTaskGroup.UpdateOrderBook(orderBook);
            if (result != null) return result;

            return null;
        }

        /// <summary>
        /// 获取所有未完成委托单。
        /// </summary>
        /// <returns></returns>
        public List<USeOrderBook> GetAllUnfinishOrderBooks()
        {
            List<USeOrderBook> list = new List<USeOrderBook>();
            list.AddRange(m_openTaskGroup.GetAllUnFinishOrderBooks());
            list.AddRange(m_closeTaskGroup.GetAllUnFinishOrderBooks());

            return list;
        }

        /// <summary>
        /// 获取所有未完成委托单。
        /// </summary>
        /// <returns></returns>
        public List<USeOrderBook> GetAllOrderBooks()
        {
            List<USeOrderBook> list = new List<USeOrderBook>();
            list.AddRange(m_openTaskGroup.GetAllOrderBooks());
            list.AddRange(m_closeTaskGroup.GetAllOrderBooks());

            return list;
        }

        /// <summary>
        /// 更新检查套利单状态。
        /// </summary>
        public void UpdataArbitrageOrderState()
        {
            switch(m_state)
            {
                case ArbitrageOrderState.Opening:
                    //检查建仓是否完成，如果完成设置为建仓完成
                    if(m_openTaskGroup.FinishTaskCount == m_openTaskGroup.TaskCount)
                    {
                        m_state = ArbitrageOrderState.Opened;
                    }
                    break;
                case ArbitrageOrderState.Closeing:
                    //检查平仓是否完成，如果完成设置为平仓完成
                    if (m_closeTaskGroup.FinishTaskCount == m_closeTaskGroup.TaskCount)
                    {
                        m_state = ArbitrageOrderState.Closed;
                    }
                    break;
            }
        }

        /// <summary>
        /// 重置失败下单次数。
        /// </summary>
        public void ResetTryOrderErrorCount()
        {
            m_openTaskGroup.ResetTryOrderErrorCount();
            m_closeTaskGroup.ResetTryOrderErrorCount();
        }
        #endregion

        #region clone
        /// <summary>
        /// 克隆。
        /// </summary>
        /// <returns></returns>
        public USeArbitrageOrder Clone()
        {
            USeArbitrageOrder order = new USeArbitrageOrder();
            order.TraderIdentify = this.TraderIdentify;
            order.AliasNum = this.AliasNum;
            order.BrokerId = this.BrokerId;
            order.Account = this.Account;
            order.State = this.State;
            order.CreateTime = this.CreateTime;
            order.FinishTime = this.FinishTime;
            if (m_argument != null)
            {
                order.Argument = m_argument.Clone();
            }
            if (m_openTaskGroup != null)
            {
                order.m_openTaskGroup = m_openTaskGroup.Clone();
            }
            if (m_closeTaskGroup != null)
            {
                order.m_closeTaskGroup = m_closeTaskGroup.Clone();
            }

            return order;
        }
        #endregion 

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Alias))
            {
                return "套利单<Null>";
            }
            else
            {
                return string.Format("套利单<{0}>", this.Alias);
            }
        }
    }
}
