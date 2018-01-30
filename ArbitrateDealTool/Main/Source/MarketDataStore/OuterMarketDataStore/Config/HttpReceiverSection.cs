using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace OuterMarketDataStore
{
    /// <summary>
    /// 配置节定义。
    /// </summary>
    public class HttpReceiverSection : ConfigurationSection
    {

        /// <summary>
        /// 获取或设置配置元素属性quoteAddress。
        /// </summary>       
        /// <value>
        /// quoteAddress属性值。
        /// </value>
        [ConfigurationProperty("quoteAddress", IsRequired = true)]
        public string QuoteAddress
        {
            get { return (string)base["quoteAddress"]; }
            set { base["quoteAddress"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性quotePort。
        /// </summary>       
        /// <value>
        /// quotePort属性值。
        /// </value>
        [ConfigurationProperty("quotePort", IsRequired = true)]
        public int QuotePort
        {
            get { return (int)base["quotePort"]; }
            set { base["quotePort"] = value; }
        }

        /// <summary>
        /// 获取系统默认配置文件里面的配置节集合。
        /// </summary>
        /// <returns>配置节。</returns>
        public static HttpReceiverSection GetSection()
        {
            object section = ConfigurationManager.GetSection("HttpReceiver");
            return section as HttpReceiverSection;
        }
    }
}
