using System;
using System.Configuration;

namespace MarketDataStore.Config
{
    /// <summary>
    /// K线文件存储器配置元素集合。
    /// </summary>
    public class FileKLineStorageElementCollection : ConfigurationElementCollection
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
        /// <returns>FileKLineStorage配置元素。</returns>
        public FileKLineStorageElement this[int index]
        {
            get
            {
                return BaseGet(index) as FileKLineStorageElement;
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
        /// <returns>FileKLineStorage配置元素。</returns>
        public new FileKLineStorageElement this[string name]
        {
            get
            {
                return BaseGet(name) as FileKLineStorageElement;
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
        /// 创建FileKLineStorage配置元素对象。
        /// </summary>
        /// <returns>FileKLineStorage配置元素对象。</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new FileKLineStorageElement();
        }

        /// <summary>
        /// 获取指定FileKLineStorage配置元素的键值。
        /// </summary>
        /// <param name="element">配置元素。</param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileKLineStorageElement)element).Name;
        }
    }
}
