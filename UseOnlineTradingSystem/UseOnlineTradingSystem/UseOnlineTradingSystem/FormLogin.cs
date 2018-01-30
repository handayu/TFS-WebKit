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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            try
            {
                this.textBox1.Text = Helper.GetAppConfig("account");
                this.textBox2.Text = Helper.GetAppConfig("password");
            }
            catch
            {

            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            DataManager.Instance.Login(this.textBox1.Text, this.textBox2.Text);
            this.button1.Enabled = true;
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
