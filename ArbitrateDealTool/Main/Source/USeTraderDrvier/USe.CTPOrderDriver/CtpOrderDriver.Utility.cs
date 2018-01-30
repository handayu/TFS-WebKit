#region Copyright & Version
//==============================================================================
// 文件名称: CtpOrderDriver.Utility.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: CTP交易驱动类-- 字段转换方法。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using USe.TradeDriver.Common;
using CTPAPI;
using USe.Common;

namespace USe.TradeDriver.Ctp
{
    public partial class CtpOrderDriver
    {
        /// <summary>
        /// CTP InstrumentField To USeInstrumentDetail。
        /// </summary>
        /// <param name="filed"></param>
        /// <returns></returns>
        private USeInstrumentDetail CtpInstrumentFieldToUSeInstrumentDetail(InstrumentField filed)
        {
            USeMarket market = CtpProtocol.FtdcExchangeToUSeMarket(filed.ExchangeID);
            Debug.Assert(market != USeMarket.Unknown, "CtpInstrumentFieldToUSeInstrumentDetail(),market is unknown.");

            USeInstrumentDetail detail = new USeInstrumentDetail();
            detail.Instrument = new USeInstrument(filed.InstrumentID,
                                                         filed.InstrumentName,
                                                         market);
            try
            {
                detail.OpenDate = DateTime.ParseExact(filed.OpenDate, "yyyyMMdd", null);
                detail.ExpireDate = DateTime.ParseExact(filed.ExpireDate, "yyyyMMdd", null);
                if (string.IsNullOrEmpty(filed.StartDelivDate) == false)
                {
                    detail.StartDelivDate = DateTime.ParseExact(filed.StartDelivDate, "yyyyMMdd", null);
                }
                else
                {
                    //[yangming] 有的合约查询不到开始交割日，暂时用到期日下一天
                    detail.StartDelivDate = detail.ExpireDate.AddDays(1);
                }
                detail.EndDelivDate = DateTime.ParseExact(filed.EndDelivDate, "yyyyMMdd", null);
                detail.VolumeMultiple = filed.VolumeMultiple;
                detail.IsTrading = filed.IsTrading == IntBoolType.Yes;
                detail.Varieties = filed.ProductID;
                detail.PriceTick = filed.PriceTick.ToDecimal();
                detail.ExchangeLongMarginRatio = filed.LongMarginRatio.ToDecimal();
                detail.ExchangeShortMarginRatio = filed.ShortMarginRatio.ToDecimal();

                detail.MaxMarketOrderVolume = filed.MaxMarketOrderVolume;
                detail.MinMarketOrderVolume = filed.MinMarketOrderVolume;
                detail.MaxLimitOrderVolume = filed.MaxLimitOrderVolume;
                detail.MinLimitOrderVolume = filed.MinLimitOrderVolume;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "CtpInstrumentFieldToUSeInstrumentDetail() convet failed," + ex.Message);
            }

            return detail;
        }

