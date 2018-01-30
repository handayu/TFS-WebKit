using System;

namespace USe.Common.AppLogger
{
	/// <summary>
	/// 日志事件级别的枚举定义。
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
	[Flags]
	public enum LogLevels
	{
		/// <summary>
		/// 完全关闭。
		/// </summary>
		Off = 0,

		/// <summary>
		/// 关键性错误级别。
		/// </summary>
		Critical = LogEventType.Critical,

		/// <summary>
		/// 错误及以上级别。
		/// </summary>
		Error = LogEventType.Critical | LogEventType.Error,

		/// <summary>
		/// 警告及以上级别。
		/// </summary>
		Warning = LogEventType.Critical | LogEventType.Error | LogEventType.Warning,

		/// <summary>
		/// 一般信息及以上级别。
		/// </summary>
		Information = LogEventType.Critical | LogEventType.Error | LogEventType.Warning | LogEventType.Information,

		/// <summary>
		/// 调试信息及以上级别。
		/// </summary>
		Verbose = LogEventType.Critical | LogEventType.Error | LogEventType.Warning | LogEventType.Information | LogEventType.Verbose,

		/// <summary>
		/// 全部打开。
		/// </summary>
		All = 0XFFFF,
	}
}
