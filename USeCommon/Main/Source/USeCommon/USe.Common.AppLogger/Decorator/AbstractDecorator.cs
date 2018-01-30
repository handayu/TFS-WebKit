using System;
using System.Text;
using System.Diagnostics;

namespace USe.Common.AppLogger.Decorator
{
    /// <summary>
    /// ʵ��Decoratorģʽ�ĳ�����ࡣ
    /// </summary>
    public abstract class AbstractDecorator : IAppLoggerImpl
    {
		/// <summary>
		/// >�ڲ�ʵ����(IAppLoggerImpl)����
		/// </summary>
        protected IAppLoggerImpl m_innerImpl;


        /// <summary>
		/// ��ʼ��AbstractDecorator�����ʵ����
		/// </summary>
		/// <param name="innerImpl">�ڲ�ʵ����(IAppLoggerImpl)����</param>
		/// <exception cref="System.ArgumentNullException">innerImpl����Ϊnullʱ��</exception>
		public AbstractDecorator(IAppLoggerImpl innerImpl)
        {
			if (innerImpl == null)
			{
				throw new ArgumentNullException("InnerImpl can not be null.");
			}

            m_innerImpl = innerImpl;
        }

        /// <summary>
		/// AbstractDecorator����������������
		/// </summary>
		~AbstractDecorator()
        {
            Dispose(false);
        }

		/// <summary>
		/// �ͷ�AbstractDecorator�������ռ�õ���Դ��
		/// </summary>
		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// �ͷ�AbstractDecorator�������ռ�õķ��й���Դ��Ҳ���Ը�����Ҫ�ͷ��й���Դ��
		/// </summary>
		/// <param name="disposing">��Դ�ͷű�־��Ϊtrue���ͷ��й���Դ�ͷ��й���Դ��Ϊfalse����ͷŷ��й���Դ��</param>
		protected virtual void Dispose(bool disposing)
		{
			Debug.Assert(m_innerImpl != null);
             m_innerImpl.Dispose();
        }


		/// <summary>
		/// ��ȡAbstractDecorator���������ơ�
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
		/// ��ȡAbstractDecorator�����ı����ʽ��
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
		/// ��ȡAbstractDecorator�������̰߳�ȫ��־��
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
		/// ���AbstractDecorator��������־�����������
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public virtual bool Flush()
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.Flush();
		}

		/// <summary>
		/// AbstractDecorator�����д��س����С�
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public virtual bool LineFeed()
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.LineFeed();
		}

		/// <summary>
		/// AbstractDecorator�����д��һ���¼���Ϣ��
		/// </summary>
		/// <param name="eventType">��־�¼����͡�</param>
		/// <param name="eventText">��־�¼���¼�ı���</param>
		/// <param name="lineFeed">�س����б�־��</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public virtual bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(eventType, eventText, lineFeed);
		}

		/// <summary>
		/// AbstractDecorator�����д��һ���¼���Ϣ��
		/// </summary>
		/// <param name="eventType">��־�¼����͡�</param>
		/// <param name="bytes">��־�¼���¼���ֽ����л�������</param>
		/// <param name="startIndex">��־�¼���¼���ֽ����л����������ʼ������</param>
		/// <param name="count">��־�¼���¼���ֽ�������</param>
		/// <param name="lineFeed">�س����б�־��</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public virtual bool WriteEvent(LogEventType eventType, byte[] bytes, int startIndex, int count, bool lineFeed)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(eventType, bytes, startIndex, count, lineFeed);
		}
	}
}

