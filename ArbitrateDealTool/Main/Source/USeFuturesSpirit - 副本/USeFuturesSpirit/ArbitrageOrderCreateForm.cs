using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;
using USeFuturesSpirit.Arbitrage;
using System.Diagnostics;
using USe.Common;


namespace USeFuturesSpirit
{
    public partial class ArbitrageOrderCreateForm : Form
    {
        #region 全局变量
        private ArbitrageCombineInstrument m_combineInstrument = null;             //组合行情合约参数
        private USeProduct m_product = null;//品种
        private ArbitrageArgument m_arbitrageArgument = null;//套利合约下单参数

        private USeMarketData m_nearMarketData = null;  // 近月合约行情
        private USeMarketData m_farMarketData = null;   // 远月合约行情

        private BindingList<ArbitrageAlarmArgumentViewModel> m_dataSourceAlarm = null; //预警绑定
        #endregion

        #region 重载构造函数
        /// <summary>
        /// 新增-产品名称
        /// </summary>
        /// <param name="product"></param>
        public ArbitrageOrderCreateForm(USeProduct product)
        {
            InitializeComponent();

            m_product = product;
        }

        /// <summary>
        /// 新增-行情组合合约
        /// </summary>
        /// <param name="combineInstrument"></param>
        public ArbitrageOrderCreateForm(ArbitrageCombineInstrument combineInstrument,USeProduct product)
        {
            InitializeComponent();

            m_combineInstrument = combineInstrument;
            m_product = product;
        }

        /// <summary>
        /// 修改-下单参数
        /// </summary>
        /// <param name="argument"></param>
        public ArbitrageOrderCreateForm(ArbitrageArgument argument,USeProduct product)
        {
            InitializeComponent();

            m_product = product;
            m_arbitrageArgument = argument;
            
        }
        #endregion


        /// <summary>
        /// 窗口初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArbitrageOrderCreateForm_Load(object sender, EventArgs e)
        {
            USeManager.Instance.QuoteDriver.OnMarketDataChanged += QuoteDriver_OnMarketDataChanged;

            ClearMarketDataLable();
            InitializeInstrument();
            InitializeAlarmList();

            if(m_arbitrageArgument != null)
            {
                SetArbitrageArgument(m_arbitrageArgument);

                this.preferentialSideControl_OpenArg.Enabled = false;
                this.nudDifferentialUnit_OpenArg.Enabled = false;
                this.nudOrderQtyUint_OpenArg.Enabled = false;
                this.nudTotalOrderQty_OpenArg.Enabled = false;

                this.preferentialSideControl_CloseArg.Enabled = false;
                this.nudDifferentialUnit_CloseArg.Enabled = false;
                this.nudOrderQtyUint_CloseArg.Enabled = false;
            }
            else
            {
                SetDefaultAribtrageArgument();
            }
        }

