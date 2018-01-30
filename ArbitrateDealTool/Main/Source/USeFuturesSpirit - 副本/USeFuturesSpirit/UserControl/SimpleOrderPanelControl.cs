using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USeFuturesSpirit.ViewModel;
using USe.TradeDriver.Common;
using System.Collections;
using System.Diagnostics;

namespace USeFuturesSpirit
{
    public partial class SimpleOrderPanelControl : UserControl
    {
        //下单板选中合约，事件通知

        /// <summary>
        /// 是否刷新价格
        /// </summary>
        private bool m_freshOrderPrice = false;

        /// <summary>
        /// 当前选中合约。
        /// </summary>
        /// <returns></returns>
        private USeInstrument m_selectedInstrument = null;


        private object m_syncObj = new object();

        public SimpleOrderPanelControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            List<USeInstrumentDetail> instrumentDetailList = USeManager.Instance.OrderDriver.QueryInstrumentDetail();
            List<USeInstrument> instrumentList = (from e in instrumentDetailList
                                                  orderby e.Instrument.InstrumentCode
                                                  select e.Instrument).ToList();
            foreach (USeInstrument instrument in instrumentList)
            {
                this.comboBoxInstrument.Items.Add(instrument);
            }

            m_freshOrderPrice = true;

            USeManager.Instance.QuoteDriver.OnMarketDataChanged += QuoteDriver_OnMarketDataChanged;
        }

        /// <summary>
        /// 更新交易面板的跟踪价格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuoteDriver_OnMarketDataChanged(object sender, USeMarketDataChangedEventArgs e)
        {
            lock (m_syncObj)
            {
                if (m_freshOrderPrice == false) return;
                if (m_selectedInstrument == null) return;
                if (m_selectedInstrument != e.MarketData.Instrument) return;
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeMarketDataChangedEventArgs>(UpddateMarketData), sender, e);
                return;
            }
        }

        private void UpddateMarketData(object sender, USeMarketDataChangedEventArgs e)
        {
            USeOrderSide orderSide = GetChoiceDirection();
            this.labelUpper.Text = e.MarketData.AskPrice.ToString();
            this.labelLower.Text = e.MarketData.BidPrice.ToString();

            USeFundDetail fundDetail = USeManager.Instance.FundCalculator.FundDetail;
            Debug.Assert(fundDetail != null);
            decimal available = fundDetail.Available;
            decimal priceScole = USeManager.Instance.OrderDriver.QueryInstrumentVolumeMultiple(m_selectedInstrument);
            USeMargin useMargin = USeManager.Instance.OrderDriver.QueryInstrumentMargin(m_selectedInstrument);
            decimal margin = 0m;

            if (orderSide == USeOrderSide.Buy)
            {
                this.numericUpDownPrice.Value = e.MarketData.AskPrice;
                margin = (useMargin.BrokerLongMarginRatioByMoney * priceScole * this.numericUpDownPrice.Value) + useMargin.BrokerLongMarginRatioByVolume;
            }
            else if (orderSide == USeOrderSide.Sell)
            {
                this.numericUpDownPrice.Value = e.MarketData.BidPrice;
                margin = (useMargin.BrokerShortMarginRatioByMoney * priceScole * this.numericUpDownPrice.Value) + useMargin.BrokerShortMarginRatioByVolume;
            }
            else
            {
                Debug.Assert(false);
            }
            decimal miniVolumnMoney = margin > 0 ? decimal.Divide(available, margin) : 0;
            this.Label_CanVolumn.Text = (Math.Floor(miniVolumnMoney)).ToString();
        }

        private void SimpleOrderPanelControl_Load(object sender, EventArgs e)
        {
        }

        private void buttonPrice_Click(object sender, EventArgs e)
        {
            if (this.buttonPrice.Text == "跟盘价")
            {
                this.buttonPrice.Text = "指定价";

                this.m_freshOrderPrice = false;
                this.numericUpDownPrice.Enabled = true;
                this.buttonPrice.Text = "指定价";
                this.buttonPrice.BackColor = Color.Transparent;
            }
            else
            {
                this.buttonPrice.Text = "跟盘价";
                this.buttonPrice.BackColor = Color.FromArgb(255, 128, 128);
                this.numericUpDownPrice.Enabled = false;
                this.m_freshOrderPrice = true;
            }
        }

        private USeOffsetType GetChoiceOffsetType()
        {
            USeOffsetType order_offset_type = USeOffsetType.Open;
            if (this.radioButton_Open.Checked)
            {
                order_offset_type = USeOffsetType.Open;
            }
            else if (this.radioButton_CloseToday.Checked)
            {
                order_offset_type = USeOffsetType.CloseToday;
            }
            else if (this.radioButton_CloseYD.Checked)
            {
                order_offset_type = USeOffsetType.CloseHistory;
            }
            else
            {
                Debug.Assert(false);
            }

            return order_offset_type;
        }

        private USeOrderSide GetChoiceDirection()
        {
            USeOrderSide order_side = new USeOrderSide();
            if (this.radioButton_Buy.Checked) order_side = USeOrderSide.Buy;
            if (this.radioButton_Sell.Checked) order_side = USeOrderSide.Sell;
            return order_side;
        }

        private decimal GetMarketOrderPrice()
        {
            //跟盘价
            decimal order_price = 0m;

            switch (GetChoiceDirection())
            {
                case USeOrderSide.Buy:
                    order_price = this.numericUpDownPrice.Value;
                    break;
                case USeOrderSide.Sell:
                    order_price = this.numericUpDownPrice.Value;
                    break;
                default:
                    break;
            }
            return order_price;
        }

        private void buttonOrder_Click(object sender, EventArgs e)
        {
            if (this.comboBoxInstrument.Text == string.Empty)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "合约项为空，请选入合约信息");
                return;
            }

            if (this.numericUpDownPrice.Value <= 0)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "价格为不能为0或负数");
                return;
            }

            if (this.numericUpDownVolume.Value <= 0)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "数量不能为0或负数");
                return;
            }

            decimal orderPrice = 0m;
            if (m_freshOrderPrice)
            {
                orderPrice = GetMarketOrderPrice();
            }
            else
            {
                orderPrice = this.numericUpDownPrice.Value;
            }

            string error_info = string.Empty;
            USeOrderNum orderNum = USeManager.Instance.OrderDriver.PlaceOrder(m_selectedInstrument, (int)this.numericUpDownVolume.Value, GetMarketOrderPrice(), GetChoiceOffsetType(), GetChoiceDirection(), out error_info);
            if (orderNum == null)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, error_info);
            }
        }

        public void ChangeInstrument(USeInstrument instrument)
        {
            if (instrument == null) return;
            lock (m_syncObj)
            {
                m_selectedInstrument = instrument;
                this.comboBoxInstrument.SelectedItem = instrument;
            }
            //信息清空
            this.numericUpDownPrice.Value = 0;
            this.numericUpDownVolume.Value = 0;

        }

        private void SelectIndexChanged(object sender, EventArgs e)
        {
            m_selectedInstrument = this.comboBoxInstrument.SelectedItem as USeInstrument;
        }

    }

}
