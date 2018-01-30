using System;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;

using USe.Common.AppLogger.Configuration;

namespace USe.Common.AppLogger
{
	/// <summary>
	/// 应用程序日志类。
	/// </summary>
	public class AppLogger : IAppLogger
	{
		private IAppLoggerImpl m_innerImpl;				// 内部实现类对象
		static private IAppLogger ms_singleInstance;	// Sigletone模式的唯一实例

		/// <summary>
		/// 初始化AppLogger类的新实例。
		/// </summary>
		/// <param name="innerImpl">内部实现类对象。</param>
		/// <exception cref="System.ArgumentNullException">innerImpl参数为null时。</exception>
		private AppLogger(IAppLoggerImpl innerImpl)
		{
			if (innerImpl == null)
			{
				throw new ArgumentNullException("Invalid IAppLoggerImpl object.");
			}

			m_innerImpl = innerImpl;
		}

		/// <summary>
		/// AppLogger类对象的析构方法。
		/// </summary>
		~AppLogger()
		{
			Dispose(false);
		}

		/// <summary>
		/// 释放AppLogger类对象所占用的资源。
		/// </summary>
		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 释放AppLogger类对象所占用的非托管资源，也可以根据需要释放托管资源。
		/// </summary>
		/// <param name="disposing">资源释放标志，为true则释放托管资源和非托管资源；为false则仅释放非托管资源。</param>
		protected virtual void Dispose(bool disposing)
		{
			if (m_innerImpl != null)
			{
				m_innerImpl.Dispose();
				m_innerImpl = null;
			}
		}


		/// <summary>
		/// 获取AppLogger类的Singleton实例对象。
		/// </summary>
		static public IAppLogger SingleInstance
		{
			get
			{
				Debug.Assert(ms_singleInstance != null);
				return ms_singleInstance;
			}

			set
			{
				Debug.Assert(value != null);
				ms_singleInstance = value;
			}
		}


		/// <summary>
		/// 获取AppLogger类对象的名称。
		/// </summary>
		public string Name
		{
			get
			{
				Debug.Assert(m_innerImpl != null);
				return m_innerImpl.Name;
			}
		}

		/// <summary>
		/// 获取AppLogger类对象的编码格式。
		/// </summary>
		public Encoding Encoding
		{
			get
			{
				Debug.Assert(m_innerImpl != null);
				return m_innerImpl.Encoding;
			}

			set
			{
				Debug.Assert(m_innerImpl != null);
				m_innerImpl.Encoding = value;
			}
		}

		/// <summary>
		/// 获取AppLogger类对象的线程安全标志。
		/// </summary>
		public bool IsThreadSafe
		{
			get
			{
				Debug.Assert(m_innerImpl != null);
				return m_innerImpl.IsThreadSafe;
			}
		}

		/// <summary>
		/// 清空AppLogger类对象的日志输出缓冲区。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool Flush()
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.Flush();
		}

