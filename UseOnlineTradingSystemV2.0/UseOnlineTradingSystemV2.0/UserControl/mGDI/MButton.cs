using System.Drawing;
using System.Windows.Forms;
namespace mPaint
{
    public class MButton : MControlBase
    {
        /// <summary>
        /// ��ȡ������ɫ
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
        /// ��ȡ������ɫ2
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
        /// ��ȡ�ײ���ɫ
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
        /// ��ȡ�ײ���ɫ2
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
        /// ��ȡ���Ƶ�������ɫ
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
        /// ����Ƿ���
        /// </summary>
        public bool isMouseDown = false;

        /// <summary>
        /// ����Ƿ����
        /// </summary>
        public bool isMouseEnter = false;

        private Image backgroundImage;
        /// <summary>
        /// ��ȡ�����ð�ť�ı���ͼƬ
        /// </summary>
        public Image BackgroundImage
        {
            get { return backgroundImage; }
            set { backgroundImage = value; }
        }

        private Image mouseEnterImage;
        /// <summary>
        /// ��ȡ������������ı���ͼƬ
        /// </summary>
        public Image MouseEnterImage
        {
            get { return mouseEnterImage; }
            set { mouseEnterImage = value; }
        }

        private Image mouseClickImage;
        /// <summary>
        /// ��ȡ��������갴�µı���ͼƬ
        /// </summary>
        public Image MouseClickImage
        {
            get { return mouseClickImage; }
            set { mouseClickImage = value; }
        }

        private SIZE imageSIZE=new SIZE(0,0);

        /// <summary>
        /// ��ȡ�������õ�ǰ����ͼƬ�Ĵ�С
        /// </summary>
        public SIZE ImageSIZE
        {
            get { return imageSIZE; }
            set { imageSIZE = value; }
        }

        /// <summary>
        /// ��ȡ��ǰ��Ҫ���Ƶı���ͼƬ
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
        /// �Ƿ���߿�
        /// </summary>
        public bool hasFrame = true;

        /// <summary>
        /// �Ƿ�ѡ��
        /// </summary>
        public bool hasChoose = false;
        /// <summary>
        /// ����
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
                    //�����䱳��
                    e.CPaint.DrawGradientRect(TopColor, TopColor2, new RECT(Rectangle.left, Rectangle.top, Rectangle.right, Rectangle.bottom / 2), 90);
                    e.CPaint.DrawGradientRect(TopColor, TopColor2, new RECT(Rectangle.left, Rectangle.top + (Rectangle.bottom - Rectangle.top) / 2, Rectangle.right, Rectangle.bottom), 90);
                    //���߿�
                    e.CPaint.DrawLine(COLOR.RGB(Color.White), 1, 0, Rectangle.left - 1, Rectangle.top - 1, Rectangle.right + 1, Rectangle.top - 1);
                    e.CPaint.DrawLine(COLOR.RGB(Color.White), 1, 0, Rectangle.left - 1, Rectangle.top - 1, Rectangle.left - 1, Rectangle.bottom + 1);
                    e.CPaint.DrawLine(COLOR.RGB(Color.White), 1, 0, Rectangle.right + 1, Rectangle.top - 1, Rectangle.right + 1, Rectangle.bottom + 1);
                    e.CPaint.DrawLine(COLOR.RGB(Color.White), 1, 0, Rectangle.left - 1, Rectangle.bottom + 1, Rectangle.right + 1, Rectangle.bottom + 1);

                    e.CPaint.DrawLine(COLOR.RGB(166, 166, 166), 1, 0, Rectangle.left + 1, Rectangle.top + 1, Rectangle.right - 1, Rectangle.top + 1);
                    e.CPaint.DrawLine(COLOR.RGB(166, 166, 166), 1, 0, Rectangle.left + 1, Rectangle.top + 1, Rectangle.left + 1, Rectangle.bottom - 1);
                    e.CPaint.DrawLine(COLOR.RGB(166, 166, 166), 1, 0, Rectangle.right - 1, Rectangle.top + 1, Rectangle.right - 1, Rectangle.bottom - 1);
                    e.CPaint.DrawLine(COLOR.RGB(166, 166, 166), 1, 0, Rectangle.left + 1, Rectangle.bottom - 1, Rectangle.right - 1, Rectangle.bottom - 1);
                }
                //����
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
        /// ��갴��
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
        /// ���̧��
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
        /// ������
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
        /// ����뿪
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
