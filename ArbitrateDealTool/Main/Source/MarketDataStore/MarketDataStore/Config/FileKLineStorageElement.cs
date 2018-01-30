using System;
using System.Configuration;

namespace MarketDataStore.Config
{
    /// <summary>
    /// K线文件存储器配置元素。
    /// </summary>
    public class FileKLineStorageElement : ConfigurationElement
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
        /// 获取或设置配置元素属性savePath。
        /// </summary>
        /// <value>
        /// savePath属性值。
        /// </value>
        [ConfigurationProperty("savePath", IsRequired = true)]
        public string SavePath
        {
            get { return (string)base["savePath"]; }
            set { base["savePath"] = value; }
        }
    }
}
