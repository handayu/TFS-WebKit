using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using USe.TradeDriver.Common;
using USe.Common.AppLogger;
using USe.Common;
using USe.Common.Manager;
using System.Collections.Specialized;
using MarketDataStore.Config;
using DataStoreCommon;

namespace MarketDataStore
{
    internal class USeManager
    {

        public event EventHandler<USeNotifyEventArgs> Notify;

        private static USeManager ms_instance = null;
        private bool m_initialized = false;

        private CTPMarketDataReceiver m_marketDataReceiver = null;

        private KLineProcessor m_kLineProcessor = null;
        private FileMarketDataStorage m_marketDataFileStorage = null;
        private List<MQTTMarketDataStoreage> m_mqttMarketDataStoreageList = new List<MQTTMarketDataStoreage>();

        private List<KLineStoreage> m_kLineStoreages = null;

        private ITradeRangeManager m_tradeRangeManager = null;
        private MainContractManager m_mainContractManager = null;
        private USeProductManager m_productManager = null;
        private AlphaDBVistor m_alphaDBVisotr = null;
        private USeTradingInstrumentManager m_instrumentManager = null;
        private TradeCalendarManager m_tradeCalendarManager = null;
        private IAppLogger m_eventLogger = null;

        private List<USeMarket> m_monitorMarketes = null;

        private USeManager()
        {

        }

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

        public DayNightType DayNightType
        {
            get;
            set;
        }
        /// <summary>
        /// 监控市场。
        /// </summary>
        public List<USeMarket> MonitorMarketes
        {
            get { return new List<USeMarket>(m_monitorMarketes); }
        }

        /// <summary>
        /// 行情接收器。
        /// </summary>
        public CTPMarketDataReceiver MarketDataReceiver
        {
            get { return m_marketDataReceiver; }
        }

        /// <summary>
        /// 行情处理器。
        /// </summary>
        public KLineProcessor MarketDataProcessor
        {
            get { return m_kLineProcessor; }
        }

        /// <summary>
        /// 行情文件存储器。
        /// </summary>
        public FileMarketDataStorage MarketDataFileStorage
        {
            get { return m_marketDataFileStorage; }
        }

        /// <summary>
        /// MQTT存储器。
        /// </summary>
        public List<MQTTMarketDataStoreage> MQTTMarketDataStoreageList
        {
            get { return m_mqttMarketDataStoreageList; }
        }

        /// <summary>
        /// K线存储器。
        /// </summary>
        public List<KLineStoreage> KLineStorages
        {
            get { return m_kLineStoreages; }
        }

        /// <summary>
        /// 交易区间管理类。
        /// </summary>
        public ITradeRangeManager TradeRangeManager
        {
            get { return m_tradeRangeManager; }
        }

