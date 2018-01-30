using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mPaint
{
    /// <summary>
    /// 绘制矩形
    /// </summary>
    public class MRectangle : MControlBase
    {
        /// <summary>
        /// 绘画
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(MPaintEventArgs e)
        {
            e.CPaint.FillRect(BackColor, Rectangle);
        }
    }
}
