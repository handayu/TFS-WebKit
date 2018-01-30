using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using USe.Common;
using System.Threading;
using System.Diagnostics;

namespace USe.TradeDriver.Test
{
    public delegate void ClientCancelOrderNoticeEventHandle(TestOrderNum orderNum);

    public partial class USeTestOrderDriver : USeOrderDriver
    {
        public event ClientCancelOrderNoticeEventHandle OnClientCancelOrder;

        private CommonIdCreator m_orderNumCreateor = new CommonIdCreator();
        private CommonIdCreator m_tradeNumCreateor = new CommonIdCreator();
        private DataBuffer m_dataBuffer = new DataBuffer();

        private Queue<USeOrderBook> m_pushOrderBookList = new Queue<USeOrderBook>();
        private Queue<USeTradeBook> m_pushTradeBookList = new Queue<USeTradeBook>();
        private Queue<USePosition> m_pushPositionList = new Queue<USePosition>();

        private Queue<TestOrderNum> m_cancelOrderQueue = new Queue<TestOrderNum>();

        private bool m_runFlag = false;
        /// <summary>
        /// Push通知线程
        /// </summary>
        private Thread m_thread_event = null;

        public USeTestOrderDriver()
        {
            //启动推送查询线程

        }


        //查询队列有任务发布事件
        public void execEventList()
        {
            while (m_runFlag)
            {
                Thread.Sleep(500);
                if (m_pushTradeBookList.Count > 0)
                {
                    USeTradeBook tradeBook = m_pushTradeBookList.Dequeue();
                    if (tradeBook != null)
                    {
                        try
                        {
                            base.FireTradeBookChanged(tradeBook, true);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.Assert(false);
                        }
                    }
                }
                if (m_pushOrderBookList.Count > 0)
                {
                    USeOrderBook order_book = m_pushOrderBookList.Dequeue();
                    if (order_book != null)
                    {
                        try
                        {
                            base.FireOrderBookChanged(order_book);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.Assert(false);
                        }
                    }
                }
                if (m_pushPositionList.Count > 0)
                {
                    USePosition position_book = m_pushPositionList.Dequeue();
                    if (position_book != null)
                    {
                        try
                        {
                            base.FirePositionChanged(position_book);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.Assert(false);
                        }
                    }
                }

            }
        }

        #region property
        /// <summary>
        /// 驱动名称。
        /// </summary>
        public override string DriverName
        {
            get { return "Test"; }
        }
        #endregion // property

        /// <summary>
        /// 连接交易服务器。
        /// </summary>
        public override void ConnectServer()
        {

            FireOrderDriverStateChanged(USeOrderDriverState.Connecting, "");
            Thread.Sleep(2000);

            FireOrderDriverStateChanged(USeOrderDriverState.Connected, "");
        }

        /// <summary>
        /// 交易期货公司认证
        /// </summary>
        /// <param name="brokerID"></param>
        /// <param name="userId"></param>
        /// <param name="authCode"></param>
        /// <param name="userProductInfo"></param>
        public override void AuthorT(string brokerID, string userId, string authCode, string userProductInfo)
        {
            FireOrderDriverStateChanged(USeOrderDriverState.AuthorSuccessOn, "");
        }

        /// <summary>
        /// 断开交易服务器。
        /// </summary>
        public override void DisConnectServer()
        {
            FireOrderDriverStateChanged(USeOrderDriverState.DisConnected, "");
        }

        /// <summary>
        /// 登录交易服务器。
        /// </summary>
        /// <param name="brokerId">经纪商ID。</param>
        /// <param name="account">帐号。</param>
        /// <param name="password">密码。</param>
        public override void Login(string brokerId, string account, string password)
        {
            FireOrderDriverStateChanged(USeOrderDriverState.LoggingOn, "");

            FireOrderDriverStateChanged(USeOrderDriverState.LoggingOn, "");
            //Thread.Sleep(2000);

            FireOrderDriverStateChanged(USeOrderDriverState.Ready, "");

            m_brokerID = brokerId;
            m_investorID = account;
            m_password = password;
            m_needSettlementConfirm = false;
            m_runFlag = true;

            
            this.m_thread_event = new Thread(new ThreadStart(execEventList));
            this.m_thread_event.Start();

            //List<USePosition> position_list = m_dataBuffer.GetPositionList();

        }

