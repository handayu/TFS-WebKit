using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using USe.TradeDriver.Common;
using USe.Common.TradingDay;
using USe.Common.AppLogger;

namespace MarketDataStore
{
    /// <summary>
    /// 分钟K线生成器。
    /// </summary>
    class MinKLineFactory :KLineFactory
    {
        #region member
        private USeKLine m_kLine = null;
        private USeCycleType m_cycle = USeCycleType.Min1;
        private bool m_isMainContract = false;
        private USeInstrument m_mainContract = null;
        #endregion

        #region construction
        /// <summary>
        /// 构造方法。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <param name="klinePublisher">发布者。</param>
        /// <param name="cycle">周期。</param>
        /// <param name="eventLogger">日志。</param>
        public MinKLineFactory (USeInstrument instrument,IKLinePublisher klinePublisher,DayTradeRange tradeRange,IAppLogger eventLogger, USeCycleType cycle,bool isMainContract)
            :base(instrument,klinePublisher,tradeRange,eventLogger)
        {
            if(cycle != USeCycleType.Min1)
            {
                throw new NotSupportedException(string.Format("Not support {0} ",cycle));
            }

            m_cycle = cycle;
            m_isMainContract = isMainContract;

            if (isMainContract)
            {
                string varieties = USeTraderProtocol.GetVarieties(instrument.InstrumentCode);
                m_mainContract = USeTraderProtocol.GetMainContractCode(varieties, instrument.Market);
            }
        }
        #endregion

        /// <summary>
        /// 周期类型。
        /// </summary>
        public override USeCycleType CycelType
        {
            get
            {
                return m_cycle;
            }
        }

        /// <summary>
        /// 是否为主力合约。
        /// </summary>
        private bool IsMainContract
        {
            get { return m_isMainContract; }
        }

        /// <summary>
        /// 行情更新。
        /// </summary>
        /// <remarks>行情数据。</remarks>
        /// <returns></returns>
        public override void UpdateMarketData(USeMarketData marketData)
        {
            Debug.Assert(marketData.Instrument.InstrumentCode == m_instrument.InstrumentCode);
            DateTime updateTime = marketData.UpdateTime;
            //if(m_lastUpdateTime.HasValue)
            //{
            //    Debug.Assert(m_lastUpdateTime.Value <= updateTime);
            //}
            //Debug.Assert(m_volume <= marketData.Volume);
            //m_lastUpdateTime = updateTime;
            //m_volume = marketData.Volume;
            if (m_tradeRange.IsTradeTime(updateTime.TimeOfDay) == false)
            {
                //非交易时间行情忽略
                return;
            }

            if (m_kLine == null)
            {
                if (m_tradeRange.IsEndTime(updateTime.TimeOfDay) == false)
                {
                    m_kLine = CreateFirstKLine(marketData);
                }
            }
            else
            {
                if (m_tradeRange.IsEndTime(updateTime.TimeOfDay))
                {
                    //交易时段结束时间行情,发布KLine并且清空
                    UpdateKLine(m_kLine, marketData);
                    m_publisher.PublishKLine(m_kLine.Clone());
                    if (m_isMainContract)
                    {
                        USeKLine mainKLine = m_kLine.Clone();
                        mainKLine.InstrumentCode = m_mainContract.InstrumentCode;
                        m_publisher.PublishKLine(mainKLine);
                    }
                    m_kLine = null;
                }
                else
                {
                    DateTime cycelTime = GetCycleTime(marketData.UpdateTime);
                    if (IsSameCycle(m_kLine.DateTime, cycelTime))
                    {
                        UpdateKLine(m_kLine, marketData);
                    }
                    else
                    {
                        m_publisher.PublishKLine(m_kLine.Clone());
                        if (m_isMainContract)
                        {
                            USeKLine mainKLine = m_kLine.Clone();
                            mainKLine.InstrumentCode = m_mainContract.InstrumentCode;
                            m_publisher.PublishKLine(mainKLine);
                        }

                        m_kLine = CreateFirstKLine(marketData);
                    }
                }
            }
        }

        #region private methods
        /// <summary>
        /// 创建KLine。
        /// </summary>
        /// <param name="marketData"></param>
        /// <returns></returns>
        private USeKLine CreateFirstKLine(USeMarketData marketData)
        {
            USeKLine kline = new USeKLine() {
                InstrumentCode = marketData.Instrument.InstrumentCode,
                Market = marketData.Instrument.Market,
                Cycle = USeCycleType.Min1,
                DateTime = GetCycleTime(marketData.UpdateTime),
                Open = marketData.LastPrice,
                High = marketData.LastPrice,
                Low = marketData.LastPrice,
                Close = marketData.LastPrice,
                Volumn = marketData.Volume,
                Turnover = marketData.Turnover,
                OpenInterest = marketData.OpenInterest,
                AskVolumn = 0,
                BidVolumn = 0,
                AvgPrice = marketData.AvgPrice,
            };
            return kline;
        }


        /// <summary>
        /// 更新K线。
        /// </summary>
        /// <param name="kLine"></param>
        /// <param name="marketData"></param>
        private void UpdateKLine(USeKLine kLine,USeMarketData marketData)
        {
            if (marketData.Volume < kLine.Volumn) return;

            int volumeDiff = marketData.Volume - kLine.Volumn;
            USeActiveSide activeSide = GetActiveSide(marketData);
            int askVolume = 0;
            int bidVolume = 0;
            if(activeSide == USeActiveSide.Ask)
            {
                askVolume = volumeDiff;
            }
            else if(activeSide == USeActiveSide.Bid)
            {
                bidVolume = volumeDiff;
            }
            else
            {
                Debug.Assert(activeSide == USeActiveSide.None);
                askVolume = (int)(volumeDiff / 2);
                bidVolume = volumeDiff - askVolume;
            }

            //更新
            if (kLine.High < marketData.LastPrice)
            {
                kLine.High = marketData.LastPrice;
            }
            if (kLine.Low > marketData.LastPrice)
            {
                kLine.Low = marketData.LastPrice;
            }
            kLine.Close = marketData.LastPrice;
            kLine.Volumn = marketData.Volume;
            kLine.Turnover = marketData.Turnover;
            kLine.OpenInterest = marketData.OpenInterest;

            kLine.AskVolumn += askVolume;
            kLine.BidVolumn += bidVolume;
            kLine.AvgPrice = marketData.AvgPrice;
        }

        /// <summary>
        /// 是否为同一周期。
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        private bool IsSameCycle(DateTime time1, DateTime time2)
        {
            //先只做一分钟判定
            if (time1.Minute == time2.Minute &&
                time1.Hour == time2.Hour &&
                time1.Day == time2.Day &&
                time1.Month == time2.Month &&
                time1.Year == time2.Year)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取周期时间。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private DateTime GetCycleTime(DateTime time)
        {
            DateTime cycelTime = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
            return cycelTime;
        }
        #endregion
    }
}
