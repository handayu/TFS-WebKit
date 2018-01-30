using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public partial class SettlementInfoConfirmForm : Form
    {
        private USeOrderDriver m_orderDriver = null;


        public SettlementInfoConfirmForm(USeOrderDriver orderDriver)
        {
            m_orderDriver = orderDriver;
            InitializeComponent();
        }

        private void SettlementInfoConfirmForm_Load(object sender, EventArgs e)
        {
            this.lblInfo.Text = "请仔细确认结算单,如有疑问请点击取消，并且联系您所在期货公司,[注意]取消后程序将自动退出!";

            try
            {
                string settlementInfo = m_orderDriver.GetSettlementInfo(null);
                this.rtxtSettlementInfo.Text = settlementInfo;
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取结算单信息失败," + ex.Message);
                this.btnOK.Enabled = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                m_orderDriver.SettlementInfoConfirm();
            }
            catch(Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, "结算单确认失败," + ex.Message);
                return;
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
