using System;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;
using Telerik.WinControls.Themes;
using System.Web;
using System.Collections.Generic;
using System.IO;
using mPaint;
using System.Diagnostics;

namespace UseOnlineTradingSystem
{
    public partial class MainForm : RadForm
    {
        private MScrollingText mScrollingText;//跑马灯
        private FormBrowser fbBase;   //基础管理
        private FormBrowser fbMoney;   //资金管理
        private FormBrowser fbDataCenter;   //数据中心
        private FormBrowser fbInformation;   //资讯
        private int counter;
        private FormKLine fl;//K线
        private MUseMainForm table;//行情页面
        private PublishForm m_publishForm;//供需发布窗口

        public MainForm()
        {
            InitializeComponent();
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            this.radDock1.ShowDocumentCloseButton = true;
            this.radDock1.DocumentManager.DocumentCloseActivation = DocumentCloseActivation.FirstInZOrder;

            #region 滚动新闻栏
            mScrollingText = new MScrollingText();
            mScrollingText.Dock = DockStyle.Fill;
            mScrollingText.Cursor = Cursors.Hand;
            mScrollingText.Font = new Font("微软雅黑", 14F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            mScrollingText.ForeColor = Color.White;
            mScrollingText.Location = new Point(0, 0);
            mScrollingText.Name = "mScrollingText";
            mScrollingText.ScrollText = "滚动新闻";
            mScrollingText.Size = new Size(462, 65);
            mScrollingText.Text = "mScrollingText";
            mScrollingText.TextScrollDistance = 2;
            mScrollingText.TextClicked += new Action<MScrollingItem>(MScrollingText_TextClicked);
            panel2.Controls.Add(mScrollingText);

            for (int i = 0; i < 30; i++)
            {
                MScrollingItem item = new MScrollingItem();
                item.Text = "滚动新闻" + i;
                mScrollingText.Items.Add(item);
            }
            #endregion
            //CreatePanel("Tool Window", DockPosition.Right, new Size(this.Width / 2, this.Height / 2), Color.FromArgb(255, 192, 192));
            //CreatePanel("Tool Window", DockPosition.Top, new Size(this.Width / 2, this.Height / 2), Color.FromArgb(192, 255, 192));
            if (File.Exists("default.xml"))
            {
                radDock1.LoadFromXml("default.xml");
            }
            else
            {
                PublishForm windows1 = new PublishForm("供需发布");
                windows1.Initialize();
                this.radDock1.DockWindow(windows1, DockPosition.Bottom);
                this.radDock1.AutoHideWindow(windows1);

                FormBrowser docWindow5 = new FormBrowser("数据中心");
                this.radDock1.AddDocument(docWindow5);

                FormBrowser docWindow6 = new FormBrowser("资讯");
                this.radDock1.AddDocument(docWindow6);

                FormKLine docWindow4 = new FormKLine("K线");
                this.radDock1.AddDocument(docWindow4);

                FormBrowser docWindow2 = new FormBrowser("资金管理");
                this.radDock1.AddDocument(docWindow2);

                FormBrowser docWindow3 = new FormBrowser("基础管理");
                this.radDock1.AddDocument(docWindow3);

                MUseMainForm docWindow1 = new MUseMainForm("行情");
                this.radDock1.AddDocument(docWindow1);

            }

            foreach (DocumentWindow v in this.radDock1.DockWindows.DocumentWindows)
            {
                if (v.Text == "行情")
                {
                    table = v as MUseMainForm;
                    if (table != null)
                    {
                        table.SelectTextChangeEvent +=new Action<object, string>( Table_SelectTextChangeEvent);
                    }
                }
                else if (v.Text == "资金管理")
                {
                    fbMoney = v as FormBrowser;
                }
                else if (v.Text == "基础管理")
                {
                    fbBase = v as FormBrowser;
                }
                else if (v.Text == "数据中心")
                {
                    fbDataCenter = v as FormBrowser;
                }
                else if (v.Text == "资讯")
                {
                    fbInformation = v as FormBrowser;
                }
                else if(v is FormKLine)
                {
                    fl = v as FormKLine;
                }
            }

            foreach (ToolWindow v in this.radDock1.DockWindows.ToolWindows)
            {
                if (v.Text == "供需发布")
                {
                    m_publishForm = v as PublishForm;
                }
            }

            ThemeResolutionService.ApplyThemeToControlTree(this, Program.vt1.ThemeName);
            DataManager.Instance.IsLoginingEvent += Instance_IsLoginingEvent;
            DataManager.Instance.IsLoginEvent += Instance_IsLoginEvent;
            DataManager.Instance.UpdataCommodityInfoEvent += Instance_UpdataCommodityInfoEvent;
            DataManager.Instance.UpdataAllContractEvent += Instance_UpdataAllContractEvent;
            DataManager.Instance.UpdataAllCommodityEvent += Instance_UpdataAllCommodityEvent;
            USeManager.Instance.MQTTService.UpdataMarketDataEvent += MQTTService_UpdataMarketDataEvent;
            //初始化数据
            //DataManager.Instance.InitializationData();
            //USeManager.Instance.Start();

            if (fbDataCenter != null)
            {
                fbDataCenter.InitializeBrowser();
            }
            if (fbInformation != null)
            {
                fbInformation.InitializeBrowser();
            }
        }

        private void Table_SelectTextChangeEvent(object arg1, string arg2)
        {
            rmiKLine_Click(null, null);
        }

        private void MScrollingText_TextClicked(MScrollingItem item)
        {
            if (item != null)
            {
                MessageBox.Show("弹出新闻:" + item.Text);
            }
        }

        /// <summary>
        /// MQTT更新挂牌列表
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        private void Instance_UpdataCommodityInfoEvent(OneListed obj, int type)
        {
            if (table != null)
            {
                table.UpdateCommodity(obj, type);
            }
        }

        /// <summary>
        /// MQTT更新行情数据
        /// </summary>
        /// <param name="obj"></param>
        private void MQTTService_UpdataMarketDataEvent(ContractLastPrice obj)
        {
            if (table != null)
            {
                table.UpdateMarketData(obj);
            }

            if (this.m_publishForm != null)
            {
                this.m_publishForm.TradingInfoCtrol.UpdateData(obj);
            }

        }

        /// <summary>
        /// HTTP重新加载挂牌列表
        /// </summary>
        private void Instance_UpdataAllCommodityEvent()
        {
            if (fbBase != null)
            {
                fbBase.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.BasicManagement));
            }
            if (fbMoney != null)
            {
                fbMoney.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.History));
            }
            //if (fbDataCenter != null)
            //{
            //    fbDataCenter.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.DataCenter));
            //}
            //if (fbInformation != null)
            //{
            //    fbInformation.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.Information));
            //}
            if (table != null)
            {
                table.UpdateTable();
            }
        }

        /// <summary>
        /// HTTP重新加载合约列表
        /// </summary>
        private void Instance_UpdataAllContractEvent()
        {
            var data = DataManager.Instance.GetContractcCategoryVo();
            if (data != null)
            {
                if (radMenu1 != null)
                {
                    radMenu1.Items.Clear();
                    rmiChooseVariety.Items.Clear();//.AddRange(new RadItem[] {this.radMenuItem1});
                    bool choose = true;
                    foreach (var v in data)
                    {
                        RadMenuButtonItem rbi = new RadMenuButtonItem("  " + v.Value.categoryName + "  ");
                        rbi.Click += new EventHandler(Rbi_Click);
                        rbi.Tag = v.Value;
                        radMenu1.Items.Add(rbi);

                        RadMenuItem ri = new RadMenuItem(v.Value.categoryName);
                        ri.Click += new EventHandler(Rbi_Click);
                        ri.Tag = v.Value;
                        rmiChooseVariety.Items.Add(ri);

                        if (choose)
                        {
                            if (table != null)
                            {
                                table.UpdateSelect(v.Value);
                            }
                            DataManager.Instance.CurrentCode = v.Key;
                        }
                        choose = false;
                    }

                }
            }
            if (table != null)
            {
                table.UpdateTable();
            }
        }

        public void UpdateTable()
        {
            if (table != null)
            {
                table.UpdateTable();
            }
        }

        private void Rbi_Click(object sender, EventArgs e)
        {
            RadMenuItemBase mb = sender as RadMenuItemBase;
            if (mb != null)
            {
                ContractCategoryDic cv = mb.Tag as ContractCategoryDic;
                if (cv != null)
                {
                    if (table != null)
                    {
                        table.UpdateSelect(cv);
                    }
                    DataManager.Instance.CurrentCode = cv.categoryCode;
                    if (table != null)
                    {
                        table.UpdateTable();
                    }
                    this.m_publishForm.SetContractIDChanged(DataManager.Instance.CurrentCategory);
                }
            }
        }

        private void Panel1_SizeChanged(object sender, EventArgs e)
        {
            radTitleBar1.Size = new Size(radPanel2.Width / 2, radTitleBar1.Height);
            radTitleBar1.Location = new Point(radPanel2.Width / 2, 0);
            radLabel1.Location = new Point(radTitleBar1.Width - 220, 5);
            radLabel2.Location = new Point(radLabel1.Location.X - radLabel2.Width, 5);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.radDock1.SavedToXml += new EventHandler(this.dockingManager1_LayoutSaved);
            this.radDock1.LoadedFromXml += new EventHandler(this.dockingManager1_LayoutLoaded);
        }

        /// <summary>
        /// 登录中事件
        /// </summary>
        /// <param name="obj"></param>
        private void Instance_IsLoginingEvent(bool obj)
        {
            //设置登录状态
            string value = HttpUtility.UrlEncode(HttpUtility.UrlEncode(DataManager.Instance.Cookies));
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("SecurityTicket", value);
            dic.Add("SecurityToken", value);
            //设置COOKIES
            BsCtl.SetCookie(USeManager.Instance.Address, dic);
        }

        /// <summary>
        /// 登录状态改变消息
        /// </summary>
        /// <param name="islogin"></param>
        private void Instance_IsLoginEvent(bool islogin)
        {
            if (islogin)
            {
                if (DataManager.Instance.LoginInfo != null)
                {
                    this.radLabel1.Text = "您好，" + DataManager.Instance.LoginData.name;
                }
                //刷新
                if (fbBase != null)
                {
                    fbBase.InitializeBrowser();
                }
                if (fbMoney != null)
                {
                    fbMoney.InitializeBrowser();
                }

                if (m_publishForm != null)
                {
                    this.m_publishForm.SetDefultWhenLogin(DataManager.Instance.GetContractCategoryDic("cu"));
                }
            }
            else
            {
                List<string> names = new List<string>();
                names.Add("SecurityToken");
                names.Add("SecurityTicket");
                BsCtl.DelectCookie(USeManager.Instance.Address, names);//删除COOKIES
                this.Hide();
                Program.LoginFm.Show();
            }
        }

        private void dockingManager1_LayoutLoaded(object sender, EventArgs e)
        {
            counter = radDock1.DockWindows.Count;
        }

        private void dockingManager1_LayoutSaved(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radMenuItem33_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML files|*.xml|All files|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //radDock1.CloseWindows(radDock1.DockWindows);
                radDock1.LoadFromXml(dialog.FileName);
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radMenuItem41_Click(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, Program.vt1.ThemeName);
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML files|*.xml|All files|*.*";
            dialog.FileName = "";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                radDock1.SaveToXml(dialog.FileName);
            }
        }
        //注销
        private void radLabel2_MouseClick(object sender, MouseEventArgs e)
        {
            DataManager.Instance.LoginOff();
        }

        private void radLabel2_MouseEnter(object sender, EventArgs e)
        {
            radLabel2.ForeColor = Color.Red;
        }

        private void radLabel2_MouseLeave(object sender, EventArgs e)
        {
            radLabel2.ForeColor = Color.White;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radPanel1_SizeChanged(object sender, EventArgs e)
        {
            int x = radMenu1.Width;
            panel2.Location = new Point(x, this.panel2.Location.Y);
            panel2.Size = new Size(radPanel1.Width - x, panel2.Height);
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.LoginFm != null && !Program.LoginFm.IsDisposed)
            {
                Program.LoginFm.Close();
            }
        }

        /// <summary>
        /// 跳转到修改密码界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiChangePassword_Click(object sender, EventArgs e)
        {
            if (fbBase != null && fbBase.DockState != DockState.Hidden)
            {
                this.radDock1.DockWindow(fbBase, DockPosition.Fill);
                fbBase.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.BasicManagement));
            }
            else
            {
                fbBase = new FormBrowser("基础管理");
                fbBase.InitializeBrowser();
                this.radDock1.AddDocument(fbBase);
            }
        }

        /// <summary>
        /// 自动升级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("尚未开放！");
        }

        /// <summary>
        /// 设置黑白名单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiSetBlackAndWhite_Click(object sender, EventArgs e)
        {
            if (fbBase != null && fbBase.DockState != DockState.Hidden)
            {
                this.radDock1.DockWindow(fbBase, DockPosition.Fill);
                fbBase.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.BlackAndWhiteManagement));
            }
            else
            {
                fbBase = new FormBrowser("基础管理");
                fbBase.InitializeBrowser(Helper.GetURL(HTTPServiceUrlCollection.BlackAndWhiteManagement));
                this.radDock1.AddDocument(fbBase);
            }
        }

        /// <summary>
        /// 账户设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiSetAccount_Click(object sender, EventArgs e)
        {
            if (fbBase != null && fbBase.DockState != DockState.Hidden)
            {
                this.radDock1.DockWindow(fbBase, DockPosition.Fill);
                fbBase.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.AccountManagement));
            }
            else
            {
                fbBase = new FormBrowser("基础管理");
                fbBase.InitializeBrowser(Helper.GetURL(HTTPServiceUrlCollection.AccountManagement));
                this.radDock1.AddDocument(fbBase);
            }
        }

        /// <summary>
        /// 报价牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiPrice_Click(object sender, EventArgs e)
        {
            if (table != null && table.DockState != DockState.Hidden)
            {
                this.radDock1.DockWindow(table, DockPosition.Fill);
            }
            else
            {
                table = new MUseMainForm("行情");
                table.SelectTextChangeEvent += new Action<object, string>(Table_SelectTextChangeEvent);
                this.radDock1.AddDocument(table);
                table.UpdateSelect(DataManager.Instance.CurrentCategory);
                table.UpdateTable();
            }
        }

        /// <summary>
        /// 切换K线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiKLine_Click(object sender, EventArgs e)
        {
            if (fl != null && fl.DockState != DockState.Hidden)
            {
                fl.Text = DataManager.Instance.CurrentContractCode;
                fl.OpenData(DataManager.Instance.CurrentContractCode);
                this.radDock1.DockWindow(fl, DockPosition.Right);
            }
            else
            {
                fl = new FormKLine(DataManager.Instance.CurrentContractCode);
                fl.OpenData(DataManager.Instance.CurrentContractCode);
                this.radDock1.AddDocument(fl);
            }
        }

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                rmiKLine_Click(null, null);
            }
            else if (e.KeyData == Keys.F9)
            {
                rmiA3Home_Click(null, null);
            }
            else if (e.KeyData == Keys.F12)
            {
                rmiA3DataCenter_Click(null, null);
            }
        }

        /// <summary>
        /// 发布报价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiPostQuote_Click(object sender, EventArgs e)
        {
            if (m_publishForm != null && m_publishForm.DockState != DockState.Hidden)
            {
                this.radDock1.FloatWindow(m_publishForm);
                //this.radDock1.DockWindow(m_publishForm, DockPosition.Bottom);
            }
            else
            {
                m_publishForm = new PublishForm("供需发布");
                m_publishForm.Initialize();
                this.radDock1.DockWindow(m_publishForm, DockPosition.Bottom);
                this.radDock1.FloatWindow(m_publishForm);
            }
        }

        /// <summary>
        /// 历史成交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiHistoricalDeal_Click(object sender, EventArgs e)
        {
            if (fbMoney != null && fbMoney.DockState != DockState.Hidden)
            {
                this.radDock1.DockWindow(fbMoney, DockPosition.Fill);
                fbMoney.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.HistoryDeal));
            }
            else
            {
                fbMoney = new FormBrowser("资金管理");
                fbMoney.InitializeBrowser(Helper.GetURL(HTTPServiceUrlCollection.HistoryDeal));
                this.radDock1.AddDocument(fbMoney);
            }
        }

        /// <summary>
        /// 资金流水
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiAccountFunds_Click(object sender, EventArgs e)
        {
            if (fbMoney != null && fbMoney.DockState != DockState.Hidden)
            {
                this.radDock1.DockWindow(fbMoney, DockPosition.Fill);
                fbMoney.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.History));
            }
            else
            {
                fbMoney = new FormBrowser("资金管理");
                fbMoney.InitializeBrowser();
                this.radDock1.AddDocument(fbMoney);
            }
        }

        /// <summary>
        /// 访问A3首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiA3Home_Click(object sender, EventArgs e)
        {
            Process.Start("http://a3.useonline.cn/datachart/html/information/index.html");
        }

        /// <summary>
        /// 资讯窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiA3Information_Click(object sender, EventArgs e)
        {
            if (fbInformation != null && fbInformation.DockState != DockState.Hidden)
            {
                this.radDock1.DockWindow(fbInformation, DockPosition.Fill);
                fbInformation.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.Information));
            }
            else
            {
                fbInformation = new FormBrowser("资讯");
                fbInformation.InitializeBrowser();
                this.radDock1.AddDocument(fbInformation);
            }
        }

        /// <summary>
        /// 当前产品数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiA3DataCenter_Click(object sender, EventArgs e)
        {

            if (fbDataCenter != null && fbDataCenter.DockState != DockState.Hidden)
            {
                this.radDock1.DockWindow(fbDataCenter, DockPosition.Fill);
                fbDataCenter.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.DataCenter));
            }
            else
            {
                fbDataCenter = new FormBrowser("数据中心");
                fbDataCenter.InitializeBrowser();
                this.radDock1.AddDocument(fbDataCenter);
            }
        }

        /// <summary>
        /// 在库融资
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiFinancingInStock_Click(object sender, EventArgs e)
        {
            Process.Start("http://192.168.0.67:9010/");
        }

        /// <summary>
        /// U钱首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiUMoneyHome_Click(object sender, EventArgs e)
        {
            Process.Start("http://192.168.0.67:9010/pay/");
        }

        /// <summary>
        /// 有色在线首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiUseOnlineHome_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.useonline.cn/");
        }

        /// <summary>
        /// Web版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiWeb_Click(object sender, EventArgs e)
        {
            Process.Start("http://"+ USeManager.Instance.Address);
        }

        /// <summary>
        /// 下载交易APP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiDownloadTransactionApp_Click(object sender, EventArgs e)
        {
            Process.Start("http://qian.useonline.cn/");
        }

        /// <summary>
        /// 优点价APP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiDownloadAPP_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.upricing.cn/fineApp");
        }

        /// <summary>
        /// 商城首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiHome_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.upricing.cn/");
        }

        /// <summary>
        /// 免责条款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiDisclaimer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("免责条款！");
        }
        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rmiAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("关于！");
        }
    }
}
