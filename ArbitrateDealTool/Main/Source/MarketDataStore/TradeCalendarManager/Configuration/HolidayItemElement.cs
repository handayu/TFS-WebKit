using System;
using System.Configuration;
using System.Diagnostics;

namespace TradeCalendarManager.Configuration
{
	/// <summary>
	/// 节假日日配置元素定义类。
	/// </summary>
	public sealed class HolidayItemElement : ConfigurationElement
	{
		
        /// <summary>
        /// 起始日。
        /// </summary>
        [ConfigurationProperty("beginDay", IsRequired = true)]
        public DateTime BeginDay
        {
            get { return (DateTime)base["beginDay"]; }
            set { base["beginDay"] = value; }
        }

        /// <summary>
        /// 截止日。
        /// </summary>
        [ConfigurationProperty("endDay", IsRequired = true)]
        public DateTime EndDay
        {
            get { return (DateTime)base["endDay"]; }
            set { base["endDay"] = value; }
        }

        
    }
}
