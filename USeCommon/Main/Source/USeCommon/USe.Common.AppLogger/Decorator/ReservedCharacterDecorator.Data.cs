using System;
using System.Collections.Generic;
using System.Text;

namespace USe.Common.AppLogger.Decorator
{
    public partial class ReservedCharacterDecorator
    {
        /// <summary>
        /// 保留字符替换信息类。
        /// </summary>
        private class ReservedCharacter
        {
            #region construciton
            /// <summary>
            /// 构造保留字符替换实例。
            /// </summary>
            /// <param name="sourceByte">源字节。</param>
            /// <param name="sourceChar">源字符。</param>
            /// <param name="replaceString">替换字符串。</param>
            /// <remarks>
            /// 替换字节数组为替换字符串ASCII编码。
            /// </remarks>
            public ReservedCharacter(byte sourceByte, char sourceChar, string replaceString)
            {
                this.SourceByte = sourceByte;
                this.SourceChar = sourceChar;
                this.ReplaceString = replaceString;
                this.ReplaceBytes = Encoding.ASCII.GetBytes(replaceString); // 字节替换默认用ASCII编码替换
            }
            #endregion // construction

            #region property
            /// <summary>
            /// 源字节。
            /// </summary>
            public byte SourceByte
            {
                get;
                private set;
            }

            /// <summary>
            /// 源字符。
            /// </summary>
            public char SourceChar
            {
                get;
                private set;
            }

            /// <summary>
            /// 替换字符串。
            /// </summary>
            public string ReplaceString
            {
                get;
                private set;
            }

            /// <summary>
            /// 替换字符串长度。
            /// </summary>
            public int ReplaceStringLength
            {
                get
                {
                    if (string.IsNullOrEmpty(this.ReplaceString))
                    {
                        return 0;
                    }
                    else
                    {
                        return this.ReplaceString.Length;
                    }
                }
            }

            /// <summary>
            /// 替换字节数组。
            /// </summary>
            public byte[] ReplaceBytes
            {
                get;
                private set;
            }

            /// <summary>
            /// 替换字节数组长度。
            /// </summary>
            public int RelaceByteLength
            {
                get
                {
                    if (this.ReplaceBytes == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return this.ReplaceBytes.Length;
                    }
                }
            }
            #endregion // property
        }

        /// <summary>
        /// 保留字符替换管理类。
        /// </summary>
        private class ReservedCharacterManager
        {
            #region member
            private Dictionary<byte, int> m_byteIndex = null;
            private Dictionary<char, int> m_charIndex = null;
            private List<ReservedCharacter> m_reservedCharacterList = null;
            #endregion // member

            #region construction
            /// <summary>
            /// 构造保留字符替换管理类实例。
            /// </summary>
            public ReservedCharacterManager()
            {
                m_byteIndex = new Dictionary<byte, int>();
                m_charIndex = new Dictionary<char, int>();
                m_reservedCharacterList = new List<ReservedCharacter>();
            }
            #endregion // construciton

            #region property
            /// <summary>
            /// 获取保留字符替换对象。
            /// </summary>
            /// <param name="value">保留字符。</param>
            /// <returns></returns>
            public ReservedCharacter this[char value]
            {
                get
                {
                    if (m_charIndex.ContainsKey(value))
                    {
                        return m_reservedCharacterList[m_charIndex[value]];
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// 获取保留字符替换对象。
            /// </summary>
            /// <param name="value">保留字节。</param>
            /// <returns></returns>
            public ReservedCharacter this[byte value]
            {
                get
                {
                    if (m_byteIndex.ContainsKey(value))
                    {
                        return m_reservedCharacterList[m_byteIndex[value]];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            #endregion // property

            #region methods
            /// <summary>
            /// 增加保留字符替换。
            /// </summary>
            /// <param name="sourceByte">源字节。</param>
            /// <param name="sourceChar">源字符。</param>
            /// <param name="replaceString">替换字符串。</param>
            public void AddReservedCharacter(byte sourceByte, char sourceChar, string replaceString)
            {
                if (m_byteIndex.ContainsKey(sourceByte))
                {
                    throw new Exception(string.Format("ReservedCharacter {0} has been exist", sourceByte));
                }
                if (m_charIndex.ContainsKey(sourceChar))
                {
                    throw new Exception(string.Format("ReservedCharacter {0} has been exist", sourceChar));
                }

                m_reservedCharacterList.Add(new ReservedCharacter(sourceByte, sourceChar, replaceString));
                m_byteIndex.Add(sourceByte, m_reservedCharacterList.Count - 1);
                m_charIndex.Add(sourceChar, m_reservedCharacterList.Count - 1);
            }

            /// <summary>
            /// 移除保留字符替换。
            /// </summary>
            /// <param name="value">保留字节。</param>
            public void RemoveReservedCharacter(byte value)
            {
                if (m_byteIndex.ContainsKey(value))
                {
                    int index = m_byteIndex[value];
                    char charValue = m_reservedCharacterList[index].SourceChar;

                    m_byteIndex.Remove(value);
                    m_charIndex.Remove(charValue);
                    m_reservedCharacterList[index] = null;

                }
            }

            /// <summary>
            /// 移除保留字符替换。
            /// </summary>
            /// <param name="value">保留字符。</param>
            public void RemoveReservedCharacter(char value)
            {
                if (m_charIndex.ContainsKey(value))
                {
                    int index = m_charIndex[value];
                    byte byteValue = m_reservedCharacterList[index].SourceByte;

                    m_charIndex.Remove(value);
                    m_byteIndex.Remove(byteValue);

                    m_reservedCharacterList[index] = null;
                }
            }

            /// <summary>
            /// 判定字符是否为保留字符。
            /// </summary>
            /// <param name="value">字符。</param>
            /// <returns></returns>
            public bool ContainsReplaceChar(char value)
            {
                return m_charIndex.ContainsKey(value);
            }

            /// <summary>
            /// 判断字节是否为保留字节。
            /// </summary>
            /// <param name="value">字节。</param>
            /// <returns></returns>
            public bool ContainsReplaceByte(byte value)
            {
                return m_byteIndex.ContainsKey(value);
            }
            #endregion // methods
        }
    }
}
