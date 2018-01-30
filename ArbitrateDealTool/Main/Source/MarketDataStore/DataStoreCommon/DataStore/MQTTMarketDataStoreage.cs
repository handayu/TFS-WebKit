using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using USe.TradeDriver.Common;
using USe.Common;
using System.Diagnostics;
using USe.Common.AppLogger;
using System.ComponentModel;
using System.IO;
using USe.Common.TradingDay;
using USe.Common.Manager;
using uPLibrary.Networking.M2Mqtt;
using System.Net;
using uPLibrary.Networking.M2Mqtt.Messages;
using StriderMqtt;

namespace DataStoreCommon
{
    public class MQTTMarketDataStoreage : IMarketDataListener, IUSeNotifier
    {
        private MqttConnection m_mqttClient = null;    //主机为IP时
        private string m_mqttServerAddress = string.Empty;
        private int m_mqttPort = int.MinValue;
        private string m_clientId = string.Empty;

        private string m_topic = "futuredata";
        private string m_topicLME = "futuredata_lme";
        private object m_locker = new object();

        private readonly TimeSpan m_pollLimit = TimeSpan.FromMilliseconds(50);

        /// <summary>
        /// 通知事件。
        /// </summary>
        public event EventHandler<USeNotifyEventArgs> Notify;
        private string m_marketDataFolderPath = string.Empty;
        private ConcurrentQueue<USeMarketData> m_marketDataQueue = null;

        private int m_sotreCount = 0;
        private int m_errorStoreCount = 0;

        protected bool m_runFlag = false;
        private IAppLogger m_eventLogger = null;

        private BackgroundWorker m_worker = null;

        private InMemoryPersistence m_mqttPersistence = new InMemoryPersistence();

        public string StoreageName
        {
            get { return "MQTTReal_" + m_clientId; }
        }

        public MQTTMarketDataStoreage(string ipAddressPort, string clientID)
        {
            try
            {
                if (string.IsNullOrEmpty(ipAddressPort))
                {
                    throw new ArgumentNullException("初始化MQTT服务器地址mqttServerAddress为空");
                }

                string[] ipAddPortArray = ipAddressPort.Split(':');

                m_mqttServerAddress = ipAddPortArray[0];
                m_mqttPort = Convert.ToInt32(ipAddPortArray[1]);
                m_clientId = clientID;
                m_eventLogger = new NullLogger("MarketDataFileStorage<NULL>");
                m_marketDataQueue = new ConcurrentQueue<USeMarketData>();
            }
            catch (Exception ex)
            {
                throw new Exception("初始化连接MQTT发生异常:" + ex.Message);
            }

        }

        protected void InnerStop()
        {
            StopMQTTChannel();
        }

        private void StopMQTTChannel()
        {
            try
            {
                if (m_mqttClient != null)
                {
                    m_mqttClient.Disconnect();
                    m_mqttClient = null;
                }
            }
            catch (Exception ex)
            {
                string text = "关闭MQTTChannel失败," + ex.Message;
                m_eventLogger.WriteWarning(text);
                USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
                SafeRaiseNotifyEvent(this, notify);
            }

        }

        /// <summary>
        /// 存储失败数。
        /// </summary>
        public int ErrorStoreCount
        {
            get { return m_errorStoreCount; }
        }

        /// <summary>
        /// 已存储数量。
        /// </summary>
        public int StoreCount
        {
            get { return m_sotreCount; }
        }

        /// <summary>
        /// 未存储数量。
        /// </summary>
        public int UnStoreCount
        {
            get { return m_marketDataQueue.Count; }
        }

        /// <summary>
        /// 是否工作。
        /// </summary>
        public bool IsBusy
        {
            get { return m_runFlag; }
        }

        public void ReceiveMarketData(USeMarketData marketData)
        {
            m_marketDataQueue.Enqueue(marketData);
        }

        public void Start(IAppLogger eventLogger)
        {
            m_runFlag = true;
            m_worker = new BackgroundWorker();
            m_worker.DoWork += M_worker_DoWork;
            m_worker.RunWorkerCompleted += M_worker_RunWorkerCompleted;
            m_worker.RunWorkerAsync();
        }

        public void Stop()
        {
            m_runFlag = false;

            int count = 20;
            while (count > 0)
            {
                Thread.Sleep(100);
                BackgroundWorker worker = m_worker;
                if (worker != null && worker.IsBusy)
                {
                    count -= 1;
                    continue;
                }
                else
                {
                    break;
                }
            }
        }

        private void M_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (m_worker != null)
            {
                m_worker.DoWork -= M_worker_DoWork;
                m_worker.RunWorkerCompleted -= M_worker_RunWorkerCompleted;
                m_worker = null;
            }
        }

