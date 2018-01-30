using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 预警通知。
    /// </summary>
    public class AlarmNotice
    {
        #region construction
        public AlarmNotice(AlarmType alarmType,string message)
            :this(DateTime.Now,alarmType,message)
        {
        }

        public AlarmNotice(DateTime alarmTime, AlarmType alarmType, string message)
        {
            this.AlarmTime = alarmTime;
            this.AlarmType = alarmType;
            this.Message = message;
        }
        #endregion

        #region property
        /// <summary>
        /// 预警时间。
        /// </summary>
        public DateTime AlarmTime { get; set; }

        /// <summary>
        /// 预警类型。
        /// </summary>
        public AlarmType AlarmType { get; set; }

        /// <summary>
        /// 消息。
        /// </summary>
        public string Message { get; set; }
        #endregion
    }
}
