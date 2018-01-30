using System;
using System.Configuration;
using System.Diagnostics;

namespace USe.Common.AppLogger.Configuration
{
	/// <summary>
	/// Decorator配置元素集合
	/// </summary>
	public class DecoratorElementCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// 配置元素名称, 只读
		/// </summary>
		protected override string ElementName
		{
			get
			{
				return "decorator";
			}
		}

		/// <summary>
		/// 整型索引器
		/// </summary>
		/// <param name="index">索引</param>
		/// <returns>Decorator配置元素</returns>
		public DecoratorElement this[int index]
		{
			get
			{
				return BaseGet(index) as DecoratorElement;
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
		/// 添加Decorator配置元素对象。
		/// </summary>
		/// <param name="element">Decorator配置元素对象。</param>
		public void Add(DecoratorElement element)
		{
			BaseAdd(element, false);
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
		/// 创建Decorator配置元素对象
		/// </summary>
		/// <returns>Decorator配置元素对象</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new DecoratorElement();
		}

		/// <summary>
		/// 获取指定Decorator配置元素的键值
		/// </summary>
		/// <param name="element">配置元素</param>
		/// <returns></returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((DecoratorElement)element).LoggerType;
		}
	}
}
