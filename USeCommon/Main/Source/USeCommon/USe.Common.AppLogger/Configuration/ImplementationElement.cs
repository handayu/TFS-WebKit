using System;
using System.Text;
using System.Configuration;
using System.Diagnostics;

namespace USe.Common.AppLogger.Configuration
{
	/// <summary>
	/// IAppLoggerImpl日志实现类配置元素
	/// </summary>
	public class ImplementationElement : ConfigurationElement
	{
		/// <summary>
		/// 日志实现类的类型
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
		/// 日志信息编码类型名称
		/// </summary>
		[ConfigurationProperty("encoding", IsRequired = true)]
		public string EncodingName
		{
			get
			{
				return (string)base["encoding"];
			}

			set
			{
				base["encoding"] = value;
			}
		}

		/// <summary>
		/// 日志信息编码类型
		/// </summary>
		public Encoding Encoding
		{
			get
			{
				if (String.IsNullOrEmpty(EncodingName))
				{
					return Encoding.Default;
				}
				else
				{
					return Encoding.GetEncoding(EncodingName);
				}
			}

			set
			{
				EncodingName = value.WebName;
			}
		}

		/// <summary>
		/// 用于文件日志确定默认文件路径的是否检查UAC标志。
		/// </summary>
		public bool IsCheckUAC
		{
			get
			{
				bool result = true;

				string value = GetCustomAttribute("checkUAC") as string;
				if (!String.IsNullOrEmpty(value) && value.Equals("False", StringComparison.OrdinalIgnoreCase))
				{
					result = false;
				}

				return result;
			}

			set
			{
				SetCustomAttribute("checkUAC", value ? "True" : "False");
			}
		}

		/// <summary>
		/// 日志实现类的事件消息格式化器
		/// </summary>
		[ConfigurationProperty("eventFormatter", IsRequired = true)]
		public string EventFormatter
		{
			get
			{
				return (string)base["eventFormatter"];
			}
			set
			{
				base["eventFormatter"] = value;
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

		/*
		//[ConfigurationProperty("appLoggerImpl", IsRequired = false)]
		public ImplementationElement AppLoggerImpl
		{
			get
			{
				if (m_hasInnerImpl == false)
				{
					return null;
				}

				return (ImplementationElement)base["appLoggerImpl"];
			}
			set
			{
				base["appLoggerImpl"] = value;
			}
		}

		protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
		{
			if (elementName != "appLoggerImpl")
			{
				return false;
			}

			ImplementationElement e = new ImplementationElement();
			e.DeserializeElement(reader, false);

			ConfigurationProperty property = new ConfigurationProperty("appLoggerImpl", typeof(ImplementationElement), null);
			base.Properties.Add(property);
			base.SetPropertyValue(property, e, false);

			m_hasInnerImpl = true;

			Debug.WriteLine("==>Name: " + elementName + ".");
			return true;
		}
		*/
	}
}
