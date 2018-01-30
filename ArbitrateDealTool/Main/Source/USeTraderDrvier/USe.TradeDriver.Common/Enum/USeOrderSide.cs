#region Copyright & Version
//==============================================================================
// 文件名称: USeOrderSide.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 买卖方向枚举定义。
//==============================================================================
#endregion

using System.Diagnostics;
using System;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 买卖方向枚举定义。
    /// </summary>
    public enum USeOrderSide
    {
        /// <summary>
        /// 买入。
        /// </summary>
        Buy = 0,

        /// <summary>
        /// 卖出。
        /// </summary>
        Sell = 1,
    }

    /// <summary>
    /// 买卖方向枚举扩展类。
    /// </summary>
    public static class USeOrderSideExtend
    {
        /// <summary>
        /// 买卖方向描述。
        /// </summary>
        /// <param name="orderSide">买卖方向。</param>
        /// <returns>描述字符串。</returns>
        public static string ToDescription(this USeOrderSide orderSide)
        {
            switch (orderSide)
            {
                case USeOrderSide.Buy: return "买入";
                case USeOrderSide.Sell: return "卖出";
                default: return "未知";
            }
        }

        /// <summary>
        /// 买卖方向反向方向。
        /// </summary>
        /// <param name="orderSide">买卖方向。</param>
        /// <returns>反向买卖方向。</returns>
        public static USeOrderSide GetOppositeOrderSide(this USeOrderSide orderSide)
        {
            if (orderSide == USeOrderSide.Buy)
            {
                return USeOrderSide.Sell;
            }
            else if (orderSide == USeOrderSide.Sell)
            {
                return USeOrderSide.Buy;
            }
            else
            {
                Debug.Assert(false);
                throw new Exception("Unknown orderSide " + orderSide.ToString());
            }
        }
    }
}
