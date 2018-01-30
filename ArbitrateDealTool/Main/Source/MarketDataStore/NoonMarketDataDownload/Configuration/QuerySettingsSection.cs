using System;
using System.Configuration;

namespace NoonMarketDataDownload
{
    /// <summary>
    /// 配置节定义。
    /// </summary>
    public class QuerySettingsSection : ConfigurationSection
    {
        /// <summary>
        /// 获取CTPDriver配置节只读。
        /// </summary>
        [ConfigurationProperty("Setting", IsRequired = true)]
        public QuerySettingsElement QuerySettings
        {
            get
            {
                return (QuerySettingsElement)base["Setting"];
            }
        }

        /// <summary>
        /// 获取系统默认配置文件里面的配置节集合。
        /// </summary>
        /// <returns></returns>
        public static QuerySettingsSection GetSection()
        {
            object section = ConfigurationManager.GetSection("QuerySettlementSettings");
            return section as QuerySettingsSection;
        }
    }
}
