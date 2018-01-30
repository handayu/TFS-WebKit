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
    public partial class PriceSpreadSideControl : UserControl
    {
        private PriceSpreadSide m_priceSpreadType = PriceSpreadSide.LessOrEqual;

        private Color m_selectedColor = Color.FromArgb(255, 128, 0);
        private Color m_unselectedColor = SystemColors.Control;

        public PriceSpreadSideControl()
        {
            InitializeComponent();
            SetButtonStyle();
        }

        public PriceSpreadSide PriceSpreadSide
        {
            get { return m_priceSpreadType; }
            set
            {
                m_priceSpreadType = value;
                SetButtonStyle();
            }
        }

        private void SetButtonStyle()
        {
            this.btnGreaterOrEqual.BackColor = GetButtonBackColor(m_priceSpreadType == PriceSpreadSide.GreaterOrEqual);
            this.btnLessOrEqual.BackColor = GetButtonBackColor(m_priceSpreadType == PriceSpreadSide.LessOrEqual);
        }

        private Color GetButtonBackColor(bool isSelected)
        {
            return isSelected ? m_selectedColor : m_unselectedColor;
        }

        private void btnGreaterOrEqual_Click(object sender, EventArgs e)
        {
            this.PriceSpreadSide = PriceSpreadSide.GreaterOrEqual;
        }

        private void btnLessOrEqual_Click(object sender, EventArgs e)
        {
            this.PriceSpreadSide = PriceSpreadSide.LessOrEqual;
        }
    }
}
