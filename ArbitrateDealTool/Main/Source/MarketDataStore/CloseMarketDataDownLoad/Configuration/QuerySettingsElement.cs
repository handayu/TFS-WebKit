using System;
using System.Configuration;

namespace CloseMarketDataDownLoad
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
        [ConfigurationProperty("queryDay", IsRequired = true)]
        public QueryDay QueryDay
        {
            get { return (QueryDay)base["queryDay"]; }
            set { base["queryDay"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性
        /// </summary>
        /// <value>
        /// 属性值。
        /// </value>
        [ConfigurationProperty("queryNum", IsRequired = true)]
        public int QueryNum
        {
            get { return (int)base["queryNum"]; }
            set { base["queryNum"] = value; }
        }

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

    }
}

