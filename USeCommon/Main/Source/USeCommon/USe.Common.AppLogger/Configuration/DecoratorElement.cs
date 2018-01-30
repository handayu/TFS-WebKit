using System;
using System.Configuration;
using System.Diagnostics;

namespace USe.Common.AppLogger.Configuration
{
	/// <summary>
	/// IAppLoggerImpl日志装饰类配置元素
	/// </summary>
	public class DecoratorElement : ConfigurationElement
	{
		/// <summary>
		/// 日志装饰类的类型
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
		/// 获取配置元素自定义属性值
		/// </summary>
		/// <param name="name">属性名称</param>
		/// <returns>属性值</returns>
		public object GetCustomAttribute(string name)
		{
			return base[name];
		}

		/// <summary>
		/// 设置配置元素自定义属性值
		/// </summary>
		/// <param name="name">属性名称。</param>
		/// <param name="value">属性值。</param>
		public void SetCustomAttribute(string name, object value)
		{
			base[name] = value;
		}

		/// <summary>
		/// 未定义配置元素属性处理方法, 添加自定义属性
		/// </summary>
		/// <param name="name">属性名称</param>
		/// <param name="value">属性值</param>
		/// <returns>处理与否标志</returns>
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			//Debug.WriteLine("==>Name: " + name + ", Value: " + value + ".");

			ConfigurationProperty property = new ConfigurationProperty(name, typeof(string), null);
			base.Properties.Add(property);
			base.SetPropertyValue(property, value, false);

			return true;
		}
	}
}
