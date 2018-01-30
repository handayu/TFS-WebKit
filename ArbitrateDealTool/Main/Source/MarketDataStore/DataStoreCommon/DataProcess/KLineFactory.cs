using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using USe.Common.TradingDay;
using USe.Common.AppLogger;

namespace DataStoreCommon
{
    /// <summary>
    /// K线生成器。
    /// </summary>
    public abstract class KLineFactory
    {
        #region member
        protected USeInstrument m_instrument = null;
        protected IKLinePublisher m_publisher = null;
        protected DayTradeRange m_tradeRange = null;
        protected IAppLogger m_eventLogger = null;
        #endregion

        #region construction
        /// <summary>
        /// 构造方法。
        /// </summary>
        /// <param name="instrument">处理合约。</param>
        /// <param name="publisher">发布者。</param>
        /// <param name="tradeRange">交易时段。</param>
        public KLineFactory(USeInstrument instrument,IKLinePublisher publisher,DayTradeRange tradeRange,IAppLogger eventLogger)
        {
            m_instrument = instrument;
            m_publisher = publisher;
            m_tradeRange = tradeRange;
            m_eventLogger = eventLogger;
        }
        #endregion

        /// <summary>
        /// 周期类型。
        /// </summary>
        public abstract USeCycleType CycelType { get; }

        /// <summary>
        /// 行情更新。
        /// </summary>
        /// <remarks>行情数据。</remarks>
        /// <returns></returns>
        public abstract void UpdateMarketData(USeMarketData marketData);

        /// <summary>
        /// 获取内外盘方向。
        /// </summary>
        /// <param name="marketData">行情数据。</param>
        /// <returns></returns>
        protected USeActiveSide GetActiveSide(USeMarketData marketData)
        {
            decimal askDiff = Math.Abs(marketData.LastPrice - marketData.AskPrice);
            decimal bidDiff = Math.Abs(marketData.LastPrice - marketData.BidPrice);

            if (bidDiff < askDiff)
            {
                return USeActiveSide.Ask;
            }
            else if (bidDiff > askDiff)
            {
                return USeActiveSide.Bid;
            }
            else
            {
                return USeActiveSide.None;
            }
        }
    }
}
