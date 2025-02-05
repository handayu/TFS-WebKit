﻿#region Copyright & Version
//==============================================================================
// 文件名称: CtpQuoteDriverElementCollection.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Justin Shen
// 创建日期: 2012/05/11
// 描    述: CtpQuoteDriver配置元素集合。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Configuration;

namespace USe.TradeDriver.Ctp
{
    /// <summary>
    /// CtpQuoteDriver配置元素集合。
    /// </summary>
    public class CtpQuoteDriverElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 配置元素名称, 只读。
        /// </summary>
        protected override string ElementName
        {
            get
            {
                return "driver";
            }
        }

        /// <summary>
        /// 整型索引器。
        /// </summary>
        /// <param name="index">索引。</param>
        /// <returns>CtpQuoteDriver配置元素。</returns>
        public CtpQuoteDriverElement this[int index]
        {
            get
            {
                return BaseGet(index) as CtpQuoteDriverElement;
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
        /// <returns>CtpQuoteDriver配置元素。</returns>
        public new CtpQuoteDriverElement this[string name]
        {
            get
            {
                return BaseGet(name) as CtpQuoteDriverElement;
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
        /// 创建CtpQuoteDriver配置元素对象。
        /// </summary>
        /// <returns>CtpQuoteDirver配置元素对象。</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CtpQuoteDriverElement();
        }

        /// <summary>
        /// 获取指定CtpQuoteDriver配置元素的键值。
        /// </summary>
        /// <param name="element">配置元素。</param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CtpQuoteDriverElement)element).Name;
        }
    }
}
