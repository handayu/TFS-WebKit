using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using USe.TradeDriver.Common;
using MarketDataStore;
using System.Diagnostics;
using USe.Common.AppLogger;
using USe.Common.TradingDay;
using USe.Common;
using USe.Common.Manager;
using System.Collections.Concurrent;
using DataStoreCommon;

namespace MarketDataStore
{
    /// <summary>
    /// 行情数据处理器。
    /// </summary>
    public class KLineProcessor: IMarketDataListener,IUSeNotifier
    {
        #region event
        /// <summary>
        /// 通知事件。
        /// </summary>
        public event EventHandler<USeNotifyEventArgs> Notify;
        #endregion

        #region members
        private int m_readCount = 0;
        
        private List<IKLineDataListener> m_storers = null;
        private IKLinePublisher m_kLinePublisher = null;
        private ITradeRangeManager m_tradeRangeManager = null;
        private USeProductManager m_productManager = null;
        private MainContractManager m_mainContractManager = null;
        private USeTradingInstrumentManager m_instrumentManager = null;
        private AlphaDBVistor m_alphaDBVistor = null;

        private Dictionary<string,List<KLineFactory>> m_kLineFactoryDic = new Dictionary<string, List<KLineFactory>>(256);
        private Dictionary<string, List<KLineFactory>> m_indexkLineFactoryDic = new Dictionary<string, List<KLineFactory>>(64);

        private IAppLogger m_eventLogger = null;
        private Thread m_workThread = null;
        private bool m_runFlag = false;

        private int m_processCount = 0;

        private TimeSpan m_dayKLinePublishInterval = new TimeSpan(0, 1, 0);

        private ConcurrentQueue<USeMarketData> m_marketDataQueue = new ConcurrentQueue<USeMarketData>();
        #endregion

        #region construction
        public KLineProcessor(TimeSpan dayKLinePublishInterval)
        {
            m_processCount = 0;
            m_dayKLinePublishInterval = dayKLinePublishInterval;
        }
        #endregion

        /// <summary>
        /// 已处理数量。
        /// </summary>
        public int ProcessCount
        {
            get
            {
                return m_processCount;
            }
        }

        /// <summary>
        /// 已读数量。
        /// </summary>
        public int ReadCount
        {
            get { return m_readCount; }
        }

        /// <summary>
        /// 未读数量。
        /// </summary>
        public int UnReadCount
        {
            get
            {
                return m_marketDataQueue.Count;
            }
        }

        /// <summary>
        /// 启动。
        /// </summary>
        /// <param name="marketDataProvider"></param>
        /// <param name="dbStore"></param>
        public void Start(List<IKLineDataListener> storers,ITradeRangeManager tradeRangeManager,
                          MainContractManager mainContractManager,USeProductManager productManager,AlphaDBVistor alphaDBVistor,
                          USeTradingInstrumentManager instrumentManager,
                          IAppLogger eventLogger)
        {
            m_runFlag = true;

            if (storers == null)
            {
                throw new ArgumentNullException("storers");
            }
            if(tradeRangeManager == null)
            {
                throw new ArgumentNullException("tradeRangeManager");
            }
            if(mainContractManager == null)
            {
                throw new ArgumentNullException("mainContractManager");
            }
            if(productManager == null)
            {
                throw new ArgumentNullException("productManager");
            }
            if(alphaDBVistor == null)
            {
                throw new ArgumentNullException("kLineVistor");
            }
            if(instrumentManager == null)
            {
                throw new ArgumentNullException("instrumentManager");
            }
            if (eventLogger == null)
            {
                throw new ArgumentNullException("eventLogger");
            }

            m_storers = storers;
            m_tradeRangeManager = tradeRangeManager;
            m_productManager = productManager;
            m_mainContractManager = mainContractManager;
            m_alphaDBVistor = alphaDBVistor;
            m_instrumentManager = instrumentManager;
            m_eventLogger = eventLogger;

            KLinePublisher kLinePublisher = new KLinePublisher();
            kLinePublisher.SetMarketDataStore(storers);
            m_kLinePublisher = kLinePublisher;

            this.m_workThread = new Thread(new ThreadStart(DoWork));
            this.m_workThread.Start();
        }

        /// <summary>
        /// 停止。
        /// </summary>
        public void Stop()
        {
            m_runFlag = false;

            Thread.Sleep(2000);
            if (m_workThread.IsAlive)
            {
                System.Diagnostics.Debug.Assert(false);
            }
        }

        /// <summary>
        /// 读数据线程
        /// </summary>
        private void DoWork()
        {
            try
            {
                while (m_runFlag)
                {
                    USeMarketData marketData = GetNextMarketData();
                    if (marketData == null)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    ProcessNextMarketData(marketData);
                }
            }
            catch(Exception ex)
            {
                USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Critical, ex.Message, ToString(), 0);
                SafeRaiseNotifyEvent(this, notify);
            }
        }


        private USeMarketData GetNextMarketData()
        {
            USeMarketData marketData = null;
            if (m_marketDataQueue.TryDequeue(out marketData))
            {
                Interlocked.Increment(ref m_readCount);
            }

            return marketData;
        }

