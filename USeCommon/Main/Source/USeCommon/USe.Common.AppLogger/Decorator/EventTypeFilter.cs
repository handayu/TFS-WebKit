using System;
using System.Text;
using System.Diagnostics;

namespace USe.Common.AppLogger.Decorator
{
	/// <summary>
	/// 日志信息过滤装饰类，按日志事件级别(类型)过滤。
	/// </summary>
	public class EventTypeFilter : AbstractDecorator
	{
		private LogLevels m_logLevel;

        /// <summary>
		/// 初始化EventTypeFilter类的新实例。
		/// </summary>
		/// <param name="logLevel">过滤日志信息的级别。</param>
		/// <param name="innerImpl">内部实现类(IAppLoggerImpl)对象。</param>
		/// <exception cref="System.ArgumentNullException">innerImpl参数为null时。</exception>
		public EventTypeFilter(LogLevels logLevel, IAppLoggerImpl innerImpl)
            : base(innerImpl)
        {
			m_logLevel = logLevel;
        }


		/// <summary>
		/// EventTypeFilter类对象写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="eventText">日志事件记录文本。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public override bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
		{
			if (((int)eventType & (int)m_logLevel) == 0)
			{
				return true;
			}

			return base.WriteEvent(eventType, eventText, lineFeed);
		}

		/// <summary>
		/// EventTypeFilter类对象写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="bytes">日志事件记录的字节序列缓冲区。</param>
		/// <param name="startIndex">日志事件记录在字节序列缓冲区里的起始索引。</param>
		/// <param name="count">日志事件记录的字节数量。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public override bool WriteEvent(LogEventType eventType, byte[] bytes, int startIndex, int count, bool lineFeed)
		{
			if (((int)eventType & (int)m_logLevel) == 0)
			{
				return true;
			}

			return base.WriteEvent(eventType, bytes, startIndex, count, lineFeed);
		}
	}
}
