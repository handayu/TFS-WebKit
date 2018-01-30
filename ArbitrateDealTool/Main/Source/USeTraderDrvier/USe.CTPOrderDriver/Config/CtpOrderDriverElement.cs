#region Copyright & Version
//==============================================================================
// 文件名称: CtpOrderDriverElement.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: Ctp交易服务器端配置元素定义。
//==============================================================================
#endregion

using System;
using System.Configuration;
using CTPAPI;
using USe.TradeDriver.Common;

namespace USe.TradeDriver.Ctp
{
    /// <summary>
    /// Ctp交易服务器端配置元素定义。
    /// </summary>
    public class CtpOrderDriverElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置配置元素属性name。
        /// </summary>
        /// <value>
        /// name属性值。
        /// </value>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

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
        /// 获取或设置连接超时时间connectTimeOut。
        /// </summary>
        [ConfigurationProperty("connectTimeOut", IsRequired = false)]
        public int ConnectTimeOut
        {
            get { return (int)base["connectTimeOut"]; }
            set { base["connectTimeOut"] = value; }
        }

        /// <summary>
        /// 获取或设置查询超时时间queryTimeOut。
        /// </summary>
        [ConfigurationProperty("queryTimeOut", IsRequired = false)]
        public int QueryTimeOut
        {
            get { return (int)base["queryTimeOut"]; }
            set { base["queryTimeOut"] = value; }
        }

        /// <summary>
        /// 获取或设置交易服务器类型代码。
        /// </summary>
        [ConfigurationProperty("driverType", IsRequired = true)]
        public int DriverTypeCode
        {
            get { return ((int)base["driverType"]); }
            set { base["driverType"] = value; }
        }

        /// <summary>
        /// 获取或设置交易服务器类型。
        /// </summary>
        public USeDriverType DriverType
        {
            get
            {
                return (USeDriverType)this.DriverTypeCode;
            }

            set
            {
                this.DriverTypeCode = (int)value;
            }
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
    }
}
