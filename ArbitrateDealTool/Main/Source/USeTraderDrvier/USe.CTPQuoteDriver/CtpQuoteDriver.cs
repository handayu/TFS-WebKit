#region Copyright & Version
//==============================================================================
// 文件名称: CtpQuoteDriver.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Justin Shen
// 创建日期: 2012/05/10
// 描    述: CTP行情驱动类。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

using CTPAPI;
using USe.Common.AppLogger;
using USe.TradeDriver.Common;

namespace USe.TradeDriver.Ctp
{
    /// <summary>
    /// CTP行情驱动类
    /// </summary>
    public partial class CtpQuoteDriver
    {
        #region member
        private const int DefaultConnectTimeOut = 5000;     // 默认连接超时时间。
        private const int DefaultQueryTimeOut = 5000;       // 默认查询超时时间。

        private const string CONNECT_EVENT_KEY = "Connect";
        private const string LOGIN_EVENT_KEY = "Login";

        private object m_object = new object();
        private CtpFeed m_ctpFeed;
        private string m_ctpFeedStreamFilePath = null; // 文件流路径

        private string m_address = string.Empty;      // 服务器地址。
        private int m_port = 0;                       // 行情服务器端口。
        private int m_connectTimeOut = 0;             // 服务器连接超时时间。
        private int m_queryTimeOut = 0;               // 查询超时时间。

        private string m_password = string.Empty;     // 密码(为自动重连准备)

        private Dictionary<string, USeInstrument> m_instrumentDic; // 产品列表
        private Dictionary<string, USeMarketData> m_marketDataDic; // 期货行情信息。
        private Dictionary<string, USeResetEvent> m_eventDic;      // 同步查询Event(注:EventID="Connect",固定为连接使用,EventID="Login",固定为登录使用)

        private System.Threading.Timer m_autoLoginTimer = null;  // 自动登录定时器
        #endregion // member

        #region construction
        public CtpQuoteDriver(string address, int port)
            :this(address,port,DefaultConnectTimeOut,DefaultQueryTimeOut,string.Empty)
        {
        }

        /// <summary>
        /// 构造CtpQuoteDriver实例。
        /// </summary>
        /// <param name="address">行情服务器地址。</param>
        /// <param name="port">行情服务器端口。</param>
        /// <param name="connectTimeOut">服务器连接超时时间。</param>
        /// <param name="queryTimeOut">查询超时时间。</param>
        public CtpQuoteDriver(string address, int port, int connectTimeOut, int queryTimeOut, string streamFilePath)
        {
            m_address = address;
            m_port = port;
            m_connectTimeOut = connectTimeOut > 0 ? connectTimeOut : DefaultConnectTimeOut;
            m_queryTimeOut = queryTimeOut > 0 ? queryTimeOut : DefaultQueryTimeOut;
            m_ctpFeedStreamFilePath = streamFilePath;
            if (string.IsNullOrEmpty(m_ctpFeedStreamFilePath))
            {
                FileInfo fileInfo = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
                string companyName = "USe";
                string appFolder = fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length);
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), companyName, appFolder, "CtpFeedStream");
                m_ctpFeedStreamFilePath = fileName;
            }

            m_instrumentDic = new Dictionary<string, USeInstrument>();
            m_eventDic = new Dictionary<string, USeResetEvent>();
            m_marketDataDic = new Dictionary<string, USeMarketData>();

            m_logger = new NullLogger("NullLogger<CtpQuoteDriver>");

            m_autoLoginTimer = new System.Threading.Timer(OnAutoLoginExpired, false, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// 构造CtpQuoteDriver实例。
        /// </summary>
        /// <param name="section">CtpQuoteSection配置节。</param>
        public CtpQuoteDriver(CtpQuoteSection section)
        {
            CtpQuoteDriverElementCollection driverColl = section.CtpQuoteDrivers;
            if (driverColl == null || driverColl.Count <= 0)
            {
                throw new Exception("Invalid CtpQuoteSection.");
            }

            CtpQuoteDriverElement driverElement = driverColl[0];
            Debug.Assert(driverElement != null);
            m_address = driverElement.Address;
            m_port = driverElement.Port;
            m_connectTimeOut = driverElement.ConnectTimeOut > 0 ? driverElement.ConnectTimeOut : DefaultConnectTimeOut;
            m_queryTimeOut = driverElement.QueryTimeOut > 0 ? driverElement.QueryTimeOut : DefaultQueryTimeOut;
            m_ctpFeedStreamFilePath = driverElement.StreamPath;

            if (string.IsNullOrEmpty(m_ctpFeedStreamFilePath))
            {
                FileInfo fileInfo = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
                string companyName = "USe";
                string appFolder = fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length);
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), companyName, appFolder, "CtpFeedStream");
                m_ctpFeedStreamFilePath = fileName;
            }

            m_instrumentDic = new Dictionary<string, USeInstrument>();
            m_eventDic = new Dictionary<string, USeResetEvent>();
            m_marketDataDic = new Dictionary<string, USeMarketData>();

            m_logger = new NullLogger("NullLogger<CtpQuoteDriver>");

            m_autoLoginTimer = new System.Threading.Timer(OnAutoLoginExpired, false, Timeout.Infinite, Timeout.Infinite);
        }
        #endregion // construction

        private void OnAutoLoginExpired(object state)
        {
            int loginCount = 0;
            while (loginCount < 3)
            {
                loginCount++;
                if (string.IsNullOrEmpty(m_investorID))
                {
                    m_logger.WriteError(string.Format("{0}.OnAutoLoginExpired() failed,InvestorID is null.", ToString()));
                    return;
                }
                if (string.IsNullOrEmpty(m_password))
                {
                    m_logger.WriteError(string.Format("{0}.OnAutoLoginExpired() failed,Password is null.", ToString()));
                    return;
                }

                try
                {
                    Login(m_brokerID, m_investorID, m_password);
                    m_logger.WriteInformation(string.Format("{0}.OnAutoLoginExpired() relogon {2} times ok,[InvestorID:{1}.]", ToString(), m_investorID, loginCount));
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(2000); // 重新登录失败,延时2秒在登录
                    m_logger.WriteError(string.Format("{0}.OnAutoLoginExpired() relogon {2} times failed,Error:{1}.", ToString(), ex.Message, loginCount));
                    break;
                }

                List<string> subInstruments = new List<string>();
                lock (m_object)
                {
                    foreach (KeyValuePair<string, USeInstrument> item in m_instrumentDic)
                    {
                        subInstruments.Add(item.Key);
                        m_logger.WriteInformation(string.Format("Resubscribe [{0}].", item.Key));
                    }
                }

                if (subInstruments.Count > 0)
                {
                    m_ctpFeed.SubscribeMarketData(subInstruments.ToArray());
                }
                return;

            }
        }

        public override string ToString()
        {
            return "CtpQuoteDriver";
        }

       
    }
}
