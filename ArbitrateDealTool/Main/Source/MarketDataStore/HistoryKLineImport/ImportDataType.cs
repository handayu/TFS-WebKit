using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryKLineImport
{
    public enum ImportDataType
    {
        Unknown = 0,

        /// <summary>
        /// K线。
        /// </summary>
        DayKLine = 1,

        /// <summary>
        /// 1分钟K线。
        /// </summary>
        Min1KLine =2,

        /// <summary>
        /// ClosePrice2。
        /// </summary>
        ClosePrice2 = 3,

        /// <summary>
        /// 合约指数K线。
        /// </summary>
        ContractIndex = 4,

        /// <summary>
        /// 主力合约K线。
        /// </summary>
        MainContractKLine = 5,
    }
}
