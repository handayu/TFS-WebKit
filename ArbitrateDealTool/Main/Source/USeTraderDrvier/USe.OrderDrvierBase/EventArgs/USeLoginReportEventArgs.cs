using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 登录状态报告。
    /// </summary>
    public class USeLoginReportEventArgs : EventArgs
    {
        #region construction
        /// <summary>
        /// 构造USeLoginReportEventArgs实例。
        /// </summary>
        /// <param name="percent">登录进度。</param>
        /// <param name="message">登录消息。</param>
        public USeLoginReportEventArgs(double percent,string message)
        {
            this.LoginPercent = percent;
            this.Message = message;
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 登录进度。
        /// </summary>
        public double LoginPercent
        {
            get;
            private set;
        }

        /// <summary>
        /// 登录消息。
        /// </summary>
        public string Message
        {
            get;
            private set;
        }
        #endregion // property
    }
}