        private void M_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DoWork();
        }

        /// <summary>
        /// 读数据线程
        /// </summary>
        private void DoWork()
        {
            lock (m_locker)
            {
                try
                {
                    //CreateMQTTChannel();
                    var connArgs = new MqttConnectionArgs()
                    {
                        ClientId = m_clientId,
                        Hostname = m_mqttServerAddress,
                        Port = m_mqttPort,
                        Keepalive = new TimeSpan(1, 0, 0)
                    };

                    using (m_mqttClient = new MqttConnection(connArgs))
                    {
                        m_mqttClient.Connect();

                        while (m_runFlag)
                        {
                            USeMarketData marketData = null;
                            m_marketDataQueue.TryDequeue(out marketData);
                            if (marketData == null)
                            {
                                Thread.Sleep(1000);
                                continue;
                            }

                            //Debug.WriteLine(string.Format("当前MQTT链接:{0}", connArgs.ClientId));
                            //[hanyu]暂时只推送上期的品种行情
                            if (marketData.Instrument.Market == USeMarket.SHFE || marketData.Instrument.Market == USeMarket.LME || marketData.Instrument.Market == USeMarket.DCE) InternalSendTotMQTT(marketData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    string text = string.Format("** {0}链接MQTT失败,{1}", this.StoreageName, ex.Message);
                    m_eventLogger.WriteError(text);
                    USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
                    SafeRaiseNotifyEvent(this, notify);
                }
            }
        }

        void BindEvents(MqttConnection conn)
        {
            conn.PublishSent += HandlePublishSent;
        }

        void HandlePublishSent(object sender, IdentifiedPacketEventArgs e)
        {
            // a publish was sent to the broker
            (sender as MqttConnection).InterruptLoop();
        }

        private void CreateMQChannel()
        {
            if (m_mqttClient.IsPublishing)
            {
                return;
            }
            StopMQTTChannel();

            try
            {
                var connArgs = new MqttConnectionArgs()
                {
                    ClientId = m_clientId,
                    Hostname = m_mqttServerAddress,
                    Port = m_mqttPort,
                    Keepalive = new TimeSpan(1, 0, 0)
                };

                m_mqttClient = new MqttConnection(connArgs, m_mqttPersistence, null);

                m_mqttClient.Connect();
            }
            catch (Exception ex)
            {
                string text = "创建MQTTChannel失败," + ex.Message;
                m_eventLogger.WriteWarning(text);
                USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
                SafeRaiseNotifyEvent(this, notify);
            }
        }

        private void InternalSendTotMQTT(USeMarketData marketData)
        {
            try
            {
                CreateMQChannel();

                byte[] body = CreateMQTTBody(marketData);

                if (marketData.Instrument.Market == USeMarket.LME)
                {
                    m_mqttClient.Publish(m_topicLME, body, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
                    Interlocked.Increment(ref m_sotreCount);
                }
                else
                {
                    m_mqttClient.Publish(m_topic, body, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
                    Interlocked.Increment(ref m_sotreCount);
                }

                Debug.WriteLine(string.Format("时间:{0} 合约:{1}  最新价:{2} body:{3}", marketData.QuoteTime, marketData.Instrument.InstrumentCode, marketData.ClosePrice, body));
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref m_errorStoreCount);
                string text = string.Format("{0}发送MQTT实时行情数据失败,{1}", this, ex.Message);
                m_eventLogger.WriteError(text);
                USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
                SafeRaiseNotifyEvent(this, notify);
            }
        }

        private byte[] CreateMQTTBody(USeMarketData marketData)
        {
            List<string> filedList = new List<string>();
            filedList.Add(marketData.Instrument.InstrumentCode);
            filedList.Add(marketData.QuoteDay.HasValue ? marketData.QuoteDay.Value.ToString("yyyy-MM-dd") : "");
            filedList.Add(marketData.QuoteTime.HasValue ? marketData.QuoteTime.Value.ToString(@"hh\:mm\:ss") : "");
            filedList.Add(marketData.LastPrice.ToString());
            filedList.Add(marketData.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            filedList.Add(marketData.AskPrice.ToString());
            filedList.Add(marketData.AskSize.ToString());
            filedList.Add(marketData.BidPrice.ToString());
            filedList.Add(marketData.BidSize.ToString());
            filedList.Add(marketData.OpenPrice.ToString());
            filedList.Add(marketData.HighPrice.ToString());
            filedList.Add(marketData.LowPrice.ToString());
            filedList.Add(marketData.OpenInterest.ToString());
            filedList.Add(marketData.PreOpenInterest.ToString());
            filedList.Add(marketData.SettlementPrice.ToString());
            filedList.Add(marketData.PreSettlementPrice.ToString());
            filedList.Add(marketData.Volume.ToString());
            filedList.Add(marketData.Turnover.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(string.Join(",", filedList));
            return byteArray;
        }

        /// <summary>
        /// 安全地发布指定的通知事件。
        /// </summary>
        /// <param name="sender">通知事件发送者对象。</param>
        /// <param name="e">通知事件参数对象。</param>
        protected void SafeRaiseNotifyEvent(object sender, USeNotifyEventArgs e)
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
                    Debug.Assert(false, ex.Message);
                }
            }
        }
    }
}
