#region Copyright & Version
//==============================================================================
// 文件名称: CtpQuoteDriver.ICtpFeedApplication.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Justin Shen
// 创建日期: 2012/05/10
// 描    述: CTP行情驱动类--实现ICtpFeedApplication接口。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using CTPAPI;
using USe.TradeDriver.Common;
using USe.Common;

namespace USe.TradeDriver.Ctp
{
    public partial class CtpQuoteDriver : ICtpFeedApplication
    {
        /// <summary>
        /// 与前置机连接成功时的回调方法。
        /// </summary>
        public void OnFrontConnect()
        {
            m_logger.WriteInformation(string.Format("{0}.OnFrontConnect ok.", ToString()));
            USeResetEvent resetEvent = GetResetEvent(CONNECT_EVENT_KEY);
            if (resetEvent != null)
            {
                resetEvent.Set(false);
            }
            else
            {
                if (string.IsNullOrEmpty(m_password) == false)
                {
                    FireDriverStateChanged(USeQuoteDriverState.Connected, "Reconnect");

                    //1秒后重新登录
                    m_autoLoginTimer.Change(1000, Timeout.Infinite);
                }
            }

        }

        /// <summary>
        /// 与前置机连接中断时的回调方法。
        /// </summary>
        /// <param name="reason">连接中断的原因，参见CTP API文档。</param>
        public void OnFrontDisconnect(int reason)
        {
            string reasonDes = string.Empty;
            switch (reason)
            {
                case 0x1001: reasonDes = "网络读失败"; break;
                case 0x1002: reasonDes = "网络写失败"; break;
                case 0x2001: reasonDes = "接收心跳超时"; break;
                case 0x2002: reasonDes = "发送心跳失败"; break;
                case 0x2003: reasonDes = "收到错误报文"; break;
                default: break;
            }

            m_logger.WriteInformation(string.Format("{0}.OnFrontDisconnect ok,[Reason:({1}){2}].", ToString(), reason, reasonDes));

            FireDriverStateChanged(USeQuoteDriverState.DisConnected, string.Format("({0}){1}", reason, reasonDes));
        }

        /// <summary>
        /// 与前置机连接的心跳超时时的回调方法。
        /// </summary>
        /// <param name="elapsed">距离上次接收报文的间隔时间。</param>
        public void OnFrontHeartbeatWarning(int elapsed)
        {
            
        }

        /// <summary>
        /// 与指定编号的请求对应的报告错误应答。
        /// </summary>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspError(ref RspInfoField resultField, int requestId, bool isLast)
        {
            if (resultField.ErrorID != 0)
            {
                FireSendErrorMessage(string.Format("({0}){1}", resultField.ErrorID, resultField.ErrorMsg));
            }
        }

        /// <summary>
        /// 行情订阅响应
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspSubMarketData(ref SpecificInstrumentField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            if (resultField.ErrorID != 0)
            {
                FireSendErrorMessage(string.Format("({0}){1}", resultField.ErrorID, resultField.ErrorMsg));
            }
        }

        /// <summary>
        /// 行情退订响应
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspUnsubMarketData(ref SpecificInstrumentField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            if (resultField.ErrorID != 0)
            {
                FireSendErrorMessage(string.Format("({0}){1}", resultField.ErrorID, resultField.ErrorMsg));
            }
        }

        /// <summary>
        /// 登录响应
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspUserLogin(ref RspUserLoginField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            m_logger.WriteInformation(string.Format("{0}.OnRspUserLogin ok.", ToString()));
            USeResetEvent resetEvent = GetResetEvent(LOGIN_EVENT_KEY);
            if (resetEvent == null) return;

            if (resultField.ErrorID != 0 || replyField.HasValue == false)
            {
                resetEvent.Tag = resultField;
                resetEvent.Set(true);
            }
            else
            {
                resetEvent.Set(false);
            }
        }

        /// <summary>
        /// 登出响应
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspUserLogout(ref UserLogoutField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            Debug.Assert(false);
        }

