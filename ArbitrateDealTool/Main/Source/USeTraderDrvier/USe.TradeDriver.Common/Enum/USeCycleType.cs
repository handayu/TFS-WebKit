using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 周期类型
    /// </summary>
    public enum USeCycleType
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Tick数据。
        /// </summary>
        Real = 0,

        /// <summary>
        /// 1分钟数据。
        /// </summary>
        Min1 = 1,

        /// <summary>
        /// 2分钟数据。
        /// </summary>
        Min2 = 2,
        /// <summary>
        /// 3分钟数据。
        /// </summary>
        Min3 = 3,
        /// <summary>
        /// 4分钟数据。
        /// </summary>
        Min4 = 4,
        /// <summary>
        /// 5分钟数据。
        /// </summary>
        Min5 = 5,
        /// <summary>
        /// 15分钟数据。
        /// </summary>
        Min15 = 15,
        /// <summary>
        /// 30分钟数据。
        /// </summary>
        Min30 = 30,
        /// <summary>
        /// 60分钟数据。
        /// </summary>
        Min60 = 60,
        /// <summary>
        /// 日线数据。
        /// </summary>
        Day = 1001,
        /// <summary>
        /// 周线数据。
        /// </summary>
        Week = 1007,

        /// <summary>
        /// 月线数据。
        /// </summary>
        Month = 1030,

        /// <summary>
        /// 年线数据。
        /// </summary>
        Year = 1365,
    }                                           
}
