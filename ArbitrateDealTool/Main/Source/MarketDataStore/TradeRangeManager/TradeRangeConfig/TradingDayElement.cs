using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TradeRangeManager
{
    public class TradingDayElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置配置元素属性beginTime。
        /// </summary>
        /// <value>
        /// beginTime属性值。
        /// </value>
        [ConfigurationProperty("beginTime", IsRequired = true)]
        public string BeginTime
        {
            get { return (string)base["beginTime"]; }
            set { base["beginTime"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性endTime。
        /// </summary>       
        /// <value>
        /// endTime属性值。
        /// </value>
        [ConfigurationProperty("endTime", IsRequired = true)]
        public string EndTime
        {
            get { return (string)base["endTime"]; }
            set { base["endTime"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性isNight。
        /// </summary>
        /// <value>
        /// isNight属性值。
        /// </value>
        [ConfigurationProperty("isNight", IsRequired = true)]
        public bool IsNight
        {
            get { return (bool)base["isNight"]; }
            set { base["isNight"] = value; }
        }

    }
}
