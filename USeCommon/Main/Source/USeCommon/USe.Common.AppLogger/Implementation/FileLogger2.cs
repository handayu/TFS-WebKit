using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using USe.Common.AppLogger.EventFormatter;

namespace USe.Common.AppLogger.Implementation
{
    /// <summary>
	/// 文件日志类。
	/// </summary>
    /// <remarks>
	/// 对象创建时打开文件，销毁时关闭，写入日志信息时不再打开/关闭文件。
    /// </remarks>
	public class FileLogger2 : AbstratFileLogger
    {
        private FileStream m_writer;

		/// <summary>
		/// 初始化FileLogger2类的新实例。
		/// </summary>
		/// <param name="fileName">日志文件的全路径名称。</param>
		/// <param name="encoding">日志信息编码格式。</param>
		/// <param name="formatter">事件消息格式化对象。</param>
		/// <exception cref="System.ArgumentNullException">fileName、encoding和formatter参数任一为null或empty时。</exception>
		/// <exception cref="System.Exception">创建/打开日志文件失败时。</exception>
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
		/// FileLogger2类对象的析构方法。
		/// </summary>
        ~FileLogger2()
        {
            Dispose(false);
        }
        
        /// <summary>
		/// 释放FileLogger2类对象所占用的资源。
		/// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

		/// <summary>
		/// 释放FileLogger2类对象所占用的非托管资源，也可以根据需要释放托管资源。
		/// </summary>
		/// <param name="disposing">资源释放标志，为true则释放托管资源和非托管资源；为false则仅释放非托管资源。</param>
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
		/// 清空FileLogger2类对象的日志输出缓冲区。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
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
