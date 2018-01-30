using System.Windows.Forms;

namespace mPaint
{
    public class MLabel : MControlBase
    {
        public bool Underline = false;
        private bool isMouseEnter = false;
        private RECT textRECT = new RECT();
        private bool reRectangle = false;
        public bool LeftAligned = false;
        public new RECT Rectangle
        {
            get { return base.Rectangle; }
            set
            {
                reRectangle = true;
                base.Rectangle = value;
            }
        }
        public RECT TextRECT
        {
            get { return textRECT; }
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(MPaintEventArgs e)
        {
            if (reRectangle)
            {
                reRectangle = false;
                if (LeftAligned)
                {
                    textRECT.left = base.Rectangle.left;
                    textRECT.top = base.Rectangle.top + (base.Rectangle.bottom - base.Rectangle.top) / 2 - e.CPaint.TextSize(base.Text, base.Font).cy / 2;
                    textRECT.right = base.Rectangle.right;
                    textRECT.bottom = base.Rectangle.bottom;
                }
                else
                {
                    textRECT.left = base.Rectangle.left + (base.Rectangle.right - base.Rectangle.left) / 2 - e.CPaint.TextSize(base.Text, base.Font).cx / 2;
                    textRECT.top = base.Rectangle.top + (base.Rectangle.bottom - base.Rectangle.top) / 2 - e.CPaint.TextSize(base.Text, base.Font).cy / 2;
                    textRECT.right = base.Rectangle.right;
                    textRECT.bottom = base.Rectangle.bottom;
                }
            }
            if(BackColor>0)
            {
                e.CPaint.FillRect(BackColor, base.Rectangle);
            }
            if(!string.IsNullOrEmpty(base.Text))
            {
                int color = base.ForeColor;
                if (isMouseEnter)
                {
                    color = base.MouseForeColor;
                }
                if (!base.Text.Contains("\r\n"))
                {
                    e.CPaint.DrawText(base.Text, base.Font, color, textRECT);
                }
                else
                {
                    string[] array = base.Text.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
                    if (array != null)
                    {
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i] != null)
                            {
                                int h = textRECT.bottom - textRECT.top;
                                int drift = 8;
                                RECT rect = new RECT(textRECT.left, textRECT.top + (h - drift) * i, textRECT.right, textRECT.bottom + (h - drift) * i);
                                e.CPaint.DrawText(array[i], base.Font, color, rect);
                                array[i] = null;
                            }
                        }
                    }
                }
                if (Underline&&isMouseEnter)
                {
                    e.CPaint.DrawLine(color,1,0, textRECT.left, textRECT.bottom+1, textRECT.right, textRECT.bottom+1);
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