        /// <summary>
        /// 初始化合约。
        /// </summary>
        private void InitializeInstrument()
        {
            this.cbxNearInstrument.Items.Clear();
            this.cbxFarInstrument.Items.Clear();
            this.cbxNearInstrument.DisplayMember = "InstrumentCode";
            this.cbxNearInstrument.DisplayMember = "InstrumentCode";

            if (m_product != null)
            {
                List<USeInstrumentDetail> instrumentList = USeManager.Instance.OrderDriver.QueryInstrumentDetail(m_product.ProductCode);
                foreach (USeInstrumentDetail item in instrumentList)
                {
                    this.cbxNearInstrument.Items.Add(item.Instrument);
                    this.cbxFarInstrument.Items.Add(item.Instrument);
                }
            }

            USeInstrument nearInstrument = null;
            USeInstrument farInstrument = null;

            if (m_arbitrageArgument != null)
            {
                nearInstrument = m_arbitrageArgument.NearInstrument;
                farInstrument = m_arbitrageArgument.FarInstrument;
            }
            else if (m_combineInstrument != null)
            {
                nearInstrument = m_combineInstrument.FirstInstrument;
                farInstrument = m_combineInstrument.SecondInstrument;
            }

            if (nearInstrument != null)
            {
                this.cbxNearInstrument.SelectedItem = nearInstrument;
            }
            else if(this.cbxNearInstrument.Items.Count >0)
            {
                this.cbxNearInstrument.SelectedIndex = 0;
            }

            if (farInstrument != null)
            {
                this.cbxFarInstrument.SelectedItem = farInstrument;
            }
            else if (this.cbxFarInstrument.Items.Count > 1)
            {
                this.cbxFarInstrument.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// 初始化预警列表。
        /// </summary>
        private void InitializeAlarmList()
        {
            m_dataSourceAlarm = new BindingList<ArbitrageAlarmArgumentViewModel>();
            this.gridAlarmCondition.AutoGenerateColumns = false;
            this.gridAlarmCondition.DataSource = m_dataSourceAlarm;

            if(m_arbitrageArgument != null && m_arbitrageArgument.AlarmArgs != null)
            {
                foreach(ArbitrageAlarmArgument alarmArg in m_arbitrageArgument.AlarmArgs)
                {
                    ArbitrageAlarmArgumentViewModel model = ArbitrageAlarmArgumentViewModel.CreatViewModel(alarmArg);
                }
            }
        }


        /// <summary>
        /// 设置前套利参数用于修改
        /// </summary>
        private void SetArbitrageArgument(ArbitrageArgument arg)
        {
            this.arbitrageOperationSideControl.OperationSide = arg.OperationSide;

            //开仓参数参数
            if (arg.OpenArg != null)
            {
                ArbitrageOpenArgument openArg = arg.OpenArg;

                this.preferentialSideControl_OpenArg.PreferentialSide = openArg.PreferentialSide;
                this.orderPriceTypeControl_OpenNearArg.OrderPriceType = openArg.NearOrderPriceType;
                this.orderPriceTypeControl_OpenFarArg.OrderPriceType = openArg.FarOrderPriceType;
                this.priceSpreadSideControl_OpenSpreadArg.PriceSpreadSide = openArg.OpenCondition.PriceSpreadSide;
                this.nudPriceSpreadThreshold_OpenArg.Value = openArg.OpenCondition.PriceSpreadThreshold;
                this.nudDifferentialUnit_OpenArg.Value = openArg.DifferentialUnit;
                this.nudOrderQtyUint_OpenArg.Value = openArg.OrderQtyUint;
                this.nudTotalOrderQty_OpenArg.Value = openArg.TotalOrderQty;
            }

            //平仓参数
            if (arg.CloseArg != null)
            {
                ArbitrageCloseArgument closeArg = arg.CloseArg;

                this.orderPriceTypeControl_CloseNearArg.OrderPriceType = closeArg.NearOrderPriceType;
                this.orderPriceTypeControl_CloseFarArg.OrderPriceType = closeArg.FarOrderPriceType;
                this.preferentialSideControl_CloseArg.PreferentialSide = closeArg.PreferentialSide;
                this.priceSpreadSideControl_CloseSpreadArg.PriceSpreadSide = closeArg.CloseCondition.PriceSpreadSide;
                this.nudPriceSpreadThreshold_CloseArg.Value = closeArg.CloseCondition.PriceSpreadThreshold;
                this.nudDifferentialUnit_CloseArg.Value = closeArg.DifferentialUnit;
                this.nudOrderQtyUint_CloseArg.Value = closeArg.OrderQtyUint;
            }

            //止损参数
            if (arg.StopLossArg != null)
            {
                this.priceSpreadSideControl_StopLossArg.PriceSpreadSide = arg.StopLossArg.StopLossCondition.PriceSpreadSide;
                this.nudPriceSpreadThreshold_StopLossArg.Value = arg.StopLossArg.StopLossCondition.PriceSpreadThreshold;
            }

            //预警参数
            if (arg.AlarmArgs != null)
            {
                foreach (ArbitrageAlarmArgument alarmArg in arg.AlarmArgs)
                {
                    m_dataSourceAlarm.Add(ArbitrageAlarmArgumentViewModel.CreatViewModel(alarmArg));
                }
            }
        }

        private void SetDefaultAribtrageArgument()
        {
            ArbitrageCombineOrderSetting combineSetting = null;
            try
            {
                string brokerId = USeManager.Instance.LoginUser.BrokerId;
                string account = USeManager.Instance.LoginUser.Account;
                combineSetting = USeManager.Instance.DataAccessor.GetCombineOrderSetting(brokerId, account, m_product.ProductCode);
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, "获取套利下单参数异常:" + ex.Message);
                return;
            }

            if (combineSetting == null)
            {
                combineSetting = CreateDefaultCombineOrderSetting();
            }

            //开仓参数      
            this.preferentialSideControl_OpenArg.PreferentialSide = (combineSetting.OpenFirstDirection == USeDirection.Long) ? USeOrderSide.Buy : USeOrderSide.Sell;
            this.orderPriceTypeControl_OpenNearArg.OrderPriceType = combineSetting.NearPriceStyle;
            this.orderPriceTypeControl_OpenFarArg.OrderPriceType = combineSetting.FarPriceStyle;
            this.priceSpreadSideControl_OpenSpreadArg.PriceSpreadSide = PriceSpreadSide.LessOrEqual;
            this.nudPriceSpreadThreshold_OpenArg.Value = 0;
            this.nudDifferentialUnit_OpenArg.Value = 0;
            this.nudOrderQtyUint_OpenArg.Value = combineSetting.OpenVolumnPerNum;
            this.nudTotalOrderQty_OpenArg.Value = combineSetting.OpenVolumn;

            //平仓参数
            this.preferentialSideControl_CloseArg.PreferentialSide = (combineSetting.CloseFirstDirection == USeDirection.Long) ? USeOrderSide.Buy : USeOrderSide.Sell;
            this.orderPriceTypeControl_CloseNearArg.OrderPriceType = combineSetting.NearPriceStyle;
            this.orderPriceTypeControl_CloseFarArg.OrderPriceType = combineSetting.FarPriceStyle;
            this.priceSpreadSideControl_CloseSpreadArg.PriceSpreadSide = PriceSpreadSide.LessOrEqual;
            this.nudPriceSpreadThreshold_CloseArg.Value = 0;
            this.nudDifferentialUnit_CloseArg.Value = 0;

            //止损
            this.priceSpreadSideControl_StopLossArg.PriceSpreadSide = PriceSpreadSide.LessOrEqual;
            this.nudPriceSpreadThreshold_StopLossArg.Value = 0;

            //预警
            this.arbitragePriceSpreadMonitorTypeControl.MonitorType = ArbitragePriceSpreadAlarmType.Open;
            this.nudPriceSpreadThreshold_Alarm.Value = 0;

#if DEBUG
            this.arbitrageOperationSideControl.OperationSide = ArbitrageOperationSide.BuyNearSellFar;
            this.nudPriceSpreadThreshold_OpenArg.Value = -50;
            this.nudPriceSpreadThreshold_CloseArg.Value = -70;
            this.nudPriceSpreadThreshold_StopLossArg.Value = -30;
            this.nudTotalOrderQty_OpenArg.Value = 10;
            this.nudDifferentialUnit_OpenArg.Value = 2;
            this.nudDifferentialUnit_CloseArg.Value = 2;
            this.nudOrderQtyUint_OpenArg.Value = 2;
            this.nudOrderQtyUint_CloseArg.Value = 2;
#endif
        }


        /// <summary>
        /// 初始化市场行情标签
        /// </summary>
        private void ClearMarketDataLable()
        {
            this.lblNearInstrumentPrice.Text = "---";
            this.lblFarInstrumentPrice.Text = "---";
            this.lblPriceSpread.Text = "---";
        }


        /// <summary>
        /// 行情通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuoteDriver_OnMarketDataChanged(object sender, USeMarketDataChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeMarketDataChangedEventArgs>(QuoteDriver_OnMarketDataChanged), sender, e);
                return;
            }

