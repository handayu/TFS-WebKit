using System;
using System.Configuration;

namespace USe.CtpOrderQuerier.Configuration
{
    /// <summary>
    /// CTP交易服务器配置节定义。
    /// </summary>
    public class CtpOrderDriverSection : ConfigurationSection
    {
        /// <summary>
        /// 获取CTPDriver配置节。, 只读。
        /// </summary>
        [ConfigurationProperty("driver", IsRequired = true)]
        public CtpOrderDriverElement CtpOrderDriver
        {
            get
            {
                return (CtpOrderDriverElement)base["driver"];
            }
        }

        /// <summary>
        /// 获取CtpAccount配置节, 只读。
        /// </summary>
        [ConfigurationProperty("account", IsRequired = true)]
        public CtpAccountElement CtpAccount
        {
            get
            {
                return (CtpAccountElement)base["account"];
            }
        }

        /// <summary>
        /// 获取系统默认配置文件里面的CtpOrderDriverSection配置节集合。
        /// </summary>
        /// <returns>CtpOrderDriverSection。</returns>
        public static CtpOrderDriverSection GetSection()
        {
            object section = ConfigurationManager.GetSection("CtpOrderDriver");
            return section as CtpOrderDriverSection;
        }
    }
}
