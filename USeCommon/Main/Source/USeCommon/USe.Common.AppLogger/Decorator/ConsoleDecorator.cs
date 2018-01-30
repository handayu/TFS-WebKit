using System;
using System.Text;
using System.Diagnostics;

using USe.Common.AppLogger.EventFormatter;
using USe.Common.AppLogger.Implementation;

namespace USe.Common.AppLogger.Decorator
{
	/// <summary>
	/// 日志对象的控制台装饰类。
	/// </summary>
	public class ConsoleDecorator : AbstractDecorator
	{
		private ConsoleLogger m_consoleLogger;

		/// <summary>
		/// 初始化ConsoleDecorator类的新实例。
		/// </summary>
		/// <param name="innerImpl">内部实现类(IAppLoggerImpl)对象。</param>
		/// <exception cref="System.ArgumentNullException">innerImpl参数为null时。</exception>
		public ConsoleDecorator(IAppLoggerImpl innerImpl)
			: base(innerImpl)
		{
			m_consoleLogger = new ConsoleLogger("ConsoleDecorator", innerImpl.Encoding, new FriendlyEventStringFormatter("==>", "[HH:mm:ss.fff] "));
		}

		/// <summary>
		/// 清空AbstractDecorator类对象的日志输出缓冲区。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public override bool Flush()
		{
			m_consoleLogger.Flush();
			return base.Flush();
		}

		/// <summary>
		/// 写入回车换行。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public override bool LineFeed()
		{
			m_consoleLogger.LineFeed();
			return base.LineFeed();
		}

		/// <summary>
		/// 写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="eventText">日志事件记录文本。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public override bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
		{
			m_consoleLogger.WriteEvent(eventType, eventText, lineFeed);
			return base.WriteEvent(eventType, eventText, lineFeed);
		}

		/// <summary>
		/// 写入一条事件信息。
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
			m_consoleLogger.WriteEvent(eventType, bytes, startIndex, count, lineFeed);
			return base.WriteEvent(eventType, bytes, startIndex, count, lineFeed);
		}
	}
}
