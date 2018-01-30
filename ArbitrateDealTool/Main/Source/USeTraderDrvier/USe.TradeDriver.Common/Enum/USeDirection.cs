#region Copyright & Version
//==============================================================================
// 文件名称: USeDirection.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: USe多空方向枚举定义。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 多空方向枚举定义。
    /// </summary>
    public enum USeDirection
    {
        /// <summary>
        /// 做多。
        /// </summary>
        Long = 0,

        /// <summary>
        /// 做空。
        /// </summary>
        Short = 1,
    }

    /// <summary>
    /// 多空方向枚举扩展类。
    /// </summary>
    public static class USeDirectionExtend
    {
        /// <summary>
        /// 多空方向描述。
        /// </summary>
        /// <param name="direction">多空方向。</param>
        /// <returns>描述字符串。</returns>
        public static string ToDescription(this USeDirection direction)
        {
            switch (direction)
            {
                case USeDirection.Long: return "买";
                case USeDirection.Short: return "卖";
                default: return "未知";
            }
        }
    }
}
