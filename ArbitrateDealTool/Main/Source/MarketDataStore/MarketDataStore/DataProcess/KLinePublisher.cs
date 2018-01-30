using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using USe.TradeDriver.Common;

namespace MarketDataStore
{
    /// <summary>
    /// K线行情发布者。
    /// </summary>
    class KLinePublisher:IKLinePublisher
    {
        #region 
        private List<IKLineDataListener> m_marketDataStore = null;
        #endregion

        #region construction
        public KLinePublisher()
        {
            m_marketDataStore = new List<IKLineDataListener>();
        }
        #endregion 

        /// <summary>
        /// 设置行情数据保存器。
        /// </summary>
        /// <param name="marketDataStore"></param>
        public void SetMarketDataStore(List<IKLineDataListener> storers)
        {
            m_marketDataStore.Clear();
            if (storers != null)
            {
                m_marketDataStore.AddRange(storers);
            }
            else
            {
                m_marketDataStore.Add(new KLineDataNullStore());
            }
        }

        /// <summary>
        /// 发布K线。
        /// </summary>
        /// <param name="kLine"></param>
        /// <param name="cycle"></param>
        public void PublishKLine(USeKLine kLine)
        {
            foreach (IKLineDataListener storer in m_marketDataStore)
            {
                storer.ReceiveKLineData(kLine);
            }
        }
    }
}
