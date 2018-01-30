using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SerialContractCalculater.Configuration
{
    /// <summary>
    /// 连续合约配置
    /// </summary>
    public class SerialContractCalculateSection : ConfigurationSection
    {
        /// <summary>
        /// 获取CalculateDate配置节, 只读。
        /// </summary>
        [ConfigurationProperty("CalculateDate", IsRequired = true)]
        public CalculateDateElement CalculateDate
        {
            get
            {
                return (CalculateDateElement)base["CalculateDate"];
            }
        }

        /// <summary>
        /// 获取QuerySetting配置节, 只读。
        /// </summary>
        [ConfigurationProperty("QuerySetting", IsRequired = true)]
        public QuerySettingElement QuerySetting
        {
            get
            {
                return (QuerySettingElement)base["QuerySetting"];
            }
        }

        /// <summary>
        /// 获取系统默认配置文件里面的SerialContractCalculateSection配置节集合。
        /// </summary>
        /// <returns>SerialContractCalculateSection配置节。</returns>
        public static SerialContractCalculateSection GetSection()
        {
            object section = ConfigurationManager.GetSection("SerialContractCalculater");
            return section as SerialContractCalculateSection;
        }
    }
}
