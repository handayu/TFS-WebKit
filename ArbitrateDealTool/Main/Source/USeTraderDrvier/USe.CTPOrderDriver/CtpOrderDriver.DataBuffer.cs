#region Copyright & Version
//==============================================================================
// 文件名称: CtpOrderDriver.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: CTP交易驱动类--数据缓存。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using USe.TradeDriver.Common;
using USe.Common;
using CTPAPI;

namespace USe.TradeDriver.Ctp
{
    public partial class CtpOrderDriver
    {
        /// <summary>
        /// 数据缓冲区域。
        /// </summary>
        private partial class CtpDataBuff
        {
            #region member
            private CtpOrderDriver m_driver = null;

            private Dictionary<string, USeInstrument> m_instrumentCodeDic = null;     // 合约代码信息
            private Dictionary<USeInstrument, USeInstrumentDetail> m_instrumentDic = null; // 合约详细信息
            private Dictionary<string, USeProductDetail> m_productDetailDic = null; // 品种详细信息
            //private Dictionary<string, USeFee> m_varietiesFeeDic = null; // 合约手续费信息<合约种类代码,手续费信息>
            private Dictionary<USeInstrument, USeFee> m_feeDic = null; // 合约手续费信息<合约信息,手续费信息>
            private Dictionary<USeInstrument, USeMargin> m_marginDic = null; // 合约保证金信息<合约信息,保证金信息>
            private List<USeOrderBook> m_orderBookList = null;   // 委托回报列表
            private List<USeTradeBook> m_tradeBookList = null;   // 成交回报列表
            private List<USePosition> m_positionList = null;     // 持仓列表
            private List<USePositionDetail> m_positionDetailList = null; // 持仓明细列表(以建仓时间正序排列)
            private USeFund m_fund = null;          // 账户信息
            private USeInvestorBaseInfo m_investorBaseInfo = null;  // 帐户基本信息
            private List<FrozonOrder> m_frozonOrderList = null; // 平仓委托单冻结仓位列表

            private List<CtpQueryInfo> m_queryList = null;     // 待查询信息

            private CommonIdCreator m_posDetailIdcreator = null;  // 持仓明细ID生成。

            private object m_object = new object(); // 加锁对象
            #endregion // member

            #region constuction
            public CtpDataBuff(CtpOrderDriver driver)
            {
                m_driver = driver;
                m_posDetailIdcreator = new CommonIdCreator();

                m_instrumentCodeDic = new Dictionary<string, USeInstrument>(256);
                m_instrumentDic = new Dictionary<USeInstrument, USeInstrumentDetail>(256, new USeInstrumentComparer());
                m_productDetailDic = new Dictionary<string, USeProductDetail>(64);
                //m_varietiesFeeDic = new Dictionary<string, USeFee>(64);
                m_feeDic = new Dictionary<USeInstrument, USeFee>(256, new USeInstrumentComparer());
                m_marginDic = new Dictionary<USeInstrument, USeMargin>(256, new USeInstrumentComparer());
                m_orderBookList = new List<USeOrderBook>(256);
                m_tradeBookList = new List<USeTradeBook>(256);
                m_positionList = new List<USePosition>(64);
                m_positionDetailList = new List<USePositionDetail>(128);
                m_fund = new USeFund();
                m_frozonOrderList = new List<FrozonOrder>();

                m_queryList = new List<CtpQueryInfo>();
            }
            #endregion // construction

            #region property
            /// <summary>
            /// 最后一笔成交时间。
            /// </summary>
            private DateTime LastTradeTime
            {
                get
                {
                    lock (m_object)
                    {
                        if (m_tradeBookList == null || m_tradeBookList.Count <= 0)
                        {
                            return DateTime.Today.AddDays(-1);
                        }
                        else
                        {
                            return m_tradeBookList[m_tradeBookList.Count - 1].TradeTime;
                        }
                    }
                }
            }
            #endregion // property

            #region methods
            /// <summary>
            /// 初始化产品基本信息。
            /// </summary>
            /// <param name="instruments"></param>
            public void InitializeInstrumentInfo(List<InstrumentField> instruments)
            {
                if (instruments == null || instruments.Count <= 0)
                {
                    throw new ArgumentNullException("instruments");
                }

                lock (m_object)
                {
                    foreach (InstrumentField field in instruments)
                    {
                        if (field.ProductClass == ProductClass.Futures) // 只要期货,组合合约不要
                        {
                            USeInstrumentDetail instrumentDetail = m_driver.CtpInstrumentFieldToUSeInstrumentDetail(field);
                            USeInstrument instrument = instrumentDetail.Instrument;

                            m_instrumentDic.Add(instrument, instrumentDetail);
                            m_instrumentCodeDic.Add(instrument.InstrumentCode, instrument);

                            m_queryList.Add(new CtpQueryFeeInfo(instrument.InstrumentCode));
                            m_queryList.Add(new CtpQueryMarginInfo(instrument.InstrumentCode));
                        }
                    }

                    List<USeInstrumentDetail> instrumentList = m_instrumentDic.Values.ToList();
                    List<USeProductDetail> productList = (from i in instrumentList
                                                          group i by i.Varieties into g
                                                          select new USeProductDetail() {
                                                              ProductCode = g.Key,
                                                              Market = g.First().Instrument.Market,
                                                              ShortName = GetVarietiesName(g.First().Instrument.InstrumentName),
                                                              LongName = GetVarietiesName(g.First().Instrument.InstrumentName),
                                                              ProductClass = g.First().ProductClass,
                                                              VolumeMultiple = g.First().VolumeMultiple,
                                                              PriceTick = g.First().PriceTick,
                                                              MaxMarketOrderVolume = g.First().MaxMarketOrderVolume,
                                                              MinMarketOrderVolume = g.First().MinMarketOrderVolume,
                                                              MaxLimitOrderVolume = g.First().MaxLimitOrderVolume,
                                                              MinLimitOrderVolume = g.First().MinLimitOrderVolume,
                                                          }).ToList();

                    foreach (USeProductDetail productDetail in productList)
                    {
                        m_productDetailDic.Add(productDetail.ProductCode, productDetail);
                    }
                }
            }

