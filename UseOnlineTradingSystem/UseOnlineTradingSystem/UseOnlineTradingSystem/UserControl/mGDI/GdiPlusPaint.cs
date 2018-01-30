using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System;
using System.Drawing.Text;
namespace mPaint
{
    /// <summary>
    /// Gdi+绘图类
    /// </summary>
    public class GdiPlusPaint:MPaint
    {
        /// <summary>
        /// 缓冲区
        /// </summary>
        private BufferedGraphics m_buffer;

        /// <summary>
        /// 缓冲区上下文
        /// </summary>
        private BufferedGraphicsContext m_currentContext = BufferedGraphicsManager.Current;

        /// <summary>
        /// 空的字符串格式
        /// </summary>
        private StringFormat m_emptyStringFormat;

        /// <summary>
        /// 绘图画面
        /// </summary>
        private Graphics m_g;

        /// <summary>
        /// 绘图画面
        /// </summary>
        private Graphics m_graphics;

        /// <summary>
        /// 横向偏移
        /// </summary>
        private int m_offsetX;

        /// <summary>
        /// 纵向偏移
        /// </summary>
        private int m_offsetY;

        /// <summary>
        /// 开始绘图
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="wRect">窗体区域</param>
        /// <param name="pRect">刷新区域</param>
        public void BeginPaint(Graphics g, RECT wRect, RECT pRect)
        {
            if (m_buffer != null)
            {
                m_g.Dispose();
                m_buffer.Dispose();
                m_buffer = null;
                m_g = null;
            }
            m_graphics = g;
            m_buffer = m_currentContext.Allocate(m_graphics, new Rectangle(pRect.left, pRect.top, pRect.right - pRect.left, pRect.bottom - pRect.top));
            m_g = m_buffer.Graphics;
            m_g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            if(m_currentContext!=null)
            {
                m_currentContext.Dispose();
                m_currentContext = null;
            }
            if (m_emptyStringFormat != null)
            {
                m_emptyStringFormat.Dispose();
                m_emptyStringFormat = null;
            }
            if (m_buffer != null)
            {
                m_buffer.Dispose();
                m_buffer = null;
            }
            if (m_graphics != null)
            {
                m_graphics.Dispose();
                m_graphics = null;
            }
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
            if (dwPenColor == COLOR.Empty()) return;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rect.right - rect.left, rect.bottom - rect.top);
            m_g.DrawEllipse(MCommon.Instance.GetPen(dwPenColor, width, style), gdiPlusRect);
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
            RECT rect = new RECT(left, top, right, bottom);
            DrawEllipse(dwPenColor, width, style, rect);
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
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rect.right - rect.left, rect.bottom - rect.top);
            LinearGradientBrush lgb = new LinearGradientBrush(gdiPlusRect, MCommon.Instance.GetColor(beginColor), MCommon.Instance.GetColor(endColor), angle);
            m_g.FillRectangle(lgb, gdiPlusRect);
            lgb.Dispose();
        }


        /// <summary>
        /// 绘制图片
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="rect">绘制区域</param>
        public void DrawImage(string imagePath, RECT rect)
        {
            Image image = Image.FromFile(imagePath);
            DrawImage(image, rect);
        }
  
        /// <summary>
        /// 绘制图片
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="rect">绘制区域</param>
        public void DrawImage(Image image, RECT rect)
        {
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rect.right - rect.left, rect.bottom - rect.top);
            m_g.DrawImage(image, gdiPlusRect);
            //image.Dispose();
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
            m_g.DrawLine(MCommon.Instance.GetPen(dwPenColor, width, style), x1 + m_offsetX, y1 + m_offsetY, x2 + m_offsetX, y2 + m_offsetY);
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
            Point[] gdiPlusPoints = new Point[points.Length];
            for (int i = 0; i < gdiPlusPoints.Length; i++)
            {
                Point p = new Point(points[i].x + m_offsetX, points[i].y + m_offsetY);
                gdiPlusPoints[i] = p;
            }
            m_g.DrawPolygon(MCommon.Instance.GetPen(dwPenColor, width, style), gdiPlusPoints);
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
            Point[] gdiPlusPoints = new Point[points.Length];
            for (int i = 0; i < gdiPlusPoints.Length; i++)
            {
                Point p = new Point(points[i].x + m_offsetX, points[i].y + m_offsetY);
                gdiPlusPoints[i] = p;
            }
            m_g.DrawLines(MCommon.Instance.GetPen(dwPenColor, width, style), gdiPlusPoints);
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
            if (dwPenColor == COLOR.Empty()) return;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rect.right - rect.left, rect.bottom - rect.top);
            m_g.DrawRectangle(MCommon.Instance.GetPen(dwPenColor, width, style), gdiPlusRect);
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
            RECT rect = new RECT(left, top, right, bottom);
            DrawRect(dwPenColor, width, style, rect);
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
            if (m_emptyStringFormat == null)
            {
                m_emptyStringFormat = StringFormat.GenericTypographic;
            }
            m_g.DrawString(text, MCommon.Instance.GetFont(font), MCommon.Instance.GetBrush(dwPenColor), mp.x + m_offsetX, mp.y + m_offsetY, m_emptyStringFormat);
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
            if (m_emptyStringFormat == null)
            {
                m_emptyStringFormat = StringFormat.GenericTypographic;
            }
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rect.right - rect.left, rect.bottom - rect.top);
            m_g.DrawString(text, MCommon.Instance.GetFont(font), MCommon.Instance.GetBrush(dwPenColor), gdiPlusRect, m_emptyStringFormat);
        }

        /// <summary>
        /// 结束绘图
        /// </summary>
        public void EndPaint()
        {
            m_buffer.Render();
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
            m_g.FillEllipse(MCommon.Instance.GetBrush(dwPenColor), left + m_offsetX, top + m_offsetY, right - left, bottom - top);
        }

        /// <summary>
        /// 填充多边形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="points">点的数组</param>
        public void FillPolygon(double dwPenColor, POINT[] points)
        {
            if (dwPenColor == COLOR.Empty()) return;
            Point[] gdiPlusPoints = new Point[points.Length];
            for (int i = 0; i < gdiPlusPoints.Length; i++)
            {
                Point p = new Point(points[i].x + m_offsetX, points[i].y + m_offsetY);
                gdiPlusPoints[i] = p;
            }
            m_g.FillPolygon(MCommon.Instance.GetBrush(dwPenColor), gdiPlusPoints);
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="rect">矩形区域</param>
        public void FillRect(double dwPenColor, RECT rect)
        {
            FillRect(dwPenColor, rect.left, rect.top, rect.right, rect.bottom);
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
            if (dwPenColor == COLOR.Empty()) return;
            m_g.FillRectangle(MCommon.Instance.GetBrush(dwPenColor), left + m_offsetX, top + m_offsetY, right - left, bottom - top);
        }

        /// <summary> 
        /// 填充圆角矩形 
        /// </summary> 
        /// <param name="dwPenColor">要填充的背景色</param> 
        /// <param name="rect">要填充的矩形</param> 
        /// <param name="r">圆角半径</param> 
        public void DrawFillRoundRectangle(double dwPenColor, RECT rect, int r)
        {
            if (dwPenColor == COLOR.Empty()) return;
            Rectangle rectangle = new Rectangle(rect.left, rect.top, rect.right-rect.left - 1, rect.bottom- rect.top - 1);
            m_g.FillPath(MCommon.Instance.GetBrush(dwPenColor), GetRoundRectangle(rectangle, r));
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
        /// 根据普通矩形得到圆角矩形的路径 
        /// </summary> 
        /// <param name="rectangle">原始矩形</param> 
        /// <param name="r">半径</param> 
        /// <returns>图形路径</returns> 
        private GraphicsPath GetRoundRectangle(Rectangle rectangle, int r)
        {
            int l = 2 * r;
            // 把圆角矩形分成八段直线、弧的组合，依次加到路径中 
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(new Point(rectangle.X + r, rectangle.Y), new Point(rectangle.Right - r, rectangle.Y));
            gp.AddArc(new Rectangle(rectangle.Right - l, rectangle.Y, l, l), 270F, 90F);

            gp.AddLine(new Point(rectangle.Right, rectangle.Y + r), new Point(rectangle.Right, rectangle.Bottom - r));
            gp.AddArc(new Rectangle(rectangle.Right - l, rectangle.Bottom - l, l, l), 0F, 90F);

            gp.AddLine(new Point(rectangle.Right - r, rectangle.Bottom), new Point(rectangle.X + r, rectangle.Bottom));
            gp.AddArc(new Rectangle(rectangle.X, rectangle.Bottom - l, l, l), 90F, 90F);

            gp.AddLine(new Point(rectangle.X, rectangle.Bottom - r), new Point(rectangle.X, rectangle.Y + r));
            gp.AddArc(new Rectangle(rectangle.X, rectangle.Y, l, l), 180F, 90F);
            return gp;
        }

        /// <summary>
        /// 设置裁剪区域
        /// </summary>
        /// <param name="rect">区域</param>
        public void SetClip(RECT rect)
        {
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rect.right - rect.left, rect.bottom - rect.top);
            m_g.SetClip(gdiPlusRect);
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
            if (m_emptyStringFormat == null)
            {
                m_emptyStringFormat = StringFormat.GenericTypographic;
            }
            Size gdiPlusSize = m_g.MeasureString(text, MCommon.Instance.GetFont(font), Int32.MaxValue, m_emptyStringFormat).ToSize();
            return new SIZE(gdiPlusSize.Width, gdiPlusSize.Height);
        }
    }
}