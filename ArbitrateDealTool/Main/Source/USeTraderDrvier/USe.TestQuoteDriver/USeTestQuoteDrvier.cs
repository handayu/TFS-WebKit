using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;

namespace USe.TradeDriver.Test
{
    public partial class USeTestQuoteDriver : USeQuoteDriver
    {
        private object m_object = new object();

        private Dictionary<string, USeInstrument> m_instrumentDic = new Dictionary<string, USeInstrument>(); // 产品列表
        private Dictionary<string, USeMarketData> m_marketDataList = new Dictionary<string, USeMarketData>(); // 期货行情信息。
        /// <summary>
        /// 连接行情服务器
        /// </summary>
        public override void ConnectServer()
        {

            FireDriverStateChanged(USeQuoteDriverState.Connecting, "");

            FireDriverStateChanged(USeQuoteDriverState.Connected, "");

        }

        /// <summary>
        /// 断开行情服务器
        /// </summary>
        public override void DisConnectServer()
        {

            FireDriverStateChanged(USeQuoteDriverState.DisConnected, "");
        }

        /// <summary>
        /// 登录行情服务器
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        public override void Login(string brokerId, string account, string password)
        {
            FireDriverStateChanged(USeQuoteDriverState.LoggingOn, "");
            FireDriverStateChanged(USeQuoteDriverState.LoggedOn, "");
            FireDriverStateChanged(USeQuoteDriverState.Ready, "");


            CreatInitMarketData_cu1706();
            CreatInitMarketData_cu1707();
            CreatInitMarketData_cu1708();
            CreatInitMarketData_cu1709();
        }

        /// <summary>
        /// 登出行情服务器
        /// </summary>
        public override void Logout()
        {
            FireDriverStateChanged(USeQuoteDriverState.DisConnected, "");
        }

        /// <summary>
        /// 查询产品行情
        /// </summary>
        /// <param name="product">被查询产品</param>
        /// <returns>被查询产品行情信息</returns>
        public override USeMarketData Query(USeInstrument product)
        {
            USeMarketData marketData = null;
            if (m_marketDataList.TryGetValue(product.InstrumentCode, out marketData))
            {
                return marketData.Clone();
            }
            else
            {
                return new USeMarketData()
                {
                    Instrument = product,
                    LastPrice = 0
                };
            }
        }

        /// <summary>
        /// 查询产品行情
        /// </summary>
        /// <param name="product">被查询产品</param>
        /// <returns>被查询产品行情信息</returns>
        public override USeMarketData QuickQuery(USeInstrument product)
        {
            return Query(product);
        }

