#region Copyright & Version
//==============================================================================
// 文件名称: USeOptionsType.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2014 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2014/04/03
// 描    述: USe产品类型枚举定义。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 期权类型枚举定义。
    /// </summary>
    public enum USeOptionsType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Call
        /// </summary>
        Call = 1,

        /// <summary>
        /// Put
        /// </summary>
        Put = 2,
    }
}
