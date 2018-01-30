#region Copyright & Version
//==============================================================================
// 文件名称: CtpProtocol.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: CTP相关常数、公共方法等的定义。
//==============================================================================
#endregion

using System;

using USe.TradeDriver.Common;

namespace USe.TradeDriver.Ctp
{
    /// <summary>
    /// CTP相关常数、公共方法等的定义。
    /// </summary>
    internal static class CtpProtocol
    {
        /// <summary>
        /// Ctp市场转换为USe市场。
        /// </summary>
        /// <param name="ctpExchange"></param>
        /// <returns></returns>
        public static USeMarket FtdcExchangeToUSeMarket(string ctpExchange)
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
        public static string USeMarketToFtdcExchange(USeMarket market)
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
    }
}
