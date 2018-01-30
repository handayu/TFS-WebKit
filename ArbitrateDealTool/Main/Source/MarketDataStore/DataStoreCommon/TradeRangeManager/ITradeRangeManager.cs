using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using USe.Common.TradingDay;

namespace DataStoreCommon
{
    /// <summary>
    /// 交易区间管理类。
    /// </summary>
    public interface ITradeRangeManager
    {
        /// <summary>
        /// 创建交易区间。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <returns></returns>
        DayTradeRange CreateTradeRange(USeInstrument instrument);

        /// <summary>
        /// 创建交易区间。
        /// </summary>
        /// <param name="varieties">品种。</param>
        /// <returns></returns>
        DayTradeRange CreateTradeRange(string varieties);
    }
}
