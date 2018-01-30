using System;
using System.Text;
using System.Diagnostics;

namespace USe.Common.AppLogger.Decorator
{
    /// <summary>
    /// 实现Decorator模式的抽象基类。
    /// </summary>
    public abstract class AbstractDecorator : IAppLoggerImpl
    {
		/// <summary>
		/// >内部实现类(IAppLoggerImpl)对象。
		/// </summary>
        protected IAppLoggerImpl m_innerImpl;


        /// <summary>
		/// 初始化AbstractDecorator类的新实例。
		/// </summary>
		/// <param name="innerImpl">内部实现类(IAppLoggerImpl)对象。</param>
		/// <exception cref="System.ArgumentNullException">innerImpl参数为null时。</exception>
		public AbstractDecorator(IAppLoggerImpl innerImpl)
        {
			if (innerImpl == null)
			{
				throw new ArgumentNullException("InnerImpl can not be null.");
			}

            m_innerImpl = innerImpl;
        }

        /// <summary>
		/// AbstractDecorator类对象的析构方法。
		/// </summary>
		~AbstractDecorator()
        {
            Dispose(false);
        }

		/// <summary>
		/// 释放AbstractDecorator类对象所占用的资源。
		/// </summary>
		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 释放AbstractDecorator类对象所占用的非托管资源，也可以根据需要释放托管资源。
		/// </summary>
		/// <param name="disposing">资源释放标志，为true则释放托管资源和非托管资源；为false则仅释放非托管资源。</param>
		protected virtual void Dispose(bool disposing)
		{
			Debug.Assert(m_innerImpl != null);
             m_innerImpl.Dispose();
        }


		/// <summary>
		/// 获取AbstractDecorator类对象的名称。
		/// </summary>
		public virtual string Name
        {
			get
			{
				Debug.Assert(m_innerImpl != null);
				return m_innerImpl.Name;
			}
		}

		/// <summary>
		/// 获取AbstractDecorator类对象的编码格式。
		/// </summary>
		public virtual Encoding Encoding
		{
			get
			{
				Debug.Assert(m_innerImpl != null);
				return m_innerImpl.Encoding;
			}

			set
			{
				Debug.Assert(m_innerImpl != null);
				m_innerImpl.Encoding = value;
			}
		}

		/// <summary>
		/// 获取AbstractDecorator类对象的线程安全标志。
		/// </summary>
		public virtual bool IsThreadSafe
		{
			get
			{
				Debug.Assert(m_innerImpl != null);
				return m_innerImpl.IsThreadSafe;
			}
		}

		/// <summary>
		/// 清空AbstractDecorator类对象的日志输出缓冲区。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public virtual bool Flush()
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.Flush();
		}

		/// <summary>
		/// AbstractDecorator类对象写入回车换行。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public virtual bool LineFeed()
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.LineFeed();
		}

		/// <summary>
		/// AbstractDecorator类对象写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="eventText">日志事件记录文本。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public virtual bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(eventType, eventText, lineFeed);
		}

		/// <summary>
		/// AbstractDecorator类对象写入一条事件信息。
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
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(eventType, bytes, startIndex, count, lineFeed);
		}
	}
}

