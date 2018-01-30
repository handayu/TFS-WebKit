using System;

namespace USe.Common
{
    /// <summary>
	/// USe通知事件数据类。
	/// </summary>
	public class USeNotifyEventArgs : EventArgs
    {
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化USeNotifyEventArgs类的新实例。
        /// </summary>
        /// <param name="level">通知事件的级别。</param>
        /// <param name="message">通知事件的描述信息。</param>
        public USeNotifyEventArgs(USeNotifyLevel level, string message)
            : this(level, message, null, 0, DateTime.Now)
        {
        }

        /// <summary>
        /// 使用指定的参数初始化USeNotifyEventArgs类的新实例。
        /// </summary>
        /// <param name="level">通知事件的级别。</param>
        /// <param name="message">通知事件的描述信息。</param>
        /// <param name="source">通知事件的来源。</param>
        /// <param name="eventId">通知事件的ID(编号)。</param>
        public USeNotifyEventArgs(USeNotifyLevel level, string message, object source, int eventId)
            : this(level, message, source, eventId, DateTime.Now)
        {
        }

        /// <summary>
        /// 使用指定的参数初始化USeNotifyEventArgs类的新实例。
        /// </summary>
        /// <param name="level">通知事件的级别。</param>
        /// <param name="message">通知事件的描述信息。</param>
        /// <param name="source">通知事件的来源。</param>
        /// <param name="eventId">通知事件的ID(编号)。</param>
        /// <param name="time">通知事件的时间。</param>
        public USeNotifyEventArgs(USeNotifyLevel level, string message, object source, int eventId, DateTime time)
        {
            this.Level = level;
            this.Message = message;
            this.Source = source;
            this.EventId = eventId;
            this.Time = time;
        }
        #endregion

        #region 属性(Property)
        /// <summary>
        /// 获取/设置通知事件的级别。
        /// </summary>
        public USeNotifyLevel Level
        {
            get;
            set;
        }

        /// <summary>
        /// 获取/设置通知事件的描述信息。
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 获取/设置通知事件的时间。
        /// </summary>
        public DateTime Time
        {
            get;
            set;
        }

        /// <summary>
        /// 获取/设置通知事件的来源。
        /// </summary>
        public object Source
        {
            get;
            set;
        }

        /// <summary>
        /// 获取通知事件来源者的名称字符串。
        /// </summary>
        public string SourceName
        {
            get
            {
                object source = this.Source;
                return (source != null ? (source is IUSeSourceNameProvider ? ((IUSeSourceNameProvider)source).SourceName : source.ToString()) : "<null>");
            }
        }

        /// <summary>
        /// 获取/设置通知事件的ID(编号)。
        /// </summary>
        public int EventId
        {
            get;
            set;
        }
        #endregion
    }
}
