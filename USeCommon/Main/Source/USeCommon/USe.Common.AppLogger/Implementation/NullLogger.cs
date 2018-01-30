using System;
using System.Text;
using System.Diagnostics;

using USe.Common.AppLogger.EventFormatter;

namespace USe.Common.AppLogger.Implementation
{
	/// <summary>
	/// 按NULL模式实现的日志实现类，用于测试或关闭实际的日志输出
	/// </summary>
	public class NullLogger : IAppLoggerImpl
	{
		private string m_name;
		private IEventFormatter m_formatter;
		private Encoding m_encoding;

		///<summary>
		/// 初始化NullLogger类的新实例。
		/// </summary>
		/// <param name="name">日志对象名称。</param>
		/// <param name="formatter">事件消息格式化对象，不能为null。</param>
		/// <exception cref="System.ArgumentNullException">name和/或formatter参数为null或empty时。</exception>
		public NullLogger(string name, IEventFormatter formatter)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("Name can not be null or empty.");
			}
			if (formatter == null)
			{
				throw new ArgumentNullException("Formatter can not be null or empty.");
			}

			m_name = name;
			m_formatter = formatter;

			m_encoding = Encoding.Default;
		}

		/// <summary>
		/// NullLogger类对象的析构方法。
		/// </summary>
		~NullLogger()
		{
			Dispose(false);
		}

		/// <summary>
		/// 释放NullLogger类对象所占用的资源。
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 释放NullLogger类对象所占用的非托管资源，也可以根据需要释放托管资源。
		/// </summary>
		/// <param name="disposing">资源释放标志，为true则释放托管资源和非托管资源；为false则仅释放非托管资源。</param>
		protected virtual void Dispose(bool disposing)
		{
		}


		/// <summary>
		/// 获取NullLogger类对象的名称。
		/// </summary>
		public string Name
		{
			get
			{
				return m_name;
			}
		}

		/// <summary>
		/// 获取NullLogger类对象的编码格式。
		/// </summary>
		public Encoding Encoding
		{
			get
			{
				return m_encoding;
			}

			set
			{
				m_encoding = value;
			}
		}

		/// <summary>
		/// 获取NullLogger类对象的线程安全标志。
		/// </summary>
		public bool IsThreadSafe
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// 清空NullLogger类对象的日志输出缓冲区。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool Flush()
		{
			return true;
		}

		/// <summary>
		/// NullLogger类对象写入回车换行。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool LineFeed()
		{
			return true;
		}

		/// <summary>
		/// NullLogger类对象写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="eventText">日志事件记录文本。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public virtual bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
		{
			return true;
		}

		/// <summary>
		/// NullLogger类对象写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="bytes">日志事件记录的字节序列缓冲区。</param>
		/// <param name="startIndex">日志事件记录在字节序列缓冲区里的起始索引。</param>
		/// <param name="count">日志事件记录的字节数量。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public virtual bool WriteEvent(LogEventType eventType, byte[] bytes, int startIndex, int count, bool lineFeed)
		{
			return true;
		}


		/// <summary> 
		/// 获取缺省的NullLogger类对象名称。
		/// </summary>
		/// <returns>
		/// 缺省的NullLogger类对象名称。
		/// </returns>
		public static string GetDefaultName()
		{
			return "NullLoggerImpl<" + Process.GetCurrentProcess().ProcessName + ">";
		}
	}
}
