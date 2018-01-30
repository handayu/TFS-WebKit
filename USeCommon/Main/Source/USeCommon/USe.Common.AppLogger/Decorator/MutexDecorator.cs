using System;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Diagnostics;

namespace USe.Common.AppLogger.Decorator
{
    /// <summary>
	/// 日志互斥同步类，以Mutex实现同步控制。
	/// </summary>
	public class MutexDecorator : AbstractDecorator
    {
        private Mutex m_mutex;

        /// <summary>
		/// 初始化LockDecorator类的新实例。
		/// </summary>
        /// <param name="mutexName">Mutex对象名称。</param>
		/// <param name="innerImpl">内部实现类(IAppLoggerImpl)对象。</param>
		/// <exception cref="System.ArgumentNullException">mutexName或innerImpl参数为null或empty时。</exception>
		public MutexDecorator(string mutexName, IAppLoggerImpl innerImpl)
            : base(innerImpl)
        {
			if (String.IsNullOrEmpty(mutexName))
			{
				throw new ArgumentNullException("MutexName can not be null.");
			}

			m_mutex = new Mutex(false, FormatMutexName(mutexName));
        }


        /// <summary>
		/// MutexDecorator类对象的析构方法。
		/// </summary>
		~MutexDecorator()
        {
            Dispose(false);
        }

		/// <summary>
		/// 释放MutexDecorator类对象所占用的资源。
		/// </summary>
		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 释放MutexDecorator类对象所占用的非托管资源，也可以根据需要释放托管资源。
		/// </summary>
		/// <param name="disposing">资源释放标志，为true则释放托管资源和非托管资源；为false则仅释放非托管资源。</param>
		protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (m_mutex != null)
            {
                m_mutex.Close();
                m_mutex = null;
            }
        }


		/// <summary>
		/// 获取MutexDecorator类对象的名称。
		/// </summary>
		public override string Name
		{
			get
			{
				Debug.Assert(m_mutex != null);

				m_mutex.WaitOne();
				try
				{
					return base.Name;
				}
				finally
				{
					m_mutex.ReleaseMutex();
				}
			}
		}

		/// <summary>
		/// 获取MutexDecorator类对象的编码格式。
		/// </summary>
		public override Encoding Encoding
        {
            get
            {
                Debug.Assert(m_mutex != null);

                m_mutex.WaitOne();
                try
                {
                    return base.Encoding;
                }
                finally
                {
                    m_mutex.ReleaseMutex();
                }
            }

            set
            {
				Debug.Assert(m_mutex != null);

                m_mutex.WaitOne();
                try
                {
                    base.Encoding = value;
                }
                finally
                {
                    m_mutex.ReleaseMutex();
                }
            }
        }

		/// <summary>
		/// 获取MutexDecorator类对象的线程安全标志。
		/// </summary>
		public override bool IsThreadSafe
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// 清空MutexDecorator类对象的日志输出缓冲区。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public override bool Flush()
		{
			Debug.Assert(m_mutex != null);

			m_mutex.WaitOne();
			try
			{
				return base.Flush();
			}
			finally
			{
				m_mutex.ReleaseMutex();
			}
		}

		/// <summary>
		/// MutexDecorator类对象写入回车换行。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public override bool LineFeed()
        {
			Debug.Assert(m_mutex != null);

            m_mutex.WaitOne();
            try
            {
                return base.LineFeed();
            }
            finally
            {
                m_mutex.ReleaseMutex();
            }
        }

		/// <summary>
		/// MutexDecorator类对象写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="eventText">日志事件记录文本。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public override bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
		{
			Debug.Assert(m_mutex != null);

			m_mutex.WaitOne();
            try
            {
				return base.WriteEvent(eventType, eventText, lineFeed);
            }
            finally
            {
                m_mutex.ReleaseMutex();
            }
        }

		/// <summary>
		/// MutexDecorator类对象写入一条事件信息。
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
			Debug.Assert(m_mutex != null);

			m_mutex.WaitOne();
			try
			{
				return base.WriteEvent(eventType, bytes, startIndex, count, lineFeed);
			}
			finally
			{
				m_mutex.ReleaseMutex();
			}
		}



        /// <summary>
        /// 转换成规定的Mutex对象名名称
        /// </summary>
		/// <param name="name">Mutex对象名称，不允许为null/Empty</param>
		/// <returns>Mutex对象名称</returns>
		private static string FormatMutexName(string name)
        {
			Debug.Assert(!String.IsNullOrEmpty(name));

			return "Global\\" + name.Replace("\\", "%5C");
        }
    }
}
