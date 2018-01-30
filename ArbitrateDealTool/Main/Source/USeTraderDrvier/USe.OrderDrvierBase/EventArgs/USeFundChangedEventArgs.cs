#region Copyright & Version
//==============================================================================
// 文件名称: USeTradingAccountChangedEventArgs.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 账户信息变更。
//==============================================================================
#endregion

using System;
using USe.TradeDriver.Common;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 账户信息变更。
    /// </summary>
    public class USeFundChangedEventArgs : EventArgs
    {
        #region construction
        /// <summary>
        /// 构造TradingAccountChangedEventArgs实例。
        /// </summary>
        /// <param name="fundInfo">账户信息。</param>
        public USeFundChangedEventArgs(USeFund fundInfo)
        {
            this.FundInfo = fundInfo;
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 账户资金信息。
        /// </summary>
        public USeFund FundInfo
        {
            get;
            private set;
        }
        #endregion // property
    }
}
