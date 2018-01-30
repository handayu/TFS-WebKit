#region Copyright & Version
//==============================================================================
// 文件名称: USeOrderPriceType.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/12
// 描    述: USe下单价格类型枚举定义。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe下单价格类型枚举定义。
    /// </summary>
    public enum USeOrderPriceType
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 限价单。
        /// </summary>
        LimitPrice = 1,

        /// <summary>
        /// 市价单。
        /// </summary>
        MarketPrice = 2,
    }
}
