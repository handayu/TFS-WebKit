using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit.Arbitrage
{
    public class ArbitrageAlarmArgument
    {
        #region property
        /// <summary>
        /// 监控类型。
        /// </summary>
        public ArbitragePriceSpreadAlarmType MonitorType { get; set; }

        /// <summary>
        /// 价差监控方向。
        /// </summary>
        public PriceSpreadSide PriceSpreadSide { get; set; }


        /// <summary>
        /// 价差阀值。
        /// </summary>
        public decimal PriceSpreadThreshold { get; set; }
        #endregion

        public ArbitrageAlarmArgument Clone()
        {
            ArbitrageAlarmArgument arg = new ArbitrageAlarmArgument();
            arg.MonitorType = this.MonitorType;
            arg.PriceSpreadSide = this.PriceSpreadSide;
            arg.PriceSpreadThreshold = this.PriceSpreadThreshold;

            return arg;
        }
    }
}
