using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using System.Collections.Concurrent;
using System.Threading;
using MarketDataStore;
using System.Diagnostics;
using USe.Common;
using System.Data;
using MySql.Data.MySqlClient;
using USe.TradeDriver.Ctp;
using DataStoreCommon;
using USe.Common.AppLogger;
using System.IO;

namespace MarketDataStore
{
    public class CTPMarketDataReceiver : IUSeNotifier
    {
        #region event
        /// <summary>
        /// 通知事件。
        /// </summary>
        public event EventHandler<USeNotifyEventArgs> Notify;

        protected IAppLogger m_eventLogger = null;

        #endregion

        #region
        private ConcurrentQueue<USeMarketData> m_marketDataQueue = new ConcurrentQueue<USeMarketData>();
        private int m_readCount = 0;
        private int m_receiverCount = 0;
        private int m_instrumentCount = 0;
        private DateTime? m_lastMarketDataTime = null;

        private CtpQuoteDriver m_quoteDriver = null;
        private CtpReceiverSection m_config = null;

        private List<IMarketDataListener> m_listenerList = null;
        private string m_dbConnStr = string.Empty;
        private string m_dbName = string.Empty;

        private Dictionary<string, USeMarket> m_instrumentDic = new Dictionary<string, USeMarket>();
        #endregion

        #region construction
        public CTPMarketDataReceiver(CtpReceiverSection config, string dbConnStr, string alphaDBName)
        {
            m_config = config;
            m_dbConnStr = dbConnStr;
            m_dbName = alphaDBName;
            m_eventLogger = new NullLogger("MarketDataStore<NULL>");
            m_listenerList = new List<IMarketDataListener>();
        }
        #endregion

        #region property
        /// <summary>
        /// 合约数量。
        /// </summary>
        public int InstrumentCount
        {
            get { return m_instrumentCount; }
        }

        /// <summary>
        /// 接收数量。
        /// </summary>
        public int ReceiveCount
        {
            get { return m_receiverCount; }
        }

        /// <summary>
        /// 最后一笔行情时间。
        /// </summary>
        public DateTime? LastMarketDataTime
        {
            get { return m_lastMarketDataTime; }
        }
        #endregion

        /// <summary>
        /// 从数据库中获得合约表。
        /// </summary>
        /// <param name="monitorMarketes"></param>
        /// <returns></returns>
        private List<USeInstrument> GetSubscribeInstruments(List<USeMarket> monitorMarketes)
        {
            List<USeInstrument> instrumentList = new List<USeInstrument>();

            string strSel = string.Format(@"select contract,contract_name,exchange, open_date,expire_date,is_trading from {0}.contracts 
                              where product_class ='Futures' and UNIX_TIMESTAMP(CURDATE()) >= UNIX_TIMESTAMP(open_date) and  UNIX_TIMESTAMP(CURDATE()) <= UNIX_TIMESTAMP(expire_date);", m_dbName);

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(strSel, connection);
                adapter.Fill(table);
            }

            foreach (DataRow row in table.Rows)
            {
                USeMarket market = (USeMarket)Enum.Parse(typeof(USeMarket), row["exchange"].ToString());

                USeInstrument instrument = new USeInstrument(row["contract"].ToString(),
                    row["contract_name"].ToString(),
                    market);

                instrumentList.Add(instrument);
            }
            return instrumentList;
        }

