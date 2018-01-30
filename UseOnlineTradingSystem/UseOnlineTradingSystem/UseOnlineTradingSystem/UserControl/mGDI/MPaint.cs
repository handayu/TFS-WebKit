

using System.Runtime.InteropServices;
using System.Drawing;
using System;
namespace mPaint
{
    /// <summary>
    /// 颜色类
    /// </summary>
    public class COLOR
    {
        /// <summary>
        /// 获取空的颜色
        /// </summary>
        /// <returns></returns>
        public static int Empty()
        {
            return -1000000000;
        }

        /// <summary>
        /// 获取RGB颜色
        /// </summary>
        /// <param name="r">红色值</param>
        /// <param name="g">绿色值</param>
        /// <param name="b">蓝色值</param>
        /// <returns>RGB颜色</returns>
        public static int RGB(int r, int g, int b)
        {
            return ColorTranslator.ToWin32(Color.FromArgb(r, g, b));
        }
        /// <summary>
        /// 获取RGB颜色
        /// </summary>
        /// <param name="r">红色值</param>
        /// <returns>RGB颜色</returns>
        public static int RGB(Color cl)
        {
            return ColorTranslator.ToWin32(cl);
        }

        /// <summary>
        /// 获取ARGB颜色
        /// </summary>
        /// <param name="a">透明指</param>
        /// <param name="r">红色</param>
        /// <param name="g">绿色</param>
        /// <param name="b">蓝色</param>
        /// <returns></returns>
        public static double ARGB(int a, int r, int g, int b)
        {
            int rgb = ColorTranslator.ToWin32(Color.FromArgb(r, g, b));
            if (a == 0) return Empty();
            else return -rgb - (double)a / 1000;
        }
    }

    /// <summary>
    /// 字体类
    /// </summary>
    public class FONT
    {
        /// <summary>
        /// 创建字体
        /// </summary>
        public FONT()
        {
        }

        /// <summary>
        /// 创建字体
        /// </summary>
        /// <param name="fontFamily">字体</param>
        /// <param name="fontSize">字号</param>
        /// <param name="bold">是否粗体</param>
        /// <param name="underline">是否有下划线</param>
        /// <param name="italic">是否斜体</param>
        public FONT(string fontFamily, int fontSize, bool bold, bool underline, bool italic)
        {
            this.m_fontFamily = fontFamily;
            this.m_fontSize = fontSize;
            this.m_bold = bold;
            this.m_underline = underline;
            this.m_italic = italic;
        }

        private bool m_bold;

        /// <summary>
        /// 获取或设置是否粗体
        /// </summary>
        public bool Bold
        {
            get { return m_bold; }
            set { m_bold = value; }
        }

        private string m_fontFamily = "Arial";

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public string FontFamily
        {
            get { return m_fontFamily; }
            set { m_fontFamily = value; }
        }

        private int m_fontSize = 12;

        /// <summary>
        /// 获取或设置字体大小
        /// </summary>
        public int FontSize
        {
            get { return m_fontSize; }
            set { m_fontSize = value; }
        }

        private bool m_italic;

        /// <summary>
        /// 获取或设置是否斜体
        /// </summary>
        public bool Italic
        {
            get { return m_italic; }
            set { m_italic = value; }
        }

        private bool m_underline;

