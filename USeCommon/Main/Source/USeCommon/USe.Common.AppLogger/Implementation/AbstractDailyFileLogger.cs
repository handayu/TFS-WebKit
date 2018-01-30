using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using USe.Common.AppLogger.EventFormatter;

namespace USe.Common.AppLogger.Implementation
{
    /// <summary>
    /// DailyFileLogger各个类的抽象基类。
    /// </summary>
    /// <remarks>
    /// </remarks>
	public abstract class AbstractDailyFileLogger : AbstratFileLogger
    {
        private const string DateFormat = "_yyyyMMdd";    // 日期格式化字符串

        private DateTime m_currentDate;
		/// <summary>
		/// 按日期生成的日志文件名称。
		/// </summary>
        protected string m_dailyFileName;

		/// <summary>
		/// 初始化AbstractDailyFileLogger类的新实例。
		/// </summary>
		/// <param name="fileName">日志文件的全路径名称。</param>
		/// <param name="encoding">日志信息编码格式。</param>
		/// <param name="formatter">事件消息格式化对象。</param>
		/// <exception cref="System.ArgumentNullException">fileName、encoding和formatter参数任一为null或empty时。</exception>
		/// <exception cref="System.Exception">创建/打开日志文件失败。</exception>
		public AbstractDailyFileLogger(string fileName, Encoding encoding, IEventFormatter formatter) 
            :base(fileName, encoding, formatter)
        {
			ResetDailyFileName(); // 成员变量初始化
			CheckDailyFileName(); // 检查并更新文件名
        }


		/// <summary>
		/// 设置文件名称和当前日期为初始值。
		/// </summary>
		protected void ResetDailyFileName()
		{
			m_dailyFileName = null;
			m_currentDate = DateTime.MinValue.Date;
		}

        /// <summary>
        /// 校验日志文件名称是否包含正确的日期信息。
        /// </summary>
        /// <returns>表示文件名是否被更新. true:已更新; false:无需更新。</returns>
        protected bool CheckDailyFileName()
        {
            // 已经是最新日期则无须校验文件名
            if (m_currentDate == DateTime.Now.Date)
            {
                return false;
            }

            FileInfo fileInfo = new FileInfo(m_fileName);
			int lastIndex = fileInfo.FullName.Length - fileInfo.Extension.Length;
			// 计算日期字符串的插入位置

            m_dailyFileName = fileInfo.FullName.Insert(lastIndex, DateTime.Now.ToString(DateFormat, null));
            m_currentDate = DateTime.Now.Date;

			return true;
        }
    }
}
