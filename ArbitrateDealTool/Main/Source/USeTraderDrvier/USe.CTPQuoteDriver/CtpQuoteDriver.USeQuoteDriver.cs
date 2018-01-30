#region Copyright & Version
//==============================================================================
// 文件名称: CtpQuoteDriver.USeQuoteDriver.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Justin Shen
// 创建日期: 2012/05/10
// 描    述: CTP行情驱动类--实现USeQuoteDriver接口。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;

using USe.TradeDriver.Common;
using CTPAPI;

namespace USe.TradeDriver.Ctp
{
    public partial class CtpQuoteDriver : USeQuoteDriver
    {
        /// <summary>
        /// 连接行情服务器
        /// </summary>
        public override void ConnectServer()
        {
            if (m_eventDic.ContainsKey(CONNECT_EVENT_KEY))
            {
                throw new Exception(string.Format("{0} is connecting.", ToString()));
            }

            FireDriverStateChanged(USeQuoteDriverState.Connecting, "");

            Debug.Assert(string.IsNullOrEmpty(m_ctpFeedStreamFilePath) == false);
            m_ctpFeed = new CtpFeed(m_ctpFeedStreamFilePath);
            m_ctpFeed.SetApplication(this);
            Debug.Assert(m_ctpFeed != null, "m_ctpFeed is null");

            try
            {
                USeResetEvent connectEvent = new USeResetEvent(0);
                m_eventDic.Add(CONNECT_EVENT_KEY, connectEvent);

                m_ctpFeed.ConnectAsync(string.Format(@"tcp://{0}:{1}", m_address, m_port));

                if (connectEvent.WaitOne(m_connectTimeOut) == false)
                {
                    throw new Exception("connect time out.");
                }

                FireDriverStateChanged(USeQuoteDriverState.Connected, "");
            }
            catch (Exception ex)
            {
                FireDriverStateChanged(USeQuoteDriverState.DisConnected, ex.Message);
                m_logger.WriteError(string.Format("{0} connect failed,Error: {1}.", ToString(), ex.Message));
                throw ex;
            }
            finally
            {
                m_eventDic.Remove(CONNECT_EVENT_KEY);
            }
        }

        /// <summary>
        /// 断开行情服务器
        /// </summary>
        public override void DisConnectServer()
        {
            try
            {
                if (m_ctpFeed != null && m_ctpFeed.IsLogin)
                {
                    m_ctpFeed.LogoutAsync(-1);
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0} disconnect failed,Error: {1}.", ToString(), ex.Message));
                throw ex;
            }
            finally
            {
                FireDriverStateChanged(USeQuoteDriverState.DisConnected, "");
            }
        }
        
        /// <summary>
        /// 登录行情服务器
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        public override void Login(string brokerId,string account, string password)
        {
            FireDriverStateChanged(USeQuoteDriverState.LoggingOn, "");

            try
            {
                USeResetEvent loginEvent = new USeResetEvent();
                m_eventDic.Add(LOGIN_EVENT_KEY, loginEvent);

                m_ctpFeed.LoginAsync(brokerId, account, password,"", -1);
                if (loginEvent.WaitOne(m_connectTimeOut) == false)
                {
                    throw new Exception("login time out.");
                }
                else
                {
                    if (loginEvent.IsError)
                    {
                        Debug.Assert(loginEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)loginEvent.Tag;
                        Debug.Assert(rspInfo.ErrorID != 0);
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }
                    else
                    {
                        FireDriverStateChanged(USeQuoteDriverState.LoggedOn, "");
                    }
                }
            }
            catch (Exception ex)
            {
                FireDriverStateChanged(USeQuoteDriverState.DisConnected, ex.Message);
                m_logger.WriteError(string.Format("{0} login failed,Error: {1}.", ToString(), ex.Message));
                throw ex;
            }
            finally
            {
                m_eventDic.Remove(LOGIN_EVENT_KEY);
            }
            m_brokerID = brokerId;
            m_investorID = account;
            m_password = password;

            FireDriverStateChanged(USeQuoteDriverState.Ready, "");
        }

        /// <summary>
        /// 登出行情服务器
        /// </summary>
        public override void Logout()
        {
            try
            {
                if (m_ctpFeed != null)
                {
                    m_ctpFeed.LogoutAsync(-1);
                    m_ctpFeed.Dispose();
                }
            }
            catch
            {
            }
            FireDriverStateChanged(USeQuoteDriverState.DisConnected, "");                                   
        }

