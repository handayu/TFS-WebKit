using System;
using System.Configuration;

namespace MarketDataStore.Config
{
    /// <summary>
    /// K线MySql存储器配置元素。
    /// </summary>
    public class MySqlKLineStorageElement : ConfigurationElement
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
        /// 获取或设置配置元素属性dbName。
        /// </summary>
        /// <value>
        /// dbName属性值。
        /// </value>
        [ConfigurationProperty("dbName", IsRequired = true)]
        public string DBName
        {
            get { return (string)base["dbName"]; }
            set { base["dbName"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性connectionString。
        /// </summary>
        /// <value>
        /// connectionString属性值。
        /// </value>
        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return (string)base["connectionString"]; }
            set { base["connectionString"] = value; }
        }
    }
}
