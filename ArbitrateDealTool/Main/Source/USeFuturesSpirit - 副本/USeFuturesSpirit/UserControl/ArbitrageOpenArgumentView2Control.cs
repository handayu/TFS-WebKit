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
    public partial class ArbitrageOpenArgumentView2Control : USeUserControl
    {
        public ArbitrageOpenArgumentView2Control()
        {
            InitializeComponent();
        }

        private void CombinationOrderPanelControl_Load(object sender, EventArgs e)
        {
        }

        private void ClearView()
        {

        }
        public void SetOpenArgument(ArbitrageOpenArgument openArg)
        {
            if (openArg == null)
            {
                ClearView();
                return;
            }

            this.lblBuyInstrument.Text = openArg.BuyInstrument.InstrumentCode;
            this.lblSellInstrument.Text = openArg.SellInstrument.InstrumentCode;

            this.lblBuyOrderPriceType.Text = openArg.BuyInstrumentOrderPriceType.ToDescription();
            this.lblSellOrderPriceType.Text = openArg.SellInstrumentOrderPriceType.ToDescription();

            switch (openArg.PreferentialSide)
            {
                case USeOrderSide.Buy:
                    this.lblPreferentialSide_Buy.Text = "优先买入";
                    this.lblPreferentialSide_Buy.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134))); 
                    this.lblPreferentialSide_Buy.ForeColor = Color.Red;

                    this.lblPreferentialSide_Sell.Text = "卖出";
                    this.lblPreferentialSide_Sell.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    this.lblPreferentialSide_Sell.ForeColor = SystemColors.ControlText;
                    break;
                case USeOrderSide.Sell:
                    this.lblPreferentialSide_Buy.Text = "买入";
                    this.lblPreferentialSide_Buy.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    this.lblPreferentialSide_Buy.ForeColor = Color.Red;

                    this.lblPreferentialSide_Sell.Text = "优先卖出";
                    this.lblPreferentialSide_Sell.Font = this.lblPreferentialSide_Sell.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    this.lblPreferentialSide_Sell.ForeColor = SystemColors.ControlText;
                    break;
            }

            switch (openArg.OpenCondition.PriceSpreadSide)
            {
                case PriceSpreadSide.GreaterOrEqual:
                    this.lblPriceSpreadSide.Text = "大于等于";
                    this.lblPriceSpreadSide.ForeColor = Color.Red;
                    break;
                case PriceSpreadSide.LessOrEqual:
                    this.lblPriceSpreadSide.Text = "小于等于";
                    this.lblPriceSpreadSide.ForeColor = Color.Blue;
                    break;
            }

            this.lblPriceSpreadThreshold.Text = openArg.OpenCondition.PriceSpreadThreshold.ToString();
            this.lblTotalOrderQty.Text = openArg.TotalOrderQty.ToString();
            this.lblOrderQtyUint.Text = openArg.OrderQtyUint.ToString();
            this.lblDifferentialUnit.Text = openArg.DifferentialUnit.ToString();
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
    }
}
