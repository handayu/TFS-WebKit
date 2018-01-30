using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 异常委托单。
    /// </summary>
    public class ErrorUSeOrderBook
    {
        /// <summary>
        /// 下单机标识。
        /// </summary>
        public Guid TradeIdentify { get; set; }

        /// <summary>
        /// 别名。
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 异常委托回报。
        /// </summary>
        public USeOrderBook OrderBook { get; set; }
    }
}
