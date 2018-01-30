using System;
using System.Text;
using System.Diagnostics;

namespace USe.Common.AppLogger
{
    /// <summary>
    /// ��Ӧ�ó�����־�ࡣ
    /// </summary>
    /// <remarks>
	/// Ӧ�ó�����־�ӿڵĿ�ʵ�֣���ִ���κ�ʵ�ʵ���־����߼��������Ի��ֹ��־���ܵĳ���ʹ�á�
    /// </remarks>
	public class NullLogger : IAppLogger
    {
        private string m_name;			// ��־����
        private Encoding m_encoding;	// ��־����

        /// <summary>
		/// ��ʼ��NullLogger�����ʵ����
        /// </summary>
		/// <param name="name">��־�������ơ�</param>
		/// <exception cref="System.ArgumentNullException">name����Ϊnull��emptyʱ��</exception>
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
		/// NullLogger����������������
		/// </summary>
		~NullLogger()
		{
			Dispose(false);
		}

		/// <summary>
		/// �ͷ�NullLogger�������ռ�õ���Դ��
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// �ͷ�NullLogger�������ռ�õķ��й���Դ��Ҳ���Ը�����Ҫ�ͷ��й���Դ��
		/// </summary>
		/// <param name="disposing">��Դ�ͷű�־��Ϊtrue���ͷ��й���Դ�ͷ��й���Դ��Ϊfalse����ͷŷ��й���Դ��</param>
		protected virtual void Dispose(bool disposing)
		{
		}


        /// <summary>
		/// ��ȡNullLogger���������ơ�
		/// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
        }

        /// <summary>
		/// ��ȡNullLogger�����ı����ʽ��
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
		/// ��ȡNullLogger�������̰߳�ȫ��־��
		/// </summary>
		public bool IsThreadSafe
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// ���NullLogger��������־�����������
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool Flush()
		{
			return true;
		}

		/// <summary>
		/// NullLogger�����д��س����С�
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool LineFeed()
        {
            return true;
        }

		/// <summary>
		/// NullLogger�����д��һ���¼���Ϣ��
		/// </summary>
		/// <param name="eventType">��־�¼����͡�</param>
		/// <param name="eventText">��־�¼���¼�ı���</param>
		/// <param name="lineFeed">�س����б�־��</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public virtual bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
        {
            return true;
        }

		/// <summary>
		/// NullLogger�����д��һ���¼���Ϣ��
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
			return true;
		}

		/// <summary>
		/// NullLogger�����д��һ��Critical�����¼���Ϣ��
		/// </summary>
		/// <param name="text">��־�¼���¼�ı���</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteCritical(string text)
		{
			return true;
		}

		/// <summary>
		/// NullLogger�����д��һ��Error�����¼���Ϣ��
		/// </summary>
		/// <param name="text">��־�¼���¼�ı���</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteError(string text)
		{
			return true;
		}

		/// <summary>
		/// NullLogger�����д��һ��Warning�����¼���Ϣ��
		/// </summary>
		/// <param name="text">��־�¼���¼�ı���</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteWarning(string text)
		{
			return true;
		}

		/// <summary>
		/// NullLogger�����д��һ��Information�����¼���Ϣ��
		/// </summary>
		/// <param name="text">��־�¼���¼�ı���</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteInformation(string text)
		{
			return true;
		}

		/// <summary>
		/// NullLogger�����д��һ��Verbose�����¼���Ϣ��
		/// </summary>
		/// <param name="text">��־�¼���¼�ı���</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteVerbose(string text)
		{
			return true;
		}

		/// <summary>
		/// NullLogger�����д��һ��Notice�����¼���Ϣ��
		/// </summary>
		/// <param name="text">��־�¼���¼�ı���</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteNotice(string text)
		{
			return true;
		}

		/// <summary>
		/// Ӧ�ó�����־д��һ��Inbound�����¼���Ϣ��
		/// </summary>
		/// <param name="message">��վ��Ϣ�ַ�����</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteInbound(string message)
		{
			return true;
		}

		/// <summary>
		/// NullLogger�����д��һ��Inbound�����¼���Ϣ��
		/// </summary>
		/// <param name="bytes">��д�����־��Ϣ�ֽ����л�������</param>
		/// <param name="startIndex">��д���ֽ������ڻ������е���ʼ������</param>
		/// <param name="count">��д���ֽ����е�������</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteInbound(byte[] bytes, int startIndex, int count)
		{
			return true;
		}

		/// <summary>
		/// Ӧ�ó�����־д��һ��Outbound�����¼���Ϣ��
		/// </summary>
		/// <param name="message">��վ��Ϣ�ַ�����</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteOutbound(string message)
		{
			return true;
		}

		/// <summary>
		/// NullLogger�����д��һ��Outbound�����¼���Ϣ��
		/// </summary>
		/// <param name="bytes">��д�����־��Ϣ�ֽ����л�������</param>
		/// <param name="startIndex">��д���ֽ������ڻ������е���ʼ������</param>
		/// <param name="count">��д���ֽ����е�������</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteOutbound(byte[] bytes, int startIndex, int count)
		{
			return true;
		}

		/// <summary>
		/// Ӧ�ó�����־д��һ��Message�����¼���Ϣ��
		/// </summary>
		/// <param name="message">��Ϣ�ַ�����</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteMessage(string message)
		{
			return true;
		}

		/// <summary>
		/// NullLogger�����д��һ��Message�����¼���Ϣ��
		/// </summary>
		/// <param name="bytes">��д�����־��Ϣ�ֽ����л�������</param>
		/// <param name="startIndex">��д���ֽ������ڻ������е���ʼ������</param>
		/// <param name="count">��д���ֽ����е�������</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteMessage(byte[] bytes, int startIndex, int count)
		{
			return true;
		}

		/// <summary>
		/// Ӧ�ó�����־д��һ��Audit�����¼���Ϣ��
		/// </summary>
		/// <param name="message">��Ϣ�ַ�����</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteAudit(string message)
		{
			return true;
		}

		/// <summary>
		/// Ӧ�ó�����־д��һ��Audit�����¼���Ϣ��
		/// </summary>
		/// <param name="bytes">��д�����־��Ϣ�ֽ����л�������</param>
		/// <param name="startIndex">��д���ֽ������ڻ������е���ʼ������</param>
		/// <param name="count">��д���ֽ����е�������</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool WriteAudit(byte[] bytes, int startIndex, int count)
		{
			return true;
		}


        /// <summary> 
		/// ��ȡȱʡ��NullLogger��������ơ�
        /// </summary>
		/// <returns>
		/// ȱʡ��NullLogger��������ơ�
		/// </returns>
		public static string GetDefaultName()
        {
            return "NullLogger<" + Process.GetCurrentProcess().ProcessName + ">";
        }
    }
}
