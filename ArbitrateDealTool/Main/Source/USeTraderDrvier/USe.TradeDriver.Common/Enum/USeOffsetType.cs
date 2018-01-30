#region Copyright & Version
//==============================================================================
// 文件名称: USeOffsetType.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 开平仓方向枚举定义。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 开平仓方向枚举定义。
    /// </summary>
    public enum USeOffsetType
    {
        /// <summary>
        /// 开仓。
        /// </summary>
        Open = 0,

        /// <summary>
        /// 平昨仓。
        /// </summary>
        CloseHistory = 1,

        /// <summary>
        /// 平今仓。
        /// </summary>
        CloseToday = 2,

        /// <summary>
        /// 平仓。
        /// </summary>
        Close = 3,
    }

    /// <summary>
    /// 开平仓方向枚举扩展类。
    /// </summary>
    public static class USeOffsetTypeExtend
    {
        /// <summary>
        /// 开平仓方向描述。
        /// </summary>
        /// <param name="offsetType">开平仓方向。</param>
        /// <returns>描述字符串。</returns>
        public static string ToDescription(this USeOffsetType offsetType)
        {
            switch (offsetType)
            {
                case USeOffsetType.Open: return "开仓";
                case USeOffsetType.CloseHistory: return "平昨仓";
                case USeOffsetType.CloseToday: return "平今仓";
                case USeOffsetType.Close: return "平仓";
                default: return "未知";
            }
        }
    }
}
