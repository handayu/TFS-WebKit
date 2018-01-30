#region Copyright & Version
//==============================================================================
// 文件名称: USeQuoteDriverStateChangedEventArgs.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Justin Shen
// 创建日期: 2012/05/10
// 描    述: 状态变更事件参数类。
//==============================================================================
#endregion

using System;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 状态变更事件参数类
    /// </summary>
    public class USeQuoteDriverStateChangedEventArgs : EventArgs
    {
        #region construction
        /// <summary>
        /// 构造USeQuoteDriverStateChangedEventArgs实例。
        /// </summary>
        /// <param name="account">帐号。</param>
        /// <param name="oldState">上一状态。</param>
        /// <param name="newState">当前状态。</param>
        /// <param name="reason">状态变更原因。</param>
        public USeQuoteDriverStateChangedEventArgs(string account, USeQuoteDriverState oldState, USeQuoteDriverState newState, string reason)
        {
            this.Account = account;
            this.NewState = newState;
            this.OldState = oldState;
            this.Reason = reason;
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 帐号。
        /// </summary>
        public string Account
        {
            get;
            private set;
        }

        /// <summary>
        /// 上一状态。
        /// </summary>
        public USeQuoteDriverState OldState
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前状态。
        /// </summary>
        public USeQuoteDriverState NewState
        {
            get;
            private set;
        }

        /// <summary>
        /// 状态变更原因。
        /// </summary>
        public string Reason
        {
            get;
            private set;
        }
        #endregion // property
    }
}
