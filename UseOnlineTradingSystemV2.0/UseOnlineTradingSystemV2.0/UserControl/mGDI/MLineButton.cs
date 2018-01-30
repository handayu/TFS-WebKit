using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace mPaint
{
    public class MLineButton: MButton
    {
        /// <summary>
        /// 下划线线宽
        /// </summary>
        public int lineWidth = 2;
        /// <summary>
        /// 下划线线宽
        /// </summary>
        public int LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(MPaintEventArgs e)
        {
            if ((Rectangle.right - Rectangle.left) >= 2 && (Rectangle.bottom - Rectangle.top) >= 2)
            {
                //文字
                if (this.Text != null && this.Text.Length > 0)
                {
                    e.CPaint.DrawText(this.Text, this.Font, drawForeColor,
                    new RECT(Rectangle.left + (Rectangle.right - Rectangle.left) / 2 - e.CPaint.TextSize(this.Text, this.Font).cx / 2 + 1
                        , Rectangle.top + (Rectangle.bottom - Rectangle.top - e.CPaint.TextSize(this.Text, this.Font).cy) / 2
                        , Rectangle.right
                        , Rectangle.bottom));
                }
                if (hasChoose)
                {
                    e.CPaint.DrawLine(drawForeColor, lineWidth, 0, Rectangle.left + 1, Rectangle.bottom - 1, Rectangle.right - 1, Rectangle.bottom - 1);
                }
            }
        }
    }
}
