#region Copyright & Version
//==============================================================================
// 文件名称: USeErrorMessageArgs.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Justin Shen
// 创建日期: 2012/05/11
// 描    述: 发送错误信息事件参数类。
//==============================================================================
#endregion

using System;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 发送错误信息事件参数类
    /// </summary>
    public class USeErrorMessageArgs : EventArgs
    {
        #region construction
        /// <summary>
        /// 构造USeErrorMessageArgs实例。
        /// </summary>
        /// <param name="future">错误信息。</param>
        public USeErrorMessageArgs(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }
        #endregion
    }
}
