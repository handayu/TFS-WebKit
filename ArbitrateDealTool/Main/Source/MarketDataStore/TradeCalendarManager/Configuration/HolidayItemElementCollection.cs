#region Copyright & Version
//==============================================================================
// 文件名称: HolidayItemElementCollection.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2013 POLARIS (Shanghai) Tec. & Res. CO., Ltd.
// 
// 创 建 人: Yang Ming
// 创建日期: 2013/07/04
// 描    述: 特殊日配置元素集合定义类。
//==============================================================================
#endregion

using System;
using System.Configuration;
using System.Diagnostics;

namespace TradeCalendarManager.Configuration
{
	/// <summary>
    /// 节假日配置元素集合定义类。
	/// </summary>
	public sealed class HolidayItemElementCollection : ConfigurationElementCollection
	{
		/// <summary>
        /// 获取<see cref="item"/>的节点名称。
		/// </summary>
		protected override string ElementName
		{
			get
			{
                return "item";
			}
		}

        /// <summary>
        /// 整型索引器。
        /// </summary>
        /// <param name="index">索引。</param>
        /// <returns>SpecialTradingDayElement配置元素。</returns>
        public HolidayItemElement this[int index]
        {
            get
            {
                return BaseGet(index) as HolidayItemElement;
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
        /// 字符串名称索引器。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <returns>HolidayItemElement配置元素。</returns>
        public new HolidayItemElement this[string name]
        {
            get
            {
                return BaseGet(name) as HolidayItemElement;
            }
        }

        /// <summary>
        /// 集合类型, 只读。
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }


        /// <summary>
        /// 创建HolidayItemElement配置元素对象。
        /// </summary>
        /// <returns>HolidayItemElement配置元素对象。</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new HolidayItemElement();
        }


        /// <summary>
        /// 获取指定HolidayItemElement配置元素的键值。
        /// </summary>
        /// <param name="element">配置元素。</param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HolidayItemElement)element).BeginDay;
        }
    }
}
