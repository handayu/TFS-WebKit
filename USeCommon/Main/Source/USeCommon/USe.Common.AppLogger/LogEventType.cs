using System;

namespace USe.Common.AppLogger
{
	/// <summary>
	/// 日志事件类型的枚举定义。
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
	[Flags]
	public enum LogEventType
	{
		/// <summary>
		/// 关键性错误或应用程序崩溃信息，ID = 'C'。
		/// </summary>
		Critical = 0X0001,

		/// <summary>
		/// 可恢复的错误信息，ID = 'E'。
		/// </summary>
		Error = 0X0002,

		/// <summary>
		/// 警告(非关键性问题)信息，ID = 'W'。
		/// </summary>
		Warning = 0X0004,

		/// <summary>
		/// 一般性信息，ID = 'I'。
		/// </summary>
		Information = 0X0008,

		/// <summary>
		/// 调试跟踪信息，ID = 'V'。
		/// </summary>
		Verbose = 0X0010,

		/// <summary>
		/// 通知性信息，ID = 'N'。
		/// </summary>
		Notice = 0X0100,

		/// <summary>
		/// 输入(入站)消息，ID = '&gt;'。
		/// </summary>
		Inbound = 0X0200,

		/// <summary>
		/// 输出(出站)消息，ID = '&lt;'。
		/// </summary>
		Outbound = 0X0400,

		/// <summary>
		/// 原始(Raw)消息，ID = 'M'。
		/// </summary>
		Message = 0X0800,

		/// <summary>
		/// 审计事件，ID = 'A'。
		/// </summary>
		Audit = 0X1000,
	}
}