            /// <summary>
            /// 初始化DataBuffer。
            /// </summary>
            /// <param name="orderFields">委托信息。</param>
            /// <param name="tradeFields">成交信息。</param>
            /// <param name="positonFields">持仓信息。</param>
            /// <param name="tradingAccountField">账户信息。</param>
            public void InitializeData(List<OrderField> orderFields, List<TradeField> tradeFields,
                                       List<InvestorPositionField> positonFields, TradingAccountField tradingAccountField,
                                       InvestorField investorField)
            {
                if (m_instrumentDic == null || m_instrumentDic.Count <= 0)
                {
                    throw new Exception("Must execute InitializeInstrumentInfo() before InitializeData()");
                }

                lock (m_object)
                {
                    //账户信息
                    m_fund = m_driver.CtpTradingAccountFieldToUSeFund(tradingAccountField);
                    m_investorBaseInfo = m_driver.CtpInvestorFieldToUSeInvestorBaseInfo(investorField);
                    //委托回报信息
                    if (orderFields != null && orderFields.Count > 0)
                    {
                        foreach (OrderField field in orderFields)
                        {
                            USeOrderBook orderBook = m_driver.CtpOrderFieldToUSeOrderBook(field);
                            m_orderBookList.Add(orderBook);

                            if(orderBook.IsFinish == false && orderBook.OffsetType != USeOffsetType.Open)
                            {
                                USeDirection direction = orderBook.OrderSide == USeOrderSide.Buy? USeDirection.Short:USeDirection.Long;
                                FrozonOrder frozonOrder = new FrozonOrder(orderBook.OrderNum,orderBook.Instrument.Clone(),direction,orderBook.OffsetType);
                                frozonOrder.CloseQty = orderBook.OrderQty;
                                frozonOrder.OrderStatus = orderBook.OrderStatus;
                                m_frozonOrderList.Add(frozonOrder);
                            }
                        }
                    }

                    //持仓信息
                    if (positonFields != null && positonFields.Count > 0)
                    {
                        //持仓信息中有时昨仓和今仓两条显示
                        foreach (InvestorPositionField field in positonFields)
                        {
                            USePosition position = m_driver.CtpInvestorPositonFieldToUSePosition(field);
                            USePosition oldPosition = null;
                            for (int i = 0; i < m_positionList.Count; i++)
                            {
                                if (position.Instrument == m_positionList[i].Instrument && position.Direction == m_positionList[i].Direction)
                                {
                                    oldPosition = m_positionList[i];
                                    break;
                                }
                            }
                            if (oldPosition == null)
                            {
                                m_positionList.Add(position);
                            }
                            else
                            {
                                oldPosition.NewPosition += position.NewPosition;
                                oldPosition.YesterdayPosition += position.YesterdayPosition;
                                oldPosition.OldPosition += position.OldPosition;
                                oldPosition.OpenQty += position.OpenQty;
                                oldPosition.CloseQty += position.CloseQty;
                            }

                            if (field.YdPosition > 0)
                            {
                                USePositionDetail positionDetail = m_driver.CtpInvestorPositonFieldToUSePositionDetail(field);
                                positionDetail.ID = m_posDetailIdcreator.Next();
                                m_positionDetailList.Add(positionDetail);
                            }
                        }
                    }

                    //成交回报
                    if (tradeFields != null && tradeFields.Count > 0)
                    {
                        tradeFields.Sort(SortTradeFieldByTradeTimeAsc);
                        foreach (TradeField field in tradeFields)
                        {
                            USeTradeBook tradeBook = m_driver.CtpTradeFieldToUSeTradeBook(field);
                            if (m_tradeBookList.Count > 0)
                            {
                                Debug.Assert(tradeBook.TradeTime >= m_tradeBookList[m_tradeBookList.Count - 1].TradeTime);
                            }
                            m_tradeBookList.Add(tradeBook);

                            //此处成交回报不更新持仓主表m_positionList信息，用查询回来的持仓信息来初始化

                            //更新持仓明细表
                            InternalUpdatePositonDetailByTradeBook(tradeBook);
                            //更新冻结委托单
                            InternalUpdateFrozonOrderByTradeBook(tradeBook);

                        }
                    }

                    foreach (USePosition item in m_positionList)
                    {
                        int volumeMultiple = InternalGetVolumeMultiple(item.Instrument);
                        decimal positionAmount = InternalCalculatePositionOpenAmount(item.Instrument, item.Direction, item.TotalPosition);
                        item.Amount = positionAmount;
                        item.AvgPirce = positionAmount.Divide(item.TotalPosition * volumeMultiple);
                    }
                }
            }

            /// <summary>
            /// 清空数据缓存。
            /// </summary>
            public void Clear()
            {
                lock (m_object)
                {
                    m_instrumentCodeDic.Clear();
                    m_instrumentDic.Clear();
                    m_productDetailDic.Clear();
                    //m_varietiesFeeDic.Clear();
                    m_feeDic.Clear();
                    m_marginDic.Clear();
                    m_orderBookList.Clear(); ;
                    m_tradeBookList.Clear();
                    m_positionList.Clear();
                    m_positionDetailList.Clear();
                    m_fund = new USeFund();
                    m_frozonOrderList.Clear();
                    m_queryList.Clear();
                }
            }
            #endregion

