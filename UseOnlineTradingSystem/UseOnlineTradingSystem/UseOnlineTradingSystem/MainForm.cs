using mPaint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using UseHttpHelper;
using UseHttpHelper.Helper;
using Xilium.CefGlue;

namespace UseOnlineTradingSystem
{
    public partial class MainForm : MUseMainForm
    {
        private BsCtl wk1;   //内嵌浏览器1
        private BsCtl wk2;   //内嵌浏览器2
        private FormLogin LoginFm;//登录窗口
        private FormHouse fh;//仓库信息
        private PublishControl PublishControl1;//供需发布

        private PublishForm publishForm;//供需发布窗口

        private DelistBrandForm m_form;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Logger.LogInfo("初始化界面开始！");
            Initialize();
            Logger.LogInfo("初始化界面结束！");
            DataManager.Instance.IsLoginingEvent += Instance_IsLoginingEvent;
            DataManager.Instance.IsLoginEvent += Instance_IsLoginEvent;
            DataManager.Instance.UpdataCommodityInfoEvent += Instance_UpdataCommodityInfoEvent;
            DataManager.Instance.UpdataAllContractEvent += Instance_UpdataAllContractEvent;
            DataManager.Instance.UpdataAllCommodityEvent += Instance_UpdataAllCommodityEvent;
            //初始化数据
            DataManager.Instance.InitializationData();

            USeManager.Instance.Start();
            USeManager.Instance.MQTTService.UpdataMarketDataEvent += MQTTService_UpdataMarketDataEvent;

            table.MouseRightUpEvent += Table_MouseRightUpEvent;
            table.MouseLeftUpEvent += Table_MouseLeftUpEvent;

            Logger.LogInfo("初始化供需发布！");
            //PublishControl1 = new PublishControl();
            //PublishControl1.Dock = DockStyle.Fill;
            //PublishControl1.Location = new Point(0, 0);
            //PublishControl1.Name = "PublishControl1";
            //PublishControl1.Size = new Size(1487, 409);
            //PublishControl1.TabIndex = 0;
            //panel1.Controls.Add(PublishControl1);
            //PublishControl1.DisposeFormEvent += PublishControl_Dispose;

            #region Form
            publishForm = new PublishForm();
            publishForm.Dock = DockStyle.Fill;
            publishForm.Location = new Point(0, 0);
            publishForm.Name = "PublishForm";
            //publishForm.Size = new Size(1487, 409);
            publishForm.TabIndex = 0;
            //panel1.Controls.Add(publishForm);
            #endregion

            m_form = new DelistBrandForm();
            m_form.StartPosition = FormStartPosition.Manual;
            m_form.OnDelistSuccessEvent += OnDelistSuccessChangedEvent;

            Logger.LogInfo("创建登录界面！");
            LoginFm = new FormLogin();//登录2
            LoginFm.TopMost = true;
            LoginFm.Show();

            Logger.LogInfo("初始化内置浏览器！");
            //初始化仓库页面
            fh = new FormHouse();

            //资金
            this.panel3.Location = new Point(0, 40);
            this.panel3.Width = this.Width;
            this.panel3.Height = this.Height - 40 - 30;
            wk1 = new BsCtl(this.panel3, Helper.GetURL(HTTPServiceUrlCollection.History), "capital");

            //基础管理
            this.panel4.Location = new Point(0, 40);
            this.panel4.Width = this.Width;
            this.panel4.Height = this.Height - 40 - 30;
            wk2 = new BsCtl(this.panel4, Helper.GetURL(HTTPServiceUrlCollection.BasicManagement), "capital");
        }

        /// <summary>
        /// 供需发布隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PublishControl_Dispose(object sender, EventArgs e)
        {
            //隐藏供需发布同时跳转行情
            //this.panel1.Visible = false;

            publishForm.Close();
        }

        /// <summary>
        /// 鼠标右击仓库详细信息事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        private void Table_MouseRightUpEvent(object obj, Point p)
        {
            if (obj == null) return;
            OneListed ci = obj as OneListed;
            if (ci == null) return;
            m_form.Location = p;
            m_form.SetCommodityInfo(ci);
            m_form.ShowDialog();
        }

        /// <summary>
        ///  鼠标左击指定行事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        private void Table_MouseLeftUpEvent(object obj, Point p)
        {
            if (obj == null) return;
            OneListed ci = obj as OneListed;
            if (ci == null) return;
            string url = Helper.GetURL(HTTPServiceUrlCollection.GetWareHouseInfoUrl, ci.warehouseId);
            fh.SetHouse(url);
            fh.Show();
        }

        public void OnDelistSuccessChangedEvent(object sender, EventArgs e)
        {
            //DataManager.Instance.GetCommodity();
            //刷新
            //this.PublishControl1.TradingInfoCtrol.OnDelistSuccessChangedEvent(sender, e);

            this.publishForm.TradingInfoCtrol.OnDelistSuccessChangedEvent(sender, e);

        }