		/// <summary>
		/// AppLogger类对象写入回车换行。
		/// </summary>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool LineFeed()
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.LineFeed();
		}

		/// <summary>
		/// AppLogger类对象写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="eventText">日志事件记录文本。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(eventType, eventText, lineFeed);
		}

		/// <summary>
		/// AppLogger类对象写入一条事件信息。
		/// </summary>
		/// <param name="eventType">日志事件类型。</param>
		/// <param name="bytes">日志事件记录的字节序列缓冲区。</param>
		/// <param name="startIndex">日志事件记录在字节序列缓冲区里的起始索引。</param>
		/// <param name="count">日志事件记录的字节数量。</param>
		/// <param name="lineFeed">回车换行标志。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteEvent(LogEventType eventType, byte[] bytes, int startIndex, int count, bool lineFeed)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(eventType, bytes, startIndex, count, lineFeed);
		}

		/// <summary>
		/// AppLogger类对象写入一条Critical类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteCritical(string text)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Critical, text, true);
		}

		/// <summary>
		/// AppLogger类对象写入一条Error类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteError(string text)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Error, text, true);
		}

		/// <summary>
		/// AppLogger类对象写入一条Warning类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteWarning(string text)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Warning, text, true);
		}

		/// <summary>
		/// AppLogger类对象写入一条Information类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteInformation(string text)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Information, text, true);
		}

		/// <summary>
		/// AppLogger类对象写入一条Verbose类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteVerbose(string text)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Verbose, text, true);
		}

		/// <summary>
		/// AppLogger类对象写入一条Notice类型事件信息。
		/// </summary>
		/// <param name="text">日志事件记录文本。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteNotice(string text)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Notice, text, true);
		}

		/// <summary>
		///  AppLogger类对象写入一条Inbound类型事件信息。
		/// </summary>
		/// <param name="message">入站消息字符串。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteInbound(string message)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Inbound, message, true);
		}

		/// <summary>
		/// AppLogger类对象写入一条Inbound类型事件信息。
		/// </summary>
		/// <param name="bytes">待写入的日志信息字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteInbound(byte[] bytes, int startIndex, int count)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Inbound, bytes, startIndex, count, true);
		}

		/// <summary>
		///  AppLogger类对象写入一条Outbound类型事件信息。
		/// </summary>
		/// <param name="message">出站消息字符串。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteOutbound(string message)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Outbound, message, true);
		}

		/// <summary>
		/// AppLogger类对象写入一条Outbound类型事件信息。
		/// </summary>
		/// <param name="bytes">待写入的日志信息字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteOutbound(byte[] bytes, int startIndex, int count)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Outbound, bytes, startIndex, count, true);
		}

		/// <summary>
		/// AppLogger类对象写入一条Message类型事件信息。
		/// </summary>
		/// <param name="message">消息字符串。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteMessage(string message)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Message, message, true);
		}

		/// <summary>
		/// AppLogger类对象写入一条Message类型事件信息。
		/// </summary>
		/// <param name="bytes">待写入的日志信息字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteMessage(byte[] bytes, int startIndex, int count)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Message, bytes, startIndex, count, true);
		}

		/// <summary>
		/// 应用程序日志写入一条Audit类型事件信息。
		/// </summary>
		/// <param name="message">消息字符串。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteAudit(string message)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Audit, message, true);
		}

		/// <summary>
		/// 应用程序日志写入一条Audit类型事件信息。
		/// </summary>
		/// <param name="bytes">待写入的日志信息字节序列缓冲区。</param>
		/// <param name="startIndex">待写入字节序列在缓冲区中的起始索引。</param>
		/// <param name="count">待写入字节序列的数量。</param>
		/// <returns>
		/// 日志写入成功与否标志。
		/// </returns>
		public bool WriteAudit(byte[] bytes, int startIndex, int count)
		{
			Debug.Assert(m_innerImpl != null);
			return m_innerImpl.WriteEvent(LogEventType.Audit, bytes, startIndex, count, true);
		}


		#region IAppLogger日志对象创建方法
		/// <summary>
		/// 按指定的配置元素名称创建应用程序日志对象。
		/// </summary>
		/// <param name="elementName">配置元素名称。</param>
		/// <returns>日志对象。</returns>
		/// <remarks>
		/// 1. 配置元素：USe.Common.AppLogger/appLoggers/appLogger。<br/>
		/// 2. 辅助的IAppLogger对象创建方法，与Singleton模式的静态实例变量无关。
		/// </remarks>
		public static IAppLogger CreateInstance(string elementName)
		{
            /*
            AppLoggerElement element = AppLoggerSectionGroup.FindAppLoggerElement(elementName);
			if (element == null)
			{
				throw new ArgumentException("Not found the special appLogger configuration element.");
			}
            */

            AppLoggersSection section = AppLoggersSection.GetSection();
            if (section == null)
            {
                throw new ConfigurationErrorsException("Not found the appLogger configuration section.");
            }

            return CreateInstance(section.AppLoggers[elementName]);
		}

		/// <summary>
		/// 使用指定的配置元素对象创建应用程序日志对象。
		/// </summary>
		/// <param name="config">配置元素对象。</param>
		/// <returns>
		/// 应用程序日志对象。
		/// </returns>
		/// <remarks>
		/// 辅助的IAppLogger对象创建方法，与Singleton模式的静态实例变量无关。
		/// </remarks>
		public static IAppLogger CreateInstance(AppLoggerElement config)
		{
			if (config == null)
			{
				throw new ArgumentException("Not found the special appLogger configuration element.");
			}

			if (config.LoggerType == "NullLogger")
			{
				// NullLogger对象, 忽略其余配置
				return new NullLogger(config.Name);
			}
			else if (config.LoggerType == "AppLogger")
			{
				// AppLogger对象, 按具体配置创建
				IAppLoggerImpl innerImpl = null;

				innerImpl = CreateImplementationObject(config.Implementation);
				Debug.Assert(innerImpl != null);
				// 创建日志实现类对象（最里层）

				for (int i = config.Decorators.Count - 1; i >= 0; i--)
				{
					innerImpl = CreateDecoratorObject(config.Decorators[i], innerImpl);
					Debug.Assert(innerImpl != null);
				}
				// 按配置顺序的逆序生成日志装饰类对象

				return new AppLogger(innerImpl);
			}

			throw new ArgumentException("Invalid [appLogger] configuration element.");
		}

		/// <summary>
		/// 初始化IAppLogger应用程序日志对象。
		/// </summary>
		/// <returns>日志对象。</returns>
		/// <remarks>
		/// Singleton模式的静态实例变量的创建方法。
		/// </remarks>
		public static IAppLogger InitInstance()
		{
			// 检查是否重复初始化
			if (ms_singleInstance != null)
			{
				Debug.Assert(false);
				throw new ApplicationException("IAppLogger object already initialized.");
			}

            AppLoggersSection section = AppLoggersSection.GetSection();
            if (section == null)
            {
                throw new ConfigurationErrorsException("Not found the appLogger configuration section.");
            }

          	ms_singleInstance = CreateInstance(section.AppLoggers["Default"]);
		    //ms_singleInstance = CreateInstance("Default");
			// 缺省使用名称为"Default"的配置元素，区分大小写

			return ms_singleInstance;
		}

		/// <summary>
		/// 释放静态的Singleton实例。
		/// </summary>
		/// <remarks>
		/// 与InitInstance对应。
		/// </remarks>
		public static void ClearInstance()
		{
			if (ms_singleInstance != null)
			{
				ms_singleInstance.Dispose();
				ms_singleInstance = null;
			}
		}


		/// <summary>
		/// 格式化文件名称，相对路径转换为绝对路径。
		/// </summary>
		/// <param name="fileName">文件名称。</param>
		/// <returns>格式化后的文件名称。</returns>
        private static string FormatFileName(string fileName)
        {
            fileName = fileName.Replace("$(Desktop)", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            fileName = fileName.Replace("$(Programs)", Environment.GetFolderPath(Environment.SpecialFolder.Programs));
            fileName = fileName.Replace("$(Personal)", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            fileName = fileName.Replace("$(MyDocuments)", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            fileName = fileName.Replace("$(Favorites)", Environment.GetFolderPath(Environment.SpecialFolder.Favorites));
            fileName = fileName.Replace("$(Startup)", Environment.GetFolderPath(Environment.SpecialFolder.Startup));
            fileName = fileName.Replace("$(Recent)", Environment.GetFolderPath(Environment.SpecialFolder.Recent));
            fileName = fileName.Replace("$(SendTo)", Environment.GetFolderPath(Environment.SpecialFolder.SendTo));
            fileName = fileName.Replace("$(StartMenu)", Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));
            fileName = fileName.Replace("$(MyMusic)", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            fileName = fileName.Replace("$(DesktopDirectory)", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            fileName = fileName.Replace("$(MyComputer)", Environment.GetFolderPath(Environment.SpecialFolder.MyComputer));
            fileName = fileName.Replace("$(Templates)", Environment.GetFolderPath(Environment.SpecialFolder.Templates));
            fileName = fileName.Replace("$(ApplicationData)", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            fileName = fileName.Replace("$(LocalApplicationData)", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            fileName = fileName.Replace("$(InternetCache)", Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
            fileName = fileName.Replace("$(Cookies)", Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
            fileName = fileName.Replace("$(History)", Environment.GetFolderPath(Environment.SpecialFolder.History));
            fileName = fileName.Replace("$(CommonApplicationData)", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            fileName = fileName.Replace("$(System)", Environment.GetFolderPath(Environment.SpecialFolder.System));
            fileName = fileName.Replace("$(ProgramFiles)", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            fileName = fileName.Replace("$(MyPictures)", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            fileName = fileName.Replace("$(CommonProgramFiles)", Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles));

            System.IO.FileInfo info = new System.IO.FileInfo(fileName);
            return info.FullName;
        }

		/// <summary>
		/// 按配置元素创建IAppLoggerImpl日志实现类对象。
		/// </summary>
		/// <param name="element">配置元素。</param>
		/// <returns>日志实现类对象。</returns>
		/// <remarks>
		/// 配置元素：USe.Common.AppLogger/appLoggers/appLogger/implementation。
		/// </remarks>
		private static IAppLoggerImpl CreateImplementationObject(ImplementationElement element)
		{
            USe.Common.AppLogger.EventFormatter.IEventFormatter formatter = null;
			IAppLoggerImpl logger = null;

			switch (element.EventFormatter)
			{
				case "EventStringFormatter":
                    formatter = new USe.Common.AppLogger.EventFormatter.EventStringFormatter();
					break;
				case "FriendlyEventStringFormatter":
                    formatter = new USe.Common.AppLogger.EventFormatter.FriendlyEventStringFormatter();
					break;
				default:
					throw new ArgumentException("Invalid [appLogger/implementation] configuration element.");
			}

			switch (element.LoggerType)
			{
				case "NullLogger":
					{
						string name = element.GetCustomAttribute("nullName") as string;
						if (String.IsNullOrEmpty(name))
						{
                            name = USe.Common.AppLogger.Implementation.NullLogger.GetDefaultName();
						}
                        logger = new USe.Common.AppLogger.Implementation.NullLogger(name, formatter);
						break;
					}

				case "ConsoleLogger":
					{
						string name = element.GetCustomAttribute("consoleName") as string;
						if (String.IsNullOrEmpty(name))
						{
                            name = USe.Common.AppLogger.Implementation.ConsoleLogger.GetDefaultConsoleName();
						}
                        logger = new USe.Common.AppLogger.Implementation.ConsoleLogger(name, element.Encoding, formatter);
						break;
					}

				case "FileLogger":
					{
						string name = element.GetCustomAttribute("fileName") as string;
						if (String.IsNullOrEmpty(name))
						{
                            name = USe.Common.AppLogger.Implementation.FileLogger.GetDefaultFileName(element.IsCheckUAC);
						}
						else
						{
							name = FormatFileName(name);
						}
                        logger = new USe.Common.AppLogger.Implementation.FileLogger(name, element.Encoding, formatter);
						break;
					}

				case "FileLogger2":
					{
						string name = element.GetCustomAttribute("fileName") as string;
						if (String.IsNullOrEmpty(name))
						{
                            name = USe.Common.AppLogger.Implementation.FileLogger2.GetDefaultFileName(element.IsCheckUAC);
						}
						else
						{
							name = FormatFileName(name);
						}
                        logger = new USe.Common.AppLogger.Implementation.FileLogger2(name, element.Encoding, formatter);
						break;
					}

				case "DailyFileLogger":
					{
						string name = element.GetCustomAttribute("fileName") as string;
						if (String.IsNullOrEmpty(name))
						{
                            name = USe.Common.AppLogger.Implementation.DailyFileLogger.GetDefaultFileName(element.IsCheckUAC);
						}
						else
						{
							name = FormatFileName(name);
						}
                        logger = new USe.Common.AppLogger.Implementation.DailyFileLogger(name, element.Encoding, formatter);
						break;
					}

				case "DailyFileLogger2":
					{
						string name = element.GetCustomAttribute("fileName") as string;
						if (String.IsNullOrEmpty(name))
						{
                            name = USe.Common.AppLogger.Implementation.DailyFileLogger2.GetDefaultFileName(element.IsCheckUAC);
						}
						else
						{
							name = FormatFileName(name);
						}
                        logger = new USe.Common.AppLogger.Implementation.DailyFileLogger2(name, element.Encoding, formatter);
						break;
					}

				default:
					throw new ArgumentException("Invalid [appLogger/implementation] configuration element.");
			}

			Debug.Assert(logger != null);
			return logger;
		}

		/// <summary>
		/// 按配置元素创建IAppLoggerImpl日志装饰类对象。
		/// </summary>
		/// <param name="element">配置元素。</param>
		/// <param name="innerImpl">内部日志对象。</param>
		/// <returns>日志装饰类对象。</returns>
		/// <remarks>
		/// 配置元素：USe.Common.AppLogger/appLoggers/appLogger/decorators/decorator。
		/// </remarks>
		private static IAppLoggerImpl CreateDecoratorObject(DecoratorElement element, IAppLoggerImpl innerImpl)
		{
			IAppLoggerImpl decorator = null;

			switch (element.LoggerType)
			{
				case "EventTypeFilter":
				{
					LogLevels level = (LogLevels)Enum.Parse(typeof(LogLevels), (string)(element.GetCustomAttribute("logLevel")));
                    decorator = new USe.Common.AppLogger.Decorator.EventTypeFilter(level, innerImpl);
					break;
				}

				case "ConsoleDecorator":
				{
                    decorator = new USe.Common.AppLogger.Decorator.ConsoleDecorator(innerImpl);
					break;
				}

				case "LockDecorator":
				{
                    decorator = new USe.Common.AppLogger.Decorator.LockDecorator(innerImpl);
					break;
				}

				case "MutexDecorator":
				{
					string name = element.GetCustomAttribute("mutexName") as string;
					if (String.IsNullOrEmpty(name))
					{
						name = innerImpl.Name;
					}
                    decorator = new USe.Common.AppLogger.Decorator.MutexDecorator(name, innerImpl);
					break;
				}

                case "ReservedCharacterDecorator":
                {
                    decorator = new USe.Common.AppLogger.Decorator.ReservedCharacterDecorator(innerImpl);
                    break;
                }

				default:
					throw new ArgumentException("Invalid [appLogger/decorators/decorator] configuration element.");
			}

			Debug.Assert(decorator != null);
			return decorator;
		}
		#endregion

		#region [Xu Linqiu] 2009/08/08 取消，改为使用配置文件
		/* 
		/// <summary>
		/// 创建IAppLogger对象
		/// </summary>
		/// <param logName="logName">string, 日志名称, 不能为Null/Empty</param>
		/// <param logName="logMode">LogMode, 日志模式</param>
		/// <returns>日志接口对象</returns>
		/// <remarks>
		/// 辅助的IAppLogger对象创建方法，与Singleton模式的静态实例变量无关。
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentNullException.#ctor(System.String,System.String)")]
		static public IAppLogger CreateInstance(string logName, LogMode logMode)
		{
			IAppLoggerImpl innerImp = null;

			LogMode impMode = (LogMode)(((int)logMode) & 0X00FF); // 日志的实现模式
			LogMode decMode = (LogMode)(((int)logMode) & 0XFF00); // 日志的装饰模式

			// 检验输入名称
			if (string.IsNullOrEmpty(logName))
			{
				throw new ArgumentNullException("logName", "Log name can not be Null when create instance.");
			}

			// NullLogger模式直接返回实例
			if (impMode == LogMode.Null)
			{
				return new NullLogger(logName);
			}

			// 按指定的日志实现模式创建IAppLoggerImpl对象
			switch (impMode)
			{
				case LogMode.File:
					innerImp = new FileLogger(logName, new EventStringFormatter());
					break;

				case LogMode.File2:
					innerImp = new FileLogger2(logName, new EventStringFormatter());
					break;

				case LogMode.DailyFile:
					innerImp = new DailyFileLogger(logName, new EventStringFormatter());
					break;

				case LogMode.DailyFile2:
					innerImp = new DailyFileLogger2(logName, new EventStringFormatter());
					break;

				case LogMode.ConsoleA:
					innerImp = new ConsoleLogger(logName, new EventStringFormatter());
					break;

				case LogMode.ConsoleB:
					innerImp = new ConsoleLogger(logName, new VisibleEventStringFormatter());
					break;

				default:
					Debug.Assert(false);
					break;
			}

			// 按指定的日志装饰模式逐级嵌套地创建IAppLoggerImpl装饰对象
			for (int i = 0X0100; i <= (int)(LogMode.MaxValue); i <<= 1) // 0X0100是日志装饰模式的起始值
			{
				if (((int)decMode & i) == 0) continue;

				switch ((LogMode)i)
				{
					case LogMode.Lock:
						innerImp = new LockLogger(innerImp);
						break;

					case LogMode.Mutex:
						innerImp = new MutexLogger(logName, innerImp);
						break;

					case LogMode.Encrypt:
						Debug.Assert(false);
						throw new ApplicationException("Not support.");

					default:
						Debug.Assert(false);
						break;
				}
			}

			return new AppLogger(innerImp);
		} // CreateInstance() end

		/// <summary>
		/// 初始化IAppLogger对象
		/// </summary>
		/// <param logName="logName">string, 日志名称</param>
		/// <param logName="logMode">LogMode, 日志模式</param>
		/// <returns>日志接口对象</returns>
		/// <remarks>
		/// Singleton模式的静态实例变量的创建方法。
		/// </remarks>
		static public IAppLogger InitInstance(string logName, LogMode logMode)
		{
			// 检查是否重复初始化
			if (ms_singleInstance != null)
			{
				Debug.Assert(false);
				throw new ApplicationException("IAppLogger object already initialized.");
			}

			// 按指定日志模式提供默认文件名称
			if (string.IsNullOrEmpty(logName))
			{
				switch ((LogMode)(((int)logMode) & 0X00FF)) // 0X00FF是LogMode之实体日志模式的掩码
				{
					case LogMode.Null:
						logName = NullLogger.GetDefaultName();
						break;

					case LogMode.File:
					case LogMode.File2:
						logName = AppLogger.GetDefaultFileName();
						break;

					case LogMode.DailyFile:
					case LogMode.DailyFile2:
						logName = AppLogger.GetDefaultFileName();
						break;

					case LogMode.ConsoleA:
					case LogMode.ConsoleB:
						logName = ConsoleLogger.GetDefaultConsoleName();
						break;

					default:
						Debug.Assert(false);
						throw new ArgumentException("Invalid log mode.");
				}
			}

			// 调用创建方法构造日志对象
			Debug.Assert(string.IsNullOrEmpty(logName) == false);
			ms_singleInstance = CreateInstance(logName, logMode);

			return ms_singleInstance;
		}
		*/
		#endregion
	}
}
