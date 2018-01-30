#region Copyright & Version
//==============================================================================
// 文件名称: USeFeeType.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 产品手续费收费类型枚举定义。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 产品手续费收费类型枚举定义。
    /// </summary>
    public enum USeFeeType
    {
        /// <summary>
        /// 按金额。
        /// </summary>
        ByMoney = 0,

        /// <summary>
        /// 按交易量。
        /// </summary>
        ByVolume = 1,
    }
}
