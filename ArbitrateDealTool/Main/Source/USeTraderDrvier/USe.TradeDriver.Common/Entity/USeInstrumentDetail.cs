#region Copyright & Version
//==============================================================================
// 文件名称: USeInstrumentDetail.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/12
// 描    述: USe合约详细信息。
//
// 修 改 人: Yang Ming
// 修改日期: 2014/04/03
// 描    述: 增加期权合约相关字段。
//==============================================================================
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe合约详细信息。
    /// </summary>
    public class USeInstrumentDetail
    {
        #region property
        /// <summary>
        /// 合约。
        /// </summary>
        public USeInstrument Instrument { get; set; }

        /// <summary>
        /// 创建日期。
        /// </summary>
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// 到期日。
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 开始交割日。
        /// </summary>
        public DateTime StartDelivDate { get; set; }

        /// <summary>
        /// 交割日。
        /// </summary>
        public DateTime EndDelivDate { get; set; }

        /// <summary>
        /// 合约乘数。
        /// </summary>
        public int VolumeMultiple { get; set; }

        /// <summary>
        /// 是否允许交易。
        /// </summary>
        public bool IsTrading { get; set; }

        /// <summary>
        /// 品种代码。
        /// </summary>
        public string Varieties { get; set; }

        /// <summary>
        /// 最小变动价位。
        /// </summary>
        public decimal PriceTick { get; set; }

        /// <summary>
        /// 交易所多头保证金率。
        /// </summary>
        public decimal ExchangeLongMarginRatio { get; set; }

        /// <summary>
        /// 交易所空头保证金率。
        /// </summary>
        public decimal ExchangeShortMarginRatio { get; set; }

        /// <summary>
        /// 产品类型。
        /// </summary>
        public USeProductClass ProductClass { get; set; }

        /// <summary>
        /// 基础商品代码。
        /// </summary>
        public string UnderlyingInstrument { get; set; }

        /// <summary>
        /// 期权执行价。
        /// </summary>
        public decimal StrikePrice { get; set; }

        /// <summary>
        /// 期权类型。
        /// </summary>
        public USeOptionsType OptionsType { get; set; }

        /// <summary>
        /// 合约系列。
        /// </summary>
        public string InstrumentSerial { get; set; }

        /// <summary>
        /// 市价单最大下单量。
        /// </summary>
        public int MaxMarketOrderVolume { get; set; }

        /// <summary>
        /// 市价单最小下单量。
        /// </summary>
        public int MinMarketOrderVolume { get; set; }

        /// <summary>
        /// 限价单最大下单量。
        /// </summary>
        public int MaxLimitOrderVolume { get; set; }

        /// <summary>
        /// 限价单最小下单量。
        /// </summary>
        public int MinLimitOrderVolume { get; set; }
        #endregion // property

        #region methods
        /// <summary>
        /// 克隆USeInstrumentDetail对象。
        /// </summary>
        /// <returns></returns>
        public USeInstrumentDetail Clone()
        {
            USeInstrumentDetail detail = new USeInstrumentDetail();
            detail.Instrument = this.Instrument;
            detail.OpenDate = this.OpenDate;
            detail.ExpireDate = this.ExpireDate;
            detail.StartDelivDate = this.StartDelivDate;
            detail.EndDelivDate = this.EndDelivDate;
            detail.VolumeMultiple = this.VolumeMultiple;
            detail.IsTrading = this.IsTrading;
            detail.Varieties = this.Varieties;
            detail.PriceTick = this.PriceTick;
            detail.ExchangeLongMarginRatio = this.ExchangeLongMarginRatio;
            detail.ExchangeShortMarginRatio = this.ExchangeShortMarginRatio;
            detail.ProductClass = this.ProductClass;
            detail.UnderlyingInstrument = this.UnderlyingInstrument;
            detail.StrikePrice = this.StrikePrice;
            detail.OptionsType = this.OptionsType;
            detail.InstrumentSerial = this.InstrumentSerial;

            detail.MaxMarketOrderVolume = this.MaxMarketOrderVolume;
            detail.MinMarketOrderVolume = this.MinMarketOrderVolume;
            detail.MaxLimitOrderVolume = this.MaxLimitOrderVolume;
            detail.MinLimitOrderVolume = this.MinLimitOrderVolume;

            return detail;
        }
        #endregion // methods
    }
}
