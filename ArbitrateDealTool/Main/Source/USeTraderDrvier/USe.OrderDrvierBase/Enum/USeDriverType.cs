#region Copyright & Version
//==============================================================================
// 文件名称: USeDriverType.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/07/06
// 描    述: USe交易驱动类型枚举。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe交易驱动类型枚举。
    /// </summary>
    public enum USeDriverType
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 正式环境。
        /// </summary>
        Real = 1,

        /// <summary>
        /// 模拟环境。
        /// </summary>
        Simulate = 2,

        /// <summary>
        /// 仿真环境。
        /// </summary>
        Imitation = 3,
    }
}
