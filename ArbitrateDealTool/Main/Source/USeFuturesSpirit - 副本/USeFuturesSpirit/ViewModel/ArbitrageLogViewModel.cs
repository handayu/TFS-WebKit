using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common;

namespace USeFuturesSpirit
{
    class ArbitrageLogViewModel:USeBaseViewModel
    {
        #region member
        private Guid m_traderIdentify = Guid.Empty;
        private string m_alias = string.Empty;
        private DateTime m_logTime = DateTime.MinValue;
        private AutoTraderNoticeType m_noticeType = AutoTraderNoticeType.Unknown;
        private string m_message = string.Empty;
        #endregion

        #region property
        /// <summary>
        /// 下单机标识。
        /// </summary>
        public Guid TraderIdentify
        {
            get { return m_traderIdentify; }
            set
            {
                if (value != m_traderIdentify)
                {
                    m_traderIdentify = value;
                    SetProperty(() => this.TraderIdentify);
                }
            }
        }

        /// <summary>
        /// 套利单别名。
        /// </summary>
        public string Alias
        {
            get { return m_alias; }
            set
            {
                if (value != m_alias)
                {
                    m_alias = value;
                    SetProperty(() => this.Alias);
                }
            }
        }

        /// <summary>
        /// 日志时间。
        /// </summary>
        public DateTime LogTime
        {
            get { return m_logTime; }
            set
            {
                if(value != m_logTime)
                {
                    m_logTime = value;
                    SetProperty(() => this.LogTime);
                }
            }
        }

        /// <summary>
        /// 消息。
        /// </summary>
        public string Message
        {
            get { return m_message; }
            set
            {
                if (value != m_message)
                {
                    m_message = value;
                    SetProperty(() => this.Message);
                }
            }
        }

        /// <summary>
        /// 通知类型。
        /// </summary>
        public AutoTraderNoticeType NoticeType
        {
            get { return m_noticeType; }
            set
            {
                if (value != m_noticeType)
                {
                    m_noticeType = value;
                    SetProperty(() => this.NoticeType);
                }
            }
        }
        #endregion
    }
}