        public TradeCalendarManager TradeCalendarManager
        {
            get { return m_tradeCalendarManager; }
        }
        /// <summary>
        /// Alpha数据库访问器。
        /// </summary>
        public AlphaDBVistor AlphaDBVistor
        {
            get { return m_alphaDBVisotr; }
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
        /// 初始化。
        /// </summary>
        public void Initialize()
        {
            if (m_initialized)
            {
                throw new ApplicationException(string.Format("USeManager already initialized.", this));
            }

            LoadMonitorMarket();
            CreateTradeCalendarManager();
            CreateTradeRangeManager();
            CreateProductManager();
            CreateAlphaDBVistor();
            CreateInstrumentManager();
            CreateMainContractManager();

            CreateMarketDataFileStorage();
            CreateMQTTMarketDataStorage();

            CreateKLineStorage();
            CreateMarketDataProcessor();
            CreateMarketDataReceiver();
            m_initialized = true;
            FlushAllLoggers();
        }

        /// <summary>
        /// 启动。
        /// </summary>
        public void Start()
        {
            InternalStart();
        }

        private void InternalStart()
        {
            foreach (KLineStoreage klineStore in m_kLineStoreages)
            {
                try
                {
                    klineStore.Start();
                    string text = string.Format("启动K线存储器{0}成功", klineStore.StorageName);
                    m_eventLogger.WriteInformation(text);
                }
                catch (Exception ex)
                {
                    m_eventLogger.WriteError(string.Format("启动K线存储器{0}失败,{1}", klineStore.StorageName, ex.Message));
                    throw new ApplicationException(string.Format("启动K线存储器{0}失败", klineStore.StorageName));
                }
            }

            try
            {
                m_marketDataReceiver.RegisterMarketDataListener(m_kLineProcessor);

                List<IKLineDataListener> listenerList = new List<IKLineDataListener>();
                foreach (KLineStoreage storeage in m_kLineStoreages)
                {
                    listenerList.Add(storeage);
                }

                m_kLineProcessor.Start(listenerList, m_tradeRangeManager, m_mainContractManager, m_productManager, m_alphaDBVisotr, m_instrumentManager, m_eventLogger);
            }
            catch (Exception ex)
            {
                m_eventLogger.WriteError("行情数据处理器启动失败," + ex.Message);
                throw new ApplicationException("行情数据处理器启动失败");
            }

            try
            {
                m_marketDataReceiver.RegisterMarketDataListener(m_marketDataFileStorage);

                m_marketDataFileStorage.Start(m_eventLogger);
            }
            catch (Exception ex)
            {
                m_eventLogger.WriteError("行情数据文件存储启动失败," + ex.Message);
                throw new ApplicationException("行情数据文件存储启动失败");
            }

            try
            {
                if(m_mqttMarketDataStoreageList != null && m_mqttMarketDataStoreageList.Count != 0 )
                {
                    foreach(MQTTMarketDataStoreage mqttStore in m_mqttMarketDataStoreageList)
                    {
                        m_marketDataReceiver.RegisterMarketDataListener(mqttStore);
                        mqttStore.Start(m_eventLogger);
                    }
                }

            }
            catch (Exception ex)
            {
                m_eventLogger.WriteError("MQTT实时行情处理器注册失败," + ex.Message);
                throw new ApplicationException("MQTT实时行情处理器注册失败");
            }

            try
            {
                m_marketDataReceiver.Start(this.MonitorMarketes);
            }
            catch (Exception ex)
            {
                m_eventLogger.WriteError("行情数据接收器启动失败," + ex.Message);
                throw new ApplicationException("行情数据接收器启动失败");
            }

        }

        public void Close()
        {
            InternalStop();

            FlushAllLoggers();
        }

        private void InternalStop()
        {
            if (m_marketDataReceiver != null)
            {
                try
                {
                    m_marketDataReceiver.Stop();
                }
                catch (Exception ex)
                {
                    string text = String.Format("{0} 关闭行情接收器失败, Error: {1}", this, ex.Message);
                    m_eventLogger.WriteWarning(text);
                }
            }

            if (m_kLineProcessor != null)
            {
                try
                {
                    m_kLineProcessor.Stop();
                }
                catch (Exception ex)
                {
                    string text = String.Format("{0} 关闭K线处理器失败, Error: {1}", this, ex.Message);
                    m_eventLogger.WriteWarning(text);
                }
            }

            if (m_marketDataFileStorage != null)
            {
                try
                {
                    m_marketDataFileStorage.Stop();
                }
                catch(Exception ex)
                {
                    string text = String.Format("{0} 关闭行情文件存储器失败, Error: {1}", this, ex.Message);
                    m_eventLogger.WriteWarning(text);
                }
            }


            if (m_mqttMarketDataStoreageList != null && m_mqttMarketDataStoreageList.Count != 0)
            {
                foreach (MQTTMarketDataStoreage mqttStore in m_mqttMarketDataStoreageList)
                {
                    try
                    {
                        mqttStore.Stop();
                    }
                    catch (Exception ex)
                    {
                        string text = String.Format("{0} 关闭MQTTDeve行情发布失败, Error: {1}", this, ex.Message);
                        m_eventLogger.WriteWarning(text);
                    }
                }

            }

            if (m_kLineStoreages != null)
            {
                foreach (KLineStoreage storeage in m_kLineStoreages)
                {
                    try
                    {
                        storeage.Stop();
                    }
                    catch (Exception ex)
                    {
                        string text = String.Format("关闭K线存储器{0}失败,错误：{1}", storeage.StorageName, ex.Message);
                        m_eventLogger.WriteWarning(text);
                    }
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
        }

        private void LoadMonitorMarket()
        {
            string marketConfig = ConfigurationManager.AppSettings["MonitorMarket"];
            try
            {
                List<USeMarket> marketList = new List<USeMarket>();

                string[] items = marketConfig.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);
                foreach(string marketValue in items)
                {
                    USeMarket market = (USeMarket)Enum.Parse(typeof(USeMarket), marketValue);
                    marketList.Add(market);
                }

                if(marketList.Count <=0)
                {
                    throw new Exception("MonitorMarket is empty");
                }

                m_monitorMarketes = marketList;
            }
            catch(Exception ex)
            {
                string text = "Load MonitorMarketConfig failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        private void CreateTradeCalendarManager()
        {
            string dbConnStr = ConfigurationManager.ConnectionStrings["KLineDB"].ConnectionString;
            if (string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentException("Not found KLineDB ConnectionString");
            }
            string alpahDBName = ConfigurationManager.AppSettings["AlphaDBName"];
            if (string.IsNullOrEmpty(alpahDBName))
            {
                throw new ArgumentException("Not foun AlphaDBName config");
            }

            try
            {
                TradeCalendarManager tradeCalendarManager = new TradeCalendarManager(dbConnStr,alpahDBName);
                tradeCalendarManager.Initialize(DateTime.Today, DateTime.Today.AddDays(15));
                m_tradeCalendarManager = tradeCalendarManager;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("初始化交易日历失败," + ex.Message);
            }
        }

        /// <summary>
        /// 创建交易区间管理类。
        /// </summary>
        private void CreateTradeRangeManager()
        {
            string dbConnStr = ConfigurationManager.ConnectionStrings["KLineDB"].ConnectionString;
            if (string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentException("Not found KLineDB ConnectionString");
            }
            string alpahDBName = ConfigurationManager.AppSettings["AlphaDBName"];
            if (string.IsNullOrEmpty(alpahDBName))
            {
                throw new ArgumentException("Not foun AlphaDBName config");
            }

            try
            {
                TradeRangeManager tradeRangeManager = new TradeRangeManager(dbConnStr,alpahDBName);
                tradeRangeManager.Initialize();
                m_tradeRangeManager = tradeRangeManager;

                string text = String.Format("{0} Create {1} OK.", this, tradeRangeManager);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                string text = "Create TradeRangeManager object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建品种管理类。
        /// </summary>
        private void CreateProductManager()
        {
            string dbConnStr = ConfigurationManager.ConnectionStrings["KLineDB"].ConnectionString;
            if (string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentException("Not found KLineDB ConnectionString");
            }
            string alpahDBName = ConfigurationManager.AppSettings["AlphaDBName"];
            if(string.IsNullOrEmpty(alpahDBName))
            {
                throw new ArgumentException("Not foun AlphaDBName config");
            }

            try
            {
                USeProductManager manager = new USeProductManager(dbConnStr,alpahDBName);
                manager.Initialize();

                m_productManager = manager;

                string text = String.Format("{0} Create {1} OK.", this, manager);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                string text = "Create ProductManager object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建Alpha数据库访问器。
        /// </summary>
        private void CreateAlphaDBVistor()
        {
            string dbConnStr = ConfigurationManager.ConnectionStrings["KLineDB"].ConnectionString;
            if (string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentException("Not found KLineDB ConnectionString");
            }
            string alpahDBName = ConfigurationManager.AppSettings["AlphaDBName"];
            if (string.IsNullOrEmpty(alpahDBName))
            {
                throw new ArgumentException("Not foun AlphaDBName config");
            }

            try
            {
                AlphaDBVistor alphaDBVistor = new AlphaDBVistor(dbConnStr, alpahDBName);

                m_alphaDBVisotr = alphaDBVistor;
                string text = String.Format("{0} Create {1} OK.", this, alphaDBVistor);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                string text = "Create AlphaDBVistor object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建交易合约管理类。
        /// </summary>
        private void CreateInstrumentManager()
        {
            string dbConnStr = ConfigurationManager.ConnectionStrings["KLineDB"].ConnectionString;
            if (string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentException("Not found KLineDB ConnectionString");
            }
            string alpahDBName = ConfigurationManager.AppSettings["AlphaDBName"];
            if (string.IsNullOrEmpty(alpahDBName))
            {
                throw new ArgumentException("Not foun AlphaDBName config");
            }

            try
            {
                USeTradingInstrumentManager manager = new USeTradingInstrumentManager(dbConnStr, alpahDBName);
                //[yangming]需要调整为交易日时间
                manager.Initialize(DateTime.Today);

                m_instrumentManager = manager;
                string text = String.Format("{0} Create {1} OK.", this, manager);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                string text = "Create InstrumentManager object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建主力合约管理类。
        /// </summary>
        private void CreateMainContractManager()
        {
            string dbConnStr = ConfigurationManager.ConnectionStrings["KLineDB"].ConnectionString;
            if (string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentException("Not found KLineDB ConnectionString");
            }
            string alphaDBName = ConfigurationManager.AppSettings["AlphaDBName"];
            if (string.IsNullOrEmpty(alphaDBName))
            {
                throw new ArgumentException("Not found alphaDBName config");
            }
            try
            {
                MainContractManager manager = new MainContractManager(dbConnStr,alphaDBName);
                manager.Initialize();
                m_mainContractManager = manager;

                string text = String.Format("{0} Create {1} OK.", this, manager);
                m_eventLogger.WriteInformation(text);

                Debug.Assert(manager != null);
                Dictionary<string, USeInstrument> mainContractDic = manager.MainContractDictionary;
                foreach(KeyValuePair<string,USeInstrument> kv in mainContractDic)
                {
                    m_eventLogger.WriteInformation(kv.Key + "--" + kv.Value.InstrumentCode);
                }

            }
            catch (Exception ex)
            {
                string text = "Create MainContractManager object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }
     
        private void CreateKLineStorage()
        {
            KLineStorageSection section = ConfigurationManager.GetSection("KLineStorage") as KLineStorageSection;
            if(section == null)
            {
                throw new ApplicationException("Not found KLineStoreage config");
            }

            m_kLineStoreages = new List<KLineStoreage>();
            if (section.FileStorages != null && section.FileStorages.Count > 0)
            {
                foreach (FileKLineStorageElement element in section.FileStorages)
                {
                    if (element.Enable)
                    {
                        KLineStoreage storage = CreateFileKLineStorage(element);
                        m_kLineStoreages.Add(storage);
                    }
                }
            }

            if (section.MySqlStorages != null && section.MySqlStorages.Count >0)
            {
                foreach (MySqlKLineStorageElement element in section.MySqlStorages)
                {
                    if (element.Enable)
                    {
                        KLineStoreage storage = CreateMySqlKLineStoreage(element);
                        m_kLineStoreages.Add(storage);
                    }
                }
            }

            if (section.RabbitMQStorages != null && section.RabbitMQStorages.Count > 0)
            {
                foreach (RabbitMQKLineStorageElement element in section.RabbitMQStorages)
                {
                    if (element.Enable)
                    {
                        KLineStoreage storage = CreateRabbitMQKLineStorage(element);
                        m_kLineStoreages.Add(storage);
                    }
                }
            }

            if (section.RocketMQStorages != null && section.RocketMQStorages.Count > 0)
            {
                foreach (RocketMQKLineStorageElement element in section.RocketMQStorages)
                {
                    if (element.Enable)
                    {
                        KLineStoreage storage = CreateRocketMQKLineStorage(element);
                        m_kLineStoreages.Add(storage);
                    }
                }
            }
        }

        /// <summary>
        /// 创建MySql K线存储器。
        /// </summary>
        private KLineStoreage CreateMySqlKLineStoreage(MySqlKLineStorageElement element)
        {
            if (string.IsNullOrEmpty(element.ConnectionString))
            {
                throw new ArgumentException("Not found KLineDB ConnectionString");
            }
            if (string.IsNullOrEmpty(element.DBName))
            {
                throw new ArgumentException("Not found dbName");
            }
            try
            {
                MySqlKLineStoreage storeage = new MySqlKLineStoreage(element.Name, element.ConnectionString, element.DBName);
                storeage.Notify += OnNotifyEventArrived;

                string text = String.Format("{0} Create {1} OK.", this, storeage);
                m_eventLogger.WriteInformation(text);

                return storeage;
            }
            catch (Exception ex)
            {
                string text = "Create MySqlKLineStorage object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建RocketMQ K线存储器。
        /// </summary>
        private KLineStoreage CreateRocketMQKLineStorage(RocketMQKLineStorageElement element)
        {
            try
            {
                if (string.IsNullOrEmpty(element.SendUrl))
                {
                    throw new ArgumentException("Not found KLineRocketMQStore/sendUrl");
                }

                RocketMQKLineStoreage store = new RocketMQKLineStoreage(element.Name,element.SendUrl);
                store.Notify += OnNotifyEventArrived;

                string text = String.Format("{0} Create {1} OK.", this, store);
                m_eventLogger.WriteInformation(text);

                return store;
            }
            catch (Exception ex)
            {
                string text = "Create RocketMQKLineStorage object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建RabbitMQ K线存储器。
        /// </summary>
        private KLineStoreage CreateRabbitMQKLineStorage(RabbitMQKLineStorageElement element)
        {
            try
            {
                if (string.IsNullOrEmpty(element.Address))
                {
                    throw new ArgumentException("Not found KLineRabbitMQStore/address");
                }

                RabbitMQKLineStoreage storeage = new RabbitMQKLineStoreage(element.Name, element.Address);
                storeage.Notify += OnNotifyEventArrived;

                string text = String.Format("{0} Create {1} OK.", this, storeage);
                m_eventLogger.WriteInformation(text);

                return storeage;
            }
            catch (Exception ex)
            {
                string text = "Create RabbitMQKLineStorage object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建文件 K线存储器。
        /// </summary>
        private KLineStoreage CreateFileKLineStorage(FileKLineStorageElement element)
        {
            try
            {
                if (string.IsNullOrEmpty(element.SavePath))
                {
                    throw new ArgumentException("Not found KLineFileStore/savePath");
                }
                Debug.Assert(m_tradeCalendarManager != null);

                FileKLineStoreage storeage = new FileKLineStoreage(element.Name, element.SavePath,m_tradeCalendarManager);
                storeage.Notify += OnNotifyEventArrived;
                

                string text = String.Format("{0} Create {1} OK.", this, storeage);
                m_eventLogger.WriteInformation(text);

                return storeage;
            }
            catch (Exception ex)
            {
                string text = "Create FileKLineStoreage object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建行情数据接收器。
        /// </summary>
        private void CreateMarketDataReceiver()
        {
            try
            {
                //读取配置文件

                CtpReceiverSection config = CtpReceiverSection.GetSection();

                string dbConnStr = ConfigurationManager.ConnectionStrings["KLineDB"].ConnectionString;
                string alphaDBName = ConfigurationManager.AppSettings["AlphaDBName"];
                CTPMarketDataReceiver receiver = new CTPMarketDataReceiver(config,dbConnStr,alphaDBName);
                receiver.Notify += OnNotifyEventArrived;
                m_marketDataReceiver = receiver;

                string text = String.Format("{0} Create {1} OK.", this, receiver);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                string text = "Create MarketDataReceiver object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建行情数据处理器。
        /// </summary>
        private void CreateMarketDataProcessor()
        {
            try
            {
                TimeSpan dayKLinePublishInterval = TimeSpan.Parse(ConfigurationManager.AppSettings["DayKLinePublishInterval"]);
                if(dayKLinePublishInterval < TimeSpan.Zero)
                {
                    throw new ApplicationException("非法的日线发布间隔");
                }

                KLineProcessor processor = new KLineProcessor(dayKLinePublishInterval);
                processor.Notify += OnNotifyEventArrived;
                m_kLineProcessor = processor;

                string text = String.Format("{0} Create {1} OK.", this, processor);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                string text = "Create MarketDataReceiver object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建行情数据处理器。
        /// </summary>
        private void CreateMarketDataFileStorage()
        {
            try
            {
                NameValueCollection settings = ConfigurationManager.GetSection("MarketDataFileStore") as NameValueCollection;

                string savePath = settings["savePath"];
                if (string.IsNullOrEmpty(savePath))
                {
                    throw new ArgumentException("Not found KLineFileStore/savePath");
                }
                Debug.Assert(m_tradeCalendarManager != null);
                FileMarketDataStorage marketDataFileStorage = new FileMarketDataStorage(savePath,m_tradeCalendarManager);
                marketDataFileStorage.Notify += OnNotifyEventArrived;

                m_marketDataFileStorage = marketDataFileStorage;

                string text = String.Format("{0} Create {1} OK.", this, marketDataFileStorage);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                string text = "Create MarketDataFileStorage object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }


        /// <summary>
        /// 创建MQTT行情数据发布器。
        /// </summary>
        private void CreateMQTTMarketDataStorage()
        {
            try
            {
                NameValueCollection settings = ConfigurationManager.GetSection("MQTTMarketDataStorage") as NameValueCollection;

                if (settings == null) return;
                for (int i = 0; i < settings.Count; i++)
                {
                    string assress_deve = settings[i];
                    if (string.IsNullOrEmpty(assress_deve))
                    {
                        throw new ArgumentException("Not found MQTTAdderss_deve");
                    }
                    string clientID = string.Format("Server_{0}", i+5);
                    MQTTMarketDataStoreage marketDataMQTTStorage = new MQTTMarketDataStoreage(assress_deve, clientID);
                    marketDataMQTTStorage.Notify += OnNotifyEventArrived;

                    m_mqttMarketDataStoreageList.Add(marketDataMQTTStorage);

                    string text = String.Format("{0} Create {1} OK.", this, marketDataMQTTStorage);
                    m_eventLogger.WriteInformation(text);
                }
            }
            catch (Exception ex)
            {
                string text = "Create MQTTMarketDataStoreage object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        private void OnNotifyEventArrived(object sender, USeNotifyEventArgs e)
        {
            string text = String.Format("Notify[{0}]: {1}", e.Level.ToString()[0], e.Message);
            Debug.WriteLine("==>" + text);

            SafeRaiseNotifyEvent(sender, e);
        }

        private void SafeRaiseNotifyEvent(object sender, USeNotifyEventArgs e)
        {
            EventHandler<USeNotifyEventArgs> handler = this.Notify;
            if (handler != null)
            {
                try
                {
                    handler(sender, e);
                }
                catch (Exception ex)
                {
                    string text = String.Format("{0} raise Notify event failed, Error: {1}", this, ex.Message);
                    m_eventLogger.WriteWarning(text);
                }
            }
        }

    }
}
