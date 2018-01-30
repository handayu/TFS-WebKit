using System;
using System.Text;
using System.Diagnostics;

namespace USe.Common.AppLogger.EventFormatter
{
    /// <summary>
	/// �ɶ��͵���־�¼���Ϣ��ʽ������
	/// </summary>
	public class FriendlyEventStringFormatter : IEventFormatter
    {
		private static string DefaultLeadBytes = "==>";
		private static string DefaultTimeFormat = "[yyyy/MM/dd HH:mm:ss.fff] ";

		private string m_leadBytes = DefaultLeadBytes;
		private string m_timeFormat = DefaultTimeFormat;

		/// <summary>
		/// ��ʼ��FriendlyEventStringFormatter�����ʵ����
		/// </summary>
		public FriendlyEventStringFormatter()
		{
		}

		/// <summary>
		/// ��ʼ��FriendlyEventStringFormatter�����ʵ����
		/// </summary>
		/// <param name="leadBytes">����ʼ�ַ�����</param>
		/// <param name="timeFormat">ʱ���ַ�����ʽ��</param>
		public FriendlyEventStringFormatter(string leadBytes, string timeFormat)
		{
			if (!String.IsNullOrEmpty(leadBytes))
			{
				m_leadBytes = leadBytes;
			}
			if (!String.IsNullOrEmpty(timeFormat))
			{
				m_timeFormat = timeFormat;
			}
		}

		/// <summary>
		/// ʹ��ָ������־�¼����͸�ʽ�������¼���Ϣ��¼��
		/// </summary>
		/// <param name="eventType">��־�¼����͡�</param>
		/// <param name="eventText">��־�¼���¼�ı���</param>
		/// <param name="lineFeed">�س����б�־��</param>
		/// <returns>
		/// ��ʽ���������¼���Ϣ��¼�ַ�����
		/// </returns>
		public string Format(LogEventType eventType, string eventText, bool lineFeed)
        {
            // Ŀ���ʽʾ��
            // string text = "==>Information[2007/08/15 21:30:20.000] Test event string of information.";
            // string text = "==>Error[2007/08/15 21:30:20.000] Test event string of information.";

            // ����¼�ַ���Ϊnull/Empty�Ĵ���:��eventText���������滻ΪҪ������¼���¼
            if (string.IsNullOrEmpty(eventText) == true)   // eventText�Ƿ�Ϊnull/empty
            {
                if (eventText == null)    // �ַ���Ϊnull
                {
                    eventText = "<null string>";
                }
                else   // �ַ���ΪEmpty
                {
                    eventText = "<empty string>";
                }
            }

			// Ԥ�ȷ���ռ䣬���� = ǰ���ַ�&�¼�����&�̶�����ʱ���ַ����������50 + ���з���󳤶�2 + ��¼����
            StringBuilder result = new StringBuilder(52 + 12 + eventText.Length);

            result.Append(m_leadBytes);

            //result.Append(eventType.ToString());
			switch (eventType)
			{
				case LogEventType.Critical: result.Append("CRIT"); break;
				case LogEventType.Error: result.Append("ERRO"); break;
				case LogEventType.Warning: result.Append("WARN"); break;
				case LogEventType.Information: result.Append("INFO"); break;
				case LogEventType.Verbose: result.Append("VERB"); break;
				case LogEventType.Notice: result.Append("NOTI"); break;
				case LogEventType.Inbound: result.Append("INPU"); break;
				case LogEventType.Outbound: result.Append("OUTP"); break;
				case LogEventType.Message: result.Append("MESS"); break;
				case LogEventType.Audit: result.Append("AUDI"); break;

				default:
					Debug.Assert(false, "Unknown LogEventType[" + eventType + "].");
					result.Append(eventType.ToString().Substring(0, 4));
					break;
			}

            result.Append(DateTime.Now.ToString(m_timeFormat, null));

            result.Append(eventText);

            // ����Ҫ����뻻�з�
            if (lineFeed)
            {
                result.Append(System.Environment.NewLine);
            }

			Debug.Assert(result.Length <= 52 + 12 + eventText.Length);

            return result.ToString();
        }
    }
}
