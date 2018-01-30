using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mPaint
{
    public class MLabels : MControlBase
    {
        /// <summary>
        /// 记录前一个被操作的控件
        /// </summary>
        internal MControlBase previousControl = null;

        public List<MLabel> lbs = new List<MLabel>();
        /// <summary>
        /// 鼠标是否按下
        /// </summary>
        private bool isMouseDown = false;

        /// <summary>
        /// 鼠标是否进入
        /// </summary>
        protected bool isMouseEnter = false;
        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(MPaintEventArgs e)
        {
            if (!isMouseEnter)
            {
                e.CPaint.FillRect(BackColor, Rectangle);
            }
            else
            {
                e.CPaint.FillRect(MouseBackColor, Rectangle);
            }
            foreach (var v in lbs)
            {
                v.DoPaint(e);
            }
        }
        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MMouseEventArgs e)
        {
            if (!this.Enable)
                return;
            if (e.MouseEventArgs.Button == MouseButtons.Left)
            {
                this.isMouseDown = true;
            }
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MMouseEventArgs e)
        {
            if (!this.Enable)
                return;
            if (e.MouseEventArgs.Button == MouseButtons.Left)
            {
                this.isMouseDown = false;
            }
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(MMouseEventArgs e)
        {
            if (!this.Enable)
                return;
            if (e.MouseEventArgs.Button == MouseButtons.None)
            {
                this.isMouseEnter = true;
                e.Board.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(MMouseEventArgs e)
        {
            if (!this.Enable)
                return;
            if (e.MouseEventArgs.Button == MouseButtons.None)
            {
                this.isMouseEnter = false;
                e.Board.Cursor = Cursors.Default;
                foreach (var v in lbs)
                {
                    v.DoMouseLeave(e);
                }
            }
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnMouseMove(MMouseEventArgs e)
        {
            if (previousControl != null)
            {
                if (previousControl.Rectangle.Contains(e.MouseEventArgs.Location))
                {
                    previousControl.DoMouseMove(e);
                }
                else
                {
                    this.previousControl.DoMouseLeave(e);
                    this.previousControl = null;
                }
            }
            else
            {
                MControlBase control = null;
                for (int i = lbs.Count - 1; i >= 0; i--)
                {
                    MControlBase controlBase = lbs[i];
                    if (controlBase.Visible && controlBase.Rectangle.Contains(e.MouseEventArgs.Location))
                    {
                        control = controlBase;
                        break;
                    }
                }
                if (control != null)
                {
                    control.DoMouseEnter(e);
                    this.previousControl = control;
                }
            }
        }
    }
}
