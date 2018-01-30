#region Copyright & Version
//==============================================================================
// 文件名称: CtpQuoteSection.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Justin Shen
// 创建日期: 2012/05/11
// 描    述: Ctp行情服务器配置节定义。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Configuration;

namespace USe.TradeDriver.Ctp
{
    /// <summary>
    /// Ctp行情服务器配置节定义。
    /// </summary>
    public class CtpQuoteSection : ConfigurationSection
    {
        /// <summary>
        /// CtpQuoteDrivers配置节集合, 只读。
        /// </summary>
        [ConfigurationProperty("drivers", IsDefaultCollection = false)]
        public CtpQuoteDriverElementCollection CtpQuoteDrivers
        {
            get
            {
                return (CtpQuoteDriverElementCollection)base["drivers"];
            }
        }

        /// <summary>
        /// 获取系统默认配置文件里面的CtpQuoteDriver配置节集合。
        /// </summary>
        /// <returns>CtpQuoteDriver配置节。</returns>
        public static CtpQuoteSection GetSection()
        {
            object section = ConfigurationManager.GetSection("CtpQuoteDriver");
            return section as CtpQuoteSection;
        }
    }
}
