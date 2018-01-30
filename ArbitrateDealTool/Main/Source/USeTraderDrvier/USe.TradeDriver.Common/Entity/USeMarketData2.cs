#region
//==============================================================================
// 文件名称: USeMarkerData2.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/05/10
// 描    述: USe行情定义包含买卖5档。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe行情定义。
    /// </summary>
    public class USeMarketData2
    {
        public USeMarketData2()
        {
        }

        public USeMarketData2(USeInstrument instrument)
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
        /// 申卖价1。
        /// </summary>
        public decimal AskPrice1
        {
            get;
            set;
        }

        /// <summary>
        /// 申卖量1。
        /// </summary>
        public int AskSize1
        {
            get;
            set;
        }

        /// <summary>
        /// 申买价1。
        /// </summary>
        public decimal BidPrice1
        {
            get;
            set;
        }

        /// <summary>
        /// 申买量1。
        /// </summary>
        public int BidSize1
        {
            get;
            set;
        }

        /// <summary>
        /// 申卖价2。
        /// </summary>
        public decimal AskPrice2
        {
            get;
            set;
        }

        /// <summary>
        /// 申卖量2。
        /// </summary>
        public int AskSize2
        {
            get;
            set;
        }

        /// <summary>
        /// 申买价2。
        /// </summary>
        public decimal BidPrice2
        {
            get;
            set;
        }

        /// <summary>
        /// 申买量2。
        /// </summary>
        public int BidSize2
        {
            get;
            set;
        }

        /// <summary>
        /// 申卖价3。
        /// </summary>
        public decimal AskPrice3
        {
            get;
            set;
        }

        /// <summary>
        /// 申卖量3。
        /// </summary>
        public int AskSize3
        {
            get;
            set;
        }

        /// <summary>
        /// 申买价3。
        /// </summary>
        public decimal BidPrice3
        {
            get;
            set;
        }

        /// <summary>
        /// 申买量3。
        /// </summary>
        public int BidSize3
        {
            get;
            set;
        }

        /// <summary>
        /// 申卖价4。
        /// </summary>
        public decimal AskPrice4
        {
            get;
            set;
        }

        /// <summary>
        /// 申卖量4。
        /// </summary>
        public int AskSize4
        {
            get;
            set;
        }

        /// <summary>
        /// 申买价4。
        /// </summary>
        public decimal BidPrice4
        {
            get;
            set;
        }

        /// <summary>
        /// 申买量4。
        /// </summary>
        public int BidSize4
        {
            get;
            set;
        }

        /// <summary>
        /// 申卖价5。
        /// </summary>
        public decimal AskPrice5
        {
            get;
            set;
        }

        /// <summary>
        /// 申卖量5。
        /// </summary>
        public int AskSize5
        {
            get;
            set;
        }

        /// <summary>
        /// 申买价5。
        /// </summary>
        public decimal BidPrice5
        {
            get;
            set;
        }

        /// <summary>
        /// 申买量5。
        /// </summary>
        public int BidSize5
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
        /// 结算价。
        /// </summary>
        public decimal SettlementPrice
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

        public USeMarketData2 Clone()
        {
            USeMarketData2 clone = new USeMarketData2();
            clone.Instrument = Instrument.Clone();
            clone.AskPrice1 = AskPrice1;
            clone.AskSize1 = AskSize1;
            clone.BidPrice1 = BidPrice1;
            clone.BidSize1 = BidSize1;
            clone.AskPrice2 = AskPrice2;
            clone.AskSize2 = AskSize2;
            clone.BidPrice2 = BidPrice2;
            clone.BidSize2 = BidSize2;
            clone.AskPrice3 = AskPrice3;
            clone.AskSize3 = AskSize3;
            clone.BidPrice3 = BidPrice3;
            clone.BidSize3 = BidSize3;
            clone.AskPrice4 = AskPrice4;
            clone.AskSize4 = AskSize4;
            clone.BidPrice4 = BidPrice4;
            clone.BidSize4 = BidSize4;
            clone.AskPrice5 = AskPrice5;
            clone.AskSize5 = AskSize5;
            clone.BidPrice5 = BidPrice5;
            clone.BidSize5 = BidSize5;
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
            clone.Volume = Volume;
            clone.Turnover = Turnover;
            clone.UpdateTime = UpdateTime;
            
            return clone;
        }
    }  
}
