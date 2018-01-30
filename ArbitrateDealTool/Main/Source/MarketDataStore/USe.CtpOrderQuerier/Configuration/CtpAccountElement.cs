using System;
using System.Configuration;

namespace USe.CtpOrderQuerier.Configuration
{
    /// <summary>
    /// CTP交易帐号配置节定义。
    /// </summary>
    public class CtpAccountElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置配置元素属性id。
        /// </summary>       
        /// <value>
        /// id属性值。
        /// </value>
        [ConfigurationProperty("id", IsRequired = true)]
        public string ID
        {
            get { return (string)base["id"]; }
            set { base["id"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性pwd。
        /// </summary>
        /// <value>
        /// pwd属性值。
        /// </value>
        [ConfigurationProperty("pwd", IsRequired = true)]
        public string Password
        {
            get { return (string)base["pwd"]; }
            set { base["pwd"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性brokerID。
        /// </summary>
        /// <value>
        /// brokerID属性值。
        /// </value>
        [ConfigurationProperty("brokerID", IsRequired = true)]
        public string BrokerID
        {
            get { return (string)base["brokerID"]; }
            set { base["brokerID"] = value; }
        }
    }
}

