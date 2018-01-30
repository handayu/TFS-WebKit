using System;
using System.Text;
using System.Configuration;
using System.Diagnostics;

namespace USe.Common.AppLogger.Configuration
{
	/// <summary>
	/// IAppLoggerImpl��־ʵ��������Ԫ��
	/// </summary>
	public class ImplementationElement : ConfigurationElement
	{
		/// <summary>
		/// ��־ʵ���������
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
		/// ��־��Ϣ������������
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
		/// ��־��Ϣ��������
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
		/// �����ļ���־ȷ��Ĭ���ļ�·�����Ƿ���UAC��־��
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
		/// ��־ʵ������¼���Ϣ��ʽ����
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
		/// ��ȡ����Ԫ���Զ�������ֵ
		/// </summary>
		/// <param name="name">��������</param>
		/// <returns>����ֵ</returns>
		public object GetCustomAttribute(string name)
		{
			return base[name];
		}

		/// <summary>
		/// ��������Ԫ���Զ�������ֵ
		/// </summary>
		/// <param name="name">�������ơ�</param>
		/// <param name="value">����ֵ��</param>
		public void SetCustomAttribute(string name, object value)
		{
			base[name] = value;
		}

		/// <summary>
		/// δ��������Ԫ�����Դ�����, ����Զ�������
		/// </summary>
		/// <param name="name">��������</param>
		/// <param name="value">����ֵ</param>
		/// <returns>��������־</returns>
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
