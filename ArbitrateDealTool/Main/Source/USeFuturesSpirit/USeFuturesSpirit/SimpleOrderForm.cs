using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace USeFuturesSpirit
{
    public partial class SimpleOrderForm : Form
    {
        public SimpleOrderForm()
        {
            InitializeComponent();
        }

        public SimpleOrderPanelControl GetSimpleOrderPanelControl
        {
            get
            {
                if(simpleOrderPanelControl1 != null) return this.simpleOrderPanelControl1;
                return null;
            }
        }

        private void SimpleOrderForm_Load(object sender, EventArgs e)
        {
            this.simpleOrderPanelControl1.Initialize();
        }

        private void SimpleOrderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
