using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using USe.Common;
namespace MarketDataStore
{
    class ErrorNoticeViewModel : USeBaseViewModel
    {
        #region member
        private USeNotifyLevel m_level = USeNotifyLevel.Information;
        private Image m_levelIcon = null;
        private DateTime m_logTime = DateTime.MinValue;
        private string m_message = string.Empty;
        #endregion

        #region property
        public USeNotifyLevel Level
        {
            get { return m_level; }
            set
            {
                if (value != m_level)
                {
                    m_level = value;
                    SetProperty(() => this.Level);
                }
            }
        }

        public Image LevelIcon
        {
            get { return m_levelIcon; }
            set
            {
                if (value != m_levelIcon)
                {
                    m_levelIcon = value;
                    SetProperty(() => this.LevelIcon);
                }
            }
        }
        /// <summary>
        /// Log当前时间。
        /// </summary>
        public DateTime LogTime
        {
            get { return m_logTime; }
            set
            {
                m_logTime = value;
                SetProperty(() => this.LogTime);
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

        #endregion
    }

}

