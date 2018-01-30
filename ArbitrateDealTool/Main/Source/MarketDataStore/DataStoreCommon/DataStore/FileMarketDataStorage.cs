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

namespace DataStoreCommon
{
    public class FileMarketDataStorage :IMarketDataListener,IUSeNotifier
    {
        #region event
        /// <summary>
        /// 通知事件。
        /// </summary>
        public event EventHandler<USeNotifyEventArgs> Notify;
        #endregion

        #region member
        private string m_marketDataFolderPath = string.Empty;
        private ConcurrentQueue<USeMarketData> m_marketDataQueue = null;
        private Dictionary<string, FileStorer> m_fileStorerDic = null;

        private int m_sotreCount = 0;
        private int m_errorStoreCount = 0;

        protected bool m_runFlag = false;
        private IAppLogger m_eventLogger = null;

        private BackgroundWorker m_worker = null;
        private TradeCalendarManager m_tradeCalendar = null;
        #endregion

        public FileMarketDataStorage(string marketDataFolderPath, TradeCalendarManager tradeCalendarManager)
        {
            if (string.IsNullOrEmpty(marketDataFolderPath))
            {
                throw new ArgumentNullException("kLineFolderPath");
            }

            m_marketDataFolderPath = marketDataFolderPath;

            if (Directory.Exists(marketDataFolderPath) == false)
            {
                Directory.CreateDirectory(marketDataFolderPath);
            }

            m_fileStorerDic = new Dictionary<string, FileStorer>();

            m_eventLogger = new NullLogger("MarketDataFileStorage<NULL>");
            m_marketDataQueue = new ConcurrentQueue<USeMarketData>();
            m_tradeCalendar = tradeCalendarManager;
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
                m_worker.DoWork += M_worker_DoWork;
                m_worker.RunWorkerCompleted += M_worker_RunWorkerCompleted;
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
            try
            {
                while (m_runFlag)
                {
                    while (m_marketDataQueue.Count > 0)
                    {
                        USeMarketData marketData = null;
                        m_marketDataQueue.TryDequeue(out marketData);
                        Debug.Assert(marketData != null);

                        FileStorer storer = GetFileStorer(marketData);
                        try
                        {
                            storer.Write(ToMarketDataLog(marketData));
                            Interlocked.Increment(ref m_sotreCount);
                        }
                        catch (Exception ex)
                        {
                            Interlocked.Increment(ref m_errorStoreCount);
                            USeNotifyEventArgs arg = new USeNotifyEventArgs(USeNotifyLevel.Error, "文件保存行情数据失败," + ex.Message);
                            SafeRaiseNotifyEvent(this, arg);
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private FileStorer GetFileStorer(USeMarketData marketData)
        {
            string key = string.Format("{0}", marketData.Instrument.InstrumentCode);

            FileStorer storer = null;
            if (m_fileStorerDic.TryGetValue(key, out storer) == false)
            {
                //路径待处理
                string fileName = Path.Combine(m_marketDataFolderPath, GetTradeDayPath(), key + ".csv");
                storer = new FileStorer(fileName);
                m_fileStorerDic.Add(key, storer);
            }

            return storer;
        }

        /// <summary>
        /// 获取交易日。
        /// </summary>
        /// <returns></returns>
        private string GetTradeDayPath()
        {
            TradeCalendar tradeCalendar = m_tradeCalendar.GetTradeCalendar(DateTime.Today);
            if (tradeCalendar == null)
            {
                string text = string.Format("未能找到{0:yyyy-MM-dd}交易日历", DateTime.Today);
                m_eventLogger.WriteError(text);
                USeNotifyEventArgs arg = new USeNotifyEventArgs(USeNotifyLevel.Error, text);
                SafeRaiseNotifyEvent(this, arg);

                return DateTime.Today.ToString("yyyyMMdd");
            }
            else
            {
                if (tradeCalendar.IsTradeDay)
                {
                    if (DateTime.Now.TimeOfDay > new TimeSpan(20, 0, 0))
                    {
                        return tradeCalendar.NextTradeDay.ToString("yyyyMMdd");
                    }
                    else
                    {
                        return DateTime.Today.ToString("yyyyMMdd");
                    }
                }
                else
                {
                    return tradeCalendar.NextTradeDay.ToString("yyyyMMdd");
                }
            }
        }

        private string ToMarketDataLog(USeMarketData data)
        {
            List<string> filedList = new List<string>();
            filedList.Add(data.Instrument.InstrumentCode);
            filedList.Add(data.AskPrice.ToString());
            filedList.Add(data.AskSize.ToString());
            filedList.Add(data.BidPrice.ToString());
            filedList.Add(data.BidSize.ToString());
            filedList.Add(data.OpenPrice.ToString());
            filedList.Add(data.HighPrice.ToString());
            filedList.Add(data.LowPrice.ToString());
            filedList.Add(data.LastPrice.ToString());
            filedList.Add(data.ClosePrice.ToString());
            filedList.Add(data.PreClosePrice.ToString());
            filedList.Add(data.UpperLimitPrice.ToString());
            filedList.Add(data.LowerLimitPrice.ToString());
            filedList.Add(data.PreSettlementPrice.ToString());
            filedList.Add(data.SettlementPrice.ToString());
            filedList.Add(data.OpenInterest.ToString());
            filedList.Add(data.Volume.ToString());
            filedList.Add(data.Turnover.ToString());
            filedList.Add(data.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            filedList.Add(data.QuoteDay.HasValue ? data.QuoteDay.Value.ToString("yyyy-MM-dd") : "");
            filedList.Add(data.QuoteTime.HasValue ? data.QuoteTime.Value.ToString(@"hh\:mm\:ss") : "");

            return string.Join(",", filedList);
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
