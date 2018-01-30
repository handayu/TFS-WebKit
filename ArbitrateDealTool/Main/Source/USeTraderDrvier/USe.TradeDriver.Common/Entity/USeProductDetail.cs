using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe产品信息。
    /// </summary>
    public class USeProductDetail
    {
        /// <summary>
        /// 产品代码。
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 交易所。
        /// </summary>
        public USeMarket Market { get; set; }

        /// <summary>
        /// 产品名称缩写。
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 产品名称全称。
        /// </summary>
        public string LongName { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public USeProductClass ProductClass { get; set; }

        /// <summary>
        /// 合约数量乘数
        /// </summary>
        public decimal VolumeMultiple { get; set; }

        /// <summary>
        /// 最小变动价位
        /// </summary>
        public decimal PriceTick { get; set; }

        /// <summary>
        /// 市价单最大下单量
        /// </summary>
        public int MaxMarketOrderVolume { get; set; }

        /// <summary>
        /// 市价单最小下单量
        /// </summary>
        public int MinMarketOrderVolume { get; set; }

        /// <summary>
        /// 限价单最大下单量
        /// </summary>
        public int MaxLimitOrderVolume { get; set; }

        /// <summary>
        /// 限价单最小下单量
        /// </summary>
        public int MinLimitOrderVolume { get; set; }

        /// <summary>
        /// 克隆USeProduct对象。
        /// </summary>
        /// <returns></returns>
        public USeProductDetail Clone()
        {
            USeProductDetail entity = new USeProductDetail();
            entity.ProductCode = this.ProductCode;
            entity.ShortName = this.ShortName;
            entity.LongName = this.LongName;
            entity.Market = this.Market;
            entity.ProductClass = this.ProductClass;
            entity.VolumeMultiple = this.VolumeMultiple;
            entity.PriceTick = this.PriceTick;
            entity.MaxMarketOrderVolume = this.MaxMarketOrderVolume;
            entity.MinMarketOrderVolume = this.MinMarketOrderVolume;
            entity.MaxLimitOrderVolume = this.MaxLimitOrderVolume;
            entity.MinLimitOrderVolume = this.MinLimitOrderVolume;

            return entity;
        }
    }
}
