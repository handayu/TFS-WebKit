using System;
using System.Configuration;
using System.Diagnostics;

namespace USe.Common.AppLogger.Configuration
{
	/// <summary>
	/// IAppLoggerImpl��־װ��������Ԫ��
	/// </summary>
	public class DecoratorElement : ConfigurationElement
	{
		/// <summary>
		/// ��־װ���������
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
	}
}
