using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using USe.Common.AppLogger.EventFormatter;

namespace USe.Common.AppLogger.Implementation
{
    /// <summary>
	/// �ļ���־�ࡣ
	/// </summary>
    /// <remarks>
	/// ���󴴽�ʱ���ļ�������ʱ�رգ�д����־��Ϣʱ���ٴ�/�ر��ļ���
    /// </remarks>
	public class FileLogger2 : AbstratFileLogger
    {
        private FileStream m_writer;

		/// <summary>
		/// ��ʼ��FileLogger2�����ʵ����
		/// </summary>
		/// <param name="fileName">��־�ļ���ȫ·�����ơ�</param>
		/// <param name="encoding">��־��Ϣ�����ʽ��</param>
		/// <param name="formatter">�¼���Ϣ��ʽ������</param>
		/// <exception cref="System.ArgumentNullException">fileName��encoding��formatter������һΪnull��emptyʱ��</exception>
		/// <exception cref="System.Exception">����/����־�ļ�ʧ��ʱ��</exception>
		public FileLogger2(string fileName, Encoding encoding, IEventFormatter formatter)
            : base(fileName, encoding, formatter)
        {
            try
            {
                m_writer = new FileStream(m_fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            }
            catch (Exception ex)
            {
                throw new Exception("Open log file failed, " + ex.Message, ex);
            }
        }

        /// <summary>
		/// FileLogger2����������������
		/// </summary>
        ~FileLogger2()
        {
            Dispose(false);
        }
        
        /// <summary>
		/// �ͷ�FileLogger2�������ռ�õ���Դ��
		/// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

		/// <summary>
		/// �ͷ�FileLogger2�������ռ�õķ��й���Դ��Ҳ���Ը�����Ҫ�ͷ��й���Դ��
		/// </summary>
		/// <param name="disposing">��Դ�ͷű�־��Ϊtrue���ͷ��й���Դ�ͷ��й���Դ��Ϊfalse����ͷŷ��й���Դ��</param>
		protected override void Dispose(bool disposing)
        {
			if (m_writer != null)
			{
				m_writer.Dispose();
				m_writer = null;
			}

			base.Dispose(disposing);
        }


		/// <summary>
		/// ���FileLogger2��������־�����������
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public override bool Flush()
		{
			Debug.Assert(m_writer != null);

			try
			{
				m_writer.Flush();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("==>FileLogger2 flush log file[" + m_fileName + "] failed, Error: " + ex.Message);
				return false;
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
		/// <remarks>
		/// д���ļ�ʧ��ʱ���׳��쳣��������Ϣд��Debug�С�
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		protected override bool do_Write(byte[] bytes, int startIndex, int count)
		{
            Debug.Assert(m_writer != null);

            try
            {
                m_writer.Write(bytes, startIndex, count);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("==>FileLogger2 write log file[" + m_fileName + "] failed, Error: " + ex.Message);
                return false;
            }

			return true;
        }
	}
}
