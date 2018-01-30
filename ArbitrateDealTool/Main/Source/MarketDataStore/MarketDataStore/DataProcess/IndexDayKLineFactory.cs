using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using USe.Common.TradingDay;
using System.Diagnostics;
using USe.Common.AppLogger;
using USe.Common;
using USe.Common.Manager;

namespace MarketDataStore
{
    /// <summary>
    /// 指数日K线处理器。
    /// </summary>
    internal class IndexDayKLineFactory : KLineFactory
    {
        #region
        private USeProduct m_product = null;
        private USeKLine m_kLine = null;

        private DateTime m_nextPublishTime = DateTime.MinValue;  // 下次发布时间
        private TimeSpan m_publishInterval = new TimeSpan(0, 1, 0);  // 发布间隔

        private Dictionary<string, USeMarketData> m_componentDic = null; // 各成分合约最新行情
        private bool m_allowCalc = false;  // 是否允许启动指数计算

        private List<USeInstrumentDetail> m_insDetailList = null;//合约的信息列表

        #endregion

        #region construction
        /// <summary>
        /// 构造方法。
        /// </summary>
        /// <param name="product">品种。</param>
        /// <param name="publisher">发布者。</param>
        /// <param name="tradeRange">交易时段。</param>
        /// <param name="publishInterval">发布间隔。</param>
        public IndexDayKLineFactory(USeProduct product, List<USeInstrument> instrumentList, USeKLine initKLine, TimeSpan publishInterval,
            IKLinePublisher publisher, DayTradeRange tradeRange, IAppLogger eventLogger, USeTradingInstrumentManager instrumentManager)
            : base(USeTraderProtocol.GetVarietiesIndexCode(product),
                   publisher, tradeRange, eventLogger)
        {

            Debug.Assert(product.PriceTick > 0);
            Debug.Assert(instrumentList != null && instrumentList.Count > 0);

            m_publishInterval = publishInterval;
            m_nextPublishTime = DateTime.Now.AddTicks(publishInterval.Ticks);
            m_product = product;
            m_componentDic = new Dictionary<string, USeMarketData>();
            foreach (USeInstrument instrument in instrumentList)
            {
                m_componentDic.Add(instrument.InstrumentCode, null);
            }

            if (initKLine != null)
            {
                Debug.Assert(initKLine.InstrumentCode == USeTraderProtocol.GetVarietiesIndexCode(product).InstrumentCode);
                m_kLine = initKLine;
            }
            else
            {
                m_kLine = CreateDefaultKLine();
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

            //所有合约都有行情了，开始计算，每天开盘不管是否活跃，每个合约都有一笔集合竞价过来产生
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
            Debug.Assert(m_kLine != null);

            Debug.Assert(m_componentDic.ContainsKey(marketData.Instrument.InstrumentCode));

            if (m_componentDic.ContainsKey(marketData.Instrument.InstrumentCode))
            {
                m_componentDic[marketData.Instrument.InstrumentCode] = marketData;
            }
            else
            {
                return;
            }

            if (m_allowCalc == false)
            {
                CheckIsAllowCalc();
            }

            if (m_allowCalc == false) return;

            CalcIndexKLine();

            if (DateTime.Now >= m_nextPublishTime || m_kLine.SettlementPrice > 0m)
            {
                m_publisher.PublishKLine(m_kLine.Clone());

                m_nextPublishTime = DateTime.Now.AddTicks(m_publishInterval.Ticks);
            }
        }

        /// <summary>
        /// 创建KLine。
        /// </summary>
        /// <param name="marketData"></param>
        /// <returns></returns>
        private USeKLine CreateDefaultKLine()
        {
            USeKLine kline = new USeKLine()
            {
                InstrumentCode = m_instrument.InstrumentCode,
                Market = m_instrument.Market,
                Cycle = USeCycleType.Day,
                DateTime = GetCycleTime(DateTime.Now),
                Open = 0m,
                High = 0m,
                Low = 0m,
                Close = 0m,
                Volumn = 0,
                Turnover = 0m,
                OpenInterest = 0m,
                SettlementPrice = 0m,
                PreSettlementPrice = 0m,
                AskVolumn = 0,
                BidVolumn = 0,
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

        private string GetName(string instrument)
        {
            if (instrument == null || instrument == "") return "";

            string result = "";

            foreach (char c in instrument)
            {
                if (c >= '0' && c <= '9')
                {
                    break;
                }

                result = result + c;

            }
            return result;
        }

        /// <summary>
        /// 计算指数。
        /// </summary>
        /// <returns></returns>
        private void CalcIndexKLine()
        {
            int totalVolume = 0;  // 总成交量
            decimal totalTurnOver = 0m; // 总成交金额
            decimal totalOpenInterest = 0m; //总持仓量
            decimal totalSendimentaryMoney = 0m; //总资金沉淀
            decimal totalFlowFund = 0m;   //总资金流向
            decimal totalSpeculateRadio = 0m;//投机度

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

                //if (GetName(marketData.Instrument.InstrumentCode) == "ZC")
                //{
                //    string str = string.Format("InsName:{0} ,OpenInterest:{1} ,LastPrice:{2} ,perSharesContract:{3},exchangeLongMarginRatio:{4},totalSendimentaryMoney:{5}",
                //       marketData.Instrument.InstrumentCode, marketData.OpenInterest, marketData.LastPrice, perSharesContract, exchangeLongMarginRatio, totalSendimentaryMoney);
                    //m_eventLogger.WriteError(str);
                //}

                if (marketData.OpenInterest != 0m)
                {
                    totalSpeculateRadio = marketData.Volume / marketData.OpenInterest;//投机度= 成交量/总持仓
                }
                else
                {
                    totalSpeculateRadio = 0m;//投机度= 成交量/总持仓
                }
            }

            decimal indexValue = 0m;
            if (totalOpenInterest > 0)
            {
                foreach (USeMarketData marketData in m_componentDic.Values)
                {
                    if (marketData == null) continue;

                    indexValue += (marketData.LastPrice * marketData.OpenInterest / totalOpenInterest);
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
            m_kLine.SendimentaryMoney = totalSendimentaryMoney;
            m_kLine.SpeculateRadio = totalSpeculateRadio;//指数投机度
            //[yangming]有问题，待确定合适算法吧
            if (m_kLine.PreSettlementPrice <= 0)
            {
                bool isReady = true;
                foreach (USeMarketData marketData in m_componentDic.Values)
                {
                    if (marketData == null || marketData.PreSettlementPrice <= 0)
                    {
                        isReady = false;
                        break;
                    }
                }
                if (isReady)
                {
                    decimal totalPreOpenInterest = 0m;
                    foreach (USeMarketData marketData in m_componentDic.Values)
                    {
                        Debug.Assert(marketData != null);
                        if (marketData == null) continue;
                        totalPreOpenInterest += marketData.PreOpenInterest;
                    }

                    if (totalPreOpenInterest > 0)
                    {
                        //计算昨日结算价
                        decimal preSettelentPrice = 0m;
                        foreach (USeMarketData marketData in m_componentDic.Values)
                        {
                            if (marketData == null) continue;
                            preSettelentPrice += marketData.PreSettlementPrice * marketData.PreOpenInterest / totalPreOpenInterest;
                        }
                        preSettelentPrice = USeMath.Round(preSettelentPrice, m_product.PriceTick);
                        m_kLine.PreSettlementPrice = preSettelentPrice;
                    }
                }
            }

            if (m_kLine.SettlementPrice <= 0)
            {
                bool isReady = true;
                foreach (USeMarketData marketData in m_componentDic.Values)
                {
                    if (marketData == null || marketData.SettlementPrice <= 0)
                    {
                        isReady = false;
                        break;
                    }
                }

                if (isReady)
                {
                    decimal indexSettlemetPrice = 0m;
                    foreach (USeMarketData marketData in m_componentDic.Values)
                    {
                        Debug.Assert(marketData != null);
                        if (marketData == null) continue;
                        indexSettlemetPrice = marketData.SettlementPrice * marketData.OpenInterest / totalOpenInterest;
                    }
                    indexSettlemetPrice = USeMath.Round(indexSettlemetPrice, m_product.PriceTick);
                    m_kLine.SettlementPrice = indexSettlemetPrice;
                }
            }
        }
    }
}
