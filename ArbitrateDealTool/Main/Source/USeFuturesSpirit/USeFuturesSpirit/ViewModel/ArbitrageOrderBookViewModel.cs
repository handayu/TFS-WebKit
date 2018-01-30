using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common;
using USeFuturesSpirit.ViewModel;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public class ArbitrageOrderBookViewModel: USeBaseViewModel
    {
        #region member
        private DateTime m_creatTime = DateTime.Now;
        private DateTime? m_finishTime = DateTime.Now;
        private string  m_alias = string.Empty;
        private decimal m_buyProfit = 0m;
        private decimal m_sellProfit = 0m;
        private decimal m_totalProft = 0m;
        private USeInstrument m_openBuyInstrument = null;
        private USeInstrument m_openSellInstrument = null;
        private USeArbitrageOrder m_arbitrageOrder = null;
        #endregion

        #region property
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime CreateTime
        {
            get { return m_creatTime; }
            set
            {
                m_creatTime = value;
                SetProperty(() => this.CreateTime);
            }
        }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? FinishTime
        {
            get { return m_finishTime; }
            set
            {
                m_finishTime = value;
                SetProperty(() => this.FinishTime);
            }
        }

        /// <summary>
        /// 别名。
        /// </summary>
        public string Alias
        {
            get { return m_alias; }
            set
            {
                m_alias = value;
                SetProperty(() => this.Alias);
            }
        }

        /// <summary>
        /// 买入盈亏
        /// </summary>
        public decimal BuyProfit
        {
            get { return m_buyProfit; }
            set
            {
                m_buyProfit = value;
                SetProperty(() => this.BuyProfit);
            }
        }

        /// <summary>
        /// 卖出盈亏
        /// </summary>
        public decimal SellProfit
        {
            get { return m_sellProfit; }
            set
            {
                m_sellProfit = value;
                SetProperty(() => this.SellProfit);
            }
        }

        /// <summary>
        /// 总盈亏
        /// </summary>
        public decimal TotalProfit
        {
            get { return m_totalProft; }
            set
            {
                m_totalProft = value;
                SetProperty(() => this.TotalProfit);
            }
        }

        /// <summary>
        /// 开仓买入合约。
        /// </summary>
        public USeInstrument OpenBuyInstrument
        {
            get { return m_openBuyInstrument; }
            set
            {
                m_openBuyInstrument = value;
                SetProperty(() => this.OpenBuyInstrument);
            }
        }

        /// <summary>
        /// 开仓买入合约。
        /// </summary>
        public USeInstrument OpenSellInstrument
        {
            get { return m_openSellInstrument; }
            set
            {
                m_openSellInstrument = value;
                SetProperty(() => this.OpenSellInstrument);
            }
        }

        public USeArbitrageOrder ArbitrageOrder
        {
            get { return m_arbitrageOrder; }
            set
            {
                m_arbitrageOrder = value;
                SetProperty(() => this.ArbitrageOrder);
            }
        }
        #endregion

        #region Construct
        public static ArbitrageOrderBookViewModel Creat(USeArbitrageOrder arbitrageOrder)
        {
            ArbitrageOrderBookViewModel viewModel = new ArbitrageOrderBookViewModel();
            viewModel.CreateTime = arbitrageOrder.CreateTime;
            viewModel.FinishTime = arbitrageOrder.FinishTime;
            viewModel.Alias = arbitrageOrder.Alias;
            viewModel.OpenBuyInstrument = arbitrageOrder.OpenArgument.BuyInstrument;
            viewModel.OpenSellInstrument = arbitrageOrder.OpenArgument.SellInstrument;
            if (arbitrageOrder.SettlementResult != null)
            {
                viewModel.BuyProfit = arbitrageOrder.SettlementResult.BuyInstrumentProfit;
                viewModel.SellProfit = arbitrageOrder.SettlementResult.SellInstrumentProfit;
                viewModel.TotalProfit = arbitrageOrder.SettlementResult.Profit;
            }
            viewModel.ArbitrageOrder = arbitrageOrder;
            return viewModel;
        }
        #endregion
    }

}



