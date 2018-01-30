using System;
using System.Text;

namespace USe.Common.AppLogger
{
	/// <summary>
	/// 应用程序日志接口。
	/// </summary>
	public interface IAppLogger : IDisposable
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
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
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
		/// <param name="lineFeed">回车换行标志。</param>
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
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteEvent(LogEventType eventType, byte[] bytes, int startIndex, int count, bool lineFeed);

		/// <summary>
		/// 应用程序日志写入一条Critical类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteCritical(string text);

		/// <summary>
		/// 应用程序日志写入一条Error类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteError(string text);

		/// <summary>
		/// 应用程序日志写入一条Warning类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteWarning(string text);

		/// <summary>
		/// 应用程序日志写入一条Information类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteInformation(string text);

		/// <summary>
		/// 应用程序日志写入一条Verbose类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteVerbose(string text);

		/// <summary>
		/// 应用程序日志写入一条Notice类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteNotice(string text);

		/// <summary>
		/// 应用程序日志写入一条Inbound类型事件信息。
		/// </summary>
		/// <param name="message">入站消息字符串。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteInbound(string message);

		/// <summary>
		/// 应用程序日志写入一条Inbound类型事件信息。
		/// </summary>
		/// <param name="bytes">待写入的日志信息字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteInbound(byte[] bytes, int startIndex, int count);

		/// <summary>
		/// 应用程序日志写入一条Outbound类型事件信息。
		/// </summary>
		/// <param name="message">出站消息字符串。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteOutbound(string message);

		/// <summary>
		/// 应用程序日志写入一条Outbound类型事件信息。
		/// </summary>
		/// <param name="bytes">待写入的日志信息字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteOutbound(byte[] bytes, int startIndex, int count);

		/// <summary>
		/// 应用程序日志写入一条Message类型事件信息。
		/// </summary>
		/// <param name="message">消息字符串。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteMessage(string message);

		/// <summary>
		/// 应用程序日志写入一条Message类型事件信息。
		/// </summary>
		/// <param name="bytes">待写入的日志信息字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteMessage(byte[] bytes, int startIndex, int count);

		/// <summary>
		/// 应用程序日志写入一条Audit类型事件信息。
		/// </summary>
		/// <param name="message">消息字符串。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteAudit(string message);

		/// <summary>
		/// 应用程序日志写入一条Audit类型事件信息。
		/// </summary>
		/// <param name="bytes">待写入的日志信息字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		bool WriteAudit(byte[] bytes, int startIndex, int count);
	}
}
