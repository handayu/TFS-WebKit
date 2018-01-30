using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;
using System.Diagnostics;
using USeFuturesSpirit.Arbitrage;

namespace USeFuturesSpirit
{
    public partial class ArbitrageOrderCreateControl : USeUserControl
    {
        #region member
        private string m_productId = string.Empty; // 当前品种

        private USeInstrument m_buyInstrument = null;
        private USeInstrument m_sellInstrument = null;

        private USeMarketData m_buyMarketData = null;  // 买入合约行情
        private USeMarketData m_sellMarketData = null; // 卖出合约行情
        #endregion

        #region construction
        public ArbitrageOrderCreateControl()
        {
            InitializeComponent();
        }
        #endregion

        private void CombinationOrderPanelControl_Load(object sender, EventArgs e)
        {
            SetControlStyle();
            ClearView();
        }

        #region public methods
        /// <summary>
        /// 更改产品。
        /// </summary>
        /// <param name="product">产品名称。</param>
        public void ChangeProduct(string product)
        {
            if (string.IsNullOrEmpty(product) || m_productId == product) return;

            this.cbxBuyInstrument.Items.Clear();
            this.cbxSellInstrument.Items.Clear();

            m_buyInstrument = null;
            m_sellInstrument = null;
            m_buyMarketData = null;
            m_sellMarketData = null;
            m_productId = product;

            SetNoticeInfo();
            try
            {
                List<USeInstrumentDetail> instrumentDetailList = USeManager.Instance.OrderDriver.QueryInstrumentDetail(product);
                if (instrumentDetailList == null) return;

                List<USeInstrument> instrumentList = (from e in instrumentDetailList
                                                      orderby e.Instrument.InstrumentCode
                                                      select e.Instrument).ToList();
                USeInstrument emptyInstrument = new USeInstrument("", "选择合约", USeMarket.Unknown);
                this.cbxBuyInstrument.Items.Add(emptyInstrument);
                this.cbxSellInstrument.Items.Add(emptyInstrument);

                foreach (USeInstrument instrument in instrumentList)
                {
                    this.cbxBuyInstrument.Items.Add(instrument);
                    this.cbxSellInstrument.Items.Add(instrument);
                }

                this.cbxBuyInstrument.SelectedIndex = 0;
                this.cbxSellInstrument.SelectedIndex = 0;

#if DEBUG
                if (this.cbxBuyInstrument.Items.Count > 2)
                {
                    this.cbxBuyInstrument.SelectedIndex = 1;
                }
                if (this.cbxSellInstrument.Items.Count > 3)
                {
                    this.cbxSellInstrument.SelectedIndex = 2;
                }
#endif
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize()
        {
            USeManager.Instance.QuoteDriver.OnMarketDataChanged += QuoteDriver_OnMarketDataChanged;
        }
        #endregion

        #region UI界面数据获取
        /// <summary>
        /// 买入合约价格类型。
        /// </summary>
        /// <returns></returns>
        private ArbitrageOrderPriceType GetBuyOrderPriceTypeFromUI()
        {
            ArbitrageOrderPriceType buyOrderPriceType = ArbitrageOrderPriceType.Unknown;
            if (this.rbnBuyOrderPriceType_LastPrice.Checked)
            {
                buyOrderPriceType = ArbitrageOrderPriceType.LastPrice;
            }
            else if (this.rbnBuyOrderPriceType_OpponentPrice.Checked)
            {
                buyOrderPriceType = ArbitrageOrderPriceType.OpponentPrice;
            }
            else if (this.rbnBuyOrderPriceType_QueuePrice.Checked)
            {
                buyOrderPriceType = ArbitrageOrderPriceType.QueuePrice;
            }
            else
            {
                Debug.Assert(false);
            }
            return buyOrderPriceType;
        }

        /// <summary>
        /// 卖出合约价格类型。
        /// </summary>
        /// <returns></returns>
        private ArbitrageOrderPriceType GetSellOrderPriceTypeFromUI()
        {
            ArbitrageOrderPriceType sellOrderPriceType = ArbitrageOrderPriceType.Unknown;
            if (this.rbnSellOrderPriceType_LastPrice.Checked)
            {
                sellOrderPriceType = ArbitrageOrderPriceType.LastPrice;
            }
            else if (this.rbnSellOrderPriceType_OpponentPrice.Checked)
            {
                sellOrderPriceType = ArbitrageOrderPriceType.OpponentPrice;
            }
            else if (this.rbnSellOrderPriceType_QueuePrice.Checked)
            {
                sellOrderPriceType = ArbitrageOrderPriceType.QueuePrice;
            }
            else
            {
                Debug.Assert(false);
            }
            return sellOrderPriceType;
        }

        /// <summary>
        /// 获取优先买卖方向。
        /// </summary>
        /// <returns></returns>
        private USeOrderSide GetPreferentialSideFromUI()
        {
            USeOrderSide preferentialSide = USeOrderSide.Buy;
            if (this.rbnPreferentialSide_Buy.Checked)
            {
                preferentialSide = USeOrderSide.Buy;
            }
            else if (this.rbnPreferentialSide_Sell.Checked)
            {
                preferentialSide = USeOrderSide.Sell;
            }
            else
            {
                Debug.Assert(false);
            }
            return preferentialSide;
        }

        /// <summary>
        /// 获取价差监控方向。
        /// </summary>
        /// <returns></returns>
        private PriceSpreadSide GetPriceSpreadSideFromUI()
        {
            PriceSpreadSide priceSpreadSide = PriceSpreadSide.Unknown;
            if (this.rbnPriceSpreadSide_Greater.Checked)
            {
                priceSpreadSide = PriceSpreadSide.GreaterOrEqual;
            }
            else if (this.rbnPriceSpreadSide_Less.Checked)
            {
                priceSpreadSide = PriceSpreadSide.LessOrEqual;
            }
            else
            {
                Debug.Assert(false);
            }
            return priceSpreadSide;
        }

        /// <summary>
        /// 获取买入合约。
        /// </summary>
        /// <returns></returns>
        private USeInstrument GetBuyInstrumentFromUI()
        {
            USeInstrument instrument = this.cbxBuyInstrument.SelectedItem as USeInstrument;
            if (instrument != null && string.IsNullOrEmpty(instrument.InstrumentCode))
            {
                instrument = null;
            }

            return instrument;
        }

        /// <summary>
        /// 获取卖出合约。
        /// </summary>
        /// <returns></returns>
        private USeInstrument GetSellInstrumentFromUI()
        {
            USeInstrument instrument = this.cbxSellInstrument.SelectedItem as USeInstrument;
            if (instrument != null && string.IsNullOrEmpty(instrument.InstrumentCode))
            {
                instrument = null;
            }

            return instrument;
        }
        #endregion

        #region private methods
        private void ClearView()
        {
            this.lblBuyInstrumentPrice.Text = "---";
            this.lblSellInstrumentPrice.Text = "---";
            this.lblPriceSpread.Text = "---";
        }
       
        /// <summary>
        /// 是否关心合约。
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        private bool IsMyCareInstrument(USeInstrument instrument)
        {
            if (m_buyInstrument != null && m_buyInstrument == instrument)
            {
                return true;
            }

            if (m_sellInstrument != null && m_sellInstrument == instrument)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 更新行情。
        /// </summary>
        /// <param name="marketData">行情数据。</param>
        private void UpdateMarketData(USeMarketData marketData)
        {
            if (marketData == null) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<USeMarketData>(UpdateMarketData), marketData);
                return;
            }

            if (m_buyInstrument != null && m_buyInstrument.Equals(marketData.Instrument))
            {
                m_buyMarketData = marketData;
            }
            if (m_sellInstrument != null && m_sellInstrument.Equals(marketData.Instrument))
            {
                m_sellMarketData = marketData;
            }

            SetPriceControlValue();
        }

        /// <summary>
        /// 获取下单价格。
        /// </summary>
        /// <param name="marketData"></param>
        /// <param name="orderPriceType"></param>
        /// <param name="orderSide"></param>
        /// <returns></returns>
        private decimal GetOrderPrice(USeMarketData marketData, ArbitrageOrderPriceType orderPriceType, USeOrderSide orderSide)
        {
            if (marketData == null) return 0m;
            if (orderPriceType == ArbitrageOrderPriceType.Unknown) return 0m;
            switch (orderPriceType)
            {
                case ArbitrageOrderPriceType.LastPrice: return marketData.LastPrice;
                case ArbitrageOrderPriceType.OpponentPrice:
                    if (orderSide == USeOrderSide.Buy)
                    {
                        return marketData.AskPrice;
                    }
                    else
                    {
                        Debug.Assert(orderSide == USeOrderSide.Sell);
                        return marketData.BidPrice;
                    }
                case ArbitrageOrderPriceType.QueuePrice:
                    {
                        if (orderSide == USeOrderSide.Buy)
                        {
                            return marketData.BidPrice;
                        }
                        else
                        {
                            return marketData.AskPrice;
                        }
                    }
                default:
                    Debug.Assert(false);
                    return 0m;
            }
        }
        #endregion

        #region 行情驱动事件
        private void QuoteDriver_OnMarketDataChanged(object sender, USeMarketDataChangedEventArgs e)
        {
            if (IsMyCareInstrument(e.MarketData.Instrument) == false)
            {
                return;
            }

            UpdateMarketData(e.MarketData);
        }
        #endregion

        #region UI更新
        private void SetNoticeInfo()
        {
            string text = string.Empty;
            if(m_productId.ToLower() == "cu")
            {
                text = "★★ cu(铜) 进入交割月后，持仓以及委托必须为5手整数倍";
            }
            else if(m_productId.ToLower() == "al")
            {
                text = "★★ al(铝) 进入交割月后，持仓以及委托必须为5手整数倍";
            }
            else if(m_productId.ToLower() == "")
            {
                text = "★★ FU(燃料油) 进入交割月后，持仓以及委托必须为10手整数倍";
            }
            this.lblNotice.Text = text;
        }
        /// <summary>
        /// 设定价格控件值。
        /// </summary>
        private void SetPriceControlValue()
        {
            decimal buyPrice = GetOrderPrice(m_buyMarketData, GetBuyOrderPriceTypeFromUI(), USeOrderSide.Buy);
            decimal sellPrice = GetOrderPrice(m_sellMarketData, GetSellOrderPriceTypeFromUI(), USeOrderSide.Sell);

            this.lblBuyInstrumentPrice.Text = buyPrice > 0m ? buyPrice.ToString() : "---";
            this.lblSellInstrumentPrice.Text = sellPrice > 0m ? sellPrice.ToString() : "---";

            if (buyPrice > 0m && sellPrice >= 0m)
            {
                decimal diffPrice = (buyPrice - sellPrice);
                this.lblPriceSpread.Text = diffPrice.ToString();
                this.lblPriceSpread.ForeColor = diffPrice >= 0m ? Color.Red : Color.Green;
            }
            else
            {
                this.lblPriceSpread.Text = "---";
            }
        }

        /// <summary>
        /// 更新空间样式。
        /// </summary>
        private void SetControlStyle()
        {
            SetRadioButtonBackColor(this.rbnPreferentialSide_Buy);
            SetRadioButtonBackColor(this.rbnPreferentialSide_Sell);

            SetRadioButtonBackColor(this.rbnBuyOrderPriceType_LastPrice);
            SetRadioButtonBackColor(this.rbnBuyOrderPriceType_OpponentPrice);
            SetRadioButtonBackColor(this.rbnBuyOrderPriceType_QueuePrice);
            SetRadioButtonBackColor(this.rbnSellOrderPriceType_LastPrice);
            SetRadioButtonBackColor(this.rbnSellOrderPriceType_OpponentPrice);
            SetRadioButtonBackColor(this.rbnSellOrderPriceType_QueuePrice);

            SetRadioButtonBackColor(this.rbnPriceSpreadSide_Greater);
            SetRadioButtonBackColor(this.rbnPriceSpreadSide_Less);
        }

        private void SetRadioButtonBackColor(RadioButton button)
        {
            if (button.Checked)
            {
                button.BackColor = Color.FromArgb(255, 135, 0);
            }
            else
            {
                button.BackColor = SystemColors.Control;
            }
        }
        #endregion

        #region UI控件事件
        private void rbnPreferentialSide_CheckedChanged(object sender, EventArgs e)
        {
            SetControlStyle();
        }

        private void rbnBuyOrderPriceType_CheckedChanged(object sender, EventArgs e)
        {
            SetControlStyle();
            SetPriceControlValue();
        }

        private void rbnSellOrderPriceType_CheckedChanged(object sender, EventArgs e)
        {
            SetControlStyle();
            SetPriceControlValue();
        }

        private void rbnPriceSpreadSide_CheckedChanged(object sender, EventArgs e)
        {
            SetControlStyle();
        }

        private void cbxBuyInstrument_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbxBuyInstrument.SelectedIndex < 0)
            {
                return;
            }

