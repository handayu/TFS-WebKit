using System;
using System.Configuration;

namespace NewNoonMarketDataStore
{
    /// <summary>
    /// 配置节定义。
    /// </summary>
    public class QueryNoonTimeSettingsElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置配置元素属性
        /// </summary>
        /// <value>
        /// 属性值。
        /// </value>
        [ConfigurationProperty("queryNoonBeginTime", IsRequired = true)]
        public TimeSpan QueryNoonBeginTime
        {
            get { return (TimeSpan)base["queryNoonBeginTime"]; }
            set { base["queryNoonBeginTime"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性
        /// </summary>
        /// <value>
        /// 属性值。
        /// </value>
        [ConfigurationProperty("queryNoonEndTime", IsRequired = true)]
        public TimeSpan QueryNoonEndTime
        {
            get { return (TimeSpan)base["queryNoonEndTime"]; }
            set { base["queryNoonEndTime"] = value; }
        }

    }
}

