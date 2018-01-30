#region Copyright & Version
//==============================================================================
// 文件名称: CtpQueryApplication.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2017 USe LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2017/06/30
// 描    述: 实现CtpUserApplicationBase完成信息同步。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using CTPAPI;
using USe.TradeDriver.Common;
using USe.Common;

namespace USe.CtpOrderQuerier
{
    /// <summary>
    /// CTP查询应用(均为同步查询)。
    /// </summary>
    public class CtpOrderQuerier : CtpUserApplicationBase
    {
        #region member
        private string m_brokerID = string.Empty;     // BrokerID。
        private string m_investorID = string.Empty;   // 投资者代码
        private int m_frontID = 0;                    // 前置编号
        private int m_sessionID = 0;                  // 会话编号  

        private USeResetEvent m_connectEvent = null;
        private USeResetEvent m_loginEvent = null;
        private Dictionary<int, USeResetEvent> m_eventDic = null;

        private CommonIdCreator m_requetSeqIDCreator = null; // 请求命令ID生成对象

        private CtpUser m_ctpUser;

        private int m_queryTimeOut = 5000;
        private int m_loginTimeOut = 5000;

        private object m_object = new object();
        #endregion // member

        #region construction
        /// <summary>
        /// 构造SettlementPriceApplication实例。
        /// </summary>
        public CtpOrderQuerier()
        {
            m_requetSeqIDCreator = new CommonIdCreator();
            m_eventDic = new Dictionary<int, USeResetEvent>(16);
        }
        #endregion // construction

        #region 连接
        /// <summary>
        /// 连接CTP交易服务器。
        /// </summary>
        /// <param name="address">交易服务器地址。</param>
        /// <param name="port">交易服务器地址端口。</param>
        public void Connect(string address, int port, int loginTimeout, int queryTimeout)
        {
            if (loginTimeout <= 0)
            {
                throw new ArgumentNullException("loginTimeout", "loginTimeout can not be null or empty.");
            }
            if (queryTimeout <= 0)
            {
                throw new ArgumentNullException("queryTimeout", "queryTimeout can not be null or empty.");
            }
            Debug.Assert(m_connectEvent == null, "ConnectEvent is not null.");

            m_loginTimeOut = loginTimeout;
            m_queryTimeOut = queryTimeout;

            m_ctpUser = new CtpUser("");
            m_ctpUser.SetApplication(this);

            Debug.Assert(m_ctpUser != null);
            try
            {
                m_connectEvent = new USeResetEvent();
                m_ctpUser.ConnectAsync(string.Format(@"tcp://{0}:{1}", address, port));

                if (m_connectEvent.WaitOne(m_loginTimeOut) == false)
                {
                    throw new Exception("连接超时");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_connectEvent = null;
            }
        }

        /// <summary>
        /// 与前置机连接成功时的回调方法。
        /// </summary>
        public override void OnFrontConnect()
        {
            if (m_connectEvent != null)
            {
                m_connectEvent.Set(false);
            }
        }
        #endregion // 连接

        #region 断开连接
        /// <summary>
        /// 断开连接。
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (m_ctpUser != null && m_ctpUser.IsLogin)
                {
                    m_ctpUser.LogoutAsync(-1);
                }

                m_ctpUser.Dispose();
                m_ctpUser = null;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 与前置机连接中断时的回调方法。
        /// </summary>
        /// <param name="reason">连接中断的原因，参见CTP API文档。</param>
        public override void OnFrontDisconnect(int reason)
        {
        }
        #endregion 

        #region 登录
        /// <summary>
        /// 登录CTP。
        /// </summary>
        /// <param name="account">交易帐号。</param>
        /// <param name="password">交易密码(明文)。</param>
        /// <param name="brokerID">所属经纪商ID。</param>
        public void Login(string account, string password, string brokerID)
        {
            m_investorID = account;
            m_brokerID = brokerID;
            int requestID = m_requetSeqIDCreator.Next();

            try
            {
                m_loginEvent = new USeResetEvent(requestID);

                m_ctpUser.LoginAsync(m_brokerID, account, password, "", requestID);
                if (m_loginEvent.WaitOne(5000) == false)
                {
                    throw new Exception("登录超时");
                }
                else
                {
                    if (m_loginEvent.IsError)
                    {
                        Debug.Assert(m_loginEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)m_loginEvent.Tag;
                        Debug.Assert(rspInfo.ErrorID != 0);
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_loginEvent = null;
            }
        }

        /// <summary>
        /// 登录请求响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public override void OnRspUserLogin(ref RspUserLoginField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            Debug.Assert(isLast, "OnRspUserLogin() islast == false");

            if (m_loginEvent == null) return;

            if (resultField.ErrorID != 0 || replyField.HasValue == false)
            {
                m_loginEvent.Tag = resultField;
                m_loginEvent.Set(true);
            }
            else
            {
                m_frontID = replyField.Value.FrontID;
                m_sessionID = replyField.Value.SessionID;

                int requestID = m_requetSeqIDCreator.Next();
                SettlementInfoConfirmField requestField = new SettlementInfoConfirmField();
                requestField.BrokerID = m_brokerID;
                requestField.InvestorID = m_investorID;

                m_ctpUser.ReqSettlementInfoConfirm(ref requestField, requestID);

                m_loginEvent.Set(false);
            }
        }
        #endregion 

        #region 查询合约列表
        /// <summary>
        /// 查询所有合约信息。
        /// </summary>
        /// <returns></returns>
        public List<InstrumentField> QueryInstument()
        {
            int requestID = m_requetSeqIDCreator.Next();

            try
            {
                List<InstrumentField> instrumentList = new List<InstrumentField>();
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = instrumentList;
                m_eventDic.Add(queryEvent.EventID, queryEvent);

                QryInstrumentField qryField = new QryInstrumentField();
                m_ctpUser.ReqQryInstrument(ref qryField, requestID);
                if (queryEvent.WaitOne(m_queryTimeOut) == false)
                {
                    throw new Exception("Query time out.");
                }
                else
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }
                    while (true)
                    {
                        if (queryEvent.IsFinish) break;
                        // 继续等待应答结果
                        if (queryEvent.WaitOne(m_queryTimeOut) == false)
                        {
                            throw new Exception(string.Format("({0}){1}", "99", "查询超时"));
                        }
                    }
                }

                return instrumentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }
        }

