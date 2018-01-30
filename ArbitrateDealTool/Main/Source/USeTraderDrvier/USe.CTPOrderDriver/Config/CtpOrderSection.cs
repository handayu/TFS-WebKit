#region Copyright & Version
//==============================================================================
// 文件名称: CtpOrderSection.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: Ctp交易服务器配置节定义。
//==============================================================================
#endregion

using System;
using System.Configuration;

namespace USe.TradeDriver.Ctp
{
    /// <summary>
    /// Ctp交易服务器配置节定义。
    /// </summary>
    public class CtpOrderSection : ConfigurationSection
    {
        /// <summary>
        /// CtpOrderDrivers配置节集合, 只读。
        /// </summary>
        [ConfigurationProperty("drivers", IsDefaultCollection = false)]
        public CtpOrderDriverElementCollection CtpOrderDrivers
        {
            get
            {
                return (CtpOrderDriverElementCollection)base["drivers"];
            }
        }

        /// <summary>
        /// 获取系统默认配置文件里面的CtpOrderDriver配置节集合。
        /// </summary>
        /// <returns>CtpOrderDriver配置节。</returns>
        public static CtpOrderSection GetSection()
        {
            object section = ConfigurationManager.GetSection("CtpOrderDriver");
            return section as CtpOrderSection;
        }
    }
}
