using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace USeFuturesSpirit
{
    public partial class TestFormPicture : Form
    {
        #region 坐标测试  
        /// <summary>
        /// 获得矩形区域坐标
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rect lpRect);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowText(
            IntPtr hWnd,//窗口句柄 
            StringBuilder lpString,//标题 
            int nMaxCount //最大值 
            );

        #endregion

        public TestFormPicture()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.pictureScollControl1.LoadScollPicture();
        }

        private void Form_Move(object sender, EventArgs e)
        {
            IntPtr myPtr = GetForegroundWindow();
            StringBuilder title = new StringBuilder(256);
            GetWindowText(myPtr, title, title.Capacity);//得到窗口的标题 
            Rect rect = new Rect();
            GetWindowRect(myPtr, out rect);

            Debug.WriteLine("rec.left: {0}  rec.top: {0}  rec.right: {0}  rec.bottom: {0}",
                rect.Left, rect.Top, rect.Right, rect.Bottom);
            //Point newPoint2 = new Point(rect.Left, rect.Top + );
        }
    }
}
