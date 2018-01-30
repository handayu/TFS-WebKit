using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using USe.Common.AppLogger.EventFormatter;

namespace USe.Common.AppLogger.Implementation
{
    /// <summary>
    /// �ļ���־������ࡣ
    /// </summary>
	public abstract class AbstratFileLogger : IAppLoggerImpl
    {
		/// <summary>
		/// ��־�ļ����ơ�
		/// </summary>
        protected string m_fileName;
        private Encoding m_encoding;
        private IEventFormatter m_formatter;
        
        /// <summary>
		/// ��ʼ��AbstratFileLogger�����ʵ����
		/// </summary>
        /// <param name="fileName">��־�ļ���ȫ·�����ơ�</param>
		/// <param name="encoding">��־��Ϣ�����ʽ��</param>
		/// <param name="formatter">�¼���Ϣ��ʽ������</param>
		/// <exception cref="System.ArgumentNullException">fileName��encoding��formatter������һΪnull��emptyʱ��</exception>
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
		/// AbstratFileLogger����������������
		/// </summary>
		~AbstratFileLogger()
		{
			Dispose(false);
		}

		/// <summary>
		/// �ͷ�AbstratFileLogger�������ռ�õ���Դ��
		/// </summary>
		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// �ͷ�AbstratFileLogger�������ռ�õķ��й���Դ��Ҳ���Ը�����Ҫ�ͷ��й���Դ��
		/// </summary>
		/// <param name="disposing">��Դ�ͷű�־��Ϊtrue���ͷ��й���Դ�ͷ��й���Դ��Ϊfalse����ͷŷ��й���Դ��</param>
		protected virtual void Dispose(bool disposing)
		{
		}


		/// <summary>
		/// ��ȡAbstratFileLogger���������ơ�
		/// </summary>
		public string Name
        {
            get
            {
                return m_fileName;
            }
        }

		/// <summary>
		/// ��ȡAbstratFileLogger�����ı����ʽ��
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
		/// ��ȡAbstratFileLogger�������̰߳�ȫ��־��
		/// </summary>
		public bool IsThreadSafe
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// ���AbstratFileLogger��������־�����������
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public abstract bool Flush();

		/// <summary>
		/// AbstratFileLogger�����д��س����С�
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public bool LineFeed()
		{
			Debug.Assert(m_encoding != null);
			//byte[] bytes = m_encoding.GetBytes(System.Environment.NewLine);
			byte[] bytes = m_encoding.GetBytes("\r\n");
			return do_Write(bytes);
		}

		/// <summary>
		/// AbstratFileLogger�����д��һ���¼���Ϣ��
		/// </summary>
		/// <param name="eventType">��־�¼����͡�</param>
		/// <param name="eventText">��־�¼���¼�ı���</param>
		/// <param name="lineFeed">�س����б�־��</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public virtual bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
		{
			Debug.Assert(m_encoding != null);
			byte[] bytes = m_encoding.GetBytes(m_formatter.Format(eventType, eventText, lineFeed));
            return do_Write(bytes, 0, bytes.Length);
        }

		/// <summary>
		/// AbstratFileLogger�����д��һ���¼���Ϣ��
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
		/// ��־�ļ���д�������
		/// </summary>
		/// <param name="bytes">��д����ֽ����л�������</param>
		/// <param name="startIndex">��д���ֽ������ڻ������е���ʼ������</param>
		/// <param name="count">��д���ֽ����е�������</param>
		/// <returns>
		/// ��־�ļ�д��ɹ�����־��
		/// </returns>
		protected abstract bool do_Write(byte[] bytes, int startIndex, int count);

		/// <summary>
		/// ��־�ļ���д�������
		/// </summary>
		/// <param name="bytes">��д����ֽ����л�������</param>
		/// <returns>
		/// ��־�ļ�д��ɹ�����־��
		/// </returns>
		protected virtual bool do_Write(byte[] bytes)
		{
			return do_Write(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// ��ȡȱʡ����־�ļ����ơ�
		/// </summary>
		/// <param name="checkUAC">�Ƿ���UAC��־��</param>
		/// <returns>
		/// ȱʡ����־�ļ����ơ�
		/// </returns> 
		/// <remarks>
		/// ��־�ļ�������ִ�г����ȫ·������ת������������ִ�г������Ƶ���չ���滻Ϊ.log��
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

			// �жϵ�ǰ�ļ����Ƿ���չ��
			if (string.IsNullOrEmpty(fileInfo.Extension) == false)
			{
				// ȥ���ļ�����չ��
				fileName = fileName.Remove(fileName.Length - fileInfo.Extension.Length);
			}

			return fileName + ".log";
		}
	}
}
