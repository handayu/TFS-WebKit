using System;
using System.Text;
using System.Diagnostics;

namespace USe.Common.AppLogger.EventFormatter
{
    /// <summary>
	/// 日志事件消息格式化器。
	/// </summary>
	public class EventStringFormatter : IEventFormatter
    {
        private const char LeadByte = '\x1b';
		private const string TimeFormat = " yyyyMMdd HHmmssfff ";

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
             // string text = "\x1bI 20070815 213020000 Test event string of information.";
             // string text = "\x1bE 20070815 213020000 Test event string of error.";

			// [Xu Linqiu] 2010/01/05 为满足新增Byte[]数组输出的需要，取消下面的代码。
			//// 当事件记录字符串为null/Empty的处理:将eventText先做处理，替换为要输出的记录字符串
			//if (string.IsNullOrEmpty(eventText) == true)   // eventText是否为null/empty
			//{
			//    if (eventText == null)    // 字符串为null
			//    {
			//        eventText = "<null string>";
			//    }
			//    else   // 字符串为Empty
			//    {
			//        eventText = "<empty string>";
			//    }
			//}
			if (string.IsNullOrEmpty(eventText))
			{
				eventText = String.Empty;
			}

            // 预先分配空间，长度 = 前导字符&事件类型&固定日期时间字符串长度22 + 换行符最大长度2 + 记录长度
			StringBuilder result = new StringBuilder(24 + eventText.Length);
           
            result.Append(LeadByte);

            // 构造按类型划分的前导符
			//switch (eventType)
			//{
			//    case LogEventType.Inbound:
			//        result.Append('>');
			//        break;
			//    case LogEventType.Outbound:
			//        result.Append('<');
			//        break; 
			//    default: // 其他情况下用名称的第一个字符
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

            // 按照要求加入换行符
			if (lineFeed)
			{
				result.Append(System.Environment.NewLine);
			}

            return result.ToString();
        }
    }
}
