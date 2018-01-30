using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace USeFuturesSpirit
{
    public partial class OrderPriceTypeControl : UserControl
    {
        private ArbitrageOrderPriceType m_orderPriceType = ArbitrageOrderPriceType.OpponentPrice;

        private Color m_selectedColor = Color.FromArgb(255, 128, 0);
        private Color m_unselectedColor = SystemColors.Control;

        public OrderPriceTypeControl()
        {
            InitializeComponent();
            SetButtonStyle();
        }

        public ArbitrageOrderPriceType OrderPriceType
        {
            get { return m_orderPriceType; }
            set
            {
                m_orderPriceType = value;
                SetButtonStyle();
            }
        }

        private void SetButtonStyle()
        {
            this.btnLastPrice.BackColor = GetButtonBackColor(m_orderPriceType == ArbitrageOrderPriceType.LastPrice);
            this.btnOpponentPrice.BackColor = GetButtonBackColor(m_orderPriceType == ArbitrageOrderPriceType.OpponentPrice);
            this.btnQueuePrice.BackColor = GetButtonBackColor(m_orderPriceType == ArbitrageOrderPriceType.QueuePrice);
        }

        private Color GetButtonBackColor(bool isSelected)
        {
            return isSelected ? m_selectedColor : m_unselectedColor;
        }

        private void btnLastPrice_Click(object sender, EventArgs e)
        {
            this.OrderPriceType = ArbitrageOrderPriceType.LastPrice;
        }

        private void btnOpponentPrice_Click(object sender, EventArgs e)
        {
            this.OrderPriceType = ArbitrageOrderPriceType.OpponentPrice;
        }

        private void btnQueuePrice_Click(object sender, EventArgs e)
        {
            this.OrderPriceType = ArbitrageOrderPriceType.QueuePrice;
        }
    }
}
