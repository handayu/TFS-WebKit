using System.Windows.Forms;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using UseOnlineTradingSystem;
using System.Drawing.Drawing2D;

namespace mPaint
{
    public class MBorad : Control, IDisposable
    {
        /// <summary>
        /// 是否移动
        /// </summary>
        private bool beginMove = false;

        /// <summary>
        /// 是否点击
        /// </summary>
        private bool isMouseDown = false;

        /// <summary>
        /// 绘图对象
        /// </summary>
        internal MPaint cp;

        /// <summary>
        /// 记录点击落下的坐标
        /// </summary>
        internal POINT mouseDownLocation = POINT.Empty;

        /// <summary>
        /// 记录前一个被操作的控件
        /// </summary>
        internal MControlBase previousControl = null;

        /// <summary>
        /// 鼠标按下的控件
        /// </summary>
        internal MControlBase mouseDownControl = null;
        private Graphics graphics;
        private MPaintEventArgs args;
        /// <summary>
        /// 构造方法
        /// </summary>
        public MBorad()
        {
            this.cp = new GdiPlusPaint();
            //this.cp = new GdiPaint();
            this.SizeChanged += new EventHandler(CBorad_SizeChanged);
            this.graphics = this.CreateGraphics();
            this.Paint += new PaintEventHandler(CBoard_Paint);
            this.MouseMove += new MouseEventHandler(CBoard_MouseMove);
            this.MouseDown += new MouseEventHandler(CBoard_MouseDown);
            this.MouseUp += new MouseEventHandler(CBoard_MouseUp);
            this.MouseEnter += new EventHandler(CBorad_MouseEnter);
            this.MouseLeave += new EventHandler(CBoard_MouseLeave);
            this.MouseWheel += new MouseEventHandler(CBoard_MouseWheel);
            args = new MPaintEventArgs(cp, this);
        }

        private void CBorad_SizeChanged(object sender, EventArgs e)
        {
            RECT rect = new RECT(0, 0, this.Width, this.Height);
            cp.BeginPaint(graphics, rect, rect);
        }

        private new bool IsDisposed = false;
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected new void Dispose(bool Disposing)
        {
            if (!IsDisposed)
            {
                if (Disposing)
                {
                    //清理托管资源
                    if (cp != null)
                    {
                        cp.Dispose();
                        cp = null;
                    }
                    if (objectsNeedToBeDraw != null)
                    {
                        foreach (var v in objectsNeedToBeDraw)
                        {

                        }
                    }
                }
                //清理非托管资源
            }
            IsDisposed = true;
        }
        ~MBorad()
        {
            Dispose(false);
        }

