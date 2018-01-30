

using System.Drawing;
using System;
using System.Runtime.InteropServices;
namespace mPaint
{
    /// <summary>
    /// Gdi绘图类
    /// </summary>
    public class GdiPaint : MPaint
    {
        /// <summary>
        /// 位图
        /// </summary>
        private Bitmap m_bitmap;

        /// <summary>
        /// 绘图对象
        /// </summary>
        private Graphics m_g;

        /// <summary>
        /// 绘图句柄
        /// </summary>
        private IntPtr m_hDC;

        /// <summary>
        /// 裁剪
        /// </summary>
        private IntPtr m_hrgn;

        /// <summary>
        /// 横向偏移
        /// </summary>
        private int m_offsetX;

        /// <summary>
        /// 纵向偏移
        /// </summary>
        private int m_offsetY;

        /// <summary>
        /// 刷新的矩形
        /// </summary>
        private RECT m_rect;

        /// <summary>
        /// 控件的HDC
        /// </summary>
        private IntPtr m_wndHdc;

        private const int FW_BOLD = 700;
        private const int FW_REGULAR = 400;
        private int GB2312_CHARSET = 134;
        private int OUT_DEFAULT_PRECIS = 0;
        private int CLIP_DEFAULT_PRECIS = 0;
        private int DEFAULT_QUALITY = 0;
        private int DEFAULT_PITCH = 0;
        private int FF_SWISS = 0;
        private const int DT_SINGLELINE = 0x20;
        private const int DT_TOP = 0;  
        private const int DT_LEFT = 0; 
        private const int DT_CENTER = 1;
        private const int DT_RIGHT = 2;  
        private const int DT_VCENTER = 4;  
        private const int DT_BOTTOM = 8;
        private const int DT_NOPREFIX = 0x00000800;
        private const int HOLLOW_BRUSH = 5;
        private const int IMAGE_BITMAP = 0;
        private const int LR_LOADFROMFILE = 0x00000010;
        private const int TRANSPARENT = 1;
        private Graphics graphics;
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(
        IntPtr hdcDest, 
        int nXDest,
        int nYDest,
        int nWidth,
        int nHeight,
        IntPtr hdcSrc,
        int nXSrc,
        int nYSrc,
        System.Int32 dwRop 
        );

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateFont(int H, int W, int E, int O, int FW, int I, int u, int S, int C, int OP, int CP, int Q, int PAF, string F);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreatePen(int style, int width, int dwPenColorREF);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRectRgnIndirect(ref RECT rect);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateSolidBrush(int dwPenColorREF);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int DrawText(IntPtr hdc, string lpStr, int nCount, ref RECT lpRect, int wFormat);

        [DllImport("gdi32.dll")]
        private static extern int Ellipse(IntPtr hdc, int left, int top, int right, int bottom);

        [DllImport("user32.dll")]
        private static extern int FillRect(IntPtr hdc, ref RECT lpRect, IntPtr brush);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetTextExtentPoint32(IntPtr hdc, string text, int length, ref SIZE size);

