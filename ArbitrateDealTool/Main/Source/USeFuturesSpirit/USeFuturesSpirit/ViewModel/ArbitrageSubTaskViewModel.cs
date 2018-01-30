using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using USe.Common;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    class ArbitrageSubTaskViewModel : USeBaseViewModel
    {
        #region member
        private int m_taskId = 0;
        private USeOrderSide m_orderSide = USeOrderSide.Buy;
        private USeInstrument m_instrument = null;
        private int m_planOrderQty = 0;
        private int m_orderQty = 0;
        private int m_tradeQty = 0;
        #endregion

        #region property
        /// <summary>
        /// 任务ID。
        /// </summary>
        public int TaskId
        {
            get { return m_taskId; }
            set
            {
                m_taskId = value;
                SetProperty(() => this.TaskId);
            }
        }

        /// <summary>
        /// 买卖方向。
        /// </summary>
        public USeOrderSide OrderSide
        {
            get { return m_orderSide; }
            set
            {
                m_orderSide = value;
                SetProperty(() => this.OrderSide);
                SetProperty(() => this.OrderSideDesc);
            }
        }

        /// <summary>
        /// 买卖方向。
        /// </summary>
        public string OrderSideDesc
        {
            get { return m_orderSide.ToDescription(); }
        }

        /// <summary>
        /// 合约。
        /// </summary>
        public USeInstrument Instrument
        {
            get { return m_instrument; }
            set
            {
                m_instrument = value;
                SetProperty(() => this.Instrument);
            }
        }

        /// <summary>
        /// 计划委托数量。
        /// </summary>
        public int PlanOrderQty
        {
            get { return m_planOrderQty; }
            set
            {
                m_planOrderQty = value;
                SetProperty(() => this.PlanOrderQty);
                SetProperty(() => this.UnOrderQty);
            }
        }

        /// <summary>
        /// 已委托数量。
        /// </summary>
        public int OrderQty
        {
            get { return m_orderQty; }
            set
            {
                m_orderQty = value;
                SetProperty(() => this.OrderQty);
                SetProperty(() => this.UnOrderQty);
            }
        }

        /// <summary>
        /// 未委托数量。
        /// </summary>
        public int UnOrderQty
        {
            get { return (this.PlanOrderQty - this.OrderQty); }
        }

        /// <summary>
        /// 成交数量。
        /// </summary>
        public int TradeQty
        {
            get { return m_tradeQty; }
            set
            {
                m_tradeQty = value;
                SetProperty(() => this.TradeQty);
            }
        }
        #endregion 
    }
}
