using System;
using System.Configuration;

namespace USe.CtpOrderQuerier.Configuration
{
    /// <summary>
    /// CTP交易服务器信息配置节定义。
    /// </summary>
    public class CtpOrderDriverElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置配置元素属性address。
        /// </summary>       
        /// <value>
        /// address属性值。
        /// </value>
        [ConfigurationProperty("address", IsRequired = true)]
        public string Address
        {
            get { return (string)base["address"]; }
            set { base["address"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性port。
        /// </summary>
        /// <value>
        /// port属性值。
        /// </value>
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)base["port"]; }
            set { base["port"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性queryTimeOut。
        /// </summary>
        /// <value>
        /// queryTimeOut属性值。
        /// </value>
        [ConfigurationProperty("queryTimeOut", IsRequired = true)]
        public int QueryTimeOut
        {
            get { return (int)base["queryTimeOut"]; }
            set { base["queryTimeOut"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性loginTimeOut。
        /// </summary>
        /// <value>
        /// loginTimeOut属性值。
        /// </value>
        [ConfigurationProperty("loginTimeOut", IsRequired = true)]
        public int LoginTimeOut
        {
            get { return (int)base["loginTimeOut"]; }
            set { base["loginTimeOut"] = value; }
        }
    }
}

