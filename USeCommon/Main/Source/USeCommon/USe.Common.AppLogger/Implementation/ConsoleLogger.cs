using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using USe.Common.AppLogger.EventFormatter;

namespace USe.Common.AppLogger.Implementation
{
    /// <summary>
    /// Console���������־�ࡣ
    /// </summary>
	public class ConsoleLogger : IAppLoggerImpl
    {
        private string m_name;
        private Encoding m_encoding;
        private IEventFormatter m_formatter;

		/// <summary>
		/// ��ʼ��ConsoleLogger�����ʵ����
		/// </summary>
		/// <param name="name">��־�������ơ�</param>
		/// <param name="encoding">��־��Ϣ�����ʽ��</param>
		/// <param name="formatter">�¼���Ϣ��ʽ������</param>
		/// <exception cref="System.ArgumentNullException">name��encoding��formatter������һΪnull��emptyʱ��</exception>
		public ConsoleLogger(string name, Encoding encoding, IEventFormatter formatter)
        {
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("Name can not be null or empty.");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("Encoding can not be null or empty.");
			}
			if (formatter == null)
			{
				throw new ArgumentNullException("Formatter can not be null or empty.");
			}

			m_name = name;
			m_encoding = encoding;
			m_formatter = formatter;
        }
        
		/// <summary>
		/// ConsoleLogger����������������
		/// </summary>
		~ConsoleLogger()
		{
			Dispose(false);
		}

		/// <summary>
		/// �ͷ�ConsoleLogger�������ռ�õ���Դ��
		/// </summary>
		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// �ͷ�ConsoleLogger�������ռ�õķ��й���Դ��Ҳ���Ը�����Ҫ�ͷ��й���Դ��
		/// </summary>
		/// <param name="disposing">��Դ�ͷű�־��Ϊtrue���ͷ��й���Դ�ͷ��й���Դ��Ϊfalse����ͷŷ��й���Դ��</param>
		protected virtual void Dispose(bool disposing)
		{
		}


		/// <summary>
		/// ��ȡConsoleLogger���������ơ�
		/// </summary>
		public string Name
		{
			get
			{
				return m_name;
			}
		}

		/// <summary>
		/// ��ȡConsoleLogger�����ı����ʽ��
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
		/// ��ȡConsoleLogger�������̰߳�ȫ��־��
		/// </summary>
		public bool IsThreadSafe
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// ���ConsoleLogger��������־�����������
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool Flush()
		{
			Console.Out.Flush();
			return true;
		}

		/// <summary>
		/// ConsoleLogger�����д��س����С�
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool LineFeed()
        {
            Console.Write(System.Environment.NewLine);
            return true;
        }

		/// <summary>
		/// ConsoleLogger�����д��һ���¼���Ϣ��
		/// </summary>
		/// <param name="eventType">��־�¼����͡�</param>
		/// <param name="eventText">��־�¼���¼�ı���</param>
		/// <param name="lineFeed">�س����б�־��</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public virtual bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
		{
			ConsoleColor color = Console.ForegroundColor; // ConsoleColor.Gray

			switch (eventType)
			{
				case LogEventType.Critical:
				case LogEventType.Error:
					Console.ForegroundColor = ConsoleColor.Red;
					break;

				case LogEventType.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;

				case LogEventType.Notice:
					Console.ForegroundColor = ConsoleColor.Green;
					break;
			}

            Console.Write(m_formatter.Format(eventType, eventText, lineFeed));

			Console.ForegroundColor = color;

            return true;
        }

		/// <summary>
		/// ConsoleLogger�����д��һ���¼���Ϣ��
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
			string text = m_encoding.GetString(bytes, startIndex, count);
			return WriteEvent(eventType, text, lineFeed);
		}


		/// <summary> 
		/// ��ȡȱʡ��ConsoleLogger��������ơ�
		/// </summary>
		/// <returns>
		/// ȱʡ��ConsoleLogger��������ơ�
		/// </returns>
		public static string GetDefaultConsoleName()
        {
            return "ConsoleLogger<" + Process.GetCurrentProcess().ProcessName + ">";
        }
    }
} 