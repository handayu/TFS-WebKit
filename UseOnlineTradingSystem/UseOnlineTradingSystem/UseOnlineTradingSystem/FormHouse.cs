using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseOnlineTradingSystem
{
    public partial class FormHouse : Form
    {
        public FormHouse()
        {
            InitializeComponent();
        }

        private  BsCtl bc=null;
        private void FormHouse_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            //bc = new BsCtl(this, "http://172.16.88.152/tradesystem/pay/warehouse/detail.html?U2FsdGVkX18UeQa5mODXtWnBD3k3Lga5DKAfETU6SUE=", "capital");
        }

        public void SetHouse(string url)
        {
            if (bc != null)
            {
                bc.LoadUrl(url);
            }
            else
            {
                bc = new BsCtl(this, url, "capital");
            }
        }

        private void FormHouse_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
