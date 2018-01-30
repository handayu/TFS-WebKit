using System;
using System.Text;

namespace USe.Common.AppLogger
{
	/// <summary>
	/// 应用程序日志内部实现类的接口。
	/// </summary>
	public interface IAppLoggerImpl : IDisposable
	{
		/// <summary>
		/// 获取应用程序日志的对象名称。
		/// </summary>
		string Name { get; }

		/// <summary>
		/// 获取/设置应用程序日志的编码格式。
		/// </summary>
		Encoding Encoding { get; set; }

		/// <summary>
		/// 获取应用程序日志的线程安全标志。
		/// </summary>
		bool IsThreadSafe { get; }

		/// <summary>
		/// 刷新应用程序日志的输出流，缓冲数据写入日志存储。
		/// </summary>
		/// <returns>刷新成功与否标志。</returns>
		bool Flush();

		/// <summary>
		/// 应用程序日志写入回车换行。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool LineFeed();

		/// <summary>
		/// 应用程序日志写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="eventText">日志事件记录文本。</param>
		/// <param name="lineFeed">换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed);

		/// <summary>
		/// 应用程序日志写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="bytes">日志事件记录的字节序列缓冲区。</param>
		/// <param name="startIndex">日志事件记录在字节序列缓冲区里的起始索引。</param>
		/// <param name="count">日志事件记录的字节数量。</param>
		/// <param name="lineFeed">换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteEvent(LogEventType eventType, byte[] bytes, int startIndex, int count, bool lineFeed);
	}
}
