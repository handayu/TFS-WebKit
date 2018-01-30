using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using USe.TradeDriver.Common;
using USe.Common.TradingDay;
using USe.Common.AppLogger;
using USe.Common;
using USe.Common.Manager;

namespace MarketDataStore
{
    /// <summary>
    /// 指数分钟K线生成器。
    /// </summary>
    class IndexMinKLineFactory : KLineFactory
    {
        #region member
        private USeProduct m_product = null;
        private USeCycleType m_cycle = USeCycleType.Min1;

        private USeKLine m_kLine = null;
        private Dictionary<string, USeMarketData> m_componentDic = null; // 各成分合约最新行情
        private bool m_allowCalc = false;  // 是否允许启动指数计算
        private List<USeInstrumentDetail> m_insDetailList = null;
        #endregion

        #region construction
        /// <summary>
        /// 构造方法。
        /// </summary>
        /// <param name="indexInstrument">指数合约。</param>
        /// <param name="klinePublisher">发布者。</param>
        /// <param name="cycle">周期。</param>
        /// <param name="eventLogger">日志。</param>
        public IndexMinKLineFactory(USeProduct product, List<USeInstrument> instrumentList,USeCycleType cycle, IKLinePublisher klinePublisher,DayTradeRange tradeRange,IAppLogger eventLogger,USeTradingInstrumentManager instrumentManager)
            :base(USeTraderProtocol.GetVarietiesIndexCode(product),
                 klinePublisher,tradeRange,eventLogger)
        {
            if(cycle != USeCycleType.Min1)
            {
                throw new NotSupportedException(string.Format("Not support {0} ",cycle));
            }
            Debug.Assert(product.PriceTick > 0);
            Debug.Assert(instrumentList != null && instrumentList.Count > 0);

            m_cycle = cycle;
            m_product = product;
            m_componentDic = new Dictionary<string, USeMarketData>();
            foreach (USeInstrument instrument in instrumentList)
            {
                m_componentDic.Add(instrument.InstrumentCode, null);
            }

            //if (initKLine != null)
            //{
            //    Debug.Assert(initKLine.InstrumentCode == USeTraderProtocol.GetVarietiesIndexCode(product).InstrumentCode);
            //    m_kLine = initKLine;
            //}

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
            get
            {
                return m_cycle;
            }
        }

        /// <summary>
        /// 检查是否允许进行指数计算。
        /// </summary>
        private void CheckIsAllowCalc()
        {
            bool hasMarketData = false;
            foreach (KeyValuePair<string, USeMarketData> item in m_componentDic)
            {
                if (item.Value != null)
                {
                    hasMarketData = true;
                    break;
                }
            }
            //所有合约都有行情了，开始计算
            if (hasMarketData)
            {
                m_allowCalc = true;
            }
        }

        /// <summary>
        /// 行情更新。
        /// </summary>
        /// <remarks>行情数据。</remarks>
        /// <returns></returns>
        public override void UpdateMarketData(USeMarketData marketData)
        {
            Debug.Assert(m_componentDic.ContainsKey(marketData.Instrument.InstrumentCode));
            if (m_componentDic.ContainsKey(marketData.Instrument.InstrumentCode) == false)
            {
                return;
            }

            m_componentDic[marketData.Instrument.InstrumentCode] = marketData;

            if (m_allowCalc == false)
            {
                CheckIsAllowCalc();
            }

            if (m_allowCalc == false) return;

                DateTime updateTime = marketData.UpdateTime;

            if (m_tradeRange.IsTradeTime(updateTime.TimeOfDay) == false)
            {
                //非交易时间行情忽略
                return;
            }

            if (m_kLine == null)
            {
                if (m_tradeRange.IsEndTime(updateTime.TimeOfDay) == false)
                {
                    //if (marketData.Instrument.InstrumentCode.StartsWith("j"))
                    //{
                    //    Debug.WriteLine(string.Format("PublishKLine,FirstKline{0},{1}",
                    //        marketData.Instrument.InstrumentCode, marketData.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
                    //}

                    m_kLine = CreateNewKLine(updateTime);
                }
            }
            else
            {
                if (m_tradeRange.IsEndTime(updateTime.TimeOfDay))
                {
                    //交易时段结束时间行情,发布KLine并且清空
                    CalcIndexKLine();
                    m_publisher.PublishKLine(m_kLine.Clone());
                    m_kLine = null;
                }
                else
                {
                    DateTime cycelTime = GetCycleTime(marketData.UpdateTime);
                    if (IsSameCycle(m_kLine.DateTime, cycelTime) == false)
                    {
                        if (cycelTime > m_kLine.DateTime)
                        {
                            m_publisher.PublishKLine(m_kLine.Clone());

                            //if (m_kLine.InstrumentCode.StartsWith("j"))
                            //{
                            //    Debug.WriteLine(string.Format("PublishKLine,{0},{1},{2},{3}",
                            //        m_kLine.InstrumentCode, m_kLine.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            //        marketData.Instrument.InstrumentCode, marketData.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")));
                            //}
                            m_kLine = CreateNewKLine(cycelTime);
                        }
                    }

                    CalcIndexKLine();
                }
            }
        }

