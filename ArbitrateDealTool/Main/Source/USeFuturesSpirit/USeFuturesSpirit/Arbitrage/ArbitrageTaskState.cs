using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 套利单任务执行状态。
    /// </summary>
    public enum ArbitrageTaskState
    {
        /// <summary>
        /// 任务未执行。
        /// </summary>
        None =0,

        /// <summary>
        /// 优先合约挂单中。
        /// </summary>
        FirstPlaceOrder = 1,

        /// <summary>
        /// 优先合约挂单完成。
        /// </summary>
        FirstPlaceOrderFinish = 2,

        /// <summary>
        /// 优先合约完全成交。
        /// </summary>
        FirstTradeFinish =3,

        /// <summary>
        /// 反向合约挂单中。
        /// </summary>
        SecondPalceOrder =4,

        /// <summary>
        /// 反向合约挂单完成。
        /// </summary>
        SecondPlaceOrderFinish = 5,

        /// <summary>
        /// 反向合约完全成交。
        /// </summary>
        SecondTradeFinish = 6,

        /// <summary>
        /// 强制完成。
        /// </summary>
        ForceFinish = 7,
    }
}
