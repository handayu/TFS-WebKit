using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public partial class PreferentialSideControl : UserControl
    {
        private USeOrderSide m_preferentialSide = USeOrderSide.Buy;

        private Color m_selectedColor = Color.FromArgb(255, 128, 0);
        private Color m_unselectedColor = SystemColors.Control;

        public PreferentialSideControl()
        {
            InitializeComponent();
            SetButtonStyle();
        }

        public USeOrderSide PreferentialSide
        {
            get { return m_preferentialSide; }
            set
            {
                m_preferentialSide = value;
                SetButtonStyle();
            }
        }

        private void SetButtonStyle()
        {
            this.btnBuy.BackColor = GetButtonBackColor(m_preferentialSide == USeOrderSide.Buy);
            this.btnBuy.Text = m_preferentialSide == USeOrderSide.Buy ? "优先买入" : "买入";

            this.btnSell.BackColor = GetButtonBackColor(m_preferentialSide == USeOrderSide.Sell);
            this.btnSell.Text = m_preferentialSide == USeOrderSide.Sell ? "优先卖出" : "卖出";
        }

        private Color GetButtonBackColor(bool isSelected)
        {
            return isSelected ? m_selectedColor : m_unselectedColor;
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            this.PreferentialSide = USeOrderSide.Buy;
        }
        private void btnSell_Click(object sender, EventArgs e)
        {
            this.PreferentialSide = USeOrderSide.Sell;
        }

        
    }
}
