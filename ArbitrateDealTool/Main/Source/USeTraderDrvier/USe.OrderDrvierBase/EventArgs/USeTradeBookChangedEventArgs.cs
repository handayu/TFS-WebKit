#region Copyright & Version
//==============================================================================
// 文件名称: USeTradeBookChangedEventArgs.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 成交回报变更(新增)。
//==============================================================================
#endregion

using System;
using USe.TradeDriver.Common;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 成交回报变更(新增)。
    /// </summary>
    public class USeTradeBookChangedEventArgs : EventArgs
    {
        #region construction
        /// <summary>
        /// 构造USeTradeBookChangedEventArgs实例。
        /// </summary>
        /// <param name="tradeBook">成交回报。</param>
        public USeTradeBookChangedEventArgs(USeTradeBook tradeBook,bool isNew)
        {
            this.TradeBook = tradeBook;
            this.IsNew = isNew;
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 成交回报。
        /// </summary>
        public USeTradeBook TradeBook
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否为新成交回报。
        /// </summary>
        public bool IsNew
        {
            get;
            private set;
        }
        #endregion // property
    }
}
