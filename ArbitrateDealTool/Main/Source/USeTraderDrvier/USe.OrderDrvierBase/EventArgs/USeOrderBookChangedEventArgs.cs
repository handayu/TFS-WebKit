#region Copyright & Version
//==============================================================================
// 文件名称: USeOrderBookChangedEventArgs.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 委托回报变更事件。
//==============================================================================
#endregion

using System;
using USe.TradeDriver.Common;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 委托回报变更事件。
    /// </summary>
    public class USeOrderBookChangedEventArgs : EventArgs
    {
        #region construction
        /// <summary>
        /// 构造OrderBookEventArgs实例。
        /// </summary>
        /// <param name="orderBook">委托回报。</param>
        public USeOrderBookChangedEventArgs(USeOrderBook orderBook)
        {
            this.OrderBook = orderBook;
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 委托回报数据。
        /// </summary>
        public USeOrderBook OrderBook
        {
            get;
            private set;
        }
        #endregion // property
    }
}
