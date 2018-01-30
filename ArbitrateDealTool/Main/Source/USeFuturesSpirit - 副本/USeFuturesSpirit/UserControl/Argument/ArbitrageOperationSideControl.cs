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

namespace USeFuturesSpirit
{
    public partial class ArbitrageOperationSideControl : UserControl
    {
        public delegate void ArbitrageOperationChangedEventHandle(ArbitrageOperationSide operationSide);

        public event ArbitrageOperationChangedEventHandle OnArbitrageOperationSideChanged;

        private ArbitrageOperationSide m_operationSide = ArbitrageOperationSide.Unknown;

        private Color m_selectedColor = Color.FromArgb(255, 128, 0);
        private Color m_unselectedColor = SystemColors.Control;

        public ArbitrageOperationSideControl()
        {
            InitializeComponent();
            SetButtonStyle();
        }

        public ArbitrageOperationSide OperationSide
        {
            get { return m_operationSide; }
            set
            {
                if (m_operationSide != value)
                {
                    m_operationSide = value;
                    SetButtonStyle();
                    SafeFireArbitrageOperationSideChanged(value);
                }
            }
        }

        private void SetButtonStyle()
        {
            this.btnSellNearBuyFar.BackColor = GetButtonBackColor(m_operationSide == ArbitrageOperationSide.SellNearBuyFar);
            this.btnBuyNearSellFar.BackColor = GetButtonBackColor(m_operationSide == ArbitrageOperationSide.BuyNearSellFar);
        }

        private Color GetButtonBackColor(bool isSelected)
        {
            return isSelected ? m_selectedColor : m_unselectedColor;
        }

        private void btnSellNearBuyFar_Click(object sender, EventArgs e)
        {
            this.OperationSide = ArbitrageOperationSide.SellNearBuyFar;
        }

        private void btnBuyNearSellFar_Click(object sender, EventArgs e)
        {
            this.OperationSide = ArbitrageOperationSide.BuyNearSellFar;
        }

        private void SafeFireArbitrageOperationSideChanged(ArbitrageOperationSide operationSide)
        {
            ArbitrageOperationChangedEventHandle handle = this.OnArbitrageOperationSideChanged;

            if (handle != null)
            {
                try
                {
                    handle(operationSide);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }
    }
}
