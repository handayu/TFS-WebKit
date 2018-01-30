#region Copyright & Version
//==============================================================================
// 文件名称: CtpQuoteDriver.Utility.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Justin Shen
// 创建日期: 2012/05/11
// 描    述: CTP行情驱动类-- 市场转换方法。
//==============================================================================
#endregion

using System;

using USe.TradeDriver.Common;

namespace USe.TradeDriver.Ctp
{
    public partial class CtpQuoteDriver
    {
        /// <summary>
        /// Ctp市场转换为USe市场。
        /// </summary>
        /// <param name="ctpExchange">Ctp市场字符串</param>
        /// <returns>USeMarket对象</returns>
        public USeMarket FtdcExchangeToUSeMarket(string ctpExchange)
        {
            ctpExchange = ctpExchange.ToUpper();
            switch (ctpExchange)
            {
                case "DCE": return USeMarket.DCE;
                case "SHFE": return USeMarket.SHFE;
                case "CZCE": return USeMarket.CZCE;
                case "CFFEX": return USeMarket.CFFEX;
                default: return USeMarket.Unknown;
            }
        }

        /// <summary>
        /// USe市场转换为Ctp市场。
        /// </summary>
        /// <param name="market">USeMarket对象</param>
        /// <returns>Ctp市场字符串</returns>
        public string USeMarketToFtdcExchange(USeMarket market)
        {
            switch (market)
            {
                case USeMarket.DCE: return "DCE";
                case USeMarket.SHFE: return "SHFE";
                case USeMarket.CZCE: return "CZCE";
                case USeMarket.CFFEX: return "CFFEX";
                default: return string.Empty;
            }
        }

        
    }
}
