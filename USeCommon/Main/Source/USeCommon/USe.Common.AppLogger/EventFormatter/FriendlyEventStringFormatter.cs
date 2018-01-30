using System;
using System.Text;
using System.Diagnostics;

namespace USe.Common.AppLogger.EventFormatter
{
    /// <summary>
	/// 可读型的日志事件消息格式化器。
	/// </summary>
	public class FriendlyEventStringFormatter : IEventFormatter
    {
		private static string DefaultLeadBytes = "==>";
		private static string DefaultTimeFormat = "[yyyy/MM/dd HH:mm:ss.fff] ";

		private string m_leadBytes = DefaultLeadBytes;
		private string m_timeFormat = DefaultTimeFormat;

		/// <summary>
		/// 初始化FriendlyEventStringFormatter类的新实例。
		/// </summary>
		public FriendlyEventStringFormatter()
		{
		}

		/// <summary>
		/// 初始化FriendlyEventStringFormatter类的新实例。
		/// </summary>
		/// <param name="leadBytes">行起始字符串。</param>
		/// <param name="timeFormat">时间字符串格式。</param>
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
		/// 使用指定的日志事件类型格式化产生事件消息记录。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="eventText">日志事件记录文本。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 格式化产生的事件消息记录字符串。
		/// </returns>
		public string Format(LogEventType eventType, string eventText, bool lineFeed)
        {
            // 目标格式示例
            // string text = "==>Information[2007/08/15 21:30:20.000] Test event string of information.";
            // string text = "==>Error[2007/08/15 21:30:20.000] Test event string of information.";

            // 当记录字符串为null/Empty的处理:将eventText先做处理，替换为要输出的事件记录
            if (string.IsNullOrEmpty(eventText) == true)   // eventText是否为null/empty
            {
                if (eventText == null)    // 字符串为null
                {
                    eventText = "<null string>";
                }
                else   // 字符串为Empty
                {
                    eventText = "<empty string>";
                }
            }

			// 预先分配空间，长度 = 前导字符&事件类型&固定日期时间字符串长度最大50 + 换行符最大长度2 + 记录长度
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

            // 按照要求加入换行符
            if (lineFeed)
            {
                result.Append(System.Environment.NewLine);
            }

			Debug.Assert(result.Length <= 52 + 12 + eventText.Length);

            return result.ToString();
        }
    }
}
