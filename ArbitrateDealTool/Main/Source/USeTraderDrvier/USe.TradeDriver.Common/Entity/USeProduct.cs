using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe产品信息。
    /// </summary>
    public class USeProduct
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
        /// 产品简称。
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 产品名称。
        /// </summary>
        public string LongName { get; set; }

        /// <summary>
        /// 合约数量乘数
        /// </summary>
        public decimal VolumeMultiple { get; set; }

        /// <summary>
        /// 最小变动价位
        /// </summary>
        public decimal PriceTick { get; set; }

        #region methods
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;

            USeProduct product = obj as USeProduct;
            //return this.InstrumentCode.Equals(product.InstrumentCode, StringComparison.OrdinalIgnoreCase);
            //产品比较只比较产品代码,因Ctp交易环境不同代码所属市场不同
            if (this.ProductCode.Equals(product.ProductCode) && this.Market == product.Market)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static bool operator ==(USeProduct product1, USeProduct product2)
        {
            return Object.Equals(product1, product2);
        }

        public static bool operator !=(USeProduct product1, USeProduct product2)
        {
            return !Object.Equals(product1, product2);
        }


        public override string ToString()
        {
            return string.Format("{0}_{1}", this.ProductCode, this.Market.ToString());
        }
        #endregion

        /// <summary>
        /// 克隆USeProduct对象。
        /// </summary>
        /// <returns></returns>
        public USeProduct Clone()
        {
            USeProduct entity = new USeProduct();
            entity.ProductCode = this.ProductCode;
            entity.ShortName = this.ShortName;
            entity.LongName = this.LongName;
            entity.Market = this.Market;
            entity.VolumeMultiple = this.VolumeMultiple;
            entity.PriceTick = this.PriceTick;

            return entity;
        }
    }

    /// <summary>
    /// USe产品比较器。
    /// </summary>
    public class USeProductComparer : IEqualityComparer<USeProduct>
    {
        #region implement IEqualityComparer
        public bool Equals(USeProduct product1, USeProduct product2)
        {
            return product1.Equals(product2);
        }

        public int GetHashCode(USeProduct product)
        {
            return product.ToString().GetHashCode();
        }
        #endregion // implement IEqualityComparer
    }
}
