using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using USe.Common.TradingDay;
using System.Diagnostics;
using USe.Common.AppLogger;
using USe.Common.Manager;

namespace MarketDataStore
{
    /// <summary>
    /// 日K线处理器。
    /// </summary>
    internal class DayKLineFactory :KLineFactory
    {
        #region
        private USeKLine m_kLine = null;
        private DateTime m_nextPublishTime = DateTime.MinValue;  // 下次发布时间
        private TimeSpan m_publishInterval = new TimeSpan(0, 1, 0);  // 发布间隔,单位:秒

        private bool m_isMainContract = false;
        private USeInstrument m_mainContractCode = null;

        private List<USeInstrumentDetail> m_insDetailList = null;

        #endregion

        #region construction
        /// <summary>
        /// 构造方法。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <param name="publisher">发布者。</param>
        /// <param name="tradeRange">交易时段。</param>
        /// <param name="publishInterval">发布间隔(单位:秒)。</param>
        public DayKLineFactory(USeInstrument instrument, USeKLine initKLine, IKLinePublisher publisher, DayTradeRange tradeRange, IAppLogger eventLogger, TimeSpan publishInterval,bool isMainContract, USeTradingInstrumentManager instrumentManager)
            : base(instrument, publisher, tradeRange, eventLogger)
        {
            m_publishInterval = publishInterval;
            m_nextPublishTime = DateTime.Now.AddTicks(publishInterval.Ticks);

            m_isMainContract = isMainContract;

            if (initKLine != null)
            {
                Debug.Assert(initKLine.InstrumentCode == instrument.InstrumentCode);
                m_kLine = initKLine;
            }

            if (isMainContract)
            {
                string varieties = USeTraderProtocol.GetVarieties(instrument.InstrumentCode);
                m_mainContractCode = USeTraderProtocol.GetMainContractCode(varieties, instrument.Market);
            }

            //获取该数据库下的合约详细信息
            try
            {
                Debug.Assert(instrumentManager != null);
                m_insDetailList = instrumentManager.GetAllInstrumentDetails();
            }
            catch (Exception ex)
            {
                throw new Exception("IndexDayKLineFactory 获取全部合约详细信息异常:" + ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 周期类型。
        /// </summary>
        public override USeCycleType CycelType
        {
            get { return USeCycleType.Day; }
        }

        /// <summary>
        /// 是否为主力合约。
        /// </summary>
        private bool IsMainContract
        {
            get { return m_isMainContract; }
        }


        private int m_diff = 0;
        /// <summary>
        /// 行情更新。
        /// </summary>
        /// <remarks>行情数据。</remarks>
        /// <returns></returns>
        public override void UpdateMarketData(USeMarketData marketData)
        {
            if (m_kLine == null)
            {
                m_kLine = CreateFirstKLine(marketData);
            }
            else
            {
                if (marketData.Volume < m_kLine.Volumn) return;

                if (GetCycleTime(marketData.UpdateTime) == m_kLine.DateTime)
                {
                    int volumeDiff = marketData.Volume - m_kLine.Volumn;
                    USeActiveSide activeSide = GetActiveSide(marketData);
                    int askVolume = 0;
                    int bidVolume = 0;
                    if (activeSide == USeActiveSide.Ask)
                    {
                        askVolume = volumeDiff;
                    }
                    else if (activeSide == USeActiveSide.Bid)
                    {
                        bidVolume = volumeDiff;
                    }
                    else
                    {
                        Debug.Assert(activeSide == USeActiveSide.None);
                        askVolume = (int)(volumeDiff / 2);
                        bidVolume = volumeDiff - askVolume;
                    }

                    m_kLine.Open = marketData.OpenPrice;
                    m_kLine.High = marketData.HighPrice;
                    m_kLine.Low = marketData.LowPrice;
                    m_kLine.Close = marketData.LastPrice;
                    m_kLine.Volumn = marketData.Volume;
                    m_kLine.Turnover = marketData.Turnover;
                    m_kLine.OpenInterest = marketData.OpenInterest;
                    m_kLine.SettlementPrice = marketData.SettlementPrice;
                    m_kLine.PreSettlementPrice = marketData.PreSettlementPrice;
                    m_kLine.BidVolumn += bidVolume;
                    m_kLine.AskVolumn += askVolume;
                    m_kLine.AvgPrice = marketData.AvgPrice;
                    if(marketData.OpenInterest != 0m)
                    {
                        m_kLine.SpeculateRadio = marketData.Volume / marketData.OpenInterest;
                    }
                    else
                    {
                        m_kLine.SpeculateRadio = 0m;
                    }

                    Debug.Assert(m_kLine.BidVolumn + m_kLine.AskVolumn == m_kLine.Volumn);

                    //计算资金总沉淀(盘中就用最新价计算资金沉淀，待下午结算价出来之后，按照结算价再更新一次)
                    int perSharesContract = GetInstrumentPerSharesContract(marketData.Instrument.InstrumentCode);//合约规模
                    decimal exchangeLongMarginRatio = GetExchangeLongMarginRatio(marketData.Instrument.InstrumentCode);//保证金
                    m_kLine.SendimentaryMoney = marketData.OpenInterest * marketData.LastPrice * perSharesContract * exchangeLongMarginRatio;//资金沉淀

                }
                else
                {
                    m_eventLogger.WriteError(string.Format("[YM]{0} receive different data,Code:{3},KLindDate:{1},MarketData:{2}",
                        ToString(), m_kLine.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        marketData.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        m_instrument.InstrumentCode.ToString()));
                    //Debug.Assert(false);
                    m_publisher.PublishKLine(m_kLine.Clone());
                    //如果是检查到该合约属于主力合约，重新克隆发送一遍存在day_kline里面作为**9999
                    if (m_isMainContract)
                    {
                        USeKLine mainKLine = m_kLine.Clone();
                        mainKLine.InstrumentCode = m_mainContractCode.InstrumentCode;
                        m_publisher.PublishKLine(mainKLine);
                    }
                    m_nextPublishTime = DateTime.Now.AddTicks(m_publishInterval.Ticks);
                    m_kLine = CreateFirstKLine(marketData);
                    return;
                }
            }

            if (DateTime.Now >= m_nextPublishTime || m_kLine.SettlementPrice >0m)
            {
                m_publisher.PublishKLine(m_kLine.Clone());
                //如果是检查到该合约属于主力合约，重新克隆发送一遍存在day_kline里面作为**9999
                if (m_isMainContract)
                {
                    USeKLine mainKLine = m_kLine.Clone();
                    mainKLine.InstrumentCode = m_mainContractCode.InstrumentCode;
                    m_publisher.PublishKLine(mainKLine);
                }
                m_nextPublishTime = DateTime.Now.AddTicks(m_publishInterval.Ticks);
            }
        }

        /// <summary>
        /// 创建KLine。
        /// </summary>
        /// <param name="marketData"></param>
        /// <returns></returns>
        private USeKLine CreateFirstKLine(USeMarketData marketData)
        {
            USeKLine kline = new USeKLine() {
                InstrumentCode = m_instrument.InstrumentCode,
                Market = m_instrument.Market,
                Cycle = USeCycleType.Day,
                DateTime = GetCycleTime(marketData.UpdateTime),
                Open = marketData.OpenPrice,
                High = marketData.HighPrice,
                Low = marketData.LowPrice,
                Close = marketData.LastPrice,
                Volumn = marketData.Volume,
                Turnover = marketData.Turnover,
                OpenInterest = marketData.OpenInterest,
                SettlementPrice = marketData.SettlementPrice,
                PreSettlementPrice = marketData.PreSettlementPrice,
                AskVolumn = (int)(marketData.Volume / 2),
                BidVolumn = marketData.Volume - (int)(marketData.Volume / 2),
                SendimentaryMoney = 0m,
                FlowFund = 0m,
                SpeculateRadio = 0m
                
            };
            return kline;
        }

        /// <summary>
        /// 获取周期时间。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private DateTime GetCycleTime(DateTime time)
        {
            DateTime tradeDay = m_tradeRange.GetTradeDay(time);
            return tradeDay;
        }


        /// <summary>
        /// 获取合约乘数
        /// </summary>
        /// <param name="instrumentCode"></param>
        /// <returns></returns>
        private int GetInstrumentPerSharesContract(string instrumentCode)
        {
            foreach (USeInstrumentDetail detail in m_insDetailList)
            {
                if (detail.Instrument.InstrumentCode == instrumentCode)
                {
                    return detail.VolumeMultiple;
                }
            }

            return 0;
        }

        /// <summary>
        /// 获取合约交易所保证金
        /// </summary>
        /// <param name="instrumentCode"></param>
        /// <returns></returns>
        private decimal GetExchangeLongMarginRatio(string instrumentCode)
        {
            foreach (USeInstrumentDetail detail in m_insDetailList)
            {
                if (detail.Instrument.InstrumentCode == instrumentCode)
                {
                    return detail.ExchangeLongMarginRatio;
                }
            }

            return 0m;
        }
    }
}
