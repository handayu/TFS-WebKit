using mPaint;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI.Docking;
using UseOnlineTradingSystem;
using UseOnlineTradingSystem.Properties;

namespace UseOnlineTradingSystem
{
    public class MUseMainForm : DocumentWindow
    {
        #region 属性参数定义
        protected MBorad board;//画板
        protected MRectangle mrbk;//第一行背景
        protected MSelect select;//选择框
        protected MLabels lbs1;//买入基价 买入基价值 卖出基价 卖出基价值
        protected MUseTable table;//表格
        #endregion

        private FormHouse fh;//仓库信息
        private DelistBrandForm m_form;//摘牌

        public event Action<object, string> SelectTextChangeEvent;

        public MUseMainForm():this("")
        {

        }
        public MUseMainForm(string text)
        {
            Initialize();
            this.Text = text;
            this.DefaultFloatingSize = new Size(1000, 800);

            table.MouseRightUpEvent += Table_MouseRightUpEvent;
            table.MouseLeftUpEvent += Table_MouseLeftUpEvent;

            //初始化仓库页面
            fh = new FormHouse();

            //初始化摘牌界面
            m_form = new DelistBrandForm();
            m_form.StartPosition = FormStartPosition.Manual;
            m_form.OnDelistSuccessEvent += OnDelistSuccessChangedEvent;
            this.MouseLeave +=new EventHandler( MUseMainForm2_MouseLeave);
        }

        private void MUseMainForm2_MouseLeave(object sender, EventArgs e)
        {
            if (select != null)
            {
                select.SetChoose();
            }
        }

        /// <summary>
        /// 鼠标右击摘牌事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        private void Table_MouseRightUpEvent(object obj, Point p)
        {
            //根据登陆之后获取到的账户类型判断，无论是什么账户，只要是非商城订单都可以摘牌
            if (obj == null) return;
            OneListed ci = obj as OneListed;
            if (ci == null) return;
            //非商城订单
            if(ci.outId == null || ci.outId == "")
            {
                m_form.Location = p;
                m_form.SetCommodityInfo(ci);
                m_form.ShowDialog();
            }
            else
            {
                //商城订单-》1 判断账户ssouser 2 商城订单--》跳转URL
                if(DataManager.Instance.LoginData.isSsoUser == "True")
                {
                    //跳转http://183.6.168.58:6620/main.html
                    string getUrl = Helper.GetURL(HTTPServiceUrlCollection.GetOsserUserURL);
                    System.Diagnostics.Process.Start(getUrl);
                }

                //商城订单-》1 判断账户upricingUser 2 商城订单--》跳转URL
                if (DataManager.Instance.LoginData.isUpricingUser == "True")
                {
                    //跳转http://183.6.168.58:6620/openapi/user/jump?type=product_mall_detail&user_access_token={token}&id={outId}
                    string getURL = string.Format(Helper.GetURL(HTTPServiceUrlCollection.GetUpricingUserURl),
                        DataManager.Instance.Cookies, ci.id);
                    System.Diagnostics.Process.Start(getURL);
                }
            }
        }

        /// <summary>
        /// 鼠标左击指定行事件
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