        /// <summary>
        /// 绘画事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBoard_Paint(object sender, PaintEventArgs e)
        {
            this.Draw();
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (beginMove)
            {
                WinAPI.ReleaseCapture();
                WinAPI.SendMessage(this.Parent.Handle, WinAPI.WM_SYSCOMMAND, WinAPI.SC_MOVE + WinAPI.HTCAPTION, 0);
            }
            else
            {
                if (previousControl != null)
                {
                    if (previousControl.Rectangle.Contains(e.Location.X, e.Location.Y))
                    {
                        MMouseEventArgs enterArgs = new MMouseEventArgs(e, this, previousControl);
                        previousControl.DoMouseMove(enterArgs);
                        this.Draw();
                    }
                    else
                    {
                        MMouseEventArgs args = new MMouseEventArgs(e, this, this.previousControl);
                        this.previousControl.DoMouseLeave(args);
                        this.Draw();
                        this.previousControl = null;
                    }
                }
                else
                {
                    MControlBase control = null;
                    for (int i = this.objectsNeedToBeDraw.Count - 1; i >= 0; i--)
                    {
                        MControlBase controlBase = objectsNeedToBeDraw[i];
                        if (controlBase.Visible && !(controlBase is MRectangle) && controlBase.Rectangle.Contains(new POINT(e.Location)))
                        {
                            control = controlBase;
                            break;
                        }
                    }
                    if (control != null)
                    {
                        MMouseEventArgs enterArgs = new MMouseEventArgs(e, this, control);
                        control.DoMouseEnter(enterArgs);
                        this.Draw();
                        this.previousControl = control;
                    }
                }
            }
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBorad_MouseEnter(object sender, EventArgs e)
        {
            //this.Draw();
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBoard_MouseLeave(object sender, EventArgs e)
        {
            //设置初始状态  
            beginMove = false;
            //this.Draw();
            //MouseEventArgs newE = new MouseEventArgs(MouseButtons.None, 0, PointToClient(MousePosition).X, PointToClient(MousePosition).Y, 0);
            //if (this.previousControl != null && this.previousControl.Visible )
            //{
            //    MMouseEventArgs args = new MMouseEventArgs(newE, this, this.previousControl);
            //    this.previousControl.DoMouseLeave(args);
            //    this.Draw();
            //    this.previousControl = null;
            //}
        }

        /// <summary>
        /// 鼠标点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBoard_MouseDown(object sender, MouseEventArgs e)
        {
            //将鼠标坐标赋给窗体左上角坐标  
            isMouseDown = true;
            mouseDownLocation = new POINT(e.Location);
            MControlBase control = null;
            for (int i = this.objectsNeedToBeDraw.Count - 1; i >= 0; i--)
            {
                MControlBase v = objectsNeedToBeDraw[i];
                if (v.Enable && v.Visible &&!(v is MRectangle)&& v.Rectangle.Contains(mouseDownLocation))
                {
                    control = v;
                    break;
                }
            }
            if (control != null)
            {
                mouseDownControl = control;
                MMouseEventArgs args = new MMouseEventArgs(e, this, control);
                control.DoMouseDown(args);
                this.Draw();
            }
            if(!(control is MUseTable))
            {
                beginMove = true;
            }
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBoard_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            beginMove = false;
            MControlBase control = null;
            for (int i = this.objectsNeedToBeDraw.Count - 1; i >= 0; i--)
            {
                MControlBase v = objectsNeedToBeDraw[i];
                if (v.Enable && v.Visible && !(v is MRectangle) && v.Rectangle.Contains(mouseDownLocation))
                {
                    control = v;
                    break;
                }
            }
            if (control != null)
            {
                MMouseEventArgs args = new MMouseEventArgs(e, this, control);
                control.DoMouseUp(args);
                if (control == this.mouseDownControl)
                {
                    control.DoClick(args);
                }
                mouseDownControl = null;
                this.Draw();
            }
            mouseDownControl = null;
        }

        /// <summary>
        /// 滚动鼠标滚轮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBoard_MouseWheel(object sender, MouseEventArgs e)
        {
            MMouseEventArgs args = new MMouseEventArgs(e, this, this.previousControl);
            foreach (var controlBase in objectsNeedToBeDraw)
            {
                if (controlBase.Visible)
                {
                    if(e.Y>controlBase.Rectangle.top&& e.Y< controlBase.Rectangle.bottom)
                    {
                        controlBase.DoMouseWheel(args);
                    }
                }
            }
            this.Draw();
        }

        private List<MControlBase> objectsNeedToBeDraw = new List<MControlBase>();

        /// <summary>
        /// 添加控件进集合
        /// </summary>
        /// <param name="control"></param>
        public void AddControl(MControlBase control)
        {
            this.objectsNeedToBeDraw.Add(control);
            control.Parent = this;
        }
        /// <summary>
        /// 从集合删除控件
        /// </summary>
        /// <param name="control"></param>
        public void RemoveControl(MControlBase control)
        {
            this.objectsNeedToBeDraw.Remove(control);
        }

        /// <summary>
        /// 全部刷新
        /// </summary>
        public void Draw()
        {
            try
            {
                RECT rect = new RECT(0, 0, this.Width, this.Height);
                cp.FillRect(COLOR.RGB(this.BackColor), rect);
                if (this.BackgroundImage != null)
                {
                    cp.DrawImage(this.BackgroundImage, rect);
                }
                foreach (MControlBase cb in objectsNeedToBeDraw)
                {
                    if (cb.Visible)
                    {
                        cb.DoPaint(args);
                    }
                }
                cp.EndPaint();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 局部刷新
        /// </summary>
        public void Draw(MControlBase cb)
        {
            try
            {
                if (cb.Visible)
                {
                    cp.SetClip(cb.Rectangle);
                    cb.DoPaint(args);
                    cp.EndPaint();
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 局部刷新
        /// </summary>
        public void Draw(List<MControlBase> cbs, RECT rect)
        {
            try
            {
                if (cbs == null)
                {
                    return;
                }
                cp.SetClip(rect);
                foreach (MControlBase cb in cbs)
                {
                    if (cb.Visible)
                    {
                        cb.DoPaint(args);
                    }
                }
                cp.EndPaint();
            }
            catch
            {

            }
        }
    }
}
