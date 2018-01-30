using System;
using System.Collections.Generic;
using System.Text;

namespace USe.Common.AppLogger.EventFormatter
{
    /// <summary>
    /// 日志保留字符格式化类。
    /// </summary>
    internal class ReservedCharacterFormatter
    {
        #region member
        private const byte CR = (byte)0X0D;
        private const byte LF = (byte)0X0A;
        private const byte FS = (byte)0X1C;
        private const byte GS = (byte)0X1D;
        private const byte RS = (byte)0X1E;
        private const byte US = (byte)0X1F;
        private const byte ESC = (byte)0X1B;

        private Encoding m_encoding = null;

        private byte[] m_CRBytes = null;
        private byte[] m_LFBytes = null;
        private byte[] m_FSBytes = null;
        private byte[] m_GSBytes = null;
        private byte[] m_RSBytes = null;
        private byte[] m_USBytes = null;
        private byte[] m_ESCBytes = null;
        #endregion // member

        #region construction
        /// <summary>
        /// 构造ReservedCharacterFormatter实例。
        /// </summary>
        /// <param name="encoding">编码方式。</param>
        public ReservedCharacterFormatter(Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding", "encoding is null.");
            }

            m_encoding = encoding;

            m_CRBytes = m_encoding.GetBytes("<CR>");
            m_LFBytes = m_encoding.GetBytes("<LF>");
            m_FSBytes = m_encoding.GetBytes("<FS>");
            m_GSBytes = m_encoding.GetBytes("<GS>");
            m_RSBytes = m_encoding.GetBytes("<RS>");
            m_USBytes = m_encoding.GetBytes("<US>");
            m_ESCBytes = m_encoding.GetBytes("<Esc>");
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 编码方式。
        /// </summary>
        public Encoding Encoding
        {
            get { return m_encoding; }
        }
        #endregion // property

        #region methods
        /// <summary>
        /// 替换保留字符。
        /// </summary>
        /// <param name="bytes">预替换字节数组。</param>
        /// <returns>替换后字节数组。</returns>
        public byte[] ReplaceReservedCharacter(byte[] bytes)
        {
            return ReplaceReservedCharacter(bytes,0,bytes.Length);
        }

        /// <summary>
        /// 替换保留字符。
        /// </summary>
        /// <param name="bytes">预替换字节数组。</param>
        /// <param name="startIndex">替换起始位置。</param>
        /// <param name="length">替换长度。</param>
        /// <returns>替换后字节数组。</returns>
        public byte[] ReplaceReservedCharacter(byte[] bytes, int startIndex, int length)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes", "bytes is null.");
            }

            if (startIndex < 0 || startIndex >= bytes.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex", "startIndex out of range.");
            }

            if (startIndex + length > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("length", "length out of range.");
            }

            List<byte> result = new List<byte>(length);
            for (int i = startIndex; i < startIndex + length; i++)
            {
                byte item = bytes[i];
                switch (item)
                {
                    case CR: result.AddRange(m_CRBytes); break;
                    case LF: result.AddRange(m_LFBytes); break;
                    case FS: result.AddRange(m_FSBytes); break;
                    case GS: result.AddRange(m_GSBytes); break;
                    case RS: result.AddRange(m_RSBytes); break;
                    case US: result.AddRange(m_USBytes); break;
                    case ESC: result.AddRange(m_ESCBytes); break;
                    default: result.Add(item); break;
                }
            }

            return result.ToArray();
        }
        #endregion // methods
    }
}
