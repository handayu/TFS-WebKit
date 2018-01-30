using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace mPaint
{
    public class MImage : MControlBase
    {
        private Image backgroundImage = null;
        /// <summary>
        /// 获取或设置背景图片
        /// </summary>
        public Image BackgroundImage
        {
            get { return backgroundImage; }
            set { backgroundImage = value; }
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(MPaintEventArgs e)
        {
            if (this.backgroundImage != null)
            {
                e.CPaint.DrawImage(this.backgroundImage, base.Rectangle);
            }
        }
    }
}
