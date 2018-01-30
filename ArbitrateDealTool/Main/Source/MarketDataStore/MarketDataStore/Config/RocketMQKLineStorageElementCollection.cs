using System;
using System.Configuration;

namespace MarketDataStore.Config
{
    /// <summary>
    /// K线RocketMQ存储器配置元素集合。
    /// </summary>
    public class RocketMQKLineStorageElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 配置元素名称, 只读。
        /// </summary>
        protected override string ElementName
        {
            get
            {
                return "Storage";
            }
        }

        /// <summary>
        /// 整型索引器。
        /// </summary>
        /// <param name="index">索引。</param>
        /// <returns>RocketMQKLineStorage配置元素。</returns>
        public RocketMQKLineStorageElement this[int index]
        {
            get
            {
                return BaseGet(index) as RocketMQKLineStorageElement;
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
        /// <returns>RocketMQKLineStorage配置元素。</returns>
        public new RocketMQKLineStorageElement this[string name]
        {
            get
            {
                return BaseGet(name) as RocketMQKLineStorageElement;
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
        /// 创建RocketMQKLineStorage配置元素对象。
        /// </summary>
        /// <returns>RocketMQKLineStorage配置元素对象。</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new RocketMQKLineStorageElement();
        }

        /// <summary>
        /// 获取指定RocketMQKLineStorage配置元素的键值。
        /// </summary>
        /// <param name="element">配置元素。</param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RocketMQKLineStorageElement)element).Name;
        }
    }
}