        /// <summary>
        /// 更新挂牌列表
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        private void Instance_UpdataCommodityInfoEvent(OneListed obj, int type)
        {
            var current = DataManager.Instance.CurrentCategory;
            if (obj != null && current != null && current.id == obj.cid)
            {
                //新增
                if (type == 0)
                {
                    //买
                    table.InsertData(obj, 0);
                    this.BeginInvoke((MethodInvoker)delegate { this.Draw(); });
                }
                else if (type == 1)
                {
                    //卖
                    table.InsertData(obj, 1);
                    this.BeginInvoke((MethodInvoker)delegate { this.Draw(); });
                }
                else if (type == 2)
                {
                    //更新
                    table.Updata(obj);
                    this.BeginInvoke((MethodInvoker)delegate { this.Draw(); });
                }
            }
        }

        /// <summary>
        /// 更新行情数据
        /// </summary>
        /// <param name="obj"></param>
        private void MQTTService_UpdataMarketDataEvent(ContractLastPrice obj)
        {
            if (DataManager.Instance.CurrentCode != null && obj != null)
            {
                if (DataManager.Instance.CurrentCode == obj.category && DataManager.Instance.CurrentContractCode == obj.contractMonth)
                {
                    UpdataMarketData(obj);
                }
                //if (this.PublishControl1 != null)
                //{
                //    this.PublishControl1.TradingInfoCtrol.UpdateData(obj);
                //}

                if (this.publishForm != null)
                {
                    this.publishForm.TradingInfoCtrol.UpdateData(obj);
                }

                table.Updata(obj.category, obj.bidPrice, obj.askPrice);
            }
        }

