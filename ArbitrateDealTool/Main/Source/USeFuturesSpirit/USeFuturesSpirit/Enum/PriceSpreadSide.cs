using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 价差监控方向。
    /// </summary>
    public enum PriceSpreadSide
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown =0,

        /// <summary>
        /// 大于等于。
        /// </summary>
        GreaterOrEqual = 1,

        /// <summary>
        /// 小于等于。
        /// </summary>
        LessOrEqual = 2,
    }
}
