using System;
using System.Configuration;
using System.Diagnostics;

namespace USe.Common.AppLogger.Configuration
{
	/// <summary>
	/// AppLoggers配置节。
	/// </summary>
	public class AppLoggersSection : ConfigurationSection
	{
		/// <summary>
		/// AppLogger配置节集合, 只读
		/// </summary>
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public AppLoggerElementCollection AppLoggers
		{
			get
			{
				return (AppLoggerElementCollection)base[""];
			}
		}

        /// <summary>
        /// 获取系统默认配置文件里面的AppLogger配置节集合。
        /// </summary>
        /// <returns>AppLogger配置节集合</returns>
        public static AppLoggersSection GetSection()
        {
            object section = ConfigurationManager.GetSection("USe.Common.AppLogger/appLoggers");
            return section as AppLoggersSection;
        }
	}
}