        /// <summary>
        /// 请求查询合约响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public override void OnRspQryInstrument(ref InstrumentField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            try
            {
                Debug.Assert(m_eventDic.ContainsKey(requestId), string.Format("RequestID({0}) didn't exist.", requestId));
                if (m_eventDic.ContainsKey(requestId) == false) return;

                USeResetEvent resetEvent = m_eventDic[requestId];

                if (resultField.ErrorID != 0)
                {
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
                Debug.Assert(false, "OnRspQryInstrument() " + ex.Message);
            }
        }
        #endregion 

        #region 查询深度行情数据
        /// <summary>
        /// 查询所有合约深度行情数据。
        /// </summary>
        /// <returns></returns>
        public List<DepthMarketDataField> QueryDepthMarketData()
        {
            int requestID = m_requetSeqIDCreator.Next();
            try
            {
                List<DepthMarketDataField> marketList = new List<DepthMarketDataField>();
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = marketList;
                m_eventDic.Add(queryEvent.EventID, queryEvent);

                QryDepthMarketDataField field = new QryDepthMarketDataField();
                m_ctpUser.ReqQryDepthMarketData(ref field, requestID);

                if (queryEvent.WaitOne(m_queryTimeOut) == false)
                {
                    throw new Exception("查询超时");
                }
                else
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }
                    while (true)
                    {
                        if (queryEvent.IsFinish) break;
                        // 继续等待应答结果
                        if (queryEvent.WaitOne(m_queryTimeOut) == false)
                        {
                            throw new Exception(string.Format("({0}){1}", "99", "time out"));
                        }
                    }
                }

