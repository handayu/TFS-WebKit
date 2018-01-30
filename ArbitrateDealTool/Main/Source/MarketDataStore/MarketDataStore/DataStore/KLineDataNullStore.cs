using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using USe.Common;
using USe.TradeDriver.Common;

namespace MarketDataStore
{
    class KLineDataNullStore : IKLineDataListener
    {
        #region event
        /// <summary>
        /// 通知事件。
        /// </summary>
        public event EventHandler<USeNotifyEventArgs> Notify;
        #endregion

        private int m_storeCount = 0;
        /// <summary>
        /// 已存储数量。
        /// </summary>
        public int StoreCount
        {
            get { return m_storeCount; }
        }

        /// <summary>
        /// 未存储数量。
        /// </summary>
        public int UnStoreCount
        {
            get { return 0; }
        }

        /// <summary>
        /// 是否有异常。
        /// </summary>

        public bool HasError
        {
            get { return false; }
        }

        /// <summary>
        /// 保存K线数据。
        /// </summary>
        /// <param name="kLine"></param>
        public void ReceiveKLineData(USeKLine kLine)
        {
            Interlocked.Increment(ref m_storeCount);
            //if (kLine.InstrumentCode == "cu1708")
            {
                Debug.WriteLine(string.Format("{0}@{1},Cycle:{7},Open:{2},High:{3},Low:{4},Close:{5},SettlentPrice:{6}",
                kLine.InstrumentCode, kLine.DateTime, kLine.Open, kLine.High, kLine.Low, kLine.Close, kLine.SettlementPrice, kLine.Cycle));
            }
        }

        /// <summary>
        /// 启动存储器。
        /// </summary>
        public void Start()
        {
        }

        /// <summary>
        /// 停止存储器。
        /// </summary>
        public void Stop()
        {

        }

        /// <summary>
        /// 重置。
        /// </summary>
        public void Reset()
        {
            m_storeCount = 0;
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

        public override string ToString()
        {
            return "NullStore";
        }
    }
}
