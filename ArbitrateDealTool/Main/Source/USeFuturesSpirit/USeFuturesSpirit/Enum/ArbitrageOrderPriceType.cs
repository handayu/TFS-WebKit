using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 套利下单价格类型。
    /// </summary>
    public enum ArbitrageOrderPriceType
    {
        /// <summary>
        ///  未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
		/// 最新价
		/// </summary>
		LastPrice = 1,

        /// <summary>
        /// 对手价。
        /// </summary>
        OpponentPrice = 2,

        /// <summary>
        /// 排队价。
        /// </summary>
        QueuePrice = 3,
    }

    public static class s
    {
        public static string ToDescription(this ArbitrageOrderPriceType orderPriceType)
        {
            switch(orderPriceType)
            {
                case ArbitrageOrderPriceType.LastPrice:return "最新价";
                case ArbitrageOrderPriceType.OpponentPrice:return "对手价";
                case ArbitrageOrderPriceType.QueuePrice:return "排队价";
                default:return "未知";
            }
        }
    }
}
