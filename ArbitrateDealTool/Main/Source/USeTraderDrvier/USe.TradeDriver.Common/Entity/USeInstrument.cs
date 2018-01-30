#region Copyright & Version
//==============================================================================
// 文件名称: USeProduct.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/12
// 描    述: USe产品定义。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe产品定义。
    /// </summary>
    public class USeInstrument
    {
        #region construction
        /// <summary>
        /// 构造USeProduct实例。
        /// </summary>
        private USeInstrument()
        {
        }

        /// <summary>
        /// 构造USeProduct实例。
        /// </summary>
        /// <param name="instrumentCode">合约代码。</param>
        /// <param name="instrumentName">合约名称。</param>
        /// <param name="market">所属市场。</param>
        public USeInstrument(string instrumentCode, string instrumentName, USeMarket market)
        {
            this.InstrumentCode = instrumentCode;
            this.InstrumentName = instrumentName;
            this.Market = market;
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 产品代码。
        /// </summary>
        public string InstrumentCode
        {
            get;
            set;
        }

        /// <summary>
        /// 产品名称。
        /// </summary>
        public string InstrumentName
        {
            get;
            set;
        }

        /// <summary>
        /// 市场。
        /// </summary>
        public USeMarket Market
        {
            get;
            set;
        }
        #endregion property

        #region methods
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;

            USeInstrument product = obj as USeInstrument;
            //return this.InstrumentCode.Equals(product.InstrumentCode, StringComparison.OrdinalIgnoreCase);
            //产品比较只比较产品代码,因Ctp交易环境不同代码所属市场不同
            if (this.InstrumentCode.Equals(product.InstrumentCode, StringComparison.OrdinalIgnoreCase))
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

        public static bool operator ==(USeInstrument product1, USeInstrument product2)
        {
            return Object.Equals(product1, product2);
        }

        public static bool operator !=(USeInstrument product1, USeInstrument product2)
        {
            return !Object.Equals(product1, product2);
        }


        public override string ToString()
        {
            return string.Format("{0}", this.InstrumentCode);
        }

        /// <summary>
        /// 克隆USeProduct。
        /// </summary>
        /// <returns></returns>
        public USeInstrument Clone()
        {
            USeInstrument product = new USeInstrument();
            product.InstrumentCode = this.InstrumentCode;
            product.InstrumentName = this.InstrumentName;
            product.Market = this.Market;
            return product;
        }
        #endregion // methods
    }

    /// <summary>
    /// USe产品比较器。
    /// </summary>
    public class USeInstrumentComparer : IEqualityComparer<USeInstrument>
    {
        #region implement IEqualityComparer
        public bool Equals(USeInstrument product1, USeInstrument product2)
        {
            return product1.Equals(product2);
        }

        public int GetHashCode(USeInstrument product)
        {
            return product.ToString().GetHashCode();
        }
        #endregion // implement IEqualityComparer
    }
}