                return marketList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }
        }

        /// <summary>
        /// 请求查询行情响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public override void OnRspQryDepthMarketData(ref DepthMarketDataField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            try
            {
                Debug.Assert(m_eventDic.ContainsKey(requestId), string.Format("RequestID({0}) didn't exist.", requestId));
                if (m_eventDic.ContainsKey(requestId) == false) return;

                USeResetEvent resetEvent = m_eventDic[requestId];

                if (resultField.ErrorID != 0)
                {
                    resetEvent.Tag = resultField;
                    resetEvent.Set(true);
                }
                else
                {
                    if (replyField.HasValue)
                    {
                        List<DepthMarketDataField> marketList = resetEvent.Tag as List<DepthMarketDataField>;
                        Debug.Assert(marketList != null);
                        marketList.Add(replyField.Value);
                    }

                    resetEvent.IsFinish = isLast;
                    resetEvent.Set(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "OnRspQryDepthMarketData() " + ex.Message);
            }
        }
        #endregion //

        #region 查询合约手续费
        /// <summary>
        /// 查询合约手续费。
        /// </summary>
        /// <param name="instrumentCode"></param>
        /// <returns></returns>
        public InstrumentCommissionRateField? QueryInstrumentFee(string instrumentCode)
        {
            int requestID = m_requetSeqIDCreator.Next();
            try
            {
                List<InstrumentCommissionRateField> feeList = new List<InstrumentCommissionRateField>();
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = feeList;
                m_eventDic.Add(queryEvent.EventID, queryEvent);

                QryInstrumentCommissionRateField field = new QryInstrumentCommissionRateField();
                field.BrokerID = m_brokerID;
                field.InvestorID = m_investorID;
                field.InstrumentID = instrumentCode;
                m_ctpUser.ReqQryInstrumentCommissionRate(ref field, requestID);

                if (queryEvent.WaitOne(m_queryTimeOut) == false)
                {
                    throw new Exception("查询超时");
                }
                else
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }
                    while (true)
                    {
                        if (queryEvent.IsFinish) break;
                        // 继续等待应答结果
                        if (queryEvent.WaitOne(m_queryTimeOut) == false)
                        {
                            throw new Exception(string.Format("({0}){1}", "99", "超时"));
                        }
                    }
                }

                if (feeList.Count > 0)
                {
                    Debug.Assert(feeList.Count == 1);
                    return feeList[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }
        }

        /// <summary>
        /// 请求查询合约手续费率响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public override void OnRspQryInstrumentCommissionRate(ref InstrumentCommissionRateField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            try
            {
                Debug.Assert(m_eventDic.ContainsKey(requestId), string.Format("RequestID({0}) didn't exist.", requestId));
                if (m_eventDic.ContainsKey(requestId) == false) return;

                USeResetEvent resetEvent = m_eventDic[requestId];

                if (resultField.ErrorID != 0)
                {
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
                Debug.Assert(false, "OnRspQryInstrumentCommissionRate(),Error:" + ex.Message);
            }
        }
        #endregion 

        #region 查询合约保证金
        /// <summary>
        /// 查询合约保证金。
        /// </summary>
        /// <param name="instrumentCode"></param>
        /// <returns></returns>
        public InstrumentMarginRateField? QueryInstrumentMargin(string instrumentCode)
        {
            int requestID = m_requetSeqIDCreator.Next();
            try
            {
                List<InstrumentMarginRateField> marginList = new List<InstrumentMarginRateField>();
                USeResetEvent queryEvent = new USeResetEvent(requestID);
                queryEvent.Tag = marginList;
                m_eventDic.Add(queryEvent.EventID, queryEvent);

                QryInstrumentMarginRateField field = new QryInstrumentMarginRateField();
                field.BrokerID = m_brokerID;
                field.InvestorID = m_investorID;
                field.InstrumentID = instrumentCode;
                field.HedgeFlag = HedgeFlagType.Speculation;
                m_ctpUser.ReqQryInstrumentMarginRate(ref field, requestID);

                if (queryEvent.WaitOne(m_queryTimeOut) == false)
                {
                    throw new Exception("查询超时");
                }
                else
                {
                    if (queryEvent.IsError)
                    {
                        Debug.Assert(queryEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)queryEvent.Tag;
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }
                    while (true)
                    {
                        if (queryEvent.IsFinish) break;
                        // 继续等待应答结果
                        if (queryEvent.WaitOne(m_queryTimeOut) == false)
                        {
                            throw new Exception(string.Format("({0}){1}", "99", "超时"));
                        }
                    }
                }

                if (marginList.Count > 0)
                {
                    Debug.Assert(marginList.Count == 1);
                    return marginList[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }
        }

        /// <summary>
        /// 请求查询合约保证金响应。
        /// </summary>
        /// <param name="replyField">与请求对应的应答信息数据结构。</param>
        /// <param name="resultField">请求的执行结果信息数据结构。</param>
        /// <param name="requestId">标识请求的请求编号。</param>
        /// <param name="isLast">最后一个应答标志。</param>
        public override void OnRspQryInstrumentMarginRate(ref InstrumentMarginRateField? replyField, ref RspInfoField resultField, int requestId, bool isLast)
        {
            try
            {
                Debug.Assert(m_eventDic.ContainsKey(requestId), string.Format("RequestID({0}) didn't exist.", requestId));
                if (m_eventDic.ContainsKey(requestId) == false) return;

                USeResetEvent resetEvent = m_eventDic[requestId];

                if (resultField.ErrorID != 0)
                {
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
                Debug.Assert(false, "OnRspQryInstrumentMarginRate(),Error:" + ex.Message);
            }
        }
        #endregion // methods
    }
}
