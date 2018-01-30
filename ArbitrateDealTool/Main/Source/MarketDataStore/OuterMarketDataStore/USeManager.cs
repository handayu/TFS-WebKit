using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using USe.Common.AppLogger;
using USe.Common;
using USe.Common.Manager;
using System.Collections.Specialized;
using USe.TradeDriver.Common;

namespace OuterMarketDataStore
{
    internal class USeManager
    {

        public event EventHandler<USeNotifyEventArgs> Notify;

        public delegate void OutLMEMarketDataReceiveHandel(USeMarketData marketData);
        public event OutLMEMarketDataReceiveHandel OutLMEMarketDataReceiveEvent;

        private static USeManager ms_instance = null;
        private bool m_initialized = false;

        private HttpServerDataReceiver m_httpDataReceiver = null;

        private List<MQTTMarketDataStoreage> m_mqttMarketDataStoreageList = null;
        private List<MySqlKLineStoreage> m_mySqlStoreageList = null;


        private IAppLogger m_eventLogger = null;

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

        /// <summary>
        /// 行情接收器。
        /// </summary>
        public HttpServerDataReceiver HttpDataReceiver
        {
            get { return m_httpDataReceiver; }
        }

        /// <summary>
        /// MQTT存储器。
        /// </summary>
        public List<MQTTMarketDataStoreage> MQTTMArketDataStoreageList
        {
            get { return m_mqttMarketDataStoreageList; }
        }


        /// <summary>
        /// MySql存储器。
        /// </summary>
        public List<MySqlKLineStoreage> MySqlStoreageList
        {
            get { return m_mySqlStoreageList; }
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

            //IAppLogger m_eventLogger = AppLogger.SingleInstance;
            //Debug.Assert(m_eventLogger != null);

            //m_eventLogger.LineFeed();



            m_mqttMarketDataStoreageList = new List<MQTTMarketDataStoreage>();
            m_mySqlStoreageList = new List<MySqlKLineStoreage>();

            CreateMQTTMarketDataStorage();
            CreateMySqlKLineStoreage();
            CreateHttpDataReceiver();

            m_httpDataReceiver.OutLMEMarketDataReceiveEvent += M_httpDataReceiver_OutLMEMarketDataReceiveEvent;

            m_initialized = true;
            FlushAllLoggers();
        }

        private void M_httpDataReceiver_OutLMEMarketDataReceiveEvent(USeMarketData marketData)
        {
            SafeOuterMarketDataEvent(marketData);
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
            try
            {
                if (m_mqttMarketDataStoreageList.Count == 0)
                {
                    throw new Exception("MQTT外盘接收器列表为空");
                }

                foreach (MQTTMarketDataStoreage mqttStore in m_mqttMarketDataStoreageList)
                {
                    m_httpDataReceiver.RegisterHttpMarketDataListener(mqttStore);
                    mqttStore.Start(m_eventLogger);
                }
            }
            catch (Exception ex)
            {
                m_eventLogger.WriteError("MQTT实时行情处理器注册失败," + ex.Message);
                throw new ApplicationException("MQTT实时行情处理器注册失败");
            }

            try
            {
                if (m_mySqlStoreageList.Count == 0)
                {
                    throw new Exception("MySql外盘接收器列表为空");
                }

                foreach (MySqlKLineStoreage mySqlStore in m_mySqlStoreageList)
                {
                    m_httpDataReceiver.RegisterHttpMarketDataListener(mySqlStore);
                    //mySqlStore.Start(m_eventLogger);
                }
            }
            catch (Exception ex)
            {
                m_eventLogger.WriteError("MQTT实时行情处理器注册失败," + ex.Message);
                throw new ApplicationException("MQTT实时行情处理器注册失败");
            }

            try
            {
                m_httpDataReceiver.Start();
            }
            catch (Exception ex)
            {
                m_eventLogger.WriteError("Http行情数据接收器启动失败," + ex.Message);
                throw new ApplicationException("Http行情数据接收器启动失败");
            }
        }

        public void Close()
        {
            InternalStop();

            FlushAllLoggers();
        }

        private void InternalStop()
        {

            if (m_httpDataReceiver != null)
            {
                try
                {
                    m_httpDataReceiver.Stop();
                }
                catch (Exception ex)
                {
                    string text = String.Format("{0} 关闭HttpServerData接收器失败, Error: {1}", this, ex.Message);
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

            if (m_mySqlStoreageList != null && m_mySqlStoreageList.Count != 0)
            {
                foreach (MySqlKLineStoreage mySqlStore in m_mySqlStoreageList)
                {
                    try
                    {
                        mySqlStore.Stop();
                    }
                    catch (Exception ex)
                    {
                        string text = String.Format("{0} 关闭MySql行情存储失败, Error: {1}", this, ex.Message);
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


        /// <summary>
        /// 创建HTTPServer数据接收器。
        /// </summary>
        private void CreateHttpDataReceiver()
        {
            try
            {
                HttpReceiverSection config = HttpReceiverSection.GetSection();
                m_httpDataReceiver = new HttpServerDataReceiver(config);
                m_httpDataReceiver.Notify += OnNotifyEventArrived;

                string text = String.Format("{0} Create {1} OK.", this, m_httpDataReceiver);
                //m_eventLogger.WriteInformation(text);

            }
            catch (Exception ex)
            {
                string text = "Create HttpDataReceiver object failed, " + ex.Message;
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
                    string clientID = string.Format("server_{0}", i);
                    MQTTMarketDataStoreage marketDataMQTTStorage = new MQTTMarketDataStoreage(assress_deve, clientID);
                    marketDataMQTTStorage.Notify += OnNotifyEventArrived;

                    m_mqttMarketDataStoreageList.Add(marketDataMQTTStorage);

                    string text = String.Format("{0} Create {1} OK.", this, marketDataMQTTStorage);
                    //m_eventLogger.WriteInformation(text);
                }
            }
            catch (Exception ex)
            {
                string text = "Create MQTTMarketDataStoreage object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 创建MySql外盘K线存储器。
        /// </summary>
        private void CreateMySqlKLineStoreage()
        {
            KLineStorageSection section = ConfigurationManager.GetSection("KLineStorage") as KLineStorageSection;
            if (section == null)
            {
                throw new ApplicationException("Not found KLineStoreage config");
            }

            if (section.MySqlStorages != null && section.MySqlStorages.Count > 0)
            {
                foreach (MySqlKLineStorageElement element in section.MySqlStorages)
                {
                    if (element.Enable)
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

                            m_mySqlStoreageList.Add(storeage);
                            string text = String.Format("{0} Create {1} OK.", this, storeage);
                        }
                        catch (Exception ex)
                        {
                            string text = "Create MySqlKLineStorage object failed, " + ex.Message;
                            throw new ApplicationException(text, ex);
                        }
                    }
                }
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

        private void SafeOuterMarketDataEvent(USeMarketData marketData)
        {
            OutLMEMarketDataReceiveHandel handler = this.OutLMEMarketDataReceiveEvent;
            if (handler != null)
            {
                try
                {
                    handler(marketData);
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
