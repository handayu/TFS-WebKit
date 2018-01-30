using System;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace USe.Common.AppLogger.Decorator
{
    /// <summary>
    /// ��־����ͬ���࣬��lockʵ��ͬ�����ơ�
    /// </summary>
	public class LockDecorator : AbstractDecorator
    {
		private object m_syncRoot = new object();

        /// <summary>
		/// ��ʼ��LockDecorator�����ʵ����
		/// </summary>
		/// <param name="innerImpl">�ڲ�ʵ����(IAppLoggerImpl)����</param>
		/// <exception cref="System.ArgumentNullException">innerImpl����Ϊnullʱ��</exception>
		public LockDecorator(IAppLoggerImpl innerImpl)
            : base(innerImpl)
        {
        }


		/// <summary>
		/// ��ȡLockDecorator���������ơ�
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
		/// ��ȡLockDecorator�����ı����ʽ��
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
		/// ��ȡLockDecorator�������̰߳�ȫ��־��
		/// </summary>
		public override bool IsThreadSafe
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// ���LockDecorator��������־�����������
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public override bool Flush()
		{
			lock (m_syncRoot)
			{
				return base.Flush();
			}
		}

		/// <summary>
		/// LockDecorator�����д��س����С�
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public override bool LineFeed()
		{
			lock (m_syncRoot)
			{
				return base.LineFeed();
			}
		}

		/// <summary>
		/// LockDecorator�����д��һ���¼���Ϣ��
		/// </summary>
		/// <param name="eventType">��־�¼����͡�</param>
		/// <param name="eventText">��־�¼���¼�ı���</param>
		/// <param name="lineFeed">�س����б�־��</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public override bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
        {
			lock (m_syncRoot)
            {
				return base.WriteEvent(eventType, eventText, lineFeed);
            }
        }

		/// <summary>
		/// LockDecorator�����д��һ���¼���Ϣ��
		/// </summary>
		/// <param name="eventType">��־�¼����͡�</param>
		/// <param name="bytes">��־�¼���¼���ֽ����л�������</param>
		/// <param name="startIndex">��־�¼���¼���ֽ����л����������ʼ������</param>
		/// <param name="count">��־�¼���¼���ֽ�������</param>
		/// <param name="lineFeed">�س����б�־��</param>
		/// <returns>
		/// ��־д��ɹ�����־��
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
