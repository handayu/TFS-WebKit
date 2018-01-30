using System;
using System.Configuration;

namespace MarketDataStore.Config
{
    /// <summary>
    /// K线RabbitMQ存储器配置元素。
    /// </summary>
    public class RabbitMQKLineStorageElement : ConfigurationElement
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
        /// 获取或设置配置元素属性address。
        /// </summary>
        /// <value>
        /// address属性值。
        /// </value>
        [ConfigurationProperty("address", IsRequired = true)]
        public string Address
        {
            get { return (string)base["address"]; }
            set { base["address"] = value; }
        }
    }
}
