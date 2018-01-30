#region Copyright & Version
//==============================================================================
// 文件名称: USeMarket.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/12
// 描    述: USe市场枚举定义。
//
// 修 改 人: Yang Ming
// 修改日期: 2014/05/13
// 描    述: 增加上交所和深交所两个市场。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 市场枚举定义。
    /// </summary>
    public enum USeMarket
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 中国金融交易所。
        /// </summary>
        CFFEX = 1,

        /// <summary>
        /// 大连商品交易所。
        /// </summary>
        DCE = 2,

        /// <summary>
        /// 上海期货交易所。
        /// </summary>
        SHFE = 3,

        /// <summary>
        /// 郑州商品交易所。
        /// </summary>
        CZCE = 4,

        /// <summary>
        /// 上海能源交易中心。
        /// </summary>
        INE = 5,

        /// <summary>
        /// 上海证券交易所。
        /// </summary>
        SH = 11,

        /// <summary>
        /// 深证证券交易所。
        /// </summary>
        SZ = 12,

        /// <summary>
        /// 伦敦金属交易所。
        /// </summary>
        LME = 21,

        /// <summary>
        /// 纽约商业交易所。
        /// </summary>
        COMEX = 22,

        /// <summary>
        /// 东京商品交易所。
        /// </summary>
        TOCOM = 23,

        /// <summary>
        /// 新加坡商品交易所。
        /// </summary>
        SGX = 24,
    }

    /// <summary>
    /// USeMarket扩展类。
    /// </summary>
    public static class USeMarketExtend
    {
        /// <summary>
        /// 市场中文描述。
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        public static string ToDescription(this USeMarket market)
        {
            switch (market)
            {
                case USeMarket.CFFEX: return "中金所";
                case USeMarket.CZCE: return "郑商所";
                case USeMarket.DCE: return "大商所";
                case USeMarket.SHFE: return "上期所";
                case USeMarket.INE: return "能源中心";
                case USeMarket.SH: return "上交所";
                case USeMarket.SZ: return "深交所";
                case USeMarket.LME: return "伦敦金属";
                case USeMarket.COMEX: return "纽约商业";
                case USeMarket.TOCOM:return "东京交易所";
                case USeMarket.SGX:return "新加坡交易所";
                default: return string.Empty;
            }
        }
    }
}