        /// <summary>
        /// 行情变更通知    
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnDepthMarketData(ref DepthMarketDataField? reportField)
        {
            try
            {
                if (reportField.HasValue == false)
                {
                    return;
                }

                string instrumentCode = reportField.Value.InstrumentID;
                USeMarketData marketData = null;

                lock (m_object)
                {
                    marketData = DepthMarketDataFieldToUSeFuture(reportField.Value);
                    m_marketDataDic[instrumentCode] = marketData;
                }

                USeResetEvent resetEvent = GetResetEvent(instrumentCode);
                if (resetEvent != null)
                {
                    resetEvent.Set(false);
                }

                FireOnMarketDataChanged(marketData);
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0}.OnRtnDepthMarketData failed ,Error:{1}.",
                                    ToString(),ex.Message));
                Debug.Assert(false, ex.Message);
            }
        }

        private USeMarketData DepthMarketDataFieldToUSeFuture(DepthMarketDataField field)
        {
            USeMarketData marketData = new USeMarketData();
            try
            {
                marketData.AskPrice = field.AskPrice1.ToDecimal();
                marketData.AskSize = field.AskVolume1;
                marketData.BidPrice = field.BidPrice1.ToDecimal();
                marketData.BidSize = field.BidVolume1;
                marketData.ClosePrice = field.ClosePrice.ToDecimal();
                marketData.AvgPrice = field.AveragePrice;

                if (m_instrumentDic.ContainsKey(field.InstrumentID))
                {
                    marketData.Instrument = m_instrumentDic[field.InstrumentID].Clone();
                }
                else
                {
                    marketData.Instrument = new USeInstrument(field.InstrumentID, field.InstrumentID, USeMarket.Unknown);
                }
                marketData.LastPrice = field.LastPrice.ToDecimal();
                marketData.LowerLimitPrice = field.LowerLimitPrice.ToDecimal();
                marketData.OpenPrice = field.OpenPrice.ToDecimal();
                marketData.PreClosePrice = field.PreClosePrice.ToDecimal();
                marketData.PreSettlementPrice = field.PreSettlementPrice.ToDecimal();
                marketData.SettlementPrice = field.SettlementPrice.ToDecimal();
                marketData.Turnover = field.Turnover.ToDecimal();

                DateTime updateTime = DateTime.Now;
                if (string.IsNullOrWhiteSpace(field.TradingDay) && string.IsNullOrWhiteSpace(field.UpdateTime))
                {
                    Debug.Assert(false);
                    updateTime = DateTime.Now;
                }
                else if (string.IsNullOrWhiteSpace(field.TradingDay))
                {
                    if (DateTime.TryParseExact(DateTime.Today.ToString("yyyyMMdd") + field.UpdateTime.Trim(), "yyyyMMddHH:mm:ss", null, System.Globalization.DateTimeStyles.None, out updateTime) == false)
                    {
                        Debug.Assert(false);
                        updateTime = DateTime.Now;
                    }
                    else
                    {
                        marketData.QuoteTime = updateTime.TimeOfDay;
                    }
                }
                else
                {
                    if (DateTime.TryParseExact(field.TradingDay.Trim() + field.UpdateTime.Trim(), "yyyyMMddHH:mm:ss", null, System.Globalization.DateTimeStyles.None, out updateTime) == false)
                    {
                        Debug.Assert(false);
                        updateTime = DateTime.Now;
                    }
                    else
                    {
                        marketData.QuoteDay = updateTime.Date;
                        marketData.QuoteTime = updateTime.TimeOfDay;
                    }
                }
                marketData.UpdateTime = updateTime;
                marketData.UpperLimitPrice = field.UpperLimitPrice.ToDecimal();
                marketData.Volume = field.Volume;
                marketData.HighPrice = field.HighestPrice.ToDecimal();
                marketData.LowPrice = field.LowestPrice.ToDecimal();
                marketData.OpenInterest = field.OpenInterest.ToDecimal();
                //marketData.PreOpenInterest = field.PreOpenInterest.ToDecimal();
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0} DepthMarketDataFieldToUSeFuture() convet failed ,Error:{1}.",
                    ToString(), ex.Message));
                Debug.Assert(false, "DepthMarketDataFieldToUSeFuture() convet failed," + ex.Message);
            }
            return marketData;
        }

        private USeResetEvent GetResetEvent(string instrumentCode)
        {
            if (m_eventDic.ContainsKey(instrumentCode))
            {
                return m_eventDic[instrumentCode];
            }
            else
            {
                return null;
            }
        }
    }
}
