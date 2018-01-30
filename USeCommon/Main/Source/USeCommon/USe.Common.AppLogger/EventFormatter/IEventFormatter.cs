using System;

namespace USe.Common.AppLogger.EventFormatter
{
	/// <summary>
	/// 日志事件消息格式化接口。
	/// </summary>
	public interface IEventFormatter
	{
		/// <summary>
		/// 使用指定的日志事件类型格式化产生事件消息记录。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="eventText">日志事件记录文本。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 格式化产生的事件消息记录字符串。
		/// </returns>
		string Format(LogEventType eventType, string eventText, bool lineFeed);
	}
}
