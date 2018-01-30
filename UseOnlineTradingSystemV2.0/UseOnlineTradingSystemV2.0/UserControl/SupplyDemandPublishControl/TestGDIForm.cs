using mPaint;
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
    public partial class TestGDIForm : Form
    {
        private MButton btnPutBrand;   //挂牌
        private MButton btnDelistBrand;//摘牌
        private MButton btnTradedBrand;//成交

        protected MUseTradedTable table;//表格
        protected MBorad board;//画板

        public TestGDIForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void Initialize()
        {
            #region 窗口阴影
            WinAPI.SetClassLong(this.Handle, WinAPI.GCL_STYLE, WinAPI.GetClassLong(this.Handle, WinAPI.GCL_STYLE) | WinAPI.CS_DropSHADOW); //API函数加载，实现窗体边框阴影效果
            #endregion

            RECT defult = new RECT();
            this.board = new MBorad();
            this.board.Dock = DockStyle.Fill;
            this.board.Location = new Point(0, 0);
            this.board.Name = "cBoard";
            this.board.BackColor = Color.Black;
            //this.board.Click += Board_Click;
            //this.board.DoubleClick += Board_DoubleClick;
            //this.board.MouseWheel += Board_MouseWheel;

            this.Controls.Add(board);

            #region 按钮
            btnPutBrand = new MButton();
            btnPutBrand.Text = "挂牌";
            btnPutBrand.ForeColor = COLOR.RGB(Color.White);
            btnPutBrand.MouseForeColor = COLOR.RGB(MCommonData.fontColor8);
            btnPutBrand.BackColor = COLOR.RGB(MCommonData.fontColor5);
            btnPutBrand.MouseBackColor = COLOR.RGB(MCommonData.fontColor5);
            btnPutBrand.MouseClickBackColor = COLOR.RGB(MCommonData.fontColor5);
            btnPutBrand.Font = MCommonData.d4Font;
            btnPutBrand.hasFrame = false;
            btnPutBrand.Visible = false;
            btnPutBrand.MouseDown += BtnPutBrand_MouseDown; ;
            //btnPutBrand.Click += BtnPutBrand_Click;

            btnDelistBrand = new MButton();
            btnDelistBrand.Text = "摘牌";
            btnDelistBrand.ForeColor = COLOR.RGB(Color.White);
            btnDelistBrand.MouseForeColor = COLOR.RGB(MCommonData.fontColor8);
            btnDelistBrand.BackColor = COLOR.RGB(MCommonData.fontColor5);
            btnDelistBrand.MouseBackColor = COLOR.RGB(MCommonData.fontColor5);
            btnDelistBrand.MouseClickBackColor = COLOR.RGB(MCommonData.fontColor5);
            btnDelistBrand.Font = MCommonData.d4Font;
            btnDelistBrand.hasFrame = false;
            btnDelistBrand.Visible = false;
            btnDelistBrand.MouseDown += BtnDelistBrand_MouseDown;


            btnTradedBrand = new MButton();
            btnTradedBrand.Text = "成交";
            btnTradedBrand.ForeColor = COLOR.RGB(Color.White);
            btnTradedBrand.MouseForeColor = COLOR.RGB(MCommonData.fontColor8);
            btnTradedBrand.BackColor = COLOR.RGB(MCommonData.fontColor5);
            btnTradedBrand.MouseBackColor = COLOR.RGB(MCommonData.fontColor5);
            btnTradedBrand.MouseClickBackColor = COLOR.RGB(MCommonData.fontColor5);
            btnTradedBrand.Font = MCommonData.d4Font;
            btnTradedBrand.hasFrame = false;
            btnTradedBrand.Visible = false;
            btnTradedBrand.MouseDown += BtnTradedBrand_MouseDown;


            this.board.AddControl(btnPutBrand);
            this.board.AddControl(btnDelistBrand);
            this.board.AddControl(btnTradedBrand);

            #endregion

            #region 添加Table
            table = new MUseTradedTable(TableStyleEnum.PutBrandTable);
            this.board.AddControl(table);
            #endregion

            Draw();
        }

        /// <summary>
        /// 成交按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTradedBrand_MouseDown(object sender, MMouseEventArgs e)
        {
            //获取新表头-清空数据-拉取数据-添加-刷新
            this.table.CheckTableHeader(TableStyleEnum.TradedTable);
            this.table.ClearData();

            //拉取数据添加
            Transaction info = new Transaction()
            {
                transTime = "2017-11-21",
                commAvailableQuantity = "10",
                commBrandName = "湿法铜",
                premium = "-100"
            };
            for (int i = 0; i <= 2; i++)
            {
                table.InsertTradedData(info);
            }

            this.BeginInvoke((MethodInvoker)delegate { this.Draw(); });

        }

        /// <summary>
        /// 摘牌按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelistBrand_MouseDown(object sender, MMouseEventArgs e)
        {
            //获取新表头-清空数据-拉取数据-添加-刷新
            this.table.CheckTableHeader(TableStyleEnum.DelistTable);
            this.table.ClearData();

            //拉取数据添加
            Transaction info = new Transaction()
            {
                transTime = "2017-11-22",
                commAvailableQuantity = "10",
                commBrandName = "平水铜",
                premium = "100"
            };
            for (int i = 0; i <= 2; i++)
            {
                table.InsertDelistData(info);
            }

            this.BeginInvoke((MethodInvoker)delegate { this.Draw(); });
        }

        /// <summary>
        /// 挂牌按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPutBrand_MouseDown(object sender, MMouseEventArgs e)
        {
            //获取新表头-清空数据-拉取数据-添加-刷新
            this.table.CheckTableHeader(TableStyleEnum.PutBrandTable);
            this.table.ClearData();

            //拉取数据添加
            SelfListed info = new SelfListed()
            {
                transTime = "2017-11-23",
                commAvailableQuantity = "10",
                commBrandName = "平水铜",
                premium = "300"
            };

            for(int i = 0;i<= 2; i++)
            {
                table.InsertPutBrandData(info);
            }
            this.BeginInvoke((MethodInvoker)delegate { this.Draw(); });

        }

        /// <summary>
        /// 拉取所有的挂牌数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public List<SelfListed> GetPutBrandDataList()
        {
            List<SelfListed> selfData = new List<SelfListed>();
            return selfData;
        }

        /// <summary>
        /// 拉取所有的摘牌数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public List<SelfListed> GetDelistDataList()
        {
            List<SelfListed> selfData = new List<SelfListed>();
            return selfData;
        }

        /// <summary>
        /// 拉取所有的成交数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public List<SelfListed> GetTradedDataList()
        {
            List<SelfListed> selfData = new List<SelfListed>();
            return selfData;
        }

        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaint(object sender, PaintEventArgs e)
        {
            Draw();
        }

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

            #region 绘制按钮
            int tableXb = 0;//表格左边X坐标
            int tableYb = 0;//表格左上角Y坐标
            int table2Xb = 50;//表头背景右边X坐标
            int table2Yb = 20;//表头背景右下角Y坐标
            btnPutBrand.Rectangle = new RECT(tableXb, tableYb, table2Xb, table2Yb);
            btnPutBrand.Visible = Visible;

            int tableXd = 60;//表格左边X坐标
            int tableYd = 0;//表格左上角Y坐标
            int table2Xd = 110;//表头背景右边X坐标
            int table2Yd = 20;//表头背景右下角Y坐标
            btnDelistBrand.Rectangle = new RECT(tableXd, tableYd, table2Xd, table2Yd);
            btnDelistBrand.Visible = Visible;

            int tableXt = 120;//表格左边X坐标
            int tableYt = 0;//表格左上角Y坐标
            int table2Xt = 170;//表头背景右边X坐标
            int table2Yt = 20;//表头背景右下角Y坐标
            btnTradedBrand.Rectangle = new RECT(tableXt, tableYt, table2Xt, table2Yt);
            btnTradedBrand.Visible = Visible;
            #endregion

            #region 绘制表格
            int tableX = 0;//表格左边X坐标
            int tableY = 30;//表格左上角Y坐标
            int table2X = this.Width;//表头背景右边X坐标
            int table2Y = this.Height;//表头背景右下角Y坐标
            table.Rectangle = new RECT(tableX, tableY, table2X, table2Y);
            #endregion

            this.board.Draw();
        }

        private void SizeChanged(object sender, EventArgs e)
        {
            table.Initialization();
            Draw();
        }
    }
}