        [DllImport("gdi32.dll")]
        private static extern IntPtr GetStockObject(int fnObject);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int LineTo(IntPtr hdc, int x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadImage(IntPtr hinst, string lpszName, int uType, int cxDesired, int cyDesired, int fuLoad);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int MoveToEx(IntPtr hdc, int x, int y, IntPtr lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int Polygon(IntPtr hdc, POINT[] apt, int cpt);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int Polyline(IntPtr hdc, POINT[] apt, int cpt);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int Rectangle(IntPtr hdc, int left, int top, int right, int bottom);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("gdi32.dll")]
        private static extern int SetBkMode(IntPtr hdc, int nBkMode);

        [DllImport("gdi32.dll")]
        private static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

        [DllImport("gdi32.dll")]
        private static extern int SetTextColor(IntPtr hdc, int dwPenColorREF);

        /// <summary>
        /// 开始绘图
        /// </summary>
        /// <param name="hdc">HDC</param>
        /// <param name="wRect">窗体区域</param>
        /// <param name="pRect">刷新区域</param>
        public void BeginPaint(Graphics g, RECT wRect, RECT pRect)
        {
            if (m_bitmap != null)
            {
                DeleteObject(m_hDC);
                m_bitmap.Dispose();
                m_bitmap = null;
                m_hDC = IntPtr.Zero;
                m_g.Dispose();
                m_g = null;
            }
            graphics = g;
            if (m_wndHdc == IntPtr.Zero)
            {
                m_wndHdc = graphics.GetHdc();
            }
            m_rect = pRect;
            int imageW = wRect.right - wRect.left;
            int imageH = wRect.bottom - wRect.top;
            if (imageW == 0) imageW = 1;
            if (imageH == 0) imageH = 1;
            m_bitmap = new Bitmap(imageW, imageH);
            m_g = Graphics.FromImage(m_bitmap);
            m_hDC = m_g.GetHdc();
            RECT rc = new RECT(-1, -1, 1, 1);
            FONT font = new FONT();
            DrawText(" ", font, 0, rc);
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            DeleteObject(m_hDC);
            DeleteObject(m_wndHdc);
            if (m_hrgn != IntPtr.Zero)
            {
                DeleteObject(m_hrgn);
                m_hrgn = IntPtr.Zero;
            }
            if (m_g != null)
            {
                m_g.Dispose();
            }
            if (m_bitmap != null)
            {
                m_bitmap.Dispose();
            }
            if (graphics != null)
            {
                graphics.Dispose();
            }
            m_hDC = IntPtr.Zero;
            m_wndHdc = IntPtr.Zero;
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="lineStyle">样式</param>
        /// <param name="rect">矩形区域</param>
        public void DrawEllipse(double dwPenColor, int width, int style, RECT rect)
        {
            DrawEllipse(dwPenColor, width, style, rect.left, rect.top, rect.right, rect.bottom);
        }

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
        public void DrawEllipse(double dwPenColor, int width, int style, int left, int top, int right, int bottom)
        {
            if (dwPenColor == COLOR.Empty()) return;
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor);
            IntPtr hPen = CreatePen((int)dwPenColor, width, style); 
            IntPtr hOldPen = SelectObject(m_hDC, hPen);
            SelectObject(m_hDC, GetStockObject(HOLLOW_BRUSH));
            Ellipse(m_hDC, left + m_offsetX, top + m_offsetY, right + m_offsetX + 1, bottom + m_offsetY);
            SelectObject(m_hDC, hOldPen);
            DeleteObject(hPen); 
        }

        /// <summary>
        /// 绘制渐变矩形
        /// </summary>
        /// <param name="beginColor">开始颜色</param>
        /// <param name="endColor">结束颜色</param>
        /// <param name="rect">矩形</param>
        /// <param name="angle">角度</param>
        public void DrawGradientRect(int beginColor, int endColor, RECT rect, int angle)
        {
        }

        /// <summary>
        /// 绘制图片
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="rect">绘制区域</param>
        public void DrawImage(string imagePath, RECT rect)
        {
            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;
            IntPtr bitmap = LoadImage(IntPtr.Zero, imagePath, 2, width, height, LR_LOADFROMFILE);
            IntPtr hdcsource = CreateCompatibleDC(IntPtr.Zero);
            SelectObject(hdcsource, bitmap);
            BitBlt(m_hDC, rect.left + m_offsetX, rect.top + m_offsetY, width, height, hdcsource, 0, 0, 13369376);
            DeleteObject(bitmap);
            DeleteObject(hdcsource);
        }

        /// <summary>
        /// 绘制图片
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="rect">绘制区域</param>
        public void DrawImage(Image image, RECT rect)
        {
            //string imagePath = @"d:\Desert.jpg";
            //DrawImage(imagePath,rect);
        }

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
        public void DrawLine(double dwPenColor, int width, int style, int x1, int y1, int x2, int y2)
        {
            if (dwPenColor == COLOR.Empty()) return;
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor);
            IntPtr hPen = CreatePen(style, width, (int)dwPenColor);
            IntPtr hOldPen = SelectObject(m_hDC, hPen);
            MoveToEx(m_hDC, x1 + m_offsetX, y1 + m_offsetY, IntPtr.Zero);
            LineTo(m_hDC, x2 + m_offsetX, y2 + m_offsetY);
            SelectObject(m_hDC, hOldPen);
            DeleteObject(hPen);
        }

        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="lineStyle">样式</param>
        /// <param name="x">第一个点的坐标</param>
        /// <param name="y">第二个点的坐标</param>
        public void DrawLine(double dwPenColor, int width, int style, POINT x, POINT y)
        {
            DrawLine(dwPenColor, width, style, x.x, x.y, y.x, y.y);
        }

        /// <summary>
        /// 绘制多边形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="lineStyle">样式</param>
        /// <param name="points">点的数组</param>
        public void DrawPolygon(double dwPenColor, int width, int style, POINT[] points)
        {
            if (dwPenColor == COLOR.Empty()) return;
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor);
            POINT[] newPoints = new POINT[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                POINT newPoint = new POINT(points[i].x + m_offsetX, points[i].y + m_offsetY);
                newPoints[i] = newPoint;
            }
            IntPtr hPen = CreatePen(style, width, (int)dwPenColor);
            IntPtr hOldPen = SelectObject(m_hDC, hPen);
            Polygon(m_hDC, newPoints, newPoints.Length);
            SelectObject(m_hDC, hOldPen);
            DeleteObject(hPen);
        }

