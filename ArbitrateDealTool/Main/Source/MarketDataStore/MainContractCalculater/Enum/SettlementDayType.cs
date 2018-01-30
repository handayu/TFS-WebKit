using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainContractCalculater
{
    /// <summary>
    /// 结算日期类型枚举定义。
    /// </summary>
    public enum SettlementDayType
    {
        /// <summary>
        /// 当日。
        /// </summary>
        Today = 0,

        /// <summary>
        /// 指定日。
        /// </summary>
        SpecialDay = 1
    }
}
