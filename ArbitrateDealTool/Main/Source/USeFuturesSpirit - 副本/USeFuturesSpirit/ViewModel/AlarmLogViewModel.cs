using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common;

namespace USeFuturesSpirit
{
    class AlarmLogViewModel : USeBaseViewModel
    {
        #region member
        private DateTime m_alarmTime = DateTime.MinValue;
        private AlarmType m_alarmType = AlarmType.Unknown;
        private string m_message = string.Empty;
        #endregion

        #region property
        /// <summary>
        /// 预警时间。
        /// </summary>
        public DateTime AlarmTime
        {
            get { return m_alarmTime; }
            set
            {
                if(value != m_alarmTime)
                {
                    m_alarmTime = value;
                    SetProperty(() => this.AlarmTime);
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
        /// 预警类型。
        /// </summary>
        public AlarmType AlarmType
        {
            get { return m_alarmType; }
            set
            {
                if (value != m_alarmType)
                {
                    m_alarmType = value;
                    SetProperty(() => this.AlarmType);
                }
            }
        }
        #endregion
    }
}
