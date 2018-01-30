using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using System.Diagnostics;
using System.Threading;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 资金计算器。
    /// </summary>
    public class FundCalculator
    {
        #region member
        private USeOrderDriver m_orderDriver = null;
        private USeQuoteDriver m_quoteDriver = null;

        private object m_syncObj = new object();
        private USeFundDetail m_fundDetail = new USeFundDetail();
        private System.Threading.Timer m_updateTimer = null;
        private bool m_runFlag = false;
        #endregion

        #region construction
        public FundCalculator()
        {

        }
        #endregion

        #region property
        /// <summary>
        /// 资金明细。
        /// </summary>
        public USeFundDetail FundDetail
        {
            get
            {
                lock (m_syncObj)
                {
                    return m_fundDetail.Clone();
                }
            }
        }
        #endregion

        #region
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="orderDriver">交易驱动。</param>
        /// <param name="quoteDriver">行情驱动。</param>
        /// <param name="autoTraderManager">自动下单机管理类。</param>
        public void Initialize(USeOrderDriver orderDriver,USeQuoteDriver quoteDriver)
        {
            m_orderDriver = orderDriver;
            m_quoteDriver = quoteDriver;

            m_updateTimer = new System.Threading.Timer(QueryAndUpdate, false, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// 启动。
        /// </summary>
        public void Start()
        {
            m_runFlag = true;
            m_updateTimer.Change(500, Timeout.Infinite);

            try
            {
               USeFundDetail fundDetail = m_orderDriver.QueryFundDetailInfo();
                if(fundDetail != null)
                {
                    lock(m_syncObj)
                    {
                        m_fundDetail = fundDetail;
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        /// <summary>
        /// 停止。
        /// </summary>
        public void Stop()
        {
            m_runFlag = false;
            m_updateTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public override string ToString()
        {
            return "FundCalculator";
        }
        #endregion

        private void QueryAndUpdate(object state)
        {
            try
            {
                USeFundDetail fundDetail = CalculateFundInfo();
                lock (m_syncObj)
                {
                    m_fundDetail = fundDetail;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            if (m_runFlag)
            {
                m_updateTimer.Change(500, Timeout.Infinite);
            }
        }

        #region 原始数据查询
        /// <summary>
        /// 查询持仓明细。
        /// </summary>
        /// <returns></returns>
        private List<PositionDetailCalcItem> QueryPositonDetails()
        {
            USeOrderDriver orderDriver = USeManager.Instance.OrderDriver;
            USeQuoteDriver quoteDriver = USeManager.Instance.QuoteDriver;
            List<USePositionDetail> positonList = orderDriver.QueryPositionDetail();

            List<PositionDetailCalcItem> list = new List<PositionDetailCalcItem>();
            if (positonList == null) return list;
            foreach (USePositionDetail posItem in positonList)
            {
                USeInstrumentDetail detail = orderDriver.QueryInstrumentDetail(posItem.Instrument);
                USeMargin productMargin = orderDriver.QueryInstrumentMargin(posItem.Instrument);
                USeMarketData marketData = quoteDriver.Query(posItem.Instrument);

                PositionDetailCalcItem calcItem = new PositionDetailCalcItem() {
                    InstrumentDetail = detail,
                    Margin = productMargin,
                    MarketData = marketData,

                    ID = posItem.ID,
                    Instrument = posItem.Instrument.Clone(),
                    Direction = posItem.Direction,
                    PositionType = posItem.PositionType,
                    OpenQty = posItem.OpenQty,
                    OpenPrice = posItem.OpenPrice,
                    OpenTime = posItem.OpenTime,
                    CloseQty = posItem.CloseQty,
                    CloseAmount = posItem.CloseAmount,
                    ClosePrice = posItem.ClosePrice
                };
                list.Add(calcItem);
            }

            return list;
        }

        /// <summary>
        /// 查询委托回报。
        /// </summary>
        /// <returns></returns>
        private List<OrderBookCalcItem> QueryOrderBooks()
        {
            USeOrderDriver orderDriver = USeManager.Instance.OrderDriver;
            List<USeOrderBook> orderBookList = orderDriver.QueryOrderBooks();

            List<OrderBookCalcItem> list = new List<OrderBookCalcItem>();
            if (orderBookList == null) return list;
            foreach (USeOrderBook orderBookItem in orderBookList)
            {
                USeInstrumentDetail detail = orderDriver.QueryInstrumentDetail(orderBookItem.Instrument);
                USeMargin productMargin = orderDriver.QueryInstrumentMargin(orderBookItem.Instrument);
                USeFee productFee = orderDriver.QueryInstrumentFee(orderBookItem.Instrument);
                OrderBookCalcItem calcItem = new OrderBookCalcItem() {
                    InstrumentDetail = detail,
                    MarginRate = productMargin,
                    FeeRate = productFee,

                    OrderNum = orderBookItem.OrderNum,
                    Account = orderBookItem.Account,
                    Instrument = orderBookItem.Instrument,
                    OrderQty = orderBookItem.OrderQty,
                    OrderPrice = orderBookItem.OrderPrice,
                    TradeQty = orderBookItem.TradeQty,
                    TradeAmount = orderBookItem.TradeAmount,
                    TradePrice = orderBookItem.TradePrice,
                    TradeFee = orderBookItem.TradeFee,
                    OrderStatus = orderBookItem.OrderStatus,
                    CancelQty = orderBookItem.CancelQty,
                    OrderSide = orderBookItem.OrderSide,
                    OffsetType = orderBookItem.OffsetType,
                    Memo = orderBookItem.Memo,
                    OrderTime = orderBookItem.OrderTime
                };
                list.Add(calcItem);
            }

            return list;
        }

        /// <summary>
        /// 查询成交回报。
        /// </summary>
        /// <returns></returns>
        private List<TradeBookCalcItem> QueryTradeBooks()
        {
            USeOrderDriver orderDriver = USeManager.Instance.OrderDriver;
            List<USeTradeBook> tradeBookList = orderDriver.QueryTradeBooks();

            List<TradeBookCalcItem> list = new List<TradeBookCalcItem>();

            if (tradeBookList == null) return list;
            foreach (USeTradeBook tradeItem in tradeBookList)
            {
                USeInstrumentDetail detail = orderDriver.QueryInstrumentDetail(tradeItem.Instrument);
                //USeMargin productMargin = orderDriver.QueryInstrumentMargin(tradeItem.Instrument);

                TradeBookCalcItem calcItem = new TradeBookCalcItem() {
                    InstrumentDetail = detail,

                    TradeNum = tradeItem.TradeNum,
                    Instrument = tradeItem.Instrument,
                    OrderNum = tradeItem.OrderNum,
                    OrderSide = tradeItem.OrderSide,
                    OffsetType = tradeItem.OffsetType,
                    Price = tradeItem.Price,
                    Qty = tradeItem.Qty,
                    Amount = tradeItem.Amount,
                    Fee = tradeItem.Fee,
                    TradeTime = tradeItem.TradeTime,
                    Account = tradeItem.Account
                };
                list.Add(calcItem);
            }

            return list;
        }
        #endregion

        /// <summary>
        /// 计算帐户数据。
        /// </summary>
        private USeFundDetail CalculateFundInfo()
        {
            USeOrderDriver orderDriver = USeManager.Instance.OrderDriver;
            USeQuoteDriver quoteDriver = USeManager.Instance.QuoteDriver;
            try
            {
                List<TradeBookCalcItem> tradeBookList = QueryTradeBooks(); ;
                List<PositionDetailCalcItem> tradeAccountPosList = QueryPositonDetails();
                List<OrderBookCalcItem> orderBookList = QueryOrderBooks();
                USeFund accountInfo = orderDriver.QueryFundInfo();

                //if (tradeBookList == null || tradeAccountPosList == null || orderBookList == null || accountInfo == null) return null;

                #region 读取
                decimal preBalance = accountInfo.PreBalance; //上日客户权益（读取）即上日结算准备金
                decimal preCredit = accountInfo.PreCredit; // 上日信用额度
                decimal preMortgage = accountInfo.PreMortgage; // 上次质押金额
                decimal mortgage = accountInfo.Mortgage; // 质押金额
                decimal withDraw = accountInfo.WithDraw; // 今日出金
                decimal deposit = accountInfo.Deposit; // 今日入金
                decimal deliveryMargin = accountInfo.DeliveryMargin;//交割保证金
                #endregion 读取

                // 静态权益 =  上日结存 - 上次信用额度 - 上次质押金额 
                //           + 质押金额(可能有延时)- 今日出金(可能有延时) + 今日入金(可能有延时)
                decimal staticBenefit = preBalance - preCredit - preMortgage + mortgage - withDraw + deposit;

                #region 成交回报推算
                // 手续费 = SUM(成交回报手续费)
                decimal tradeFee = tradeBookList.Sum(t => t.Fee);

                // 平历史仓盈亏 = SUM((平仓价 - 上日结算价) × 多头合约平仓量 × 合约乘数+ (上日结算价-平仓价)× 空头合约平仓量 × 合约乘数))
                // 平当日仓盈亏 = SUM((平仓价 - 开仓价)× 多头合约持仓量 × 合约乘数 + (开仓价-平仓价）× 空头合约平仓量 × 合约乘数）
                // 历史持仓盈亏 = SUM((最新价 - 上日结算价) × 多头合约持仓量× 合约乘数 + (上日结算价-最新价)× 空头合约持仓量 × 合约乘数)
                // 当日持仓盈亏 = SUM((最新价 - 开仓价) × 多头合约持仓量 × 合约乘数 + (开仓价-最新价) × 空头合约持仓量 × 合约乘数)
                // 历史持仓保证金 = SUM(上日结算价 × 合约乘数 × 持仓手数 × 保证金率)
                // 当日持仓保证金 = SUM(开仓价×合约乘数×持仓手数×保证金率)

                decimal closeProfit = 0m;
                decimal holdProfit = 0m;
                decimal holdMargin = 0m;
                decimal frozonMargin = 0m;
                decimal frozonFee = 0m;

                #region 计算持仓盈亏
                holdProfit = tradeAccountPosList.Sum((p) => CalcHoldingProfit(p));
                closeProfit = tradeAccountPosList.Sum((p) => CalcCloseProfit(p));
                #endregion

                #region 计算持仓占用保证金
                List<MarginCalcResultByProductAndDirection> holdMarginList = (tradeAccountPosList.Where(p => p.RemainQty > 0)
                    .GroupBy(p => new { p.InstrumentDetail.Varieties, p.Instrument.Market, p.Direction })
                    .Select(g => new MarginCalcResultByProductAndDirection {
                        Product = g.Key.Varieties,
                        Market = g.Key.Market,
                        Direction = g.Key.Direction,
                        HoldMargin = g.Sum((i) => CalcHoldingPositonMargin(i)),
                    })).ToList();

                List<MarginCalcResultByProductAndDirection> fronzonMarginList = (orderBookList.Where(o => (o.IsFinish == false && o.OffsetType == USeOffsetType.Open))
                    .GroupBy(o => new { o.InstrumentDetail.Varieties, o.Instrument.Market, o.Direction })
                    .Select(g => new MarginCalcResultByProductAndDirection {
                        Product = g.Key.Varieties,
                        Market = g.Key.Market,
                        Direction = g.Key.Direction,
                        HoldMargin = g.Sum((i) => CalcOrderFronzonMargin(i))
                    })).ToList();

                List<MarginCalcResultByProduct> holdProductMargin = (holdMarginList.GroupBy(p => new { p.Product, p.Market })
                    .Select(g => new MarginCalcResultByProduct {
                        Product = g.Key.Product,
                        Market = g.Key.Market,
                        HoldLongMargin = g.Where(i => i.Direction == USeDirection.Long).Sum(i => i.HoldMargin),
                        HoldShortMargin = g.Where(i => i.Direction == USeDirection.Short).Sum(i => i.HoldMargin)
                    })).ToList();

                List<MarginCalcResultByProduct> fronzonProductMargin = (fronzonMarginList.GroupBy(p => new { p.Product, p.Market })
                    .Select(g => new MarginCalcResultByProduct {
                        Product = g.Key.Product,
                        Market = g.Key.Market,
                        FrozonLongMargin = g.Where(i => i.Direction == USeDirection.Long).Sum(i => i.HoldMargin),
                        FrozonShortMargin = g.Where(i => i.Direction == USeDirection.Short).Sum(i => i.HoldMargin)
                    })).ToList();

                List<MarginCalcResultByProduct> marginList = new List<MarginCalcResultByProduct>();
                marginList.AddRange(holdProductMargin);
                marginList.AddRange(fronzonProductMargin);

                holdMargin = CalculateHoldMargin(marginList);
                frozonMargin = CalculateFronzonMargin(marginList);
                frozonFee = orderBookList.Where(o => o.IsFinish == false).Sum((i) => CalcOrderFronzonFee(i));
                #endregion

                #endregion

                // 动态权益 = 静态权益 + 持仓盈亏 + 平仓盈亏 - 手续费（取成交回报手续费）
                decimal dynamicBenefit = staticBenefit + holdProfit + closeProfit - tradeFee;

                #region 委托回报推算
                // 买冻结保证金 = SUM(委托价格 * 合约乘数 * 委托手数 * 多头保证金率)
                // 卖冻结保证金 = SUM(委托价格 * 合约乘数 * 委托手数 * 空头保证金率)
                // 买冻结手续费	= SUM(委托价格 * 合约乘数 * 委托手数 * 期货多头手续费率)
                // 卖冻结手续费 = SUM(委托价格 * 合约乘数 * 委托手数 * 期货空头手续费率)
                decimal frozon = frozonMargin + frozonFee;
                #endregion

                decimal available = 0m;
                // 可用资金(当持仓盈亏>=0时) = 动态权益 - 持仓盈亏 – 占用保证金 – 下单冻结 - 交割保证金
                //[yangming]新版本好似可用资金要减去平仓盈亏

                // 可用资金(当持仓盈亏 <0时) = 动态权益 – 占用保证金 – 下单冻结 - 交割保证金
                available = dynamicBenefit - holdMargin - frozon - deliveryMargin;

                if (holdProfit > 0)
                {
                    available = available - holdProfit;
                }
                if (closeProfit > 0)
                {
                    available = available - closeProfit;
                }

                //风险度 = (占用保证金 + 交割保证金 + 买冻结保证金 + 卖冻结保证金) / 动态权益
                //decimal risk = decimal.Divide((holdMargin + deliveryMargin + frozonMargin),(dynamicBenefit));
                decimal risk = decimal.Divide(holdMargin, (dynamicBenefit));
                decimal preferCash = available;
                if (tradeBookList.Count != 0 || tradeAccountPosList.Count != 0)
                {
                    preferCash = (decimal)(preferCash * 7 / 10);
                }

                USeFundDetail fundDetail = new USeFundDetail();
                fundDetail.AccountID = string.Empty;
                fundDetail.Available = available;
                fundDetail.Deposit = deposit;
                fundDetail.Mortgage = mortgage;
                fundDetail.PreBalance = preBalance;
                fundDetail.PreCredit = preCredit;
                fundDetail.PreMortgage = preMortgage;
                fundDetail.WithDraw = withDraw;
                fundDetail.StaticBenefit = staticBenefit;
                fundDetail.CloseProfit = closeProfit;
                fundDetail.TradeFee = tradeFee;
                fundDetail.HoldProfit = holdProfit;
                fundDetail.HoldMargin = holdMargin;
                fundDetail.DynamicBenefit = dynamicBenefit;
                fundDetail.FrozonMargin = frozonMargin;
                fundDetail.FrozonFee = frozonFee;
                fundDetail.Fronzon = frozon;
                fundDetail.Risk = risk;
                fundDetail.PreferCash = preferCash;
                return fundDetail;
            }
            catch (Exception ex)
            {
                //Debug.Assert(false, ex.Message);
                return null;
            }
        }

        private USeFundDetail CalculFoundBeforeTrade()
        {
            USeOrderDriver orderDriver = USeManager.Instance.OrderDriver;
            USeQuoteDriver quoteDriver = USeManager.Instance.QuoteDriver;

            try
            {
                List<USeTradeBook> tradeBookList = orderDriver.QueryTradeBooks();
                List<USePositionDetail> tradeAccountPosList = orderDriver.QueryPositionDetail();
                List<USeOrderBook> orderBookList = orderDriver.QueryOrderBooks();
                USeFund accountInfo = orderDriver.QueryFundInfo();
                if (tradeBookList == null || tradeAccountPosList == null || orderBookList == null || accountInfo == null) return null;

                #region 读取
                decimal preBalance = accountInfo.PreBalance; //上日客户权益（读取）即上日结算准备金
                decimal preCredit = accountInfo.PreCredit; // 上日信用额度
                decimal preMortgage = accountInfo.PreMortgage; // 上次质押金额
                decimal mortgage = accountInfo.Mortgage; // 质押金额
                decimal withDraw = accountInfo.WithDraw; // 今日出金
                decimal deposit = accountInfo.Deposit; // 今日入金
                decimal deliveryMargin = accountInfo.DeliveryMargin;//交割保证金
                #endregion 读取

                // 静态权益 =  上日结存 - 上次信用额度 - 上次质押金额 
                //           + 质押金额(可能有延时)- 今日出金(可能有延时) + 今日入金(可能有延时)
                decimal staticBenefit = preBalance - preCredit - preMortgage + mortgage - withDraw + deposit;


                #region 成交回报推算                                                           
                decimal holdHistoryMargin = 0m;
                decimal holdMargin = 0m;

                foreach (USePositionDetail posItem in tradeAccountPosList)
                {
                    USeInstrumentDetail detail = orderDriver.QueryInstrumentDetail(posItem.Instrument);
                    int volumeMultiple = detail.VolumeMultiple;
                    USeMargin productMargin = orderDriver.QueryInstrumentMargin(posItem.Instrument);
                    USeMarketData marketData = quoteDriver.Query(posItem.Instrument);

                    if (posItem.RemainQty > 0) // 有持仓
                    {
                        if (posItem.PositionType == USePositionType.Yestorday && posItem.Direction == USeDirection.Long)
                        {
                            // 历史多头持仓保证金 = 上日结算价 × 合约乘数 × 持仓手数 × 交易所多头保证金率              
                            decimal margin = (marketData.PreSettlementPrice * volumeMultiple * posItem.RemainQty * productMargin.BrokerLongMarginRatioByMoney) +
                                             (posItem.RemainQty * productMargin.BrokerLongMarginRatioByVolume);
                            holdHistoryMargin += margin;
                        }
                        else if (posItem.PositionType == USePositionType.Yestorday && posItem.Direction == USeDirection.Short)
                        {
                            decimal margin = (marketData.PreSettlementPrice * volumeMultiple * posItem.RemainQty * productMargin.BrokerShortMarginRatioByMoney) +
                                             (posItem.RemainQty * productMargin.BrokerLongMarginRatioByVolume);
                            holdHistoryMargin += margin;
                        }
                        else
                        {
                            Debug.Assert(false);
                        }
                    }
                }
                holdMargin = holdHistoryMargin;
                #endregion

                // 动态权益 = 静态权益
                decimal dynamicBenefit = staticBenefit;
                decimal frozon = 0;
                decimal closeProfit = 0m;
                decimal holdProfit = 0m;
                decimal tradeFee = 0m;

                decimal available = dynamicBenefit - holdMargin - frozon - deliveryMargin;

                //风险度 = (占用保证金 + 交割保证金) / 动态权益
                decimal risk = decimal.Divide((holdMargin + deliveryMargin), dynamicBenefit);
                decimal preferCash = available;
                if (tradeBookList.Count != 0 || tradeAccountPosList.Count != 0)
                {
                    preferCash = (decimal)(preferCash * 7 / 10);
                }

                USeFundDetail fundDetail = new USeFundDetail();
                fundDetail.AccountID = string.Empty;
                fundDetail.Available = available;
                fundDetail.Deposit = deposit;
                fundDetail.Mortgage = mortgage;
                fundDetail.PreBalance = preBalance;
                fundDetail.PreCredit = preCredit;
                fundDetail.PreMortgage = preMortgage;
                fundDetail.WithDraw = withDraw;
                fundDetail.StaticBenefit = staticBenefit;
                fundDetail.CloseProfit = closeProfit;
                fundDetail.TradeFee = tradeFee;
                fundDetail.HoldProfit = holdProfit;
                fundDetail.HoldMargin = holdMargin;
                fundDetail.DynamicBenefit = dynamicBenefit;
                fundDetail.FrozonMargin = 0; fundDetail.AccountID = string.Empty;
                fundDetail.Available = available;
                fundDetail.Deposit = deposit;
                fundDetail.Mortgage = mortgage;
                fundDetail.PreBalance = preBalance;
                fundDetail.PreCredit = preCredit;
                fundDetail.PreMortgage = preMortgage;
                fundDetail.WithDraw = withDraw;
                fundDetail.StaticBenefit = staticBenefit;
                fundDetail.CloseProfit = closeProfit;
                fundDetail.TradeFee = tradeFee;
                fundDetail.HoldProfit = holdProfit;
                fundDetail.HoldMargin = holdMargin;
                fundDetail.DynamicBenefit = dynamicBenefit;
                fundDetail.FrozonMargin = 0;
                fundDetail.FrozonFee = 0;
                fundDetail.Fronzon = frozon;
                fundDetail.Risk = risk;
                fundDetail.PreferCash = preferCash;
                return fundDetail;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                return null;
            }
        }

        #region 计算方法
        /// <summary>
        /// 计算持仓保证金。
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private decimal CalculateHoldMargin(List<MarginCalcResultByProduct> list)
        {
            decimal holdMargin = 0m;
            foreach (MarginCalcResultByProduct item in list)
            {
                if (item.Market == USeMarket.SHFE)
                {
                    holdMargin += Math.Max(item.HoldLongMargin, item.HoldShortMargin);
                }
                else
                {
                    holdMargin += (item.HoldLongMargin + item.HoldShortMargin);
                }
            }
            return holdMargin;
        }

        /// <summary>
        /// 计算账户冻结保证金。
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private decimal CalculateFronzonMargin(List<MarginCalcResultByProduct> list)
        {
            decimal fronzonMargin = 0m;
            foreach (MarginCalcResultByProduct item in list)
            {
                if (item.Market == USeMarket.SHFE)
                {
                    decimal holdMargin = Math.Max(item.HoldLongMargin, item.HoldShortMargin);
                    decimal sumMargin = Math.Max((item.HoldLongMargin + item.FrozonLongMargin), (item.HoldShortMargin + item.FrozonShortMargin));
                    Debug.Assert(sumMargin >= holdMargin);
                    fronzonMargin += (sumMargin - holdMargin);
                }
                else
                {
                    fronzonMargin += (item.FrozonLongMargin + item.FrozonShortMargin);
                }
            }
            return fronzonMargin;
        }

        /// <summary>
        /// 计算持仓损益。
        /// </summary>
        /// <param name="posItem"></param>
        /// <returns></returns>
        private decimal CalcHoldingProfit(PositionDetailCalcItem posItem)
        {
            int volumeMultiple = posItem.InstrumentDetail.VolumeMultiple;
            USeMarketData marketData = posItem.MarketData;

            decimal holdProfit = 0m;// 持仓盈亏

            if (posItem.RemainQty > 0) // 有持仓
            {
                if (posItem.PositionType == USePositionType.Today && posItem.Direction == USeDirection.Long)
                {
                    // 当日多头持仓盈亏 = (最新价 - 开仓价) × 多头合约持仓量 × 合约乘数
                    // 当日多头持仓保证金 = 开仓价 × 合约乘数 × 持仓手数 × 多头保证金率
                    if (marketData.LastPrice > 0)
                    {
                        holdProfit += (marketData.LastPrice - posItem.OpenPrice) * posItem.RemainQty * volumeMultiple;
                    }
                }
                else if (posItem.PositionType == USePositionType.Today && posItem.Direction == USeDirection.Short)
                {
                    // 当日空头持仓盈亏 = (开仓价 - 最新价) × 空头合约持仓量 × 合约乘数
                    // 当日持仓保证金 = 开仓价 × 合约乘数 × 持仓手数 × 空头保证金率
                    if (marketData.LastPrice > 0)
                    {
                        holdProfit += (posItem.OpenPrice - marketData.LastPrice) * posItem.RemainQty * volumeMultiple;
                    }
                }
                else if (posItem.PositionType == USePositionType.Yestorday && posItem.Direction == USeDirection.Long)
                {
                    // 历史多头持仓盈亏 = (最新价 - 上日结算价) × 多头合约持仓量× 合约乘数
                    // 历史多头持仓保证金 = 上日结算价 × 合约乘数 × 持仓手数 × 交易所多头保证金率
                    if (marketData.LastPrice > 0)
                    {
                        holdProfit += (marketData.LastPrice - marketData.PreSettlementPrice) * posItem.RemainQty * volumeMultiple;
                    }
                }
                else if (posItem.PositionType == USePositionType.Yestorday && posItem.Direction == USeDirection.Short)
                {
                    // 历史空头持仓盈亏 = (上日结算价-最新价)× 空头合约持仓量 × 合约乘数
                    // 历史空头持仓保证金 = 上日结算价 × 合约乘数 × 持仓手数 × 交易所空头保证金率
                    if (marketData.LastPrice > 0)
                    {
                        holdProfit += (marketData.PreSettlementPrice - marketData.LastPrice) * posItem.RemainQty * volumeMultiple;
                    }
                }
                else
                {
                    Debug.Assert(false);
                }
            }
            return holdProfit;
        }

        /// <summary>
        /// 计算平仓损益。
        /// </summary>
        /// <param name="posItem"></param>
        /// <returns></returns>
        private decimal CalcCloseProfit(PositionDetailCalcItem posItem)
        {
            int volumeMultiple = posItem.InstrumentDetail.VolumeMultiple;
            USeMarketData marketData = posItem.MarketData;

            decimal closeProfit = 0m;// 平仓盈亏

            if (posItem.CloseQty > 0) // 有平仓
            {
                if (posItem.PositionType == USePositionType.Today && posItem.Direction == USeDirection.Long)
                {
                    // 平当日多头仓盈亏 = (平仓价 - 开仓价）× 多头合约持仓量 × 合约乘数 
                    closeProfit += ((posItem.ClosePrice - posItem.OpenPrice) * posItem.CloseQty * volumeMultiple);
                }
                else if (posItem.PositionType == USePositionType.Today && posItem.Direction == USeDirection.Short)
                {
                    // 平当日空头仓盈亏 = (开仓价 - 平仓价）× 空头合约平仓量 × 合约乘数
                    closeProfit += ((posItem.OpenPrice - posItem.ClosePrice) * posItem.CloseQty * volumeMultiple);
                }
                else if (posItem.PositionType == USePositionType.Yestorday && posItem.Direction == USeDirection.Long)
                {
                    // 平历史仓多头盈亏 = (平仓价 - 上日结算价) × 多头合约平仓量 × 合约乘数
                    closeProfit += ((posItem.ClosePrice - posItem.OpenPrice) * posItem.CloseQty * volumeMultiple);
                }
                else if (posItem.PositionType == USePositionType.Yestorday && posItem.Direction == USeDirection.Short)
                {
                    // 平历史仓空头盈亏 = (上日结算价 - 平仓价) × 空头合约平仓量 × 合约乘数
                    closeProfit += ((posItem.OpenPrice - posItem.ClosePrice) * posItem.CloseQty * volumeMultiple);
                }
                else
                {
                    Debug.Assert(false);
                }
            }

            return closeProfit;
        }

        /// <summary>
        /// 计算持仓保证金。
        /// </summary>
        /// <param name="posItem"></param>
        /// <returns></returns>
        private decimal CalcHoldingPositonMargin(PositionDetailCalcItem posItem)
        {
            int volumeMultiple = posItem.InstrumentDetail.VolumeMultiple;
            USeMarketData marketData = posItem.MarketData;

            decimal margin = 0m;

            if (posItem.RemainQty > 0) // 有持仓
            {
                if (posItem.PositionType == USePositionType.Today && posItem.Direction == USeDirection.Long)
                {
                    margin = posItem.OpenPrice * volumeMultiple * posItem.RemainQty * posItem.Margin.BrokerLongMarginRatioByMoney +
                                     posItem.RemainQty * posItem.Margin.BrokerLongMarginRatioByVolume;
                    //holdTodayMargin += margin;
                }
                else if (posItem.PositionType == USePositionType.Today && posItem.Direction == USeDirection.Short)
                {
                    margin = (posItem.OpenPrice * volumeMultiple * posItem.RemainQty * posItem.Margin.BrokerShortMarginRatioByMoney) +
                                     (posItem.RemainQty * posItem.Margin.BrokerShortMarginRatioByVolume);
                    //holdTodayMargin += margin;
                }
                else if (posItem.PositionType == USePositionType.Yestorday && posItem.Direction == USeDirection.Long)
                {
                    margin = (marketData.PreSettlementPrice * volumeMultiple * posItem.RemainQty * posItem.Margin.BrokerLongMarginRatioByMoney) +
                                     (posItem.RemainQty * posItem.Margin.BrokerLongMarginRatioByVolume);
                    //holdHistoryMargin += margin;
                }
                else if (posItem.PositionType == USePositionType.Yestorday && posItem.Direction == USeDirection.Short)
                {
                    margin = (marketData.PreSettlementPrice * volumeMultiple * posItem.RemainQty * posItem.Margin.BrokerShortMarginRatioByMoney) +
                                     (posItem.RemainQty * posItem.Margin.BrokerLongMarginRatioByVolume);
                    //holdHistoryMargin += margin;
                }
                else
                {
                    Debug.Assert(false);
                }
            }
            return margin;
        }

        /// <summary>
        /// 计算冻结保证金。
        /// </summary>
        /// <param name="orderBook"></param>
        /// <returns></returns>
        private decimal CalcOrderFronzonMargin(OrderBookCalcItem orderBook)
        {
            if (orderBook.IsFinish) return 0m; ;
            if (orderBook.OffsetType != USeOffsetType.Open) return 0m;

            int volumeMultiple = orderBook.InstrumentDetail.VolumeMultiple;
            USeMargin productMargin = orderBook.MarginRate;

            decimal frozonMargin = 0;
            if (orderBook.OrderSide == USeOrderSide.Buy)
            {
                frozonMargin = ((orderBook.OrderQty - orderBook.TradeQty) * productMargin.BrokerLongMarginRatioByVolume) +
                                   (orderBook.OrderPrice * (orderBook.OrderQty - orderBook.TradeQty) * volumeMultiple * productMargin.BrokerLongMarginRatioByMoney);
            }
            else if (orderBook.OrderSide == USeOrderSide.Sell)
            {
                frozonMargin += ((orderBook.OrderQty - orderBook.TradeQty) * productMargin.BrokerShortMarginRatioByVolume) +
                                    (orderBook.OrderPrice * (orderBook.OrderQty - orderBook.TradeQty) * volumeMultiple * productMargin.BrokerShortMarginRatioByMoney);
            }
            else
            {
                Debug.Assert(false);
            }

            return frozonMargin;
        }

        /// <summary>
        /// 计算冻结手续费。
        /// </summary>
        /// <param name="orderBook"></param>
        /// <returns></returns>
        private decimal CalcOrderFronzonFee(OrderBookCalcItem orderBook)
        {
            if (orderBook.IsFinish) return 0m; ;
            if (orderBook.OffsetType != USeOffsetType.Open) return 0m;

            int volumeMultiple = orderBook.InstrumentDetail.VolumeMultiple;
            USeFee productFee = orderBook.FeeRate;

            decimal frozonFee = 0m;
            if (orderBook.OrderSide == USeOrderSide.Buy)
            {
                frozonFee += ((orderBook.OrderQty - orderBook.TradeQty) * productFee.OpenRatioByVolume) +
                                (orderBook.OrderPrice * (orderBook.OrderQty - orderBook.TradeQty) * volumeMultiple * productFee.OpenRatioByMoney);
            }
            else if (orderBook.OrderSide == USeOrderSide.Sell)
            {
                frozonFee += ((orderBook.OrderQty - orderBook.TradeQty) * productFee.OpenRatioByVolume) +
                                 (orderBook.OrderPrice * (orderBook.OrderQty - orderBook.TradeQty) * volumeMultiple * productFee.OpenRatioByMoney);
            }
            else
            {
                Debug.Assert(false);
            }

            return frozonFee;
        }
        #endregion

       

        #region private class
        /// <summary>
        /// 持仓明细。
        /// </summary>
        private class PositionDetailCalcItem : USePositionDetail
        {
            #region property
            /// <summary>
            /// 合约明细。
            /// </summary>
            public USeInstrumentDetail InstrumentDetail { get; set; }

            /// <summary>
            /// 行情。
            /// </summary>
            public USeMarketData MarketData { get; set; }

            /// <summary>
            /// 保证金。
            /// </summary>
            public USeMargin Margin { get; set; }
            #endregion //
        }

        private class OrderBookCalcItem : USeOrderBook
        {
            #region property
            /// <summary>
            /// 合约明细。
            /// </summary>
            public USeInstrumentDetail InstrumentDetail { get; set; }

            /// <summary>
            /// 保证金。
            /// </summary>
            public USeMargin MarginRate { get; set; }

            public USeFee FeeRate { get; set; }

            public USeDirection Direction
            {
                get
                {
                    if (this.OffsetType == USeOffsetType.Open)
                    {
                        if (this.OrderSide == USeOrderSide.Buy)
                        {
                            return USeDirection.Long;
                        }
                        else
                        {
                            return USeDirection.Short;
                        }
                    }
                    else
                    {
                        if (this.OrderSide == USeOrderSide.Buy)
                        {
                            return USeDirection.Short;
                        }
                        else
                        {
                            return USeDirection.Long;
                        }
                    }
                }
            }
            #endregion // property
        }

        private class TradeBookCalcItem : USeTradeBook
        {
            #region property
            /// <summary>
            /// 合约明细。
            /// </summary>
            public USeInstrumentDetail InstrumentDetail { get; set; }
            #endregion // property
        }

        private class MarginCalcResultByProductAndDirection
        {
            public string Product { get; set; }

            public USeMarket Market { get; set; }

            public USeDirection Direction { get; set; }

            public decimal HoldMargin { get; set; }
        }

        private class MarginCalcResultByProduct
        {
            public string Product { get; set; }

            public USeMarket Market { get; set; }

            public decimal HoldLongMargin { get; set; }

            public decimal HoldShortMargin { get; set; }

            public decimal FrozonLongMargin { get; set; }

            public decimal FrozonShortMargin { get; set; }
        }

        #endregion
    }
}
