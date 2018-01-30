using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data;
using DataStoreCommon;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Collections.Concurrent;
using USe.Common.AppLogger;
using USe.Common;
using USe.TradeDriver.Common;
using USe.Common.Manager;
using USe.Common.TradingDay;

namespace DataStoreCommon
{
    /// <summary>
    /// K线存储器(文件存储）。
    /// </summary>
    public class FileKLineStoreage : KLineStoreage
    {
        #region member
        private string m_kLineFolderPath = string.Empty;
        private Dictionary<string, FileStorer> m_fileStorerDic = null;
        private TradeCalendarManager m_tradeCalendar = null;
        #endregion

        #region construction
        public FileKLineStoreage(string storageName,string kLineFolderPath,TradeCalendarManager tradeCalendarManager)
            :base(storageName)
        {
            if (string.IsNullOrEmpty(kLineFolderPath))
            {
                throw new ArgumentNullException("kLineFolderPath");
            }

            m_kLineFolderPath = kLineFolderPath;

            if(Directory.Exists(kLineFolderPath) == false)
            {
                Directory.CreateDirectory(kLineFolderPath);
            }

            m_fileStorerDic = new Dictionary<string, FileStorer>();
            m_tradeCalendar = tradeCalendarManager;
        }
        #endregion

        #region 
        protected override bool FilterKLineData(USeKLine kLine)
        {
            return true;
        }
        #endregion

        #region 工作线程
        /// <summary>
        /// 读数据线程
        /// </summary>
        protected override  void DoWork()
        {
            try
            {
                while (m_runFlag)
                {

                    while (m_kLineQueue.Count > 0)
                    {
                        USeKLine kLine = null;
                        m_kLineQueue.TryDequeue(out kLine);
                        Debug.Assert(kLine != null);

                        FileStorer storer = GetFileStorer(kLine);
                        try
                        {
                            storer.Write(ToKLineLog(kLine));
                            Interlocked.Increment(ref m_sotreCount);
                        }
                        catch(Exception ex)
                        {
                            Interlocked.Increment(ref m_errorStoreCount);
                            USeNotifyEventArgs arg = new USeNotifyEventArgs(USeNotifyLevel.Error, "文件保存K线失败," + ex.Message);
                            SafeRaiseNotifyEvent(this, arg);
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private FileStorer GetFileStorer(USeKLine kLine)
        {
            string key = string.Format("{0}_{1}", kLine.Cycle.ToString(), kLine.InstrumentCode);

            FileStorer storer = null;
            if(m_fileStorerDic.TryGetValue(key,out storer) == false)
            {
                string fileName = Path.Combine(m_kLineFolderPath, GetTradeDayPath(), key + ".csv");
                storer = new FileStorer(fileName);
                m_fileStorerDic.Add(key, storer);
            }

            return storer;
        }

        /// <summary>
        /// 获取交易日(临时做法周五不适用)。
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

        private string ToKLineLog(USeKLine kLine)
        {
            List<string> filedList = new List<string>();
            filedList.Add(kLine.InstrumentCode);
            filedList.Add(kLine.Market.ToString());
            filedList.Add(kLine.Cycle.ToString());
            filedList.Add(kLine.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            filedList.Add(kLine.Open.ToString());
            filedList.Add(kLine.High.ToString());
            filedList.Add(kLine.Low.ToString());
            filedList.Add(kLine.Close.ToString());
            filedList.Add(kLine.Volumn.ToString());
            filedList.Add(kLine.Turnover.ToString());
            filedList.Add(kLine.OpenInterest.ToString());

            filedList.Add(kLine.SettlementPrice.ToString());
            filedList.Add(kLine.PreSettlementPrice.ToString());
            filedList.Add(kLine.AskVolumn.ToString());
            filedList.Add(kLine.BidVolumn.ToString());

            return string.Join(",", filedList);
        }

        
        #endregion


        public override string ToString()
        {
            return this.StorageName;
        }
    }
}
