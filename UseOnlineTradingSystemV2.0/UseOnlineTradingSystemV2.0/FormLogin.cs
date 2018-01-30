using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseOnlineTradingSystem
{
    public partial class FormLogin : Form
    {
        private bool isMouseDown = false;
        private Pen pen1 = new Pen(Color.Gray,2);
        private int x0 = 0;
        private int x1 = 0;//区域1
        private int x2 = 0;//区域2
        private int x3 = 0;//区域3
        private int beginY=0;
        private int endY = 0;
        public FormLogin()
        {
            InitializeComponent();
            Color c1 = Color.FromArgb(17, 165, 239);
            Color c2 = Color.FromArgb(170, 81, 245);
            Color c3 = Color.FromArgb(255, 144, 4);
            Color c4 = Color.FromArgb(101, 112, 248);
            Color c1_1 = Color.FromArgb(35, 166, 236);
            x0 = 80;
            x1 = x2 = this.Width / 3;
            //x2 = this.Width * 2 / 4;
            x3 = this.Width * 2 / 3;
            beginY = 10;
            endY = this.Height - 10;
            int hy1 = pb_1.Height + 30 + 10 ;
            int hy2 = pb_1.Height + 30 + 40;
            //第一部分头部
            pb_1.Location = new Point(x0, 30);
            lblh1_1.Location = new Point(x0, hy1);
            lblh1_2.Location = new Point(x0, hy2);
            lblh1_1.ForeColor = c1;
            lblh1_2.ForeColor = c1;
            //第二部分头部
            pb_2.Location = new Point(x1 + x0, 30);
            lblh2_1.Location = new Point(x1 + x0, hy1);
            lblh2_2.Location = new Point(x1 + x0, hy2);
            lblh2_1.ForeColor = c2;
            lblh2_2.ForeColor = c2;
            //第三部分头部
            pb_3.Location = new Point(x2 + x0, 30);
            lblh3_1.Location = new Point(x2 + x0, hy1);
            lblh3_2.Location = new Point(x2 + x0, hy2);
            lblh3_1.ForeColor = c3;
            lblh3_2.ForeColor = c3;
            //第四部分头部
            pb_4.Location = new Point(x3 + x0, 30);
            lblh4_1.Location = new Point(x3 + x0, hy1);
            lblh4_2.Location = new Point(x3 + x0, hy2);
            lblh4_1.ForeColor = c4;
            lblh4_2.ForeColor = c4;

            #region 第一部分
            int y = this.Height / 2;
            //服务器设置
            lblcb1.Location = new Point(x0,y);
            comb1.Location = new Point(x0, y + 20);
            comb1.SelectedIndex = 0;

            y += 50;
            //账号登录
            lbltxt1.Location = new Point(x0, y);
            txt1.Location = new Point(x0, y + 20);

            y += 50;
            //密码
            lbltxt2.Location = new Point(x0, y);
            txt2.Location = new Point(x0, y + 20);

            y += 50;
            //自动登录 找回密码
            cb1.Location = new Point(x0, y );
            lbl1_1.Location = new Point(x0+cb1.Width+10, y );

            y += 30;
            //登录
            btn1.Location = new Point(x0, y);
            btn1.BackColor = c1_1;
            #endregion

            #region 第二部分
            y = this.Height / 2;
            //代理服务器
            cb2.Location = new Point(x1 + x0, y);

            y += 30;
            //代理服务器地址
            lblAddress.Location = new Point(x1+ x0, y+5);
            txtAddress.Location = new Point(x1 + x0+ lblAddress.Width+ 15, y);

            y += 30;
            //端口
            lblPort.Location = new Point(x1 + x0, y + 5);
            txtPort.Location = new Point(txtAddress.Location.X, y);

            y += 40;
            //代理服务器身份验证
            lbl2_1.Location = new Point(x1 + x0, y);

            y += 30;
            //代理用户名
            lblUser.Location = new Point(x1 + x0, y + 5);
            txtUser.Location = new Point(txtAddress.Location.X, y);

            y += 30;
            //代理密码
            lblPassword.Location = new Point(x1 + x0, y + 5);
            txtPassword.Location = new Point(txtAddress.Location.X, y);

            y += 30;
            //代理域
            lblY.Location = new Point(x1 + x0, y + 5);
            txtY.Location = new Point(txtAddress.Location.X, y);
            #endregion

            #region 第三部分
            y = this.Height*3 / 5;
            //客服热线
            lbl3_1.Location = new Point(x2 + x0, y);
            lbl3_2.Location = new Point(x2 + x0, y+20);

            y += 60;
            //海外客服热线
            lbl3_3.Location = new Point(x2 + x0, y);
            lbl3_4.Location = new Point(x2 + x0, y + 20);

            y += 60;
            //服务邮箱
            lbl3_5.Location = new Point(x2 + x0, y);
            lbl3_6.Location = new Point(x2 + x0, y + 20);
            #endregion

            #region 第四部分
            y = this.Height * 3 / 5;
            //客服热线
            lbl4_1.Location = new Point(x3 + x0, y);
            #endregion

            try
            {
                this.txt1.Text = Helper.GetAppConfig("account");
                this.txt2.Text = Helper.GetAppConfig("password");
            }
            catch
            {

            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            TestSuppleForm form = new TestSuppleForm();
            form.ShowDialog();
            return;

            string msg;
            this.btn1.Enabled = false;
            if (DataManager.Instance.Login(this.txt1.Text, this.txt2.Text,out msg))
            {
                this.Hide();
                Program.main.Show();
                //Program.mf.Show();
            }
            else
            {
                MessageBox.Show("登录错误，"+msg);
            }
            this.btn1.Enabled = true;
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (Program.main != null && !Program.main.IsDisposed)
            //{
            //    Program.main.Close();
            //}

            //this.Hide();
            //e.Cancel = true;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.pbClose.Image = Properties.Resources.icon_close;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            this.pbClose.Image = Properties.Resources.icon_close_hover;
        }

        private void FormLogin2_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
        }

        private void FormLogin2_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void FormLogin2_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                WinAPI.ReleaseCapture();
                WinAPI.SendMessage(this.Handle, WinAPI.WM_SYSCOMMAND, WinAPI.SC_MOVE + WinAPI.HTCAPTION, 0);
            }
        }

        private void FormLogin2_Paint(object sender, PaintEventArgs e)
        {
            //虚线
            pen1.DashStyle = DashStyle.Dot;
            e.Graphics.DrawLine(pen1, x1, beginY, x1, endY);
            e.Graphics.DrawLine(pen1, x2, beginY, x2, endY);
            e.Graphics.DrawLine(pen1, x3, beginY, x3, endY);
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl1_1_Click(object sender, EventArgs e)
        {
            Process.Start("http://sso.test.1b2b.cn/sso/sign/resetpwd.html?next=http://utrade.useonline.cn/tradesystem/pay/home/index.html&label=useonline-gold");
        }

        private void FormLogin2_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
        }
    }
}
