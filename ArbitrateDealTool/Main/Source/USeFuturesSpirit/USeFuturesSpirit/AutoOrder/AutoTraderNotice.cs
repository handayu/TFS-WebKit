using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 自动下单机通知。
    /// </summary>
    public class AutoTraderNotice
    {
        #region construction
        public AutoTraderNotice(Guid tradeIdentity, string alias, AutoTraderNoticeType noticeType, string message)
            : this(DateTime.Now, tradeIdentity, alias, noticeType, message)
        {
        }

        public AutoTraderNotice(DateTime noticeTime, Guid tradeIdentity, string alias, AutoTraderNoticeType noticeType, string message)
        {
            this.NoticeTime = noticeTime;
            this.TradeIdentity = tradeIdentity;
            this.Alias = alias;
            this.NoticeType = noticeType;
            this.Message = message;
        }
        #endregion

        #region property
        /// <summary>
        /// 下单机标识。
        /// </summary>
        public Guid TradeIdentity { get; set; }

        /// <summary>
        /// 套利单别名。
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 通知时间。
        /// </summary>
        public DateTime NoticeTime { get; set; }

        /// <summary>
        /// 通知类型。
        /// </summary>
        public AutoTraderNoticeType NoticeType { get; set; }

        /// <summary>
        /// 通知级别。
        /// </summary>
        public AutoTraderNoticeLevel Level { get; set; }
        /// <summary>
        /// 消息。
        /// </summary>
        public string Message { get; set; }
        #endregion
    }
}