            UpdateMarketData(e.MarketData);
        }

        /// <summary>
        /// 设定价格控件值。
        /// </summary>
        private void UpdateMarketData(USeMarketData marketData)
        {
            //行情显示的时候，先应该根据价差的监控类型--大于？小于确定组合合约的买入卖出方向
            //如果是小于则是买入组合合约，近月买入远月卖出，做的是价差扩大；反之，大于是卖出组合合约，近月卖出，远月买入。
            //所以行情的显示依赖于价差的类型的选择(大于，小于)，价差的类型决定了买入卖出的合约即合约方向，在合约方向确定的
            //前提下，依据价格类型选择显示行情以及计算的价差

            if (m_nearMarketData != null && m_nearMarketData.Instrument.Equals(marketData.Instrument))
            {
                m_nearMarketData = marketData;
            }
            else if (m_farMarketData != null && m_farMarketData.Instrument.Equals(marketData.Instrument))
            {
                m_farMarketData = marketData;
            }
            else
            {
                return;
            }

            UpdatePriceLable();
        }

        private void UpdatePriceLable()
        {
            decimal nearPrice = 0m;
            decimal farPrice = 0m;

            ArbitrageOperationSide operationSide = this.arbitrageOperationSideControl.OperationSide;
            ArbitrageOrderPriceType nearOrderPriceType = this.orderPriceTypeControl_OpenNearArg.OrderPriceType;
            ArbitrageOrderPriceType farOrderPriceType = this.orderPriceTypeControl_OpenFarArg.OrderPriceType;

            if (operationSide == ArbitrageOperationSide.BuyNearSellFar)
            {
                nearPrice = GetOrderPrice(m_nearMarketData, nearOrderPriceType, USeOrderSide.Buy);
                farPrice = GetOrderPrice(m_farMarketData, farOrderPriceType, USeOrderSide.Sell);
            }
            else if (operationSide == ArbitrageOperationSide.SellNearBuyFar)
            {
                nearPrice = GetOrderPrice(m_nearMarketData, nearOrderPriceType, USeOrderSide.Sell);
                farPrice = GetOrderPrice(m_farMarketData, farOrderPriceType, USeOrderSide.Buy);
            }

            this.lblNearInstrumentPrice.Text = nearPrice > 0m ? nearPrice.ToString() : "---";
            this.lblFarInstrumentPrice.Text = farPrice > 0m ? farPrice.ToString() : "---";

            if (nearPrice > 0m && farPrice >= 0m)
            {
                decimal diffPrice = (nearPrice - farPrice);
                this.lblPriceSpread.Text = diffPrice.ToString();
                this.lblPriceSpread.ForeColor = diffPrice >= 0m ? Color.Red : Color.Green;
            }
            else
            {
                this.lblPriceSpread.Text = "---";
            }
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
            if (marketData == null)
            {
                return 0m;
            }
            if (orderPriceType == ArbitrageOrderPriceType.Unknown)
            {
                return 0m;
            }

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

        /// <summary>
        /// 套利组合下单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenArbitrageOrder_Click(object sender, EventArgs e)
        {
            string errorMessage = "";
            if (CheckArgument(out errorMessage) == false)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "[套利参数设定]" + "\n" +errorMessage);
                return;
            }

