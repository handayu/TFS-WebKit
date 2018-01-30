using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    public enum AlarmType
    {
        Unknown =0,

        /// <summary>
        /// 错误。
        /// </summary>
        SystemError = 1,

        /// <summary>
        /// 下单机错误。
        /// </summary>
        AutoTraderError =2,

        /// <summary>
        /// 下单机预警。
        /// </summary>
        AutoTraderWarning = 3,

        /// <summary>
        /// 交易驱动断线。
        /// </summary>
        TradeDriverDisconect = 4,
    }
}
