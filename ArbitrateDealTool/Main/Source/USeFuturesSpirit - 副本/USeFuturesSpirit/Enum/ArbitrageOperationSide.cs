using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 套利下单操作类型。
    /// </summary>
    public enum ArbitrageOperationSide
    {
        /// <summary>
        ///  未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
		/// 卖近买远
		/// </summary>
		SellNearBuyFar = 1,

        /// <summary>
        /// 买近卖远。
        /// </summary>
        BuyNearSellFar = 2,
    }

    public static class ArbitrageOperationSideExtend
    {
        public static string ToDescription(this ArbitrageOperationSide operationSide)
        {
            switch (operationSide)
            {
                case ArbitrageOperationSide.SellNearBuyFar: return "卖近买远";
                case ArbitrageOperationSide.BuyNearSellFar: return "买近卖远";
                default: return "未知";
            }
        }

        public static ArbitrageOperationSide GetOppositeSide(this ArbitrageOperationSide operationSide)
        {
            switch (operationSide)
            {
                case ArbitrageOperationSide.SellNearBuyFar: return ArbitrageOperationSide.BuyNearSellFar;
                case ArbitrageOperationSide.BuyNearSellFar: return ArbitrageOperationSide.SellNearBuyFar;
                default: return ArbitrageOperationSide.Unknown;
            }
        }
    }
}
