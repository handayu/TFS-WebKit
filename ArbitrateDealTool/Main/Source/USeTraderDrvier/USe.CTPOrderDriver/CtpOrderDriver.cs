#region Copyright & Version
//==============================================================================
// 文件名称: CtpOrderDriver.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: CTP交易驱动类。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;

using CTPAPI;
using USe.TradeDriver.Common;
using USe.Common.AppLogger;
using USe.Common;

namespace USe.TradeDriver.Ctp
{
    /// <summary>
    ///  CTP交易驱动类。
    /// </summary>
    public partial class CtpOrderDriver
    {
        #region member
        private const int DEFAULT_CONNECT_TIMEOUT = 5000;     // 默认连接超时时间。
        private const int DEFAULT_QUERY_TIMEOUT = 5000;       // 默认查询超时时间
        private const int QUERY_ACCOUNT_INFO_INTERVAL = 60 * 1000; // 资金账户信息查询间隔。
        private const int LOGIN_REPEAT_COUNT = 3;
        private object m_object = new object();

        private CtpUser m_ctpUser;
        private string m_ctpUserStreamFilePath = null; // 文件流路径
        private string m_address = string.Empty;      // 服务器地址。
        private int m_port = 0;                       // 交易服务器端口。
        private int m_connectTimeOut = 0;             // 服务器连接超时时间
        private int m_queryTimeOut = 0;               // 查询超时时间

        private int m_frontID = 0;                    // 前置编号
        private int m_sessionID = 0;                  // 会话编号  

        private string m_brokerID = string.Empty;//经纪商编号
        private string m_userId = string.Empty;  //用户ID
        private string m_userProductInfo = string.Empty; //用户产品信息

        private CommonIdCreator m_orderRefIDCreator = null; // 下单OrderReference生成对象
        private CommonIdCreator m_requetSeqIDCreator = null; // 请求命令ID生成对象

        private Dictionary<int, USeResetEvent> m_eventDic = null;  // 同步查询Event(注:EventID=0固定为连接使用)
        private CtpDataBuff m_dataBuffer = null;            // 数据缓存
        private System.Threading.Timer m_queryTimer = null;   // 查询手续费保证金定时器
        private System.Threading.Timer m_autoLoginTimer = null;  // 自动登录定时器
        private System.Threading.Timer m_queryAccountTimer = null; // 定时查询用户资金信息
        #endregion // member 

        #region construction
        /// <summary>
        /// 构造CtpOrderDriver实例。
        /// </summary>
        /// <param name="driverType">服务器环境类型。</param>
        /// <param name="address">交易服务器地址。</param>
        /// <param name="port">交易服务器端口。</param>
        public CtpOrderDriver(USeDriverType driverType, string address, int port)
            : this(driverType, address, port, DEFAULT_CONNECT_TIMEOUT, DEFAULT_QUERY_TIMEOUT, string.Empty)
        {

        }

        /// <summary>
        /// 构造CtpOrderDriver实例。
        /// </summary>
        /// <param name="driverType">服务器环境类型。</param>
        /// <param name="address">交易服务器地址。</param>
        /// <param name="port">交易服务器端口。</param>
        public CtpOrderDriver(USeDriverType driverType, string address, int port, string streamFilePath)
            : this(driverType, address, port, DEFAULT_CONNECT_TIMEOUT, DEFAULT_QUERY_TIMEOUT, streamFilePath)
        {
        }

