using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;
using USe.Common;
using System.Diagnostics;
using System.Threading;

namespace USeFuturesSpirit
{
    public partial class InvestorFundControl : USeUserControl
    {
        private System.Threading.Timer m_updateTimer = null;

        public InvestorFundControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize()
        {
            m_updateTimer = new System.Threading.Timer(QueryAndUpdate, false, Timeout.Infinite, Timeout.Infinite);

            m_updateTimer.Change(500, Timeout.Infinite);
        }

        public void Stop()
        {
            m_updateTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void QueryAndUpdate(object state)
        {
            FundCalculator calculator = USeManager.Instance.FundCalculator;
            try
            {
                USeFundDetail fundDetail = calculator.FundDetail;
                decimal aribtrageOrderUseMargin = USeManager.Instance.AutoTraderManager.CalculatUseMargin();
               
                UpDateAccountInfo(fundDetail,aribtrageOrderUseMargin);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            m_updateTimer.Change(500, Timeout.Infinite);
        }


        private void UpDateAccountInfo(USeFundDetail fundDetail,decimal aribtrageOrderUseMargin)
        {
            if (fundDetail == null) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<USeFundDetail, decimal>(UpDateAccountInfo), fundDetail, aribtrageOrderUseMargin);
                return;
            }

            this.lblStaticBenefit.Text = fundDetail.StaticBenefit.ToString("#,0");
            this.lblCloseProfit.Text = fundDetail.CloseProfit.ToString("#,0");
            this.lblHoldProfit.Text = fundDetail.HoldProfit.ToString("#,0");
            this.lblDynamicBenefit.Text = fundDetail.DynamicBenefit.ToString("#,0");
            this.lblHoldMargin.Text = fundDetail.HoldMargin.ToString("#,0");
            this.lblFronzon.Text = fundDetail.Fronzon.ToString("#,0");
            this.lblAvailable.Text = fundDetail.Available.ToString("#,0");
            this.lblRisk.Text = fundDetail.Risk.ToString("P1");
            this.lblArbitrageOrderMargin.Text = aribtrageOrderUseMargin.ToString("#,0");
        }
    }
}
