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

using System.Collections.Concurrent;
using USe.Common.AppLogger;
using USe.Common;
using USe.TradeDriver.Common;
using RabbitMQ.Client;

namespace DataStoreCommon
{
    /// <summary>
    /// 行情存储器(RabbitMQ)。
    /// </summary>
    public class RabbitMQKLineStoreage : KLineStoreage
    {
        #region member
        private const string MQ_EXCHNAGE = "useonline.alpha.analysis";
        private const string MQ_ROUTING_KEY = "ctp";
        private const string MQ_CLOSE_ROUTING_KEY = "ctp_day";

        private TimeSpan m_closeBeginTime = new TimeSpan(15, 1, 0);
        private TimeSpan m_closeEndTime = new TimeSpan(15, 5, 0);

        private string m_mqServerAddress = string.Empty;

        private IConnection m_mqConnnection = null;
        private IModel m_mqChannel = null;

        private Dictionary<string, DateTime> m_closeSendDic = new Dictionary<string, DateTime>(); // 收盘K线发送字典
        private Dictionary<string, DateTime> m_selttlementSendDic = new Dictionary<string, DateTime>(); // 结算K线发送字典

        private Dictionary<string, USeKLine> m_settlementKlineDic = new Dictionary<string, USeKLine>();//带结算价的K线的缓存
        #endregion

        #region construction
        public RabbitMQKLineStoreage(string storageName, string mqServerAddress)
            :base(storageName)
        {
            if (string.IsNullOrEmpty(mqServerAddress))
            {
                throw new ArgumentNullException("mqServerAddress");
            }

            m_mqServerAddress = mqServerAddress;
        }
        #endregion

        #region 
        /// <summary>
        /// 启动。
        /// </summary>
        protected override void PreStart()
        {
            CreateMQChannel();
        }

        protected override void InnerStop()
        {
            StopMQChannel();
        }

        protected override bool FilterKLineData(USeKLine kLine)
        {
            return (kLine.Cycle == USeCycleType.Day);
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
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private byte[] CreateMQBody(USeKLine kLine)
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

            byte[] byteArray = Encoding.UTF8.GetBytes(string.Join(",", filedList));
            return byteArray;
        }

        private void CreateMQChannel()
        {
            if(m_mqChannel != null && m_mqChannel.IsOpen)
            {
                return;
            }
            StopMQChannel();

            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.HostName = m_mqServerAddress;
                IConnection mqConnection = factory.CreateConnection();// factory.CreateConnection(m_mqServerAddress);
                IModel channel = mqConnection.CreateModel();

                m_mqConnnection = mqConnection;
                m_mqChannel = channel;
            }
            catch(Exception ex)
            {
                string text = "创建RabbitMQ Channel失败," + ex.Message;
                m_eventLogger.WriteWarning(text);
                USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
                SafeRaiseNotifyEvent(this, notify);
            }
        }

        public void StopMQChannel()
        {
            try
            {
                if (m_mqChannel != null)
                {
                    if (m_mqChannel.IsOpen)
                    {
                        m_mqChannel.Close();
                    }

                    m_mqChannel = null;
                }
            }
            catch (Exception ex)
            {
                string text = "关闭RabbitMQ Channel失败," + ex.Message;
                m_eventLogger.WriteWarning(text);
                USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
                SafeRaiseNotifyEvent(this, notify);
            }

            try
            {
                if (m_mqConnnection != null)
                {
                    if (m_mqConnnection.IsOpen)
                    {
                        m_mqConnnection.Close();
                    }

                    m_mqConnnection = null;
                }
            }
            catch (Exception ex)
            {
                string text = "关闭RabbitMQ Connection失败," + ex.Message;
                m_eventLogger.WriteWarning(text);
                USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
                SafeRaiseNotifyEvent(this, notify);
            }
        }

        /// <summary>
        /// K先保存。
        /// </summary>
        /// <param name="kLineList"></param>
        private void InternalSendToRocketMQ(USeKLine kLine)
        {
            Debug.Assert(kLine.Cycle == USeCycleType.Day);

            try
            {
                CreateMQChannel();

                byte[] body = CreateMQBody(kLine);

                if (kLine.SettlementPrice > 0m)
                {
                    //有结算价K线
                    m_mqChannel.BasicPublish(MQ_EXCHNAGE, MQ_CLOSE_ROUTING_KEY, null, body);
                }
                else
                {
                    //无结算价K线
                    if (IsInCloseTimeRange() == false)
                    {
                        m_mqChannel.BasicPublish(MQ_EXCHNAGE, MQ_ROUTING_KEY, null, body);
                    }
                    else
                    {
                        m_mqChannel.BasicPublish(MQ_EXCHNAGE, MQ_CLOSE_ROUTING_KEY, null, body);
                    }
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
        private bool IsInCloseTimeRange()
        {
            TimeSpan time = DateTime.Now.TimeOfDay;

            if(time >= m_closeBeginTime && time <= m_closeEndTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return this.StorageName;
        }
    }
}
