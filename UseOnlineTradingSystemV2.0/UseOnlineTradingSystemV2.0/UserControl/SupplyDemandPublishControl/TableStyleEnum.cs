using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseOnlineTradingSystem
{
    public enum TableStyleEnum
    {
        /// <summary>
        /// 挂牌表
        /// </summary>
        PutBrandTable = 0,

        /// <summary>
        /// 摘牌表
        /// </summary>
        DelistTable,

        /// <summary>
        /// 交易成交表
        /// </summary>
        TradedTable,

        /// <summary>
        /// 未知
        /// </summary>
        Unknow
    }
}
