

using System.Drawing;
namespace mPaint
{
    /// <summary>
    /// 线
    /// </summary>
    public class MLine : MControlBase
    {
        public bool EnableMove = false;
        private POINT firstPoint;
        /// <summary>
        /// 获取或设置线的第一个点
        /// </summary>
        public POINT FirstPoint
        {
            get { return firstPoint; }
            set { firstPoint = value; }
        }

        private POINT secondPoint;
        /// <summary>
        /// 获取或设置线的第二个点
        /// </summary>
        public POINT SecondPoint
        {
            get { return secondPoint; }
            set { secondPoint = value; }
        }

        private int width=2;

        /// <summary>
        /// 线的宽度
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int lineColor = COLOR.RGB(Color.Gray);

        /// <summary>
        /// 线的颜色
        /// </summary>
        public int LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }
        /// <summary>
        /// 绘画
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(MPaintEventArgs e)
        {
            e.CPaint.DrawLine(lineColor, width, 0, this.firstPoint, this.secondPoint);
        }
    }
}
