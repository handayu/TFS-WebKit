using System;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;

namespace USe.Common.AppLogger.Decorator
{
    /// <summary>
	/// 保留字符替换类。
	/// </summary>
    public partial class ReservedCharacterDecorator : AbstractDecorator
    {
        #region member
        private ReservedCharacterManager m_reservedCharacterManager = null;
        #endregion // member

        /// <summary>
        /// 初始化ReservedCharacterDecorator类的新实例。
        /// </summary>
        /// <param name="innerImpl">内部实现类(IAppLoggerImpl)对象。</param>
        /// <exception cref="System.ArgumentNullException">innerImpl参数为null时。</exception>
        public ReservedCharacterDecorator(IAppLoggerImpl innerImpl)
            : base(innerImpl)
        {
            m_reservedCharacterManager = new ReservedCharacterManager();
            m_reservedCharacterManager.AddReservedCharacter((byte)0X0D, '\r', "<CR>");
            m_reservedCharacterManager.AddReservedCharacter((byte)0X0A, '\n', "<LF>");
            m_reservedCharacterManager.AddReservedCharacter((byte)0X1B, (char)((byte)0X1B), "<Esc>");
        }

        /// <summary>
        /// ReservedCharacterDecorator类对象写入一条事件信息。
        /// </summary>
        /// <param name="eventType">日志事件类型。</param>
        /// <param name="eventText">日志事件记录文本。</param>
        /// <param name="lineFeed">回车换行标志。</param>
        /// <returns>
        /// 日志写入成功与否标志。
        /// </returns>
        public override bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
        {
            if (string.IsNullOrEmpty(eventText))
            {
                return base.WriteEvent(eventType, eventText, lineFeed);
            }

            bool needReplace = false;
            int newLength = eventText.Length;

            for (int i = 0; i < eventText.Length; i++)
            {
                if (m_reservedCharacterManager.ContainsReplaceChar(eventText[i]))
                {
                    newLength += (m_reservedCharacterManager[eventText[i]].ReplaceStringLength - 1);
                    needReplace = true;
                }
            }

            if (needReplace == false)
            {
                return base.WriteEvent(eventType, eventText, lineFeed);
            }
            else
            {
                StringBuilder sbReplae = new StringBuilder(newLength);
                for (int i = 0; i < eventText.Length; i++)
                {
                    if (m_reservedCharacterManager.ContainsReplaceChar(eventText[i]))
                    {
                        sbReplae.Append(m_reservedCharacterManager[eventText[i]].ReplaceString);
                    }
                    else
                    {
                        sbReplae.Append(eventText[i]);
                    }
                }
                Debug.Assert(sbReplae.Length == newLength);
                return base.WriteEvent(eventType, sbReplae.ToString(), lineFeed);
            }
        }

        /// <summary>
        /// ReservedCharacterDecorator类对象写入一条事件信息。
        /// </summary>
        /// <param name="eventType">日志事件类型。</param>
        /// <param name="bytes">日志事件记录的字节序列缓冲区。</param>
        /// <param name="startIndex">日志事件记录在字节序列缓冲区里的起始索引。</param>
        /// <param name="count">日志事件记录的字节数量。</param>
        /// <param name="lineFeed">回车换行标志。</param>
        /// <returns>
        /// 日志写入成功与否标志。
        /// </returns>
        public override bool WriteEvent(LogEventType eventType, byte[] bytes, int startIndex, int count, bool lineFeed)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                return base.WriteEvent(eventType, bytes, startIndex, count, lineFeed);
            }

            bool needReplace = false;
            int newLength = bytes.Length;

            for (int i = 0; i < bytes.Length; i++)
            {
                if (m_reservedCharacterManager.ContainsReplaceByte(bytes[i]))
                {
                    newLength += (m_reservedCharacterManager[bytes[i]].RelaceByteLength - 1);
                    needReplace = true;
                }
            }

            if (needReplace == false)
            {
                return base.WriteEvent(eventType, bytes, startIndex, count, lineFeed);
            }
            else
            {
                byte[] replaceBytes = new byte[newLength];
                int index = 0;
                for (int i = 0; i < bytes.Length; i++)
                {
                    if (m_reservedCharacterManager.ContainsReplaceByte(bytes[i]))
                    {
                        ReservedCharacter reservedChar = m_reservedCharacterManager[bytes[i]];
                        if (reservedChar.RelaceByteLength > 0)
                        {
                            Array.Copy(reservedChar.ReplaceBytes, 0, replaceBytes, index, reservedChar.RelaceByteLength);
                            index += reservedChar.RelaceByteLength;
                        }
                    }
                    else
                    {
                        replaceBytes[index] = bytes[i];
                        index++;
                    }
                }
                Debug.Assert(index == newLength);
                return base.WriteEvent(eventType, replaceBytes, 0, newLength, lineFeed);
            }
        }
    }
}
