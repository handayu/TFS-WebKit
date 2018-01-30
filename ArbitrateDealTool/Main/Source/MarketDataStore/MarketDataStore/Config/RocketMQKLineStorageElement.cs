using System;
using System.Configuration;

namespace MarketDataStore.Config
{
    /// <summary>
    /// K线RocketMQ存储器配置元素。
    /// </summary>
    public class RocketMQKLineStorageElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置配置元素属性name。
        /// </summary>
        /// <value>
        /// name属性值。
        /// </value>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性enable。
        /// </summary>       
        /// <value>
        /// enable属性值。
        /// </value>
        [ConfigurationProperty("enable", IsRequired = true)]
        public bool Enable
        {
            get { return (bool)base["enable"]; }
            set { base["enable"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性sendUrl。
        /// </summary>
        /// <value>
        /// sendUrl属性值。
        /// </value>
        [ConfigurationProperty("sendUrl", IsRequired = true)]
        public string SendUrl
        {
            get { return (string)base["sendUrl"]; }
            set { base["sendUrl"] = value; }
        }
    }
}
