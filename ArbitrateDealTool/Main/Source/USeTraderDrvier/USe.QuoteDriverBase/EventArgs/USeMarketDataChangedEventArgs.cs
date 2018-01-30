#region Copyright & Version
//==============================================================================
// 文件名称: USeQuoteChangedEventArgs.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Justin Shen
// 创建日期: 2012/05/10
// 描    述: 行情变更事件参数类。
//==============================================================================
#endregion

using System;


namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 行情变更事件参数类。
    /// </summary>
    public class USeMarketDataChangedEventArgs : EventArgs
    {
        #region construction
        /// <summary>
        /// 构造USeMarketDataChangedEventArgs实例。
        /// </summary>
        /// <param name="future">期货行情信息。</param>
        public USeMarketDataChangedEventArgs(USeMarketData future)
        {
            this.MarketData = future;
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 行情信息。
        /// </summary>
        public USeMarketData MarketData
        {
            get;
            private set;
        }
        #endregion // property
    }
}
