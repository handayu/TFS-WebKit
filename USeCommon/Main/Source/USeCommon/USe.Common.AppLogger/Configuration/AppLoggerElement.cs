using System;
using System.Configuration;
using System.Diagnostics;

namespace USe.Common.AppLogger.Configuration
{
	/// <summary>
	/// AppLogger配置元素。
	/// </summary>
	public class AppLoggerElement : ConfigurationElement
	{
		/// <summary>
		/// AppLogger配置元素名称
		/// </summary>
		[ConfigurationProperty("name", IsKey = true, IsRequired = true)]
		public string Name
		{
			get
			{
				return (string)base["name"];
			}

			set
			{
				base["name"] = value;
			}
		}

		/// <summary>
		/// 日志类的类型
		/// </summary>
		[ConfigurationProperty("type", IsRequired = true)]
		public string LoggerType
		{
			get
			{
				return (string)base["type"];
			}
			set
			{
				base["type"] = value;
			}
		}

		/// <summary>
		/// Decorator配置节集合
		/// </summary>
		[ConfigurationProperty("decorators", IsDefaultCollection = false)]
		public DecoratorElementCollection Decorators
		{
			get
			{
				return (DecoratorElementCollection)base["decorators"];
			}

			set
			{
				base["decorators"] = value;
			}
		}

		/// <summary>
		/// IAppLoggerImpl日志实现类配置元素
		/// </summary>
		[ConfigurationProperty("implementation", IsRequired = false)]
		public ImplementationElement Implementation
		{
			get
			{
				return (ImplementationElement)base["implementation"];
			}

			set
			{
				base["implementation"] = value;
			}
		}
	}
}
