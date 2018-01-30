#region Copyright & Version
//==============================================================================
// 文件名称: USeOrderDriverStateChangedEventArgs.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 交易驱动类状态变更。
//==============================================================================
#endregion

using System;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 交易驱动类状态变更。
    /// </summary>
    public class USeOrderDriverStateChangedEventArgs : EventArgs
    {
        #region construciton
        /// <summary>
        /// 构造USeOrderDriverStateChangedEventArgs实例。
        /// </summary>
        /// <param name="account">交易账户。</param>
        /// <param name="oldState">前一状态。</param>
        /// <param name="newState">当前状态。</param>
        /// <param name="reason">变更原因。</param>
        public USeOrderDriverStateChangedEventArgs(string account, USeOrderDriverState oldState, USeOrderDriverState newState, string reason)
        {
            this.Account = account;
            this.OldState = oldState;
            this.NewState = newState;
            this.Reason = reason;
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 交易帐号。
        /// </summary>
        public string Account
        {
            get;
            private set;
        }

        /// <summary>
        /// 前一状态。
        /// </summary>
        public USeOrderDriverState OldState
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前状态。
        /// </summary>
        public USeOrderDriverState NewState
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