        /// <summary>
        /// 构造CtpOrderDriver实例。
        /// </summary>
        /// <param name="driverType">服务器环境类型。</param>
        /// <param name="address">行情服务器地址。</param>
        /// <param name="port">交易服务器端口。</param>
        /// <param name="connectTimeOut">连接超时时间。</param>
        /// <param name="queryTimeOut">查询超时时间。</param>
        public CtpOrderDriver(USeDriverType driverType, string address, int port, int connectTimeOut, int queryTimeOut, string streamFilePath)
        {
            m_driverType = driverType;
            m_address = address;
            m_port = port;
            m_connectTimeOut = connectTimeOut > 0 ? connectTimeOut : DEFAULT_CONNECT_TIMEOUT;
            m_queryTimeOut = queryTimeOut > 0 ? queryTimeOut : DEFAULT_QUERY_TIMEOUT;
            m_ctpUserStreamFilePath = streamFilePath;

            if (string.IsNullOrEmpty(m_ctpUserStreamFilePath))
            {
                FileInfo fileInfo = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
                string companyName = "USe";
                string appFolder = fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length);
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), companyName, appFolder, "CtpUserStream");
                m_ctpUserStreamFilePath = fileName;
            }

            m_logger = new NullLogger("NullLogger<CtpOrderDriver>");

            Initialize();
        }

        /// <summary>
        /// 构造CtpOrderDriver实例。
        /// </summary>
        /// <param name="section">CtpOrderSection配置节。</param>
        public CtpOrderDriver(CtpOrderSection section)
        {
            CtpOrderDriverElementCollection driverColl = section.CtpOrderDrivers;
            if (driverColl == null || driverColl.Count <= 0)
            {
                throw new Exception("Invalid CtpOrderSection.");
            }

            CtpOrderDriverElement driverElement = driverColl[0];
            Debug.Assert(driverElement != null);
            m_driverType = driverElement.DriverType;
            m_address = driverElement.Address;
            m_port = driverElement.Port;
            m_connectTimeOut = driverElement.ConnectTimeOut > 0 ? driverElement.ConnectTimeOut : DEFAULT_CONNECT_TIMEOUT;
            m_queryTimeOut = driverElement.QueryTimeOut > 0 ? driverElement.QueryTimeOut : DEFAULT_QUERY_TIMEOUT;
            m_ctpUserStreamFilePath = driverElement.StreamPath;

            if (string.IsNullOrEmpty(m_ctpUserStreamFilePath))
            {
                FileInfo fileInfo = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
                string companyName = "USe";
                string appFolder = fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length);
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), companyName, appFolder, "CtpUserStream");
                m_ctpUserStreamFilePath = fileName;
            }

            m_logger = new NullLogger("NullLogger<CtpOrderDriver>");

            Initialize();
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 驱动名称。
        /// </summary>
        public override string DriverName
        {
            get { return "CTP"; }
        }
        #endregion // property

        private void Initialize()
        {
            m_orderRefIDCreator = new CommonIdCreator();
            m_requetSeqIDCreator = new CommonIdCreator();
            m_eventDic = new Dictionary<int, USeResetEvent>();
            m_dataBuffer = new CtpDataBuff(this);

            m_queryTimer = new System.Threading.Timer(OnQueryExpired, false, Timeout.Infinite, Timeout.Infinite);
            m_autoLoginTimer = new System.Threading.Timer(OnAutoLoginExpired, false, Timeout.Infinite, Timeout.Infinite);
            m_queryAccountTimer = new System.Threading.Timer(OnQueryAccountExpired, false, Timeout.Infinite, Timeout.Infinite);
        }


        /// <summary>
        /// 定时查询方法。
        /// </summary>
        /// <param name="state">与超时事件关联的State对象。</param>
        private void OnQueryExpired(object state)
        {
            if (this.DriverState != USeOrderDriverState.Ready && m_ctpUser.IsLogin == false)
            {
                m_queryTimer.Change(1000, Timeout.Infinite);
                return;
            }

            CtpQueryInfo queryInfo = m_dataBuffer.GetNextQueryInfo();

            if (queryInfo == null)
            {
                //所有查询都已查询完毕,等待2秒继续检查
                m_queryTimer.Change(2000, Timeout.Infinite);
                return;
            }

            switch (queryInfo.QueryType)
            {
                case CtpQueryType.Fee:
                    OnQueryFeeExpired(queryInfo as CtpQueryFeeInfo);
                    break;
                case CtpQueryType.Margin:
                    OnQueryMarginExpired(queryInfo as CtpQueryMarginInfo);
                    break;
                case CtpQueryType.Fund:
                    OnQueryFundExpired(queryInfo as CtpQueryFundInfo);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            m_queryTimer.Change(1000, Timeout.Infinite);
        }

        /// <summary>
        /// 定时查询--手续费查询。
        /// </summary>
        /// <param name="queryInfo"></param>
        private void OnQueryFeeExpired(CtpQueryFeeInfo queryInfo)
        {
            try
            {
                InstrumentCommissionRateField field = QueryCommissionRateFieldFromCtp(queryInfo.InstrumentCode);
                m_dataBuffer.UpdateInstrumentFee(field, queryInfo.InstrumentCode);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("OnQueryExpired() query[{0}] fee failed,{1}", queryInfo.InstrumentCode, ex.Message));
                m_logger.WriteError(string.Format("{0}.OnQueryExpired() query[{1}] fee failed,Error:{2}.",
                                ToString(), queryInfo.InstrumentCode, ex.Message));
                return;
            }

            try
            {
                USeInstrument instrument = m_dataBuffer.GetInstrumnetByCode(queryInfo.InstrumentCode);
                List<USeTradeBook> tradeBookList = m_dataBuffer.GetTradeBook(instrument);
                List<USeOrderBook> orderBookList = m_dataBuffer.GetCheckedOrderBook(instrument);

                if (tradeBookList != null && tradeBookList.Count > 0)
                {
                    foreach (USeTradeBook tradeBook in tradeBookList)
                    {
                        FireTradeBookChanged(tradeBook,false);
                    }
                }
                if (orderBookList != null && orderBookList.Count >0)
                {
                    foreach (USeOrderBook orderBook in orderBookList)
                    {
                        FireOrderBookChanged(orderBook);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                m_logger.WriteError(string.Format("{0}.OnQueryFeeExpired() failed,Error:{1}.", ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 定时查询--保证金查询。
        /// </summary>
        /// <param name="queryInfo"></param>
        private void OnQueryMarginExpired(CtpQueryMarginInfo queryInfo)
        {
            try
            {
                InstrumentMarginRateField field = QueryMarginFromCtp(queryInfo.InstrumentCode);
                m_dataBuffer.UpdateInstrumentMagin(field);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("OnQueryExpired() query[{0}] margin failed,{1}", queryInfo.InstrumentCode, ex.Message));
                m_logger.WriteError(string.Format("{0}.OnQueryExpired() query[{1}] margin failed,Error:{2}.",
                                ToString(), queryInfo.InstrumentCode, ex.Message));
            }
        }


        /// <summary>
        /// 定时查询--账户资金查询。
        /// </summary>
        /// <param name="queryInfo"></param>
        private void OnQueryFundExpired(CtpQueryFundInfo queryInfo)
        {
            try
            {
                TradingAccountField field = QueryTradingAccountFromCtp();
                m_dataBuffer.UpdateTradingAccountInfo(field);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("OnQueryFundExpired() query fund info failed,{0}", ex.Message));
                m_logger.WriteError(string.Format("{0}.OnQueryFundExpired() query fund info failed,Error:{1}.",
                                ToString(), ex.Message));
                return;
            }

            try
            {
                USeFund fundInfo = m_dataBuffer.GetFund();

                FireFundChanged(fundInfo);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                m_logger.WriteError(string.Format("{0}.OnQueryFundExpired() failed,Error:{1}.", ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 自动登录。
        /// </summary>
        /// <param name="state"></param>
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
                    m_logger.WriteInformation(string.Format("{0}.OnAutoLoginExpired() relogon {2} times ok,[InvestorID:{1}.]", ToString(), m_investorID,loginCount));
                    return;
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(2000); // 重新登录失败,延时2秒在登录
                    m_logger.WriteError(string.Format("{0}.OnAutoLoginExpired() relogon {2} times failed,Error:{1}.", ToString(), ex.Message,loginCount));

                }
            }
        }

        /// <summary>
        /// 触发定时账户资金查询。
        /// </summary>
        /// <param name="state"></param>
        private void OnQueryAccountExpired(object state)
        {
            if (this.DriverState != USeOrderDriverState.Ready && m_ctpUser.IsLogin == false)
            {
                m_queryAccountTimer.Change(QUERY_ACCOUNT_INFO_INTERVAL, Timeout.Infinite);
                return;
            }

            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());

            m_queryAccountTimer.Change(QUERY_ACCOUNT_INFO_INTERVAL, Timeout.Infinite);
        }
    }
}
