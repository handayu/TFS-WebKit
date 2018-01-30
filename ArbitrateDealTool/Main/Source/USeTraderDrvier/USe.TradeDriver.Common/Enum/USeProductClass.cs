#region Copyright & Version
//==============================================================================
// 文件名称: USeProductClass.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2014/04/03
// 描    述: USe产品类型枚举定义。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe产品类型枚举定义。
    /// </summary>
    public enum USeProductClass
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown =0,

        /// <summary>
		/// 期货
		/// </summary>
		Futures = 1,

		/// <summary>
		/// 期货期权
		/// </summary>
		Options = 2,

		/// <summary>
		/// 组合
		/// </summary>
		Combination = 3,

		/// <summary>
		/// 即期
		/// </summary>
		Spot = 4,

		/// <summary>
		/// 期转现
		/// </summary>
		EFP = 5,

		/// <summary>
		/// 现货期权
		/// </summary>
		SpotOption = 6,
    }
}
