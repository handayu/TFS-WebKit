using System;
using System.Configuration;

namespace SerialContractCalculater.Configuration
{
    /// <summary>
    /// 查询间隔设置配节定义。
    /// </summary>
    public class QuerySettingElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置配置元素属性interval(查询间隔时间)。
        /// </summary>       
        /// <value>
        /// interval属性值。
        /// </value>
        [ConfigurationProperty("interval", IsRequired = true)]
        public TimeSpan Interval
        {
            get { return (TimeSpan)base["interval"]; }
            set { base["interval"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性count(查询次数)。
        /// </summary>       
        /// <value>
        /// count属性值。
        /// </value>
        [ConfigurationProperty("count", IsRequired = true)]
        public int Count
        {
            get { return (int)base["count"]; }
            set { base["count"] = value; }
        }
    }
}
