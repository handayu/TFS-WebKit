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
    public partial class FormBrowser : DocumentWindow
    {
        private BsCtl wk;
        public FormBrowser() : this("")
        {
        }
        public FormBrowser(string text) : base(text)
        {
            InitializeComponent();
            this.Text = text;
        }

        public void InitializeBrowser(string url=null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                if (this.Text == "资金管理")
                {
                    wk = new BsCtl(this, Helper.GetURL(HTTPServiceUrlCollection.History), "capital");
                }
                else if (this.Text == "基础管理")
                {
                    wk = new BsCtl(this, Helper.GetURL(HTTPServiceUrlCollection.BasicManagement), "capital");
                }
                else if (this.Text == "数据中心")
                {
                    wk = new BsCtl(this, Helper.GetURL(HTTPServiceUrlCollection.DataCenter), "capital");
                }
                else if (this.Text == "资讯")
                {
                    wk = new BsCtl(this, Helper.GetURL(HTTPServiceUrlCollection.Information), "capital");
                }
            }
            else
            {
                wk = new BsCtl(this, url, "capital");
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
