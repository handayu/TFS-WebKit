﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TradeRangeManager
{
    public class ProductTradeRangeItemElementCollection : ConfigurationElementCollection
    {

        /// <summary>
        /// 配置元素名称, 只读。
        /// </summary>
        protected override string ElementName
        {
            get
            {
                return "productItem";
            }
        }

        /// <summary>
        /// 整型索引器。
        /// </summary>
        /// <param name="index">索引。</param>
        /// <returns>TradingDay配置元素。</returns>
        public ProductTradeRangeItemElement this[int index]
        {
            get
            {
                return BaseGet(index) as ProductTradeRangeItemElement;
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
        /// <returns>TradingDay配置元素。</returns>
        public new ProductTradeRangeItemElement this[string name]
        {
            get
            {
                return BaseGet(name) as ProductTradeRangeItemElement;
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
        /// 创建配置元素对象。
        /// ExchangeTradeRangeElement</summary>
        /// <returns>TradingDay配置元素对象。</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProductTradeRangeItemElement();
        }

        /// <summary>
        /// 获取指定配置元素的键值。
        /// </summary>
        /// <param name="element">ExchangeTradeRangeElement配置元素。</param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProductTradeRangeItemElement)element).Name;
        }
    }
}
