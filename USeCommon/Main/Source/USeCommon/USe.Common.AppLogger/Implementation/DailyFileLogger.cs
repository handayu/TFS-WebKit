using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using USe.Common.AppLogger.EventFormatter;

namespace USe.Common.AppLogger.Implementation
{
    /// <summary>
	/// Daily�ļ���־�ࡣ
    /// </summary>
    /// <remarks>
	/// ÿ��д����־��Ϣʱ�ظ���/�ر��ļ���
	/// </remarks>
	public class DailyFileLogger : AbstractDailyFileLogger
    {
		/// <summary>
		/// ��ʼ��DailyFileLogger�����ʵ����
		/// </summary>
		/// <param name="fileName">��־�ļ���ȫ·�����ơ�</param>
		/// <param name="encoding">��־��Ϣ�����ʽ��</param>
		/// <param name="formatter">�¼���Ϣ��ʽ������</param>
		/// <exception cref="System.ArgumentNullException">fileName��encoding��formatter������һΪnull��emptyʱ��</exception>
		/// <exception cref="System.Exception">����/����־�ļ�ʧ��ʱ��</exception>
		public DailyFileLogger(string fileName, Encoding encoding, IEventFormatter formatter)
            : base(fileName, encoding, formatter)
        {
            FileStream writer = null;

            try
            {
                // Ԥ�Ȳ����ļ��ܷ�������
                writer = new FileStream(m_dailyFileName, FileMode.Append, FileAccess.Write, FileShare.Read);
            }
            catch (Exception ex)
            {
                throw new Exception("Open log file failed, " + ex.Message, ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                    writer = null;
                }
            }
        }


		/// <summary>
		/// ���DailyFileLogger��������־�����������
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		public override bool Flush()
		{
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
            FileStream writer = null;

            try
            {
                CheckDailyFileName();
                writer = new FileStream(m_dailyFileName, FileMode.Append, FileAccess.Write, FileShare.Read);

                writer.Write(bytes, startIndex, count);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("==>DailyFileLogger write log file[" + m_dailyFileName + "] failed, Error: " + ex.Message);
                return false;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                    writer = null;
                }
            }

			return true;
		}
	}
}
