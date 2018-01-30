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
    public partial class ArbitrageOpenArgumentViewControl : USeUserControl
    {
        public ArbitrageOpenArgumentViewControl()
        {
            InitializeComponent();
        }

        private void CombinationOrderPanelControl_Load(object sender, EventArgs e)
        {
            UpdateUIControl();
        }

        /// <summary>
        /// 初始化产品。
        /// </summary>
        /// <param name="product">产品名称。</param>
        private void InitializeInstument(string product)
        {
            this.cbxBuyInstrument.Items.Clear();
            this.cbxSellInstrument.Items.Clear();

            try
            {

                List<USeInstrumentDetail> instrumentDetailList = null;
                if (string.IsNullOrEmpty(product))
                {
                    instrumentDetailList = USeManager.Instance.OrderDriver.QueryInstrumentDetail();
                }
                else
                {
                    instrumentDetailList = USeManager.Instance.OrderDriver.QueryInstrumentDetail(product);
                }
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
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        public void SetOpenArgument(ArbitrageOpenArgument openArg)
        {
            InitializeInstument(string.Empty);
            if (openArg == null)
            {
                return;
            }

            for (int i = 0; i < this.cbxBuyInstrument.Items.Count; i++)
            {
                if ((this.cbxBuyInstrument.Items[i] as USeInstrument).InstrumentCode == openArg.BuyInstrument.InstrumentCode)
                {
                    this.cbxBuyInstrument.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < this.cbxSellInstrument.Items.Count; i++)
            {
                if ((this.cbxSellInstrument.Items[i] as USeInstrument).InstrumentCode == openArg.SellInstrument.InstrumentCode)
                {
                    this.cbxSellInstrument.SelectedIndex = i;
                    break;
                }
            }

            switch (openArg.BuyInstrumentOrderPriceType)
            {
                case ArbitrageOrderPriceType.LastPrice:
                    this.rbnBuyOrderPriceType_LastPrice.Checked = true;
                    break;
                case ArbitrageOrderPriceType.OpponentPrice:
                    this.rbnBuyOrderPriceType_OpponentPrice.Checked = true;
                    break;
                case ArbitrageOrderPriceType.QueuePrice:
                    this.rbnBuyOrderPriceType_QueuePrice.Checked = true;
                    break;
            }

            switch (openArg.SellInstrumentOrderPriceType)
            {
                case ArbitrageOrderPriceType.LastPrice:
                    this.rbnSellOrderPriceType_LastPrice.Checked = true;
                    break;
                case ArbitrageOrderPriceType.OpponentPrice:
                    this.rbnSellOrderPriceType_OpponentPrice.Checked = true;
                    break;
                case ArbitrageOrderPriceType.QueuePrice:
                    this.rbnSellOrderPriceType_QueuePrice.Checked = true;
                    break;
            }

            switch (openArg.PreferentialSide)
            {
                case USeOrderSide.Buy:
                    this.rbnPreferentialSide_Buy.Checked = true;
                    break;
                case USeOrderSide.Sell:
                    this.rbnPreferentialSide_Sell.Checked = true;
                    break;
            }

            switch (openArg.OpenCondition.PriceSpreadSide)
            {
                case PriceSpreadSide.GreaterOrEqual:
                    this.rbnPriceSpreadSide_Greater.Checked = true;
                    break;
                case PriceSpreadSide.LessOrEqual:
                    this.rbnPriceSpreadSide_Less.Checked = true;
                    break;
            }

            this.nudPriceSpreadThreshold.Value = openArg.OpenCondition.PriceSpreadThreshold;
            this.nudTotalOrderQty.Value = openArg.TotalOrderQty;
            this.nudOrderQtyUint.Value = openArg.OrderQtyUint;
            this.nudDifferentialUnit.Value = openArg.DifferentialUnit;
        }

        public ArbitrageOpenArgument GetOpenArgument(out string errorMessage)
        {
            if (VerifyOpenArgument(out errorMessage) == false)
            {
                return null;
            }

            USeInstrument buyInstrument = this.cbxBuyInstrument.SelectedItem as USeInstrument;
            USeInstrument sellInstrument = this.cbxSellInstrument.SelectedItem as USeInstrument;

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

            return openArg;
        }

        /// <summary>
        /// 校验开仓参数。
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool VerifyOpenArgument(out string errorMessage)
        {
            errorMessage = string.Empty;

            USeInstrument buyInstrument = this.cbxBuyInstrument.SelectedItem as USeInstrument;
            if (buyInstrument == null)
            {
                errorMessage = "请选择买入合约";
                return false;
            }

            USeInstrument sellInstrument = this.cbxSellInstrument.SelectedItem as USeInstrument;
            if (sellInstrument == null)
            {
                errorMessage = "请选择卖出合约";
                return false;
            }

            if (this.rbnPreferentialSide_Buy.Checked == false &&
                this.rbnPreferentialSide_Sell.Checked == false)
            {
                errorMessage = "请选择优先建仓合约";
                return false;
            }

            if(this.rbnBuyOrderPriceType_LastPrice.Checked == false &&
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

            if(this.rbnPriceSpreadSide_Greater.Checked == false &&
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

            if(((int)this.nudTotalOrderQty.Value) <=0m)
            {
                errorMessage = "请设定单边开仓手数";
                return false;
            }

            if(((int)this.nudOrderQtyUint.Value) < 1)
            {
                errorMessage = "请设定开仓单位";
                return false;
            }
            if(((int)this.nudDifferentialUnit.Value <0))
            {
                errorMessage = "请设定合理的仓差单位";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新UI界面。
        /// </summary>
        private void UpdateUIControl()
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
                button.BackColor = System.Drawing.Color.RoyalBlue;
            }
            else
            {
                button.BackColor = SystemColors.Control;
            }
        }

        private void rbnPreferentialSide_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUIControl();
        }

        private void rbnBuyOrderPriceType_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUIControl();
        }

        private void rbnSellOrderPriceType_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUIControl();
        }

        private void rbnPriceSpreadSide_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUIControl();
        }
    }
}
