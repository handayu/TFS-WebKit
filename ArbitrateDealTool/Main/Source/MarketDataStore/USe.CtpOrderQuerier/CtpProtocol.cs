using System;
using System.Collections.Generic;
using System.Text;

using USe.TradeDriver.Common;
using CTPAPI;

namespace USe.CtpOrderQuerier
{
    /// <summary>
    /// CTP相关常数、公共方法等的定义。
    /// </summary>
    public static class CtpProtocol
    {
        /// <summary>
        /// Ctp市场转换为USe市场。
        /// </summary>
        /// <param name="ctpExchange"></param>
        /// <returns></returns>
        public static USeMarket CtpExchangeToUSeMarket(string ctpExchange)
        {
            ctpExchange = ctpExchange.ToUpper();
            switch (ctpExchange)
            {
                case "DCE": return USeMarket.DCE;
                case "SHFE": return USeMarket.SHFE;
                case "CZCE": return USeMarket.CZCE;
                case "CFFEX": return USeMarket.CFFEX;
                case "INE":return USeMarket.INE;
                default: return USeMarket.Unknown;
            }
        }

        /// <summary>
        /// USe市场转换为Ctp市场。
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        public static string USeMarketToCtpExchange(USeMarket market)
        {
            switch (market)
            {
                case USeMarket.DCE: return "DCE";
                case USeMarket.SHFE: return "SHFE";
                case USeMarket.CZCE: return "CZCE";
                case USeMarket.CFFEX: return "CFFEX";
                case USeMarket.INE:return "INE";
                default: return string.Empty;
            }
        }

        /// <summary>
        /// Ctp市场转换为USe市场。
        /// </summary>
        /// <param name="ctpExchange"></param>
        /// <returns></returns>
        public static USeProductClass CtpProductClassToUSeProductClass(ProductClass productClass)
        {
            switch (productClass)
            {
                case ProductClass.Futures:return USeProductClass.Futures;
                case ProductClass.Options:return USeProductClass.Options;
                case ProductClass.Combination:return USeProductClass.Combination;
                case ProductClass.Spot:return USeProductClass.Spot;
                case ProductClass.EFP:return USeProductClass.EFP;
                default: return USeProductClass.Unknown;
            }
        }
    }
}
