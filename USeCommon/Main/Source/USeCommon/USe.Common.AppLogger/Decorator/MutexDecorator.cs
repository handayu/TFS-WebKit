using System;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Diagnostics;

namespace USe.Common.AppLogger.Decorator
{
    /// <summary>
	/// ��־����ͬ���࣬��Mutexʵ��ͬ�����ơ�
	/// </summary>
	public class MutexDecorator : AbstractDecorator
    {
        private Mutex m_mutex;

        /// <summary>
		/// ��ʼ��LockDecorator�����ʵ����
		/// </summary>
        /// <param name="mutexName">Mutex�������ơ�</param>
		/// <param name="innerImpl">�ڲ�ʵ����(IAppLoggerImpl)����</param>
		/// <exception cref="System.ArgumentNullException">mutexName��innerImpl����Ϊnull��emptyʱ��</exception>
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
		/// MutexDecorator����������������
		/// </summary>
		~MutexDecorator()
        {
            Dispose(false);
        }

		/// <summary>
		/// �ͷ�MutexDecorator�������ռ�õ���Դ��
		/// </summary>
		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// �ͷ�MutexDecorator�������ռ�õķ��й���Դ��Ҳ���Ը�����Ҫ�ͷ��й���Դ��
		/// </summary>
		/// <param name="disposing">��Դ�ͷű�־��Ϊtrue���ͷ��й���Դ�ͷ��й���Դ��Ϊfalse����ͷŷ��й���Դ��</param>
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
		/// ��ȡMutexDecorator���������ơ�
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
		/// ��ȡMutexDecorator�����ı����ʽ��
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
		/// ��ȡMutexDecorator�������̰߳�ȫ��־��
		/// </summary>
		public override bool IsThreadSafe
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// ���MutexDecorator��������־�����������
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
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
		/// MutexDecorator�����д��س����С�
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
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
		/// MutexDecorator�����д��һ���¼���Ϣ��
		/// </summary>
		/// <param name="eventType">��־�¼����͡�</param>
		/// <param name="eventText">��־�¼���¼�ı���</param>
		/// <param name="lineFeed">�س����б�־��</param>
		/// <returns>
		/// ��־д��ɹ�����־��
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
		/// MutexDecorator�����д��һ���¼���Ϣ��
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
        /// ת���ɹ涨��Mutex����������
        /// </summary>
		/// <param name="name">Mutex�������ƣ�������Ϊnull/Empty</param>
		/// <returns>Mutex��������</returns>
		private static string FormatMutexName(string name)
        {
			Debug.Assert(!String.IsNullOrEmpty(name));

			return "Global\\" + name.Replace("\\", "%5C");
        }
    }
}