        /// <summary>
        /// 重新加载挂牌列表
        /// </summary>
        private void Instance_UpdataAllCommodityEvent()
        {
            if (wk1 != null && wk2 != null)
            {
                wk1.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.History));
                wk2.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.BasicManagement));
            }
            UpdateTable();
        }

        /// <summary>
        /// 重新加载合约列表
        /// </summary>
        private void Instance_UpdataAllContractEvent()
        {
            var data = DataManager.Instance.GetContractcCategoryVo();
            if (data != null)
            {
                ClearFastBtns();
                bool choose = true;
                foreach (var v in data)
                {
                    AddFastBtns(v.Value, choose);
                    if (choose)
                    {
                        UpdataSelect(v.Value);
                        DataManager.Instance.CurrentCode = v.Key;
                    }
                    choose = false;
                }
                this.BeginInvoke((MethodInvoker)delegate { this.Draw(); });
            }
            UpdateTable();
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
            dic.Add("SecurityTicket", value);
            dic.Add("SecurityToken", value);
            //设置COOKIES
            BsCtl.SetCookie(USeManager.Instance.Address, dic);

            //刷新
            if (wk1 != null && wk2 != null)
            {
                wk1.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.History));
                wk2.LoadUrl(Helper.GetURL(HTTPServiceUrlCollection.BasicManagement));
            }
        }

        /// <summary>
        /// 登录状态改变消息
        /// </summary>
        /// <param name="islogin"></param>
        private void Instance_IsLoginEvent(bool islogin)
        {
            if (islogin)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    if (LoginFm != null)
                    {
                        LoginFm.Hide();//登录2
                    }
                    USeManager.Instance.MQTTService.UpdataMarketDataEvent += MQTTService_UpdataMarketDataEvent;
                    UpdateTable();
                    UpdataLogin(true);
                    Draw();
                    //登陆之后拉挂牌全量
                    //this.PublishControl1.SetDefultWhenLogin(DataManager.Instance.GetContractCategoryDic("cu"));

                    this.publishForm.SetDefultWhenLogin(DataManager.Instance.GetContractCategoryDic("cu"));

                });
            }
            else
            {
                this.panel1.Visible = false;
                this.panel3.Visible = false;
                this.panel4.Visible = false;
                btns1.SetChoose(0);
                btns5.SetChoose(0);
                USeManager.Instance.MQTTService.UpdataMarketDataEvent += MQTTService_UpdataMarketDataEvent;
                List<string> names = new List<string>();
                names.Add("SecurityToken");
                names.Add("SecurityTicket");
                BsCtl.DelectCookie(USeManager.Instance.Address, names);//删除COOKIES

                UpdataLogin(false);

                DataManager.Instance.GetCommodity();
            }
        }

        /// <summary>
        /// 更新表格
        /// </summary>
        public void UpdateTable()
        {
            if (DataManager.Instance.CurrentCategory != null)
            {
                var current = DataManager.Instance.CurrentCategory;
                List<OneListed> list = DataManager.Instance.SortCommodityData();
                table.ClearData();
                foreach (var v in list)
                {
                    if (current.id == v.cid)
                    {
                        if (v.transStatus == "1" || v.transStatus == "2")
                        {
                            //0：采购 1：销售
                            table.InsertData(v, v.transType);
                            this.BeginInvoke((MethodInvoker)delegate { this.Draw(); });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 第一列K线图按钮
        /// </summary>
        protected override void KlineBtnClick()
        {
            btns5.SetChoose(0);
            MessageBox.Show("功能暂未开放");
        }

        /// <summary>
        /// 第一列资讯按钮
        /// </summary>
        protected override void InformationBtnClick()
        {
            btns5.SetChoose(0);
            MessageBox.Show("功能暂未开放");
        }

        /// <summary>
        /// 第一列供需发布按钮
        /// </summary>
        protected override void SupplyAndDemandClick()
        {
            if (DataManager.Instance.IsLogin)
            {
                //PublishControl1.Initialize();
                //this.panel1.Visible = true;

                publishForm = new PublishForm();
                publishForm.Initialize();
                publishForm.Show();
                //this.panel1.Visible = true;
            }
            else
            {
                btns5.SetChoose(0);
                MessageBox.Show("请登录！");
            }
        }

        /// <summary>
        /// 行情按钮
        /// </summary>
        protected override void QuotesClick()
        {
            this.panel1.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            DataManager.Instance.GetWhiteAndBlack();
            DataManager.Instance.InitializationData();
            if (DataManager.Instance.CurrentCategory != null)
            {
                //this.PublishControl1.SetContractIDChanged(DataManager.Instance.CurrentCategory);
                this.publishForm.SetContractIDChanged(DataManager.Instance.CurrentCategory);

            }
        }

        /// <summary>
        /// 基础管理按钮点击事件
        /// </summary>
        protected override void BasicManagementClick()
        {
            if (DataManager.Instance.IsLogin)
            {
                this.panel3.Visible = false;
                this.panel4.Visible = true;
                btns5.SetChoose(0);
            }
            else
            {
                btns1.SetChoose(0);
                MessageBox.Show("请登录！");
            }
        }

        /// <summary>
        /// 我的资金按钮点击事件
        /// </summary>
        protected override void MyMoneyClick()
        {
            if (DataManager.Instance.IsLogin)
            {
                this.panel3.Visible = true;
                this.panel4.Visible = false;
                btns5.SetChoose(0);
            }
            else
            {
                MessageBox.Show("请登录！");
                btns1.SetChoose(0);
            }
        }

        /// <summary>
        /// 登录按钮事件
        /// </summary>
        protected override void BtnloginMouseDown()
        {
            if (DataManager.Instance.IsLogin)
            {
                MessageBox.Show("已登录！");
                return;
            }
            LoginFm.Show();
        }

        /// <summary>
        /// 注册按钮事件
        /// </summary>
        protected override void BtnregisteredMouseDown()
        {
            System.Diagnostics.Process.Start(Helper.GetURL(HTTPServiceUrlCollection.Registered));
        }

        /// <summary>
        /// 注销按钮事件
        /// </summary>
        protected override void BtnWriteOffMouseDown()
        {
            DataManager.Instance.LoginOff();
        }

        /// <summary>
        /// 快捷菜单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void FastBtnClick(object sender, MMouseEventArgs e)
        {
            MButton mb = sender as MButton;
            if (mb != null)
            {
                ContractCategoryDic cv = mb.Tag as ContractCategoryDic;
                if (cv != null && mb.hasChoose)
                {
                    UpdataSelect(cv);
                    DataManager.Instance.CurrentCode = cv.categoryCode;
                    UpdateTable();
                    //this.PublishControl1.SetContractIDChanged(cv);
                    this.publishForm.SetContractIDChanged(cv);

                }
            }
        }

        /// <summary>
        /// 选择框改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="text"></param>
        protected override void SelectTextChange(object sender, string text)
        {
            DataManager.Instance.CurrentContractCode = text;
            //立刻取一次更新
            if (text != "请选择")
            {
                if (text != null)
                {
                    var d = DataManager.Instance.GetContractLastPrice(text);
                    if (d == null)
                    {
                        UpdataMarketData(d);
                    }
                    else
                    {
                        var vv = DataManager.Instance.GetContractLastPrice(text);
                        if (vv != null)
                            UpdataMarketData(vv.contractMonth, vv.bidPrice, vv.askPrice);

                        //同时刷一遍供需发布
                        //if(this.PublishControl1 != null && DataManager.Instance != null)
                        //{
                        //    this.PublishControl1.SetDefultWhenLogin(DataManager.Instance.GetContractCategoryDic("cu"));
                        //}

                        if (this.publishForm != null && DataManager.Instance != null)
                        {
                            this.publishForm.SetDefultWhenLogin(DataManager.Instance.GetContractCategoryDic("cu"));
                        }
                    }
                }
            }
            else
            {
                UpdataMarketData(null);
            }
        }

        /// <summary>
        /// 窗口大小改变事件
        /// </summary>
        protected override void FormSizeChanged()
        {
            if (this.panel3 != null)
            {
                this.panel3.Size = new Size(this.Width, this.Height - this.panel3.Location.Y);
            }
            if (this.panel4 != null)
            {
                this.panel4.Size = new Size(this.Width, this.Height - this.panel4.Location.Y);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (LoginFm != null)
                LoginFm.Close();
            if (fh != null)
                fh.Close();
            if (m_form != null)
                m_form.Close();
            if (publishForm != null)
                publishForm.Close();
        }
    }
}