        /// <summary>
        /// 绘制大量直线
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="dashPattern">样式</param>
        public void DrawPolyline(double dwPenColor, int width, int style, POINT[] points)
        {
            if (dwPenColor == COLOR.Empty()) return;
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor);
            POINT[] newPoints = new POINT[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                POINT newPoint = new POINT(points[i].x + m_offsetX, points[i].y + m_offsetY);
                newPoints[i] = newPoint;
            }
            IntPtr hPen = CreatePen(style, width, (int)dwPenColor);
            IntPtr hOldPen = SelectObject(m_hDC, hPen);
            Polyline(m_hDC, newPoints, newPoints.Length);
            SelectObject(m_hDC, hOldPen);
            DeleteObject(hPen);
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="lineStyle">样式</param>
        /// <param name="rect">矩形区域</param>
        public void DrawRect(double dwPenColor, int width, int style, RECT rect)
        {
            DrawRect(dwPenColor, width, style, rect.left, rect.top, rect.right, rect.bottom);
        }

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
        public void DrawRect(double dwPenColor, int width, int style, int left, int top, int right, int bottom)
        {
            if (dwPenColor == COLOR.Empty()) return;
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor);
            IntPtr hPen = CreatePen(style, width, (int)dwPenColor);
	        IntPtr hOldPen = SelectObject(m_hDC, hPen);
	        SelectObject(m_hDC, GetStockObject(HOLLOW_BRUSH));
	        Rectangle(m_hDC, left + m_offsetX, top + m_offsetY, right + m_offsetX + 1, bottom + m_offsetY);
	        SelectObject(m_hDC, hOldPen);
	        DeleteObject(hPen);
        }

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <param name="color">颜色</param>
        /// <param name="mp">坐标</param>
        public void DrawText(string text, FONT font, double dwPenColor, POINT mp)
        {
            if (dwPenColor == COLOR.Empty()) return;
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor);
            IntPtr hFont = CreateFont
            (
                font.FontSize, 0,
                0, 0,
                font.Bold ? FW_BOLD : FW_REGULAR,
                0, 0, 0,
                GB2312_CHARSET,
                OUT_DEFAULT_PRECIS,
                CLIP_DEFAULT_PRECIS,
                DEFAULT_QUALITY,
                DEFAULT_PITCH | FF_SWISS,
                font.FontFamily
            );
            SIZE size = TextSize(text, font);
            RECT newRect = new RECT(mp.x + m_offsetX, mp.y + m_offsetY, mp.x + m_offsetX + size.cx, mp.y + m_offsetY + size.cy);
            SetBkMode(m_hDC, TRANSPARENT);
            SetTextColor(m_hDC, (int)dwPenColor);
            IntPtr hOldFont = SelectObject(m_hDC, hFont);
            DrawText(m_hDC, text, -1, ref newRect, 0 | DT_NOPREFIX);
            SelectObject(m_hDC, hOldFont);
            DeleteObject(hFont);
        }

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <param name="color">颜色</param>
        /// <param name="rect">矩形区域</param>
        public void DrawText(string text, FONT font, double dwPenColor, RECT rect)
        {
            if (dwPenColor == COLOR.Empty()) return;
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor);
            IntPtr hFont = CreateFont
            (
                font.FontSize, 0,
                0, 0,
                font.Bold ? FW_BOLD : FW_REGULAR,
                font.Italic ? 1 : 0,
                font.Underline ? 1 : 0,
                0,
                GB2312_CHARSET,
                OUT_DEFAULT_PRECIS,
                CLIP_DEFAULT_PRECIS,
                DEFAULT_QUALITY,
                DEFAULT_PITCH | FF_SWISS,
                font.FontFamily
            );
            RECT newRect = new RECT(rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY);
	        SetBkMode(m_hDC, TRANSPARENT);
            SetTextColor(m_hDC, (int)dwPenColor);
	        IntPtr hOldFont = SelectObject(m_hDC, hFont);
	        DrawText(m_hDC, text, -1, ref newRect, 0 | DT_NOPREFIX);
	        SelectObject(m_hDC, hOldFont);
	        DeleteObject(hFont);
        }

        /// <summary>
        /// 结束绘图
        /// </summary>
        public void EndPaint()
        {
            BitBlt(m_wndHdc, m_rect.left, m_rect.top, m_rect.right - m_rect.left, m_rect.bottom - m_rect.top, m_hDC, m_rect.left, m_rect.top, 13369376);
            //DeleteObject(m_hDC);
            //DeleteObject(m_wndHdc);
            //if (m_hrgn!=IntPtr.Zero)
            //{
            //    DeleteObject(m_hrgn);
            //    m_hrgn = IntPtr.Zero;
            //}
            //if (m_g != null)
            //{
            //    m_g.Dispose();
            //}
            //if (m_bitmap != null)
            //{
            //    m_bitmap.Dispose();
            //}
            //if (graphics != null)
            //{
            //    graphics.Dispose();
            //}
            //m_hDC = IntPtr.Zero;
            //m_wndHdc = IntPtr.Zero;
            m_offsetX = 0;
            m_offsetY = 0;
        }

        /// <summary>
        /// 填充椭圆
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="rect">矩形区域</param>
        public void FillEllipse(double dwPenColor, RECT rect)
        {
            FillEllipse(dwPenColor, rect.left, rect.top, rect.right, rect.bottom);
        }

        /// <summary>
        /// 填充椭圆
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public void FillEllipse(double dwPenColor, int left, int top, int right, int bottom)
        {
            if (dwPenColor == COLOR.Empty()) return;
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor);
            IntPtr brush = CreateSolidBrush((int)dwPenColor);
	        SelectObject(m_hDC, brush);
	        Ellipse(m_hDC, left + m_offsetX, top + m_offsetY, right + m_offsetX, bottom + m_offsetY);
	        DeleteObject(brush);
        }

        /// <summary>
        /// 填充多边形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="points">点的数组</param>
        public void FillPolygon(double dwPenColor, POINT[] points)
        {
            if (dwPenColor == COLOR.Empty()) return;
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor);
            POINT[] newPoints = new POINT[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                POINT newPoint = new POINT(points[i].x + m_offsetX, points[i].y + m_offsetY);
                newPoints[i] = newPoint;
            }
            IntPtr brush = CreateSolidBrush((int)dwPenColor);
            SelectObject(m_hDC, brush);
            Polygon(m_hDC, newPoints, newPoints.Length);
            DeleteObject(brush);
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="rect">矩形区域</param>
        public void FillRect(double dwPenColor, RECT rect)
        {
            if (dwPenColor == COLOR.Empty()) return;
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor);
            RECT newRect = new RECT(rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY);
            IntPtr brush = CreateSolidBrush((int)dwPenColor);
	        SelectObject(m_hDC, brush);
	        FillRect(m_hDC, ref newRect, brush);
	        DeleteObject(brush);
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public void FillRect(double dwPenColor, int left, int top, int right, int bottom)
        {
            FillRect(dwPenColor, new RECT(left, top, right, bottom));
        }

        /// <summary>
        /// 获取绘图对戏
        /// </summary>
        /// <returns></returns>
        public Graphics GetGraphics()
        {
            return m_g;
        }

        /// <summary> 
        /// 填充圆角矩形 
        /// </summary> 
        /// <param name="dwPenColor">要填充的背景色</param> 
        /// <param name="rect">要填充的矩形</param> 
        /// <param name="r">圆角半径</param> 
        public void DrawFillRoundRectangle(double dwPenColor, RECT rect, int r)
        {

        }

        /// <summary>
        /// 设置偏移
        /// </summary>
        /// <param name="x">横向偏移</param>
        /// <param name="y">纵向偏移</param>
        public void SetOffset(int x, int y)
        {
            m_offsetX = x;
            m_offsetY = y;
        }

        /// <summary>
        /// 获取文字大小
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <returns>字体大小</returns>
        public SIZE TextSize(string text, FONT font)
        {
            SIZE size = new SIZE();
            IntPtr hFont = CreateFont
            (
                font.FontSize, 0,
                0, 0,
                font.Bold ? FW_BOLD : FW_REGULAR,
                font.Italic ? 1 : 0,
                font.Underline ? 1 : 0,
                0,
                GB2312_CHARSET,
                OUT_DEFAULT_PRECIS,
                CLIP_DEFAULT_PRECIS,
                DEFAULT_QUALITY,
                DEFAULT_PITCH | FF_SWISS,
                font.FontFamily
            );
            IntPtr hOldFont = SelectObject(m_hDC, hFont);
            GetTextExtentPoint32(m_hDC, text, text.Length, ref size);
            SelectObject(m_hDC, hOldFont);
            DeleteObject(hFont);
            return size;
        }

        /// <summary>
        /// 设置裁剪区域
        /// </summary>
        /// <param name="rect">区域</param>
        public void SetClip(RECT rect)
        {
            if (m_hrgn != IntPtr.Zero)
            {
                DeleteObject(m_hrgn);
            }
            RECT newRect = new RECT(rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY);
            m_hrgn = CreateRectRgnIndirect(ref newRect);
            SelectClipRgn(m_hDC, m_hrgn);
        }
    }
}