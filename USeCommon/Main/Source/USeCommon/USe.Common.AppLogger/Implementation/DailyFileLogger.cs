using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using USe.Common.AppLogger.EventFormatter;

namespace USe.Common.AppLogger.Implementation
{
    /// <summary>
	/// Daily文件日志类。
    /// </summary>
    /// <remarks>
	/// 每次写入日志信息时重复打开/关闭文件。
	/// </remarks>
	public class DailyFileLogger : AbstractDailyFileLogger
    {
		/// <summary>
		/// 初始化DailyFileLogger类的新实例。
		/// </summary>
		/// <param name="fileName">日志文件的全路径名称。</param>
		/// <param name="encoding">日志信息编码格式。</param>
		/// <param name="formatter">事件消息格式化对象。</param>
		/// <exception cref="System.ArgumentNullException">fileName、encoding和formatter参数任一为null或empty时。</exception>
		/// <exception cref="System.Exception">创建/打开日志文件失败时。</exception>
		public DailyFileLogger(string fileName, Encoding encoding, IEventFormatter formatter)
            : base(fileName, encoding, formatter)
        {
            FileStream writer = null;

            try
            {
                // 预先测试文件能否正常打开
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
		/// 清空DailyFileLogger类对象的日志输出缓冲区。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public override bool Flush()
		{
			return true;
		}

		/// <summary>
		/// 日志文件的写入操作。
		/// </summary>
		/// <param name="bytes">待写入的字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志文件写入成功与否标志。
		/// </returns>
		/// <remarks>
		/// 写入文件失败时不抛出异常，错误信息写入Debug中。
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
