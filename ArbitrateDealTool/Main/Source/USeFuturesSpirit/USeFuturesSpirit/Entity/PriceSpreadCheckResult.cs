using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    class PriceSpreadCheckResult
    {
        /// <summary>
        /// 下单原因。
        /// </summary>
        public TaskOrderReason OrderReason { get; set; }

        /// <summary>
        /// 价差阀值。
        /// </summary>
        public decimal PriceSpreadThreshold { get; set; }

        public static PriceSpreadCheckResult CreateNoOrderResult()
        {
            PriceSpreadCheckResult result = new PriceSpreadCheckResult();
            result.OrderReason = TaskOrderReason.None;
            result.PriceSpreadThreshold = 0;
            return result;
        }
    }
}