        /// <summary>
        /// CTP OrderField To USeOrderBook。
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private USeOrderBook CtpOrderFieldToUSeOrderBook(OrderField field)
        {
            USeOrderBook orderbook = new USeOrderBook();
            try
            {          
                orderbook.OrderNum = new CtpOrderNum(field.FrontID,
                                                      field.SessionID,
                                                      field.OrderRef,
                                                      field.ExchangeID,
                                                      field.OrderSysID);
                orderbook.Account = field.InvestorID.ToString().Trim();
                orderbook.Instrument = new USeInstrument(field.InstrumentID.Trim(),
                                                    field.InstrumentID.Trim(),
                                                    CtpProtocol.FtdcExchangeToUSeMarket(field.ExchangeID));
                orderbook.OrderQty = field.VolumeTotalOriginal;
                orderbook.OrderPrice = Convert.ToDecimal(field.LimitPrice);
                orderbook.TradeQty = field.VolumeTraded;
                orderbook.TradeAmount = 0m; // 此处转换不处理,最终再计算
                switch (field.OrderStatus)
                {
                    case OrderStatusType.AllTraded: orderbook.OrderStatus = USeOrderStatus.AllTraded; break;
                    case OrderStatusType.PartTradedNotQueueing:
                    case OrderStatusType.PartTradedQueueing: orderbook.OrderStatus = USeOrderStatus.PartTraded; break;
                    case OrderStatusType.Canceled: orderbook.OrderStatus = field.VolumeTraded > 0 ? USeOrderStatus.PartCanceled : USeOrderStatus.AllCanceled; break;
                    case OrderStatusType.NoTradeQueueing:
                    case OrderStatusType.NoTradeNotQueueing: orderbook.OrderStatus = USeOrderStatus.NoTraded; break;
                    case OrderStatusType.Unknown: orderbook.OrderStatus = USeOrderStatus.Unknown;break;
                    default:
                        {
                            Debug.Assert(false, string.Format("CtpOrderFieldToUSeOrderBook() unknown OrderStatus [{0}]", field.OrderStatus));
                            orderbook.OrderStatus = USeOrderStatus.Unknown;
                            break;
                        }
                }
                orderbook.CancelQty = field.OrderStatus == OrderStatusType.Canceled ? field.VolumeTotalOriginal - field.VolumeTraded : 0;
                orderbook.OrderSide = field.Direction == DirectionType.Buy ? USeOrderSide.Buy : USeOrderSide.Sell;
                switch (field.CombOffsetFlag1)
                {
                    case OffsetFlagType.Close:
                    case OffsetFlagType.ForceClose: orderbook.OffsetType = USeOffsetType.Close; break;
                    case OffsetFlagType.CloseToday: orderbook.OffsetType = USeOffsetType.CloseToday; break;
                    case OffsetFlagType.CloseYesterday: orderbook.OffsetType = USeOffsetType.CloseHistory; break;
                    case OffsetFlagType.Open:
                        orderbook.OffsetType = USeOffsetType.Open; break;
                    default:
                        Debug.Assert(false, "Invalid comb offset Flag1.");
                        break;
                }
                orderbook.Memo = field.StatusMsg;
                

                DateTime orderTime = DateTime.Now;
                if (string.IsNullOrEmpty(field.InsertDate) && string.IsNullOrEmpty(field.InsertTime))
                {
                    Debug.Assert(false);
                    orderTime = DateTime.Now;
                }
                else if (string.IsNullOrEmpty(field.InsertDate))
                {
                    if (DateTime.TryParseExact(DateTime.Today.ToString("yyyyMMdd") + field.InsertTime, "yyyyMMddHH:mm:ss", null, System.Globalization.DateTimeStyles.None, out orderTime) == false)
                    {
                        Debug.Assert(false);
                        orderTime = DateTime.Now;
                    }
                }
                else
                {
                    if (DateTime.TryParseExact(field.InsertDate + field.InsertTime, "yyyyMMddHH:mm:ss", null, System.Globalization.DateTimeStyles.None, out orderTime) == false)
                    {
                        Debug.Assert(false);
                        orderTime = DateTime.Now;
                    }
                }
                orderbook.OrderTime = orderTime;

            }
            catch (Exception ex)
            {
                Debug.Assert(false, "CtpOrderFieldToUSeOrderBook() convet failed," + ex.Message);
            }
            return orderbook;
        }

