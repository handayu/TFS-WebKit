using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using UseOnlineTradingSystem;
using UseOnlineTradingSystem.Properties;

namespace mPaint
{
    public class MUseMainForm : Form
    {
        #region 属性参数定义
        protected MBorad board;//画板
        protected MRectangle mrbk;//第一行背景
        protected MButtons btns1;//行情按钮 历史交易按钮 基础管理按钮
        protected MButtons btns2;//注册按钮 登录按钮
        protected MLabel lblogin;//登录后状态
        protected MButton btnloginoff;//注销按钮

        protected MButtons btns3;//最小化按钮 最大化按钮 关闭按钮
        protected MButtons btns4;//快捷按钮
        protected MButtons btns5;//左侧边栏:行情 供需发布 K线图
        protected MSelect select;//选择框
        protected MLabels lbs1;//买入基价 买入基价值 卖出基价 卖出基价值
        protected MUseTable table;//表格
        #endregion

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

            #region 第一行 
            Logger.LogInfo("初始化第一行背景！");
            mrbk = new MRectangle();
            mrbk.BackColor = COLOR.RGB(MCommonData.fontColor5);
            this.board.AddControl(mrbk);

            #region 按钮
            btns1 = new MButtons();

            Logger.LogInfo("初始化第一行行情按钮！");
            MLineButton btnhq = new MLineButton();
            btnhq.Font = MCommonData.d2Font;
            btnhq.Text = "行情";
            btnhq.ForeColor = COLOR.RGB(MCommonData.fontColor1);
            btnhq.MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
            btnhq.hasChoose = true;
            btnhq.MouseDown += Btnhq_Click;
            btns1.btns.Add(btnhq);

            Logger.LogInfo("初始化第一行我的资金按钮！");
            MLineButton btnls = new MLineButton();
            btnls.Font = MCommonData.d2Font;
            btnls.Text = "我的资金";
            btnls.ForeColor = COLOR.RGB(MCommonData.fontColor1);
            btnls.MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
            btnls.MouseDown += Btnls_Click;
            btns1.btns.Add(btnls);

            Logger.LogInfo("初始化第一行基础管理按钮！");
            MLineButton btnjc = new MLineButton();
            btnjc.Font = MCommonData.d2Font;
            btnjc.Text = "基础管理";
            btnjc.ForeColor = COLOR.RGB(MCommonData.fontColor1);
            btnjc.MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
            btnjc.MouseDown += Btnjc_Click;
            btns1.btns.Add(btnjc);
            this.board.AddControl(btns1);

            btns2 = new MButtons();
            Logger.LogInfo("初始化第一行登录按钮！");
            MButton btnlogin = new MButton();
            btnlogin.MouseClickImage = Resources.icon_login_hover;
            btnlogin.BackgroundImage = Resources.icon_login; ;
            btnlogin.MouseEnterImage = Resources.icon_login_hover;
            btnlogin.hasFrame = false;
            btnlogin.MouseDown += Btnlogin_MouseDown; ;
            btns2.btns.Add(btnlogin);

            Logger.LogInfo("初始化第一行注册按钮！");
            MButton btnregistered = new MButton();
            btnregistered.MouseClickImage = Resources.icon_registered_hover;
            btnregistered.BackgroundImage = Resources.icon_registered; ;
            btnregistered.MouseEnterImage = Resources.icon_registered_hover;
            btnregistered.hasFrame = false;
            btnregistered.MouseDown += Btnregistered_MouseDown; ;
            btns2.btns.Add(btnregistered);
            this.board.AddControl(btns2);

            Logger.LogInfo("初始化第一行登录后状态！");
            lblogin = new MLabel();
            lblogin.Text = "您好， ";
            lblogin.ForeColor = COLOR.RGB(Color.White);
            lblogin.BackColor = COLOR.RGB(MCommonData.fontColor5);
            lblogin.Font = MCommonData.d4Font;
            lblogin.LeftAligned = true;
            lblogin.Visible = false;
            this.board.AddControl(lblogin);

