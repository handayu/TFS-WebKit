using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    public partial class ArbitrageOrderControl
    {
        /// <summary>
        /// 损益计算结果。
        /// </summary>
        private class ProfitResult
        {
            /// <summary>
            /// 买入合约损益。
            /// </summary>
            public decimal BuyProfit { get; set; }

            /// <summary>
            /// 卖出合约损益。
            /// </summary>
            public decimal SellProfit { get; set; }

            /// <summary>
            /// 总损益。
            /// </summary>
            public decimal TotalProfit
            {
                get { return (this.BuyProfit + this.SellProfit); }
            }

            /// <summary>
            /// 当前价差。
            /// </summary>
            public decimal? CurrentPriceSpread { get; set; }
        }
    }
}
