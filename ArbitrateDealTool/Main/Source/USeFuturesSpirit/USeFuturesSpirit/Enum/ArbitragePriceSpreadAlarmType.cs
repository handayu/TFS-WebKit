using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 套利单价差预警类型。
    /// </summary>
    public enum ArbitragePriceSpreadAlarmType
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown =0,

        /// <summary>
        /// 开仓。
        /// </summary>
        Open = 1,

        /// <summary>
        /// 平仓。
        /// </summary>
        Close = 2,
    }

    public static class ArbitragePriceSpreadAlarmTypeExtend
    {
        public static string ToDescription(this ArbitragePriceSpreadAlarmType alarmType)
        {
            switch (alarmType)
            {
                case ArbitragePriceSpreadAlarmType.Open:return "开仓";
                case ArbitragePriceSpreadAlarmType.Close:return "平仓";
                default:return "未知";
            }
        }
    }
}
