using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mPaint
{
    public class MSelect : MControlBase
    {
        /// <summary>
        /// 数据组
        /// </summary>
        public List<string> Items = new List<string>();

        private void ChangeRectangle()
        {
            if (hasChoose)
            {
                Rectangle = new RECT(imageRectangle.left, imageRectangle.top, imageRectangle.right, imageRectangle.top + 30 * Items.Count + 30);
            }
            else
            {
                Rectangle = ImageRectangle;
            }
        }

        private string currentObj = null;

        /// <summary>
        /// 鼠标是否按下
        /// </summary>
        private bool isMouseDown = false;

        /// <summary>
        /// 鼠标是否进入
        /// </summary>
        protected bool isMouseEnter = false;

        private bool hasChoose = false;

        public void SetChoose()
        {
            if (imageRectangle.Contains(XY))
            {
                hasChoose = true;
            }
            else if (Rectangle.Contains(XY))
            {
                if (currentObj != null)
                {
                    this.Text = currentObj;
                }
                hasChoose = false;
            }
            else
            {
                hasChoose = false;
            }
        }

        private Image backgroundImage;
        /// <summary>
        /// 获取或设置背景图片
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

        /// <summary>
        /// 获取当前需要绘制的背景图片
        /// </summary>
        private Image image
        {
            get
            {
                if (hasChoose)
                    return this.mouseClickImage;
                return this.backgroundImage;
            }
        }

        private int dropDownBoxForeColor;
        /// <summary>
        /// 下拉框中字体颜色
        /// </summary>
        public int DropDownBoxForeColor
        {
            get { return dropDownBoxForeColor; }
            set { dropDownBoxForeColor = value; }
        }

        private int dropDownBoxBackColor;
        
        /// <summary>
        /// 下拉框背景颜色
        /// </summary>
        public int DropDownBoxBackColor
        {
            get { return dropDownBoxBackColor; }
            set { dropDownBoxBackColor = value; }
        }

        private int dropDownBoxRowMouseEnterColor;
        /// <summary>
        /// 下拉框行悬浮行颜色
        /// </summary>
        public int DropDownBoxRowMouseEnterColor
        {
            get { return dropDownBoxRowMouseEnterColor; }
            set { dropDownBoxRowMouseEnterColor = value; }
        }


        private RECT imageRectangle;

        /// <summary>
        /// 选择框区域
        /// </summary>
        public RECT ImageRectangle
        {
            get { return imageRectangle; }
            set
            {
                imageRectangle = value;
                ChangeRectangle();
            }
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(MPaintEventArgs e)
        {
            Image currentImage = image;
            if (currentImage != null)
            {
                e.CPaint.DrawImage(image, imageRectangle);

                //文字
                if (this.Text != null && this.Text.Length > 0)
                {
                    e.CPaint.DrawText(this.Text, this.Font, ForeColor,
                    new RECT(imageRectangle.left+10
                        , imageRectangle.top + (imageRectangle.bottom - imageRectangle.top - e.CPaint.TextSize(this.Text, this.Font).cy) / 2 - 1
                        , imageRectangle.right
                        , imageRectangle.bottom));
                }

                if (hasChoose)
                {
                    //下拉框
                    RECT rect1 = new RECT(imageRectangle.left, imageRectangle.bottom + 2, imageRectangle.right + 10, imageRectangle.bottom + 2 + 30 * Items.Count);
                    e.CPaint.FillRect(dropDownBoxBackColor, rect1);

                    if (XY.x != int.MaxValue && XY.y != int.MaxValue)
                    {
                        //选中的行
                        for (int i = 0; i < Items.Count; i++)
                        {
                            RECT rect2 = new RECT(rect1.left, rect1.top + 30 * i, rect1.right, rect1.top + 30 * (i + 1));
                            if (rect2.Contains(XY))
                            {
                                e.CPaint.FillRect(dropDownBoxRowMouseEnterColor, rect2);
                                currentObj= Items[i];
                                break;
                            }
                        }
                    }

                    //绘制下拉框中的文字
                    for (int i = 0; i < Items.Count; i++)
                    {
                        e.CPaint.DrawText(Items[i], this.Font, dropDownBoxForeColor, new RECT(rect1.left + 10, rect1.top + 30 * i + (30 - e.CPaint.TextSize(Items[i], this.Font).cy) / 2 - 1
                                , rect1.right, rect1.top + 30 * i + 30));
                    }
                }
            }
        }
  
        private POINT XY = new POINT();
        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MMouseEventArgs e)
        {
            XY.x = e.MouseEventArgs.X;
            XY.y = e.MouseEventArgs.Y;
            base.OnMouseMove(e);
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
                SetChoose();
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
                XY.x = e.MouseEventArgs.X;
                XY.y = e.MouseEventArgs.Y;
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
                XY.x = int.MaxValue;
                XY.y = int.MaxValue;
                currentObj = null;
            }
        }
    }
}
