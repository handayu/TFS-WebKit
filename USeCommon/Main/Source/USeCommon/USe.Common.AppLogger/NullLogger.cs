using System;
using System.Text;
using System.Diagnostics;

namespace USe.Common.AppLogger
{
    /// <summary>
    /// 空应用程序日志类。
    /// </summary>
    /// <remarks>
	/// 应用程序日志接口的空实现，不执行任何实际的日志输出逻辑，供测试或禁止日志功能的场合使用。
    /// </remarks>
	public class NullLogger : IAppLogger
    {
        private string m_name;			// 日志名称
        private Encoding m_encoding;	// 日志编码

        /// <summary>
		/// 初始化NullLogger类的新实例。
        /// </summary>
		/// <param name="name">日志对象名称。</param>
		/// <exception cref="System.ArgumentNullException">name参数为null或empty时。</exception>
		public NullLogger(string name)
        {
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("Name can not be null or empty.");
			}

			m_name = name;
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
		/// NullLogger类对象写入一条Critical类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteCritical(string text)
		{
			return true;
		}

		/// <summary>
		/// NullLogger类对象写入一条Error类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteError(string text)
		{
			return true;
		}

		/// <summary>
		/// NullLogger类对象写入一条Warning类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteWarning(string text)
		{
			return true;
		}

		/// <summary>
		/// NullLogger类对象写入一条Information类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteInformation(string text)
		{
			return true;
		}

		/// <summary>
		/// NullLogger类对象写入一条Verbose类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteVerbose(string text)
		{
			return true;
		}

		/// <summary>
		/// NullLogger类对象写入一条Notice类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteNotice(string text)
		{
			return true;
		}

		/// <summary>
		/// 应用程序日志写入一条Inbound类型事件信息。
		/// </summary>
		/// <param name="message">入站消息字符串。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteInbound(string message)
		{
			return true;
		}

		/// <summary>
		/// NullLogger类对象写入一条Inbound类型事件信息。
		/// </summary>
		/// <param name="bytes">待写入的日志信息字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteInbound(byte[] bytes, int startIndex, int count)
		{
			return true;
		}

		/// <summary>
		/// 应用程序日志写入一条Outbound类型事件信息。
		/// </summary>
		/// <param name="message">出站消息字符串。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteOutbound(string message)
		{
			return true;
		}

		/// <summary>
		/// NullLogger类对象写入一条Outbound类型事件信息。
		/// </summary>
		/// <param name="bytes">待写入的日志信息字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteOutbound(byte[] bytes, int startIndex, int count)
		{
			return true;
		}

		/// <summary>
		/// 应用程序日志写入一条Message类型事件信息。
		/// </summary>
		/// <param name="message">消息字符串。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteMessage(string message)
		{
			return true;
		}

		/// <summary>
		/// NullLogger类对象写入一条Message类型事件信息。
		/// </summary>
		/// <param name="bytes">待写入的日志信息字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteMessage(byte[] bytes, int startIndex, int count)
		{
			return true;
		}

		/// <summary>
		/// 应用程序日志写入一条Audit类型事件信息。
		/// </summary>
		/// <param name="message">消息字符串。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteAudit(string message)
		{
			return true;
		}

		/// <summary>
		/// 应用程序日志写入一条Audit类型事件信息。
		/// </summary>
		/// <param name="bytes">待写入的日志信息字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteAudit(byte[] bytes, int startIndex, int count)
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
            return "NullLogger<" + Process.GetCurrentProcess().ProcessName + ">";
        }
    }
}