            #region 更新数据信息
            /// <summary>
            /// 更新合约手续费。
            /// </summary>
            /// <param name="field">CTP 手续费信息。</param>
            public void UpdateInstrumentFee(InstrumentCommissionRateField field,string instrumentCode)
            {
                USeFee fee = m_driver.CtpInstrumentCommissionRateFieldToUSeFeeInfo(field);
                lock (m_object)
                {
                    USeInstrument instrument = InternalGetInstrumnetByCode(instrumentCode);
                    fee.Instrument = instrument.Clone();

                    m_feeDic[instrument] = fee;  // 添加到手续费列表中

                    for (int i = 0; i < m_queryList.Count; i++)
                    {
                        if (m_queryList[i].QueryType == CtpQueryType.Fee)
                        {
                            if ((m_queryList[i] as CtpQueryFeeInfo).InstrumentCode == instrument.InstrumentCode)
                            {
                                m_queryList.RemoveAt(i); // 从未查询列表中剔除
                                break;
                            }
                        }
                    }

                    foreach (USeTradeBook tradeBook in m_tradeBookList)
                    {
                        if (tradeBook.Instrument.Equals(instrument))
                        {
                            decimal feeValue = InternalCalculateFee(tradeBook.Instrument, tradeBook.OffsetType, tradeBook.Qty, tradeBook.Price);
                            if (feeValue != tradeBook.Fee)
                            {
                                tradeBook.Fee = feeValue;
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// 更新合约保证金。
            /// </summary>
            /// <param name="field">CTP 保证金信息。</param>
            public void UpdateInstrumentMagin(InstrumentMarginRateField field)
            {
                USeMargin margin = m_driver.CtpMarginRateFieldToUSeProductMarginInfo(field);
                lock (m_object)
                {
                    m_marginDic[margin.Instrument] = margin;  // 添加到保证金列表中
                    for (int i = 0; i < m_queryList.Count; i++)
                    {
                        if (m_queryList[i].QueryType == CtpQueryType.Margin)
                        {
                            if ((m_queryList[i] as CtpQueryMarginInfo).InstrumentCode == margin.Instrument.InstrumentCode)
                            {
                                m_queryList.RemoveAt(i); // 从未查询列表中剔除
                                break;
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// 更新账户信息。
            /// </summary>
            /// <param name="field">CTP 账户资金信息。</param>
            public void UpdateTradingAccountInfo(TradingAccountField field)
            {
                USeFund fundInfo = m_driver.CtpTradingAccountFieldToUSeFund(field);
                lock (m_object)
                {
                    m_fund.AccountID = fundInfo.AccountID;
                    m_fund.PreMortgage = fundInfo.PreMortgage;
                    m_fund.PreCredit = fundInfo.PreCredit;
                    m_fund.PreBalance = fundInfo.PreBalance;
                    m_fund.Deposit = fundInfo.Deposit;
                    m_fund.WithDraw = fundInfo.WithDraw;
                    m_fund.Mortgage = fundInfo.Mortgage;
                    m_fund.DeliveryMargin = fundInfo.DeliveryMargin;

                    for (int i = 0; i < m_queryList.Count; i++)
                    {
                        if (m_queryList[i].QueryType == CtpQueryType.Fund)
                        {
                            m_queryList.RemoveAt(i); // 从未查询列表中剔除
                        }
                    }
                }
            }

            /// <summary>
            /// 更新委托回报。
            /// </summary>
            /// <param name="orderField">CTP委托回报。</param>
            /// <returns>委托单号。</returns>
            public USeOrderNum UpdateOrderField(OrderField orderField)
            {
                USeOrderBook orderBook = m_driver.CtpOrderFieldToUSeOrderBook(orderField);
                lock (m_object)
                {
                    CtpOrderNum orderNum = orderBook.OrderNum as CtpOrderNum;
                    int index = InternalGetOrderBookIndex(orderNum);
                    if (index >=0)
                    {
                        Debug.Assert(m_orderBookList[index].OrderNum.Equals(orderBook.OrderNum), "UpdateOrderField(),different orderNum.");
                        m_orderBookList[index] = orderBook;  // 更新委托回报
                        for(int i= m_frozonOrderList.Count-1;i>=0;i--)
                        {
                            if (m_frozonOrderList[i].OrderNum.Equals(orderBook.OrderNum))
                            {
                                if (orderBook.IsFinish)
                                {
                                    m_frozonOrderList.RemoveAt(i);
                                }
                                else
                                {
                                    m_frozonOrderList[i].OrderNum = orderBook.OrderNum; // 重新设置委托单号，可能委托单好中包含了交易所编码
                                    m_frozonOrderList[i].OrderStatus = orderBook.OrderStatus;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        m_orderBookList.Add(orderBook);      // 新增委托回报
                        if (orderBook.OffsetType != USeOffsetType.Open && orderBook.IsFinish == false)
                        {
                            USeDirection direction = orderBook.OrderSide == USeOrderSide.Buy ? USeDirection.Short : USeDirection.Long;
                            FrozonOrder frozonOrder = new FrozonOrder(orderBook.OrderNum, orderBook.Instrument.Clone(), direction, orderBook.OffsetType);
                            frozonOrder.CloseQty = orderBook.OrderQty;
                            frozonOrder.OrderStatus = orderBook.OrderStatus;
                            m_frozonOrderList.Add(frozonOrder);
                        }
                    }
                    return orderBook.OrderNum;
                }
            }

            /// <summary>
            /// 委托回报作废(下单失败等原因)。
            /// </summary>
            /// <param name="orderNum">委托单号。</param>
            /// <param name="reason">作废原因。</param>
            public void BlankOrder(USeOrderNum orderNum, string reason)
            {
                lock (m_object)
                {
                    int index = InternalGetOrderBookIndex(orderNum);
                    if (index < 0)
                    {
                        //如果上次委托失败，重新登录会收到OnErrRtnOrderInsert,但实际找不到委托单
                        //Debug.Assert(false, string.Format("BlankOrder() not found ordernum[{0}].", orderNum.OrderString));
                        //未查询到委托单
                        return;
                    }

                    USeOrderBook orderBook = m_orderBookList[index];
                    orderBook.OrderStatus = USeOrderStatus.BlankOrder;
                    orderBook.Memo = reason;

                    for (int i = m_frozonOrderList.Count - 1; i >= 0; i--)
                    {
                        if (m_frozonOrderList[i].OrderNum.Equals(orderNum))
                        {
                            m_frozonOrderList.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            /// <summary>
            /// 委托单撤单失败。
            /// </summary>
            /// <param name="orderNum">委托单号。</param>
            /// <param name="reason">撤单失败原因。</param>
            public void CancelOrderFaild(USeOrderNum orderNum, string reason)
            {
                lock (m_object)
                {
                    int index = InternalGetOrderBookIndex(orderNum);
                    if (index <0)
                    {
                        Debug.Assert(false, string.Format("CancelOrderFaild() not found ordernum[{0}].", orderNum.OrderString));
                        //未查询到委托单
                        return;
                    }

                    USeOrderBook orderBook = m_orderBookList[index];
                    orderBook.Memo = reason;
                }
            }

            /// <summary>
            /// 按先后顺序更新成交回报。
            /// </summary>
            /// <param name="tradeBook"></param>
            public void AddTradeField(TradeField tradeField)
            {
                USeTradeBook tradeBook = m_driver.CtpTradeFieldToUSeTradeBook(tradeField);
                if (tradeBook.TradeTime < this.LastTradeTime)
                {
                    Debug.Assert(false, string.Format("AddTradeField() tradeTime {0:yyyy-MM-dd HH:mm:ss} less than last trade time {1:yyyy-MM-dd HH:mm:ss}",
                                 tradeBook.TradeTime, this.LastTradeTime));
                }

                lock (m_object)
                {
                    bool exist = false; // 检验是否已经收到TradeBook
                    foreach (USeTradeBook tradeBookItem in m_tradeBookList)
                    {
                        if (tradeBookItem.TradeNum == tradeBook.TradeNum && tradeBookItem.OrderNum.Equals(tradeBook.OrderNum))
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (exist == true)
                    {
                        Debug.Assert(false, string.Format("Trade field repeat,TradeNum = {0}.", tradeBook.TradeNum));
                        return;
                    }

                    m_tradeBookList.Add(tradeBook); // 添加到成交列表中

                    InternalUpdatePositonListByTradeBook(tradeBook); // 更新持仓主表信息

                    InternalUpdatePositonDetailByTradeBook(tradeBook); // 更新持仓明细信息

                    InternalUpdateFrozonOrderByTradeBook(tradeBook); // 更新冻结委托单

                    foreach (USePosition item in m_positionList)  // 计算主表持仓的持仓金额和均价
                    {
                        if (item.Instrument == tradeBook.Instrument)
                        {
                            int volumeMultiple = InternalGetVolumeMultiple(item.Instrument);
                            decimal positionAmount = InternalCalculatePositionOpenAmount(item.Instrument, item.Direction, item.TotalPosition);
                            item.Amount = positionAmount;
                            item.AvgPirce = positionAmount.Divide(item.TotalPosition * volumeMultiple);
                        }
                    }
                }
            }
            #endregion // 更新数据信息

            #region 内部更新数据方法(不加锁)
            /// <summary>
            /// 更新持仓主表信息。
            /// </summary>
            /// <param name="tradeBook">成交回报。</param>
            private void InternalUpdatePositonListByTradeBook(USeTradeBook tradeBook)
            {
                USePosition position = null;

                USeDirection direction = tradeBook.OrderSide == USeOrderSide.Buy ? USeDirection.Long : USeDirection.Short;
                if (tradeBook.OffsetType == USeOffsetType.Close ||
                   tradeBook.OffsetType == USeOffsetType.CloseToday ||
                   tradeBook.OffsetType == USeOffsetType.CloseHistory)
                {
                    //平仓Direction取反
                    direction = direction == USeDirection.Long ? USeDirection.Short : USeDirection.Long;
                }
                else
                {
                    Debug.Assert(tradeBook.OffsetType == USeOffsetType.Open);
                }


                foreach (USePosition item in m_positionList)
                {
                    if (item.Instrument == tradeBook.Instrument && item.Direction == direction)
                    {
                        position = item;
                        break;
                    }
                }

                switch (tradeBook.OffsetType)
                {
                    case USeOffsetType.Open:
                        {
                            #region 开仓
                            if (position != null)
                            {
                                //更新(增加)持仓
                                position.NewPosition += tradeBook.Qty;
                                position.OpenQty += tradeBook.Qty;
                            }
                            else
                            {
                                //新增持仓
                                position = new USePosition();
                                position.Instrument = tradeBook.Instrument.Clone();
                                position.Direction = direction;
                                position.NewPosition = tradeBook.Qty;
                                position.YesterdayPosition = 0;
                                position.OldPosition = 0;
                                position.OpenQty = tradeBook.Qty;
                                m_positionList.Add(position);
                            }
                            #endregion
                        }
                        break;
                    case USeOffsetType.CloseToday:
                        {
                            #region 平今仓
                            Debug.Assert(position != null);
                            Debug.Assert(position.NewPosition >= tradeBook.Qty);
                            position.NewPosition -= tradeBook.Qty;
                            position.CloseQty += tradeBook.Qty;
                            #endregion // 平今仓
                        }
                        break;
                    case USeOffsetType.CloseHistory:
                        {
                            #region 平昨仓
                            Debug.Assert(position != null);
                            Debug.Assert(position.OldPosition >= tradeBook.Qty);
                            position.OldPosition -= tradeBook.Qty;
                            position.CloseQty += tradeBook.Qty;
                            #endregion // 平昨仓
                        }
                        break;
                    case USeOffsetType.Close:
                        {
                            #region 平仓(未指定平今还是平昨则以先平左后平今原则进行)
                            Debug.Assert(position != null);
                            Debug.Assert(position.TotalPosition >= tradeBook.Qty);

                            if (position.OldPosition >= tradeBook.Qty)
                            {
                                position.OldPosition -= tradeBook.Qty;
                            }
                            else
                            {
                                position.NewPosition -= (tradeBook.Qty - position.OldPosition);
                                position.OldPosition = 0;
                            }
                            position.CloseQty += tradeBook.Qty;
                            #endregion // 平仓
                        }
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            /// <summary>
            /// 更新持仓明细信息。
            /// </summary>
            /// <param name="tradeBook">成交回报。</param>
            private void InternalUpdatePositonDetailByTradeBook(USeTradeBook tradeBook)
            {
                int volumeMultiple = InternalGetVolumeMultiple(tradeBook.Instrument);

                if (tradeBook.OffsetType == USeOffsetType.Open)
                {
                    //建仓新增一条记录
                    USePositionDetail positonDetail = new USePositionDetail();
                    positonDetail.ID = m_posDetailIdcreator.Next();
                    positonDetail.Instrument = tradeBook.Instrument.Clone();
                    positonDetail.Direction = tradeBook.OrderSide == USeOrderSide.Buy ? USeDirection.Long : USeDirection.Short;
                    positonDetail.PositionType = USePositionType.Today;
                    positonDetail.OpenQty = tradeBook.Qty;
                    positonDetail.OpenPrice = tradeBook.Price;
                    positonDetail.OpenTime = tradeBook.TradeTime;
                    positonDetail.CloseQty = 0;
                    positonDetail.ClosePrice = 0m;

                    m_positionDetailList.Add(positonDetail);
                }
                else
                {
                    //平仓
                    m_positionDetailList.Sort(USePositionDetail.SortByOpenTimeAsc);

                    int qty = tradeBook.Qty;
                    for (int i = 0; i < m_positionDetailList.Count; i++)
                    {
                        USePositionDetail posDetail = m_positionDetailList[i];
                        if (tradeBook.Instrument != posDetail.Instrument)
                        {
                            //合约不符
                            continue;
                        }

                        if (tradeBook.OffsetType == USeOffsetType.CloseToday && posDetail.PositionType != USePositionType.Today)
                        {
                            //指定平仓类型不符
                            continue;
                        }
                        if (tradeBook.OffsetType == USeOffsetType.CloseHistory && posDetail.PositionType != USePositionType.Yestorday)
                        {
                            //指定平仓类型不符
                            continue;
                        }

                        // 如果tradeBook.OffsetType=USeOffsetType.Close则以先平昨后平今原则，则按序平仓即可
                        USeDirection direction = tradeBook.OrderSide == USeOrderSide.Sell ? USeDirection.Long : USeDirection.Short;
                        if (direction != posDetail.Direction)
                        {
                            //买卖方向与持仓方向不符
                            continue;
                        }

                        int tradeQty = posDetail.RemainQty >= qty ? qty : posDetail.RemainQty;
                        Debug.Assert(tradeQty >= 0, "AddTradeField(),trade qty less than zero.");

                        posDetail.CloseQty += tradeQty;
                        posDetail.CloseAmount += tradeQty * tradeBook.Price * volumeMultiple;
                        posDetail.ClosePrice = posDetail.CloseAmount.Divide(posDetail.CloseQty * volumeMultiple);
                        qty -= tradeQty;

                        if (qty <= 0)
                        {
                            Debug.Assert(qty == 0, "AddTradeField(),qty less than zero.");
                            break;
                        }
                    }

                    //[yangming]待核查
                    //Debug.Assert(qty == 0, "AddTradeField(),qty not equal zero.");
                }
            }

            /// <summary>
            /// 更新持仓明细信息。
            /// </summary>
            /// <param name="tradeBook">成交回报。</param>
            private void InternalUpdateFrozonOrderByTradeBook(USeTradeBook tradeBook)
            {
                if (tradeBook.OffsetType == USeOffsetType.Open) return;

                foreach (FrozonOrder order in m_frozonOrderList)
                {
                    if (tradeBook.OrderNum.Equals(order.OrderNum))
                    {
                        order.TradeQty += tradeBook.Qty;
                        break;
                    }
                }
            }

            /// <summary>
            /// 根据建仓明细计算建仓金额。
            /// </summary>
            /// <param name="instrument">合约。</param>
            /// <param name="direction">建仓方向。</param>
            /// <param name="holdQty">持仓总手数(此参数无用，只用用来做校验，检查明细和持仓主表手数是否一致)。</param>
            /// <returns>建仓金额。</returns>
            private decimal InternalCalculatePositionOpenAmount(USeInstrument instrument, USeDirection direction, int holdQty)
            {
                decimal amount = 0m;
                int qty = 0;

                int volumeMultiple = InternalGetVolumeMultiple(instrument);

                foreach (USePositionDetail detail in m_positionDetailList)
                {
                    if (detail.Instrument == instrument && detail.Direction == direction)
                    {
                        qty += detail.RemainQty;
                        amount += detail.RemainQty * detail.OpenPrice * volumeMultiple;
                    }
                }

                //[yangming]待查
                //Debug.Assert(qty == holdQty, "CalculatePositionOpenAmount(),qty != holdQty");
                return amount;
            }
            #endregion

            #region 获取数据信息方法
            public void InsertQueryInfo(CtpQueryInfo queryInfo)
            {
                lock (m_object)
                {
                    if (queryInfo.QueryType == CtpQueryType.Fund)
                    {
                        for (int i = m_queryList.Count - 1; i >= 0; i--)
                        {
                            if (m_queryList[i].QueryType == CtpQueryType.Fund)
                            {
                                m_queryList.RemoveAt(i);
                            }
                        }
                    }
                    m_queryList.Insert(0, queryInfo);
                }
            }


            /// <summary>
            /// 获取下一个手续费查询合约。
            /// </summary>
            /// <returns>合约。</returns>
            public CtpQueryInfo GetNextQueryInfo()
            {
                lock (m_object)
                {
                    if (m_queryList.Count <= 0)
                    {
                        return null;
                    }
                    else
                    {
                        return m_queryList[0];
                    }
                }
            }

            /// <summary>
            /// 根据合约代码获取USeInstrument对象。
            /// </summary>
            /// <param name="instrumentCode">合约代码。</param>
            /// <returns>USeInstrument对象。</returns>
            public USeInstrument GetInstrumnetByCode(string instrumentCode)
            {
                lock (m_object)
                {
                    return InternalGetInstrumnetByCode(instrumentCode);
                }
            }

            public bool ExistInstrument(string instrumentCode)
            {
                lock (m_object)
                {
                    return m_instrumentCodeDic.ContainsKey(instrumentCode);
                }
            }

            /// <summary>
            /// 获取所有合约明细信息。
            /// </summary>
            /// <returns>合约明细信息列表。</returns>
            public List<USeInstrumentDetail> GetInstrumentDetail()
            {
                List<USeInstrumentDetail> list = new List<USeInstrumentDetail>();
                lock (m_object)
                {
                    foreach (KeyValuePair<USeInstrument, USeInstrumentDetail> item in m_instrumentDic)
                    {
                        list.Add(item.Value.Clone());
                    }
                }
                return list;
            }

            /// <summary>
            /// 获取指定合约明细信息。
            /// </summary>
            /// <param name="instrument">合约。</param>
            /// <returns>合约明细信息。</returns>
            public USeInstrumentDetail GetInstrumentDetail(USeInstrument instrument)
            {
                lock (m_object)
                {
                    USeInstrumentDetail detail = null;
                    if (m_instrumentDic.TryGetValue(instrument, out detail))
                    {
                        return detail.Clone();
                    }
                    else
                    {
                        return null;
                    }
                }
            }


            /// <summary>
            /// 获取指定合约明细信息。
            /// </summary>
            /// <param name="instrument">合约。</param>
            /// <returns>合约明细信息。</returns>
            public USeInstrumentDetail GetInstrumentDetail(string instrumentCode)
            {
                lock (m_object)
                {
                    USeInstrumentDetail detail = null;
                    if (m_instrumentCodeDic.ContainsKey(instrumentCode) == false)
                    {
                        return null;
                    }
                    else
                    {
                        USeInstrument instrument = m_instrumentCodeDic[instrumentCode];
                        if (m_instrumentDic.TryGetValue(instrument, out detail))
                        {
                            return detail.Clone();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

            /// <summary>
            /// 查询指定品种合约明细。
            /// </summary>
            /// <param name="varieties">品种代码。</param>
            /// <returns></returns>
            public List<USeInstrumentDetail> GetInstrumentDetailByVarieties(string varieties)
            {
                lock (m_object)
                {
                    List<USeInstrumentDetail> details = new List<USeInstrumentDetail>();
                    foreach (KeyValuePair<USeInstrument, USeInstrumentDetail> item in m_instrumentDic)
                    {
                        if (item.Value.Varieties == varieties)
                        {
                            details.Add(item.Value.Clone());
                        }
                    }

                    return details;
                }
            }

            /// <summary>
            /// 获取品种明细信息。
            /// </summary>
            /// <returns>品种明细信息。</returns>
            public List<USeProductDetail> GetProductDetail()
            {
                lock (m_object)
                {
                    List<USeProductDetail> list = new List<USeProductDetail>();
                    lock (m_object)
                    {
                        foreach (USeProductDetail item in m_productDetailDic.Values)
                        {
                            list.Add(item.Clone());
                        }
                    }
                    return list;
                }
            }

            /// <summary>
            /// 获取品种明细信息。
            /// </summary>
            /// <param name="productCode">产品代码。</param>
            /// <returns>品种明细信息。</returns>
            public USeProductDetail GetProductDetail(string productCode)
            {
                lock (m_object)
                {
                    USeProductDetail detail = null;
                    if (m_productDetailDic.TryGetValue(productCode, out detail))
                    {
                        return detail.Clone();
                    }
                    else
                    {
                        return null;
                    }
                }
            }


            /// <summary>
            /// 获取指定委托单。
            /// </summary>
            /// <param name="orderNum">委托单号。</param>
            /// <returns>委托单</returns>
            public USeOrderBook GetOrderBook(USeOrderNum orderNum)
            {
                lock (m_object)
                {
                    int index = InternalGetOrderBookIndex(orderNum);
                    if (index >=0)
                    {
                        return m_orderBookList[index].Clone();
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// 获取指定成交单。
            /// </summary>
            /// <param name="orderNum">委托单号。</param>
            /// <param name="tradeNum">成交编号。</param>
            /// <returns></returns>
            public USeTradeBook GetTradeBook(USeOrderNum orderNum, string tradeNum)
            {
                lock (m_object)
                {
                    int index = InternalGetTradeBookIndex(orderNum, tradeNum);
                    if (index >= 0)
                    {
                        return m_tradeBookList[index].Clone();
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// 获取所有委托单(同成交回报校验后包含成交金额手续费信息的委托单)。
            /// </summary>
            /// <returns>委托回报列表。</returns>
            public List<USeOrderBook> GetAllCheckedOrderBook()
            {
                lock (m_object)
                {
                    List<USeOrderBook> list = new List<USeOrderBook>();
                    foreach(USeOrderBook orderBook in m_orderBookList)
                    {
                        list.Add(InternalGetCheckedOrderBook(orderBook.OrderNum));
                    }

                    return list;
                }
            }

            /// <summary>
            /// 根据合约获取所有委托回报。
            /// </summary>
            /// <param name="instrument">合约。</param>
            /// <returns></returns>
            public List<USeOrderBook> GetCheckedOrderBook(USeInstrument instrument)
            {
                lock (m_object)
                {
                    List<USeOrderBook> list = new List<USeOrderBook>();
                    foreach (USeOrderBook orderBook in m_orderBookList)
                    {
                        if (orderBook.Instrument.Equals(instrument))
                        {
                            list.Add(InternalGetCheckedOrderBook(orderBook.OrderNum));
                        }
                    }

                    return list;
                }
            }

            /// <summary>
            /// 根据合约获取所有成交回报。
            /// </summary>
            /// <param name="instrument"></param>
            /// <returns></returns>
            public List<USeTradeBook> GetTradeBook(USeInstrument instrument)
            {
                lock (m_object)
                {
                    List<USeTradeBook> list = new List<USeTradeBook>();
                    foreach (USeTradeBook tradeBook in m_tradeBookList)
                    {
                        if (tradeBook.Instrument.Equals(instrument))
                        {
                            list.Add(tradeBook.Clone());
                        }
                    }

                    return list;
                }
            }

            /// <summary>
            /// 获取所有成交回报。
            /// </summary>
            /// <returns>成交回报列表。</returns>
            public List<USeTradeBook> GetAllTradeBook()
            {
                lock (m_object)
                {
                    List<USeTradeBook> list = new List<USeTradeBook>();
                    foreach (USeTradeBook item in m_tradeBookList)
                    {
                        list.Add(item.Clone());
                    }

                    return list;
                }
            }

            /// <summary>
            /// 获取所有持仓信息。
            /// </summary>
            /// <returns>持仓信息列表。</returns>
            public List<USePosition> GetAllPosition()
            {
                lock (m_object)
                {
                    List<USePosition> list = new List<USePosition>();
                    foreach (USePosition item in m_positionList)
                    {
                        USePosition position = item.Clone();

                        int newFrozonQty = 0;
                        int oldFrozonQty = 0;
                        InternalGetFrozonQty(position.Instrument, position.Direction, position.NewPosition, position.OldPosition, out newFrozonQty, out oldFrozonQty);
                        position.NewFrozonPosition = newFrozonQty;
                        position.OldFrozonPosition = oldFrozonQty;

                        list.Add(position);
                    }
                    return list;
                }
            }

            /// <summary>
            /// 获取指定合约指定方向持仓。
            /// </summary>
            /// <param name="instrument">合约。</param>
            /// <param name="direction">持仓方向。</param>
            /// <returns>持仓信息。</returns>
            public USePosition GetPosition(USeInstrument instrument, USeDirection direction)
            {
                lock (m_object)
                {
                    USePosition position = null;
                    foreach (USePosition item in m_positionList)
                    {
                        if (item.Instrument == instrument && item.Direction == direction)
                        {
                            position = item.Clone();
                            break;
                        }
                    }
                    if (position != null)
                    {
                        int newFrozonQty = 0;
                        int oldFrozonQty = 0;
                        InternalGetFrozonQty(instrument, direction, position.NewPosition, position.OldPosition, out newFrozonQty, out oldFrozonQty);
                        position.NewFrozonPosition = newFrozonQty;
                        position.OldFrozonPosition = oldFrozonQty;
                    }

                    return position;
                }
            }

            /// <summary>
            /// 获取所有持仓明细信息。
            /// </summary>
            /// <returns>持仓明细列表。</returns>
            public List<USePositionDetail> GetAllPositionDetail()
            {
                lock (m_object)
                {
                    List<USePositionDetail> list = new List<USePositionDetail>();
                    foreach (USePositionDetail item in m_positionDetailList)
                    {
                        list.Add(item.Clone());
                    }
                    return list;
                }
            }

            /// <summary>
            /// 获取指定合约指定持仓方向持仓信息。
            /// </summary>
            /// <param name="instrument">合约信息。</param>
            /// <param name="direction">持仓方向。</param>
            /// <returns>持仓明细信息。</returns>
            public List<USePositionDetail> GetPositionDetail(USeInstrument instrument, USeDirection direction)
            {
                lock (m_object)
                {
                    List<USePositionDetail> positionDetail = new List<USePositionDetail>();
                    foreach (USePositionDetail item in m_positionDetailList)
                    {
                        if (item.Instrument == instrument && item.Direction == direction)
                        {
                            positionDetail.Add(item.Clone());
                        }
                    }
                    return positionDetail;
                }
            }

            /// <summary>
            /// 获取账户信息。
            /// </summary>
            /// <returns>账户信息。</returns>
            public USeFund GetFund()
            {
                lock (m_object)
                {
                    return m_fund.Clone();
                }
            }

            /// <summary>
            /// 获取投资者基本信息。
            /// </summary>
            /// <returns></returns>
            public USeInvestorBaseInfo GetInvestorBaseInfo()
            {
                lock (m_object)
                {
                    return m_investorBaseInfo.Clone();
                }
            }

            /// <summary>
            /// 获取合约乘数。
            /// </summary>
            /// <param name="instrument">合约。</param>
            /// <returns>合约乘数。</returns>
            public int GetVolumeMultiple(USeInstrument instrument)
            {
                lock (m_object)
                {
                    return InternalGetVolumeMultiple(instrument);
                }
            }

            /// <summary>
            /// 获取合约手续费。
            /// </summary>
            /// <param name="instrument">合约。</param>
            /// <returns>手续费信息。</returns>
            public USeFee GetFee(USeInstrument instrument)
            {
                lock (m_object)
                {
                    return InternalGetFee(instrument);
                }
            }

            /// <summary>
            /// 获取合约保证金。
            /// </summary>
            /// <param name="instrument">合约。</param>
            /// <returns>保证金信息。</returns>
            public USeMargin GetMargin(USeInstrument instrument)
            {
                lock (m_object)
                {
                    return InternalGetMargin(instrument);
                }
            }

            /// <summary>
            /// 获取指定委托单号的委托回报
            /// </summary>
            /// <param name="orderNum">委托单号。</param>
            /// <returns></returns>
            public USeOrderBook GetCheckedOrderBook(USeOrderNum orderNum)
            {
                lock (m_object)
                {
                    return InternalGetCheckedOrderBook(orderNum);
                }
            }
            #endregion 获取数据信息方法

            #region private methods
            /// <summary>
            /// 根据合约代码获取USeInstrument对象。
            /// </summary>
            /// <param name="instrumentCode">合约代码。</param>
            /// <returns>USeInstrument对象。</returns>
            private USeInstrument InternalGetInstrumnetByCode(string instrumentCode)
            {
                if (m_instrumentCodeDic.ContainsKey(instrumentCode))
                {
                    return m_instrumentCodeDic[instrumentCode].Clone();
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 从合约明细列表中读取合约乘数。
            /// </summary>
            /// <param name="instrument">合约。</param>
            /// <returns>合约乘数。</returns>
            private int InternalGetVolumeMultiple(USeInstrument instrument)
            {
                USeInstrumentDetail detail;
                if (m_instrumentDic.TryGetValue(instrument, out detail))
                {
                    return detail.VolumeMultiple;
                }
                else
                {
                    //Debug.Assert(false, string.Format("GetVolumeMultiple({0}) failed", instrument));
                    return 0;
                }
            }

            /// <summary>
            /// 从手续费列表中读取手续费。
            /// </summary>
            /// <param name="instrument">合约名称。</param>
            /// <returns></returns>
            private USeFee InternalGetFee(USeInstrument instrument)
            {
                USeFee fee;
                if (m_feeDic.TryGetValue(instrument, out fee))
                {
                    return fee.Clone();
                }
                else
                {
                    //将对应的手续费查询提升到待查询队列头部
                    CtpQueryInfo queryInfo = null;
                    for (int i = 0; i < m_queryList.Count; i++)
                    {
                        if (m_queryList[i].QueryType == CtpQueryType.Fee)
                        {
                            if ((m_queryList[i] as CtpQueryFeeInfo).InstrumentCode == instrument.InstrumentCode)
                            {
                                queryInfo = m_queryList[i];
                                m_queryList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    Debug.Assert(queryInfo != null);
                    if (queryInfo != null)
                    {
                        m_queryList.Insert(0, queryInfo);
                    }
                    return null;
                }
            }

            /// <summary>
            /// 从保证金列表中读取保证金。
            /// </summary>
            /// <param name="instrument"></param>
            /// <returns></returns>
            private USeMargin InternalGetMargin(USeInstrument instrument)
            {
                USeMargin margin;
                if (m_marginDic.TryGetValue(instrument, out margin))
                {
                    return margin.Clone();
                }
                else
                {
                    //将对应的保证金查询提升到待查询队列头部
                    CtpQueryInfo queryInfo = null;
                    for (int i = 0; i < m_queryList.Count; i++)
                    {
                        if (m_queryList[i].QueryType == CtpQueryType.Margin)
                        {
                            if ((m_queryList[i] as CtpQueryMarginInfo).InstrumentCode == instrument.InstrumentCode)
                            {
                                queryInfo = m_queryList[i];
                                m_queryList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    Debug.Assert(queryInfo != null);
                    if (queryInfo != null)
                    {
                        m_queryList.Insert(0, queryInfo);
                    }
                    return null;
                }
            }

            /// <summary>
            /// 获取指定委托单号的委托回报
            /// </summary>
            /// <param name="orderNum">委托单号。</param>
            /// <returns></returns>
            private USeOrderBook InternalGetCheckedOrderBook(USeOrderNum orderNum)
            {
                USeOrderBook checkedOrderBook = new USeOrderBook();

                int index = InternalGetOrderBookIndex(orderNum);

                if (index <0)
                {
                    //Debug.Assert(false);
                    return null;
                }

                USeOrderBook orderBook = m_orderBookList[index];

                int volumeMultiple = GetVolumeMultiple(orderBook.Instrument);
                int tradeQty = 0;
                decimal tradeAmount = 0m;  //根据成交回报计算成交量以及成交金额
                decimal fee = 0m;

                foreach (USeTradeBook tradeBook in m_tradeBookList)
                {
                    if (tradeBook.OrderNum.Equals(orderBook.OrderNum))
                    {
                        tradeAmount += tradeBook.Price * tradeBook.Qty * volumeMultiple;
                        tradeQty += tradeBook.Qty;
                        fee += tradeBook.Fee;
                    }
                }

                checkedOrderBook.OrderNum = orderBook.OrderNum.Clone();
                checkedOrderBook.Account = orderBook.Account;
                checkedOrderBook.Instrument = orderBook.Instrument.Clone();
                checkedOrderBook.OrderQty = orderBook.OrderQty;
                checkedOrderBook.OrderPrice = orderBook.OrderPrice;
                checkedOrderBook.TradeQty = tradeQty;
                checkedOrderBook.TradeAmount = tradeAmount;
                checkedOrderBook.TradeFee = fee;
                checkedOrderBook.TradePrice = tradeAmount.Divide(tradeQty * volumeMultiple);

                if (orderBook.TradeQty == tradeQty) // 委托中的成交量与成交回报的成交量一致则说明委托状态是正确的
                {
                    checkedOrderBook.CancelQty = orderBook.CancelQty;
                    checkedOrderBook.OrderStatus = orderBook.OrderStatus;
                }
                else if (orderBook.TradeQty < tradeQty) // 委托回报还未更新 
                {
                    //以当前委托回报发送即可
                    checkedOrderBook.CancelQty = orderBook.CancelQty;
                    checkedOrderBook.OrderStatus = orderBook.OrderStatus;
                }
                else // 成交回报还未更新
                {
                    Debug.Assert(orderBook.TradeQty > tradeQty);
                    checkedOrderBook.OrderStatus = tradeQty > 0 ? USeOrderStatus.PartTraded : USeOrderStatus.NoTraded;
                }
                checkedOrderBook.OrderSide = orderBook.OrderSide;
                checkedOrderBook.OffsetType = orderBook.OffsetType;
                checkedOrderBook.Memo = orderBook.Memo;
                checkedOrderBook.OrderTime = orderBook.OrderTime;

                return checkedOrderBook;
            }

            /// <summary>
            /// 获取指定合约指定持仓方向冻结仓位。
            /// </summary>
            /// <param name="orderNum">委托单号。</param>
            /// <returns></returns>
            private void InternalGetFrozonQty(USeInstrument instrument, USeDirection direction, int newQty, int oldQty, out int newFrozonQty, out int oldFrozonQty)
            {
                newFrozonQty = 0;
                oldFrozonQty = 0;

                foreach (FrozonOrder order in m_frozonOrderList)
                {
                    if (order.Instrument != instrument || order.Direction != direction)
                    {
                        continue; // 持仓合约方向不符，跳过
                    }

                    if (order.OrderStatus == USeOrderStatus.AllTraded ||
                       order.OrderStatus == USeOrderStatus.AllCanceled ||
                       order.OrderStatus == USeOrderStatus.BlankOrder ||
                       order.OrderStatus == USeOrderStatus.PartCanceled)
                    {
                        continue; // 结束委托单无冻结仓位
                    }

                    int frozonQty = order.CloseQty - order.TradeQty;
                    if (frozonQty == 0)
                    {
                        continue;   // 全部已成交无冻结仓位
                    }

                    switch (order.OffsetType)
                    {
                        case USeOffsetType.CloseToday:
                            newFrozonQty += frozonQty; break;
                        case USeOffsetType.CloseHistory:
                            oldFrozonQty += frozonQty; break;
                        case USeOffsetType.Close:
                            if (instrument.Market == USeMarket.SHFE) // 上海市场Close即为平昨
                            {
                                oldFrozonQty += frozonQty; break;
                            }
                            else
                            {
                                if (oldQty >= frozonQty)
                                {
                                    oldFrozonQty += frozonQty;
                                    oldQty -= frozonQty;
                                    frozonQty = 0;
                                }
                                else
                                {
                                    oldFrozonQty += oldQty;
                                    frozonQty -= oldQty;
                                    oldQty = 0;
                                }

                                if (frozonQty > 0)
                                {
                                    Debug.Assert(newQty >= frozonQty, string.Format("newQty:{0},frozonQty:{1}", newQty, frozonQty));
                                    newFrozonQty += frozonQty;
                                    newQty -= frozonQty;
                                    frozonQty = 0;
                                }
                            }
                            break;
                        default:
                            Debug.Assert(false, "Invalid offsetTye");
                            break;
                    }
                }
            }
            #endregion // private methods


            /// <summary>
            /// 按成交时间正向排序。
            /// </summary>
            /// <param name="detail1"></param>
            /// <param name="detail2"></param>
            /// <returns></returns>
            private int SortTradeFieldByTradeTimeAsc(TradeField field1, TradeField field2)
            {
                int result =field1.TradeTime.CompareTo(field2.TradeTime);
                if(result != 0)
                {
                    return result;
                }
                else 
                {
                    return field1.TradeID.ToInt().CompareTo(field2.TradeID.ToInt());
                }
            }

            private int InternalGetOrderBookIndex(USeOrderNum orderNum)
            {
                int index = -1;
                for (int i = 0; i < m_orderBookList.Count; i++)
                {
                    if (m_orderBookList[i].OrderNum.Equals(orderNum))
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }

            private int InternalGetTradeBookIndex(USeOrderNum orderNum, string tradeNum)
            {
                int index = -1;
                for (int i = 0; i < m_tradeBookList.Count; i++)
                {
                    if (m_tradeBookList[i].TradeNum.Trim() == tradeNum.Trim() &&
                        m_tradeBookList[i].OrderNum.Equals(orderNum))
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }

            #region private class
            /// <summary>
            /// 冻结持仓委托单。
            /// </summary>
            private class FrozonOrder
            {
                /// <summary>
                /// 构造FrozonOrder实例。
                /// </summary>
                /// <param name="orderNum">委托单号。</param>
                /// <param name="instrument">合约。</param>
                /// <param name="direction">持仓方向。</param>
                /// <param name="offsetType">平仓方向。</param>
                public FrozonOrder(USeOrderNum orderNum, USeInstrument instrument, USeDirection direction,USeOffsetType offsetType)
                {
                    this.OrderNum = orderNum.Clone();
                    this.Instrument = instrument.Clone();
                    this.Direction = direction;

                    if (offsetType != USeOffsetType.Close && offsetType != USeOffsetType.CloseHistory && offsetType != USeOffsetType.CloseToday)
                    {
                        throw new Exception("Invalid offsetType.");
                    }
                    this.OffsetType = offsetType;
                }

                #region 
                /// <summary>
                /// 委托单号。
                /// </summary>
                public USeOrderNum OrderNum
                {
                    get;
                    set;
                }

                /// <summary>
                /// 合约。
                /// </summary>
                public USeInstrument Instrument
                {
                    get;
                    private set;
                }

                /// <summary>
                /// 冻结仓位方向。
                /// </summary>
                public USeDirection Direction
                {
                    get;
                    private set;
                }

                /// <summary>
                /// 开平方向。
                /// </summary>
                public USeOffsetType OffsetType
                {
                    get;
                    private set;
                }

                /// <summary>
                /// 平仓量。
                /// </summary>
                public int CloseQty
                {
                    get;
                    set;
                }

                /// <summary>
                /// 成交量。
                /// </summary>
                public int TradeQty
                {
                    get;
                    set;
                }

                public USeOrderStatus OrderStatus
                {
                    get;
                    set;
                }
                #endregion // property
            }
            #endregion // private class
        }
    }
}
