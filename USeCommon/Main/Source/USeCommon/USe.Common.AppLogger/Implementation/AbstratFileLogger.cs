using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using USe.Common.AppLogger.EventFormatter;

namespace USe.Common.AppLogger.Implementation
{
    /// <summary>
    /// 文件日志抽象基类。
    /// </summary>
	public abstract class AbstratFileLogger : IAppLoggerImpl
    {
		/// <summary>
		/// 日志文件名称。
		/// </summary>
        protected string m_fileName;
        private Encoding m_encoding;
        private IEventFormatter m_formatter;
        
        /// <summary>
		/// 初始化AbstratFileLogger类的新实例。
		/// </summary>
        /// <param name="fileName">日志文件的全路径名称。</param>
		/// <param name="encoding">日志信息编码格式。</param>
		/// <param name="formatter">事件消息格式化对象。</param>
		/// <exception cref="System.ArgumentNullException">fileName、encoding和formatter参数任一为null或empty时。</exception>
		public AbstratFileLogger(string fileName, Encoding encoding, IEventFormatter formatter)
        {
			if (String.IsNullOrEmpty(fileName))
			{
				throw new ArgumentNullException("FileName can not be null or empty.");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("Encoding can not be null or empty.");
			}
			if (formatter == null)
			{
				throw new ArgumentNullException("Formatter can not be null or empty.");
			}

			FileInfo info = new FileInfo(fileName);
			if (!Directory.Exists(info.DirectoryName))
			{
				Directory.CreateDirectory(info.DirectoryName);
			}

			m_fileName = info.FullName;
			m_encoding = encoding;
			m_formatter = formatter;
		}
        
		/// <summary>
		/// AbstratFileLogger类对象的析构方法。
		/// </summary>
		~AbstratFileLogger()
		{
			Dispose(false);
		}

		/// <summary>
		/// 释放AbstratFileLogger类对象所占用的资源。
		/// </summary>
		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 释放AbstratFileLogger类对象所占用的非托管资源，也可以根据需要释放托管资源。
		/// </summary>
		/// <param name="disposing">资源释放标志，为true则释放托管资源和非托管资源；为false则仅释放非托管资源。</param>
		protected virtual void Dispose(bool disposing)
		{
		}


		/// <summary>
		/// 获取AbstratFileLogger类对象的名称。
		/// </summary>
		public string Name
        {
            get
            {
                return m_fileName;
            }
        }

		/// <summary>
		/// 获取AbstratFileLogger类对象的编码格式。
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
		/// 获取AbstratFileLogger类对象的线程安全标志。
		/// </summary>
		public bool IsThreadSafe
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 清空AbstratFileLogger类对象的日志输出缓冲区。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public abstract bool Flush();

		/// <summary>
		/// AbstratFileLogger类对象写入回车换行。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool LineFeed()
		{
			Debug.Assert(m_encoding != null);
			//byte[] bytes = m_encoding.GetBytes(System.Environment.NewLine);
			byte[] bytes = m_encoding.GetBytes("\r\n");
			return do_Write(bytes);
		}

		/// <summary>
		/// AbstratFileLogger类对象写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="eventText">日志事件记录文本。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public virtual bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
		{
			Debug.Assert(m_encoding != null);
			byte[] bytes = m_encoding.GetBytes(m_formatter.Format(eventType, eventText, lineFeed));
            return do_Write(bytes, 0, bytes.Length);
        }

		/// <summary>
		/// AbstratFileLogger类对象写入一条事件信息。
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
			Debug.Assert(m_encoding != null);
			byte[] header = m_encoding.GetBytes(m_formatter.Format(eventType, String.Empty, false));
			if (!do_Write(header, 0, header.Length))
			{
				return false;
			}

			if (!do_Write(bytes, startIndex, count))
			{
				return false;
			}

			if (lineFeed)
			{
				if (!LineFeed())
				{
					return false;
				}
			}

			return true;
		}


		/// <summary>
		/// 日志文件的写入操作。
		/// </summary>
		/// <param name="bytes">待写入的字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志文件写入成功与否标志。
		/// </returns>
		protected abstract bool do_Write(byte[] bytes, int startIndex, int count);

		/// <summary>
		/// 日志文件的写入操作。
		/// </summary>
		/// <param name="bytes">待写入的字节序列缓冲区。</param>
		/// <returns>
		/// 日志文件写入成功与否标志。
		/// </returns>
		protected virtual bool do_Write(byte[] bytes)
		{
			return do_Write(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// 获取缺省的日志文件名称。
		/// </summary>
		/// <param name="checkUAC">是否检查UAC标志。</param>
		/// <returns>
		/// 缺省的日志文件名称。
		/// </returns> 
		/// <remarks>
		/// 日志文件名称由执行程序的全路径名称转换而来，即将执行程序名称的扩展名替换为.log。
		/// </remarks>
		public static string GetDefaultFileName(bool checkUAC)
		{
			FileInfo fileInfo = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);

            string fileName;
			if (checkUAC && OSVersionHelper.HasUAC())
            {
                string appFolder = fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length);
                fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appFolder);
                fileName = Path.Combine(fileName, fileInfo.Name);
            }
            else
            {
                fileName = fileInfo.FullName;
            }

			// 判断当前文件名是否含扩展名
			if (string.IsNullOrEmpty(fileInfo.Extension) == false)
			{
				// 去掉文件的扩展名
				fileName = fileName.Remove(fileName.Length - fileInfo.Extension.Length);
			}

			return fileName + ".log";
		}
	}
}
