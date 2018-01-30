using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mPaint
{
    public class MVScrollBar : MControlBase
    {
        public double value = 0;
        public double minValue = 0;
        public double maxValue = 100;
        public double scrollInterval = 20;
        /// <summary>
        /// 鼠标是否按下
        /// </summary>
        public bool isMouseDown = false;

        /// <summary>
        /// 鼠标是否进入
        /// </summary>
        public bool isMouseEnter = false;

        private bool isSliderMouseDown = false;
        private int sliderMouseDownY = 0;//点击时滑块Y轴Top值

        private RECT sliderRectangle = new RECT();

        private double sliderMoveValue = 0;//滑块移动量

        public event Action<double> ValueChanged;

        public event Action MinInitialized;

        public event Action MaxInitialized;

        private int sliderWith;
        public void CalculationSliderWith(int w)
        {
            if (MinInitialized != null)
            {
                MinInitialized();
            }
            value = 0;
            sliderMoveValue = 0;
            double mw = Rectangle.bottom - Rectangle.top;
            if (w > mw)
            {
                double t = (1 - (w - mw) / mw) * mw;
                sliderWith = (int)t;
            }
            if (sliderWith < mw / 10)
            {
                sliderWith = (int)(mw / 10);
            }
        }

        public void ChangeRectangle()
        {
            if (sliderWith > 0)
            {
                double top = Rectangle.top + sliderMoveValue;
                double bottom = Rectangle.top + sliderWith + sliderMoveValue;
                double valueTemp = 0;
                if (top < Rectangle.top)
                {
                    top = Rectangle.top;
                    bottom = Rectangle.top + sliderWith;
                    sliderMoveValue = 0;
                    valueTemp = minValue;
                }
                else if (bottom > Rectangle.bottom)
                {
                    top = Rectangle.bottom - sliderWith;
                    bottom = Rectangle.bottom;
                    sliderMoveValue = Rectangle.bottom - sliderWith - Rectangle.top;
                    valueTemp = maxValue;
                }
                else
                {
                    valueTemp = ((top - Rectangle.top) * maxValue) / (Rectangle.bottom - Rectangle.top - sliderWith);
                }
                sliderRectangle = new RECT(Rectangle.left + 1, (int)top, Rectangle.right - 1, (int)bottom);
                if (ValueChanged != null)
                {
                    ValueChanged(valueTemp - value);
                }
                value = valueTemp;
                if (value < 1)
                {
                    if (MinInitialized != null)
                    {
                        MinInitialized();
                    }
                }
                else if (value>maxValue-1)
                {
                    if (MaxInitialized != null)
                    {
                        MaxInitialized();
                    }
                }
            }
            else
            {
                sliderMoveValue = 0;
            }
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(MPaintEventArgs e)
        {
            if ((Rectangle.right - Rectangle.left) >= 2 && (Rectangle.bottom - Rectangle.top) >= 2)
            {
                if (isMouseDown)
                {
                    e.CPaint.FillRect(MouseClickBackColor, Rectangle);
                    e.CPaint.FillRect(ForeColor, sliderRectangle);
                }
                else if (isMouseEnter)
                {
                    e.CPaint.FillRect(MouseBackColor, Rectangle);
                    e.CPaint.FillRect(MouseForeColor, sliderRectangle);
                }
                else
                {
                    e.CPaint.FillRect(BackColor, Rectangle);
                    e.CPaint.FillRect(ForeColor, sliderRectangle);
                }
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
                if (sliderRectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                {
                    isSliderMouseDown = true;
                    sliderMouseDownY = e.MouseEventArgs.Y;
                }
                else
                {
                    int aa = sliderRectangle.top + (sliderRectangle.bottom - sliderRectangle.top) / 2;
                    sliderMoveValue += e.MouseEventArgs.Y - aa;
                    ChangeRectangle(); 
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
                isSliderMouseDown = false;
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
            if (e.MouseEventArgs.Button == MouseButtons.Left)
            {
                //按着滑块移动
                if (isSliderMouseDown)
                {
                    sliderMoveValue += e.MouseEventArgs.Y - sliderMouseDownY;
                    sliderMouseDownY = e.MouseEventArgs.Y;
                    ChangeRectangle();
                }
            }
        }

        /// <summary>
        /// 滚动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MMouseEventArgs e)
        {
            if (e.MouseEventArgs.Delta > 0)
            {
                sliderMoveValue -= scrollInterval;
            }
            else
            {
                sliderMoveValue += scrollInterval;
            }
            ChangeRectangle();
        }
    }
}