            Logger.LogInfo("初始化第一行注销按钮！");
            btnloginoff = new MButton();
            btnloginoff.Text = "注销";
            btnloginoff.ForeColor = COLOR.RGB(Color.White);
            btnloginoff.MouseForeColor = COLOR.RGB(MCommonData.fontColor8);
            btnloginoff.BackColor = COLOR.RGB(MCommonData.fontColor5);
            btnloginoff.MouseBackColor = COLOR.RGB(MCommonData.fontColor5);
            btnloginoff.MouseClickBackColor = COLOR.RGB(MCommonData.fontColor5);
            btnloginoff.Font = MCommonData.d4Font;
            btnloginoff.hasFrame = false;
            btnloginoff.Visible = false;
            btnloginoff.MouseDown += BtnWriteOff_MouseDown; ; ;
            this.board.AddControl(btnloginoff);


            btns3 = new MButtons();
            Logger.LogInfo("初始化第一行最小化按钮！");
            MButton btnmin = new MButton();
            btnmin.BackgroundImage = Resources.icon_mini;
            btnmin.MouseEnterImage = Resources.icon_mini_hover;
            btnmin.MouseClickImage = Resources.icon_mini_hover;
            btnmin.ImageSIZE = new SIZE(12, 2);
            btnmin.MouseDown += Btnmin_Click;
            btns3.btns.Add(btnmin);

            Logger.LogInfo("初始化第一行最大化按钮！");
            MButton btnmax = new MButton();
            btnmax.BackgroundImage = Resources.icon_max;
            btnmax.MouseEnterImage = Resources.icon_max_hover;
            btnmax.MouseClickImage = Resources.icon_max_hover;
            btnmax.MouseDown += Btnmax_Click;
            btns3.btns.Add(btnmax);

            Logger.LogInfo("初始化第一行关闭按钮！");
            MButton btnclose = new MButton();
            btnclose.BackgroundImage = Resources.icon_close;
            btnclose.MouseEnterImage = Resources.icon_close_hover;
            btnclose.MouseClickImage = Resources.icon_close_hover;
            btnclose.MouseDown += Btnclose_Click;
            btns3.btns.Add(btnclose);
            this.board.AddControl(btns3);
            #endregion

            #endregion

            #region 第一列
            btns5 = new MButtons();
            Logger.LogInfo("初始化第一列行情！");
            MPolygonButton pbtn1 = new MPolygonButton();
            pbtn1.Text = "行情";
            pbtn1.Font = MCommonData.d2Font;
            pbtn1.ForeColor = COLOR.RGB(MCommonData.fontColor1);
            pbtn1.MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
            pbtn1.PolygonForeColor = COLOR.RGB(MCommonData.fontColor5);
            pbtn1.hasChoose = true;
            pbtn1.MouseDown += Pbtn1_Click;
            btns5.btns.Add(pbtn1);

            Logger.LogInfo("初始化第一列供需发布！");
            MPolygonButton pbtn2 = new MPolygonButton();
            pbtn2.Text = "供需发布";
            pbtn2.Font = MCommonData.d2Font;
            pbtn2.ForeColor = COLOR.RGB(MCommonData.fontColor1);
            pbtn2.MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
            pbtn2.PolygonForeColor = COLOR.RGB(MCommonData.fontColor5);
            pbtn2.MouseDown += Pbtn2_Click;
            btns5.btns.Add(pbtn2);

            Logger.LogInfo("初始化第一列K线图！");
            MPolygonButton pbtn3 = new MPolygonButton();
            pbtn3.Text = "K线图";
            pbtn3.Font = MCommonData.d2Font;
            pbtn3.ForeColor = COLOR.RGB(MCommonData.fontColor1);
            pbtn3.MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
            pbtn3.PolygonForeColor = COLOR.RGB(MCommonData.fontColor5);
            pbtn3.MouseDown += Pbtn3_Click;
            btns5.btns.Add(pbtn3);

            Logger.LogInfo("初始化第一列资讯！");
            MPolygonButton pbtn4 = new MPolygonButton();
            pbtn4.Text = "资讯";
            pbtn4.Font = MCommonData.d2Font;
            pbtn4.ForeColor = COLOR.RGB(MCommonData.fontColor1);
            pbtn4.MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
            pbtn4.PolygonForeColor = COLOR.RGB(MCommonData.fontColor5);
            pbtn4.MouseDown += Pbtn4_Click;
            btns5.btns.Add(pbtn4);
            this.board.AddControl(btns5);
            #endregion

            #region 第二行    

            #region 快捷按钮
            Logger.LogInfo("初始化第二行快捷按钮！");
            btns4 = new MButtons();
            btns4.Font = MCommonData.d2Font;
            this.board.AddControl(btns4);
            #endregion

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
            SelectTextChange(sender, text);
        }