        #region private methods
        /// <summary>
        /// 创建KLine。
        /// </summary>
        /// <param name="marketData"></param>
        /// <returns></returns>
        private USeKLine CreateNewKLine(DateTime time)
        {
            USeKLine kline = new USeKLine() {
                InstrumentCode = m_instrument.InstrumentCode,
                Market = m_instrument.Market,
                Cycle = USeCycleType.Min1,
                DateTime = GetCycleTime(time),
                Open = 0,
                High = 0,
                Low = 0,
                Close = 0,
                Volumn = 0,
                Turnover = 0,
                OpenInterest = 0,
                SendimentaryMoney = 0m,
                FlowFund = 0m
            };
            return kline;
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


        /// <summary>
        /// 更新K线。
        /// </summary>
        /// <param name="kLine"></param>
        /// <param name="marketData"></param>
        private void UpdateKLine(USeKLine kLine,USeMarketData marketData)
        {
            //Debug.Assert(marketData.Volume >= kLine.Volumn);
            //Debug.Assert(marketData.UpdateTime >= kLine.DateTime);

            if (marketData.Volume < kLine.Volumn) return;

            //更新
            if (m_kLine.High < marketData.LastPrice)
            {
                m_kLine.High = marketData.LastPrice;
            }
            if (m_kLine.Low > marketData.LastPrice)
            {
                m_kLine.Low = marketData.LastPrice;
            }
            m_kLine.Close = marketData.LastPrice;
            m_kLine.Volumn = marketData.Volume;
            m_kLine.Turnover = marketData.Turnover;
            m_kLine.OpenInterest = marketData.OpenInterest;
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

        /// <summary>
        /// 计算指数。
        /// </summary>
        /// <returns></returns>
        private void CalcIndexKLine()
        {
            int totalVolume = 0;  // 总成交量
            decimal totalTurnOver = 0m; // 总成交金额
            decimal totalOpenInterest = 0m; //总持仓量
            decimal totalSendimentaryMoney = 0m; //总资金撤
            decimal totalFlowFund = 0m;   //总资金流向

            foreach (USeMarketData marketData in m_componentDic.Values)
            {
                if (marketData == null) continue;

                Debug.Assert(marketData != null);
                totalVolume += marketData.Volume;
                totalTurnOver += marketData.Turnover;
                totalOpenInterest += marketData.OpenInterest;

                int perSharesContract = GetInstrumentPerSharesContract(marketData.Instrument.InstrumentCode);//合约规模
                decimal exchangeLongMarginRatio = GetExchangeLongMarginRatio(marketData.Instrument.InstrumentCode);//保证金
                totalSendimentaryMoney += marketData.OpenInterest * marketData.LastPrice * perSharesContract * exchangeLongMarginRatio;//资金沉淀

            }

            decimal indexValue = 0m;
            if (totalOpenInterest > 0)
            {
                foreach (USeMarketData marketData in m_componentDic.Values)
                {
                    if (marketData == null) continue;
                    indexValue += marketData.LastPrice * marketData.OpenInterest / totalOpenInterest;
                }
                indexValue = USeMath.Round(indexValue, m_product.PriceTick);
            }
            if (indexValue <= 0) return;

            if (m_kLine.Open <= 0m)
            {
                m_kLine.Open = indexValue;
            }
            if (m_kLine.High < indexValue)
            {
                m_kLine.High = indexValue;
            }
            if (m_kLine.Low <= 0m)
            {
                m_kLine.Low = indexValue;
            }
            else if (m_kLine.Low < indexValue)
            {
                m_kLine.Low = indexValue;
            }
            m_kLine.Close = indexValue;
            m_kLine.Volumn = totalVolume;
            m_kLine.Turnover = totalTurnOver;
            m_kLine.OpenInterest = totalOpenInterest;
        }

    }
}