            if (CreateNewArbitrageOrder())
            {
                this.Close();
            }
        }

        /// <summary>
        /// 检查各项输入参数
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool CheckArgument(out string errorMessage)
        {
            errorMessage = "";

            if (VerifyCombineInstrumentArgument(out errorMessage) == false)
            {
                errorMessage = "套利合约选项设定有误:" + errorMessage;
                return false;
            }
            else if (VerifyOpenArgument(out errorMessage) == false)
            {
                errorMessage = "开仓套利参数设定有误:" + errorMessage;
                return false;
            }
            else if (VerifyCloseArgument(out errorMessage) == false)
            {
                errorMessage = "平仓套利参数设定有误:" + errorMessage;
                return false;
            }
            else if (VerifyStopLossArgument(out errorMessage) == false)
            {
                errorMessage = "止损套利参数设定有误:" + errorMessage;
                return false;
            }
            else if (VerifyNotifyArgument(out errorMessage) == false)
            {
                errorMessage = "预警套利参数设定有误:" + errorMessage;
                return false;
            }
            else if (VerifyGlobleArgument(out errorMessage) == false)
            {
                errorMessage = "全局相关关系套利参数设定有误:" + errorMessage;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建组合套利单下单参数
        /// </summary>
        private bool CreateNewArbitrageOrder()
        {
            USeInstrument nearInstrument = this.cbxNearInstrument.SelectedItem as USeInstrument;
            USeInstrument farInstrument = this.cbxFarInstrument.SelectedItem as USeInstrument;
            ArbitrageOperationSide operationSide = this.arbitrageOperationSideControl.OperationSide;

            ArbitrageOpenArgument openArg = new ArbitrageOpenArgument();
            if (operationSide == ArbitrageOperationSide.BuyNearSellFar)
            {
                openArg.BuyInstrument = nearInstrument;
                openArg.SellInstrument = farInstrument;
                openArg.BuyInstrumentOrderPriceType = this.orderPriceTypeControl_OpenNearArg.OrderPriceType;
                openArg.SellInstrumentOrderPriceType = this.orderPriceTypeControl_OpenFarArg.OrderPriceType;
            }
            else if (operationSide == ArbitrageOperationSide.SellNearBuyFar)
            {
                openArg.BuyInstrument = farInstrument;
                openArg.SellInstrument = nearInstrument;
                openArg.BuyInstrumentOrderPriceType = this.orderPriceTypeControl_OpenFarArg.OrderPriceType;
                openArg.SellInstrumentOrderPriceType = this.orderPriceTypeControl_OpenNearArg.OrderPriceType;
            }
            else
            {
                Debug.Assert(false);
            }
            openArg.NearOrderPriceType = this.orderPriceTypeControl_OpenNearArg.OrderPriceType;
            openArg.FarOrderPriceType = this.orderPriceTypeControl_OpenFarArg.OrderPriceType;

            openArg.PreferentialSide = this.preferentialSideControl_OpenArg.PreferentialSide;
            openArg.OpenCondition = new PriceSpreadCondition()
            {
                PriceSpreadSide = this.priceSpreadSideControl_OpenSpreadArg.PriceSpreadSide,
                PriceSpreadThreshold = this.nudPriceSpreadThreshold_OpenArg.Value
            };
            openArg.TotalOrderQty = (int)this.nudTotalOrderQty_OpenArg.Value;
            openArg.OrderQtyUint = (int)this.nudOrderQtyUint_OpenArg.Value;
            openArg.DifferentialUnit = (int)this.nudDifferentialUnit_OpenArg.Value;


            ArbitrageCloseArgument closeArg = new ArbitrageCloseArgument();
            if (operationSide == ArbitrageOperationSide.BuyNearSellFar)
            {
                closeArg.BuyInstrument = farInstrument;
                closeArg.SellInstrument = nearInstrument;
                closeArg.BuyInstrumentOrderPriceType = this.orderPriceTypeControl_CloseFarArg.OrderPriceType;
                closeArg.SellInstrumentOrderPriceType = this.orderPriceTypeControl_CloseNearArg.OrderPriceType;
            }
            else if (operationSide == ArbitrageOperationSide.SellNearBuyFar)
            {
                closeArg.BuyInstrument = nearInstrument;
                closeArg.SellInstrument = farInstrument;
                closeArg.BuyInstrumentOrderPriceType = this.orderPriceTypeControl_CloseNearArg.OrderPriceType;
                closeArg.SellInstrumentOrderPriceType = this.orderPriceTypeControl_CloseFarArg.OrderPriceType;
            }
            closeArg.NearOrderPriceType = this.orderPriceTypeControl_CloseNearArg.OrderPriceType;
            closeArg.FarOrderPriceType = this.orderPriceTypeControl_CloseFarArg.OrderPriceType;

            closeArg.PreferentialSide = this.preferentialSideControl_CloseArg.PreferentialSide;
            closeArg.CloseCondition = new PriceSpreadCondition()
            {
                PriceSpreadSide = this.priceSpreadSideControl_CloseSpreadArg.PriceSpreadSide,
                PriceSpreadThreshold = this.nudPriceSpreadThreshold_CloseArg.Value
            };
            closeArg.OrderQtyUint = (int)this.nudOrderQtyUint_CloseArg.Value;
            closeArg.DifferentialUnit = (int)this.nudDifferentialUnit_CloseArg.Value;

            ArbitrageStopLossArgument stopLossArg = null;
            if (this.cbxStopLossFlag.Checked)
            {
                stopLossArg = new ArbitrageStopLossArgument();
                stopLossArg.StopLossCondition = new PriceSpreadCondition() {
                    PriceSpreadSide = this.priceSpreadSideControl_StopLossArg.PriceSpreadSide,
                    PriceSpreadThreshold = this.nudPriceSpreadThreshold_StopLossArg.Value
                };
            }


            List<ArbitrageAlarmArgument> alarmArgList = new List<ArbitrageAlarmArgument>();
            if (m_dataSourceAlarm != null && m_dataSourceAlarm.Count > 0)
            {
                foreach (ArbitrageAlarmArgumentViewModel alarmView in m_dataSourceAlarm)
                {
                    alarmArgList.Add(ArbitrageAlarmArgumentViewModel.CreatAlarmData(alarmView));
                }
            }

            ArbitrageArgument argument = new ArbitrageArgument();
            argument.ProductID = m_product.ProductCode;
            argument.NearInstrument = nearInstrument;
            argument.FarInstrument = farInstrument;
            argument.OperationSide = operationSide;

            argument.OpenArg = openArg;
            argument.CloseArg = closeArg;
            argument.StopLossArg = stopLossArg;
            argument.AlarmArgs = alarmArgList;

            string errorMessage = string.Empty;
            if (VerifyMargin(argument.OpenArg, out errorMessage) == false)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, errorMessage);
                return false;
            }

            decimal evaluateMargin = EvaluateMargin(argument.OpenArg);
            string text = string.Format("套利单预计占用保证金 {0},确定跟单么?", evaluateMargin.ToString("#,0"));
            if (DialogResult.Yes != USeFuturesSpiritUtility.ShowYesNoMessageBox(this, text))
            {
                return false;
            }

            try
            {
                AutoTraderManager traderManager = USeManager.Instance.AutoTraderManager;
                Debug.Assert(traderManager != null);

                AutoTrader trader = traderManager.CreateNewAutoTrader(argument, USeManager.Instance.LoginUser);
                trader.BeginOpen();
                //[yangming]创建后应该启动跟单
                trader.StartOpenOrCloseMonitor();

                USeManager.Instance.DataSaver.AddSaveTask(trader.GetArbitrageOrder());

                //同时保存所有的ArbitrageArgument便于下次修改

            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, ex.Message);
                return false;
            }

            return true;
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
        /// 添加预警
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_AddNotify_Click(object sender, EventArgs e)
        {
            ArbitragePriceSpreadAlarmType monitorType = this.arbitragePriceSpreadMonitorTypeControl.MonitorType;
            PriceSpreadSide priceSpreadSide = this.priceSpreadSideControl_Alarm.PriceSpreadSide;
            decimal threshold = this.nudPriceSpreadThreshold_Alarm.Value;

            ArbitrageAlarmArgument args = new ArbitrageAlarmArgument();
            args.MonitorType = monitorType;
            args.PriceSpreadSide = priceSpreadSide;
            args.PriceSpreadThreshold = threshold;

            ArbitrageAlarmArgumentViewModel model = ArbitrageAlarmArgumentViewModel.CreatViewModel(args);
            ArbitrageAlarmArgumentViewModel hasModel = (from m in m_dataSourceAlarm
                                                        where m.Equals(model)
                                                        select m).FirstOrDefault();
            if (hasModel != null)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "不能添加重复的预警");
                return;
            }

            m_dataSourceAlarm.Add(ArbitrageAlarmArgumentViewModel.CreatViewModel(args));
        }

        /// <summary>
        /// 移除预警
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CancelNotify_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectRows = this.gridAlarmCondition.SelectedRows;
            if (selectRows == null || selectRows.Count == 0)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "请选择要移除的预警");
                return;
            };

            foreach (DataGridViewRow row in selectRows)
            {
                ArbitrageAlarmArgumentViewModel model = row.DataBoundItem as ArbitrageAlarmArgumentViewModel;
                m_dataSourceAlarm.Remove(model);
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #region 参数校验
        /// <summary>
        /// 校验近远月合约选项
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool VerifyCombineInstrumentArgument(out string errorMessage)
        {
            errorMessage = "";

            USeInstrument nearInstrument = this.cbxNearInstrument.SelectedItem as USeInstrument;
            USeInstrument farInstrument = this.cbxFarInstrument.SelectedItem as USeInstrument;

            if (nearInstrument.InstrumentCode.CompareTo(farInstrument.InstrumentCode) >=0)
            {
                errorMessage = "请选择正确的近月合约和远月合约";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 近远月比较
        /// </summary>
        /// <param name="firIns"></param>
        /// <param name="secIns"></param>
        /// <returns></returns>
        private bool CompareExpireDateSecUpperFir(USeInstrument nearInstrument, USeInstrument farInstrument)
        {
            if (nearInstrument == null || farInstrument == null) return false;

            try
            {
                string productIDFir = USeTraderProtocol.GetVarieties(nearInstrument.InstrumentCode);
                string ProductIDSec = USeTraderProtocol.GetVarieties(farInstrument.InstrumentCode);
                Debug.Assert(productIDFir.Equals(ProductIDSec));

                List<USeInstrumentDetail> instrumentDetailList = USeManager.Instance.OrderDriver.QueryInstrumentDetail(productIDFir);

                USeInstrumentDetail nearDetail = (from d in instrumentDetailList
                                                  where nearInstrument.InstrumentCode == d.Instrument.InstrumentCode
                                                  select d).FirstOrDefault();

                USeInstrumentDetail farDetail = (from d in instrumentDetailList
                                                 where farInstrument.InstrumentCode == d.Instrument.InstrumentCode
                                                 select d).FirstOrDefault();

                Debug.Assert(nearDetail != null);
                Debug.Assert(farDetail != null);

                if (nearDetail.ExpireDate >= farDetail.ExpireDate)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "合约设定有误" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 校验开仓各项准备参数
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool VerifyOpenArgument(out string errorMessage)
        {
            errorMessage = "";

            if (this.nudOrderQtyUint_OpenArg.Value > this.nudTotalOrderQty_OpenArg.Value)
            {
                errorMessage = "开仓下单单位不能超过开仓单边总手数，请重新输入";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 校验平仓各项准备参数
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool VerifyCloseArgument(out string errorMessage)
        {
            errorMessage = "";

            return true;
        }
        /// <summary>
        /// 校验止损各项准备参数
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool VerifyStopLossArgument(out string errorMessage)
        {
            errorMessage = "";
            return true;
        }
        /// <summary>
        /// 校验预警参数
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool VerifyNotifyArgument(out string errorMessage)
        {
            errorMessage = "";
            return true;
        }
        /// <summary>
        /// 校验整体参数相关关系
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool VerifyGlobleArgument(out string errorMessage)
        {
            errorMessage = "";

            if (this.arbitrageOperationSideControl.OperationSide == ArbitrageOperationSide.BuyNearSellFar)
            {
                if (nudPriceSpreadThreshold_CloseArg.Value <= this.nudPriceSpreadThreshold_OpenArg.Value)
                {
                    errorMessage = "买进卖远下，平仓参数的价差要大于开仓设定的价差，请重新设置";
                    return false;
                }

                if (nudPriceSpreadThreshold_StopLossArg.Value >= this.nudPriceSpreadThreshold_OpenArg.Value)
                {
                    errorMessage = "买进卖远下，止损参数的价差要小于开仓设定的价差，请重新设置";
                    return false;
                }
            }
            else
            {
                if (nudPriceSpreadThreshold_CloseArg.Value >= this.nudPriceSpreadThreshold_OpenArg.Value)
                {
                    errorMessage = "卖近买远下，平仓参数的价差要小于开仓设定的价差，请重新设置";
                    return false;
                }

                if (nudPriceSpreadThreshold_StopLossArg.Value <= this.nudPriceSpreadThreshold_OpenArg.Value)
                { 
                    errorMessage = "卖近买远下，止损参数的价差要大于开仓设定的价差，请重新设置";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 创建默认下单参数
        /// </summary>
        /// <returns></returns>
        private ArbitrageCombineOrderSetting CreateDefaultCombineOrderSetting()
        {
            ArbitrageCombineOrderSetting orderSetting = new ArbitrageCombineOrderSetting();
            orderSetting.OpenFirstDirection = USeDirection.Long;
            orderSetting.NearPriceStyle = ArbitrageOrderPriceType.OpponentPrice;
            orderSetting.FarPriceStyle = ArbitrageOrderPriceType.OpponentPrice;

            orderSetting.CloseFirstDirection = USeDirection.Long;
            orderSetting.OpenVolumn = 0;
            orderSetting.OpenVolumnPerNum = 0;




            return orderSetting;
        }

        /// <summary>
        /// 设置合约默认下单参数
        /// </summary>
        /// <param name="combineOrderSetting"></param>
        private void SetOrderArgument(ArbitrageCombineOrderSetting combineOrderSetting)
        {
            Debug.Assert(combineOrderSetting != null);

            
        }

        #endregion

        #region 开仓参数

        /// <summary>
        /// 开仓参数--获取优先买卖方向。
        /// </summary>
        /// <returns></returns>
        private USeOrderSide GetOpenArgumentPreferentialSideFromUI()
        {
            USeOrderSide orderSide = USeOrderSide.Buy;

            if (this.preferentialSideControl_OpenArg.PreferentialSide == USeOrderSide.Buy)
            {
                return USeOrderSide.Buy;
            }
            else if (this.preferentialSideControl_OpenArg.PreferentialSide == USeOrderSide.Sell)
            {
                return USeOrderSide.Sell;
            }
            else
            {
                Debug.Assert(false);
            }

            return orderSide;
        }

        /// <summary>
        /// 开仓参数--近月合约价格类型。
        /// </summary>
        /// <returns></returns>
        /// 
        private ArbitrageOrderPriceType GetOpenArgumentNearOrderPriceTypeFromUI()
        {
            ArbitrageOrderPriceType buyOrderPriceType = ArbitrageOrderPriceType.Unknown;
            if (orderPriceTypeControl_OpenNearArg.OrderPriceType == ArbitrageOrderPriceType.LastPrice)
            {
                return ArbitrageOrderPriceType.LastPrice;
            }
            else if (orderPriceTypeControl_OpenNearArg.OrderPriceType == ArbitrageOrderPriceType.OpponentPrice)
            {
                return ArbitrageOrderPriceType.OpponentPrice;
            }
            else if (orderPriceTypeControl_OpenNearArg.OrderPriceType == ArbitrageOrderPriceType.QueuePrice)
            {
                return ArbitrageOrderPriceType.QueuePrice;
            }
            else if (orderPriceTypeControl_OpenNearArg.OrderPriceType == ArbitrageOrderPriceType.Unknown)
            {
                return ArbitrageOrderPriceType.Unknown;
            }
            else
            {
                Debug.Assert(false);
            }

            return buyOrderPriceType;
        }

        /// <summary>
        /// 开仓参数--远月合约价格类型。
        /// </summary>
        /// <returns></returns>
        private ArbitrageOrderPriceType GetOpenArgumentFarOrderPriceTypeFromUI()
        {

            ArbitrageOrderPriceType buyOrderPriceType = ArbitrageOrderPriceType.Unknown;
            if (orderPriceTypeControl_OpenNearArg.OrderPriceType == ArbitrageOrderPriceType.LastPrice)
            {
                return ArbitrageOrderPriceType.LastPrice;
            }
            else if (orderPriceTypeControl_OpenNearArg.OrderPriceType == ArbitrageOrderPriceType.OpponentPrice)
            {
                return ArbitrageOrderPriceType.OpponentPrice;
            }
            else if (orderPriceTypeControl_OpenNearArg.OrderPriceType == ArbitrageOrderPriceType.QueuePrice)
            {
                return ArbitrageOrderPriceType.QueuePrice;
            }
            else if (orderPriceTypeControl_OpenNearArg.OrderPriceType == ArbitrageOrderPriceType.Unknown)
            {
                return ArbitrageOrderPriceType.Unknown;
            }
            else
            {
                Debug.Assert(false);
            }

            return buyOrderPriceType;
        }

        /// <summary>
        /// 开仓参数--获取价差监控类型
        /// </summary>
        /// <returns></returns>
        private PriceSpreadSide GetOpenArgumentSpreadTypeFromUI()
        {
            PriceSpreadSide spreadType = PriceSpreadSide.Unknown;

            if (priceSpreadSideControl_OpenSpreadArg.PriceSpreadSide == PriceSpreadSide.GreaterOrEqual)
            {
                return PriceSpreadSide.GreaterOrEqual;
            }
            else if (priceSpreadSideControl_OpenSpreadArg.PriceSpreadSide == PriceSpreadSide.LessOrEqual)
            {
                return PriceSpreadSide.LessOrEqual;
            }
            else
            {
                Debug.Assert(false);
            }

            return spreadType;
        }

        /// <summary>
        /// 开仓参数--获取价差监控数值
        /// </summary>
        /// <returns></returns>
        private decimal GetOpenArgumentSpreadValueFromUI()
        {
            return this.nudPriceSpreadThreshold_OpenArg.Value;
        }

        /// <summary>
        /// 开仓参数--获取最大仓差
        /// </summary>
        /// <returns></returns>
        private decimal GetOpenArgumentDifferentialUnitValueFromUI()
        {
            return this.nudDifferentialUnit_OpenArg.Value;
        }

        /// <summary>
        /// 开仓参数--获取单位(手/次)
        /// </summary>
        /// <returns></returns>
        private decimal GetOpenArgumentOrderQtyUintValueFromUI()
        {
            return this.nudOrderQtyUint_OpenArg.Value;
        }

        /// <summary>
        /// 开仓参数--获取单边手数
        /// </summary>
        /// <returns></returns>
        private decimal GetOpenArgumentTotalOrderQtyValueFromUI()
        {
            return this.nudTotalOrderQty_OpenArg.Value;
        }

        #endregion



        /// <summary>
        /// 近月合约组合框选择更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbx_NearInstrumentSelectChanged(object sender, EventArgs e)
        {
            USeInstrument instrument = this.cbxNearInstrument.SelectedItem as USeInstrument;
            Debug.Assert(instrument != null);
            try
            {
                USeManager.Instance.QuoteDriver.Subscribe(instrument);
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "行情订阅异常:" + ex.Message);
                return;
            }
        }

        /// <summary>
        /// 远月合约组合框选择更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbx_FarInstrumentSelectChanged(object sender, EventArgs e)
        {
            USeInstrument instrument = this.cbxFarInstrument.SelectedItem as USeInstrument;
            Debug.Assert(instrument != null);
            try
            {
                USeManager.Instance.QuoteDriver.Subscribe(instrument);
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "行情订阅异常:" + ex.Message);
                return;
            }
        }

        private void arbitrageOperationSideControl1_OnArbitrageOperationSideChanged(ArbitrageOperationSide operationSide)
        {
            switch (operationSide)
            {
                case ArbitrageOperationSide.SellNearBuyFar:
                    {
                        this.priceSpreadSideControl_OpenSpreadArg.PriceSpreadSide = PriceSpreadSide.GreaterOrEqual;
                        this.priceSpreadSideControl_CloseSpreadArg.PriceSpreadSide = PriceSpreadSide.LessOrEqual;
                        this.priceSpreadSideControl_StopLossArg.PriceSpreadSide = PriceSpreadSide.GreaterOrEqual;
                        break;
                    }
                case ArbitrageOperationSide.BuyNearSellFar:
                    {
                        this.priceSpreadSideControl_OpenSpreadArg.PriceSpreadSide = PriceSpreadSide.LessOrEqual;
                        this.priceSpreadSideControl_CloseSpreadArg.PriceSpreadSide = PriceSpreadSide.GreaterOrEqual;
                        this.priceSpreadSideControl_StopLossArg.PriceSpreadSide = PriceSpreadSide.LessOrEqual;
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// 视图
        /// </summary>
        private class ArbitrageAlarmArgumentViewModel : USeBaseViewModel
        {
            private ArbitragePriceSpreadAlarmType m_monitorType = ArbitragePriceSpreadAlarmType.Unknown;
            private PriceSpreadSide m_priceSpreadSide = PriceSpreadSide.Unknown;
            private decimal m_priceSpreadThreshold = 0m;

            #region property
            /// <summary>
            /// 监控类型。
            /// </summary>
            public ArbitragePriceSpreadAlarmType MonitorType
            {
                get { return m_monitorType; }
                set
                {
                    if (value != m_monitorType)
                    {
                        m_monitorType = value;
                        SetProperty(() => this.MonitorType);
                        SetProperty(() => this.MonitorTypeDesc);
                    }
                }
            }

            public string MonitorTypeDesc
            {
                get { return this.MonitorType.ToDescription(); }
            }

            /// <summary>
            /// 价差监控方向。
            /// </summary>
            public PriceSpreadSide PriceSpreadSide
            {
                get { return m_priceSpreadSide; }
                set
                {
                    if (value != m_priceSpreadSide)
                    {
                        m_priceSpreadSide = value;
                        SetProperty(() => this.PriceSpreadSide);
                        SetProperty(() => this.SpreadSideThreSholdDesc);
                    }
                }
            }


            /// <summary>
            /// 价差阀值。
            /// </summary>
            public decimal PriceSpreadThreshold
            {
                get { return m_priceSpreadThreshold; }
                set
                {
                    if (value != m_priceSpreadThreshold)
                    {
                        m_priceSpreadThreshold = value;
                        SetProperty(() => this.PriceSpreadThreshold);
                        SetProperty(() => this.SpreadSideThreSholdDesc);
                    }
                }
            }


            /// <summary>
            /// 显示方向+阀值
            /// </summary>
            public string SpreadSideThreSholdDesc
            {
                get
                {
                    switch (PriceSpreadSide)
                    {
                        case PriceSpreadSide.LessOrEqual:
                            return ("<=" + this.PriceSpreadThreshold.ToString());
                        case PriceSpreadSide.GreaterOrEqual:
                            return (">=" + this.PriceSpreadThreshold.ToString());
                        case PriceSpreadSide.Unknown:
                        default:
                            Debug.Assert(false, "SpreadSideThreShold");
                            return this.PriceSpreadThreshold.ToString();
                    }
                }
            }


            public static ArbitrageAlarmArgumentViewModel CreatViewModel(ArbitrageAlarmArgument args)
            {
                ArbitrageAlarmArgumentViewModel model = new ArbitrageAlarmArgumentViewModel();
                model.MonitorType = args.MonitorType;
                model.PriceSpreadSide = args.PriceSpreadSide;
                model.PriceSpreadThreshold = args.PriceSpreadThreshold;

                return model;
            }

            public static ArbitrageAlarmArgument CreatAlarmData(ArbitrageAlarmArgumentViewModel model)
            {
                ArbitrageAlarmArgument args = new ArbitrageAlarmArgument();
                args.MonitorType = model.MonitorType;
                args.PriceSpreadSide = model.PriceSpreadSide;
                args.PriceSpreadThreshold = model.PriceSpreadThreshold;

                return args;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;

                ArbitrageAlarmArgumentViewModel model = obj as ArbitrageAlarmArgumentViewModel;
                if (model == null) return false;

                if (this.MonitorType == model.MonitorType &&
                    this.PriceSpreadSide == model.PriceSpreadSide &&
                    this.PriceSpreadThreshold == model.PriceSpreadThreshold)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            public override int GetHashCode()
            {
                return string.Format("{0}_{1}_{2}", this.MonitorType, this.PriceSpreadSide, this.PriceSpreadThreshold).GetHashCode();
            }
            #endregion
        }

        private void cbxStopLossFlag_CheckedChanged(object sender, EventArgs e)
        {
            this.nudPriceSpreadThreshold_StopLossArg.Enabled = this.cbxStopLossFlag.Checked;
        }
    }
}