        /// <summary>
        /// 登出交易服务器。
        /// </summary>
        public override void Logout()
        {
            m_runFlag = false;

            Thread.Sleep(2000);
            if (m_thread_event.IsAlive)
            {
                System.Diagnostics.Debug.Assert(false);
            }
            //if (m_thread_event != null)
            //{
            //    m_thread_event.Abort();
            //}

            FireOrderDriverStateChanged(USeOrderDriverState.DisConnected, "");
        }

        /// <summary>
        /// 结算单确认。
        /// </summary>
        public override void SettlementInfoConfirm()
        {

        }

        /// <summary>
        /// 获取确认结算单。
        /// </summary>
        /// <returns>确认结算单信息。</returns>
        public override string GetSettlementInfo(string tradingDate)
        {
            return string.Empty;

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
            List<USeProductDetail> list = new List<USeProductDetail>();
            USeProductDetail cuProduct = new USeProductDetail()
            {
                ProductCode = "cu",
                ShortName = "铜",
                LongName = "铜",
                Market = USeMarket.SHFE,
                ProductClass = USeProductClass.Futures,
                VolumeMultiple = 5,
                PriceTick = 10,
            };
            list.Add(cuProduct);

            USeProductDetail alProduct = new USeProductDetail()
            {
                ProductCode = "al",
                ShortName = "铝",
                LongName = "铝",
                Market = USeMarket.SHFE,
                ProductClass = USeProductClass.Futures,
                VolumeMultiple = 5,
                PriceTick = 5,
            };
            list.Add(alProduct);

            return list;
        }

        /// <summary>
        /// 查询产品信息。
        /// </summary>
        /// <param name="productCode">产品代码。</param>
        /// <returns>产品信息。</returns>
        public override USeProductDetail QueryProductDetail(string productCode)
        {
            List<USeProductDetail> list = QueryProductDetails();
            foreach(USeProductDetail detail in list)
            {
                if(detail.ProductCode == productCode)
                {
                    return detail;
                }
            }

            return null;
        }