        /// <summary>
        /// 获取或设置是否有下划线
        /// </summary>
        public bool Underline
        {
            get { return m_underline; }
            set { m_underline = value; }
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4}",FontFamily, FontSize, Bold, Underline, Italic);
        }
    }

    /// <summary>
    /// 点类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public static POINT Empty
        {
            get
            {
                return new POINT(0, 0);
            }
        }
        /// <summary>
        /// 创建点
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 创建点
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        public POINT(float x, float y)
        {
            this.x = (int)x;
            this.y = (int)y;
        }
        /// <summary>
        /// 创建点
        /// </summary>
        /// <param name="p">点</param>
        public POINT(Point p)
        {
            this.x = p.X;
            this.y = p.Y;
        }
        /// <summary>
        /// 横坐标
        /// </summary>
        public int x;

        /// <summary>
        /// 纵坐标
        /// </summary>
        public int y;
    }

    /// <summary>
    /// 字体类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        /// <summary>
        /// 创建矩形
        /// </summary>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部坐标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// 创建矩形
        /// </summary>
        /// <param name="rect">矩形</param>
        public RECT(RECT rect)
        {
            this.left =rect.left;
            this.top = rect.top;
            this.right = rect.right;
            this.bottom = rect.bottom;
        }

        /// <summary>
        /// 创建矩形
        /// </summary>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部坐标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public RECT(float left, float top, float right, float bottom)
        {
            this.left = (int)left;
            this.top = (int)top;
            this.right = (int)right;
            this.bottom = (int)bottom;
        }

        public bool Contains(POINT p)
        {
            return Contains(p.x,p.y);
        }

        public bool Contains(Point p)
        {
            return Contains(p.X, p.Y);
        }

        public bool Contains(int x,int y)
        {
            if (left < x && right > x && top < y && bottom > y)
            {
                return true;
            }
            return false;
        }

        public void Join(RECT rc)
        {
            if (rc.left < left) left = rc.left;
            if (rc.top < top) top = rc.top;
            if (rc.right > right) right = rc.right;
            if (rc.bottom > bottom) bottom = rc.bottom;
        }

        /// <summary>
        /// 相交矩形
        /// </summary>
        /// <param name="rc"></param>
        /// <returns></returns>
        public RECT IntersectRect(RECT rc)
        {
            RECT r = new RECT();
            //那么两个矩形rect1{(minx1,miny1)(maxx1,   maxy1)},   rect2{(minx2,miny2)(maxx2,   maxy2)}  
            //相交的结果一定是个矩形，构成这个相交矩形rect{(minx,miny)(maxx,   maxy)}的点对坐标是：  
            int minx = Math.Max(left, rc.left);//从最小x中找最大的x
            int miny = Math.Max(top, rc.top);//从最小的y中找最大的y
            int maxx = Math.Min(right, rc.right);//从最大的x中找最小的x
            int maxy = Math.Min(bottom, rc.bottom);//从最大的y中找最小的y
            if (minx < maxx && miny < maxy) //这样才有面积的交集
            {
                r.left = minx;
                r.top = miny;
                r.right = maxx;
                r.bottom = maxy;

            }
            return r;
        }

        /// <summary>
        /// 移动矩形
        /// </summary>
        /// <param name="left">左移</param>
        /// <param name="top">上移</param>
        public void Offset(int left, int top)
        {
            this.left = this.left + left;
            this.top = this.top + top;
            this.right = this.right + left;
            this.bottom = this.bottom + top;
        }

        public static bool IsEmpty(RECT rect)
        {
            if (rect.left == 0 && rect.top == 0 && rect.right == 0 && rect.bottom == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 左侧坐标
        /// </summary>
        public int left;

        /// <summary>
        /// 顶部坐标
        /// </summary>
        public int top;

        /// <summary>
        /// 右侧坐标
        /// </summary>
        public int right;

        /// <summary>
        /// 底部坐标
        /// </summary>
        public int bottom;
    }

    /// <summary>
    /// 尺寸类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        /// <summary>
        /// 创建尺寸
        /// </summary>
        /// <param name="cx">宽</param>
        /// <param name="cy">高</param>
        public SIZE(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }

        /// <summary>
        /// 宽
        /// </summary>
        public int cx;
        /// <summary>
        /// 高
        /// </summary>
        public int cy;
    }
    /// <summary>
    /// 绘图类
    /// </summary>
    public interface MPaint:IDisposable
    {
        /// <summary>
        /// 开始绘图
        /// </summary>
        /// <param name="hdc">HDC</param>
        /// <param name="wRect">窗体区域</param>
        /// <param name="pRect">刷新区域</param>
        void BeginPaint(Graphics g, RECT wRect, RECT pRect);

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="lineStyle">样式</param>
        /// <param name="rect">矩形区域</param>
        void DrawEllipse(double dwPenColor, int width, int style, RECT rect);

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="lineStyle">样式</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        void DrawEllipse(double dwPenColor, int width, int style, int left, int top, int right, int bottom);

        /// <summary>
        /// 绘制渐变矩形
        /// </summary>
        /// <param name="beginColor">开始颜色</param>
        /// <param name="endColor">结束颜色</param>
        /// <param name="rect">矩形</param>
        /// <param name="angle">角度</param>
        void DrawGradientRect(int beginColor, int endColor, RECT rect, int angle);

        /// <summary>
        /// 绘制图片
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="rect">绘制区域</param>
        void DrawImage(string imagePath, RECT rect);

        /// <summary>
        /// 绘制图片
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="rect">绘制区域</param>
        void DrawImage(Image image, RECT rect);

        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="lineStyle">样式</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        void DrawLine(double dwPenColor, int width, int style, int x1, int y1, int x2, int y2);

        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="lineStyle">样式</param>
        /// <param name="x">第一个点的坐标</param>
        /// <param name="y">第二个点的坐标</param>
        void DrawLine(double dwPenColor, int width, int style, POINT x, POINT y);

        /// <summary>
        /// 绘制多边形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="lineStyle">样式</param>
        /// <param name="points">点的数组</param>
        void DrawPolygon(double dwPenColor, int width, int style, POINT[] points);

        /// <summary>
        /// 绘制大量直线
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="dashPattern">样式</param>
        void DrawPolyline(double dwPenColor, int width, int style, POINT[] points);

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="lineStyle">样式</param>
        /// <param name="rect">矩形区域</param>
        void DrawRect(double dwPenColor, int width, int style, RECT rect);

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="lineStyle">样式</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        void DrawRect(double dwPenColor, int width, int style, int left, int top, int right, int bottom);

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <param name="color">颜色</param>
        /// <param name="mp">坐标</param>
        void DrawText(string text, FONT font, double dwPenColor, POINT mp);

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <param name="color">颜色</param>
        /// <param name="rect">矩形区域</param>
        void DrawText(string text, FONT font, double dwPenColor, RECT rect);

        /// <summary>
        /// 结束绘图
        /// </summary>
        void EndPaint();

        /// <summary>
        /// 填充椭圆
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="rect">矩形区域</param>
        void FillEllipse(double dwPenColor, RECT rect);

        /// <summary>
        /// 填充椭圆
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        void FillEllipse(double dwPenColor, int left, int top, int right, int bottom);

        /// <summary>
        /// 填充多边形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="points">点的数组</param>
        void FillPolygon(double dwPenColor, POINT[] points);

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="rect">矩形区域</param>
        void FillRect(double dwPenColor, RECT rect);

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        void FillRect(double dwPenColor, int left, int top, int right, int bottom);

        /// <summary>
        /// 获取绘图对象
        /// </summary>
        /// <returns></returns>
        Graphics GetGraphics();

        /// <summary> 
        /// 填充圆角矩形 
        /// </summary> 
        /// <param name="dwPenColor">要填充的背景色</param> 
        /// <param name="rect">要填充的矩形</param> 
        /// <param name="r">圆角半径</param> 
        void DrawFillRoundRectangle(double dwPenColor, RECT rect, int r);

        /// <summary>
        /// 设置裁剪区域
        /// </summary>
        /// <param name="rect">区域</param>
        void SetClip(RECT rect);

        /// <summary>
        /// 设置偏移
        /// </summary>
        /// <param name="x">横向偏移</param>
        /// <param name="y">纵向偏移</param>
        void SetOffset(int x, int y);

        /// <summary>
        /// 获取文字大小
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <returns>字体大小</returns>
        SIZE TextSize(string text, FONT font);
    }
}