            USeInstrument buyInstrument = GetBuyInstrumentFromUI();

            if (buyInstrument == null || m_buyInstrument == buyInstrument)
            {
                return;
            }

            m_buyInstrument = buyInstrument;
            m_buyMarketData = null;
            try
            {
                USeManager.Instance.QuoteDriver.Subscribe(buyInstrument);
                USeMarketData marketData = USeManager.Instance.QuoteDriver.QuickQuery(buyInstrument);
                UpdateMarketData(marketData);
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "订阅行情失败," + ex.Message);
            }
            SetPriceControlValue();
        }

        private void cbxSellInstrument_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbxSellInstrument.SelectedIndex < 0)
            {
                //Debug.Assert(false);
                return;
            }

            USeInstrument sellInstrument = GetSellInstrumentFromUI();
            if (sellInstrument == null || m_sellInstrument == sellInstrument)
            {
                return;
            }

            m_sellInstrument = sellInstrument;
            m_sellMarketData = null;

            try
            {
                USeManager.Instance.QuoteDriver.Subscribe(sellInstrument);
                USeMarketData marketData = USeManager.Instance.QuoteDriver.QuickQuery(sellInstrument);
                UpdateMarketData(marketData);
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "订阅行情失败," + ex.Message);
            }
            SetPriceControlValue();
        }
        #endregion

        #region 开仓跟单
        /// <summary>
        /// 校验开仓参数。
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool VerifyOpenArgument(out string errorMessage)
        {
            errorMessage = string.Empty;

            USeInstrument buyInstrument = GetBuyInstrumentFromUI(); 
            if (buyInstrument == null)
            {
                errorMessage = "请选择买入合约";
                return false;
            }

            USeInstrument sellInstrument = GetSellInstrumentFromUI();
            if (sellInstrument == null)
            {
                errorMessage = "请选择卖出合约";
                return false;
            }

            if (buyInstrument.Equals(sellInstrument))
            {
                errorMessage = "买入卖出合约不能为同一合约";
                return false;
            }

            if (this.rbnPreferentialSide_Buy.Checked == false &&
                this.rbnPreferentialSide_Sell.Checked == false)
            {
                errorMessage = "请选择优先开仓合约";
                return false;
            }

            if (this.rbnBuyOrderPriceType_LastPrice.Checked == false &&
                this.rbnBuyOrderPriceType_OpponentPrice.Checked == false &&
                this.rbnBuyOrderPriceType_QueuePrice.Checked == false)
            {
                errorMessage = "请选择买入合约下单价格类型";
                return false;
            }

            if (this.rbnSellOrderPriceType_LastPrice.Checked == false &&
               this.rbnSellOrderPriceType_OpponentPrice.Checked == false &&
               this.rbnSellOrderPriceType_QueuePrice.Checked == false)
            {
                errorMessage = "请选择卖出合约下单价格类型";
                return false;
            }

            if (this.rbnPriceSpreadSide_Greater.Checked == false &&
                this.rbnPriceSpreadSide_Less.Checked == false)
            {
                errorMessage = "请选择开仓价差条件";
                return false;
            }

            //if(this.nudPriceSpreadThreshold.Value <=0)
            //{
            //    errorMessage = "请设定价差阀值";
            //    return false;
            //}

            if (((int)this.nudTotalOrderQty.Value) <= 0m)
            {
                errorMessage = "请设定单边开仓手数";
                return false;
            }

            if (((int)this.nudOrderQtyUint.Value) < 1)
            {
                errorMessage = "请设定开仓单位";
                return false;
            }
            if (((int)this.nudDifferentialUnit.Value < 0))
            {
                errorMessage = "请设定合理的仓差单位";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 评估创建套利单需要保证金。
        /// </summary>
        /// <param name="openArg"></param>
        /// <returns></returns>
        public decimal EvaluateMargin(ArbitrageOpenArgument openArg)
        {
            USeOrderDriver orderDriver = USeManager.Instance.OrderDriver;
            USeQuoteDriver quoteDriver = USeManager.Instance.QuoteDriver;

            USeInstrumentDetail buyInstrumentDetail = orderDriver.QueryInstrumentDetail(openArg.BuyInstrument);
            USeInstrumentDetail sellInstrumentDetail = orderDriver.QueryInstrumentDetail(openArg.SellInstrument);
            USeMarketData buyMarketData = quoteDriver.Query(openArg.BuyInstrument);
            USeMarketData sellMarketData = quoteDriver.Query(openArg.SellInstrument);
            USeMargin buyMarginRate = orderDriver.QueryInstrumentMargin(openArg.BuyInstrument);
            USeMargin sellMarginRate = orderDriver.QueryInstrumentMargin(openArg.SellInstrument);

            decimal buyMargin = (openArg.TotalOrderQty * buyMarginRate.BrokerLongMarginRatioByVolume) +
                               (buyMarketData.LastPrice * openArg.TotalOrderQty * buyInstrumentDetail.VolumeMultiple * buyMarginRate.BrokerLongMarginRatioByMoney);
            decimal sellMargin = (openArg.TotalOrderQty * sellMarginRate.BrokerShortMarginRatioByVolume) +
                               (sellMarketData.LastPrice * openArg.TotalOrderQty * sellInstrumentDetail.VolumeMultiple * sellMarginRate.BrokerShortMarginRatioByMoney);

            if (openArg.BuyInstrument.Market == USeMarket.SHFE && openArg.SellInstrument.Market == USeMarket.SHFE)
            {
                return Math.Max(buyMargin, sellMargin);
            }
            else
            {
                return (buyMargin + sellMargin);
            }
        }
        /// <summary>
        /// 校验保证金。
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool VerifyMargin(ArbitrageOpenArgument openArg, out string errorMessage)
        {
            errorMessage = string.Empty;
            USeOrderDriver orderDriver = USeManager.Instance.OrderDriver;
            try
            {
                OrderMarginSetting marginSetting = USeManager.Instance.SystemConfigManager.GetOrderMarginSetting();
                USeFundDetail fundDetail = orderDriver.QueryFundDetailInfo();
                if (fundDetail == null)
                {
                    errorMessage = "查询资金信息失败";
                    return false;
                }

                decimal evaluateMargin = EvaluateMargin(openArg);  // 评估当前开仓参数可能占用保证金

                decimal investorMaxUse = fundDetail.DynamicBenefit * marginSetting.MaxUseRate;  // 账户最大占用保证金
                decimal usedMargin = USeManager.Instance.AutoTraderManager.CalculatUseMargin(); // 当前套利单占用保证金

                if (investorMaxUse < usedMargin + evaluateMargin)
                {
                    errorMessage = string.Format("套利单预计占用{0}保证金,当前账户占用保证金超出设定预警阀值", evaluateMargin.ToString("#,0"));
                    return false;
                }

                if (fundDetail.Available < evaluateMargin)
                {
                    errorMessage = string.Format("套利单预计占用{0}保证金，当前账户可用余额不足", evaluateMargin.ToString("#,0"));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                errorMessage = "校验保证金失败";
                return false;
            }
        }

        private void btnOpenArbitrageOrder_Click(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            if (VerifyOpenArgument(out errorMessage) == false)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, errorMessage);
                return;
            }

            USeInstrument buyInstrument = this.cbxBuyInstrument.SelectedItem as USeInstrument;
            USeInstrument sellInstrument = this.cbxSellInstrument.SelectedItem as USeInstrument;

            ArbitrageOrderPriceType buyOrderPriceType = GetBuyOrderPriceTypeFromUI();
            ArbitrageOrderPriceType sellOrderPriceType = GetSellOrderPriceTypeFromUI();
            USeOrderSide preferentialSide = GetPreferentialSideFromUI();
            PriceSpreadSide priceSpreadSide = GetPriceSpreadSideFromUI();

            ArbitrageOpenArgument openArg = new ArbitrageOpenArgument();
            openArg.BuyInstrument = buyInstrument;
            openArg.BuyInstrumentOrderPriceType = buyOrderPriceType;
            openArg.SellInstrument = sellInstrument;
            openArg.SellInstrumentOrderPriceType = sellOrderPriceType;
            openArg.PreferentialSide = preferentialSide;
            openArg.OpenCondition = new PriceSpreadCondition()
            {
                PriceSpreadSide = priceSpreadSide,
                PriceSpreadThreshold = this.nudPriceSpreadThreshold.Value
            };
            openArg.TotalOrderQty = (int)this.nudTotalOrderQty.Value;
            openArg.OrderQtyUint = (int)this.nudOrderQtyUint.Value;
            openArg.DifferentialUnit = (int)this.nudDifferentialUnit.Value;

            if (VerifyMargin(openArg, out errorMessage) == false)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, errorMessage);
                return;
            }

            decimal evaluateMargin = EvaluateMargin(openArg);
            string text = string.Format("套利单预计占用保证金 {0},确定跟单么?", evaluateMargin.ToString("#,0"));
            if (DialogResult.Yes != USeFuturesSpiritUtility.ShowYesNoMessageBox(this, text))
            {
                return;
            }

            //try
            //{
            //    AutoTraderManager traderManager = USeManager.Instance.AutoTraderManager;
            //    Debug.Assert(traderManager != null);

            //    AutoTrader trader = traderManager.CreateNewAutoTrader(openArg, USeManager.Instance.LoginUser);
            //    trader.BeginOpen();
            //    //[yangming]创建后应该启动跟单
            //    trader.StartOpenOrCloseMonitor();

            //    USeManager.Instance.DataSaver.AddSaveTask(trader.GetArbitrageOrder());
            //}
            //catch (Exception ex)
            //{
            //    USeFuturesSpiritUtility.ShowWarningMessageBox(this, ex.Message);
            //    return;
            //}
        }
        #endregion
    }
}
