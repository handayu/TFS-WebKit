using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace USeFuturesSpirit
{
    public partial class ArbitrageOrderViewForm : Form
    {
        private USeArbitrageOrder m_arbitrageOrder = null;

        public ArbitrageOrderViewForm()
        {
            InitializeComponent();
        }

        public ArbitrageOrderViewForm(USeArbitrageOrder arbitrageOrder)
        {
            m_arbitrageOrder = arbitrageOrder;
            InitializeComponent();
        }

        private void ArbitrageOrderViewForm_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            try
            {
                if (m_arbitrageOrder != null)
                {
                    if(m_arbitrageOrder.State <= ArbitrageOrderState.Opened)
                    {
                        this.tabControl1.SelectedTab = this.tpOpen;
                    }
                    else
                    {
                        this.tabControl1.SelectedTab = this.tpClose;
                    }
                    this.Text = string.Format("套利单 [{0}] 详情", m_arbitrageOrder.Alias);
                    this.arbitrageCloseArgumentView2Control1.SetCloseArgument(m_arbitrageOrder.CloseArgument);
                    this.arbitrageOpenArgumentView2Control1.SetOpenArgument(m_arbitrageOrder.OpenArgument);
                    this.openTaskGroupView.SetDataSource(m_arbitrageOrder.OpenTaskGroup);
                    this.closeTaskGroupView.SetDataSource(m_arbitrageOrder.CloseTaskGroup);
                }
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (m_arbitrageOrder == null) return;
            try
            {
                AutoTrader trader = USeManager.Instance.AutoTraderManager.GetAutoTrader(m_arbitrageOrder.TraderIdentify);
                USeArbitrageOrder arbitrageOrder = trader.GetArbitrageOrder();
                Debug.Assert(arbitrageOrder != null);
                m_arbitrageOrder = arbitrageOrder;
                BindData();
            }
            catch(Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }
    }
}