        /// <summary>
        /// 摘牌成功事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDelistSuccessChangedEvent(object sender, EventArgs e)
        {
            //DataManager.Instance.GetCommodity();
            //刷新
            //this.PublishControl1.TradingInfoCtrol.OnDelistSuccessChangedEvent(sender, e);
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        protected void Initialize()
        {
            #region 窗口阴影
            WinAPI.SetClassLong(this.Handle, WinAPI.GCL_STYLE, WinAPI.GetClassLong(this.Handle, WinAPI.GCL_STYLE) | WinAPI.CS_DropSHADOW); //API函数加载，实现窗体边框阴影效果
            #endregion

            #region 画板初始化
            Logger.LogInfo("画板初始化！");
            RECT defult = new RECT();
            this.board = new MBorad();
            this.board.Dock = DockStyle.Fill;
            this.board.Location = new Point(0, 0);
            this.board.Name = "cBoard";
            this.board.BackColor = Color.Black;
            this.board.Click += Board_Click;
            this.board.DoubleClick += Board_DoubleClick;
            this.board.MouseWheel += Board_MouseWheel;

            this.Controls.Add(board);
            #endregion

            #region 第二行    

            #region 买卖基价
            lbs1 = new MLabels();
            Logger.LogInfo("初始化买入基价！");
            MLabel lbbuy = new MLabel();
            lbbuy.Text = "买入基价：";
            lbbuy.ForeColor = COLOR.RGB(Color.Red);
            lbbuy.BackColor = -1;
            lbbuy.Font = MCommonData.d2Font;
            lbbuy.LeftAligned = true;
            lbs1.lbs.Add(lbbuy);

            Logger.LogInfo("初始化买入基价值！");
            MLabel lbbuyvalue = new MLabel();
            lbbuyvalue.Text = "     ";
            lbbuyvalue.ForeColor = COLOR.RGB(Color.White);
            lbbuyvalue.BackColor = -1;
            lbbuyvalue.Font = MCommonData.d3Font;
            lbbuyvalue.LeftAligned = true;
            lbs1.lbs.Add(lbbuyvalue);

            Logger.LogInfo("初始化卖出基价！");
            MLabel lbsell = new MLabel();
            lbsell.Text = "卖出基价：";
            lbsell.ForeColor = COLOR.RGB(Color.Green);
            lbsell.BackColor = -1;
            lbsell.Font = MCommonData.d2Font;
            lbsell.LeftAligned = true;
            lbs1.lbs.Add(lbsell);

            Logger.LogInfo("初始化卖出基价值！");
            MLabel lbsellvalue = new MLabel();
            lbsellvalue.Text = "     ";
            lbsellvalue.ForeColor = COLOR.RGB(Color.White);
            lbsellvalue.BackColor = -1;
            lbsellvalue.Font = MCommonData.d3Font;
            lbsellvalue.LeftAligned = true;
            lbs1.lbs.Add(lbsellvalue);
            this.board.AddControl(lbs1);

            #endregion

            #endregion

            #region 表格

            table = new MUseTable();
            this.board.AddControl(table);

            #endregion

            #region 选择框

            Logger.LogInfo("初始化第二行快捷按钮！");
            select = new MSelect();
            select.BackgroundImage = Resources.select_normal;
            select.MouseClickImage = Resources.select_press;
            select.MouseEnterImage = Resources.select_normal;
            select.Font = MCommonData.d4Font;
            select.ForeColor = COLOR.RGB(MCommonData.fontColor4);
            select.DropDownBoxForeColor = COLOR.RGB(MCommonData.fontColor5);
            select.DropDownBoxBackColor = COLOR.RGB(MCommonData.fontColor4);
            select.DropDownBoxRowMouseEnterColor = COLOR.RGB(MCommonData.fontColor13);
            select.Text = "请选择";
            select.TextChangeEvent += Select_TextChangeEvent;
            this.board.AddControl(select);
            #endregion       

            Initialized();

            this.SizeChanged += Form1_SizeChanged;

            Draw();
        }

        private void Select_TextChangeEvent(object sender, string text)
        {
            if (text != DataManager.Instance.CurrentContractCode)
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
                            UpdateMarketData(d);
                        }
                        else
                        {
                            var vv = DataManager.Instance.GetContractLastPrice(text);
                            if (vv != null)
                            {
                                UpdateMarketData(vv.contractMonth, vv.bidPrice, vv.askPrice);
                            }

                            ////同时刷一遍供需发布
                            //if (this.PublishControl1 != null && DataManager.Instance != null)
                            //{
                            //    this.PublishControl1.SetDefultWhenLogin(DataManager.Instance.GetContractCategoryDic("cu"));
                            //}
                        }
                    }
                }
                else
                {
                    UpdateMarketData(null);
                }
                SelectTextChangeEvent?.Invoke(sender, text);
            }
        }

        public void UpdateCommodity(OneListed obj, int type)
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

        public void UpdateMarketData(ContractLastPrice obj)
        {
            if (DataManager.Instance.CurrentCode != null && obj != null)
            {
                if (DataManager.Instance.CurrentCode == obj.category && DataManager.Instance.CurrentContractCode == obj.contractMonth)
                {
                    if (obj != null)
                    {
                        if (lbs1 != null && lbs1.lbs.Count >= 4)
                        {
                            lbs1.lbs[1].Text = obj.bidPrice.ToString();
                            lbs1.lbs[3].Text = obj.askPrice.ToString();
                        }
                    }
                    else
                    {
                        lbs1.lbs[1].Text = "0";
                        lbs1.lbs[3].Text = "0";
                        table.Updata("", 0, 0);
                    }
                }
                //if (this.PublishControl1 != null)
                //{
                //    this.PublishControl1.TradingInfoCtrol.UpdateData(obj);
                //}
                table.Updata(obj.category, obj.bidPrice, obj.askPrice);
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
        public void UpdateMarketData(string category,float buyPrice, float sellPrice)
        {
            lbs1.lbs[1].Text = buyPrice.ToString();
            lbs1.lbs[3].Text = sellPrice.ToString();
            table.Updata(category, buyPrice, sellPrice);
            Draw();
        }
        public void UpdateSelect(ContractCategoryDic cv)
        {
            select.Items.Clear();
            if (cv != null)
            {
                foreach (var v in cv.contractMonthMap)
                {
                    select.Items.Add(v.Value.categoryName);
                }
            }
            if (select.Items.Count > 0)
            {
                select.Text = select.Items[0];
            }
            else
            {
                select.Text = "请选择";
            }
            Draw();
        }
        #region 事件

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Board_Click(object sender, EventArgs e)
        {
            if (select != null)
            {
                select.SetChoose();
            }
            //if (image3 != null)
            //{
            //    image3.Visible = false;
            //}
            Draw();
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Board_DoubleClick(object sender, EventArgs e)
        {
            //if (this.WindowState == FormWindowState.Maximized)
            //{
            //    this.WindowState = FormWindowState.Normal;
            //}
            //else
            //{
            //    this.WindowState = FormWindowState.Maximized;
            //}
        }

    
        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Board_MouseWheel(object sender, MouseEventArgs e)
        {
            FormMouseWheel(e);
            Draw();
        }

        #endregion

        /// <summary>
        /// 绘图
        /// </summary>
        protected void Draw()
        {
            if (board == null)
            {
                return;
            }
            //修改画板大小
            this.board.Size = this.Size;

            #region 第二行 
            int secondRow2X = this.Width - 340;
            int secondRow1Y = 10;
            int secondRow2Y = secondRow1Y + 35;

            if (secondRow2X > 0)
            {
                //选择框
                select.ImageRectangle = new RECT(secondRow2X - 160, secondRow1Y + 2, secondRow2X - 160 + 112, secondRow1Y + 2 + 30);
                //买入基价标签位置
                lbs1.lbs[0].Rectangle = new RECT(secondRow2X, secondRow1Y, secondRow2X + 90, secondRow2Y);
                //买入基价值位置
                lbs1.lbs[1].Rectangle = new RECT(secondRow2X + 70, secondRow1Y, secondRow2X + 150, secondRow2Y);
                //卖出基价标签位置
                lbs1.lbs[2].Rectangle = new RECT(secondRow2X + 150, secondRow1Y, secondRow2X + 240, secondRow2Y);
                //卖出基价值位置
                lbs1.lbs[3].Rectangle = new RECT(secondRow2X + 220, secondRow1Y, secondRow2X + 300, secondRow2Y);
            }
            #endregion

            #region 绘制表格

            int tableX =10;//表格左边X坐标
            int tableY = secondRow2Y + 10;//表格左上角Y坐标
            int table2X = this.Width - 30;//表头背景右边X坐标
            int table2Y = this.Height;//表头背景右下角Y坐标
            table.Rectangle = new RECT(tableX, tableY, table2X, table2Y);
            #endregion

            this.board.Draw();
        }

        /// <summary>
        /// 窗口大小改变时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            table.Initialization();
            Draw();
            FormSizeChanged();
        }

        #region 可重写方法

        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        protected virtual void FormMouseWheel(MouseEventArgs e) { }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Initialized() { }

        protected virtual void SelectTextChange(object sender, string text)
        {

        }
        protected virtual void BtnWriteOffMouseDown()
        {

        }
        protected virtual void FormSizeChanged()
        {

        }

        #endregion
    }
}
