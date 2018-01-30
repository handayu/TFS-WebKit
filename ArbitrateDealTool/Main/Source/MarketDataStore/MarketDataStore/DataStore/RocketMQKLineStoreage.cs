using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data;
using MarketDataStore;
using System.Data;
using System.Threading;
using System.Diagnostics;

using System.Collections.Concurrent;
using USe.Common.AppLogger;
using USe.Common.HttpJson;
using Newtonsoft.Json;
using USe.Common;
using USe.TradeDriver.Common;

namespace MarketDataStore
{
    /// <summary>
    /// 行情存储器(RocketMQ)。
    /// </summary>
    public class RocketMQKLineStoreage : KLineStoreage
    {
        #region member
        private string m_sendUrl = string.Empty;
        private HttpJsonDataVistor m_httpVistor = null;
        #endregion

        #region construction
        public RocketMQKLineStoreage(string storageName, string sendUrl)
            :base(storageName)
        {
            if (string.IsNullOrEmpty(sendUrl))
            {
                throw new ArgumentNullException("sendUrl");
            }

            m_sendUrl = sendUrl;
            m_httpVistor = new HttpJsonDataVistor();
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
        protected override void DoWork()
        {
            try
            {
                while (m_runFlag)
                {
                    USeKLine kLine = null;
                    m_kLineQueue.TryDequeue(out kLine);
                    if (kLine == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    InternalSendToRocketMQ(kLine);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private string CreatePostParameter(USeKLine kLine)
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

            RocketMQMessage message = new RocketMQMessage()
            {
                body = string.Join(",", filedList),
                keys = "ctp",
                producerGroup = "basedata",
                tags = "kline",
                topic = "alpha"
            };
            string parameter = JsonConvert.SerializeObject(message);
            return parameter;
        }


        /// <summary>
        /// K先保存。
        /// </summary>
        /// <param name="kLineList"></param>
        private void InternalSendToRocketMQ(USeKLine kLine)
        {
            try
            {
                HttpHeader header = HttpHeader.DefaultPostHeader;
                header.Param = CreatePostParameter(kLine);

                JsonData jsonData = m_httpVistor.GetJsonData(m_sendUrl, header, null);
                if (jsonData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    RocketMQReply reply = JsonConvert.DeserializeObject<RocketMQReply>(jsonData.JsonString);
                }
                else
                {
                    string jsonReply = jsonData.JsonString;
                }
                Interlocked.Increment(ref m_sotreCount);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref m_errorStoreCount);
                string text = string.Format("{0}保存K线数据失败,{1}", this, ex.Message);
                m_eventLogger.WriteError(text);
                USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
                SafeRaiseNotifyEvent(this, notify);
            }
        }
        #endregion

        public override string ToString()
        {
            return this.StorageName;
        }
    }
}
