#region Copyright & Version
//==============================================================================
// 文件名称: USePositionType.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/05/07
// 描    述: 持仓类型枚举定义。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 持仓类型枚举定义。
    /// </summary>
    public enum USePositionType
    {
        /// <summary>
        /// 今仓。
        /// </summary>
        Today,

        /// <summary>
        /// 昨仓。
        /// </summary>
        Yestorday,
    }

    /// <summary>
    /// 持仓类型枚举扩展类。
    /// </summary>
    public static class USePositionTypeExtend
    {
        /// <summary>
        /// 今昨仓描述。
        /// </summary>
        /// <param name="USePositionType">今昨。</param>
        /// <returns>描述字符串。</returns>
        public static string ToDescription(this USePositionType usePositionType)
        {
            switch (usePositionType)
            {
                case USePositionType.Today: return "今仓";
                case USePositionType.Yestorday: return "昨仓";
                default: return "未知";
            }
        }
    }

}
