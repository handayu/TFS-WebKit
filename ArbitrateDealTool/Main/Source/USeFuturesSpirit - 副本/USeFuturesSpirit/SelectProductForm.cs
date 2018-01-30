using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;
using USeFuturesSpirit;
using System.Runtime.InteropServices;



namespace USeFuturesSpirit
{
    /// <summary>
    /// 窗口矩形坐标值
    /// </summary>
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public partial class SelectProductForm : Form
    {
        #region 起始坐标Win32接口设置
        /// <summary>
        /// 获得矩形区域坐标
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rect lpRect);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(string lpClassName,string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowText(
            IntPtr hWnd,//窗口句柄 
            StringBuilder lpString,//标题 
            int nMaxCount //最大值 
            );
        #endregion

        private USeProduct m_product = null;
        private USeUserControl m_parentControl = null;

        public SelectProductForm()
        {
            InitializeComponent();
        }

        public SelectProductForm(USeUserControl parent, USeProduct product)
        {
            m_parentControl = parent;
            m_product = product;
            InitializeComponent();
        }

        public SelectProductForm(USeProduct product)
        {
            m_product = product;
            InitializeComponent();

        }

        public void SetFormlocation()
        {
            this.StartPosition = FormStartPosition.Manual;

            IntPtr myPtr = GetForegroundWindow();
            StringBuilder title = new StringBuilder(256);
            GetWindowText(myPtr, title, title.Capacity);
            Rect rect = new Rect();
            GetWindowRect(myPtr, out rect);

            Point newPoint = new Point(rect.Left, rect.Bottom - m_parentControl.Size.Height - this.Size.Height);
            this.Location = newPoint;
        }

        public USeProduct SelectedProduct
        {
            get { return m_product; }
        }

        private void SelectProductForm_Load(object sender, EventArgs e)
        {
            this.selectProductControl1.Initialize();
            this.selectProductControl1.DoubleClickEvent += ProductListButtonDoubleClickHandler;

            SetFormlocation();

            //初始化完毕SelectedProductControl之后，如果Product不为null开始寻找tab页面指定
            if (m_product == null) return;
            if (m_product.ShortName.Equals(string.Empty)) return;
            this.selectProductControl1.SetDefultBottomButtonText(m_product);
        }

        public void ProductListButtonDoubleClickHandler(object sender, EventArgs e)
        {
            m_product = this.selectProductControl1.SelectedProduct;
            this.DialogResult = DialogResult.Yes;

            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_product = this.selectProductControl1.SelectedProduct;
            this.DialogResult = DialogResult.Yes;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