        /// <summary>
        /// 处理下条行情。
        /// </summary>
        private void ProcessNextMarketData(USeMarketData marketData)
        {
            try
            {
                List<KLineFactory> kLineFactoryList = GetKLineFactory(marketData.Instrument);

                List<KLineFactory> indexKLineFactoryList = GetIndexKLineFactory(marketData.Instrument);
                if (indexKLineFactoryList != null)
                {
                    kLineFactoryList.AddRange(indexKLineFactoryList);
                }

                if (kLineFactoryList != null)
                {
                    foreach (KLineFactory factory in kLineFactoryList)
                    {
                        try
                        {
                            factory.UpdateMarketData(marketData);
                        }
                        catch(Exception exx)
                        {
                            USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Error, exx.Message, ToString(), 0);
                            SafeRaiseNotifyEvent(this, notify);
                        }
                    }
                }

                Interlocked.Increment(ref m_processCount);
            }
            catch (Exception ex)
            {
                USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Error, ex.Message, ToString(), 0);
                SafeRaiseNotifyEvent(this, notify);
            }
        }

        /// <summary>
        /// 获取K线生成器。
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        private List<KLineFactory> GetKLineFactory(USeInstrument instrument)
        {
            Debug.Assert(instrument.Market != USeMarket.Unknown);
            List<KLineFactory> factoryList = null;
            if (m_kLineFactoryDic.TryGetValue(instrument.InstrumentCode, out factoryList) == false)
            {
                factoryList = new List<KLineFactory>();
                DayTradeRange tradeRange = m_tradeRangeManager.CreateTradeRange(instrument);
                bool isMainConract = m_mainContractManager.IsMainContract(instrument.InstrumentCode);
                DateTime tradeDay = tradeRange.GetTradeDay(DateTime.Now);
                USeKLine dayKLine = GetDayKLine(tradeDay, instrument);

                DayKLineFactory dayFactory = new DayKLineFactory(instrument,dayKLine, m_kLinePublisher, tradeRange, m_eventLogger, m_dayKLinePublishInterval, isMainConract,m_instrumentManager);
                factoryList.Add(dayFactory);
                MinKLineFactory min1Factory = new MinKLineFactory(instrument, m_kLinePublisher, tradeRange, m_eventLogger, USeCycleType.Min1, isMainConract);
                factoryList.Add(min1Factory);

                m_kLineFactoryDic.Add(instrument.InstrumentCode, factoryList);
            }

            Debug.Assert(factoryList != null && factoryList.Count == 2);

            return new List<KLineFactory>(factoryList);
        }

        /// <summary>
        /// 获取K线生成器。
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        private List<KLineFactory> GetIndexKLineFactory(USeInstrument instrument)
        {
            Debug.Assert(instrument.Market != USeMarket.Unknown);
            string varieties = USeTraderProtocol.GetVarieties(instrument.InstrumentCode);

            
            List<KLineFactory> factoryList = null;
            if (m_indexkLineFactoryDic.TryGetValue(varieties, out factoryList) == false)
            {
                USeProduct product = m_productManager.GetPruduct(varieties);

                factoryList = new List<KLineFactory>();
                DayTradeRange tradeRange = m_tradeRangeManager.CreateTradeRange(varieties);
                DateTime tradeDay = tradeRange.GetTradeDay(DateTime.Now);
                USeInstrument indexInstrument = USeTraderProtocol.GetVarietiesIndexCode(product);
                USeKLine dayKLine = GetDayKLine(tradeDay, indexInstrument);
                
                List<USeInstrument> instrumentList = m_instrumentManager.GetAllInstruments(varieties, product.Market);


                IndexDayKLineFactory dayFactory = new IndexDayKLineFactory(product, instrumentList, dayKLine, m_dayKLinePublishInterval, m_kLinePublisher, tradeRange, m_eventLogger, m_instrumentManager);
                factoryList.Add(dayFactory);
                IndexMinKLineFactory min1Factory = new IndexMinKLineFactory(product, instrumentList, USeCycleType.Min1, m_kLinePublisher, tradeRange, m_eventLogger, m_instrumentManager);
                factoryList.Add(min1Factory);

                m_indexkLineFactoryDic.Add(varieties, factoryList);
            }

            Debug.Assert(factoryList != null && factoryList.Count == 2);
            return factoryList;
        }

        private USeKLine GetDayKLine(DateTime day, USeInstrument instrument)
        {
            try
            {
                USeKLine kline = m_alphaDBVistor.GetDayKLine(instrument, day);
                return kline;
            }
            catch (Exception ex)
            {
                m_eventLogger.WriteError(string.Format("获取{0}@{1:yyyy-MM-dd}日K线数据失败,{2}", instrument, day, ex.Message));
                return null;
            }
        }

        //private USeKLine GetLastMin1DayKLine(DateTime day, USeInstrument instrument)
        //{
        //    try
        //    {
        //        USeKLine kline = m_alphaDBVistor.GetMin1KLine(.GetDayKLine(instrument, day);
        //        return kline;
        //    }
        //    catch (Exception ex)
        //    {
        //        m_eventLogger.WriteError(string.Format("获取{0}@{1:yyyy-MM-dd HH:mm:00}1分钟K线数据失败,{2}", instrument, day, ex.Message));
        //        return null;
        //    }
        //}

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

        public void ReceiveMarketData(USeMarketData marketData)
        {
            m_marketDataQueue.Enqueue(marketData);
        }


        public override string ToString()
        {
            return "MarketDataProcessor";
        }
    }
}
