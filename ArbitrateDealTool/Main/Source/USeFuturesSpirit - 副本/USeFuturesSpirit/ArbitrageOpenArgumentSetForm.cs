using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USeFuturesSpirit.Arbitrage;

namespace USeFuturesSpirit
{
    public partial class ArbitrageOpenArgumentSetForm : Form
    {
        private AutoTrader m_autoTrader = null;
        private USeArbitrageOrder m_arbitrageOrder = null;

        public ArbitrageOpenArgumentSetForm(AutoTrader autoTrader)
        {
            m_autoTrader = autoTrader;
            InitializeComponent();
        }

        private void ModifyArbitrageOpenArgumentForm_Load(object sender, EventArgs e)
        {
            if (m_autoTrader != null)
            {
                m_arbitrageOrder = m_autoTrader.GetArbitrageOrder();
            }
            if(m_arbitrageOrder != null)
            {
                this.Text = string.Format("套利单[{0}]开仓参数", m_arbitrageOrder.Alias);
                this.arbitrageOrderOpenArgumentViewControl1.SetOpenArgument(m_arbitrageOrder.OpenArgument);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            MessageBox.Show("开仓参数不允许修改");
            return;

            string errorMessage = string.Empty;
            ArbitrageOpenArgument openArg = this.arbitrageOrderOpenArgumentViewControl1.GetOpenArgument(out errorMessage);
            if(openArg == null)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, errorMessage);
                return;
            }

            try
            {
                m_autoTrader.SetOpenArgument(openArg);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