        /// <summary>
        /// 查询所有产品信息。
        /// </summary>
        /// <returns>产品信息。</returns>
        public override List<USeProduct> QueryProducts()
        {
            List<USeProductDetail> list = QueryProductDetails();

            List<USeProduct> result = (from p in list
                                       select new USeProduct()
                                       {
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
            List<USeProductDetail> list = QueryProductDetails();
            foreach (USeProductDetail detail in list)
            {
                if (detail.ProductCode == productCode)
                {
                    return new USeProduct()
                    {
                        ProductCode = detail.ProductCode,
                        ShortName = detail.ShortName,
                        LongName = detail.LongName,
                        Market = detail.Market,
                        VolumeMultiple = detail.VolumeMultiple,
                        PriceTick = detail.PriceTick
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// 查询合约的合约乘数。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <returns>合约乘数。</returns>
        public override int QueryInstrumentVolumeMultiple(USeInstrument instrument)
        {
            USeInstrumentDetail detail = QueryInstrumentDetail(instrument);
            if (detail != null)
            {
                return detail.VolumeMultiple;
            }
            else
            {
                return 0;
            }
            //return m_dataBuffer.GetVolumeMultiple(instrument);
        }

        /// <summary>
        /// 查询合约手续费。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <returns></returns>
        public override USeFee QueryInstrumentFee(USeInstrument instrument)
        {
            USeFee fee = new USeFee()
            {
                Instrument = instrument.Clone(),
                OpenRatioByMoney = 0.00005m,
                OpenRatioByVolume = 0m,
                CloseRatioByMoney = 0.00005m,
                CloseRatioByVolume = 0m,
                CloseTodayRatioByMoney = 0.00005m,
                CloseTodayRatioByVolume = 0m,
                StrikeRatioByMoney = 0.00005m,
                StrikeRatioByVolume = 0m
            };
            return fee;
        }

        /// <summary>
        /// 查询合约保证金。
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        public override USeMargin QueryInstrumentMargin(USeInstrument instrument)
        {
            USeMargin margin = new USeMargin()
            {
                Instrument = instrument.Clone(),
                ExchangeLongMarginRatio = 0.09m,
                ExchangeShortMarginRatio = 0.09m,
                BrokerLongMarginRatioByMoney = 0.09m,
                BrokerLongMarginRatioByVolume = 0,
                BrokerShortMarginRatioByMoney = 0.09m,
                BrokerShortMarginRatioByVolume = 0
            };
            return margin;
        }

        /// <summary>
        /// 查询当日所有委托回报。
        /// </summary>
        /// <returns></returns>
        public override List<USeOrderBook> QueryOrderBooks()
        {
            List<USeOrderBook> list_order_book = new List<USeOrderBook>();
            if (m_dataBuffer.OrderBookList == null) return list_order_book;
            return m_dataBuffer.OrderBookList;
        }

        /// <summary>
        /// 查询指定委托单号的委托回报。
        /// </summary>
        /// <param name="orderNum"></param>
        /// <returns></returns>
        public override USeOrderBook QueryOrderBook(USeOrderNum orderNum)
        {
            USeOrderBook order_book = new USeOrderBook();
            order_book = (from p in m_dataBuffer.OrderBookList
                          where p.OrderNum == orderNum
                          select p).FirstOrDefault();
            if (order_book == null) return null;

            return order_book;
        }

        /// <summary>
        /// 查询当日所有成交回报。
        /// </summary>
        /// <returns></returns>
        public override List<USeTradeBook> QueryTradeBooks()
        {
            List<USeTradeBook> trade_book_list = m_dataBuffer.GetTradeBookList();
            return trade_book_list;
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
            //造假的
            List<USePosition> list_position = new List<USePosition>();
            if (m_dataBuffer.PositionDataList == null) return list_position;
            return m_dataBuffer.PositionDataList;
        }

        public List<USePosition> GetDefaultPositionList()
        {
            return m_dataBuffer.CreateDefaultPositonList();
        }

        public void ClearPositionList()
        {
            if(m_dataBuffer.PositionDataList != null)
            {
                m_dataBuffer.PositionDataList.Clear();
            }
        }

        public void AddPositionList(USePosition position)
        {
            if (m_dataBuffer.PositionDataList != null)
            {
                m_dataBuffer.PositionDataList.Add(position);
            }
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
            //return m_dataBuffer.GetPosition(instrument, direction);

            return null;
        }

        /// <summary>
        /// 查询持仓明细。
        /// </summary>
        /// <returns></returns>
        public override List<USePositionDetail> QueryPositionDetail()
        {
            //List<USePositionDetail> list = m_dataBuffer.GetAllPositionDetail();
            //return list;

            return null;
        }

        /// <summary>
        /// 查询账户信息。
        /// </summary>
        /// <returns></returns>
        public override USeFund QueryFundInfo()
        {
            USeFund fund = new USeFund();
            fund.AccountID = m_investorID;
            fund.Deposit = 100000000;
            fund.Mortgage = 100000000;
            fund.PreBalance = 10000000000;
            fund.PreCredit = 33;
            fund.PreMortgage = 77777;
            fund.WithDraw = 777;
            return fund;
        }

        /// <summary>
        /// 查询帐户详细资金信息。
        /// </summary>
        /// <returns></returns>
        public override USeFundDetail QueryFundDetailInfo()
        {
            USeFundDetail fundDetail = new USeFundDetail();
            fundDetail.AccountID = m_investorID;
            fundDetail.Available = 10000000000;
            fundDetail.Deposit = 100000000;
            fundDetail.Mortgage = 100000000;
            fundDetail.PreBalance = 3234;
            fundDetail.PreCredit = 33;
            fundDetail.PreMortgage = 77777;
            fundDetail.WithDraw = 777;
            fundDetail.StaticBenefit = 8876645;
            fundDetail.CloseProfit = 4455;
            fundDetail.TradeFee = 445;
            fundDetail.HoldProfit = 77766;
            fundDetail.HoldMargin = 5555;
            fundDetail.DynamicBenefit = 10000000000;
            fundDetail.FrozonMargin = 0;
            fundDetail.FrozonFee = 0;
            fundDetail.Fronzon = 0;
            fundDetail.Risk = 0.098m;
            fundDetail.PreferCash = 33;
            return fundDetail;

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
            error = "";
            System.Diagnostics.Debug.Assert(instrument != null);

            int orderNumValue = m_orderNumCreateor.Next();

            USeOrderBook orderBook = new USeOrderBook();
            orderBook.OrderNum = new TestOrderNum(orderNumValue);
            orderBook.Account = m_investorID;
            orderBook.Instrument = instrument;
            orderBook.OrderQty = qty;
            orderBook.OrderPrice = price;
            orderBook.TradeQty = 0;
            orderBook.TradeAmount = 0;
            orderBook.TradePrice = 0;
            orderBook.TradeFee = 0;
            orderBook.OrderStatus = USeOrderStatus.Unknown;
            orderBook.CancelQty = 0;
            orderBook.OrderSide = orderSide;
            orderBook.OffsetType = offsetType;
            orderBook.Memo = "";
            orderBook.OrderTime = DateTime.Now;

            string error_orderInfo = string.Empty;
            VerfiyPlaceOrderToReturn(orderBook, out error_orderInfo);

            if (error_orderInfo == "开仓")
            {
                m_dataBuffer.OrderBookList.Add(orderBook);

                m_pushOrderBookList.Enqueue(orderBook);
                error = "Place Order Ok";

                return orderBook.OrderNum;
            }
            else if (error_orderInfo == "CloseSuccessed")
            {
                //开仓
                orderBook.Memo = "平仓委托成功";
                m_dataBuffer.OrderBookList.Add(orderBook);

                m_pushOrderBookList.Enqueue(orderBook);
                error = "Place Order Ok";
                return orderBook.OrderNum;
            }
            else
            {
                //平仓委托失败
                orderBook.Memo = error_orderInfo;
                orderBook.OrderStatus = USeOrderStatus.BlankOrder;

                m_pushOrderBookList.Enqueue(orderBook);
                error = "Place Order Failed";
                return orderBook.OrderNum;
            }

        }

        private bool CheckOrderDirection(USeDirection direction, USeOrderSide orderSide)
        {
            if (direction == USeDirection.Long && orderSide == USeOrderSide.Buy)
            {
                return true;
            }
            else if (direction == USeDirection.Short && orderSide == USeOrderSide.Sell)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查委托单委托类型和数量
        /// </summary>
        /// <param name="orderBook"></param>
        public void VerfiyPlaceOrderToReturn(USeOrderBook orderBook, out string error)
        {
            error = "";
            Debug.Assert(orderBook != null);

            error = "开仓";
            if (orderBook.OffsetType == USeOffsetType.Open) return;
            List<USePosition> positionHoldNowList = m_dataBuffer.PositionDataList;
            if (positionHoldNowList == null) return;

            foreach (USePosition p in positionHoldNowList)
            {
                //同一合约名，持仓和目前的委托方向不一致开始检查
                if (p.Instrument != orderBook.Instrument) continue;
                if (CheckOrderDirection(p.Direction, orderBook.OrderSide)) continue;
                if (orderBook.OrderQty > p.TotalPosition && orderBook.OffsetType == USeOffsetType.Close)
                {
                    error = "平仓仓位不足";
                    return;
                }
                if (orderBook.OrderQty > p.NewPosition && orderBook.OffsetType == USeOffsetType.CloseToday)
                {
                    error = "平今仓位不足";
                    return;
                }
                if (orderBook.OrderQty > p.OldPosition && orderBook.OffsetType == USeOffsetType.CloseHistory)
                {
                    error = "平昨仓位不足";
                    return;
                }
            }
            error = "CloseSuccessed";
            return;
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
            error = string.Empty;
            //m_cancelOrderQueue.Enqueue(orderNum as TestOrderNum);

            if (this.OnClientCancelOrder != null)
            {
                this.OnClientCancelOrder(orderNum as TestOrderNum);
            }
            return true;
            ////Orderlist中找到OrderNum对应的单子删除
            //error = "Error_info";
            //if (orderNum == null) return false;
            //if (m_dataBuffer.OrderBookList.Count <= 0) return false;
            //USeOrderBook order_book = (from p in m_dataBuffer.OrderBookList
            //                           where p.OrderNum.Equals(orderNum)
            //                           select p).FirstOrDefault();
            //if (order_book == null) return false;

            //if (order_book.TradeQty == order_book.OrderQty)
            //{
            //    return false;
            //}

            //if (order_book.TradeQty == 0)
            //{
            //    order_book.OrderStatus = USeOrderStatus.AllCanceled;
            //    order_book.CancelQty = order_book.OrderQty;
            //}
            //else
            //{
            //    System.Diagnostics.Debug.Assert(order_book.TradeQty > 0);
            //    order_book.OrderStatus = USeOrderStatus.PartCanceled;
            //    order_book.CancelQty = order_book.OrderQty - order_book.TradeQty;
            //}

            //m_pushOrderBookList.Enqueue(order_book.Clone());

            //return true;
            //CtpOrderNum ctpOrderNum = orderNum as CtpOrderNum;
            //if (ctpOrderNum == null)
            //{
            //    throw new ArgumentException("Invalid orderNum type", "orderNum");
            //}

            //error = string.Empty;

            //int requestID = m_requetSeqIDCreator.Next();
            //try
            //{
            //    InputOrderActionField requestField = new InputOrderActionField();
            //    requestField.ActionFlag = ActionFlagType.Delete;
            //    requestField.BrokerID = m_brokerID;
            //    requestField.InvestorID = m_investorID;
            //    requestField.InstrumentID = product.InstrumentCode;
            //    requestField.FrontID = ctpOrderNum.FrontID;
            //    requestField.SessionID = ctpOrderNum.SessionID;
            //    requestField.OrderRef = ctpOrderNum.OrderRef;
            //    requestField.ExchangeID = ctpOrderNum.ExchangeID;
            //    requestField.OrderSysID = ctpOrderNum.OrderSysID;

            //    m_ctpUser.ReqOrderAction(ref requestField, requestID);

            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    m_logger.WriteError(string.Format("{0} cancelorder[OrderNum:{1}] failed,Error:{2}.",
            //         ToString(), orderNum, ex.Message));
            //    error = ex.Message;
            //    return false;
            //}
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

        public void AllTrade(USeOrderNum orderNum)
        {
            AllTrade(orderNum, 0);
        }

        public void AllTrade(USeOrderNum orderNum, int slipPoint)
        {
            USeOrderBook orderBook = (from p in m_dataBuffer.OrderBookList
                                      where p.OrderNum.Equals(orderNum)
                                      select p).FirstOrDefault();
            if (orderBook == null) return;

            USeInstrumentDetail instrumentDetail = QueryInstrumentDetail(orderBook.Instrument);

            int tradeQty = (orderBook.OrderQty - orderBook.TradeQty);
            decimal tradePrice = 0;
            if (orderBook.OrderSide == USeOrderSide.Buy)
            {
                tradePrice = orderBook.OrderPrice - instrumentDetail.PriceTick * slipPoint;
            }
            else if (orderBook.OrderSide == USeOrderSide.Sell)
            {
                tradePrice = orderBook.OrderPrice + instrumentDetail.PriceTick * slipPoint;
            }
            int volumeMultiple = instrumentDetail.VolumeMultiple;
            int orderBookTradeQty = orderBook.TradeQty + tradeQty;

            orderBook.TradeQty = orderBookTradeQty;
            orderBook.TradeAmount = tradePrice * orderBookTradeQty * volumeMultiple;
            orderBook.TradePrice = tradePrice;
            orderBook.TradeFee = 5 * orderBookTradeQty;
            orderBook.CancelQty = 0;
            orderBook.Memo = orderBook.Memo;
            orderBook.OrderTime = DateTime.Now;
            orderBook.OrderStatus = USeOrderStatus.AllTraded;

            USeTradeBook tradeBook = CreateTradeBook(orderBook, tradeQty, tradePrice, tradeQty * tradePrice * volumeMultiple, 5 * tradeQty);

            USePosition positionBook = CreatePositionBook(tradeBook);

            m_pushTradeBookList.Enqueue(tradeBook);

            m_pushOrderBookList.Enqueue(orderBook.Clone());
            //推送持仓信息
            m_pushPositionList.Enqueue(positionBook.Clone());

        }
        public void PartTrade(USeOrderNum orderNum)
        {
            PartTrade(orderNum, 0);
        }
        public void PartTrade(USeOrderNum orderNum, int slipPoint)
        {
            USeOrderBook orderBook = (from p in m_dataBuffer.OrderBookList
                                      where p.OrderNum.Equals(orderNum)
                                      select p).FirstOrDefault();
            if (orderBook == null) return;

            USeInstrumentDetail instrumentDetail = QueryInstrumentDetail(orderBook.Instrument);

            int tradeQty = (int)Math.Ceiling((orderBook.OrderQty - orderBook.TradeQty) / 2.0d);
            decimal tradePrice = 0;
            if (orderBook.OrderSide == USeOrderSide.Buy)
            {
                tradePrice = orderBook.OrderPrice - instrumentDetail.PriceTick * slipPoint;
            }
            else if (orderBook.OrderSide == USeOrderSide.Sell)
            {
                tradePrice = orderBook.OrderPrice + instrumentDetail.PriceTick * slipPoint;
            }

            int volumeMultiple = QueryInstrumentVolumeMultiple(orderBook.Instrument);
            int orderBookTradeQty = orderBook.TradeQty + tradeQty;

            orderBook.TradeQty = orderBookTradeQty;
            orderBook.TradeAmount = tradePrice * orderBookTradeQty * volumeMultiple;
            orderBook.TradePrice = tradePrice;
            orderBook.TradeFee = 5 * orderBookTradeQty;
            orderBook.CancelQty = 0;
            orderBook.Memo = orderBook.Memo;
            orderBook.OrderTime = DateTime.Now;
            orderBook.OrderStatus = orderBook.OrderQty == orderBook.TradeQty ? USeOrderStatus.AllTraded : USeOrderStatus.PartTraded;

            USeTradeBook tradeBook = CreateTradeBook(orderBook, tradeQty, tradePrice, tradeQty * tradePrice * volumeMultiple, 5 * tradeQty);
            USePosition positionBook = CreatePositionBook(tradeBook);

            m_pushPositionList.Enqueue(positionBook.Clone());
            m_pushTradeBookList.Enqueue(tradeBook);
            m_pushOrderBookList.Enqueue(orderBook.Clone());
        }
        private USeTradeBook CreateTradeBook(USeOrderBook orderBook, int tradeQty, decimal tradePrice, decimal amount, decimal fee)
        {
            USeTradeBook tradeBook = new USeTradeBook();
            tradeBook.TradeNum = m_tradeNumCreateor.Next().ToString();
            tradeBook.Instrument = orderBook.Instrument.Clone();
            tradeBook.OrderNum = orderBook.OrderNum;
            tradeBook.OrderSide = orderBook.OrderSide;
            tradeBook.OffsetType = orderBook.OffsetType;
            tradeBook.Price = tradePrice;
            tradeBook.Qty = tradeQty;
            tradeBook.Amount = amount;
            tradeBook.Fee = fee;
            tradeBook.TradeTime = DateTime.Now;
            tradeBook.Account = this.Account;

            return tradeBook;
        }

        private USePosition CreatePositionBook(USeTradeBook traderBook)
        {
            System.Diagnostics.Debug.Assert(traderBook != null);
             
            USeDirection direction ;
            if(traderBook.OffsetType == USeOffsetType.Open)
            {
                direction = traderBook.OrderSide == USeOrderSide.Buy? USeDirection.Long:USeDirection.Short;
            }
            else 
            {
                direction = traderBook.OrderSide == USeOrderSide.Buy? USeDirection.Short:USeDirection.Long;
            }

            USePosition position = null;

            //维护持仓列表-从持仓列表中要移除总持仓为0的Item
            for(int i = 0;i < m_dataBuffer.PositionDataList.Count;i++)
            {
                if (m_dataBuffer.PositionDataList[i].TotalPosition == 0)
                {
                    m_dataBuffer.PositionDataList.RemoveAt(i);
                }
            }

            position = (from p in m_dataBuffer.PositionDataList
                     where (p.Instrument == traderBook.Instrument && p.Direction == direction)
                     select p).FirstOrDefault();

            if (position == null)
            {
                Debug.Assert(traderBook.OffsetType == USeOffsetType.Open);
                position = new USePosition()
                {
                    Instrument = traderBook.Instrument,
                    Direction = direction,
                    NewPosition = traderBook.Qty,
                    Amount = traderBook.Amount,
                    AvgPirce = traderBook.Price
                };

                m_dataBuffer.PositionDataList.Add(position);
            }
            else
            {
                switch (traderBook.OffsetType)
                {
                    case USeOffsetType.Open:
                        position.NewPosition = position.NewPosition + traderBook.Qty;
                        break;
                    case USeOffsetType.CloseToday:
                        position.NewPosition = position.NewPosition - traderBook.Qty;
                        break;
                    case USeOffsetType.CloseHistory:
                        position.OldPosition = position.OldPosition - traderBook.Qty;
                        break;
                    case USeOffsetType.Close:
                        int oldCloseQty = Math.Min(traderBook.Qty, position.OldPosition);
                        position.OldPosition = position.OldPosition - oldCloseQty;
                        int newCloseQty = traderBook.Qty - oldCloseQty;
                        position.NewPosition = position.NewPosition - newCloseQty;
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            return position;
        }

        public void CanceledOrder(USeOrderNum orderNum)
        {
            USeOrderBook order_book = (from p in m_dataBuffer.OrderBookList
                                       where p.OrderNum.Equals(orderNum)
                                       select p).FirstOrDefault();
            if (order_book == null) return;

            string error = string.Empty;
            CancelOrder(order_book.OrderNum, null, out error);

        }

        public bool CanceledOrderActionReturn(USeOrderNum orderNum, bool isCancelSucceed)
        {
            if (isCancelSucceed == false)
            {
                return false;
            }

            string error = "errorInfo";
            //Orderlist中找到OrderNum对应的单子删除
            if (orderNum == null) return false;
            if (m_dataBuffer.OrderBookList.Count <= 0) return false;
            USeOrderBook order_book = (from p in m_dataBuffer.OrderBookList
                                       where p.OrderNum.Equals(orderNum)
                                       select p).FirstOrDefault();
            if (order_book == null) return false;

            if (order_book.TradeQty == order_book.OrderQty)
            {
                return false;
            }

            if (order_book.TradeQty == 0)
            {
                order_book.OrderStatus = USeOrderStatus.AllCanceled;
                order_book.CancelQty = order_book.OrderQty;
            }
            else
            {
                System.Diagnostics.Debug.Assert(order_book.TradeQty > 0);
                order_book.OrderStatus = USeOrderStatus.PartCanceled;
                order_book.CancelQty = order_book.OrderQty - order_book.TradeQty;
            }

            m_pushOrderBookList.Enqueue(order_book.Clone());

            return true;
        }

        /// <summary>
        /// 将修改的初始持仓数据推送到客户端
        /// </summary>
        public void ReloadInitPositionData(USePosition position)
        {
            try
            {
                base.FirePositionChanged(position);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Assert(false);
            }
        }


        public void ManualSetOrderState(USeOrderDriverState newState)
        {
            FireOrderDriverStateChanged(newState, "测试");
        }

    }
}
