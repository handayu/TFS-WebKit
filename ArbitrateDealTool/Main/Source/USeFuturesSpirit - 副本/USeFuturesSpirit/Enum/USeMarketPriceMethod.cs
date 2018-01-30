using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// USe市场价定义。
    /// </summary>
    public enum USeMarketPriceMethod
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// 对手价。
        /// </summary>
        OpponentPrice = 0,


        /// <summary>
        /// 一个最小变动单位。
        /// </summary>
        OnePriceTick =1,

        /// <summary>
        /// 两个最小变动单位。
        /// </summary>
        TwoPriceTick = 2,

        // <summary>
        /// 三个最小变动单位。
        /// </summary>
        ThreePriceTick = 3,
    }
}
