using System;
using System.Diagnostics;
using USe.Common;
using USe.TradeDriver.Common;

namespace ContractIndexTestForm
{
    public sealed class MarketDataViewModel : USeBaseViewModel
    {
        #region member
        private USeInstrument m_instrument = null;
        private decimal m_askPrice = 0m;
        private int m_askSize = 0;
        private decimal m_bidPrice = 0m;
        private int m_bidSize = 0;
        private decimal m_openPrice = 0m;
        private decimal m_highPrice = 0m;
        private decimal m_lowPrice = 0m;
        private decimal m_lastPrice = 0m;
        private decimal m_closePrice = 0m;
        private decimal m_preClosePrice = 0m;
        private decimal m_upperLimitPrice = 0m;
        private decimal m_lowerLimitPrice = 0m;
        private decimal m_preSettlementPrice = 0m;
        private decimal m_settlementPrice = 0m;
        private decimal m_openInterest = 0m;
        private decimal m_preOpenInterest = 0m;
        private int m_volume = 0;
        private decimal m_turnover = 0m;
        private DateTime m_updateTime = DateTime.MinValue;
        #endregion
        public MarketDataViewModel(USeInstrument instrument,bool isIndex)
        {
            this.Instrument = instrument;
            this.IsIndex = isIndex;
        }
        #region property
        public bool IsIndex { get; set; }
        /// <summary>
        /// 合约。
        /// </summary>
        public USeInstrument Instrument
        {
            get { return m_instrument; }
            private set
            {
                m_instrument = value;
                SetProperty(() => this.Instrument);
                SetProperty(() => this.InstrumentName);
            }
        }

        /// <summary>
        /// 合约名称。
        /// </summary>
        public string InstrumentName
        {
            get
            {
                return this.Instrument != null ? this.Instrument.InstrumentName : string.Empty;
            }
        }

        /// <summary>
        /// 申卖价。
        /// </summary>
        public decimal AskPrice
        {
            get { return m_askPrice; }
            set
            {
                if (value != m_askPrice)
                {
                    m_askPrice = value;
                    SetProperty(() => this.AskPrice);
                }
            }
        }

        /// <summary>
        /// 申卖量。
        /// </summary>
        public int AskSize
        {
            get { return m_askSize; }
            set
            {
                if (value != m_askSize)
                {
                    m_askSize = value;
                    SetProperty(() => this.AskSize);
                }
            }
        }

        /// <summary>
        /// 申买价。
        /// </summary>
        public decimal BidPrice
        {
            get { return m_bidPrice; }
            set
            {
                if (value != m_bidPrice)
                {
                    m_bidPrice = value;
                    SetProperty(() => this.BidPrice);
                }
            }
        }

        /// <summary>
        /// 申买量。
        /// </summary>
        public int BidSize
        {
            get { return m_bidSize; }
            set
            {
                if (value != m_bidSize)
                {
                    m_bidSize = value;
                    SetProperty(() => this.BidSize);
                }
            }
        }

        /// <summary>
        /// 今开盘
        /// </summary>
        public decimal OpenPrice
        {
            get { return m_openPrice; }
            set
            {
                if (value != m_openPrice)
                {
                    m_openPrice = value;
                    SetProperty(() => this.OpenPrice);
                }
            }
        }

        /// <summary>
        /// 最高价。
        /// </summary>
        public decimal HighPrice
        {
            get { return m_highPrice; }
            set
            {
                if (value != m_highPrice)
                {
                    m_highPrice = value;
                    SetProperty(() => this.HighPrice);
                }
            }
        }

        /// <summary>
        /// 最低价。
        /// </summary>
        public decimal LowPrice
        {
            get { return m_lowPrice; }
            set
            {
                if (value != m_lowPrice)
                {
                    m_lowPrice = value;
                    SetProperty(() => this.LowPrice);
                }
            }
        }

        /// <summary>
        /// 最新价。
        /// </summary>
        public decimal LastPrice
        {
            get { return m_lastPrice; }
            set
            {
                if (value != m_lastPrice)
                {
                    m_lastPrice = value;
                    SetProperty(() => this.LastPrice);
                    SetProperty(() => this.NetChange);
                    SetProperty(() => this.PctChange);
                }
            }
        }

        /// <summary>
        /// 涨跌。
        /// </summary>
        public decimal NetChange
        {
            get
            {
                if (this.LastPrice <= 0 || this.PreSettlementPrice <= 0)
                {
                    return 0;
                }
                else
                {
                    return (this.LastPrice - this.PreSettlementPrice);
                }
            }
        }

        /// <summary>
        /// 涨跌幅度。
        /// </summary>
        public decimal PctChange
        {
            get
            {
                if (this.PreSettlementPrice == 0m || this.LastPrice == 0m)
                {
                    return 0m;
                }
                else
                {
                    return (this.LastPrice - this.PreSettlementPrice) / this.PreSettlementPrice;
                }
            }
        }