        /// <summary>
        /// 订阅行情
        /// </summary>
        /// <param name="products">订阅产品列表</param>
        public override void Subscribe(List<USeInstrument> products)
        {
            try
            {
                string[] instruments = new string[products.Count];
                lock (m_object)
                {
                    for (int i = 0; i < products.Count; i++)
                    {
                        m_instrumentDic[products[i].InstrumentCode] = products[i].Clone();
                        instruments[i] = products[i].InstrumentCode;
                    }

                    if (m_instrumentDic.Count == 0) return;
                    //构造数据并返回初始数据
                    foreach (USeMarketData marketData in m_marketDataList.Values)
                    {
                        try
                        {
                            this.FireOnMarketDataChanged(marketData);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("FireMarketDataEvent Failed:{0}" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 订阅行情
        /// </summary>
        /// <param name="products">订阅产品</param>
        public override void Subscribe(USeInstrument instrument)
        {

            lock (m_object)
            {
                m_instrumentDic[instrument.InstrumentCode] = instrument.Clone();
            }

        }

        public USeMarketData CreatInitMarketData_cu1706()
        {
            USeMarketData market_data_cu1706 = new USeMarketData();
            market_data_cu1706.Instrument = new USeInstrument("cu1706", "沪铜1706", USeMarket.SHFE);
            market_data_cu1706.LastPrice = 45240;
            market_data_cu1706.AskPrice = 45250;
            market_data_cu1706.BidPrice = 45230;

            market_data_cu1706.OpenPrice = 45240;
            market_data_cu1706.HighPrice = 45240;
            market_data_cu1706.LowPrice = 45240;
            market_data_cu1706.ClosePrice = 45240;
            market_data_cu1706.PreClosePrice = 45240;
            market_data_cu1706.PreSettlementPrice = 45200;
            market_data_cu1706.AskSize = 10;
            market_data_cu1706.BidSize = 8;

            m_marketDataList.Add(market_data_cu1706.Instrument.InstrumentCode, market_data_cu1706);
            return market_data_cu1706;

        }
        public USeMarketData CreatInitMarketData_cu1707()
        {
            USeMarketData market_data_cu1707 = new USeMarketData();
            market_data_cu1707.Instrument = new USeInstrument("cu1707", "沪铜1707", USeMarket.SHFE);

            market_data_cu1707.LastPrice = 45280;
            market_data_cu1707.AskPrice = 45290;
            market_data_cu1707.BidPrice = 45270;

            market_data_cu1707.OpenPrice = 45280;
            market_data_cu1707.HighPrice = 45280;
            market_data_cu1707.LowPrice = 45280;
            market_data_cu1707.ClosePrice = 45280;
            market_data_cu1707.PreClosePrice = 45280;
            market_data_cu1707.PreSettlementPrice = 45210;
            market_data_cu1707.AskSize = 20;
            market_data_cu1707.BidSize = 18;

            m_marketDataList.Add(market_data_cu1707.Instrument.InstrumentCode, market_data_cu1707);
            return market_data_cu1707;
        }
        public USeMarketData CreatInitMarketData_cu1708()
        {
            USeMarketData market_data_cu1708 = new USeMarketData();
            market_data_cu1708.Instrument = new USeInstrument("cu1708", "沪铜1708", USeMarket.SHFE);
            market_data_cu1708.LastPrice = 45310;
            market_data_cu1708.AskPrice = 45320;
            market_data_cu1708.BidPrice = 45300;


            market_data_cu1708.OpenPrice = 45310;
            market_data_cu1708.HighPrice = 45310;
            market_data_cu1708.LowPrice = 45310;
            market_data_cu1708.ClosePrice = 45310;
            market_data_cu1708.PreClosePrice = 45310;
            market_data_cu1708.PreSettlementPrice = 45300;
            market_data_cu1708.AskSize = 12;
            market_data_cu1708.BidSize = 13;

            m_marketDataList.Add(market_data_cu1708.Instrument.InstrumentCode, market_data_cu1708);

            return market_data_cu1708;
        }
        public USeMarketData CreatInitMarketData_cu1709()
        {
            USeMarketData market_data_cu1709 = new USeMarketData();
            market_data_cu1709.Instrument = new USeInstrument("cu1709", "沪铜1709", USeMarket.SHFE);

            market_data_cu1709.LastPrice = 45350;
            market_data_cu1709.AskPrice = 45360;
            market_data_cu1709.BidPrice = 45340;

            market_data_cu1709.OpenPrice = 45350;
            market_data_cu1709.HighPrice = 45350;
            market_data_cu1709.LowPrice = 45350;
            market_data_cu1709.ClosePrice = 45350;
            market_data_cu1709.PreClosePrice = 45350;
            market_data_cu1709.PreSettlementPrice = 45320;
            market_data_cu1709.AskSize = 11;
            market_data_cu1709.BidSize = 9;
            m_marketDataList.Add(market_data_cu1709.Instrument.InstrumentCode, market_data_cu1709);
            return market_data_cu1709;
        }

        /// <summary>
        /// 退订行情
        /// </summary>
        /// <param name="products">退订产品列表</param>
        public override void Unsubscribe(List<USeInstrument> products)
        {
            //try
            //{
            //    string[] instruments = new string[products.Count];
            //    for (int i = 0; i < products.Count; i++)
            //    {
            //        instruments[i] = products[i].InstrumentCode;
            //        Debug.WriteLine(string.Format("Unsubscribe [{0}].", products[i].InstrumentCode));
            //    }
            //    m_ctpFeed.UnsubscribeMarketData(instruments);
            //}
            //catch (Exception ex)
            //{
            //    m_logger.WriteError(string.Format("{0} unsubscribeMarketData failed,Error: {1}.", ToString(), ex.Message));
            //    throw ex;
            //}
        }

        /// <summary>
        /// 退订行情
        /// </summary>
        /// <param name="instrument">退订产品</param>
        public override void Unsubscribe(USeInstrument instrument)
        {
            //try
            //{
            //    string[] instruments = new string[] { instrument.InstrumentCode };
            //    Debug.WriteLine(string.Format("Unsubscribe [{0}].", instrument.InstrumentCode));
            //    m_ctpFeed.UnsubscribeMarketData(instruments);
            //}
            //catch (Exception ex)
            //{
            //    m_logger.WriteError(string.Format("{0} unsubscribeMarketData failed,Error: {1}.", ToString(), ex.Message));
            //    throw ex;
            //}
        }

        public void changeMarketData(USeMarketData market_data)
        {
            System.Diagnostics.Debug.Assert(market_data != null);

            foreach (USeMarketData data in m_marketDataList.Values)
            {
                if (data.Instrument.InstrumentCode != market_data.Instrument.InstrumentCode) continue;
                if (market_data.LastPrice > data.HighPrice) market_data.HighPrice = market_data.LastPrice;
                if (market_data.LastPrice < data.LowPrice) market_data.LowPrice = market_data.LastPrice;
                market_data.ClosePrice = market_data.LastPrice;
            }

            lock (m_object)
            {
                m_marketDataList[market_data.Instrument.InstrumentCode] = market_data;
            }
            this.FireOnMarketDataChanged(market_data);
        }

    }
}
