using System;
using System.Text;
using System.Diagnostics;

namespace USe.Common.AppLogger.EventFormatter
{
    /// <summary>
	/// ��־�¼���Ϣ��ʽ������
	/// </summary>
	public class EventStringFormatter : IEventFormatter
    {
        private const char LeadByte = '\x1b';
		private const string TimeFormat = " yyyyMMdd HHmmssfff ";

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
             // string text = "\x1bI 20070815 213020000 Test event string of information.";
             // string text = "\x1bE 20070815 213020000 Test event string of error.";

			// [Xu Linqiu] 2010/01/05 Ϊ��������Byte[]�����������Ҫ��ȡ������Ĵ��롣
			//// ���¼���¼�ַ���Ϊnull/Empty�Ĵ���:��eventText���������滻ΪҪ����ļ�¼�ַ���
			//if (string.IsNullOrEmpty(eventText) == true)   // eventText�Ƿ�Ϊnull/empty
			//{
			//    if (eventText == null)    // �ַ���Ϊnull
			//    {
			//        eventText = "<null string>";
			//    }
			//    else   // �ַ���ΪEmpty
			//    {
			//        eventText = "<empty string>";
			//    }
			//}
			if (string.IsNullOrEmpty(eventText))
			{
				eventText = String.Empty;
			}

            // Ԥ�ȷ���ռ䣬���� = ǰ���ַ�&�¼�����&�̶�����ʱ���ַ�������22 + ���з���󳤶�2 + ��¼����
			StringBuilder result = new StringBuilder(24 + eventText.Length);
           
            result.Append(LeadByte);

            // ���찴���ͻ��ֵ�ǰ����
			//switch (eventType)
			//{
			//    case LogEventType.Inbound:
			//        result.Append('>');
			//        break;
			//    case LogEventType.Outbound:
			//        result.Append('<');
			//        break; 
			//    default: // ��������������Ƶĵ�һ���ַ�
			//        result.Append(eventType.ToString()[0]);
			//        break;
			//}
			switch (eventType)
			{
				case LogEventType.Critical:    result.Append('C'); break;
				case LogEventType.Error:       result.Append('E'); break;
				case LogEventType.Warning:     result.Append('W'); break;
				case LogEventType.Information: result.Append('I'); break;
				case LogEventType.Verbose:     result.Append('V'); break;
				case LogEventType.Notice:      result.Append('N'); break;
				case LogEventType.Inbound:     result.Append('>'); break;
				case LogEventType.Outbound:    result.Append('<'); break;
				case LogEventType.Message:     result.Append('M'); break;
				case LogEventType.Audit:       result.Append('A'); break;
				
				default:
					Debug.Assert(false, "Unknown LogEventType[" + eventType + "].");
					result.Append(eventType.ToString()[0]);
					break;
			}

            result.Append(DateTime.Now.ToString(TimeFormat, null));
			result.Append(eventText);

            // ����Ҫ����뻻�з�
			if (lineFeed)
			{
				result.Append(System.Environment.NewLine);
			}

            return result.ToString();
        }
    }
}
