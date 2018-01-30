using System;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;

namespace USe.Common.AppLogger.Decorator
{
    /// <summary>
	/// �����ַ��滻�ࡣ
	/// </summary>
    public partial class ReservedCharacterDecorator : AbstractDecorator
    {
        #region member
        private ReservedCharacterManager m_reservedCharacterManager = null;
        #endregion // member

        /// <summary>
        /// ��ʼ��ReservedCharacterDecorator�����ʵ����
        /// </summary>
        /// <param name="innerImpl">�ڲ�ʵ����(IAppLoggerImpl)����</param>
        /// <exception cref="System.ArgumentNullException">innerImpl����Ϊnullʱ��</exception>
        public ReservedCharacterDecorator(IAppLoggerImpl innerImpl)
            : base(innerImpl)
        {
            m_reservedCharacterManager = new ReservedCharacterManager();
            m_reservedCharacterManager.AddReservedCharacter((byte)0X0D, '\r', "<CR>");
            m_reservedCharacterManager.AddReservedCharacter((byte)0X0A, '\n', "<LF>");
            m_reservedCharacterManager.AddReservedCharacter((byte)0X1B, (char)((byte)0X1B), "<Esc>");
        }

        /// <summary>
        /// ReservedCharacterDecorator�����д��һ���¼���Ϣ��
        /// </summary>
        /// <param name="eventType">��־�¼����͡�</param>
        /// <param name="eventText">��־�¼���¼�ı���</param>
        /// <param name="lineFeed">�س����б�־��</param>
        /// <returns>
        /// ��־д��ɹ�����־��
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
        /// ReservedCharacterDecorator�����д��һ���¼���Ϣ��
        /// </summary>
        /// <param name="eventType">��־�¼����͡�</param>
        /// <param name="bytes">��־�¼���¼���ֽ����л�������</param>
        /// <param name="startIndex">��־�¼���¼���ֽ����л����������ʼ������</param>
        /// <param name="count">��־�¼���¼���ֽ�������</param>
        /// <param name="lineFeed">�س����б�־��</param>
        /// <returns>
        /// ��־д��ɹ�����־��
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