        /// <summary>
        /// 今收盘。
        /// </summary>
        public decimal ClosePrice
        {
            get { return m_closePrice; }
            set
            {
                if (value != m_closePrice)
                {
                    m_closePrice = value;
                    SetProperty(() => this.ClosePrice);
                }
            }
        }

        /// <summary>
        /// 昨收盘价
        /// </summary>
        public decimal PreClosePrice
        {
            get { return m_preClosePrice; }
            set
            {
                if (value != m_preClosePrice)
                {
                    m_preClosePrice = value;
                    SetProperty(() => this.PreClosePrice);
                }
            }
        }
        /// <summary>
        /// 涨停板价
        /// </summary>
        public decimal UpperLimitPrice
        {
            get { return m_upperLimitPrice; }
            set
            {
                if (value != m_upperLimitPrice)
                {
                    m_upperLimitPrice = value;
                    SetProperty(() => this.UpperLimitPrice);
                }
            }
        }

        /// <summary>
        /// 跌停板价。
        /// </summary>
        public decimal LowerLimitPrice
        {
            get { return m_lowerLimitPrice; }
            set
            {
                if (value != m_lowerLimitPrice)
                {
                    m_lowerLimitPrice = value;
                    SetProperty(() => this.LowerLimitPrice);
                }
            }
        }

        /// <summary>
        /// 上次结算价
        /// </summary>
        public decimal PreSettlementPrice
        {
            get { return m_preSettlementPrice; }
            set
            {
                if (value != m_preSettlementPrice)
                {
                    m_preSettlementPrice = value;
                    SetProperty(() => this.PreSettlementPrice);
                    SetProperty(() => this.NetChange);
                    SetProperty(() => this.PctChange);
                }
            }
        }

        public decimal SettlementPrice
        {
            get { return m_settlementPrice; }
            set
            {
                if (value != m_settlementPrice)
                {
                    m_settlementPrice = value;
                    SetProperty(() => this.SettlementPrice);
                }
            }
        }

        /// <summary>
        /// 持仓量
        /// </summary>
        public decimal OpenInterest
        {
            get { return m_openInterest; }
            set
            {
                if (value != m_openInterest)
                {
                    m_openInterest = value;
                    SetProperty(() => this.OpenInterest);
                }
            }
        }

        /// <summary>
        /// 昨日持仓量
        /// </summary>
        public decimal PreOpenInterest
        {
            get { return m_preOpenInterest; }
            set
            {
                if (value != m_preOpenInterest)
                {
                    m_preOpenInterest = value;
                    SetProperty(() => this.PreOpenInterest);
                }
            }
        }

        /// <summary>
        /// 成交量
        /// </summary>
        public int Volume
        {
            get { return m_volume; }
            set
            {
                if (value != m_volume)
                {
                    m_volume = value;
                    SetProperty(() => this.Volume);
                }
            }
        }

        /// <summary>
        /// 成交金额
        /// </summary>
        public decimal Turnover
        {
            get { return m_turnover; }
            set
            {
                if (value != m_turnover)
                {
                    m_turnover = value;
                    SetProperty(() => this.Turnover);
                }
            }
        }

        /// <summary>
        /// 更新时间。
        /// </summary>
        public DateTime UpdateTime
        {
            get { return m_updateTime; }
            set
            {
                if (value != m_updateTime)
                {
                    m_updateTime = value;
                    SetProperty(() => this.UpdateTime);
                }
            }
        }
        #endregion


        public void Update(USeMarketData data)
        {
            if (data.Instrument.InstrumentCode != this.Instrument.InstrumentCode)
            {
                Debug.Assert(false);
                return;
            }

            this.AskPrice = data.AskPrice;
            this.AskSize = data.AskSize;
            this.BidPrice = data.BidPrice;
            this.BidSize = data.BidSize;
            this.OpenPrice = data.OpenPrice;
            this.HighPrice = data.HighPrice;
            this.LowPrice = data.LowPrice;
            this.LastPrice = data.LastPrice;
            this.ClosePrice = data.ClosePrice;
            this.PreClosePrice = data.PreClosePrice;
            this.UpperLimitPrice = data.UpperLimitPrice;
            this.LowerLimitPrice = data.LowerLimitPrice;
            this.PreSettlementPrice = data.PreSettlementPrice;
            this.SettlementPrice = data.SettlementPrice;
            this.OpenInterest = data.OpenInterest;
            this.PreOpenInterest = data.PreOpenInterest;
            this.Volume = data.Volume;
            this.Turnover = data.Turnover;
            this.UpdateTime = data.UpdateTime;
        }
    }
}
