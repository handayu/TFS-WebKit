using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace USeFuturesSpirit.Configuration
{
    public class ProductTradeRangeItemElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置配置元素属性beginTime。
        /// </summary>
        /// <value>
        /// beginTime属性值。
        /// </value>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name 
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性endTime。
        /// </summary>       
        /// <value>
        /// endTime属性值。
        /// </value>
        [ConfigurationProperty("description", IsRequired = true)]
        public string Description
        {
            get { return (string)base["description"]; }
            set { base["description"] = value; }
        }


        /// <summary>
        /// 获取或设置配置元素属性exchange。
        /// </summary>       
        /// <value>
        /// exchange属性值。
        /// </value>
        [ConfigurationProperty("exchange", IsRequired = true)]
        public string Exchange
        {
            get { return (string)base["exchange"]; }
            set { base["exchange"] = value; }
        }

        /// <summary>
        /// 配置节集合, 只读。
        /// </summary>
        [ConfigurationProperty("tradingDays", IsDefaultCollection = false)]
        public TradingDayElementCollection TradingDaysElementCollection
        {
            get
            {
                return (TradingDayElementCollection)base["tradingDays"];
            }
        }
    }
}