        public void UpdataLogin(bool islogin)
        {
            if (islogin)
            {
                btns2.Visible = false;
                lblogin.Visible = true;
                btnloginoff.Visible = true;
                if (DataManager.Instance.LoginInfo != null)
                {
                    lblogin.Text = "您好," + DataManager.Instance.LoginInfo.username;
                }
                else
                {
                    lblogin.Text = "您好," + DataManager.Instance.Account;
                }
            }
            else
            {
                btns2.Visible = true;
                lblogin.Visible = false;
                btnloginoff.Visible = false;
                lblogin.Text = "您好, ";
            }
        }

        public void UpdataMarketData(ContractLastPrice md)
        {
            if (md != null)
            {
                if (lbs1 != null && lbs1.lbs.Count >= 4)
                {
                    lbs1.lbs[1].Text = md.bidPrice.ToString();
                    lbs1.lbs[3].Text = md.askPrice.ToString();
                }
            }
            else
            {
                lbs1.lbs[1].Text = "0";
                lbs1.lbs[3].Text = "0";
                table.Updata("",0, 0);
            }
        }

        public void UpdataMarketData(string category,float buyPrice, float sellPrice)
        {
            lbs1.lbs[1].Text = buyPrice.ToString();
            lbs1.lbs[3].Text = sellPrice.ToString();
            table.Updata(category, buyPrice, sellPrice);
        }
        public void UpdataSelect(ContractCategoryDic cv)
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
        }

        public void AddFastBtns(ContractCategoryDic cv,bool ischoose)
        {
            MButton btn = new MButton();
            btn.Text = cv.categoryName;
            btn.hasChoose = ischoose;
            btn.hasFrame = false;
            btn.ForeColor = COLOR.RGB(MCommonData.fontColor4);
            btn.MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
            btn.BackColor = COLOR.RGB(MCommonData.fontColor0);
            btn.MouseBackColor = COLOR.RGB(MCommonData.fontColor0);
            btn.MouseClickBackColor = COLOR.RGB(MCommonData.fontColor3);
            btn.MouseDown += Btn_Click;
            btn.Tag = cv;
            btns4.btns.Add(btn);

            MLine vline = new MLine();
            vline.Width = 1;
            vline.LineColor = COLOR.RGB(MCommonData.LineColor);
            btns4.vlines.Add(vline);
        }
        public void ClearFastBtns()
        {
            if (btns4 != null)
            {
                if (btns4.btns != null)
                {
                    btns4.btns.Clear();
                }
                if (btns4.btns != null)
                {
                    btns4.vlines.Clear();
                }
            }
        }

        #region 事件
        /// <summary>
        /// 注册按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnregistered_MouseDown(object sender, MMouseEventArgs e)
        {
            BtnregisteredMouseDown();
        }

        /// <summary>
        /// 注销按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWriteOff_MouseDown(object sender, MMouseEventArgs e)
        {
            BtnWriteOffMouseDown();
        }
        /// <summary>
        /// 登录按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnlogin_MouseDown(object sender, MMouseEventArgs e)
        {
            BtnloginMouseDown();
        }
        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnclose_Click(object sender, MMouseEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 最大化按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnmax_Click(object sender, MMouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        /// <summary>
        /// 最小化按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnmin_Click(object sender, MMouseEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

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
        /// 第一列资讯按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pbtn4_Click(object sender, MMouseEventArgs e)
        {
            InformationBtnClick();
        }

        /// <summary>
        /// 第一列K线图按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pbtn3_Click(object sender, MMouseEventArgs e)
        {
            KlineBtnClick();
        }

        /// <summary>
        /// 第一列供需发布按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pbtn2_Click(object sender, MMouseEventArgs e)
        {
            SupplyAndDemandClick();
        }

        /// <summary>
        /// 第一列行情按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pbtn1_Click(object sender, MMouseEventArgs e)
        {
            Draw();
            QuotesClick();
        }

        /// <summary>
        /// 基础管理按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnjc_Click(object sender, MMouseEventArgs e)
        {
            BasicManagementClick();
        }

        /// <summary>
        /// 我的资金按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnls_Click(object sender, MMouseEventArgs e)
        {
            MyMoneyClick();
        }

        /// <summary>
        /// 行情按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnhq_Click(object sender, MMouseEventArgs e)
        {
            //Random rd = new Random();
            //for (int i = 0; i < 100; i++)
            //{
            //    int num = rd.Next(0, 2);
            //    if (num == 1)
            //    {
            //        table.InsertSellData();
            //        this.board.Draw(table);
            //    }
            //    else
            //    {
            //        table.InsertBuyData();
            //        this.board.Draw(table);
            //    }
            //}
            //Draw();
            QuotesClick();
        }

        /// <summary>
        /// 快捷按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Click(object sender, MMouseEventArgs e)
        {
            FastBtnClick(sender, e);
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

            #region 第一行
            //画线位置
            int firstRow1X = 20;
            int firstRow2X = this.Width - 100;
            int firstRow1Y = 0;
            int firstRow2Y = firstRow1Y + 40;

            //第一行背景
            mrbk.Rectangle = new RECT(0, 0, this.Width, firstRow2Y);

            //hline.FirstPoint = new POINT(0, firstRow2Y);
            //hline.SecondPoint = new POINT(this.Width, firstRow2Y);
            //按钮位置
            //行情按钮
            btns1.btns[0].Rectangle = new RECT(firstRow1X + 20, firstRow1Y, firstRow1X + 20 + 65, firstRow2Y);
            //我的资金按钮
            btns1.btns[1].Rectangle = new RECT(firstRow1X + 130, firstRow1Y, firstRow1X + 130 + 100, firstRow2Y);
            //基础管理按钮
            btns1.btns[2].Rectangle = new RECT(firstRow1X + 275, firstRow1Y, firstRow1X + 275 + 100, firstRow2Y);
            btns1.ChangeRectangle();

            if (firstRow2X > 0)
            {
                //登录按钮
                btns2.btns[0].Rectangle = new RECT(firstRow2X - 100, firstRow1Y + 5, firstRow2X - 100 + 60, firstRow1Y + 5 + 25);
                //注册按钮
                btns2.btns[1].Rectangle = new RECT(firstRow2X - 180, firstRow1Y + 5, firstRow2X - 180 + 60, firstRow1Y + 5 + 25);
                btns2.ChangeRectangle();

                //登录后状态
                int jg = lblogin.Text.Length * 13;
                int isloginbegin = firstRow2X - jg;
                btnloginoff.Rectangle = new RECT(isloginbegin-50, firstRow1Y + 5, isloginbegin, firstRow1Y + 5 + 25);
                lblogin.Rectangle= new RECT(isloginbegin, firstRow1Y + 5, isloginbegin + jg, firstRow1Y + 5 + 25);

                //最小化按钮
                btns3.btns[0].Rectangle = new RECT(firstRow2X + 0, firstRow1Y + 10, firstRow2X + +0 + 12, firstRow1Y + 10 + 12);
                //最大化按钮
                btns3.btns[1].Rectangle = new RECT(firstRow2X + 30, firstRow1Y + 10, firstRow2X + 30 + 12, firstRow1Y + 10 + 12);
                //关闭按钮
                btns3.btns[2].Rectangle = new RECT(firstRow2X + 60, firstRow1Y + 10, firstRow2X + 60 + 12, firstRow1Y + 10 + 12);
                btns3.ChangeRectangle();
            }

            #endregion

            #region 第一列
            int firstColumnX = 0;
            int firstColumnY = firstRow2Y + 55;
            //左边行情标签位置
            ((MPolygonButton)btns5.btns[0]).points[0] = new POINT(firstColumnX, firstColumnY);//左上
            ((MPolygonButton)btns5.btns[0]).points[1] = new POINT(firstColumnX + 40, firstColumnY + 5);//右上
            ((MPolygonButton)btns5.btns[0]).points[2] = new POINT(firstColumnX + 40, firstColumnY + 90);//右下
            ((MPolygonButton)btns5.btns[0]).points[3] = new POINT(firstColumnX, firstColumnY + 100);//左下
            ((MPolygonButton)btns5.btns[0]).Rectangle = new RECT(firstColumnX, firstColumnY, firstColumnX + 40, firstColumnY + 100);
            //左边供需发布标签位置
            firstColumnY += 120;
            ((MPolygonButton)btns5.btns[1]).points[0] = new POINT(firstColumnX, firstColumnY);//左上
            ((MPolygonButton)btns5.btns[1]).points[1] = new POINT(firstColumnX + 40, firstColumnY + 5);//右上
            ((MPolygonButton)btns5.btns[1]).points[2] = new POINT(firstColumnX + 40, firstColumnY + 90);//右下
            ((MPolygonButton)btns5.btns[1]).points[3] = new POINT(firstColumnX, firstColumnY + 100);//左下
            ((MPolygonButton)btns5.btns[1]).Rectangle = new RECT(firstColumnX, firstColumnY, firstColumnX + 40, firstColumnY + 100);
            //左边K线图标签位置
            firstColumnY += 120;
            ((MPolygonButton)btns5.btns[2]).points[0] = new POINT(firstColumnX, firstColumnY);//左上
            ((MPolygonButton)btns5.btns[2]).points[1] = new POINT(firstColumnX + 40, firstColumnY + 5);//右上
            ((MPolygonButton)btns5.btns[2]).points[2] = new POINT(firstColumnX + 40, firstColumnY + 90);//右下
            ((MPolygonButton)btns5.btns[2]).points[3] = new POINT(firstColumnX, firstColumnY + 100);//左下
            ((MPolygonButton)btns5.btns[2]).Rectangle = new RECT(firstColumnX, firstColumnY, firstColumnX + 40, firstColumnY + 100);

            firstColumnY += 120;
            ((MPolygonButton)btns5.btns[3]).points[0] = new POINT(firstColumnX, firstColumnY);//左上
            ((MPolygonButton)btns5.btns[3]).points[1] = new POINT(firstColumnX + 40, firstColumnY + 5);//右上
            ((MPolygonButton)btns5.btns[3]).points[2] = new POINT(firstColumnX + 40, firstColumnY + 90);//右下
            ((MPolygonButton)btns5.btns[3]).points[3] = new POINT(firstColumnX, firstColumnY + 100);//左下
            ((MPolygonButton)btns5.btns[3]).Rectangle = new RECT(firstColumnX, firstColumnY, firstColumnX + 40, firstColumnY + 100);
            btns5.ChangeRectangle();
            #endregion

            #region 第二行 
            int secondRow1X = btns5.btns[0].Rectangle.right + 10;
            int secondRow2X = this.Width - 340;
            int secondRow1Y = firstRow2Y + 10;
            int secondRow2Y = secondRow1Y + 35;
            //快捷钮位置
            for (int i = 0; i < btns4.btns.Count; i++)
            {
                btns4.btns[i].Rectangle = new RECT(secondRow1X + i * 60, secondRow1Y, secondRow1X + i * 60 + 35, secondRow2Y);
            }
            //竖线位置
            for (int i = 0; i < btns4.vlines.Count; i++)
            {
                btns4.vlines[i].FirstPoint = new POINT(secondRow1X + 50 + i * 60, secondRow1Y + 5);
                btns4.vlines[i].SecondPoint = new POINT(secondRow1X + 50 + i * 60, secondRow1Y + 25);
            }
            btns4.ChangeRectangle();

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

            int tableX = btns5.btns[0].Rectangle.right + 10;//表格左边X坐标
            int tableY = secondRow2Y + 10;//表格左上角Y坐标
            int table2X = this.Width - 30;//表头背景右边X坐标
            int table2Y = this.Height;//表头背景右下角Y坐标
            table.Rectangle = new RECT(tableX, tableY, table2X, table2Y);
            #endregion

            this.board.Draw();
        }

        ///// <summary>
        ///// 上下滚动条
        ///// </summary>
        ///// <param name="offset">偏移量</param>
        //private void DrawTable(int offset1,int offset2)
        //{
        //    int tableX = btns5.btns[0].Rectangle.right+10;//表格左上角X坐标
        //    int tableY = btns4.btns[0].Rectangle.bottom + 10;//表格左上角Y坐标
        //    int table2X = this.Width - 30;//表头背景右下角X坐标
        //    int avg = 70;//表头分隔权值
        //    int interval = (table2X - tableX - avg * 11) / 10;//表头分割值
        //    int lbY = tableY + 10;//列头文字Y坐标

        //    int dataWith = 100;//数据文字宽度
        //    int of = 0;//数据左右偏移量

        //    #region 表头

        //    //表头背景位置
        //    if (table2X > tableX)
        //    {
        //        //表头背景绘图区域设置
        //        tableHeader.Rectangle = new RECT(tableX, tableY, table2X, tableY + 45);
        //    }

        //    //列头位置
        //    //标记
        //    int lb1x = tableX + 10;//列头开始位置Y坐标
        //    //标记坐标
        //    tableHeader.lbs[0].Rectangle = new RECT(lb1x + 10, lbY, lb1x + 40, lbY + 20);
        //    //标记贴图位置
        //    int image2x = tableHeader.lbs[0].Rectangle.right+1;
        //    int image2y = tableHeader.lbs[0].Rectangle.top+2;
        //    image2.Rectangle = new RECT(image2x - 2, image2y, image2x + image2.BackgroundImage.Width - 5, image2y + image2.BackgroundImage.Height - 4);
        //    //其余表头文字区域设置
        //    for (int i = 1; i < tableHeader.lbs.Count; i++)
        //    {
        //        int tableHeaderX = tableHeader.lbs[i - 1].Rectangle.right + interval;
        //        tableHeader.lbs[i].Rectangle = new RECT(tableHeaderX + 10, lbY, tableHeaderX + 70, lbY + 20);
        //    }
        //    #endregion

        //    #region 数据

        //    //表背景位置
        //    if (table2X > tableX)
        //    {
        //        int mrtX2= table2X;
        //        if (scrollbar1.Visible|| scrollbar2.Visible)
        //        {
        //            mrtX2 = table2X - 10;
        //        }
        //        //表的背景绘图区域设置 
        //        mrt.Rectangle = new RECT(tableX, tableY + 45, mrtX2, this.Height-60);
        //    }
   
        //    int dataX1 = mrt.Rectangle.left;//左边
        //    int dataX2 = mrt.Rectangle.right;//右边
        //    //卖
        //    int scrollbarY1 = mrt.Rectangle.top;
        //    //分割线
        //    int scrollbarfg = mrt.Rectangle.top + (int)(0.6 * (mrt.Rectangle.bottom - mrt.Rectangle.top));
        //    //买
        //    int scrollbarY2 = mrt.Rectangle.bottom;
        //    bool needScrollbar1 = false;
        //    bool needScrollbar2 = false;
        //    #region 卖
        //    int data1y = tableY + 22 + dataInterval;//数据行中文字的Y坐标
        //    int dataChoose1Y = tableY + 45;//是否选中背景颜色的Y坐标
        //    //卖
        //    foreach (var v in sellData)
        //    {
        //        int TopY = dataChoose1Y + offset1;//数据（卖）数据绘图区域上部Y坐标
        //        int bottomY = dataChoose1Y + dataInterval + offset1; //数据（卖）数据绘图区域下部Y坐标
        //        if (bottomY < scrollbarY1 || bottomY > scrollbarfg)
        //        {
        //            //无需绘制，隐藏
        //            v.Hide();
        //            needScrollbar1 = true;
        //        }
        //        else
        //        {
        //            //需要绘制显示
        //            v.Show();
        //        }
        //        //数据（卖）背景绘制位置
        //        if (table2X > tableX)
        //        {
        //            v.data.Rectangle = new RECT(dataX1, TopY, dataX2, bottomY);
        //        }
        //        //数据（卖）图片绘制位置
        //        if (v.data1image!=null)
        //        {
        //            int data1imagex = tableHeader.lbs[0].Rectangle.left + of;
        //            if (v.type != 0)
        //            {
        //                v.data1image.Rectangle = new RECT(data1imagex + 5, data1y + 5 + offset1, data1imagex + 5 + 15, data1y + 5 + 15 + offset1);
        //            }
        //            else
        //            {
        //                v.data1image.Rectangle = new RECT(data1imagex - 10+10, data1y + 5 + offset1, data1imagex + 10 + 15, data1y + 5 + 15 + offset1);
        //            }

        //        }
        //        //数据（卖）文字绘制位置
        //        for (int i = 0; i < v.data.lbs.Count; i++)
        //        {
        //            int data1x = tableHeader.lbs[i + 1].Rectangle.left+ of;
        //            v.data.lbs[i].Rectangle = new RECT(data1x, data1y + offset1, data1x + dataWith, data1y + 20 + offset1);
        //        }
        //        //下一条数据
        //        data1y += dataInterval;
        //        dataChoose1Y += dataInterval;
        //    }
        //    #endregion

        //    #region 分割线
        //    //买卖分割横线位置
        //    int lineY = dataChoose1Y + offset1;//买卖分割横线Y坐标
        //    //不能超过分割线
        //    if (lineY > scrollbarfg)
        //    {
        //        lineY = scrollbarfg;
        //    }
        //    line1.FirstPoint = new POINT(dataX1, lineY);
        //    line1.SecondPoint = new POINT(dataX2, lineY);
        //    #endregion

        //    #region 买
        //    int data2y = lineY;//数据行中文字的Y坐标
        //    int dataChoose2Y = lineY;//是否选中背景颜色的Y坐标
        //    //买
        //    foreach (var v in buyData)
        //    {
        //        int TopY = dataChoose2Y + offset2;//数据（买）数据绘图区域上部Y坐标
        //        int bottomY = dataChoose2Y + dataInterval + offset2; //数据（买）数据绘图区域下部Y坐标
        //        if (TopY < lineY || TopY > scrollbarY2)
        //        {
        //            //无需绘制，隐藏
        //            v.Hide();
        //            needScrollbar2 = true;
        //        }
        //        else
        //        {
        //            //需要绘制显示
        //            v.Show();
        //        }
        //        //数据（买）背景绘制位置
        //        if (table2X > tableX)
        //        {
        //            v.data.Rectangle = new RECT(dataX1, TopY, dataX2, bottomY);
        //        }
        //        //数据（买）图片绘制位置
        //        if (v.data1image != null)
        //        {
        //            int data1imagex = tableHeader.lbs[0].Rectangle.left + of;
        //            if (v.type != 0)
        //            {
        //                v.data1image.Rectangle = new RECT(data1imagex + 5, data2y + 5 + offset2, data1imagex + 5 + 15, data2y + 5 + 15 + offset2);
        //            }
        //            else
        //            {
        //                v.data1image.Rectangle = new RECT(data1imagex - 10 + 10, data2y + 5 + offset2, data1imagex + 10 + 15, data2y + 5 + 15 + offset2);
        //            }
        //        }
        //        //数据（买）文字绘制位置
        //        for (int i = 0; i < v.data.lbs.Count; i++)
        //        {
        //            int data2x = tableHeader.lbs[i + 1].Rectangle.left + of;
        //            v.data.lbs[i].Rectangle = new RECT(data2x, data2y + offset2, data2x + dataWith, data2y + 20 + offset2);
        //        }
        //        data2y += dataInterval;
        //        dataChoose2Y += dataInterval;
        //    }
        //    #endregion

        //    #endregion

        //    #region 滚动条
        //    if (needScrollbar1)
        //    {
        //        scrollbar1.Visible = true;
        //                    scrollbar1.Rectangle = new RECT(dataX2, scrollbarY1, dataX2 + 10, lineY);
        //    scrollbar1.ChangeRectangle();
        //    }
        //    else
        //    {
        //        scrollbar1.Visible = false;
        //    }

        //    if (needScrollbar2)
        //    {
        //        scrollbar2.Visible = true;
        //        scrollbar2.Rectangle = new RECT(dataX2, lineY, dataX2 + 10, scrollbarY2);
        //        scrollbar2.ChangeRectangle();
        //    }
        //    else
        //    {
        //        scrollbar2.Visible = false;
        //    }
        //    #endregion
        //}

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

        //Random rdd= new Random();
        //public void InsertSellData()
        //{
        //    MLabels data = new MLabels();
        //    data.BackColor = COLOR.RGB(MCommonData.fontColor6);
        //    data.MouseBackColor = COLOR.RGB(MCommonData.fontColor10);
        //    for (int i = 0; i < 10; i++)
        //    {
        //        MLabel ml = new MLabel();
        //        ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
        //        ml.BackColor = -1;
        //        ml.Font = MCommonData.d4Font;
        //        ml.LeftAligned = true;
        //        data.lbs.Add(ml);
        //    }
        //    data.lbs[0].Text = "卖(8)";
        //    data.lbs[0].ForeColor = COLOR.RGB(MCommonData.fontColor7);
        //    data.lbs[1].Text = "销售";
        //    data.lbs[2].Text = "平水铜一类";
        //    data.lbs[3].Text = "-10";
        //    data.lbs[4].Text = "54020";
        //    data.lbs[5].Text = "1";
        //    //data.lbs[6].Text = "上海国储天威仓储有限公司";
        //    data.lbs[6].Text = "上海国储天威...";
        //    data.lbs[7].Text = "左岗集团";
        //    data.lbs[8].Text = "15:41";
        //    data.lbs[9].Text = "15:41:15";
        //    data.lbs[9].ForeColor = COLOR.RGB(MCommonData.fontColor8);

        //    UData ud = new UData();
        //    ud.data1image = new MImage();
        //    int d = rdd.Next(0, 5);
        //    if (d == 0)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_new;
        //    }
        //    else if (d == 1)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_publish;
        //    }
        //    else if (d == 2)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_white;
        //    }
        //    else if (d == 3)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_black;
        //    }
        //    else
        //    {
        //        ud.data1image = null;
        //    }
        //    ud.type = d;
        //    ud.data = data;
        //    sellData.Add(ud);
        //    this.board.AddControl(data);
        //    if (ud.data1image != null)
        //    {
        //        this.board.AddControl(ud.data1image);
        //    }
        //    scrollbar1.CalculationSliderWith(((sellData.Count) * dataInterval));
        //}

        //public void InsertBuyData()
        //{
        //    MLabels data = new MLabels();
        //    data.BackColor = COLOR.RGB(MCommonData.fontColor6);
        //    data.MouseBackColor = COLOR.RGB(MCommonData.fontColor10);
        //    for (int i = 0; i < 10; i++)
        //    {
        //        MLabel ml = new MLabel();
        //        ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
        //        ml.BackColor = -1;
        //        ml.Font = MCommonData.d4Font;
        //        ml.LeftAligned = true;
        //        data.lbs.Add(ml);
        //    }
        //    data.lbs[0].Text = "买(9)";
        //    data.lbs[0].ForeColor = COLOR.RGB(MCommonData.fontColor9);
        //    data.lbs[1].Text = "采购";
        //    data.lbs[2].Text = "升水铜";
        //    data.lbs[3].Text = "100";
        //    data.lbs[4].Text = "54040";
        //    data.lbs[5].Text = "2";
        //    data.lbs[6].Text = "上海主流仓库";
        //    data.lbs[7].Text = "左岗集团";
        //    data.lbs[8].Text = "";
        //    data.lbs[9].Text = "16:22:22";
        //    data.lbs[9].ForeColor = COLOR.RGB(MCommonData.fontColor8);

        //    UData ud = new UData();
        //    ud.data = data;
        //    ud.data1image = new MImage();
        //    int d = rdd.Next(0, 5);
        //    if (d == 0)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_new;
        //    }
        //    else if (d == 1)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_publish;
        //    }
        //    else if (d == 2)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_white;
        //    }
        //    else if (d == 3)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_black;
        //    }
        //    else
        //    {
        //        ud.data1image = null;
        //    }
        //    ud.type = d;
        //    buyData.Add(ud);
        //    this.board.AddControl(data);
        //    if (ud.data1image != null)
        //    {
        //        this.board.AddControl(ud.data1image);
        //    }
        //    scrollbar2.CalculationSliderWith(((buyData.Count) * dataInterval));
        //}

        ///// <summary>
        ///// 置顶表头分割线
        ///// </summary>
        //public void StickyDataLineAndTableHander()
        //{
        //    //tableHeader.BringToFront();
        //    //image2.BringToFront();
        //    //line1.BringToFront();
        //    //return;
        //    this.board.RemoveControl(tableHeader);
        //    this.board.AddControl(tableHeader);
        //    this.board.RemoveControl(image2);
        //    this.board.AddControl(image2);

        //    this.board.RemoveControl(line1);
        //    this.board.AddControl(line1);
        //}

        #region 可重写方法

        /// <summary>
        /// 基础管理按钮点击事件
        /// </summary>
        protected virtual void BasicManagementClick() { }

        /// <summary>
        /// 登录按钮事件
        /// </summary>
        protected virtual void BtnloginMouseDown() { }

        /// <summary>
        /// 注销按钮事件
        /// </summary>
        protected virtual void BtnregisteredMouseDown(){}

        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        protected virtual void FormMouseWheel(MouseEventArgs e) { }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Initialized() { }

        /// <summary>
        /// 资讯按钮点击事件
        /// </summary>
        protected virtual void InformationBtnClick() { }

        /// <summary>
        /// K线按钮点击事件
        /// </summary>
        protected virtual void KlineBtnClick() { }

        /// <summary>
        /// 我的资金按钮点击事件
        /// </summary>
        protected virtual void MyMoneyClick() { }

        /// <summary>
        /// 行情按钮点击事件
        /// </summary>
        protected virtual void QuotesClick() { }

        /// <summary>
        /// 供需发布按钮点击事件
        /// </summary>
        protected virtual void SupplyAndDemandClick() { }

        /// <summary>
        /// 快捷点击菜单按钮事件
        /// </summary>
        protected virtual void FastBtnClick(object sender, MMouseEventArgs e) { }

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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MUseMainForm
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "MUseMainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }
    }
}
