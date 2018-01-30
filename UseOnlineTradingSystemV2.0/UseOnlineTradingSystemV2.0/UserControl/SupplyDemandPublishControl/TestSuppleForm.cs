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
    public partial class TestSuppleForm : Form
    {
        protected MBorad m_board;//画板
        private MLine lineUp;
        private MLine lineLeft;
        private MLine lineBottom;
        private MLine lineRight;


        public TestSuppleForm()
        {
            InitializeComponent();

            this.m_board = new MBorad();
            this.m_board.Dock = DockStyle.Fill;
            this.m_board.Location = new Point(0, 0);
            this.m_board.Name = "cBoard";
            this.m_board.BackColor = Color.Black;
            //this.board.Click += Board_Click;
            //this.board.DoubleClick += Board_DoubleClick;
            //this.board.MouseWheel += Board_MouseWheel;

            this.Controls.Add(m_board);

            //四周的线条
            lineUp = new MLine();
            lineUp.FirstPoint = new POINT(this.Location.X + 10, this.Location.Y + 10);
            lineUp.SecondPoint = new POINT(this.Location.X + this.Width - 20, this.Location.Y + 10);
            lineUp.LineColor = 255;
            lineUp.Width = 1;

            lineLeft = new MLine();
            lineLeft.FirstPoint = new POINT(this.Location.X + 10, this.Location.Y + 10);
            lineLeft.SecondPoint = new POINT(this.Location.X + 10, this.Location.Y + this.Height - 20);
            lineLeft.LineColor = 255;
            lineLeft.Width = 1;

            lineBottom = new MLine();
            lineBottom.FirstPoint = new POINT(this.Location.X + 10, this.Location.Y + this.Height - 20);
            lineBottom.SecondPoint = new POINT(this.Location.X + this.Width - 20, this.Location.Y + this.Height - 20);
            lineBottom.LineColor = 255;
            lineBottom.Width = 1;

            lineRight = new MLine();
            lineRight.FirstPoint = new POINT(this.Location.X + this.Width - 20, this.Location.Y + 10);
            lineRight.SecondPoint = new POINT(this.Location.X + this.Width - 20, this.Location.Y + this.Height - 20);
            lineRight.LineColor = 255;
            lineRight.Width = 1;

            m_board.AddControl(lineUp);
            m_board.AddControl(lineLeft);
            m_board.AddControl(lineBottom);
            m_board.AddControl(lineRight);


            this.m_board.Draw();//调用所有容器内的Onpaint重绘-MLine

        }

        /// <summary>
        /// 重绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaint(object sender, PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;

            this.Draw(rec);
        }

        private void Draw(Rectangle rec)
        {
            this.m_board.Size = this.Size;

            lineUp.SecondPoint = new POINT(this.Location.X + this.Width - 20, this.Location.Y + 10);

            this.m_board.Draw();

        }


    }
}