        /// <summary>
        /// CTP TradeField To USeTradeBook。
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private USeTradeBook CtpTradeFieldToUSeTradeBook(TradeField field)
        {
            USeTradeBook tradeBook = new USeTradeBook();
            try
            {
                tradeBook.Instrument = new USeInstrument(field.InstrumentID.Trim(),
                                                    field.InstrumentID.Trim(),
                                                    CtpProtocol.FtdcExchangeToUSeMarket(field.ExchangeID));
                tradeBook.TradeNum = field.TradeID.Trim();
                USeOrderNum orderNum = new CtpOrderNum(field.ExchangeID, field.OrderSysID);
                USeOrderBook orderBook = m_dataBuffer.GetOrderBook(orderNum);
                if (orderBook != null)
                {
                    orderNum = orderBook.OrderNum.Clone();  // 去搜索对应委托单，若存在则将委托单号设置为完整委托单号
                }
                tradeBook.OrderNum = orderNum;
                tradeBook.OrderSide = field.Direction == DirectionType.Buy ? USeOrderSide.Buy : USeOrderSide.Sell;
                switch (field.OffsetFlag)
                {
                    case OffsetFlagType.Close:
                    case OffsetFlagType.ForceClose:
                        tradeBook.OffsetType = USeOffsetType.Close; break;
                    case OffsetFlagType.CloseToday:
                        tradeBook.OffsetType = USeOffsetType.CloseToday; break;
                    case OffsetFlagType.CloseYesterday:
                        tradeBook.OffsetType = USeOffsetType.CloseHistory; break;
                    case OffsetFlagType.Open:
                        tradeBook.OffsetType = USeOffsetType.Open; break;
                    default:
                        Debug.Assert(false, string.Format("CtpTradeFieldToUSeTradeBook(),Invalid offsetfalg.", field.OffsetFlag));
                        break;
                }
                tradeBook.Price = Convert.ToDecimal(field.Price);
                tradeBook.Qty = field.Volume;
                tradeBook.Account = field.InvestorID;
                DateTime tradeTime = DateTime.Now;
                if (string.IsNullOrEmpty(field.TradeDate) && string.IsNullOrEmpty(field.TradeTime))
                {
                    Debug.Assert(false);
                    tradeTime = DateTime.Now;
                }
                else if (string.IsNullOrEmpty(field.TradeDate))
                {
                    if (DateTime.TryParseExact(DateTime.Today.ToString("yyyyMMdd") + field.TradeTime, "yyyyMMddHH:mm:ss", null, System.Globalization.DateTimeStyles.None, out tradeTime) == false)
                    {
                        Debug.Assert(false);
                        tradeTime = DateTime.Now;
                    }
                }
                else
                {
                    if (DateTime.TryParseExact(field.TradeDate + field.TradeTime, "yyyyMMddHH:mm:ss", null, System.Globalization.DateTimeStyles.None, out tradeTime) == false)
                    {
                        Debug.Assert(false);
                        tradeTime = DateTime.Now;
                    }
                }

                tradeBook.TradeTime = tradeTime;

                int volumeMultiple = m_dataBuffer.GetVolumeMultiple(tradeBook.Instrument);

                tradeBook.Amount = tradeBook.Price * tradeBook.Qty * volumeMultiple;
                // 手续费尝试计算
                tradeBook.Fee = m_dataBuffer.CalculateFee(tradeBook.Instrument, tradeBook.OffsetType, tradeBook.Qty, tradeBook.Price);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "CtpTradeFieldToUSeTradeBook() convet failed," + ex.Message);
            }
            return tradeBook;
        }

        /// <summary>
        /// CTP InvestorPositionField To USePosition。
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private USePosition CtpInvestorPositonFieldToUSePosition(InvestorPositionField field)
        {
            USePosition position = new USePosition();
            position.Instrument = m_dataBuffer.GetInstrumnetByCode(field.InstrumentID.Trim());
            Debug.Assert(position.Instrument != null && position.InstrumentCode == field.InstrumentID.Trim(), "position.Instrument is null");

            int volumeMultiple = m_dataBuffer.GetVolumeMultiple(position.Instrument);
            switch (field.PosiDirection)
            {
                case PosiDirectionType.Long: position.Direction = USeDirection.Long; break;
                case PosiDirectionType.Short: position.Direction = USeDirection.Short; break;
                default:
                    Debug.Assert(false, "Invalid PosiDirection");
                    break;
            }
            position.NewPosition = field.TodayPosition;
            position.YesterdayPosition = field.YdPosition;
            position.OldPosition = field.Position - field.TodayPosition;
            position.AvgPirce = 0m;
            position.Amount = 0m;
            position.OpenQty = field.OpenVolume;
            position.CloseQty = field.CloseVolume;

            return position;
        }

        /// <summary>
        /// CTP InvestorPositionField To USePositionDetail,
        /// (注 :此方法只是针对有昨日持仓的持仓信息来构造USePositionDetail。
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private USePositionDetail CtpInvestorPositonFieldToUSePositionDetail(InvestorPositionField field)
        {
            if (field.YdPosition <= 0) return null; // 只能生成历史持仓

            USePositionDetail positionDetail = new USePositionDetail();
            positionDetail.Instrument = m_dataBuffer.GetInstrumnetByCode(field.InstrumentID.Trim());
            switch (field.PosiDirection)
            {
                case PosiDirectionType.Long: positionDetail.Direction = USeDirection.Long; break;
                case PosiDirectionType.Short: positionDetail.Direction = USeDirection.Short; break;
                default:
                    Debug.Assert(false, "Invalid PosiDirection");
                    break;
            }
            positionDetail.PositionType = USePositionType.Yestorday;
            positionDetail.OpenQty = field.YdPosition;
            positionDetail.OpenPrice = Convert.ToDecimal(field.PreSettlementPrice); // 昨仓取昨日结算价为建仓价
            positionDetail.OpenTime = DateTime.Today.AddDays(-1); // 建仓时间设为昨日日期
            positionDetail.CloseQty = 0;
            positionDetail.ClosePrice = 0m;
            positionDetail.CloseAmount = 0m;
            return positionDetail;
        }

        /// <summary>
        /// CTP InvestorPositionField To USePosition。
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private USeFund CtpTradingAccountFieldToUSeFund(TradingAccountField field)
        {
            USeFund fundInfo = new USeFund();
            try
            {
                fundInfo.AccountID = field.AccountID.Trim();
                fundInfo.PreMortgage = field.PreMortgage.ToDecimal();
                fundInfo.PreCredit = field.PreCredit.ToDecimal();
                fundInfo.PreBalance = field.PreBalance.ToDecimal();
                fundInfo.Deposit = field.Deposit.ToDecimal();
                fundInfo.WithDraw = field.Withdraw.ToDecimal();
                fundInfo.Mortgage = field.Mortgage.ToDecimal();
                fundInfo.DeliveryMargin = field.DeliveryMargin.ToDecimal();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "CtpTradingAccountFieldToUSeAccountInfo() convet failed," + ex.Message);
            }
            return fundInfo;
        }

        /// <summary>
        /// POS PosInvestor field To USeInvestorBaseInfo。
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private USeInvestorBaseInfo CtpInvestorFieldToUSeInvestorBaseInfo(InvestorField field)
        {
            USeInvestorBaseInfo investorInfo = new USeInvestorBaseInfo();
            try
            {
                investorInfo.Account = field.InvestorID;
                investorInfo.BrokerID = field.BrokerID;
                investorInfo.BrokerName = "";
                investorInfo.IdCardType = (USeIDCardType)((byte)field.IdentifiedCardType);
                investorInfo.IdentifiedCardNo = field.IdentifiedCardNo;
                DateTime openDate;
                if (DateTime.TryParseExact(field.OpenDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out openDate) == false)
                {
                    Debug.Assert(false);
                    openDate = DateTime.Today;
                }
                investorInfo.OpenDate = openDate;
                investorInfo.Mobile = field.Mobile;
                investorInfo.RealName = field.InvestorName;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "PosInvestorFieldToUSeInvestorBaseInfo() convet failed," + ex.Message);
            }
            return investorInfo;
        }

        /// <summary>
        /// CTP InvestorPositionField To USeFundDetail。
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private USeFundDetail CtpTradingAccountFieldToUSeFundDetail(TradingAccountField field)
        {
            USeFundDetail fundDetail = new USeFundDetail();
            try
            {
                fundDetail.AccountID = field.AccountID.Trim();
                fundDetail.Available = field.Available.ToDecimal();
                fundDetail.Deposit = field.Deposit.ToDecimal();
                fundDetail.Mortgage = field.Mortgage.ToDecimal();
                fundDetail.PreBalance = field.PreBalance.ToDecimal();
                fundDetail.PreCredit = field.PreCredit.ToDecimal();
                fundDetail.PreMortgage = field.PreMortgage.ToDecimal();
                fundDetail.WithDraw = field.Withdraw.ToDecimal();

                // 静态权益 =  上日结存 - 上次信用额度 - 上次质押金额 
                //           + 质押金额- 今日出金 + 今日入金
                decimal staticBenefit = field.PreBalance.ToDecimal() - field.PreCredit.ToDecimal() - field.PreMortgage.ToDecimal() +
                                field.Mortgage.ToDecimal() - field.Withdraw.ToDecimal() + field.Deposit.ToDecimal();
                // 动态权益 = 静态权益 + 持仓盈亏 + 平仓盈亏 - 手续费（取成交回报手续费）
                decimal dynamicBenefit = staticBenefit + field.CloseProfit.ToDecimal() + field.PositionProfit.ToDecimal() - field.Commission.ToDecimal();
                fundDetail.StaticBenefit = staticBenefit;
                fundDetail.CloseProfit = field.CloseProfit.ToDecimal();
                fundDetail.TradeFee = field.Commission.ToDecimal();
                fundDetail.HoldProfit = field.PositionProfit.ToDecimal();
                fundDetail.HoldMargin = field.CurrMargin.ToDecimal();
                fundDetail.DynamicBenefit = dynamicBenefit;
                fundDetail.FrozonMargin = field.FrozenMargin.ToDecimal();
                fundDetail.FrozonFee = field.FrozenCommission.ToDecimal();
                fundDetail.Fronzon = field.FrozenCommission.ToDecimal() + field.FrozenCash.ToDecimal() + field.FrozenMargin.ToDecimal();
                //风险度 = (占用保证金 + 交割保证金 + 买冻结保证金 + 卖冻结保证金) / 动态权益
                //[yangming]修正风险度 = (占用保证金 + 交割保证金 ) / 动态权益
                //decimal risk = (field.CurrMargin.ToDecimal() + field.DeliveryMargin.ToDecimal() + field.FrozenMargin.ToDecimal()).Divide(dynamicBenefit);
                decimal risk = (field.CurrMargin.ToDecimal() + field.DeliveryMargin.ToDecimal()).Divide(dynamicBenefit);
                fundDetail.Risk = risk;
                fundDetail.PreferCash = field.WithdrawQuota.ToDecimal();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "CtpTradingAccountFieldToUSeFundDetail() convet failed," + ex.Message);
            }
            return fundDetail;
        }

        /// <summary>
        /// CTP InstrumentCommissionRateField To USeFee。
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private USeFee CtpInstrumentCommissionRateFieldToUSeFeeInfo(InstrumentCommissionRateField filed)
        {
            USeFee fee = new USeFee();
            try
            {
                fee.OpenRatioByMoney = filed.OpenRatioByMoney == double.MaxValue ? 0m : Convert.ToDecimal(filed.OpenRatioByMoney);
                fee.CloseRatioByMoney = filed.CloseRatioByMoney == double.MaxValue ? 0m : Convert.ToDecimal(filed.CloseRatioByMoney);
                fee.CloseTodayRatioByMoney = filed.CloseTodayRatioByMoney == double.MaxValue ? 0m : Convert.ToDecimal(filed.CloseTodayRatioByMoney);
                fee.OpenRatioByVolume = filed.OpenRatioByVolume == double.MaxValue ? 0m : Convert.ToDecimal(filed.OpenRatioByVolume);
                fee.CloseRatioByVolume = filed.CloseRatioByVolume == double.MaxValue ? 0m : Convert.ToDecimal(filed.CloseRatioByVolume);
                fee.CloseTodayRatioByVolume = filed.CloseTodayRatioByVolume == double.MaxValue ? 0m : Convert.ToDecimal(filed.CloseTodayRatioByVolume);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "CtpInstrumentCommissionRateFieldToUSeFeeInfo() convet failed," + ex.Message);
            }

            return fee;
        }


        /// <summary>
        /// CTP InstrumentMarginRateField To USeMargin。
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private USeMargin CtpMarginRateFieldToUSeProductMarginInfo(InstrumentMarginRateField filed)
        {
            USeMargin margin = new USeMargin();
            margin.Instrument = m_dataBuffer.GetInstrumnetByCode(filed.InstrumentID);
            try
            {
                margin.BrokerLongMarginRatioByMoney = filed.LongMarginRatioByMoney == double.MaxValue ? 0m : Convert.ToDecimal(filed.LongMarginRatioByMoney);
                margin.BrokerLongMarginRatioByVolume = filed.LongMarginRatioByVolume == double.MaxValue ? 0m : Convert.ToDecimal(filed.LongMarginRatioByVolume);
                margin.BrokerShortMarginRatioByMoney = filed.ShortMarginRatioByMoney == double.MaxValue ? 0m : Convert.ToDecimal(filed.ShortMarginRatioByMoney);
                margin.BrokerShortMarginRatioByVolume = filed.ShortMarginRatioByVolume == double.MaxValue ? 0m : Convert.ToDecimal(filed.ShortMarginRatioByVolume);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "InstrumentMarginRateFieldToFulcProductMarginInfo() convet failed," + ex.Message);
            }

            return margin;
        }

        /// <summary>
        /// 计算保证金。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <param name="direction">方向。</param>
        /// <param name="qty">数量。</param>
        /// <param name="price">价格。</param>
        /// <returns></returns>
        private decimal CalculateMargin(USeInstrument instrument, USeDirection direction, int qty, decimal price)
        {
            USeMargin margin = m_dataBuffer.GetMargin(instrument);
            int volumeMultiple = m_dataBuffer.GetVolumeMultiple(instrument);

            switch (direction)
            {
                case USeDirection.Long:
                    return (margin.BrokerLongMarginRatioByMoney * price * qty * volumeMultiple + margin.BrokerLongMarginRatioByVolume * qty);
                case USeDirection.Short:
                    return (margin.BrokerShortMarginRatioByMoney * price * qty * volumeMultiple + margin.BrokerShortMarginRatioByVolume * qty);
                default:
                    Debug.Assert(false, "CalculateMargin(),invalid direction");
                    return 0m;
            }
        }

        private string TradeFieldToLogString(TradeField field)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(string.Format("[BrokerID:{0}]", field.BrokerID));
                sb.Append(string.Format("[Direction:{0}]", field.Direction));
                sb.Append(string.Format("[ExchangeID:{0}]", field.ExchangeID));
                sb.Append(string.Format("[InstrumentID:{0}]", field.InstrumentID));
                sb.Append(string.Format("[OffsetFlag:{0}]", field.OffsetFlag));
                sb.Append(string.Format("[OrderRef:{0}]", field.OrderRef));
                sb.Append(string.Format("[OrderSysID:{0}]", field.OrderSysID));
                sb.Append(string.Format("[Price:{0}]", field.Price));
                sb.Append(string.Format("[TradeDate:{0}]", field.TradeDate));
                sb.Append(string.Format("[TradeID:{0}]", field.TradeID));
                sb.Append(string.Format("[TradeTime:{0}]", field.TradeTime));
                sb.Append(string.Format("[UserID:{0}]", field.UserID));
                sb.Append(string.Format("[Volume:{0}]", field.Volume));
            }
            catch (Exception ex)
            {
                sb.Append("TradeFieldToLogString() failed,Error:" + ex.Message);
            }

            return sb.ToString();
        }

        private string OrderFieldToLogString(OrderField field)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(string.Format("[FrontID:{0}]", field.FrontID));
                sb.Append(string.Format("[SessionID:{0}]", field.SessionID));
                sb.Append(string.Format("[OrderRef:{0}]", field.OrderRef));
                sb.Append(string.Format("[ExchangeID:{0}]", field.ExchangeID));
                sb.Append(string.Format("[OrderSysID:{0}]", field.OrderSysID));
                sb.Append(string.Format("[InvestorID:{0}]", field.InvestorID));
                sb.Append(string.Format("[InstrumentID:{0}]", field.InstrumentID));
                sb.Append(string.Format("[VolumeTotalOriginal:{0}]", field.VolumeTotalOriginal));
                sb.Append(string.Format("[LimitPrice:{0}]", field.LimitPrice));
                sb.Append(string.Format("[VolumeTraded:{0}]", field.VolumeTraded));
                sb.Append(string.Format("[OrderStatus:{0}]", field.OrderStatus));
                sb.Append(string.Format("[Direction:{0}]", field.Direction));
                sb.Append(string.Format("[CombOffsetFlag1:{0}]", field.CombOffsetFlag1));
                sb.Append(string.Format("[StatusMsg:{0}]", field.StatusMsg));
                sb.Append(string.Format("[InsertDate:{0}]", field.InsertDate));
                sb.Append(string.Format("[InsertTime:{0}]", field.InsertTime));
            }
            catch (Exception ex)
            {
                sb.Append("OrderFieldToLogString() failed,Error:" + ex.Message);
            }

            return sb.ToString();
        }
    }
}
