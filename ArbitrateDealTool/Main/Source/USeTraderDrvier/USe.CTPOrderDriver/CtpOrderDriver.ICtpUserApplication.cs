#region Copyright & Version
//==============================================================================
// 文件名称: CtpOrderDriver.ICtpUserApplication.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: CTP交易驱动类--实现ICtpUserApplication接口。
//==============================================================================
#endregion
using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Diagnostics;

using CTPAPI;
using USe.TradeDriver.Common;

namespace USe.TradeDriver.Ctp
{
    public partial class CtpOrderDriver : ICtpUserApplication
    {
        private USeResetEvent GetResetEvent(int requestID)
        {
            if (m_eventDic.ContainsKey(requestID))
            {
                return m_eventDic[requestID];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 与前置机连接成功时的回调方法。
        /// </summary>
        public void OnFrontConnect()
        {
            m_logger.WriteInformation(string.Format("{0}.OnFrontConnect ok.", ToString()));

            USeResetEvent resetEvent = GetResetEvent(0);
            if (resetEvent != null) 
            {
                resetEvent.Set(false);
            }
            else
            {
                if (string.IsNullOrEmpty(m_password) == false)
                {
                    FireOrderDriverStateChanged(USeOrderDriverState.Connected, "");
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

            FireOrderDriverStateChanged(USeOrderDriverState.DisConnected, string.Format("({0}){1}", reason, reasonDes));
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
        public void OnRspError(ref RspInfoField? resultField, int requestId, bool isLast)
        {
            Debug.Assert(isLast, "OnRspError() isLast is false");

            m_logger.WriteInformation(string.Format("{0}.OnRspError() ok,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}]",
                ToString(), requestId,
                resultField.HasValue ? "" : resultField.Value.ErrorID.ToString(),
                resultField.HasValue ? "" : resultField.Value.ErrorMsg));

            USeResetEvent resetEvent = GetResetEvent(requestId);
            if (resetEvent == null) return;

            try
            {
                if (resultField.HasValue && resultField.Value.ErrorID != 0)
                {
                    resetEvent.Tag = resultField.Value;
                    resetEvent.Set(true);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspError() exception:" + ex.Message);
                m_logger.WriteError(string.Format("OnRspError(),Error:{0},StackTrace:{1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// 客户端认证响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspAuthenticate(ref RspAuthenticateField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            Debug.Assert(isLast, "OnRspAuthenticate() isLast is false");
            Debug.Assert(requestId > 0);

            USeResetEvent resetEvent = GetResetEvent(requestId);
            if (resetEvent == null) return;

            if (resultField.ErrorID != 0 || replyField.HasValue == false)
            {
                m_logger.WriteInformation(string.Format("{0}.OnRspAuthenticate() failed,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}]",
                        ToString(), requestId, resultField.ErrorID, resultField.ErrorMsg));

                resetEvent.Tag = resultField;
                resetEvent.Set(true);
            }
            else
            {
                m_brokerID = replyField.Value.BrokerID;
                m_userId = replyField.Value.UserID;
                m_userProductInfo = replyField.Value.UserProductInfo;

                m_logger.WriteInformation(string.Format("{0}.OnRspAuthenticate() ok,[RequestID:{1}][BrokerID:{2}][UserID:{3}][UserProductInfo:{4}]",
                       ToString(), requestId, m_brokerID, m_userId, m_userProductInfo));

                resetEvent.Set(false);
            }


        }

        /// <summary>
        /// 登录请求响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspUserLogin(ref RspUserLoginField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            Debug.Assert(isLast, "OnRspUserLogin() isLast is false");
            Debug.Assert(requestId > 0);

            USeResetEvent resetEvent = GetResetEvent(requestId);
            if (resetEvent == null) return;

            if (resultField.ErrorID != 0 || replyField.HasValue == false)
            {
                m_logger.WriteInformation(string.Format("{0}.OnRspUserLogin() failed,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}]",
                        ToString(), requestId, resultField.ErrorID, resultField.ErrorMsg));

                resetEvent.Tag = resultField;
                resetEvent.Set(true);
            }
            else
            {
                m_frontID = replyField.Value.FrontID;
                m_sessionID = replyField.Value.SessionID;

                m_logger.WriteInformation(string.Format("{0}.OnRspUserLogin() ok,[RequestID:{1}][FrontID:{2}][SessionID:{3}]",
                       ToString(), requestId, m_frontID, m_sessionID));

                resetEvent.Set(false);
            }
        }

        /// <summary>
        /// 登出请求响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspUserLogout(ref UserLogoutField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 用户口令更新请求响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspUserPasswordUpdate(ref UserPasswordUpdateField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 资金账户口令更新请求响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspTradingAccountPasswordUpdate(ref TradingAccountPasswordUpdateField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 报单录入请求响应(Thost报单拒绝)。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspOrderInsert(ref InputOrderField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            Debug.Assert(isLast, "OnRspOrderInsert() islast is false");
            Debug.Assert(resultField.ErrorID != 0, "OnRspOrderInsert() resultField.ErrorID less than zeror");
            Debug.Assert(replyField.HasValue, "OnRspOrderInsert() replyField.HasValue is false");

            try
            {
                string orderRef = replyField.Value.OrderRef;
                CtpOrderNum orderNum = new CtpOrderNum(m_frontID, m_sessionID, orderRef);

                m_logger.WriteInformation(string.Format("{0}.OnRspOrderInsert(),[OrderNum:{1}][ErrorID:{2}][ErrorMessage:{3}]",
                            ToString(), orderNum.OrderString, resultField.ErrorID, resultField.ErrorMsg));

                if (resultField.ErrorID != 0)
                {
                    m_dataBuffer.BlankOrder(orderNum, string.Format("({0}){1}", resultField.ErrorID, resultField.ErrorMsg));

                    USeOrderBook orderBook = m_dataBuffer.GetCheckedOrderBook(orderNum);
                    FireOrderBookChanged(orderBook);

                    if (orderBook != null)
                    {
                        USeDirection direction;
                        if (orderBook.OffsetType == USeOffsetType.Open)
                        {
                            direction = orderBook.OrderSide == USeOrderSide.Buy ? USeDirection.Long : USeDirection.Short;
                        }
                        else
                        {
                            direction = orderBook.OrderSide == USeOrderSide.Buy ? USeDirection.Short : USeDirection.Long;
                        }
                        USePosition position = m_dataBuffer.GetPosition(orderBook.Instrument, direction);
                        if (position != null)
                        {
                            FirePositionChanged(position);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspOrderInsert() exception:" + ex.Message);
                m_logger.WriteError(string.Format("{0}.OnRspOrderInsert() failed,Error:{1}.",
                            ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 报单操作请求响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspOrderAction(ref InputOrderActionField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            //撤单失败
            Debug.Assert(isLast, "OnRspOrderAction() islast is false");
            Debug.Assert(resultField.ErrorID != 0, "OnRspOrderAction() resultField.ErrorID less than zeror");
            Debug.Assert(replyField.HasValue, "OnRspOrderAction() replyField.HasValue is false");

            try
            {
                if (resultField.ErrorID != 0 && replyField.HasValue)
                {
                    CtpOrderNum orderNum = new CtpOrderNum(replyField.Value.FrontID,
                                                                 replyField.Value.SessionID,
                                                                 replyField.Value.OrderRef,
                                                                 replyField.Value.ExchangeID,
                                                                 replyField.Value.OrderSysID);

                    m_logger.WriteInformation(string.Format("{0}.OnRspOrderAction(),[OrderNum:{1}][ErrorID:{2}][ErrorMessage:{3}]",
                           ToString(), orderNum.OrderString, resultField.ErrorID, resultField.ErrorMsg));

                    m_dataBuffer.CancelOrderFaild(orderNum, string.Format("({0}){1}", resultField.ErrorID, resultField.ErrorMsg));

                    USeOrderBook orderBook = m_dataBuffer.GetOrderBook(orderNum);
                    FireOrderBookChanged(orderBook);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspOrderAction() exception:" + ex.Message);
                m_logger.WriteError(string.Format("{0}.OnRspOrderAction() failed,Error:{1}.",
                            ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 预埋单录入请求响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspParkedOrderInsert(ref ParkedOrderField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 预埋撤单录入请求响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspParkedOrderAction(ref ParkedOrderActionField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 删除预埋单响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspRemoveParkedOrder(ref RemoveParkedOrderField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 删除预埋撤单响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspRemoveParkedOrderAction(ref RemoveParkedOrderActionField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 查询最大报单数量响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQueryMaxOrderVolume(ref QueryMaxOrderVolumeField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 投资者结算结果确认响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspSettlementInfoConfirm(ref SettlementInfoConfirmField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询报单响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryOrder(ref OrderField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            try
            {
                USeResetEvent resetEvent = GetResetEvent(requestId);
                if (resetEvent == null) return;

                if (resultField.ErrorID != 0)
                {
                    m_logger.WriteError(string.Format("{0}.OnRspQryOrder() failed,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}.",
                                    ToString(), requestId, resultField.ErrorID, resultField.ErrorMsg));
                    resetEvent.Tag = resultField;
                    resetEvent.Set(true);
                }
                else
                {
                    if (replyField.HasValue)
                    {
                        List<OrderField> orderFields = resetEvent.Tag as List<OrderField>;
                        Debug.Assert(orderFields != null, "OnRspQryOrder() resetEvent.Tag is null");
                        orderFields.Add(replyField.Value);
                    }

                    resetEvent.IsFinish = isLast;
                    resetEvent.Set(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspQryOrder() exception:" + ex.Message);
                m_logger.WriteError(string.Format("OnRspQryOrder(),Error:{0},StackTrace:{1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// 请求查询成交响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryTrade(ref TradeField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            try
            {
                USeResetEvent resetEvent = GetResetEvent(requestId);
                if (resetEvent == null) return;

                if (resultField.ErrorID != 0)
                {
                    m_logger.WriteError(string.Format("{0}.OnRspQryTrade() failed,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}.",
                                   ToString(), requestId, resultField.ErrorID, resultField.ErrorMsg));
                    resetEvent.Tag = resultField;
                    resetEvent.Set(true);
                }
                else
                {
                    if (replyField.HasValue)
                    {
                        List<TradeField> tradeFields = resetEvent.Tag as List<TradeField>;
                        Debug.Assert(tradeFields != null, "OnRspQryTrade() resetEvent.Tag is null");
                        tradeFields.Add(replyField.Value);
                    }

                    resetEvent.IsFinish = isLast;
                    resetEvent.Set(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspQryTrade() exception:" + ex.Message);
                m_logger.WriteError(string.Format("OnRspQryTrade(),Error:{0},StackTrace:{1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// 请求查询投资者持仓响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryInvestorPosition(ref InvestorPositionField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            try
            {
                USeResetEvent resetEvent = GetResetEvent(requestId);
                if (resetEvent == null) return;

                if (resultField.ErrorID != 0)
                {
                    m_logger.WriteInformation(string.Format("{0}.OnRspQryInvestorPosition() failed,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}.",
                                  ToString(), requestId, resultField.ErrorID, resultField.ErrorMsg));
                    resetEvent.Tag = resultField;
                    resetEvent.Set(true);
                }
                else
                {
                    if (replyField.HasValue)
                    {
                        List<InvestorPositionField> positionFields = resetEvent.Tag as List<InvestorPositionField>;
                        Debug.Assert(positionFields != null, "OnRspQryInvestorPosition() resetEvent.Tag is null");
                        positionFields.Add(replyField.Value);
                    }

                    resetEvent.IsFinish = isLast;
                    resetEvent.Set(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspQryInvestorPosition() exception:" + ex.Message);
                m_logger.WriteError(string.Format("{0}.OnRspQryInvestorPosition(),Error:{1}.",
                     ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 请求查询资金账户响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryTradingAccount(ref TradingAccountField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            Debug.Assert(isLast, "OnRspQryTradingAccount() isLast is false");
            USeResetEvent resetEvent = GetResetEvent(requestId);
            if (resetEvent == null) return;

            try
            {
                if (resultField.ErrorID != 0)
                {
                    m_logger.WriteInformation(string.Format("{0}.OnRspQryTradingAccount() failed,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}.",
                                 ToString(), requestId, resultField.ErrorID, resultField.ErrorMsg));
                    resetEvent.Tag = resultField;
                    resetEvent.Set(true);
                }
                else
                {
                    if (replyField.HasValue)
                    {
                        List<TradingAccountField> tradingAccountFields = resetEvent.Tag as List<TradingAccountField>;
                        Debug.Assert(tradingAccountFields != null, "OnRspQryTradingAccount() resetEvent.Tag is null");
                        tradingAccountFields.Add(replyField.Value);
                    }

                    resetEvent.IsFinish = true;
                    resetEvent.Set(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspQryTradingAccount() exception:" + ex.Message);
                m_logger.WriteError(string.Format("{0}.OnRspQryTradingAccount(),Error:{1}.",
                    ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 请求查询投资者响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryInvestor(ref InvestorField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            Debug.Assert(isLast, "OnRspQryInvestor() isLast is false");
            USeResetEvent resetEvent = GetResetEvent(requestId);
            if (resetEvent == null) return;

            try
            {
                if (resultField.ErrorID != 0)
                {
                    m_logger.WriteInformation(string.Format("{0}.OnRspQryInvestor() failed,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}.",
                                 ToString(), requestId, resultField.ErrorID, resultField.ErrorMsg));
                    resetEvent.Tag = resultField;
                    resetEvent.Set(true);
                }
                else
                {
                    if (replyField.HasValue)
                    {
                        List<InvestorField> investorFields = resetEvent.Tag as List<InvestorField>;
                        Debug.Assert(investorFields != null, "OnRspQryInvestor() resetEvent.Tag is null");
                        investorFields.Add(replyField.Value);
                    }

                    resetEvent.IsFinish = true;
                    resetEvent.Set(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspQryInvestor() exception:" + ex.Message);
                m_logger.WriteError(string.Format("{0}.OnRspQryInvestor(),Error:{1}.",
                    ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 请求查询交易编码响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryTradingCode(ref TradingCodeField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询合约保证金率响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryInstrumentMarginRate(ref InstrumentMarginRateField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            Debug.Assert(isLast, "OnRspQryInstrumentMarginRate() isLast is false");
            USeResetEvent resetEvent = GetResetEvent(requestId);
            if (resetEvent == null) return;

            try
            {
                if (resultField.ErrorID != 0)
                {
                    m_logger.WriteInformation(string.Format("{0}.OnRspQryInstrumentMarginRate() failed,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}.",
                                ToString(), requestId, resultField.ErrorID, resultField.ErrorMsg));

                    resetEvent.Tag = resultField;
                    resetEvent.Set(true);
                }
                else
                {
                    if (replyField.HasValue)
                    {
                        List<InstrumentMarginRateField> marginFields = resetEvent.Tag as List<InstrumentMarginRateField>;
                        Debug.Assert(marginFields != null);
                        marginFields.Add(replyField.Value);
                    }

                    resetEvent.IsFinish = isLast;
                    resetEvent.Set(false);
                }
            }
            catch (Exception ex)
            {
                //Debug.Assert(false, "OnRspQryInstrumentMarginRate() exception:" + ex.Message);
                m_logger.WriteError(string.Format("{0}.OnRspQryInstrumentMarginRate(),Error:{1}.",
                    ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 请求查询合约手续费率响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryInstrumentCommissionRate(ref InstrumentCommissionRateField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            Debug.Assert(isLast, "OnRspQryInstrumentCommissionRate() isLast is false");
            USeResetEvent resetEvent = GetResetEvent(requestId);
            if (resetEvent == null) return;

            try
            {
                if (resultField.ErrorID != 0)
                {
                    m_logger.WriteInformation(string.Format("{0}.OnRspQryInstrumentCommissionRate() failed,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}.",
                                ToString(), requestId, resultField.ErrorID, resultField.ErrorMsg));

                    resetEvent.Tag = resultField;
                    resetEvent.Set(true);
                }
                else
                {
                    if (replyField.HasValue)
                    {
                        List<InstrumentCommissionRateField> commissionFields = resetEvent.Tag as List<InstrumentCommissionRateField>;
                        Debug.Assert(commissionFields != null);
                        commissionFields.Add(replyField.Value);
                    }

                    resetEvent.IsFinish = isLast;
                    resetEvent.Set(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspQryInstrumentCommissionRate() exception:" + ex.Message);
                m_logger.WriteError(string.Format("{0}.OnRspQryInstrumentCommissionRate(),Error:{1}.",
                    ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 请求查询交易所响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryExchange(ref ExchangeField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询合约响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryInstrument(ref InstrumentField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            USeResetEvent resetEvent = GetResetEvent(requestId);
            if (resetEvent == null) return;

            try
            {
                if (resultField.ErrorID != 0)
                {
                    m_logger.WriteInformation(string.Format("{0}.OnRspQryInstrument() failed,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}.",
                                ToString(), requestId, resultField.ErrorID, resultField.ErrorMsg));

                    resetEvent.Tag = resultField;
                    resetEvent.Set(true);
                }
                else
                {
                    if (replyField.HasValue)
                    {
                        List<InstrumentField> instrumentFields = resetEvent.Tag as List<InstrumentField>;
                        Debug.Assert(instrumentFields != null);
                        instrumentFields.Add(replyField.Value);
                    }

                    resetEvent.IsFinish = isLast;
                    resetEvent.Set(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspQryInstrument() exception:" + ex.Message);
                m_logger.WriteError(string.Format("{0}.OnRspQryInstrument(),Error:{1}.",
                    ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 请求查询行情响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryDepthMarketData(ref DepthMarketDataField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询投资者结算结果响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQrySettlementInfo(ref SettlementInfoField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            try
            {
                USeResetEvent resetEvent = GetResetEvent(requestId);
                if (resetEvent == null) return;

                if (resultField.ErrorID != 0)
                {
                    m_logger.WriteInformation(string.Format("{0}.OnRspQrySettlementInfo() failed,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}.",
                                  ToString(), requestId, resultField.ErrorID, resultField.ErrorMsg));
                    resetEvent.Tag = resultField;
                    resetEvent.Set(true);
                }
                else
                {
                    if (replyField.HasValue)
                    {
                        List<SettlementInfoField> settlementInfoFields = resetEvent.Tag as List<SettlementInfoField>;
                        Debug.Assert(settlementInfoFields != null, "OnRspQrySettlementInfo() resetEvent.Tag is null");
                        settlementInfoFields.Add(replyField.Value);
                    }

                    resetEvent.IsFinish = isLast;
                    resetEvent.Set(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspQrySettlementInfo() exception:" + ex.Message);
                m_logger.WriteError(string.Format("{0}.OnRspQrySettlementInfo(),Error:{1}.",
                     ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 请求查询转帐银行响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryTransferBank(ref TransferBankField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询投资者持仓明细响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryInvestorPositionDetail(ref InvestorPositionDetailField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询客户通知。响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryNotice(ref NoticeField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询结算信息确认响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQrySettlementInfoConfirm(ref SettlementInfoConfirmField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            try
            {
                USeResetEvent resetEvent = GetResetEvent(requestId);
                if (resetEvent == null) return;

                if (resultField.ErrorID != 0)
                {
                    m_logger.WriteInformation(string.Format("{0}.OnRspQrySettlementInfoConfirm() failed,[RequestID:{1}][ErrorID:{2}][ErrorMessage:{3}.",
                                  ToString(), requestId, resultField.ErrorID, resultField.ErrorMsg));
                    resetEvent.Tag = resultField;
                    resetEvent.Set(true);
                }
                else
                {
                    if (replyField.HasValue)
                    {
                        List<SettlementInfoConfirmField> settlementInfoFields = resetEvent.Tag as List<SettlementInfoConfirmField>;
                        Debug.Assert(settlementInfoFields != null, "OnRspQrySettlementInfoConfirm() resetEvent.Tag is null");
                        settlementInfoFields.Add(replyField.Value);
                    }

                    resetEvent.IsFinish = isLast;
                    resetEvent.Set(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspQrySettlementInfoConfirm() exception:" + ex.Message);
                m_logger.WriteError(string.Format("{0}.OnRspQrySettlementInfoConfirm(),Error:{1}.",
                     ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 请求查询投资者持仓明细响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryInvestorPositionCombineDetail(ref InvestorPositionCombineDetailField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 查询保证金监管系统经纪公司资金账户密钥响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryCFMMCTradingAccountKey(ref CFMMCTradingAccountKeyField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询仓单折抵信息响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryEWarrantOffset(ref EWarrantOffsetField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询转帐流水响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryTransferSerial(ref TransferSerialField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询银期签约关系响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryAccountRegister(ref AccountRegisterField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询签约银行响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryContractBank(ref ContractBankField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询预埋单响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryParkedOrder(ref ParkedOrderField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询预埋撤单响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryParkedOrderAction(ref ParkedOrderActionField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询交易通知响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryTradingNotice(ref TradingNoticeField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询经纪公司交易参数响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryBrokerTradingParams(ref BrokerTradingParamsField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 请求查询经纪公司交易算法响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQryBrokerTradingAlgos(ref BrokerTradingAlgosField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 期货发起银行资金转期货应答。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspFromBankToFutureByFuture(ref ReqTransferField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 期货发起期货资金转银行应答。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspFromFutureToBankByFuture(ref ReqTransferField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }

        /// <summary>
        /// 期货发起查询银行余额应答。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public void OnRspQueryBankAccountMoneyByFuture(ref ReqQueryAccountField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
        }


        /// <summary>
        /// 报单通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnOrder(ref OrderField? reportField)
        {
            Debug.Assert(reportField.HasValue, "OnRtnOrder(),reportField is null");
            if (reportField.HasValue == false) return;

            if (this.DriverState != USeOrderDriverState.Ready)
            {
                //未完成登录初始化收到OrderBook,应暂存在处理，稍后处理
                return;
            }

            try
            {
                m_logger.WriteInformation(string.Format("{0}.OnRtnOrder(),{1}.", ToString(), OrderFieldToLogString(reportField.Value)));

                USeOrderNum orderNum = m_dataBuffer.UpdateOrderField(reportField.Value);

                USeOrderBook checkOrderBook = m_dataBuffer.GetCheckedOrderBook(orderNum);

                USePosition position = null;
                if (checkOrderBook.OffsetType != USeOffsetType.Open)
                {
                    USeDirection direction = checkOrderBook.OrderSide == USeOrderSide.Buy ? USeDirection.Short : USeDirection.Long;
                    position = m_dataBuffer.GetPosition(checkOrderBook.Instrument, direction);
                    Debug.Assert(position != null, "OnRtnOrder(),position is null");
                }

                FireOrderBookChanged(checkOrderBook);

                if (position != null)
                {
                    FirePositionChanged(position);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("OnRtnOrder(),Error:{0}", ex.Message));
                m_logger.WriteError(string.Format("{0}.OnRtnOrder() failed,Error:{1}.",
                            ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 成交通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnTrade(ref TradeField? reportField)
        {
            Debug.Assert(reportField.HasValue, "OnRtnTrade(),reportField is null");
            if (reportField.HasValue == false) return;

            if (this.DriverState != USeOrderDriverState.Ready)
            {
                //未完成登录初始化收到OrderBook,应暂存在处理，稍后处理
                return;
            }

            try
            {
                m_logger.WriteInformation(string.Format("{0}.OnRtnTrade(),{1}.", ToString(), TradeFieldToLogString(reportField.Value)));

                m_dataBuffer.AddTradeField(reportField.Value);
                string exchangeID = reportField.Value.ExchangeID;
                string orderSysID = reportField.Value.OrderSysID;
                string tradeID = reportField.Value.TradeID.Trim();
                USeTradeBook tradeBook = m_dataBuffer.GetTradeBook(new CtpOrderNum(exchangeID,orderSysID), tradeID);

                USeOrderBook orderBook = m_dataBuffer.GetCheckedOrderBook(tradeBook.OrderNum);
                //Debug.Assert(orderBook != null, "OnRtnTrade(),OrderBook is null");

                USeDirection direciton = tradeBook.OrderSide == USeOrderSide.Buy ? USeDirection.Long : USeDirection.Short;
                if (tradeBook.OffsetType != USeOffsetType.Open)
                {
                    direciton = direciton == USeDirection.Long ? USeDirection.Short : USeDirection.Long;
                }
                USePosition position = m_dataBuffer.GetPosition(tradeBook.Instrument, direciton);
                Debug.Assert(position != null, "OnRtnTrade(),position is null");

                List<USePositionDetail> positionDetail = m_dataBuffer.GetPositionDetail(tradeBook.Instrument, direciton);
                Debug.Assert(positionDetail != null && positionDetail.Count > 0, "OnRtnTrade(),positionDetail is null");

                FireTradeBookChanged(tradeBook,true);
                if (orderBook != null)
                {
                    //委托回报没产生，先收到成交回报
                    FireOrderBookChanged(orderBook);
                }
                FirePositionChanged(position);
                if (positionDetail != null && positionDetail.Count > 0)
                {
                    foreach (USePositionDetail detail in positionDetail)
                    {
                        FirePositionDetailChanged(detail);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, string.Format("OnRtnTrade(),Error:{0}", ex.Message));
                m_logger.WriteError(string.Format("{0}.OnRtnOrder() failed,Error:{1}.",
                            ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 报单录入错误回报。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        public void OnErrRtnOrderInsert(ref InputOrderField? reportField, ref RspInfoField resultField)
        {
            Debug.Assert(resultField.ErrorID != 0, "OnErrRtnOrderInsert() resultField.ErrorID less than zeror");
            Debug.Assert(reportField.HasValue, "OnErrRtnOrderInsert() reportField.HasValue is false");

            try
            {
                string orderRef = reportField.Value.OrderRef;
                CtpOrderNum orderNum = new CtpOrderNum(m_frontID, m_sessionID, orderRef);

                m_logger.WriteInformation(string.Format("{0}.OnErrRtnOrderInsert() ok,[OrderNum:{1}][ErrorID:{2}][ErrorMessage:{3}]",
                            ToString(), orderNum.OrderString, resultField.ErrorID, resultField.ErrorMsg));

                if (resultField.ErrorID != 0)
                {
                    m_dataBuffer.BlankOrder(orderNum, string.Format("({0}){1}", resultField.ErrorID, resultField.ErrorMsg));

                    USeOrderBook orderBook = m_dataBuffer.GetCheckedOrderBook(orderNum);
                    FireOrderBookChanged(orderBook);

                    if (orderBook != null)
                    {
                        USeDirection direction;
                        if (orderBook.OffsetType == USeOffsetType.Open)
                        {
                            direction = orderBook.OrderSide == USeOrderSide.Buy ? USeDirection.Long : USeDirection.Short;
                        }
                        else
                        {
                            direction = orderBook.OrderSide == USeOrderSide.Buy ? USeDirection.Short : USeDirection.Long;
                        }
                        USePosition position = m_dataBuffer.GetPosition(orderBook.Instrument, direction);
                        if (position != null)
                        {
                            FirePositionChanged(position);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnErrRtnOrderInsert() exception:" + ex.Message);
                m_logger.WriteInformation(string.Format("{0}.OnErrRtnOrderInsert() failed,Error:{1}.",
                            ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 报单操作错误回报。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        public void OnErrRtnOrderAction(ref OrderActionField? reportField, ref RspInfoField resultField)
        {
            try
            {
                //Debug.Assert(resultField.ErrorID != 0, "OnErrRtnOrderAction(),resultField.ErrorID !=0");
                //Debug.Assert(reportField.HasValue, "OnErrRtnOrderAction(),reportField.HasValue is false");

                if (resultField.ErrorID != 0)
                {
                    CtpOrderNum orderNum = new CtpOrderNum(reportField.Value.FrontID,
                                                                 reportField.Value.SessionID,
                                                                 reportField.Value.OrderRef);

                    m_logger.WriteInformation(string.Format("{0}.OnErrRtnOrderAction() ok,[OrderNum:{1}][ErrorID:{2}][ErrorMessage:{3}]",
                            ToString(), orderNum.OrderString, resultField.ErrorID, resultField.ErrorMsg));

                    USeOrderBook orderBook = m_dataBuffer.GetOrderBook(orderNum);
                    if (orderBook != null)
                    {
                        if (reportField.Value.ActionFlag != ActionFlagType.Delete)
                        {
                            orderBook.OrderStatus = USeOrderStatus.BlankOrder;
                        }
                        orderBook.Memo = resultField.ErrorMsg;

                        FireOrderBookChanged(orderBook);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnErrRtnOrderAction() exception:" + ex.Message);
                m_logger.WriteInformation(string.Format("{0}.OnErrRtnOrderAction() failed,Error:{1}.",
                            ToString(), ex.Message));
            }
        }

        /// <summary>
        /// 合约交易状态通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnInstrumentStatus(ref InstrumentStatusField? reportField)
        {
        }

        /// <summary>
        /// 交易通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnTradingNotice(ref TradingNoticeInfoField? reportField)
        {
        }

        /// <summary>
        /// 提示条件单校验错误。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnErrorConditionalOrder(ref ErrorConditionalOrderField? reportField)
        {
        }

        /// <summary>
        /// 银行发起银行资金转期货通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnFromBankToFutureByBank(ref RspTransferField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 银行发起期货资金转银行通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnFromFutureToBankByBank(ref RspTransferField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 银行发起冲正银行转期货通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnRepealFromBankToFutureByBank(ref RspRepealField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 银行发起冲正期货转银行通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnRepealFromFutureToBankByBank(ref RspRepealField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 期货发起银行资金转期货通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnFromBankToFutureByFuture(ref RspTransferField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 期货发起期货资金转银行通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnFromFutureToBankByFuture(ref RspTransferField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnRepealFromBankToFutureByFutureManual(ref RspRepealField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnRepealFromFutureToBankByFutureManual(ref RspRepealField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 期货发起查询银行余额通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnQueryBankBalanceByFuture(ref NotifyQueryAccountField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 期货发起银行资金转期货错误回报。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        public void OnErrRtnBankToFutureByFuture(ref ReqTransferField? reportField, ref RspInfoField resultField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 期货发起期货资金转银行错误回报。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        public void OnErrRtnFutureToBankByFuture(ref ReqTransferField? reportField, ref RspInfoField resultField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 系统运行时期货端手工发起冲正银行转期货错误回报。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        public void OnErrRtnRepealBankToFutureByFutureManual(ref ReqRepealField? reportField, ref RspInfoField resultField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 系统运行时期货端手工发起冲正期货转银行错误回报。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        public void OnErrRtnRepealFutureToBankByFutureManual(ref ReqRepealField? reportField, ref RspInfoField resultField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 期货发起查询银行余额错误回报。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        public void OnErrRtnQueryBankBalanceByFuture(ref ReqQueryAccountField? reportField, ref RspInfoField resultField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnRepealFromBankToFutureByFuture(ref RspRepealField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnRepealFromFutureToBankByFuture(ref RspRepealField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 银行发起银期开户通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnOpenAccountByBank(ref OpenAccountField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 银行发起银期销户通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnCancelAccountByBank(ref CancelAccountField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }

        /// <summary>
        /// 银行发起变更银行账号通知。
        /// </summary>
        /// <param name="reportField">回报信息数据结构。</param>
        public void OnRtnChangeAccountByBank(ref ChangeAccountField? reportField)
        {
            m_dataBuffer.InsertQueryInfo(new CtpQueryFundInfo());
        }
    }
}
