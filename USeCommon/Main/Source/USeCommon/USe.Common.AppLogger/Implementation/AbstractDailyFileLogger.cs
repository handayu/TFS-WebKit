using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using USe.Common.AppLogger.EventFormatter;

namespace USe.Common.AppLogger.Implementation
{
    /// <summary>
    /// DailyFileLogger������ĳ�����ࡣ
    /// </summary>
    /// <remarks>
    /// </remarks>
	public abstract class AbstractDailyFileLogger : AbstratFileLogger
    {
        private const string DateFormat = "_yyyyMMdd";    // ���ڸ�ʽ���ַ���

        private DateTime m_currentDate;
		/// <summary>
		/// ���������ɵ���־�ļ����ơ�
		/// </summary>
        protected string m_dailyFileName;

		/// <summary>
		/// ��ʼ��AbstractDailyFileLogger�����ʵ����
		/// </summary>
		/// <param name="fileName">��־�ļ���ȫ·�����ơ�</param>
		/// <param name="encoding">��־��Ϣ�����ʽ��</param>
		/// <param name="formatter">�¼���Ϣ��ʽ������</param>
		/// <exception cref="System.ArgumentNullException">fileName��encoding��formatter������һΪnull��emptyʱ��</exception>
		/// <exception cref="System.Exception">����/����־�ļ�ʧ�ܡ�</exception>
		public AbstractDailyFileLogger(string fileName, Encoding encoding, IEventFormatter formatter) 
            :base(fileName, encoding, formatter)
        {
			ResetDailyFileName(); // ��Ա������ʼ��
			CheckDailyFileName(); // ��鲢�����ļ���
        }


		/// <summary>
		/// �����ļ����ƺ͵�ǰ����Ϊ��ʼֵ��
		/// </summary>
		protected void ResetDailyFileName()
		{
			m_dailyFileName = null;
			m_currentDate = DateTime.MinValue.Date;
		}

        /// <summary>
        /// У����־�ļ������Ƿ������ȷ��������Ϣ��
        /// </summary>
        /// <returns>��ʾ�ļ����Ƿ񱻸���. true:�Ѹ���; false:������¡�</returns>
        protected bool CheckDailyFileName()
        {
            // �Ѿ�����������������У���ļ���
            if (m_currentDate == DateTime.Now.Date)
            {
                return false;
            }

            FileInfo fileInfo = new FileInfo(m_fileName);
			int lastIndex = fileInfo.FullName.Length - fileInfo.Extension.Length;
			// ���������ַ����Ĳ���λ��

            m_dailyFileName = fileInfo.FullName.Insert(lastIndex, DateTime.Now.ToString(DateFormat, null));
            m_currentDate = DateTime.Now.Date;

			return true;
        }
    }
}
