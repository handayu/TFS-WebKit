using System;
using System.Configuration;

namespace SerialContractCalculater.Configuration
{
    /// <summary>
    /// 品种连续合约计算日期配置定义。
    /// </summary>
    public class CalculateDateElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置配置元素属性type。
        /// </summary>
        /// <value>
        /// type属性值。
        /// </value>
        [ConfigurationProperty("type", IsRequired = true, DefaultValue = SettlementDayType.Today)]
        public SettlementDayType CalculateDayType
        {
            get { return (SettlementDayType)base["type"]; }
            set { base["type"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性begin。
        /// </summary>
        /// <value>
        /// begin属性值。
        /// </value>
        [ConfigurationProperty("beginDate", IsRequired = false)]
        public DateTime? BeginDay
        {
            get
            {
                if (base["beginDate"] == null)
                {
                    return null;
                }
                else
                {
                    return (DateTime)base["beginDate"];
                }
            }
            set
            {
                if (value == null)
                {
                    base["beginDate"] = string.Empty;
                }
                else
                {
                    base["beginDate"] = value.Value;
                }
            }
        }

        /// <summary>
        /// 获取或设置配置元素属性end。
        /// </summary>
        /// <value>
        /// end属性值。
        /// </value>
        [ConfigurationProperty("endDate", IsRequired = false)]
        public DateTime? EndDay
        {
            get
            {
                if (base["endDate"] == null)
                {
                    return null;
                }
                else
                {
                    return (DateTime)base["endDate"];
                }
            }
            set
            {
                if (value == null)
                {
                    base["endDate"] = string.Empty;
                }
                else
                {
                    base["beginDate"] = value.Value;
                }
            }
        }

    }
}
