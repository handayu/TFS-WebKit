#region
//==============================================================================
// 文件名称: USeMarkerData.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Justin Shen
// 创建日期: 2012/05/10
// 描    述: USe期货行情定义。
//==============================================================================
#endregion

using System;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe期货行情定义。
    /// </summary>
    public class USeMarketData
    {
        public USeMarketData()
        {
        }

        public USeMarketData(USeInstrument instrument)
        {
            this.Instrument = instrument;
        }

        /// <summary>
        /// 合约。
        /// </summary>
        public USeInstrument Instrument
        {
            get;
            set;
        }

        /// <summary>
        /// 申卖价。
        /// </summary>
        public decimal AskPrice
        {
            get;
            set;
        }
 
        /// <summary>
        /// 申卖量。
        /// </summary>
        public int AskSize
        {
            get;
            set;
        }

        /// <summary>
        /// 申买价。
        /// </summary>
        public decimal BidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 申买量。
        /// </summary>
        public int BidSize
        {
            get;
            set;
        }

        /// <summary>
        /// 今开盘
        /// </summary>
        public decimal OpenPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 最高价。
        /// </summary>
        public decimal HighPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 最低价。
        /// </summary>
        public decimal LowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 最新价。
        /// </summary>
        public decimal LastPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 今收盘。
        /// </summary>
        public decimal ClosePrice
        {
            get;
            set;
        }

        /// <summary>
        /// 昨收盘价
        /// </summary>
        public decimal PreClosePrice
        {
            get;
            set;
        }
        /// <summary>
        /// 涨停板价
        /// </summary>
        public decimal UpperLimitPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 跌停板价。
        /// </summary>
        public decimal LowerLimitPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 上次结算价
        /// </summary>
        public decimal PreSettlementPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 结算价
        /// </summary>
        public decimal SettlementPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 持仓量
        /// </summary>
        public decimal OpenInterest
        {
            get;
            set;
        }

        /// <summary>
        /// 昨日持仓量。
        /// </summary>
        public decimal PreOpenInterest
        {
            get;
            set;
        }

        /// <summary>
        /// 成交量
        /// </summary>
        public int Volume
        {
            get;
            set;
        }    

        /// <summary>
        /// 成交金额
        /// </summary>
        public decimal Turnover
        {
            get;
            set;
        }

        /// <summary>
        /// 更新时间。
        /// </summary>
        public DateTime UpdateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 平均价
        /// </summary>
        public double AvgPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 投机度
        /// </summary>
        public decimal SpeculateRadio
        {
            get;
            set;
        }

        /// <summary>
        /// 行情日期。
        /// </summary>
        public DateTime? QuoteDay { get; set; }    

        /// <summary>
        /// 行情时间。
        /// </summary>
        public TimeSpan? QuoteTime { get; set; }

        public USeMarketData Clone()
        {
            USeMarketData clone = new USeMarketData();
            clone.Instrument = Instrument.Clone();
            clone.AskPrice = AskPrice;
            clone.AskSize = AskSize;
            clone.BidPrice = BidPrice;
            clone.BidSize = BidSize;
            clone.OpenPrice = OpenPrice;
            clone.HighPrice = HighPrice;
            clone.LowPrice = LowPrice;
            clone.LastPrice = LastPrice;
            clone.ClosePrice = ClosePrice;
            clone.PreClosePrice = PreClosePrice;
            clone.UpperLimitPrice = UpperLimitPrice;
            clone.LowerLimitPrice = LowerLimitPrice;
            clone.PreSettlementPrice = PreSettlementPrice;
            clone.SettlementPrice = this.SettlementPrice;
            clone.OpenInterest = this.OpenInterest;
            clone.Volume = Volume;
            clone.Turnover = Turnover;
            clone.UpdateTime = UpdateTime;
            clone.QuoteDay = this.QuoteDay;
            clone.QuoteTime = this.QuoteTime;
            clone.AvgPrice = this.AvgPrice;
            clone.SpeculateRadio = this.SpeculateRadio;
            return clone;
        }
    }  
}