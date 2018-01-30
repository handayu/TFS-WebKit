using System;
using System.Configuration;
using System.Diagnostics;

namespace USe.Common.AppLogger.Configuration
{
	/// <summary>
	/// AppLogger配置元素集合。
	/// </summary>
	public class AppLoggerElementCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// 配置元素名称, 只读
		/// </summary>
		protected override string ElementName
		{
			get
			{
				return "appLogger";
			}
		}

		/// <summary>
		/// 整型索引器
		/// </summary>
		/// <param name="index">索引</param>
		/// <returns>AppLogger配置元素</returns>
		public AppLoggerElement this[int index]
		{
			get
			{
				return BaseGet(index) as AppLoggerElement;
			}

			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		/// <summary>
		/// 字符串名称索引器
		/// </summary>
		/// <param name="name">名称</param>
		/// <returns>AppLogger配置元素</returns>
		public new AppLoggerElement this[string name]
		{
			get
			{
				return BaseGet(name) as AppLoggerElement;
			}
		}

		/// <summary>
		/// 集合类型, 只读
		/// </summary>
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}


		/// <summary>
		/// 创建AppLogger配置元素对象
		/// </summary>
		/// <returns>AppLogger配置元素对象</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new AppLoggerElement();
		}

		/// <summary>
		/// 获取指定AppLogger配置元素的键值
		/// </summary>
		/// <param name="element">配置元素</param>
		/// <returns></returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((AppLoggerElement)element).Name;
		}
	}
}
