#region Copyright & Version
//==============================================================================
// 文件名称: CtpOrderDriver.USeOrderDriver.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: CTP交易驱动类--实现USeOrderDriver接口。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading;

using USe.TradeDriver.Common;
using CTPAPI;

namespace USe.TradeDriver.Ctp
{
    public partial class CtpOrderDriver : USeOrderDriver
    {
        /// <summary>
        /// 连接交易服务器。
        /// </summary>
        public override void ConnectServer()
        {
            if (m_eventDic.ContainsKey(0))
            {
                throw new Exception(string.Format("{0} is connecting.", ToString()));
            }

            FireOrderDriverStateChanged(USeOrderDriverState.Connecting, "");

            Debug.Assert(string.IsNullOrEmpty(m_ctpUserStreamFilePath) == false);
            m_ctpUser = new CtpUser(m_ctpUserStreamFilePath);
            m_ctpUser.SetApplication(this);
            Debug.Assert(m_ctpUser != null, "m_ctpUser is null");

            try
            {
                USeResetEvent connectEvent = new USeResetEvent(0);
                m_eventDic.Add(connectEvent.EventID, connectEvent);

                m_ctpUser.ConnectAsync(string.Format(@"tcp://{0}:{1}", m_address, m_port));

                if (connectEvent.WaitOne(m_connectTimeOut) == false)
                {
                    throw new Exception("Connect time out.");
                }

                FireOrderDriverStateChanged(USeOrderDriverState.Connected, "");
            }
            catch (Exception ex)
            {
                FireOrderDriverStateChanged(USeOrderDriverState.DisConnected, ex.Message);
                m_logger.WriteError(string.Format("{0} connect failed,Error: {1}.", ToString(), ex.Message));
                throw ex;
            }
            finally
            {
                m_eventDic.Remove(0);
            }
        }

        /// <summary>
        /// 期货公司认证
        /// </summary>
        public override void AuthorT(string brokerID ,string userId,string authCode,string userProductInfo)
        {
            FireOrderDriverStateChanged(USeOrderDriverState.Authoring, "");

            Debug.Assert(m_ctpUser != null, "m_ctpUser is null");

            int requestID = m_requetSeqIDCreator.Next();

            try
            {
                USeResetEvent resetEvent = new USeResetEvent(requestID);
                m_eventDic.Add(resetEvent.EventID, resetEvent);

                ReqAuthenticateField field = new ReqAuthenticateField()
                {
                    BrokerID = brokerID,
                    UserID = userId,
                    AuthCode = authCode,
                    UserProductInfo = userProductInfo
                };

                m_ctpUser.ReqAuthenticate(ref field, requestID);

                if (resetEvent.WaitOne(m_connectTimeOut) == false)
                //if (resetEvent.WaitOne(50000) == false)
                {
                    throw new Exception("AuthorT time out.");
                }
                else
                {
                    if (resetEvent.IsError)
                    {
                        Debug.Assert(resetEvent.Tag != null);
                        ReqAuthenticateField rspInfo = (ReqAuthenticateField)resetEvent.Tag;
                        Debug.Assert(rspInfo.UserProductInfo != null && rspInfo.UserProductInfo != "");
                        throw new Exception(string.Format("({0}){1}", rspInfo.AuthCode, rspInfo.UserProductInfo));
                    }
                }
            }
            catch (Exception ex)
            {
                FireOrderDriverStateChanged(USeOrderDriverState.AuthorFieldOut, ex.Message);
                m_logger.WriteError(string.Format("{0} AuthorT failed,Error: {1}.", ToString(), ex.Message));
                throw ex;
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }

            FireOrderDriverStateChanged(USeOrderDriverState.AuthorSuccessOn, "");
        }

        /// <summary>
        /// 断开交易服务器。
        /// </summary>
        public override void DisConnectServer()
        {
            try
            {
                if (m_ctpUser != null && m_ctpUser.IsLogin)
                {
                    m_ctpUser.LogoutAsync(-1);
                }
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0} disconnect failed,Error: {1}.", ToString(), ex.Message));
                throw ex;
            }
            finally
            {
                FireOrderDriverStateChanged(USeOrderDriverState.DisConnected, "");
            }
        }

