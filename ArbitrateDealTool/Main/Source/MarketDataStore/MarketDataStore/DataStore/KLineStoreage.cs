using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USe.Common;
using USe.TradeDriver.Common;
using System.Diagnostics;
using USe.Common.AppLogger;
using System.ComponentModel;
using System.Threading;

namespace MarketDataStore
{
    public abstract class KLineStoreage :IKLineDataListener
    {
        #region event
        /// <summary>
        /// 通知事件。
        /// </summary>
        public event EventHandler<USeNotifyEventArgs> Notify;
        #endregion

        #region member
        private string m_storageName = string.Empty;
        protected ConcurrentQueue<USeKLine> m_kLineQueue = null;
        protected int m_errorStoreCount = 0;
        protected int m_sotreCount = 0;
        protected bool m_runFlag = false;
        protected IAppLogger m_eventLogger = null;

        private BackgroundWorker m_worker = null;
        #endregion

        public KLineStoreage(string storageName)
        {
            m_storageName = storageName;
            m_eventLogger = new NullLogger("MarketDataStore<NULL>");
            m_kLineQueue = new ConcurrentQueue<USeKLine>();
        }

        #region proeprty
        /// <summary>
        /// 存储异常数量。
        /// </summary>
        public virtual int ErrorStoreCount
        {
            get { return m_errorStoreCount; }
        }

        /// <summary>
        /// 已存储数量。
        /// </summary>
        public virtual int StoreCount
        {
            get { return m_sotreCount; }
        }

        /// <summary>
        /// 未存储数量。
        /// </summary>
        public virtual int UnStoreCount
        {
            get { return m_kLineQueue.Count; }
        }

        /// <summary>
        /// 是否工作。
        /// </summary>
        public bool IsBusy
        {
            get { return m_runFlag; }
        }

        /// <summary>
        /// 存储器名称。
        /// </summary>
        public string StorageName
        {
            get { return m_storageName; }
        }
        #endregion

        /// <summary>
        /// 启动存储器。
        /// </summary>
        public void Start()
        {
            PreStart();
            m_runFlag = true;
            m_worker = new BackgroundWorker();
            m_worker.DoWork += M_worker_DoWork;
            m_worker.RunWorkerCompleted += M_worker_RunWorkerCompleted;

            m_worker.RunWorkerAsync();
        }

        protected virtual void PreStart()
        {
        }

        /// <summary>
        /// 停止。
        /// </summary>
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

            InnerStop();
        }

        protected virtual void InnerStop()
        {
        }

        /// <summary>
        /// 重置。
        /// </summary>
        public void Reset()
        {
            m_sotreCount = 0;
            m_errorStoreCount = 0;
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

        protected abstract void DoWork();

        /// <summary>
        /// 接收K线数据。
        /// </summary>
        /// <param name="kLine"></param>
        public void ReceiveKLineData(USeKLine kLine)
        {
            if (FilterKLineData(kLine))
            {
                m_kLineQueue.Enqueue(kLine);
            }
        }

        protected abstract bool FilterKLineData(USeKLine kLine);

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
