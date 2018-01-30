#region Copyright & Version
//==============================================================================
// 文件名称: USeMarginType.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 产品保证金比例类型枚举定义。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 产品保证金比例类型枚举定义。
    /// </summary>
    public enum USeMarginType
    {
        /// <summary>
        /// 按金额。
        /// </summary>
        ByMoney,

        /// <summary>
        /// 按交易量。
        /// </summary>
        ByVolume,
    }
}
