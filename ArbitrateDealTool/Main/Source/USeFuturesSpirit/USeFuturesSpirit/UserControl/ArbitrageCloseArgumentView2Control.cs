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
    public partial class ArbitrageCloseArgumentView2Control : USeUserControl
    {
        public ArbitrageCloseArgumentView2Control()
        {
            InitializeComponent();
        }

        private void ClearView()
        {

        }
        public void SetCloseArgument(ArbitrageCloseArgument closeArg)
        {
            if (closeArg == null)
            {
                ClearView();
                return;
            }

            this.lblBuyInstrument.Text = closeArg.BuyInstrument.InstrumentCode;
            this.lblSellInstrument.Text = closeArg.SellInstrument.InstrumentCode;

            this.lblBuyOrderPriceType.Text = closeArg.BuyInstrumentOrderPriceType.ToDescription();
            this.lblSellOrderPriceType.Text = closeArg.SellInstrumentOrderPriceType.ToDescription();

            switch (closeArg.PreferentialSide)
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

            switch (closeArg.CloseCondition.PriceSpreadSide)
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

            this.lblPriceSpreadThreshold.Text = closeArg.CloseCondition.PriceSpreadThreshold.ToString();
            this.lblOrderQtyUint.Text = closeArg.OrderQtyUint.ToString();
            this.lblDifferentialUnit.Text = closeArg.DifferentialUnit.ToString();
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
