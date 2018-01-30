using System.Drawing;
using System.Windows.Forms;
namespace mPaint
{
    public class MButton : MControlBase
    {
        /// <summary>
        /// 获取顶层颜色
        /// </summary>
        public int TopColor
        {
            get
            {
                if (isMouseDown)
                {
                    return COLOR.RGB(230, 230, 230);
                }
                else if (isMouseEnter)
                {
                    return COLOR.RGB(250, 250, 250);
                }
                return COLOR.RGB(242, 242, 242);
            }
        }

        /// <summary>
        /// 获取顶部颜色2
        /// </summary>
        public int TopColor2
        {
            get
            {
                if (isMouseDown)
                {
                    return COLOR.RGB(220, 220, 220);
                }
                else if (isMouseEnter)
                {
                    return COLOR.RGB(240, 240, 240);
                }
                return COLOR.RGB(230, 230, 230);
            }
        }

        /// <summary>
        /// 获取底部颜色
        /// </summary>
        public int BottomColor
        {
            get
            {
                if (isMouseDown)
                {
                    return COLOR.RGB(200, 200, 200);
                }
                else if (isMouseEnter)
                {
                    return COLOR.RGB(230, 230, 230);
                }
                return COLOR.RGB(217, 217, 217);
            }
        }

        /// <summary>
        /// 获取底部颜色2
        /// </summary>
        public int BottomColor2
        {
            get
            {
                if (isMouseDown)
                {
                    return COLOR.RGB(192, 192, 192);
                }
                else if (isMouseEnter)
                {
                    return COLOR.RGB(220, 220, 220);
                }
                return COLOR.RGB(212, 212, 212);
            }
        }

        /// <summary>
        /// 获取绘制的文字颜色
        /// </summary>
        public int drawForeColor
        {
            get
            {
                if (!this.Enable)
                {
                    return COLOR.RGB(Color.Gray.R,Color.Gray.G,Color.Gray.B);
                }
                if (this.isMouseEnter||hasChoose)
                {
                    return base.MouseForeColor;
                }
                return base.ForeColor;
            }
        }

        /// <summary>
        /// 鼠标是否按下
        /// </summary>
        public bool isMouseDown = false;

        /// <summary>
        /// 鼠标是否进入
        /// </summary>
        public bool isMouseEnter = false;

        private Image backgroundImage;
        /// <summary>
        /// 获取或设置按钮的背景图片
        /// </summary>
        public Image BackgroundImage
        {
            get { return backgroundImage; }
            set { backgroundImage = value; }
        }

        private Image mouseEnterImage;
        /// <summary>
        /// 获取或设置鼠标进入的背景图片
        /// </summary>
        public Image MouseEnterImage
        {
            get { return mouseEnterImage; }
            set { mouseEnterImage = value; }
        }

        private Image mouseClickImage;
        /// <summary>
        /// 获取或设置鼠标按下的背景图片
        /// </summary>
        public Image MouseClickImage
        {
            get { return mouseClickImage; }
            set { mouseClickImage = value; }
        }

        private SIZE imageSIZE=new SIZE(0,0);

        /// <summary>
        /// 获取或者设置当前绘制图片的大小
        /// </summary>
        public SIZE ImageSIZE
        {
            get { return imageSIZE; }
            set { imageSIZE = value; }
        }

        /// <summary>
        /// 获取当前需要绘制的背景图片
        /// </summary>
        public Image image
        {
            get
            {
                if (this.isMouseDown)
                    return this.mouseClickImage;
                if (this.isMouseEnter)
                    return this.mouseEnterImage;
                return this.backgroundImage;
            }
        }
        /// <summary>
        /// 是否带边框
        /// </summary>
        public bool hasFrame = true;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool hasChoose = false;
        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(MPaintEventArgs e)
        {
            //if (!Redraw) { return; }
            Image currentImage = image;
            if (currentImage != null)
            {
                if (imageSIZE.cx > 0 && imageSIZE.cy>0)
                {
                    e.CPaint.DrawImage(image, new RECT(this.Rectangle.left, this.Rectangle.bottom - ImageSIZE.cy, this.Rectangle.left+ ImageSIZE.cx, this.Rectangle.bottom));
                }
                else
                {
                    e.CPaint.DrawImage(image, this.Rectangle);
                }
            }
            else if ((Rectangle.right - Rectangle.left) >= 2 && (Rectangle.bottom - Rectangle.top) >= 2)
            {
                if (hasChoose)
                {
                    e.CPaint.FillRect(MouseClickBackColor, Rectangle);
                }
                else if (isMouseEnter)
                {
                    e.CPaint.FillRect(MouseBackColor, Rectangle);
                }
                else
                {
                    e.CPaint.FillRect(BackColor, Rectangle);
                }

                if (hasFrame)
                {
                    //画渐变背景
                    e.CPaint.DrawGradientRect(TopColor, TopColor2, new RECT(Rectangle.left, Rectangle.top, Rectangle.right, Rectangle.bottom / 2), 90);
                    e.CPaint.DrawGradientRect(TopColor, TopColor2, new RECT(Rectangle.left, Rectangle.top + (Rectangle.bottom - Rectangle.top) / 2, Rectangle.right, Rectangle.bottom), 90);
                    //画边框
                    e.CPaint.DrawLine(COLOR.RGB(Color.White), 1, 0, Rectangle.left - 1, Rectangle.top - 1, Rectangle.right + 1, Rectangle.top - 1);
                    e.CPaint.DrawLine(COLOR.RGB(Color.White), 1, 0, Rectangle.left - 1, Rectangle.top - 1, Rectangle.left - 1, Rectangle.bottom + 1);
                    e.CPaint.DrawLine(COLOR.RGB(Color.White), 1, 0, Rectangle.right + 1, Rectangle.top - 1, Rectangle.right + 1, Rectangle.bottom + 1);
                    e.CPaint.DrawLine(COLOR.RGB(Color.White), 1, 0, Rectangle.left - 1, Rectangle.bottom + 1, Rectangle.right + 1, Rectangle.bottom + 1);

                    e.CPaint.DrawLine(COLOR.RGB(166, 166, 166), 1, 0, Rectangle.left + 1, Rectangle.top + 1, Rectangle.right - 1, Rectangle.top + 1);
                    e.CPaint.DrawLine(COLOR.RGB(166, 166, 166), 1, 0, Rectangle.left + 1, Rectangle.top + 1, Rectangle.left + 1, Rectangle.bottom - 1);
                    e.CPaint.DrawLine(COLOR.RGB(166, 166, 166), 1, 0, Rectangle.right - 1, Rectangle.top + 1, Rectangle.right - 1, Rectangle.bottom - 1);
                    e.CPaint.DrawLine(COLOR.RGB(166, 166, 166), 1, 0, Rectangle.left + 1, Rectangle.bottom - 1, Rectangle.right - 1, Rectangle.bottom - 1);
                }
                //文字
                if (this.Text != null && this.Text.Length > 0)
                {
                    e.CPaint.DrawText(this.Text, this.Font, drawForeColor,
                    new RECT(Rectangle.left + (Rectangle.right - Rectangle.left) / 2 - e.CPaint.TextSize(this.Text, this.Font).cx / 2 + 1
                        , Rectangle.top + (Rectangle.bottom - Rectangle.top - e.CPaint.TextSize(this.Text, this.Font).cy) / 2-1
                        , Rectangle.right
                        , Rectangle.bottom));
                }
            }
            //this.Redraw = false;
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
                this.isMouseDown = true;
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
                this.isMouseDown = false;
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
    }
}