        /// <summary>
        /// 查询产品行情
        /// </summary>
        /// <param name="product">被查询产品</param>
        /// <returns>被查询产品行情信息</returns>
        public override USeMarketData Query(USeInstrument product)
        {
            try
            {
                if (m_marketDataDic.ContainsKey(product.InstrumentCode))
                {
                    return m_marketDataDic[product.InstrumentCode];
                }
                else
                {
                    USeResetEvent queryEvent = new USeResetEvent();
                    lock (m_object)
                    {
                        if (!m_eventDic.ContainsKey(product.InstrumentCode))
                        {
                            m_eventDic.Add(product.InstrumentCode, queryEvent);
                        }
                    }

                    m_ctpFeed.SubscribeMarketData(new string[] { product.InstrumentCode });

                    while (true)
                    {                    
                        if (queryEvent.WaitOne(m_queryTimeOut) == false)
                        {
                            USeMarketData nullMarketData = new USeMarketData();
                            nullMarketData.Instrument = product.Clone();
                            return nullMarketData;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (m_marketDataDic.ContainsKey(product.InstrumentCode))
                    {
                        return m_marketDataDic[product.InstrumentCode];
                    }
                    else
                    {
                        USeMarketData nullMarketData = new USeMarketData();
                        nullMarketData.Instrument = product.Clone();
                        return nullMarketData;
                    }
                }
            }
            catch(Exception ex)
            {
                m_logger.WriteError(string.Format("{0} Query failed,Error: {1}.", ToString(), ex.Message));

                USeMarketData nullMarketData = new USeMarketData();
                nullMarketData.Instrument = product.Clone();
                return nullMarketData;
            }
            finally
            {
                lock (m_object)
                {
                    m_eventDic.Remove(product.InstrumentCode);
                }
            }
        }

        /// <summary>
        /// 查询产品行情
        /// </summary>
        /// <param name="product">被查询产品</param>
        /// <returns>被查询产品行情信息</returns>
        public override USeMarketData QuickQuery(USeInstrument product)
        {
            try
            {
                USeMarketData marketData = null;
                lock (m_object)
                {
                    m_marketDataDic.TryGetValue(product.InstrumentCode, out marketData);
                }
                if (marketData != null)
                {
                    return marketData.Clone();
                }
                else
                {
                    USeMarketData nullMarketData = new USeMarketData();
                    nullMarketData.Instrument = product.Clone();
                    return nullMarketData;
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0} QuickQuery failed,Error: {1}.", ToString(), ex.Message));
                USeMarketData nullMarketData = new USeMarketData();
                nullMarketData.Instrument = product.Clone();
                return nullMarketData;
            }
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
                        Debug.WriteLine(string.Format("Subscribe [{0}].", products[i].InstrumentCode));
                    }
                }
                m_ctpFeed.SubscribeMarketData(instruments);
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0} subscribeMarketData failed,Error: {1}.", ToString(), ex.Message));
                throw ex;
            }  
        }

        /// <summary>
        /// 订阅行情
        /// </summary>
        /// <param name="products">订阅产品</param>
        public override void Subscribe(USeInstrument instrument)
        {
            try
            {
                lock (m_object)
                {
                    m_instrumentDic[instrument.InstrumentCode] = instrument.Clone();
                }

                string[] instruments = new string[] { instrument.InstrumentCode};
                Debug.WriteLine(string.Format("Subscribe [{0}].", instrument.InstrumentCode));
                m_ctpFeed.SubscribeMarketData(instruments);
            }   
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0} subscribeMarketData failed,Error: {1}.", ToString(), ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// 退订行情
        /// </summary>
        /// <param name="products">退订产品列表</param>
        public override void Unsubscribe(List<USeInstrument> products)
        {
            try
            {
                string[] instruments = new string[products.Count];
                for (int i = 0; i < products.Count; i++)
                {
                    instruments[i] = products[i].InstrumentCode;
                    Debug.WriteLine(string.Format("Unsubscribe [{0}].", products[i].InstrumentCode));
                }
                m_ctpFeed.UnsubscribeMarketData(instruments);
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0} unsubscribeMarketData failed,Error: {1}.", ToString(), ex.Message));
                throw ex;
            }      
        }

        /// <summary>
        /// 退订行情
        /// </summary>
        /// <param name="instrument">退订产品</param>
        public override void Unsubscribe(USeInstrument instrument)
        {
            try
            {
                string[] instruments = new string[] { instrument.InstrumentCode};
                Debug.WriteLine(string.Format("Unsubscribe [{0}].", instrument.InstrumentCode));
                m_ctpFeed.UnsubscribeMarketData(instruments);
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0} unsubscribeMarketData failed,Error: {1}.", ToString(), ex.Message));
                throw ex;
            }      
        }
    }
}
