using System;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace USe.Common.AppLogger.Decorator
{
    /// <summary>
    /// 日志锁定同步类，以lock实现同步控制。
    /// </summary>
	public class LockDecorator : AbstractDecorator
    {
		private object m_syncRoot = new object();

        /// <summary>
		/// 初始化LockDecorator类的新实例。
		/// </summary>
		/// <param name="innerImpl">内部实现类(IAppLoggerImpl)对象。</param>
		/// <exception cref="System.ArgumentNullException">innerImpl参数为null时。</exception>
		public LockDecorator(IAppLoggerImpl innerImpl)
            : base(innerImpl)
        {
        }


		/// <summary>
		/// 获取LockDecorator类对象的名称。
		/// </summary>
		public override string Name
		{
			get
			{
				lock (m_syncRoot)
				{
					return base.Name;
				}
			}
		}

		/// <summary>
		/// 获取LockDecorator类对象的编码格式。
		/// </summary>
		public override Encoding Encoding
        {
            get
            {
				lock (m_syncRoot)
                {
                    return base.Encoding;
                }
            }

            set
            {
				lock (m_syncRoot)
                {
                    base.Encoding = value;
                }
            }
        }

		/// <summary>
		/// 获取LockDecorator类对象的线程安全标志。
		/// </summary>
		public override bool IsThreadSafe
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// 清空LockDecorator类对象的日志输出缓冲区。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public override bool Flush()
		{
			lock (m_syncRoot)
			{
				return base.Flush();
			}
		}

		/// <summary>
		/// LockDecorator类对象写入回车换行。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public override bool LineFeed()
		{
			lock (m_syncRoot)
			{
				return base.LineFeed();
			}
		}

		/// <summary>
		/// LockDecorator类对象写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="eventText">日志事件记录文本。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public override bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
        {
			lock (m_syncRoot)
            {
				return base.WriteEvent(eventType, eventText, lineFeed);
            }
        }

		/// <summary>
		/// LockDecorator类对象写入一条事件信息。
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
			lock (m_syncRoot)
			{
				return base.WriteEvent(eventType, bytes, startIndex, count, lineFeed);
			}
		}
	}
}
