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
    public partial class QuerySettlementInfoForm : Form
    {
        private USeOrderDriver m_orderDriver = null;


        public QuerySettlementInfoForm(USeOrderDriver orderDriver)
        {
            m_orderDriver = orderDriver;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string tradingDateStr = this.dateTimePicker_Start.Value.ToString("yyyyMMdd");
                string settlementInfo = m_orderDriver.GetSettlementInfo(tradingDateStr);
                this.rtxtSettlementInfo.Text = settlementInfo;
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取结算单信息失败," + ex.Message);
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {

        }
    }
}
