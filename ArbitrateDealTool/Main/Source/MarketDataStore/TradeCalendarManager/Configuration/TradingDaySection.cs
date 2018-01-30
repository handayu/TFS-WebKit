#region Copyright & Version
//==============================================================================
// 文件名称: TradingDaySection.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2013 POLARIS (Shanghai) Tec. & Res. CO., Ltd.
// 
// 创 建 人: Yang Ming
// 创建日期: 2013/01/25
// 描    述: TradingDaySection配置节定义类
//==============================================================================
#endregion

using System;
using System.Configuration;
using System.Diagnostics;

namespace TradeCalendarManager.Configuration
{
    /// <summary>
    /// TradingDaySection配置节定义类。
    /// </summary>
    public sealed class TradingDaySection : ConfigurationSection
    {
        /// <summary>
        /// 获取/设置HolidayItemElement配置节集合。
        /// </summary>
        [ConfigurationProperty("Holidays", IsRequired = false, IsDefaultCollection = false)]
        public HolidayItemElementCollection Holidays
        {
            get { return (HolidayItemElementCollection)base["Holidays"]; }
            set { base["Holidays"] = value; }
        }

        /// <summary>
        /// 起始日。
        /// </summary>
        [ConfigurationProperty("beginDay", IsRequired = false)]
        public DateTime BeginDay
        {
            get { return (DateTime)base["beginDay"]; }
            set { base["beginDay"] = value; }
        }

        /// <summary>
        /// 截止日。
        /// </summary>
        [ConfigurationProperty("endDay", IsRequired = false)]
        public DateTime EndDay
        {
            get { return (DateTime)base["endDay"]; }
            set { base["endDay"] = value; }
        }

        /// <summary>
        /// 开始日前一交易日。
        /// </summary>
        [ConfigurationProperty("beginDayPreTradeDay", IsRequired = true)]
        public DateTime BeginDayPreTradeDay
        {
            get { return (DateTime)base["beginDayPreTradeDay"]; }
            set { base["beginDayPreTradeDay"] = value; }
        }

        /// <summary>
        /// 截止日后一交易日。
        /// </summary>
        [ConfigurationProperty("endDayNextTradeDay", IsRequired = true)]
        public DateTime EndDayNextTradeDay
        {
            get { return (DateTime)base["endDayNextTradeDay"]; }
            set { base["endDayNextTradeDay"] = value; }
        }
    }
}
