using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mPaint
{
    public class MButtons : MControlBase
    {
        public List<MButton> btns= new List<MButton>();
        public List<MLine> vlines = new List<MLine>();//竖线
        public void ChangeRectangle()
        {
            if (btns.Count > 0)
            {
                RECT rect = new RECT(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
                foreach (var v in btns)
                {
                    if (rect.left > v.Rectangle.left)
                    {
                        rect.left = v.Rectangle.left;
                    }
                    if (rect.top > v.Rectangle.top)
                    {
                        rect.top = v.Rectangle.top;
                    }
                    if (rect.right < v.Rectangle.right)
                    {
                        rect.right = v.Rectangle.right;
                    }
                    if (rect.bottom < v.Rectangle.bottom)
                    {
                        rect.bottom = v.Rectangle.bottom;
                    }
                }
                Rectangle = rect;
            }
        }

        public void SetChoose(int x, int y)
        {
            foreach (var v in btns)
            {
                v.hasChoose = false;
            }

            foreach (var v in btns)
            {
                if (v.Rectangle.Contains(x, y))
                {
                    v.hasChoose = true;
                    Current = v;
                    break;
                }
            }
        }

        public void SetChoose(int num)
        {
            if (num >= 0 && num < btns.Count && btns.Count > 0)
            {
                foreach (var v in btns)
                {
                    v.hasChoose = false;
                }
                var btn= btns[num];
                btn.hasChoose = true;
                Current = btn;
            }
        }

        /// <summary>
        /// 当前点击的控件
        /// </summary>
        public MButton Current = null;

        /// <summary>
        /// 记录前一个被操作的控件
        /// </summary>
        private MControlBase previousControl = null;

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
            foreach (var v in btns)
            {
                v.DoPaint(e);
            }
            foreach (var v in vlines)
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
                SetChoose(e.MouseEventArgs.X, e.MouseEventArgs.Y);
                if (Current != null)
                {
                    Current.DoMouseDown(e);
                }
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
                foreach (var v in btns)
                {
                    v.isMouseDown = false;
                }
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
                foreach (var v in btns)
                {
                    if (v.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                    {
                        v.DoMouseEnter(e);
                        break;
                    }
                }
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
                foreach (var v in btns)
                {
                    v.DoMouseLeave(e);
                }
            }
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MMouseEventArgs e)
        {
            if (!this.Enable)
                return;
            if (e.MouseEventArgs.Button == MouseButtons.None)
            {
                if (previousControl != null)
                {
                    if (!previousControl.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                    {
                        previousControl.DoMouseLeave(e);
                        previousControl = null;
                    }
                }
                else
                {
                    foreach (var v in btns)
                    {
                        if (v.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                        {
                            v.DoMouseEnter(e);
                            previousControl = v;
                            break;
                        }
                    }
                }

            }
        }
    }
}