        /// <summary>
        /// 启动行情接收。
        /// </summary>
        /// <param name="monitorMarketes">监控市场。</param>
        public void Start(List<USeMarket> monitorMarketes)
        {
            List<USeInstrument> subList = null;
            try
            {
                subList = GetSubscribeInstruments(monitorMarketes);
                m_instrumentCount = subList.Count;
                m_instrumentDic.Clear();
                foreach (USeInstrument instrument in subList)
                {
                    m_instrumentDic.Add(instrument.InstrumentCode, instrument.Market);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取合约列表失败," + ex.Message);
            }

            CtpQuoteDriver quoteDriver = null;
            try
            {
                quoteDriver = new CtpQuoteDriver(m_config.QuoteAddress, m_config.QuotePort);
                quoteDriver.ConnectServer();
                quoteDriver.Login(m_config.BrokerID, m_config.Account, m_config.PassWord);
                quoteDriver.OnMarketDataChanged += QuoteDriver_OnMarketDataChanged;
                quoteDriver.OnDriverStateChanged += QuoteDriver_OnDriverStateChanged;
                //quoteDriver.Notify += QuoteDriver_Notify;

                quoteDriver.Subscribe(subList);
                //quoteDriver.Subscribe(new List<USeInstrument>() {
                //    new USeInstrument("p1711", "p1707", USeMarket.DCE) });

                m_quoteDriver = quoteDriver;
            }
            catch (Exception ex)
            {
                if (quoteDriver != null)
                {
                    //quoteDriver.Logout();
                    quoteDriver.DisConnectServer();
                }


                throw new Exception("订阅行情失败," + ex.Message);
            }
        }

        private void QuoteDriver_OnDriverStateChanged(object sender, USeQuoteDriverStateChangedEventArgs e)
        {
            string text = string.Format("行情驱动状态为{0},{1}", e.NewState.ToString(), e.Reason);

            USeNotifyEventArgs args = new USeNotifyEventArgs(USeNotifyLevel.Information, text);
            SafeRaiseNotifyEvent(this, args);
        }

        /// <summary>
        /// 输出指定信息到文本文件
        /// </summary>
        /// <param name="msg">输出信息</param>
        public void WriteMessage(string msg)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "MarketData_MorningData.log";

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sw.WriteLine("{0}\n", msg, DateTime.Now);
                    sw.Flush();
                }
            }
        }


        public void QuoteDriver_OnMarketDataChanged(object sender, USeMarketDataChangedEventArgs e)
        {
            USeMarketData marketData = e.MarketData;

            #region 测试观察早盘的集合竞价的数据
            string str = string.Format("InsName:{0} ,Open:{1} ,High:{2} ,Low:{3},Close:{4},LastPrice:{5},OpenInterest:{6},SettlementPrice:{7},Volume{8},DateTime:{9}",
                marketData.Instrument.InstrumentCode, marketData.OpenPrice, marketData.HighPrice, marketData.LowPrice, marketData.ClosePrice,
                marketData.LastPrice, marketData.OpenInterest, marketData.SettlementPrice, marketData.Volume,marketData.UpdateTime);

            WriteMessage(str);
            #endregion


            if (IgnoreMarketData(marketData))    // 行情忽略检查
            {
                return;
            }

            ProcessUSeMarketData(marketData);

            if (m_listenerList != null && m_listenerList.Count > 0)
            {
                foreach (IMarketDataListener listener in m_listenerList)
                {
                    try
                    {
                        listener.ReceiveMarketData(marketData);
                    }
                    catch (Exception ex)
                    {
                        Debug.Assert(false, ex.Message);
                    }
                }
            }

            Interlocked.Increment(ref m_receiverCount);
            m_lastMarketDataTime = marketData.UpdateTime;
        }

        /// <summary>
        /// 行情忽略检查，没办法通过CTP进行有效判断
        /// </summary>
        /// <param name="marketData"></param>
        /// <returns></returns>
        private bool IgnoreMarketData(USeMarketData marketData)
        {
            TimeSpan currTime = DateTime.Now.TimeOfDay;

            //Ctp7:30左右会把夜盘数据重新推送一遍，强制忽略
            if (currTime >= new TimeSpan(3, 0, 0) && currTime <= new TimeSpan(8, 45, 0))
            {
                return true;
            }

            //成交量小于0的行情忽略掉
            if (marketData.Volume <= 0)
            {
                return true;
            }

            //时间日期字段缺失，忽略掉
            if (marketData.QuoteTime.HasValue == false || marketData.QuoteDay.HasValue == false)
            {
                return true;
            }
            if (m_instrumentDic.ContainsKey(marketData.Instrument.InstrumentCode) == false)
            {
                Debug.Assert(false);
                return true;
            }

            if (USeManager.Instance.DayNightType == DayNightType.Day) // 如果是日盘
            {
                if (marketData.UpdateTime.TimeOfDay > DateTime.Now.TimeOfDay.Add(new TimeSpan(0, 10, 0)))
                {
                    return true;
                }
            }

            return false;
        }

        private void ProcessUSeMarketData(USeMarketData marketData)
        {
            Debug.Assert(marketData.QuoteDay.HasValue);
            Debug.Assert(marketData.QuoteTime.HasValue);

            if (USeManager.Instance.DayNightType == DayNightType.Night)
            {
                USeMarket market = USeMarket.Unknown;
                if (m_instrumentDic.TryGetValue(marketData.Instrument.InstrumentCode, out market) == false)
                {
                    Debug.Assert(false);
                }

                if (market == USeMarket.SHFE || market == USeMarket.DCE)// 如果是上期所，大连
                {
                    DateTime preTradeDay = USeManager.Instance.TradeCalendarManager.GetPreTradingDate(marketData.QuoteDay.Value);
                    if (marketData.QuoteTime.Value > new TimeSpan(20, 45, 0))
                    {
                        marketData.UpdateTime = preTradeDay.Add(marketData.QuoteTime.Value);
                    }
                    else
                    {
                        marketData.UpdateTime = preTradeDay.AddDays(1).Add(marketData.QuoteTime.Value);
                    }
                }
            }
        }

        public void Stop()
        {
            if (m_quoteDriver != null)
            {
                m_quoteDriver.Logout();
                m_quoteDriver.DisConnectServer();
            }
        }

        #region
        public USeMarketData GetNextMarketData()
        {
            USeMarketData marketData = null;
            if (m_marketDataQueue.TryDequeue(out marketData))
            {
                Interlocked.Increment(ref m_readCount);
            }

            return marketData;
        }
        #endregion

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

        public override string ToString()
        {
            return "CTPMarketDataReceiver";
        }

        /// <summary>
        /// 注册行情监听者。
        /// </summary>
        /// <param name="listener">行情监听者。</param>
        public void RegisterMarketDataListener(IMarketDataListener listener)
        {
            m_listenerList.Add(listener);
        }

        /// <summary>
        /// 注销行情监听者。
        /// </summary>
        /// <param name="listener"></param>
        public void UnRegisterMarketDataListener(IMarketDataListener listener)
        {
            for (int i = 0; i < m_listenerList.Count; i++)
            {
                if (m_listenerList[i] == listener)
                {
                    m_listenerList.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
