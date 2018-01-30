using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit.Arbitrage
{
    /// <summary>
    /// 止损参数设定。
    /// </summary>
    public class ArbitrageStopLossArgument
    {
        #region proeperty
        /// <summary>
        /// 止损条件。
        /// </summary>
        public PriceSpreadCondition StopLossCondition { get; set; }
        #endregion

        /// <summary>
        /// 克隆方法。
        /// </summary>
        /// <returns></returns>
        public ArbitrageStopLossArgument Clone()
        {
            ArbitrageStopLossArgument arg = new ArbitrageStopLossArgument();
            arg.StopLossCondition = this.StopLossCondition;

            return arg;
        }
    }
}
