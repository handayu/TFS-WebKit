using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace mPaint
{
    public class MPolygonButton : MButton
    {
        /// <summary>
        /// 左上角的点，右上角的点，右下角的点,左下角的点
        /// </summary>
        public POINT[] points = new POINT[4] { new POINT(), new POINT(), new POINT(), new POINT() };

        /// <summary>
        /// 多边形颜色
        /// </summary>
        public int PolygonForeColor;

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(MPaintEventArgs e)
        {
            if (points != null && points.Length == 4)
            {
                //多边形
                if (this.isMouseEnter || hasChoose)
                {
                    e.CPaint.FillPolygon(PolygonForeColor, points);
                }
                //文字
                if (this.Text != null && this.Text.Length > 0)
                {
                    for (int i = 0; i < this.Text.Length; i++)
                    {
                        int a = e.CPaint.TextSize(this.Text, this.Font).cy;
                        string text = this.Text[i].ToString();
                        e.CPaint.DrawText(text, this.Font, drawForeColor,
                        new RECT(Rectangle.left + (Rectangle.right - Rectangle.left) / 2 - e.CPaint.TextSize(text, this.Font).cx / 2 + 1
                            , Rectangle.top + ((Rectangle.bottom - Rectangle.top - (this.Text.Length * a)) / 2) + (a + 2) * i
                            , Rectangle.right
                            , Rectangle.bottom));
                    }
                }
            }
        }
    }
}
