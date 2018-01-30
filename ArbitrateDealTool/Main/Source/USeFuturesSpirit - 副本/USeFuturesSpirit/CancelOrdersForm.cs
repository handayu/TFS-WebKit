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
    public partial class CancelOrdersForm : Form
    {
        private List<USeOrderBook> m_cancelOrdersList = null;
        public CancelOrdersForm(List<USeOrderBook> allCancelOrdersList)
        {
            System.Diagnostics.Debug.Assert(allCancelOrdersList != null);

            m_cancelOrdersList = allCancelOrdersList;
            InitializeComponent();
        }

        private void CancelOrdersForm_Load(object sender, EventArgs e)
        {
            foreach(USeOrderBook order in m_cancelOrdersList)
            {
                string cancelOrderInfo = string.Format("撤单:{0} 开仓方向:{1} 合约:{2} 手数:{3} 价格:{4}\n", order.OrderNum,order.OrderSide.ToDescription(),
                    order.InstrumentCode, order.OrderQty,order.OrderPrice);

                this.richTextBox_CancelOrdersInfo.AppendText(cancelOrderInfo);
            }

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
