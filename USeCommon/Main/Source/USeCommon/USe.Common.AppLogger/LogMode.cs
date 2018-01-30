using System;

namespace USe.Common.AppLogger
{
	/// <summary>
	/// 日志模式的枚举定义。
	/// </summary>
	/// <remarks>
	/// 实现类别的日志与装饰类别的日志模式间可按位(bit)进行组合。
	/// </remarks>
	[Obsolete("This enum is no longer used.")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
	[Flags]
	public enum LogMode
	{
		/// <summary>
		/// 空日志。
		/// </summary>
		Null = 0X0001,

		/// <summary>
		/// 实现类别的文件日志。
		/// </summary>
		/// <remarks>
		/// 每次写入信息时打开/关闭文件。
		/// </remarks>
		File = 0X0002,

		/// <summary>
		/// 实现类别的文件日志。
		/// </summary>
		/// <remarks>
		/// 日志对象创建时打开文件，直到对象关闭时才关闭文件。
		/// </remarks>
		File2 = 0X0003,

		/// <summary>
		/// 实现类别的每天自动更换文件名称的文件日志。
		/// </summary>
		/// <remarks>
		/// 每次写入信息时打开/关闭文件。
		/// </remarks>
		DailyFile = 0X0004,

		/// <summary>
		/// 实现类别的每天自动更换文件名称的文件日志。
		/// </summary>
		/// <remarks>
		/// 日志对象创建时打开文件，直到对象关闭时才关闭文件。
		/// </remarks>
		DailyFile2 = 0X0005,

		/// <summary>
		/// 实现类别的控制台输出的日志。
		/// </summary>
		/// <remarks>
		/// 使用标准的日志信息记录格式。
		/// </remarks>
		ConsoleA = 0X0006,

		/// <summary>
		/// 实现类别的控制台输出的日志。
		/// </summary>
		/// <remarks>
		/// 使用可读的日志信息记录格式。
		/// </remarks>
		ConsoleB = 0X0007,

		/// <summary>
		/// 日志对象的锁定装饰类别。
		/// </summary>
		Lock = 0X0100,

		/// <summary>
		/// 日志对象的互斥装饰类别。
		/// </summary>
		Mutex = 0X0200,

		/// <summary>
		/// 日志对象的加密装饰类别。
		/// </summary>
		Encrypt = 0X0400,

		/// <summary>
		/// 日志对象的控制台装饰类别。
		/// </summary>
		ConsoleDecorator = 0x0800,
	}
}
