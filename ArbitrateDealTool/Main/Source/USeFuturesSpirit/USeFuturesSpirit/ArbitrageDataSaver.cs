using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using USe.Common.AppLogger;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 套利单数据保存。
    /// </summary>
    class ArbitrageDataSaver
    {
        #region member
        private USeDataAccessor m_dataAccessor = null;

        private BackgroundWorker m_worker = null;  //工作线程
        private Queue<USeArbitrageOrder> m_saveQueue = new Queue<USeArbitrageOrder>();

        private IAppLogger m_eventLogger = new NullLogger("ArbitrageDataSaver<NUll>");
        private AutoResetEvent m_resetEvent = new AutoResetEvent(false);

        private bool m_runFlag = false;
        #endregion

        #region methods
        /// <summary>
        /// 设置数据访问器。
        /// </summary>
        /// <param name="dataAccessor"></param>
        public void SetDataAccesssor(USeDataAccessor dataAccessor)
        {
            m_dataAccessor = dataAccessor;
        }

        /// <summary>
        /// 设置事件日志。
        /// </summary>
        /// <param name="eventLogger"></param>
        public void SetEventLogger(IAppLogger eventLogger)
        {
            if (eventLogger != null)
            {
                m_eventLogger = eventLogger;
            }
        }

        /// <summary>
        /// 启动数据保存。
        /// </summary>
        public void Start()
        {
            if(m_dataAccessor == null)
            {
                throw new ApplicationException("DataAccessor is empty");
            }
           
            if(m_worker != null && m_worker.IsBusy)
            {
                throw new Exception("ArbitrageDataSaver is running");
            }

            try
            {
                m_runFlag = true;

                m_worker = new BackgroundWorker();
                m_worker.DoWork += M_worker_DoWork;
                m_worker.RunWorkerCompleted += M_worker_RunWorkerCompleted;
                m_worker.RunWorkerAsync();
            }
            catch(Exception ex)
            {
                m_runFlag = false;
                throw new Exception("Start ArbitrageDataSaver failed," + ex.Message);
            }
        }

        /// <summary>
        /// 停止数据保存。
        /// </summary>
        public void Stop()
        {
            m_runFlag = false;
            m_resetEvent.Set();
            Thread.Sleep(500);

            //Debug.Assert(m_worker == null || m_worker.IsBusy == false);
        }

        private void M_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(m_worker != null)
            {
                m_worker.DoWork -= M_worker_DoWork;
                m_worker.RunWorkerCompleted -= M_worker_RunWorkerCompleted;
                m_worker = null;
            }
        }

        private void M_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (m_runFlag)
            {
                m_resetEvent.WaitOne(2000);

                BeginSaveAribtrageOrder();
            }
        }

        private void BeginSaveAribtrageOrder()
        {
            while (m_saveQueue.Count > 0)
            {
                USeArbitrageOrder arbitrageOrder = m_saveQueue.Dequeue();
                Debug.Assert(arbitrageOrder != null);
                try
                {
                    m_dataAccessor.SaveUSeArbitrageOrder(arbitrageOrder);
                }
                catch (Exception ex)
                {
                    m_eventLogger.WriteError("保存套利单信息失败," + ex.Message);
                }
            }
        }


        public void AddSaveTask(USeArbitrageOrder arbitrageOrder)
        {
            m_saveQueue.Enqueue(arbitrageOrder);
            m_resetEvent.Set();
        }
        #endregion
    }
}
