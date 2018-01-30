using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 任务下单原因。
    /// </summary>
    public enum TaskOrderReason
    {
        /// <summary>
        /// 不下单
        /// </summary>
        None =0,

        /// <summary>
        /// 开仓。
        /// </summary>
        Open = 1,

        /// <summary>
        /// 盈利平仓。
        /// </summary>
        Close = 2,

        /// <summary>
        /// 止损。
        /// </summary>
        StopLoss = 3,
    }
}
