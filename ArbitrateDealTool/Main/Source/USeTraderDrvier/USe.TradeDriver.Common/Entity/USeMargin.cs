#region Copyright & Version
//==============================================================================
// 文件名称: USeMargin.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 保证金信息定义。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 合约保证金。
    /// </summary>
    public class USeMargin
    {
        #region property
        /// <summary>
        /// 合约。
        /// </summary>
        public USeInstrument Instrument
        {
            get;
            set;
        }

        /// <summary>
        /// 交易所多头保证金率。
        /// </summary>
        public decimal ExchangeLongMarginRatio
        {
            get;
            set;
        }

        /// <summary>
        /// 交易所空头保证金率。
        /// </summary>
        public decimal ExchangeShortMarginRatio
        {
            get;
            set;
        }

        /// <summary>
        /// 期货公司多头保证金率。
        /// </summary>
        public decimal BrokerLongMarginRatioByMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 期货公司多头保证金费
        /// </summary>
        public decimal BrokerLongMarginRatioByVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 期货公司空头保证金率
        /// </summary>
        public decimal BrokerShortMarginRatioByMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 期货公司空头保证金费
        /// </summary>
        public decimal BrokerShortMarginRatioByVolume
        {
            get;
            set;
        }
        #endregion // property

        /// <summary>
        /// 克隆USeMargin对象。
        /// </summary>
        /// <returns></returns>
        public USeMargin Clone()
        {
            USeMargin margin = new USeMargin();
            margin.Instrument = this.Instrument.Clone();
            margin.ExchangeLongMarginRatio = this.ExchangeLongMarginRatio;
            margin.ExchangeShortMarginRatio = this.ExchangeShortMarginRatio;
            margin.BrokerLongMarginRatioByMoney = this.BrokerLongMarginRatioByMoney;
            margin.BrokerLongMarginRatioByVolume = this.BrokerLongMarginRatioByVolume;
            margin.BrokerShortMarginRatioByMoney = this.BrokerShortMarginRatioByMoney;
            margin.BrokerShortMarginRatioByVolume = this.BrokerShortMarginRatioByVolume;

            return margin;
        }
    }
}
