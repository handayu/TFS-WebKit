using System;
using System.Configuration;
using System.Diagnostics;

namespace USe.Common.AppLogger.Configuration
{
	/// <summary>
	/// AppLogger配置节组。
	/// </summary>
	public sealed class AppLoggerSectionGroup : ConfigurationSectionGroup
	{
		/// <summary>
		/// AppLogger集合配置节，只读。
		/// </summary>
		public AppLoggersSection AppLoggers
		{
			get
			{
				return Sections["appLoggers"] as AppLoggersSection;
			}
		}

		/// <summary>
		/// 从应用程序缺省的配置获取AppLogger配置组对象。
		/// </summary>
		/// <returns>
		/// AppLogger配置组对象，不会为null。
		/// </returns>
		/// <exception cref="ApplicationException">未找到应用程序缺省的配置时。</exception>
		public static AppLoggerSectionGroup GetSectionGroup()
		{
			string exePath = Process.GetCurrentProcess().MainModule.FileName;
			Debug.Assert(!String.IsNullOrEmpty(exePath));

			System.Configuration.Configuration config;
			config = ConfigurationManager.OpenExeConfiguration(exePath) as System.Configuration.Configuration;
			if (config == null)
			{
				throw new ApplicationException("Not found the app.config.");
			}

			return GetSectionGroup(config);
		}

		/// <summary>
		/// 从指定的配置对象获取AppLogger配置组对象
		/// </summary>
		/// <param name="config">配置对象，不能为null。</param>
		/// <returns>
		/// AppLogger配置组对象，不会为null。
		/// </returns>
		/// <exception cref="System.ArgumentNullException">config对象为null。</exception>
		/// <exception cref="ArgumentException">config中未找到对应的配置组节点。</exception>
		public static AppLoggerSectionGroup GetSectionGroup(System.Configuration.Configuration config)
		{
			if (config == null)
			{
				throw new ArgumentNullException();
			}

			ConfigurationSectionGroup group = null;
			AppLoggerSectionGroup appLoggerGroup = null;

			//group = config.SectionGroups["USe.Common.AppLogger"];
			foreach (ConfigurationSectionGroup g in config.SectionGroups)
			{
				if (g.Name == "USe.Common.AppLogger")
				{
					group = g;
					break;
				}
			}
			if (group == null)
			{
				throw new ArgumentException("Not found the special configuration section group.");
			}

			appLoggerGroup = group as AppLoggerSectionGroup;
			Debug.Assert(appLoggerGroup != null);

			return appLoggerGroup;
		}

		/// <summary>
		/// 使用指定的配置元素名称查找对应的应用程序日志配置参数对象。
		/// </summary>
		/// <param name="elementName">配置元素名称，大小写敏感。</param>
		/// <returns>
		/// 应用程序日志配置参数对象，未找到时返回null。
		/// </returns>
		public static AppLoggerElement FindAppLoggerElement(string elementName)
		{
			AppLoggerSectionGroup group = GetSectionGroup();
			Debug.Assert(group != null);

			AppLoggerElement result = null;
			foreach (AppLoggerElement e in group.AppLoggers.AppLoggers)
			{
				if (e.Name == elementName)
				{
					result = e;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// 使用指定的配置元素名称查找对应的应用程序日志配置参数对象。
		/// </summary>
		/// <param name="config">配置对象，不能为null。</param>
		/// <param name="elementName">配置元素名称，大小写敏感。</param>
		/// <returns>
		/// 应用程序日志配置参数对象，未找到时返回null。
		/// </returns>
		public static AppLoggerElement FindAppLoggerElement(System.Configuration.Configuration config, string elementName)
		{
			AppLoggerSectionGroup group = GetSectionGroup(config);
			Debug.Assert(group != null);

			AppLoggerElement result = null;
			foreach (AppLoggerElement e in group.AppLoggers.AppLoggers)
			{
				if (e.Name == elementName)
				{
					result = e;
					break;
				}
			}

			return result;
		}
	}
}
