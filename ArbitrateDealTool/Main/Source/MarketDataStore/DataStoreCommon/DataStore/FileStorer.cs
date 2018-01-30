using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace DataStoreCommon
{
    public class FileStorer
    {
        private string m_fileName = string.Empty;
        private FileStream m_writer;
        private Encoding m_encoding = Encoding.UTF8;

        public FileStorer(string fileName)
        {
            FileInfo info = new FileInfo(fileName);
            if (!Directory.Exists(info.DirectoryName))
            {
                Directory.CreateDirectory(info.DirectoryName);
            }

            m_fileName = info.FullName;

            //try
            //{
            //    m_writer = new FileStream(m_fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Open log file failed, " + ex.Message, ex);
            //}
        }

        public void Write(string text)
        {
            FileStream writer = null;

            text = string.Format("{0:yyyyMMdd HHmmssfff} {1}\r\n",DateTime.Now, text);
            try
            {
                writer = new FileStream(m_fileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                byte[] bytes = m_encoding.GetBytes(text);

                writer.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("==>write file[" + m_fileName + "] failed, Error: " + ex.Message);
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
    }
}
