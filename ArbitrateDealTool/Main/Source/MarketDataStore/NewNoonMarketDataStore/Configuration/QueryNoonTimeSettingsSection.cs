using System;
using System.Configuration;

namespace NewNoonMarketDataStore
{
    /// <summary>
    /// 配置节定义。
    /// </summary>
    public class QueryNoonTimeSettingsSection : ConfigurationSection
    {
        /// <summary>
        /// 获取配置节只读。
        /// </summary>
        [ConfigurationProperty("TimeSpread", IsRequired = true)]
        public QueryNoonTimeSettingsElement QuerySettings
        {
            get
            {
                return (QueryNoonTimeSettingsElement)base["TimeSpread"];
            }
        }

        /// <summary>
        /// 获取系统默认配置文件里面的配置节集合。
        /// </summary>
        /// <returns></returns>
        public static QueryNoonTimeSettingsSection GetSection()
        {
            object section = ConfigurationManager.GetSection("QueryNoonTimeSettings");
            return section as QueryNoonTimeSettingsSection;
        }
    }
}
