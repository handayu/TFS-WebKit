using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 表右键菜单控件
    /// </summary>
    public partial class UseMenuTrip : ContextMenuStrip
    {
        private SelfListed m_commdityInfo;
        private Transaction m_delistInfo;
        private ContractCategoryDic m_vo;
        private Control m_ownerControl;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">操作类型-挂牌摘牌菜单区分</param>
        /// <param name="screenLocation">菜单位置</param>
        /// <param name="info">挂牌选中行信息</param>
        /// <param name="delistInfo">摘牌选中行信息</param>
        /// <param name="view">挂牌-摘牌表-用于挂牌摘牌刷新操作</param>
        public UseMenuTrip(OperationType type, Point screenLocation, ContractCategoryDic vo, SelfListed info, Transaction delistInfo, Control OwnerControl)
        {
            InitializeComponent();

            m_commdityInfo = info;
            m_delistInfo = delistInfo;
            m_vo = vo;
            m_ownerControl = OwnerControl;

            Debug.Assert(m_ownerControl != null);
            Debug.Assert(m_vo != null);

            //挂牌
            if (type == OperationType.PutBrand)
            {
                InitializePutBrandItems();
            }
            else if (type == OperationType.DelistBrand)
            {
                InitializeDelistItems();
            }

            //设置自定义菜单的风格重设
            this.Renderer = new USeToolStripRendererEx();

            this.Show(screenLocation);
        }

        /// <summary>
        /// 初始化挂牌下拉Items
        /// </summary>
        private void InitializePutBrandItems()
        {
            Items.Add("撤单");
            Items.Add("刷新");

            Items[0].Click += USeMenuTripPutBrandActionOrderClick;
            Items[1].Click += USeMenuTripPutBrandRefrashClick;
        }

        /// <summary>
        /// 初始化摘牌下拉Items
        /// </summary>
        private void InitializeDelistItems()
        {
            Items.Add("输入交收盘面价");
            Items.Add("刷新");

            Items[0].Click += USeMenuTripDelistBrandTradingInputClick;
            Items[1].Click += USeMenuTripDelistBrandRefrashClick;
        }

        /// <summary>
        /// 摘牌刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USeMenuTripDelistBrandRefrashClick(object sender, EventArgs e)
        {
            RefrashDelistBrandControlView();
        }

        /// <summary>
        /// 摘牌输入交收价格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USeMenuTripDelistBrandTradingInputClick(object sender, EventArgs e)
        {
            InputTradingPriceForm form = new InputTradingPriceForm(m_delistInfo);
            form.ShowDialog();

            //核销成功刷新摘牌列表/成交列表
            bool checkResult = form.Result;
            if (checkResult)
            {
                if (m_ownerControl == null) return;
                TradingInfoControl control = m_ownerControl as TradingInfoControl;
                control.InitializeDelistBrandList(m_vo);
                control.InitializeTradedBrandList(m_vo);
            }

            RefrashDelistBrandControlView();
        }

        /// <summary>
        /// 挂牌刷新事件响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USeMenuTripPutBrandRefrashClick(object sender, EventArgs e)
        {
            RefrashPutBrandControlView();
        }

        /// <summary>
        /// 挂牌列表重新刷新
        /// </summary>
        private void RefrashPutBrandControlView()
        {
            if (m_ownerControl == null) return;
            TradingInfoControl control = m_ownerControl as TradingInfoControl;
            control.InitializePutBrandList(m_vo);
        }

        /// <summary>
        /// 摘牌列表重新刷新
        /// </summary>
        private void RefrashDelistBrandControlView()
        {
            if (m_ownerControl == null) return;
            TradingInfoControl control = m_ownerControl as TradingInfoControl;
            control.InitializeDelistBrandList(m_vo);
        }


        /// <summary>
        /// 挂牌撤单点击事件响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USeMenuTripPutBrandActionOrderClick(object sender, EventArgs e)
        {
            Debug.Assert(m_commdityInfo != null);

            //测试撤单
            #region 测试数据
            if (DataManager.Instance.Cookies != null)
            {
                CancelOrderRequest requireActionBrandArgs = new CancelOrderRequest();

                if (DataManager.Instance.LoginData == null)
                {
                    MessageBox.Show("登陆状态有误，请重新登陆");
                    return;
                }

                requireActionBrandArgs.clientId = DataManager.Instance.LoginData.currentCompany.id + "_pc";
                requireActionBrandArgs.mqId = "test";
                requireActionBrandArgs.commId = Convert.ToInt32(m_commdityInfo.commId);//唯一的挂单标示
                requireActionBrandArgs.operationType = 4;
                requireActionBrandArgs.securityToken = DataManager.Instance.Cookies;

                CancelOrderResponse response = HttpService.PostActionBrandOrder(requireActionBrandArgs);
                //string actionBrandOrderByteArray = Helper.Serialize(requireActionBrandArgs);
                //ActionOrderResponseArguments response = (ActionOrderResponseArguments)service.HttpPostUrl(HTTPServiceUrlCollection.PostActionBrandOrderRequireInfoUrl, actionBrandOrderByteArray);

                if (response != null && response.Success && response.result != null)
                {
                    //重置
                    MessageBox.Show("撤单成功!");
                    RefrashPutBrandControlView();
                    if (Program.mf != null)
                    {
                        //撤单成功刷新主界面
                        if (DataManager.Instance.RemoveCommodityData(response.result.commId.ToString()))
                        {
                            Program.mf.UpdateTable();
                        }
                    }

                }
                else
                {
                    MessageBox.Show("撤单失败，请检查!");
                    return;
                }
            }
            #endregion
        }
    }

    /// <summary>
    /// CustmerMenuTripColor
    /// </summary>
    public class CaptureImageToolColorTable
    {
        private static readonly Color _borderColor = Color.FromArgb(65, 173, 236);
        private static readonly Color _backColorNormal = Color.FromArgb(229, 243, 251);
        private static readonly Color _backColorHover = Color.FromArgb(65, 173, 236);
        private static readonly Color _backColorPressed = Color.FromArgb(255, 135, 0);
        private static readonly Color _foreColor = Color.FromArgb(255, 135, 0);
        private static readonly Color _backColorChoose = Color.FromArgb(255, 255, 255);
        public CaptureImageToolColorTable() { }

        public Color BackColorChoose
        {
            get { return _backColorChoose; }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
        }

        public Color BackColorNormal
        {
            get { return _backColorNormal; }
        }

        public Color BackColorHover
        {
            get { return _backColorHover; }
        }

        public Color BackColorPressed
        {
            get { return _backColorPressed; }
        }

        public Color ForeColor
        {
            get { return _foreColor; }
        }
    }

    /// <summary>
    /// 自定义Renderer外观设计
    /// </summary>
    public class USeToolStripRendererEx : ToolStripProfessionalRenderer
    {
        private CaptureImageToolColorTable m_color = new CaptureImageToolColorTable();
        public USeToolStripRendererEx() : base()
        {
            //this.RoundedEdges = true;
        }
        public USeToolStripRendererEx(CaptureImageToolColorTable color) : base()
        {
            this.RoundedEdges = true;
            m_color = color;
        }

        /// <summary>
        /// 获取圆角矩形区域radius=直径
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            // 左上角
            //arcRect.Y = rect.Top;
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // 右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            // 左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// 渲染背景 包括menustrip背景 toolstripDropDown背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;//抗锯齿
            Rectangle bounds = e.AffectedBounds;
            LinearGradientBrush lgbrush = new LinearGradientBrush(new Point(0, 0), new Point(0, toolStrip.Height), Color.FromArgb(255, m_color.BackColorChoose), Color.FromArgb(255, m_color.BackColorChoose));
            if (toolStrip is MenuStrip)
            {
                //由menuStrip的Paint方法定义 这里不做操作
            }
            else if (toolStrip is ToolStripDropDown)
            {
                int diameter = 10;//直径
                GraphicsPath path = new GraphicsPath();
                Rectangle rect = new Rectangle(Point.Empty, toolStrip.Size);
                Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));

                path.AddLine(0, 0, 10, 0);
                // 右上角
                arcRect.X = rect.Right - diameter;
                path.AddArc(arcRect, 270, 90);

                // 右下角
                arcRect.Y = rect.Bottom - diameter;
                path.AddArc(arcRect, 0, 90);

                // 左下角
                arcRect.X = rect.Left;
                path.AddArc(arcRect, 90, 90);
                path.CloseFigure();
                toolStrip.Region = new Region(path);
                g.FillPath(lgbrush, path);
            }
            else
            {
                base.OnRenderToolStripBackground(e);
            }
        }

        /// <summary>
        /// 渲染边框 不绘制边框
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            //不调用基类的方法,屏蔽掉该方法去掉边框
        }

        /// <summary>
        /// 渲染箭头 更改箭头颜色
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = m_color.BackColorPressed;
            base.OnRenderArrow(e);
        }

        /// <summary>
        /// 渲染项 不调用基类同名方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            ToolStripItem item = e.Item;
            ToolStrip toolstrip = e.ToolStrip;

            //渲染顶级项
            if (toolstrip is MenuStrip)
            {
                LinearGradientBrush lgbrush = new LinearGradientBrush(new Point(0, 0), new Point(0, item.Height), Color.FromArgb(100, Color.White), Color.FromArgb(0, Color.White));
                SolidBrush brush = new SolidBrush(Color.FromArgb(255, Color.White));
                if (e.Item.Selected)
                {
                    GraphicsPath gp = GetRoundedRectPath(new Rectangle(new Point(0, 0), item.Size), 5);
                    g.FillPath(lgbrush, gp);
                }
                if (item.Pressed)
                {
                    //创建上面左右2圆角的矩形路径
                    GraphicsPath path = new GraphicsPath();
                    int diameter = 8;
                    Rectangle rect = new Rectangle(Point.Empty, item.Size);
                    Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
                    // 左上角
                    path.AddArc(arcRect, 180, 90);
                    // 右上角
                    arcRect.X = rect.Right - diameter;
                    path.AddArc(arcRect, 270, 90);
                    path.AddLine(new Point(rect.Width, rect.Height), new Point(0, rect.Height));
                    path.CloseFigure();
                    //填充路径
                    g.FillPath(brush, path);
                    g.FillRectangle(Brushes.White, new Rectangle(Point.Empty, item.Size));
                }
            }
            //渲染下拉项
            else if (toolstrip is ToolStripDropDown)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                LinearGradientBrush lgbrush = new LinearGradientBrush(new Point(0, 0), new Point(item.Width, 0), Color.FromArgb(255, m_color.BackColorPressed), Color.FromArgb(255, m_color.BackColorPressed));
                if (item.Selected)
                {
                    GraphicsPath gp = GetRoundedRectPath(new Rectangle(0, 0, item.Width, item.Height), 10);
                    g.FillPath(lgbrush, gp);
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        /// <summary>
        /// 渲染分割线
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            Graphics g = e.Graphics;

            LinearGradientBrush lgbrush = new LinearGradientBrush(new Point(0, 0), new Point(e.Item.Width, 0), m_color.BackColorNormal, Color.FromArgb(0, m_color.BackColorNormal));
            g.FillRectangle(lgbrush, new Rectangle(3, e.Item.Height / 2, e.Item.Width, 1));
            //base.OnRenderSeparator(e);
        }

        /// <summary>
        /// 渲染图片区域 下拉菜单左边的图片区域
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            //base.OnRenderImageMargin(e);
            //屏蔽掉左边图片竖条
        }

        /// <summary>
        /// 背景条背景颜色
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderItemBackground(e);
        }

    }
}
