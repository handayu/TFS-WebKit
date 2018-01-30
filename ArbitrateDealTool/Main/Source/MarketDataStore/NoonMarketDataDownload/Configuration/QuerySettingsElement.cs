using System;
using System.Configuration;

namespace NoonMarketDataDownload
{
    /// <summary>
    /// 配置节定义。
    /// </summary>
    public class QuerySettingsElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置配置元素属性
        /// </summary>
        /// <value>
        /// 属性值。
        /// </value>
        [ConfigurationProperty("queryFrequence", IsRequired = true)]
        public TimeSpan QueryFrequence
        {
            get { return (TimeSpan)base["queryFrequence"]; }
            set { base["queryFrequence"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性
        /// </summary>
        /// <value>
        /// 属性值。
        /// </value>
        [ConfigurationProperty("exchangeNoonEndTime", IsRequired = true)]
        public TimeSpan ExchangeNoonEndTime
        {
            get { return (TimeSpan)base["exchangeNoonEndTime"]; }
            set { base["exchangeNoonEndTime"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性
        /// </summary>
        /// <value>
        /// 属性值。
        /// </value>
        [ConfigurationProperty("exchangeNoonBeginTime", IsRequired = true)]
        public TimeSpan ExchangeNoonBeginTime
        {
            get { return (TimeSpan)base["exchangeNoonBeginTime"]; }
            set { base["exchangeNoonBeginTime"] = value; }
        }

    }
}

