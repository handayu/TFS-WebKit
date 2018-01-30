using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common;
using USe.TradeDriver.Common;

namespace MarketDataStore
{
    /// <summary>
    /// K线数据监听接口。
    /// </summary>
    public interface IKLineDataListener:IUSeNotifier
    {
        /// <summary>
        /// 接收K线数据。
        /// </summary>
        /// <param name="kLine"></param>
        void ReceiveKLineData(USeKLine kLine);
    }
}
