using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using USe.TradeDriver.Ctp;
using USe.TradeDriver.Common;
using USe.Common.AppLogger;
using System.Windows.Forms;

namespace USeFuturesSpirit
{
    internal class USeManager
    {
        #region member
        private static USeManager ms_instance = null;
        private bool m_initialized = false;
        private bool m_checkErrorOrderBook = false;  // 异常单是否处理

        private InvestorAccount m_loginUser = null;

        private USeOrderDriver m_orderDriver = null;
        private USeQuoteDriver m_quoteDriver = null;

        private AlarmManager m_alarmManager = null;
        private AutoTraderManager m_autoTraderManager = null;
        private ArbitrageDataSaver m_dataSaver = null;
        private USeDataAccessor m_dataAccessor = null;
        private SystemConfigManager m_systemConfigManager = null;
        private FundCalculator m_fundCalculator = null;

        private IAppLogger m_eventLogger = null;
        private IAppLogger m_commandLogger = null;

        private TestTraderDriverSimulateForm m_testTradeDriverForm = null;

        #endregion

        #region construction
        private USeManager()
        {

        }
        #endregion

        #region Singleton实例
        /// <summary>
        /// 单件实例。
        /// </summary>
        public static USeManager Instance
        {
            get
            {
                if (ms_instance == null)
                {
                    ms_instance = new USeManager();
                }

                return ms_instance;
            }
        }
        #endregion

        #region property
        /// <summary>
        /// 当前登录账户。
        /// </summary>
        public InvestorAccount LoginUser
        {
            get { return m_loginUser; }

            //set { m_loginUser = value; }
        }

        /// <summary>
        /// 下单驱动。
        /// </summary>
        public USeOrderDriver OrderDriver
        {
            get { return m_orderDriver; }
        }

        /// <summary>
        /// 行情驱动。
        /// </summary>
        public USeQuoteDriver QuoteDriver
        {
            get { return m_quoteDriver; }
        }

        /// <summary>
        /// 预警管理。
        /// </summary>
        public AlarmManager AlarmManager
        {
            get { return m_alarmManager; }
        }

        /// <summary>
        /// 系统配置。
        /// </summary>
        public SystemConfigManager SystemConfigManager
        {
            get { return m_systemConfigManager; }
        }

        /// <summary>
        /// 数据访问器。
        /// </summary>
        public USeDataAccessor DataAccessor
        {
            get { return m_dataAccessor; }
        }

        /// <summary>
        /// 数据保存者。
        /// </summary>
        public ArbitrageDataSaver DataSaver
        {
            get { return m_dataSaver; }
        }

        /// <summary>
        /// 自动下单机管理类。
        /// </summary>
        public AutoTraderManager AutoTraderManager
        {
            get { return m_autoTraderManager; }
        }

        /// <summary>
        /// 资金计算器。
        /// </summary>
        public FundCalculator FundCalculator
        {
            get { return m_fundCalculator; }
        }

        /// <summary>
        /// 下单板窗口。
        /// </summary>
        public SimpleOrderForm SimpleOrderForm
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public TestTraderDriverSimulateForm TestTradeDriverSimulateForm
        {
            get
            {
                if (m_testTradeDriverForm == null)
                {
                    m_testTradeDriverForm = new TestTraderDriverSimulateForm();
                }
                return m_testTradeDriverForm;
            }
        }

        /// <summary>
        /// 事件Logger。
        /// </summary>
        public IAppLogger EventLogger
        {
            get { return m_eventLogger; }
            set { m_eventLogger = value; }
        }

        /// <summary>
        /// 命令Logger。
        /// </summary>
        public IAppLogger CommandLogger
        {
            get { return m_commandLogger; }
        }
        #endregion

        /// <summary>
		/// 返回表示当前NetBidManager类对象的字符串。
		/// </summary>
		/// <returns>表示当前NetBidManager类对象的字符串。</returns>
		public override string ToString()
        {
            return GetType().Name;
        }

