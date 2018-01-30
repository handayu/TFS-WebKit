using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI.Docking;

namespace UseOnlineTradingSystem
{
    public partial class ToolWindowBrower : ToolWindow
    {
        private BsCtl wk;
        public ToolWindowBrower() : this("")
        {

        }
        public ToolWindowBrower(string text) : base(text)
        {
            InitializeComponent();
            this.Text = text;
            DefaultFloatingSize = new Size(1000, 500);
        }

        public void InitializeBrowser()
        {
            if (this.Text == "资金管理")
            {
                wk = new BsCtl(this, Helper.GetURL(HTTPServiceUrlCollection.History), "capital");
            }
            else if (this.Text == "基础管理")
            {
                wk = new BsCtl(this, Helper.GetURL(HTTPServiceUrlCollection.BasicManagement), "capital");
            }
            else if (this.Text == "资金分布")
            {

            }
            else if (this.Text == "合约持仓")
            {

            }
            else if (this.Text == "券商持仓")
            {

            }
            else if (this.Text == "天气数据")
            {

            }
            else if (this.Text == "汇率")
            {

            }
            else if (this.Text == "投资日历")
            {

            }
        }

        public void LoadUrl(string url)
        {
            if (wk != null)
            {
                wk.LoadUrl(url);
            }
        }
    }
}
