using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;

namespace MarketDataStore
{
    /// <summary>
    /// K线行情发布接口。
    /// </summary>
    public interface IKLinePublisher
    {
        /// <summary>
        /// 发布K线行情数据。
        /// </summary>
        /// <param name="kLine">K线。</param>
        void PublishKLine(USeKLine kLine);
    }
}