        #region 初始化/关闭/设置方法
        public void Initialize()
        {
            if (m_initialized)
            {
                throw new ApplicationException(string.Format("USeManager already initialized.", this));
            }

            LoadConfig();

            CreateAlarmManager();
            CreateUSeDataAccessor();
            CreateDataSaver();
            CreateSystemConfigManager();
            CreateAutoTraderManager();
            CreateFundCalculator();

            m_initialized = true;

            FlushAllLoggers();
        }

        /// <summary>
        /// 预启动。
        /// </summary>
        /// <param name="orderDriver"></param>
        /// <param name="quoteDriver"></param>
        public void PreStart(USeOrderDriver orderDriver, USeQuoteDriver quoteDriver)
        {
            if (orderDriver.DriverState != USeOrderDriverState.Ready)
            {
                throw new Exception("交易服务未登录");
            }

            if (quoteDriver.DriverState != USeQuoteDriverState.Ready)
            {
                throw new Exception("行情服务未登录");
            }
            m_loginUser = new InvestorAccount(orderDriver.BrokerId, orderDriver.Account, orderDriver.Password);

            m_orderDriver = orderDriver;
            m_quoteDriver = quoteDriver;

            m_orderDriver.SetAppLogger(m_eventLogger);
        }

        public bool ProcessBeforStartWork()
        {
            if(PorcessErrorOrderBook() == false)
            {
                return false;
            }
            if(ProcessExpireCombineInstrument() == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 处理过期组合合约。
        /// </summary>
        /// <returns></returns>
        private bool ProcessExpireCombineInstrument()
        {
            List<ArbitrageCombineInstrument> arbitrageInstrumentList = null;

            try
            {
                arbitrageInstrumentList = m_dataAccessor.GetCombineInstruments(m_loginUser.BrokerId, m_loginUser.Account);
            }
            catch (Exception ex)
            {
                throw new Exception("从文件获取套利组合合约异常：" + ex.Message);
            }

            bool processFlag = false;
            for (int i = 0; i < arbitrageInstrumentList.Count; i++)
            {
                USeInstrument firstInstrument = arbitrageInstrumentList[i].FirstInstrument;
                USeInstrument secondInstrument = arbitrageInstrumentList[i].SecondInstrument;

                List<USeInstrumentDetail> instrumentDetailList = USeManager.Instance.OrderDriver.QueryInstrumentDetail(arbitrageInstrumentList[i].ProductID);

                USeInstrumentDetail insDetailFir = (from d in instrumentDetailList
                                                    where d.Instrument.InstrumentCode == firstInstrument.InstrumentCode
                                                    select d).FirstOrDefault();

                USeInstrumentDetail insDetailSec = (from d in instrumentDetailList
                                                    where d.Instrument.InstrumentCode == secondInstrument.InstrumentCode
                                                    select d).FirstOrDefault();
                if (insDetailFir != null && insDetailSec != null)
                {
                    continue;
                }
                else
                {
                    arbitrageInstrumentList.Remove(arbitrageInstrumentList[i]);
                    processFlag = true;
                }
            }

            if (processFlag == true)
            {
                m_dataAccessor.SaveCombineInstruments(m_loginUser.BrokerId, m_loginUser.Account, arbitrageInstrumentList);
            }

            return true;
        }

        /// <summary>
        /// 处理异常委托单。
        /// </summary>
        /// <returns></returns>
        private bool PorcessErrorOrderBook()
        {
            //m_checkErrorOrderBook = true;
            //return true;

            string text = string.Empty;
            try
            {
                //1.读取所有未完成套利单信息
                List<USeArbitrageOrder> arbitrageOrderList = m_dataAccessor.GetUnfinishArbitrageOrders(m_loginUser.BrokerId, m_loginUser.Account);
                text = string.Format("读取[{0}]所有未完成套利单信息成功,共计{1}个套利单", m_loginUser, arbitrageOrderList.Count);
                m_eventLogger.WriteInformation(text);

                //2.检查是否有异常套利单
                Dictionary<Guid, ErrorArbitrageOrder> errorArbitrageOrderDic = new Dictionary<Guid, ErrorArbitrageOrder>();
                List<ErrorUSeOrderBook> errorOrderBookList = new List<ErrorUSeOrderBook>();

                foreach (USeArbitrageOrder arbitrageOrder in arbitrageOrderList)
                {
                    ErrorArbitrageOrder errorArbitrageOrder = CheckErrorArbitrageOrder(arbitrageOrder);
                    if (errorArbitrageOrder.HasError)
                    {
                        errorArbitrageOrderDic.Add(arbitrageOrder.TraderIdentify, errorArbitrageOrder);
                        errorOrderBookList.AddRange(errorArbitrageOrder.ErrorOrderBooks);
                    }
                    if (errorArbitrageOrder.HasChanged)
                    {
                        //有变更先记录到文件
                        m_dataAccessor.SaveUSeArbitrageOrder(errorArbitrageOrder.ArbitrageOrder);
                    }
                }

                text = string.Format("[{0}]有{1}个异常套利单共计{2}条委托回报需人工介入",
                    m_loginUser, errorArbitrageOrderDic.Count, errorOrderBookList.Count);
                m_eventLogger.WriteError(text);

                //3.人工处理异常套利单
                if (errorOrderBookList.Count > 0)
                {
                    ErrorOrderBookProcessForm errorOrderBookForm = new ErrorOrderBookProcessForm(errorOrderBookList);
                    if (DialogResult.Yes != errorOrderBookForm.ShowDialog())
                    {
                        return false;
                    }

                    List<ErrorUSeOrderBook> checkBookList = errorOrderBookForm.Result;
                    Debug.Assert(checkBookList.Count == errorOrderBookList.Count);

                    foreach (ErrorUSeOrderBook checkOrderBook in checkBookList)
                    {
                        Debug.Assert(checkOrderBook.OrderBook.IsFinish);
                        ErrorArbitrageOrder errorArbitrageOrder = null;
                        if (errorArbitrageOrderDic.TryGetValue(checkOrderBook.TradeIdentify, out errorArbitrageOrder) == false)
                        {
                            Debug.Assert(false);
                            continue;
                        }
                        OrderBookUpdateResult updateResult = errorArbitrageOrder.ArbitrageOrder.UpdateOrderBook(checkOrderBook.OrderBook);
                        Debug.Assert(updateResult != null);
                        if (updateResult != null)
                        {
                            updateResult.Task.UpdateTaskState();
                            errorArbitrageOrder.ArbitrageOrder.UpdataArbitrageOrderState();
                        }
                    }

                    foreach (ErrorArbitrageOrder errorArbitageOrder in errorArbitrageOrderDic.Values)
                    {
                        m_dataAccessor.SaveUSeArbitrageOrder(errorArbitageOrder.ArbitrageOrder);
                    }
                }

                m_checkErrorOrderBook = true;
                return true;
            }
            catch (Exception ex)
            {
                text = "异常单处理失败," + ex.Message;
                m_eventLogger.WriteError(text);
                throw new ApplicationException(text);
            }
        }

        /// <summary>
        /// 启动。
        /// </summary>
        public void Start()
        {
            if (m_orderDriver == null || m_orderDriver.DriverState != USeOrderDriverState.Ready)
            {
                throw new Exception("交易服务未登录");
            }

            if (m_quoteDriver == null || m_quoteDriver.DriverState != USeQuoteDriverState.Ready)
            {
                throw new Exception("行情服务未登录");
            }

            if (m_checkErrorOrderBook == false)
            {
                throw new Exception("异常单未处理");
            }

            InternalStart(); // 启动基础部件
        }

        public void Close()
        {
            InternalStop();

            FlushAllLoggers();
        }

        private void InternalStart()
        {
            try
            {
                m_dataSaver.Start();
            }
            catch (Exception ex)
            {
                m_eventLogger.WriteError("数据保存器启动失败," + ex.Message);
                throw new ApplicationException("数据保存器启动失败");
            }

            string text = string.Empty;
            //1.读取所有未完成套利单信息
            List<USeArbitrageOrder> arbitrageOrderList = m_dataAccessor.GetUnfinishArbitrageOrders(m_loginUser.BrokerId, m_loginUser.Account);
            text = string.Format("读取[{0}]所有未完成套利单信息成功,共计{1}个套利单", m_loginUser, arbitrageOrderList.Count);
            m_eventLogger.WriteInformation(text);

            //2.检查是否有异常套利单
            //[yangming] 测试环境暂时不校验

            if ((m_orderDriver is USe.TradeDriver.Test.USeTestOrderDriver) == false)
            {
                foreach (USeArbitrageOrder arbitrageOrder in arbitrageOrderList)
                {
                    ErrorArbitrageOrder errorArbitrageOrder = CheckErrorArbitrageOrder(arbitrageOrder);
                    if (errorArbitrageOrder.HasError)
                    {
                        text = string.Format("{0}有未处理异常委托单", arbitrageOrder);
                        m_eventLogger.WriteError(text);
                        throw new Exception(text);
                    }
                    if (errorArbitrageOrder.HasChanged)
                    {
                        m_dataAccessor.SaveUSeArbitrageOrder(arbitrageOrder);
                    }
                }
            }

            //3.初始化自动下单机管理类
            Debug.Assert(m_orderDriver != null);
            Debug.Assert(m_quoteDriver != null);
            Debug.Assert(m_alarmManager != null);
            m_autoTraderManager.Initialize(arbitrageOrderList, m_orderDriver, m_quoteDriver, m_alarmManager, m_systemConfigManager);

            m_fundCalculator.Initialize(m_orderDriver, m_quoteDriver);
            m_fundCalculator.Start();
        }

        /// <summary>
        /// 检查套利单是否有异常委托。
        /// </summary>
        /// <remarks>
        /// 1.非今日的历史委托回报未结束，无法更新，列入异常套利单
        /// </remarks>
        private ErrorArbitrageOrder CheckErrorArbitrageOrder(USeArbitrageOrder arbitrageOrder)
        {
            ErrorArbitrageOrder errorArbitrageOrder = new ErrorArbitrageOrder();
            errorArbitrageOrder.ArbitrageOrder = arbitrageOrder;

            List<ErrorUSeOrderBook> errorBookList = new List<ErrorUSeOrderBook>();

            if (arbitrageOrder.HasUnFinishOrderBook == false)
            {
                return errorArbitrageOrder;
            }

            List<USeOrderBook> unFinishOrderBookList = arbitrageOrder.GetAllUnfinishOrderBooks();
            Debug.Assert(unFinishOrderBookList != null && unFinishOrderBookList.Count > 0);

            foreach (USeOrderBook orderBook in unFinishOrderBookList)
            {
                USeOrderBook newOrderBook = m_orderDriver.QueryOrderBook(orderBook.OrderNum);
                if (newOrderBook != null)
                {
                    OrderBookUpdateResult updateResult = arbitrageOrder.UpdateOrderBook(newOrderBook);
                    updateResult.Task.UpdateTaskState();
                    arbitrageOrder.UpdataArbitrageOrderState();
                    errorArbitrageOrder.HasChanged = true;
                }
                else
                {
                    ErrorUSeOrderBook errorOrderBook = new ErrorUSeOrderBook() {
                        TradeIdentify = arbitrageOrder.TraderIdentify,
                        Alias = arbitrageOrder.Alias,
                        OrderBook = orderBook.Clone()
                    };
                    errorBookList.Add(errorOrderBook);
                }
            }

            errorArbitrageOrder.ErrorOrderBooks = errorBookList;

            return errorArbitrageOrder;
        }

        private void InternalStop()
        {
            if (m_fundCalculator != null)
            {
                try
                {
                    m_fundCalculator.Stop();
                }
                catch (Exception ex)
                {
                    string text = String.Format("{0} Close fundCalculator failed, Error: {1}", this, ex.Message);
                    m_eventLogger.WriteWarning(text);
                }
            }

            if (m_orderDriver != null ) 
            {
                try
                {
                    m_orderDriver.Logout();
                    m_orderDriver.DisConnectServer();
                    // 1.登出
                    // 2.关闭
                    string text = String.Format("{0} Close OrderDriver OK.", this);
                    m_eventLogger.WriteInformation(text);
                }
                catch (Exception ex)
                {
                    string text = String.Format("{0} Close OrderDriver failed, Error: {1}", this, ex.Message);
                    m_eventLogger.WriteWarning(text);
                }
            }

            if (m_quoteDriver != null)
            {
                try
                {
                    // 1.登出
                    m_quoteDriver.Logout();
                    // 2.关闭
                    m_quoteDriver.DisConnectServer();
                    string text = String.Format("{0} Close QuoteDriver OK.", this);
                    m_eventLogger.WriteInformation(text);
                }
                catch (Exception ex)
                {
                    string text = String.Format("{0} Close QuoteDriver failed, Error: {1}", this, ex.Message);
                    m_eventLogger.WriteWarning(text);
                }
            }

            if(m_dataSaver != null)
            {
                try
                {
                    m_dataSaver.Stop();
                }
                catch(Exception ex)
                {
                    string text = String.Format("{0} Close ArbitrageDataSaver failed, Error: {1}", this, ex.Message);
                    m_eventLogger.WriteWarning(text);
                }
            }
        }

        public void FlushAllLoggers()
        {
            if (m_eventLogger != null)
            {
                try
                {
                    m_eventLogger.Flush();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Call m_eventLogger.Flush() failed, Error: " + ex.Message);
                }
            }
            if (m_commandLogger != null)
            {
                try
                {
                    m_commandLogger.Flush();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Call m_commandLogger.Flush() failed, Error: " + ex.Message);
                }
            }
        }
        #endregion

        #region 配置访问方法
        /// <summary>
        /// 加载配置。
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                m_commandLogger = AppLogger.CreateInstance("CommandLogger");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Create CommandLogger failed, " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 创建数据保存器。
        /// </summary>
        private void CreateDataSaver()
        {
            try
            {
                ArbitrageDataSaver dataSaver = new ArbitrageDataSaver();
                Debug.Assert(m_dataAccessor != null);
                dataSaver.SetEventLogger(m_eventLogger);
                dataSaver.SetDataAccesssor(m_dataAccessor);

                m_dataSaver = dataSaver;
                string text = String.Format("{0} Create {1} OK.", this, dataSaver);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                string text = "Create ArbitrageDataSaver object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        private void CreateAlarmManager()
        {
            try
            {
                AlarmManager alarmManager = new AlarmManager();
                m_alarmManager = alarmManager;
            }
            catch(Exception ex)
            {
                string text = "Create AlarmManager object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }

        }
        /// <summary>
        /// 创建数据访问器。
        /// </summary>
        private void CreateUSeDataAccessor()
        {
            try
            {
                USeDataAccessor dataAccessor = new USeDataAccessor();
                dataAccessor.Initialize();
                m_dataAccessor = dataAccessor;
                string text = String.Format("{0} Create {1} OK.", this, dataAccessor);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                string text = "Create USeDataAccessor object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建数据访问器。
        /// </summary>
        private void CreateSystemConfigManager()
        {
            try
            {
                SystemConfigManager configManager = new SystemConfigManager();
                configManager.SetDataAccessor(m_dataAccessor);
                configManager.Initialize();

                m_systemConfigManager = configManager;
                string text = String.Format("{0} Create {1} OK.", this, configManager);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                string text = "Create USeDataAccessor object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建自动下单机管理类。
        /// </summary>
        private void CreateAutoTraderManager()
        {
            try
            {
                AutoTraderManager traderManager = new AutoTraderManager();
                m_autoTraderManager = traderManager;
                m_autoTraderManager.OnArbitrageOrderChanged += M_autoTraderManager_OnArbitrageOrderChanged;
                string text = String.Format("{0} Create {1} OK.", this, traderManager);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                string text = "Create AutoTraderManager object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建资金计算器。
        /// </summary>
        private void CreateFundCalculator()
        {
            try
            {
                FundCalculator fundCalculator = new FundCalculator();
                m_fundCalculator = fundCalculator;
                string text = String.Format("{0} Create {1} OK.", this, fundCalculator);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                string text = "Create FundCalculator object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        private void M_autoTraderManager_OnArbitrageOrderChanged(Guid tradeIdentify)
        {
            try
            {
                AutoTrader trader = m_autoTraderManager.GetAutoTrader(tradeIdentify);
                USeArbitrageOrder arbitrageOrder = trader.GetArbitrageOrder();
                m_dataSaver.AddSaveTask(arbitrageOrder);
            }
            catch(Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }
        #endregion
    }
}
