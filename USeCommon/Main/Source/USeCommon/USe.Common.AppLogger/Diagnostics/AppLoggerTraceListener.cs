using System;
using System.Text;
using System.Diagnostics;

namespace USe.Common.AppLogger.Diagnostics
{
	/// <summary>
	/// AppLogger -> TraceListener Adapter
	/// </summary>
	/// <remarks>
	/// 不支持TraceOutputOptions，忽略。
	/// </remarks>
	public class AppLoggerTraceListener : TraceListener
	{
		private IAppLogger m_logger;
		private bool m_IsOwner;


		/// <summary>
		/// Constructor，使用AppLogger的Singleton实例
		/// </summary>
		public AppLoggerTraceListener()
		{
			m_logger = AppLogger.SingleInstance;
			m_IsOwner = false;
		}

		/// <summary>
		/// Constructor，按配置文件创建IAppLogger对象
		/// </summary>
		/// <param name="elementName">配置元素名称</param>
		public AppLoggerTraceListener(string elementName)
		{
			m_logger = AppLogger.CreateInstance(elementName);
			m_IsOwner = true;
		}

		/// <summary>
		/// 清理资源
		/// </summary>
		/// <param name="disposing">资源释放标志，true: 释放所有资源；false: 仅释放非受控资源</param>
		protected override void Dispose(bool disposing)
		{
			if (m_logger != null)
			{
				if (m_IsOwner == true)
				{
					m_logger.Dispose();
				}

				m_logger = null;
				m_IsOwner = false;
			}
		}


		/// <summary>
		/// 线程安全标志
		/// </summary>
		public override bool IsThreadSafe
		{
			get
			{
				return m_logger.IsThreadSafe;
			}
		}


		/// <summary>
		/// Gets the custom attributes supported by the trace listener.
		/// </summary>
		/// <returns>A string array naming the custom attributes supported by the trace listener, or null if there are no custom attributes.</returns>
		protected override string[] GetSupportedAttributes()
		{
			return null;
		}

		/// <summary>
		/// Flushes the output buffer.
		/// </summary>
		public override void Flush()
		{
			m_logger.Flush();
		}

		/// <summary>
		/// Emits an error message.
		/// </summary>
		/// <param name="message">A message to emit.</param>
		public override void Fail(string message)
		{
			m_logger.WriteError("Fail: " + message);
		}

		/// <summary>
		/// Emits an error message and a detailed error message.
		/// </summary>
		/// <param name="message">A message to emit.</param>
		/// <param name="detailMessage">A detailed message to emit.</param>
		public override void Fail(string message, string detailMessage)
		{
			m_logger.WriteError("Fail: " + message + " Detail: " + detailMessage);
		}

		/// <summary>
		/// Writes trace information, a data object and event information.
		/// </summary>
		/// <param name="eventCache">A System.Diagnostics.TraceEventCache object that contains the current process ID, thread ID, and stack trace information.</param>
		/// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
		/// <param name="eventType">One of the System.Diagnostics.TraceEventType values specifying the type of event that has caused the trace.</param>
		/// <param name="id">A numeric identifier for the event.</param>
		/// <param name="data">The trace data to emit.</param>
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			TraceEvent(eventCache, source, eventType, id, data.ToString());
		}

		/// <summary>
		/// Writes trace information, a data object and event information.
		/// </summary>
		/// <param name="eventCache">A System.Diagnostics.TraceEventCache object that contains the current process ID, thread ID, and stack trace information.</param>
		/// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
		/// <param name="eventType">One of the System.Diagnostics.TraceEventType values specifying the type of event that has caused the trace.</param>
		/// <param name="id">A numeric identifier for the event.</param>
		/// <param name="data">An array of objects to emit as data.</param>
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
			StringBuilder builder = new StringBuilder(256);
			
			foreach(object obj in data)
			{
				if (builder.Length > 0) builder.Append(", ");
				builder.Append(obj.ToString());
			}

			TraceEvent(eventCache, source, eventType, id, builder.ToString());
		}

		/// <summary>
		/// Writes trace and event information to the listener specific output.
		/// </summary>
		/// <param name="eventCache">A System.Diagnostics.TraceEventCache object that contains the current process ID, thread ID, and stack trace information.</param>
		/// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
		/// <param name="eventType">One of the System.Diagnostics.TraceEventType values specifying the type of event that has caused the trace.</param>
		/// <param name="id">A numeric identifier for the event.</param>
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
		{
			TraceEvent(eventCache, source, eventType, id, null);
		}

		/// <summary>
		/// Writes trace and event information to the listener specific output.
		/// </summary>
		/// <param name="eventCache">A System.Diagnostics.TraceEventCache object that contains the current process ID, thread ID, and stack trace information.</param>
		/// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
		/// <param name="eventType">One of the System.Diagnostics.TraceEventType values specifying the type of event that has caused the trace.</param>
		/// <param name="id">A numeric identifier for the event.</param>
		/// <param name="message">A message to write.</param>
		/// <remarks>
		/// 不支持TraceOutputOptions，忽略。
		/// </remarks>
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
		{
			LogEventType logEventType;
			string eventText;

			//switch (eventType)
			//{
			//    case TraceEventType.Critical: logEventType = LogEventType.Critical; break;
			//    case TraceEventType.Error: logEventType = LogEventType.Error; break;
			//    case TraceEventType.Warning: logEventType = LogEventType.Warning; break;
			//    case TraceEventType.Verbose: logEventType = LogEventType.Verbose; break;
			//    case TraceEventType.Information: logEventType = LogEventType.Information; break;
			//    case TraceEventType.Transfer:
			//    case TraceEventType.Start:
			//    case TraceEventType.Stop:
			//    case TraceEventType.Suspend:
			//    case TraceEventType.Resume:
			//    default:
			//        logEventType = LogEventType.Verbose;
			//        break;
			//}
			if (eventType <= TraceEventType.Verbose)
			{
				logEventType = (LogEventType)eventType;
			}
			else
			{
				logEventType = LogEventType.Verbose;
			}

			eventText = String.Format("Source: {0}, Id: {1}, Message: {2}", source, id, message);

			m_logger.WriteEvent(logEventType, eventText, true);

			//if (this.TraceOutputOptions != TraceOptions.None)
			//{
			//    // Output ......
			//}
		}

		/// <summary>
		/// Writes trace and event information to the listener specific output.
		/// </summary>
		/// <param name="eventCache">A System.Diagnostics.TraceEventCache object that contains the current process ID, thread ID, and stack trace information.</param>
		/// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
		/// <param name="eventType">One of the System.Diagnostics.TraceEventType values specifying the type of event that has caused the trace.</param>
		/// <param name="id">A numeric identifier for the event.</param>
		/// <param name="format">A format string that contains zero or more format items, which correspond to objects in the args array.</param>
		/// <param name="args">An object array containing zero or more objects to format.</param>
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
		{
			TraceEvent(eventCache, source, eventType, id, String.Format(format, args));
		}

		//public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId);

		//public override void Write(object o, string category);
		//public override void Write(string message, string category);

		/// <summary>
		/// Writes the specified message.
		/// </summary>
		/// <param name="message">A message to write.</param>
		public override void Write(string message)
		{
			m_logger.WriteVerbose(message);
		}

		//protected override void WriteIndent();

		//public override void WriteLine(object o);
		//public override void WriteLine(object o, string category);
		//public override void WriteLine(string message, string category);

		/// <summary>
		/// Writes the specified message, followed by a line terminator.
		/// </summary>
		/// <param name="message">A message to write.</param>
		public override void WriteLine(string message)
		{
			m_logger.WriteVerbose(message);
		}

	}
}