        /// <summary>
        /// 登录交易服务器。
        /// </summary>
        /// <param name="brokerId">经纪商ID。</param>
        /// <param name="account">帐号。</param>
        /// <param name="password">密码。</param>
        public override void Login(string brokerId,string account, string password)
        {
            FireOrderDriverStateChanged(USeOrderDriverState.LoggingOn, "");

            m_dataBuffer.Clear();

            m_brokerID = brokerId;
            m_investorID = account;
            int requestID = m_requetSeqIDCreator.Next();

            try
            {
                USeResetEvent resetEvent = new USeResetEvent(requestID);
                m_eventDic.Add(resetEvent.EventID, resetEvent);

                m_ctpUser.LoginAsync(m_brokerID, account, password,"", requestID);
                if (resetEvent.WaitOne(m_connectTimeOut) == false)
                //if (resetEvent.WaitOne(50000) == false)
                {
                    throw new Exception("Login time out.");
                }
                else
                {
                    if (resetEvent.IsError)
                    {
                        Debug.Assert(resetEvent.Tag != null);
                        RspInfoField rspInfo = (RspInfoField)resetEvent.Tag;
                        Debug.Assert(rspInfo.ErrorID != 0);
                        throw new Exception(string.Format("({0}){1}", rspInfo.ErrorID, rspInfo.ErrorMsg));
                    }
                }
            }
            catch (Exception ex)
            {
                FireOrderDriverStateChanged(USeOrderDriverState.DisConnected, ex.Message);
                m_logger.WriteError(string.Format("{0} login failed,Error: {1}.", ToString(), ex.Message));
                throw ex;
            }
            finally
            {
                m_eventDic.Remove(requestID);
            }
            FireOrderDriverStateChanged(USeOrderDriverState.LoggingOn, "");

            List<InstrumentField> instrumentList = null;
            List<OrderField> orderBookList = null;
            List<TradeField> tradeBookList = null;
            List<InvestorPositionField> positionList = null;
            TradingAccountField tradeAccountField;
            InvestorField investorField;
            try
            {
                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "正在查询投资者信息");
                investorField = QueryInvestorBaseInfoFromCtp();
                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "查询投资者信息完成");
                Thread.Sleep(1000);

                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "正在查询合约信息");
                instrumentList = QueryAllInstrumentFromCtp();
                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "查询合约信息完成");
                Thread.Sleep(1000);

                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "正在查询委托单信息");
                orderBookList = QueryAllOrderFieldFromCtp();
                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "查询委托单信息完成");
                Thread.Sleep(1000);

                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "正在查询成交回报信息");
                tradeBookList = QueryAllTradeFieldFromCtp();
                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "查询成交回报信息完成");
                Thread.Sleep(1000);

                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "正在查询持仓信息");
                positionList = QueryInvestorPositionFromCtp();
                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "查询持仓信息完成");
                Thread.Sleep(1000);

                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "正在查询资金帐号信息");
                tradeAccountField = QueryTradingAccountFromCtp();
                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "查询资金帐号信息完成");
                Thread.Sleep(1000);

                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "正在查询结算单信息");
                m_needSettlementConfirm = QueryNeedSettlementInfoConfirmFromCtp();
                FireOrderDriverStateChanged(USeOrderDriverState.Loading, "查询结算单信息完成");
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                m_logger.WriteError(ex.Message);
                FireOrderDriverStateChanged(USeOrderDriverState.DisConnected, ex.Message);
                throw ex;
            }

            try
            {
                m_dataBuffer.InitializeInstrumentInfo(instrumentList);
                m_dataBuffer.InitializeData(orderBookList, tradeBookList, positionList, tradeAccountField, investorField);
            }
            catch (Exception ex)
            {
                m_logger.WriteError(ex.Message);
                m_dataBuffer.Clear();
                FireOrderDriverStateChanged(USeOrderDriverState.DisConnected, ex.Message);
                throw ex;
            }

            m_password = password;
            FireOrderDriverStateChanged(USeOrderDriverState.Ready, "");

            m_queryTimer.Change(2000, Timeout.Infinite); // 启动手续费保证金查询
            m_queryAccountTimer.Change(QUERY_ACCOUNT_INFO_INTERVAL, Timeout.Infinite); // 启动帐号资金定时查询
        }

        /// <summary>
        /// 登出交易服务器。
        /// </summary>
        public override void Logout()
        {
            try
            {
                m_queryTimer.Change(Timeout.Infinite, Timeout.Infinite);
                m_queryAccountTimer.Change(Timeout.Infinite, Timeout.Infinite);

                Thread.Sleep(1000); // 延时1秒防止正在查询
                int requestID = m_requetSeqIDCreator.Next();
                if (m_ctpUser != null)
                {
                    m_ctpUser.LogoutAsync(requestID);
                    m_ctpUser.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            FireOrderDriverStateChanged(USeOrderDriverState.DisConnected, "");
        }

        /// <summary>
        /// 结算单确认。
        /// </summary>
        public override void SettlementInfoConfirm()
        {
            int requestID = m_requetSeqIDCreator.Next();
            SettlementInfoConfirmField requestField = new SettlementInfoConfirmField();
            requestField.BrokerID = m_brokerID;
            requestField.InvestorID = m_investorID;

            m_ctpUser.ReqSettlementInfoConfirm(ref requestField, requestID);
        }

        /// <summary>
        /// 获取确认结算单。
        /// </summary>
        /// <returns>确认结算单信息。</returns>
        public override string GetSettlementInfo(string tradingDate)
        {
            List<SettlementInfoField> fields = QuerySettlementInfoFromCtp(tradingDate);
            StringBuilder sb = new StringBuilder();
            if (fields != null)
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    sb.Append(fields[i].Content);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 查询投资者基本信息。
        /// </summary>
        /// <returns></returns>
        public override USeInvestorBaseInfo QueryInvestorInfo()
        {
            return m_dataBuffer.GetInvestorBaseInfo();
        }

        /// <summary>
        /// 查询所有期货合约详细信息。
        /// </summary>
        /// <returns></returns>
        public override List<USeInstrumentDetail> QueryInstrumentDetail()
        {
            List<USeInstrumentDetail> list = m_dataBuffer.GetInstrumentDetail();
            return list;
        }

        /// <summary>
        /// 查询指定合约详细信息。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <returns>合约详细信息。</returns>
        public override USeInstrumentDetail QueryInstrumentDetail(USeInstrument instrument)
        {
            return m_dataBuffer.GetInstrumentDetail(instrument);
        }

        /// <summary>
        /// 查询指定品种的所有合约详细信息。
        /// </summary>
        /// <param name="varieties">品种类型。</param>
        /// <returns></returns>
        public override List<USeInstrumentDetail> QueryInstrumentDetail(string varieties)
        {
            List<USeInstrumentDetail> list = m_dataBuffer.GetInstrumentDetailByVarieties(varieties);
            return list;
        }

        /// <summary>
        /// 查询所有产品信息。
        /// </summary>
        /// <returns>产品信息。</returns>
        public override List<USeProductDetail> QueryProductDetails()
        {
            List<USeProductDetail> list = m_dataBuffer.GetProductDetail();
            return list;
        }

        /// <summary>
        /// 查询产品信息。
        /// </summary>
        /// <param name="productCode">产品代码。</param>
        /// <returns>产品信息。</returns>
        public override USeProductDetail QueryProductDetail(string productCode)
        {
            return m_dataBuffer.GetProductDetail(productCode);
        }

        /// <summary>
        /// 查询所有产品信息。
        /// </summary>
        /// <returns>产品信息。</returns>
        public override List<USeProduct> QueryProducts()
        {
            List<USeProductDetail> list = m_dataBuffer.GetProductDetail();
            List<USeProduct> result = (from p in list
                                        select new USeProduct() {
                                            ProductCode = p.ProductCode,
                                            ShortName = p.ShortName,
                                            LongName = p.LongName,
                                            Market = p.Market,
                                            VolumeMultiple = p.VolumeMultiple,
                                            PriceTick = p.PriceTick
                                        }).ToList();
            return result;
        }

        /// <summary>
        /// 查询产品信息。
        /// </summary>
        /// <param name="productCode">产品代码。</param>
        /// <returns>产品信息。</returns>
        public override USeProduct QueryProduct(string productCode)
        {
            USeProductDetail productDetail = m_dataBuffer.GetProductDetail(productCode);
            if (productDetail != null)
            {
                return new USeProduct() {
                    ProductCode = productDetail.ProductCode,
                    ShortName = productDetail.ShortName,
                    LongName = productDetail.LongName,
                    Market = productDetail.Market,
                    VolumeMultiple = productDetail.VolumeMultiple,
                    PriceTick = productDetail.PriceTick
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询合约的合约乘数。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <returns>合约乘数。</returns>
        public override int QueryInstrumentVolumeMultiple(USeInstrument instrument)
        {
            return m_dataBuffer.GetVolumeMultiple(instrument);
        }

        /// <summary>
        /// 查询合约手续费。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <returns></returns>
        public override USeFee QueryInstrumentFee(USeInstrument instrument)
        {
            if (m_dataBuffer.ExistInstrument(instrument.InstrumentCode) == false)
            {
                throw new Exception(string.Format("Unsupport instrument[{0}]", instrument));
            }

            USeFee fee = m_dataBuffer.GetFee(instrument);
            if (fee != null)
            {
                return fee;
            }
            else
            {
                m_queryTimer.Change(Timeout.Infinite, Timeout.Infinite); // 先暂停自动查询
                try
                {
                    int index = 3;
                    while (index > 0)
                    {
                        index--;
                        try
                        {
                            InstrumentCommissionRateField field = QueryCommissionRateFieldFromCtp(instrument.InstrumentCode);
                            m_dataBuffer.UpdateInstrumentFee(field, instrument.InstrumentCode);

                            List<USeTradeBook> tradeBookList = m_dataBuffer.GetTradeBook(instrument);
                            List<USeOrderBook> orderBookList = m_dataBuffer.GetCheckedOrderBook(instrument);

                            if (tradeBookList != null && tradeBookList.Count > 0)
                            {
                                foreach (USeTradeBook tradeBook in tradeBookList)
                                {
                                    FireTradeBookChanged(tradeBook,false);
                                }
                            }
                            if (orderBookList != null && orderBookList.Count > 0)
                            {
                                foreach (USeOrderBook orderBook in orderBookList)
                                {
                                    FireOrderBookChanged(orderBook);
                                }
                            }


                            fee = m_dataBuffer.GetFee(instrument);
                            Debug.Assert(fee != null, "QueryInstrumentFee(),fee is null");
                            return fee;
                        }
                        catch (Exception ex)
                        {
                            m_logger.WriteError(string.Format("{0}.QueryInstrumentFee() failed,Error:{1}.", ToString(), ex.Message));
                        }

                        Thread.Sleep(1000);
                    }

                    Debug.Assert(false, "QueryInstrumentFee(),fee is null");

                    // 如果还查不到则抛出异常
                    throw new Exception(string.Format("Query instrument[{0}] fee failed", instrument));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    m_queryTimer.Change(1000, Timeout.Infinite); // 启动自动查询
                }
            }
        }

        /// <summary>
        /// 查询合约保证金。
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        public override USeMargin QueryInstrumentMargin(USeInstrument instrument)
        {
            if (m_dataBuffer.ExistInstrument(instrument.InstrumentCode) == false)
            {
                throw new Exception(string.Format("Unsupport instrument[{0}]", instrument));
            }

            USeMargin margin = m_dataBuffer.GetMargin(instrument);
            if (margin != null)
            {
                return margin;
            }
            else
            {
                int index = 3;
                while (index > 0)
                {
                    index--;
                    try
                    {
                        InstrumentMarginRateField field = QueryMarginFromCtp(instrument.InstrumentCode);
                        m_dataBuffer.UpdateInstrumentMagin(field);

                        margin = m_dataBuffer.GetMargin(instrument);
                        Debug.Assert(margin != null, "QueryInstrumentMargin(),margin is null");
                        return margin;
                    }
                    catch
                    {
                    }

                    Thread.Sleep(1200);
                }
                Debug.Assert(false, "QueryInstrumentMargin(),margin is null");

                // 如果还查不到则抛出异常
                throw new Exception(string.Format("Query instrument[{0}] margin failed", instrument));
            }
        }

        /// <summary>
        /// 查询当日所有委托回报。
        /// </summary>
        /// <returns></returns>
        public override List<USeOrderBook> QueryOrderBooks()
        {
            List<USeOrderBook> list = m_dataBuffer.GetAllCheckedOrderBook();
            return list;
        }

        /// <summary>
        /// 查询指定委托单号的委托回报。
        /// </summary>
        /// <param name="orderNum"></param>
        /// <returns></returns>
        public override USeOrderBook QueryOrderBook(USeOrderNum orderNum)
        {
            return m_dataBuffer.GetCheckedOrderBook(orderNum);
        }

        /// <summary>
        /// 查询当日所有成交回报。
        /// </summary>
        /// <returns></returns>
        public override List<USeTradeBook> QueryTradeBooks()
        {
            List<USeTradeBook> list = m_dataBuffer.GetAllTradeBook();
            return list;
        }

        ///// <summary>
        ///// 查询当日某一合约的成交回报。
        ///// </summary>
        ///// <param name="product">合约。</param>
        ///// <returns></returns>
        //public override USeTradeBook[] QueryTradeBooks(USeInstrument product)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// 查询当日所有持仓。
        /// </summary>
        /// <returns></returns>
        public override List<USePosition> QueryPositions()
        {
            List<USePosition> list = m_dataBuffer.GetAllPosition();
            return list;
        }

        /// <summary>
        /// 查询某一合约当前持仓。
        /// </summary>
        /// <param name="product">产品。</param>
        /// <returns></returns>
        public override List<USePosition> QueryPositions(USeInstrument product)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询某一合约指定方向当前持仓。
        /// </summary>
        /// <param name="product"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override USePosition QueryPositions(USeInstrument instrument, USeDirection direction)
        {
            return m_dataBuffer.GetPosition(instrument, direction);
        }

        /// <summary>
        /// 查询持仓明细。
        /// </summary>
        /// <returns></returns>
        public override List<USePositionDetail> QueryPositionDetail()
        {
            List<USePositionDetail> list = m_dataBuffer.GetAllPositionDetail();
            return list;
        }

        /// <summary>
        /// 查询账户信息。
        /// </summary>
        /// <returns></returns>
        public override USeFund QueryFundInfo()
        {
            return m_dataBuffer.GetFund();
        }

        /// <summary>
        /// 查询帐户详细资金信息。
        /// </summary>
        /// <returns></returns>
        public override USeFundDetail QueryFundDetailInfo()
        {
            try
            {
                TradingAccountField field = QueryTradingAccountFromCtp();
                USeFundDetail fundDetail = CtpTradingAccountFieldToUSeFundDetail(field);
                return fundDetail;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 委托下单。
        /// </summary>
        /// <param name="instrument">委托产品。</param>
        /// <param name="qty">委托量。</param>
        /// <param name="price">委托价格。</param>
        /// <param name="offsetType">开平仓方向。</param>
        /// <param name="orderSide">买卖方向。</param>
        /// <param name="error">[out]委托失败原因。</param>
        /// <returns>委托单号。</returns>
        /// <remarks>返回为null代表失败,否则为委托单号。</remarks>
        public override USeOrderNum PlaceOrder(USeInstrument instrument, int qty, decimal price, USeOffsetType offsetType, USeOrderSide orderSide, out string error)
        {
            if (m_ctpUser == null || m_ctpUser.IsLogin == false)
            {
                error = "OrderServer unable";
                return null;
            }

            string orderRef = m_orderRefIDCreator.Next().ToString(); // 生成报单引用
            int requestID = m_requetSeqIDCreator.Next();
            error = string.Empty;

            try
            {
                OffsetFlagType ctpOffsetFlag;
                switch (offsetType)
                {
                    case USeOffsetType.Open: ctpOffsetFlag = OffsetFlagType.Open; break;
                    case USeOffsetType.Close: ctpOffsetFlag = OffsetFlagType.Close; break;
                    case USeOffsetType.CloseToday: ctpOffsetFlag = OffsetFlagType.CloseToday; break;
                    case USeOffsetType.CloseHistory: ctpOffsetFlag = OffsetFlagType.CloseYesterday; break;
                    default:
                        throw new ArgumentException(string.Format("Invalid offsetType {0}.", offsetType), "offsetType");
                }

                DirectionType ctpDirection;
                switch (orderSide)
                {
                    case USeOrderSide.Buy: ctpDirection = DirectionType.Buy; break;
                    case USeOrderSide.Sell: ctpDirection = DirectionType.Sell; break;
                    default:
                        throw new ArgumentException(string.Format("Invalid orderside {0}.", orderSide), "orderSide");
                }

                InputOrderField requestField = new InputOrderField();
                requestField.BrokerID = m_brokerID;
                requestField.InvestorID = m_investorID;
                requestField.InstrumentID = instrument.InstrumentCode;
                requestField.OrderRef = orderRef;
                requestField.UserID = m_investorID;
                requestField.OrderPriceType = OrderPriceType.LimitPrice;
                requestField.Direction = ctpDirection;
                requestField.CombOffsetFlag1 = ctpOffsetFlag;
                requestField.CombHedgeFlag1 = HedgeFlagType.Speculation;
                requestField.LimitPrice = Convert.ToDouble(price);
                requestField.VolumeTotalOriginal = qty;
                requestField.TimeCondition = TimeConditionType.GFD;
                requestField.VolumeCondition = VolumeConditionType.AV;
                requestField.MinVolume = 1;
                requestField.ContingentCondition = ContingentConditionType.Immediately;
                requestField.ForceCloseReason = ForceCloseReasonType.NotForceClose;
                requestField.IsAutoSuspend = IntBoolType.No;
                requestField.BusinessUnit = null;
                requestField.RequestID = requestID;
                requestField.UserForceClose = IntBoolType.No;

                //构造一个委托回报,防止报单不合规等问题遭CTP拒绝，但未有委托回报推送
                OrderField orderField = new OrderField();
                orderField.BrokerID = m_brokerID;
                orderField.FrontID = m_frontID;
                orderField.SessionID = m_sessionID;
                orderField.OrderRef = orderRef;
                orderField.OrderSysID = string.Empty;
                orderField.InvestorID = m_investorID;
                orderField.InstrumentID = instrument.InstrumentCode;
                orderField.ExchangeID = CtpProtocol.USeMarketToFtdcExchange(instrument.Market);
                orderField.VolumeTotalOriginal = qty;
                orderField.LimitPrice = Convert.ToDouble(price);
                orderField.VolumeTraded = 0;
                orderField.OrderStatus = OrderStatusType.Unknown;
                orderField.Direction = orderSide == USeOrderSide.Buy ? DirectionType.Buy : DirectionType.Sell;

                switch (offsetType)
                {
                    case USeOffsetType.Open: orderField.CombOffsetFlag1 = OffsetFlagType.Open; break;
                    case USeOffsetType.Close: orderField.CombOffsetFlag1 = OffsetFlagType.Close; break;
                    case USeOffsetType.CloseHistory: orderField.CombOffsetFlag1 = OffsetFlagType.CloseYesterday; break;
                    case USeOffsetType.CloseToday: orderField.CombOffsetFlag1 = OffsetFlagType.CloseToday; break;
                }
                orderField.InsertDate = DateTime.Now.ToString("yyyyMMdd");
                orderField.InsertTime = DateTime.Now.ToString("HH:mm:ss");

                m_dataBuffer.UpdateOrderField(orderField);


                m_ctpUser.ReqOrderInsert(ref requestField, requestID);

                USeOrderNum orderNum = new CtpOrderNum(m_frontID, m_sessionID, orderRef);

                m_logger.WriteInformation(string.Format("{0}.PlaceOrder() ok,[RequestID:{1}][Instrument:{2}][FulcOffsetType{3}][Qty:{4}][Price:{5}].",
                        ToString(), requestID, instrument.InstrumentCode, offsetType, qty, price));

                return orderNum;
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0} placeorder[Instrument:{1}][FulcOffsetType:{2}][Qty:{3}][Price:{4}] failed,Error:{5}.",
                      ToString(), instrument.InstrumentCode, offsetType, qty, price, ex.Message));
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 撤单。
        /// </summary>
        /// <param name="orderNum">委托单号。</param>
        /// <param name="product">委托产品。</param>
        /// <param name="error">[out]撤单失败错误信息。</param>
        /// <returns>撤单是否成功。</returns>
        public override bool CancelOrder(USeOrderNum orderNum, USeInstrument product, out string error)
        {
            CtpOrderNum ctpOrderNum = orderNum as CtpOrderNum;
            if (ctpOrderNum == null)
            {
                throw new ArgumentException("Invalid orderNum type", "orderNum");
            }

            error = string.Empty;

            int requestID = m_requetSeqIDCreator.Next();
            try
            {
                InputOrderActionField requestField = new InputOrderActionField();
                requestField.ActionFlag = ActionFlagType.Delete;
                requestField.BrokerID = m_brokerID;
                requestField.InvestorID = m_investorID;
                requestField.InstrumentID = product.InstrumentCode;
                requestField.FrontID = ctpOrderNum.FrontID;
                requestField.SessionID = ctpOrderNum.SessionID;
                requestField.OrderRef = ctpOrderNum.OrderRef;
                requestField.ExchangeID = ctpOrderNum.ExchangeID;
                requestField.OrderSysID = ctpOrderNum.OrderSysID;

                m_ctpUser.ReqOrderAction(ref requestField, requestID);

                return true;
            }
            catch (Exception ex)
            {
                m_logger.WriteError(string.Format("{0} cancelorder[OrderNum:{1}] failed,Error:{2}.",
                     ToString(), orderNum, ex.Message));
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改密码。
        /// </summary>
        /// <param name="oldPasswrod">旧密码。</param>
        /// <param name="newPassword">新密码。</param>
        /// <returns>修改密码是否成功。</returns>
        public override bool ModifyPassword(string oldPasswrod, string newPassword, out string error)
        {
            error = "不支持此功能";
            return false;
        }
    }
}
