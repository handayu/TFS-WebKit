#region Copyright & Version
//==============================================================================
// 文件名称: USeOrderStatus.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 委托单状态枚举定义。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 委托单状态枚举定义。
    /// </summary>
    public enum USeOrderStatus
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 未成交。
        /// </summary>
        NoTraded = 1,

        /// <summary>
        /// 部分撤单。
        /// </summary>
        PartCanceled = 2,

        /// <summary>
        /// 全部已撤。
        /// </summary>
        AllCanceled = 3,

        /// <summary>
        /// 部分成交。
        /// </summary>
        PartTraded = 4,

        /// <summary>
        /// 全部已成交。
        /// </summary>
        AllTraded = 5,

        /// <summary>
        /// 废单。
        /// </summary>
        BlankOrder = 6,
    }

    /// <summary>
    /// 委托单状态枚举扩展类。
    /// </summary>
    public static class USeOrderStatusExtend
    {
        /// <summary>
        /// 委托单状态描述。
        /// </summary>
        /// <param name="orderStatus">委托状态。</param>
        /// <returns>描述字符串。</returns>
        public static string ToDescription(this USeOrderStatus orderStatus)
        {
            switch (orderStatus)
            {
                case USeOrderStatus.NoTraded: return "未成交";
                case USeOrderStatus.PartCanceled: return "部撤";
                case USeOrderStatus.AllCanceled: return "已撤";
                case USeOrderStatus.PartTraded: return "部成";
                case USeOrderStatus.AllTraded: return "已成";
                case USeOrderStatus.BlankOrder: return "废单";
                default: return "未知";
            }
        }
    }
}
