#region Copyright & Version
//==============================================================================
// 文件名称: CtpOrderDriver.CtpQuery.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: CTP交易驱动类 -- Ctp同步查询方法。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;

using USe.TradeDriver.Common;
using CTPAPI;

namespace USe.TradeDriver.Ctp
{
    public partial class CtpOrderDriver
    {
        /// <summary>
        /// 查询所有合约代码。
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        private List<InstrumentField> QueryAllInstrumentFromCtp()
        {
            List<InstrumentField> instrumentFields = new List<InstrumentField>();
            int requestID = m_requetSeqIDCreator.Next();
            try
            {
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = instrumentFields;
                m_eventDic.Add(requestID, queryEvent);

                QryInstrumentField qryField = new QryInstrumentField();
                m_ctpUser.ReqQryInstrument(ref qryField, requestID);

                while (true)
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }

                    if (queryEvent.IsFinish) break;

                    if (queryEvent.WaitOne(m_queryTimeOut) == false)
                    {
                        throw new Exception(string.Format("({0}){1}", "99", "time out."));
                    }
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0}.QueryAllInstrumentFromCtp() failed,Error:{1}.",
                                    ToString(), ex.Message));
                throw new Exception(string.Format("Query instrument failed,Error:{0}.", ex.Message));
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }

            return instrumentFields;
        }

        /// <summary>
        /// 查询所有委托单明细。
        /// </summary>
        ///<param name="error">查询结果信息。</param>
        /// <returns>查询委托单明细是否成功。</returns>
        private List<OrderField> QueryAllOrderFieldFromCtp()
        {
            List<OrderField> orderBooks = new List<OrderField>();
            int requestID = m_requetSeqIDCreator.Next();

            try
            {
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = orderBooks;
                m_eventDic.Add(queryEvent.EventID, queryEvent);

                QryOrderField requestField = new QryOrderField();
                requestField.BrokerID = m_brokerID;
                requestField.InvestorID = m_investorID;

                m_ctpUser.ReqQryOrder(ref requestField, requestID);

                while (true)
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }

                    if (queryEvent.IsFinish) break;

                    if (queryEvent.WaitOne(m_queryTimeOut) == false)
                    {
                        throw new Exception(string.Format("({0}){1}", "99", "time out"));
                    }
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0}.QueryAllOrderFieldFromCtp() failed,Error:{1}.",
                              ToString(), ex.Message));
                throw new Exception(string.Format("Query order book failed,Error:{0}.", ex.Message));
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }

            return orderBooks;
        }

        /// <summary>
        /// 查询成交回报明细。
        /// </summary>
        ///<param name="error">查询结果信息。</param>
        /// <returns>查询成交回报是否成功。</returns>
        private List<TradeField> QueryAllTradeFieldFromCtp()
        {
            List<TradeField> tradeBooks = new List<TradeField>();
            int requestID = m_requetSeqIDCreator.Next();
            try
            {
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = tradeBooks;
                m_eventDic.Add(requestID, queryEvent);

                QryTradeField requestField = new QryTradeField();
                requestField.BrokerID = m_brokerID;
                requestField.InvestorID = m_investorID;
                m_ctpUser.ReqQryTrade(ref requestField, requestID);

                while (true)
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }

                    if (queryEvent.IsFinish) break;

                    if (queryEvent.WaitOne(m_queryTimeOut) == false)
                    {
                        throw new Exception(string.Format("({0}){1}", "99", "time out"));
                    }
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0}.QueryAllTradeFieldFromCtp() failed,Error:{1}.",
                             ToString(), ex.Message));
                throw new Exception(string.Format("Query trade book failed,Error:{0}.", ex.Message));
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }

            return tradeBooks;
        }

        /// <summary>
        /// 查询账户持仓信息。
        /// </summary>
        ///<param name="error">查询结果信息。</param>
        /// <returns>查询持仓信息是否成功。</returns>
        private List<InvestorPositionField> QueryInvestorPositionFromCtp()
        {
            List<InvestorPositionField> positionsFields = new List<InvestorPositionField>();
            int requestID = m_requetSeqIDCreator.Next();
            try
            {
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = positionsFields;
                m_eventDic.Add(queryEvent.EventID, queryEvent);

                QryInvestorPositionField requestField = new QryInvestorPositionField();
                requestField.BrokerID = m_brokerID;
                requestField.InvestorID = m_investorID;
                requestField.InstrumentID = null;

                m_ctpUser.ReqQryInvestorPosition(ref requestField, requestID);

                while (true)
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }

                    if (queryEvent.IsFinish) break;

                    if (queryEvent.WaitOne(m_queryTimeOut) == false)
                    {
                        throw new Exception(string.Format("({0}){1}", "99", "time out"));
                    }
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0}.QueryInvestorPositionFromCtp() failed,Error:{1}.",
                             ToString(), ex.Message));
                throw new Exception(string.Format("Query position failed,Error:{0}.", ex.Message));
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }

            return positionsFields;
        }

        /// <summary>
        /// 查询账户资金。
        /// </summary>
        ///<param name="error">查询结果信息。</param>
        /// <returns>查询账户资金是否成功。</returns>
        private TradingAccountField QueryTradingAccountFromCtp()
        {
            int requestID = m_requetSeqIDCreator.Next();
            List<TradingAccountField> tradingAccounts = new List<TradingAccountField>();
            try
            {
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = tradingAccounts;
                m_eventDic.Add(queryEvent.EventID, queryEvent);

                QryTradingAccountField requestField = new QryTradingAccountField();
                requestField.BrokerID = m_brokerID;
                requestField.InvestorID = m_investorID;

                m_ctpUser.ReqQryTradingAccount(ref requestField, requestID);

                while (true)
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }

                    if (queryEvent.IsFinish) break;

                    if (queryEvent.WaitOne(m_queryTimeOut) == false)
                    {
                        throw new Exception(string.Format("({0}){1}", "99", "time out"));
                    }
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0}.QueryTradingAccountFromCtp() failed,Error:{1}.",
                             ToString(), ex.Message));
                throw new Exception(string.Format("Query trading account failed,Error:{0}.", ex.Message));
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }

            if (tradingAccounts.Count > 0)
            {
                Debug.Assert(tradingAccounts.Count == 1);
                return tradingAccounts[0];
            }
            else
            {
                throw new Exception(string.Format("Query trading account failed,Error:{0}.", "no record"));
            }
        }

        /// <summary>
        /// 查询账户基本信息
        /// </summary>
        ///<param name="error">查询结果信息。</param>
        /// <returns>查询账户基本信息。</returns>
        private InvestorField QueryInvestorBaseInfoFromCtp()
        {
            int requestID = m_requetSeqIDCreator.Next();
            List<InvestorField> investorInfos = new List<InvestorField>();
            try
            {
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = investorInfos;
                m_eventDic.Add(queryEvent.EventID, queryEvent);

                QryInvestorField requestField = new QryInvestorField();
                requestField.BrokerID = m_brokerID;
                requestField.InvestorID = m_investorID;

                m_ctpUser.ReqQryInvestor(ref requestField, requestID);

                while (true)
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }

                    if (queryEvent.IsFinish) break;

                    if (queryEvent.WaitOne(m_queryTimeOut) == false)
                    {
                        throw new Exception(string.Format("({0}){1}", "99", "time out"));
                    }
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0}.QueryInvestorBaseInfoFromCtp() failed,Error:{1}.",
                             ToString(), ex.Message));
                throw new Exception(string.Format("Query investor base info failed,Error:{0}.", ex.Message));
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }

            if (investorInfos.Count > 0)
            {
                Debug.Assert(investorInfos.Count == 1);
                return investorInfos[0];
            }
            else
            {
                throw new Exception(string.Format("Query investor base info failed,Error:{0}.", "no record"));
            }
        }

        /// <summary>
        /// 从Ctp查询手续费。
        /// </summary>
        /// <param name="instrumentCode">合约代码。</param>
        /// <returns></returns>
        private InstrumentCommissionRateField QueryCommissionRateFieldFromCtp(string instrumentCode)
        {
            List<InstrumentCommissionRateField> commissionFields = new List<InstrumentCommissionRateField>();
            int requestID = m_requetSeqIDCreator.Next();

            try
            {
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = commissionFields;
                m_eventDic.Add(queryEvent.EventID, queryEvent);

                QryInstrumentCommissionRateField requestFields = new QryInstrumentCommissionRateField();
                requestFields.BrokerID = m_brokerID;
                requestFields.InvestorID = m_investorID;
                requestFields.InstrumentID = instrumentCode;

                m_ctpUser.ReqQryInstrumentCommissionRate(ref requestFields, requestID);

                while (true)
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }

                    if (queryEvent.IsFinish) break;

                    if (queryEvent.WaitOne(m_queryTimeOut) == false)
                    {
                        throw new Exception(string.Format("({0}){1}", "99", "time out"));
                    }
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0}.QueryCommissionRateFieldFromCtp() failed,Error:{1}.",
                            ToString(), ex.Message));
                throw new Exception(string.Format("Query [{0}] commission rate failed,Error:{1}.",instrumentCode, ex.Message));
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }

            if (commissionFields.Count > 0)
            {
                Debug.Assert(commissionFields.Count == 1);
                return commissionFields[0];
            }
            else
            {
                //查询有应答但无值,构造一个空的手续费
                USeInstrumentDetail instrumentDetail = m_dataBuffer.GetInstrumentDetail(instrumentCode);
                Debug.Assert(instrumentDetail != null);
                InstrumentCommissionRateField rateField = new InstrumentCommissionRateField();
                rateField.BrokerID = m_brokerID;
                rateField.CloseRatioByMoney = 0d;
                rateField.CloseRatioByVolume = 0d;
                rateField.CloseTodayRatioByMoney = 0d;
                rateField.CloseTodayRatioByVolume = 0d;
                rateField.InstrumentID = instrumentDetail.Varieties;
                rateField.InvestorID = m_investorID;
                rateField.InvestorRange = InvestorRangeType.All;
                rateField.OpenRatioByMoney = 0d;
                rateField.OpenRatioByVolume = 0d;

                return rateField;
            }
        }

        /// <summary>
        /// 从Ctp查询保证金。
        /// </summary>
        /// <param name="instrumentCode"></param>
        /// <returns></returns>
        private InstrumentMarginRateField QueryMarginFromCtp(string instrumentCode)
        {
            List<InstrumentMarginRateField> marginFields = new List<InstrumentMarginRateField>();
            int requestID = m_requetSeqIDCreator.Next();

            try
            {

                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = marginFields;
                m_eventDic.Add(queryEvent.EventID, queryEvent);

                QryInstrumentMarginRateField requestField = new QryInstrumentMarginRateField();
                requestField.BrokerID = m_brokerID;
                requestField.InvestorID = m_investorID;
                requestField.InstrumentID = instrumentCode;
                requestField.HedgeFlag = HedgeFlagType.Speculation;

                m_ctpUser.ReqQryInstrumentMarginRate(ref requestField, requestID);

                while (true)
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }

                    if (queryEvent.IsFinish) break;

                    if (queryEvent.WaitOne(m_queryTimeOut) == false)
                    {
                        throw new Exception(string.Format("({0}){1}", "99", "time oute"));
                    }
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0}.QueryMarginFromCtp() failed,Error:{1}.",
                            ToString(), ex.Message));
                throw new Exception(string.Format("Query [{0}]margin failed,Error:{1}.",instrumentCode, ex.Message));
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }

            if (marginFields.Count > 0)
            {
                Debug.Assert(marginFields.Count == 1);
                return marginFields[0];
            }
            else
            {
                //查询有应答但无值,构造一个空的保证金
                InstrumentMarginRateField marginField = new InstrumentMarginRateField();
                marginField.BrokerID = m_brokerID;
                marginField.HedgeFlag = HedgeFlagType.Speculation;
                marginField.InstrumentID = instrumentCode;
                marginField.InvestorID = m_investorID;
                marginField.InvestorRange = InvestorRangeType.All;
                marginField.IsRelative = IntBoolType.Yes;
                marginField.LongMarginRatioByMoney = 0d;
                marginField.LongMarginRatioByVolume = 0d;
                marginField.ShortMarginRatioByMoney = 0d;
                marginField.ShortMarginRatioByVolume = 0d;

                return marginField;
            }
        }

        /// <summary>
        /// 从Ctp查询是否需要投资者结算信息确认。
        /// </summary>
        /// <returns></returns>
        private bool QueryNeedSettlementInfoConfirmFromCtp()
        {
            List<SettlementInfoConfirmField> replyFields = new List<SettlementInfoConfirmField>();
            int requestID = m_requetSeqIDCreator.Next();

            try
            {
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = replyFields;
                m_eventDic.Add(queryEvent.EventID, queryEvent);

                QrySettlementInfoConfirmField requestFields = new QrySettlementInfoConfirmField();
                requestFields.BrokerID = m_brokerID;
                requestFields.InvestorID = m_investorID;

                m_ctpUser.ReqQrySettlementInfoConfirm(ref requestFields, requestID);

                while (true)
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }

                    if (queryEvent.IsFinish) break;

                    if (queryEvent.WaitOne(m_queryTimeOut) == false)
                    {
                        throw new Exception(string.Format("({0}){1}", "99", "time out"));
                    }
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0}.QueryNeedSettlementInfoConfirmFromCtp() failed,Error:{1}.",
                            ToString(), ex.Message));
                throw new Exception(string.Format("Query settlementInfo confirm failed,Error:{0}.", ex.Message));
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }

            if (replyFields.Count <=0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 从Ctp查询是否需要投资者结算信息确认。
        /// </summary>
        /// <returns></returns>
        private List<SettlementInfoField> QuerySettlementInfoFromCtp(string tradingDate)
        {
            List<SettlementInfoField> replyFields = new List<SettlementInfoField>();
            int requestID = m_requetSeqIDCreator.Next();

            try
            {
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = replyFields;
                m_eventDic.Add(queryEvent.EventID, queryEvent);

                QrySettlementInfoField requestFields = new QrySettlementInfoField();
                requestFields.BrokerID = m_brokerID;
                requestFields.InvestorID = m_investorID;
                requestFields.TradingDay = tradingDate;

                m_ctpUser.ReqQrySettlementInfo(ref requestFields, requestID);

                while (true)
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }

                    if (queryEvent.IsFinish) break;

                    if (queryEvent.WaitOne(m_queryTimeOut) == false)
                    {
                        throw new Exception(string.Format("({0}){1}", "99", "time out"));
                    }
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0}.QuerySettlementInfoFromCtp() failed,Error:{1}.",
                            ToString(), ex.Message));
                throw new Exception(string.Format("Query settlementInfo failed,Error:{0}.", ex.Message));
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }

            return replyFields;
        }
    }
}
