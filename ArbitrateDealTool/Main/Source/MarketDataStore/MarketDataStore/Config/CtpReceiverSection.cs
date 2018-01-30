using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MarketDataStore
{
    /// <summary>
    /// Ctp交易服务器配置节定义。
    /// </summary>
    public class CtpReceiverSection : ConfigurationSection
    {
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

        /// <summary>
        /// 获取或设置配置元素属性quoteFrontAddress。
        /// </summary>       
        /// <value>
        /// quoteFrontAddress属性值。
        /// </value>
        [ConfigurationProperty("quoteAddress", IsRequired = true)]
        public string QuoteAddress
        {
            get { return (string)base["quoteAddress"]; }
            set { base["quoteAddress"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性quoteFrontPort。
        /// </summary>       
        /// <value>
        /// quoteFrontPort属性值。
        /// </value>
        [ConfigurationProperty("quotePort", IsRequired = true)]
        public int QuotePort
        {
            get { return (int)base["quotePort"]; }
            set { base["quotePort"] = value; }
        }


        /// <summary>
        /// 获取或设置连接超时时间connectTimeOut。
        /// </summary>
        [ConfigurationProperty("connectTimeOut", IsRequired = false)]
        public int ConnectTimeOut
        {
            get { return (int)base["connectTimeOut"]; }
            set { base["connectTimeOut"] = value; }
        }

        /// <summary>
        /// 获取或设置文件流路径streamPath。
        /// </summary>
        [ConfigurationProperty("streamPath", IsRequired = false)]
        public string StreamPath
        {
            get { return (string)base["streamPath"]; }
            set { base["streamPath"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性account。
        /// </summary>
        /// <value>
        /// account属性值。
        /// </value>
        [ConfigurationProperty("account", IsRequired = true)]
        public string Account
        {
            get { return (string)base["account"]; }
            set { base["account"] = value; }
        }

        /// <summary>
        /// 获取或设置配置元素属性password。
        /// </summary>       
        /// <value>
        /// password属性值。
        /// </value>
        [ConfigurationProperty("password", IsRequired = true)]
        public string PassWord
        {
            get { return (string)base["password"]; }
            set { base["password"] = value; }
        }

        /// <summary>
        /// 获取系统默认配置文件里面的CtpOrderDriver配置节集合。
        /// </summary>
        /// <returns>配置节。</returns>
        public static CtpReceiverSection GetSection()
        {
            object section = ConfigurationManager.GetSection("CtpReceiver");
            return section as CtpReceiverSection;
        }
    }
}
