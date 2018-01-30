using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace mPaint
{
    public sealed class MCommon
    {
        private Dictionary<string, Font> dy = new Dictionary<string, Font>();
        /// <summary>
        /// 画刷的颜色
        /// </summary>
        private double m_brushColor;

        /// <summary>
        ///  画笔的颜色 
        /// </summary>
        private double m_penColor;

        /// <summary>
        /// 画笔的宽度 
        /// </summary>
        private int m_penWidth;

        /// <summary>
        ///  画笔的样式 
        /// </summary>
        private int m_penStyle;

        private static readonly MCommon instance = new MCommon();
        /// <summary>
        /// 画刷
        /// </summary>
        private SolidBrush m_brush;

        /// <summary>
        /// 画笔
        /// </summary>
        private Pen m_pen;

        static MCommon() { }

        private MCommon() {
        }

        public static MCommon Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 颜色缓存
        /// </summary>
        private Dictionary<double, Color> m_colors = new Dictionary<double, Color>();

        /// <summary>
        /// 获取画刷
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>画刷</returns>
        public  SolidBrush GetBrush(double dwPenColor)
        {
            Color gdiColor = GetColor(dwPenColor);
            if (m_brush == null)
            {
                m_brush = new SolidBrush(gdiColor);
                m_brushColor = dwPenColor;
            }
            else
            {
                if (m_brushColor != dwPenColor)
                {
                    m_brush.Color = gdiColor;
                    m_brushColor = dwPenColor;
                }
            }
            return m_brush;
        }

        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="color">整型颜色</param>
        /// <returns>Gdi颜色</returns>
        public Color GetColor(double dwPenColor)
        {
            if (MCommon.Instance.m_colors.ContainsKey(dwPenColor))
            {
                return MCommon.Instance.m_colors[dwPenColor];
            }
            else
            {
                Color gdiColor = Color.Empty;
                if (dwPenColor >= 0)
                {
                    gdiColor = ColorTranslator.FromWin32((int)dwPenColor);
                }
                else
                {
                    double absDwPenColor = Math.Abs(dwPenColor);
                    gdiColor = ColorTranslator.FromWin32((int)absDwPenColor);
                    int alpha = (int)(absDwPenColor * 1000 % (int)(absDwPenColor));
                    if (alpha < 255)
                    {
                        gdiColor = Color.FromArgb(alpha, gdiColor);
                    }
                }
                MCommon.Instance.m_colors[dwPenColor] = gdiColor;
                return gdiColor;
            }
        }

        /// <summary>
        /// 获取Gdi字体
        /// </summary>
        /// <param name="font">字体</param>
        /// <returns>Gdi字体</returns>
        public Font GetFont(FONT font)
        {
            string key = font.ToString();
            if (!dy.ContainsKey(key))
            {
                Font f ;
                if (font.Bold && font.Underline && font.Italic)
                {
                    f= new Font(font.FontFamily, font.FontSize, FontStyle.Bold | FontStyle.Underline | FontStyle.Italic, GraphicsUnit.Pixel);
                }
                else if (font.Bold && font.Underline)
                {
                    f= new Font(font.FontFamily, font.FontSize, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
                }
                else if (font.Bold && font.Italic)
                {
                    f= new Font(font.FontFamily, font.FontSize, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Pixel);
                }
                else if (font.Underline && font.Italic)
                {
                    f= new Font(font.FontFamily, font.FontSize, FontStyle.Underline | FontStyle.Italic, GraphicsUnit.Pixel);
                }
                else if (font.Bold)
                {
                    f= new Font(font.FontFamily, font.FontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                }
                else if (font.Underline)
                {
                    f= new Font(font.FontFamily, font.FontSize, FontStyle.Underline, GraphicsUnit.Pixel);
                }
                else if (font.Italic)
                {
                    f= new Font(font.FontFamily, font.FontSize, FontStyle.Italic, GraphicsUnit.Pixel);
                }
                else
                {
                    f= new Font(font.FontFamily, font.FontSize, GraphicsUnit.Pixel);
                }
                dy.Add(key, f);
            }
            return dy[key];
        }

        /// <summary>
        /// 获取画笔
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">宽度</param>
        /// <param name="lineStyle">样式</param>
        /// <returns>画笔</returns>
        public Pen GetPen(double dwPenColor, int width, int style)
        {
            Color gdiColor = GetColor(dwPenColor);
            if (m_pen == null)
            {
                m_pen = new Pen(gdiColor, width);
                if (style == 0)
                {
                    m_pen.DashStyle = DashStyle.Solid;
                }
                else if (style == 1)
                {
                    m_pen.DashStyle = DashStyle.Dash;
                }
                else if (style == 2)
                {
                    m_pen.DashStyle = DashStyle.Dot;
                }
                m_penColor = dwPenColor;
                m_penWidth = width;
                m_penStyle = style;
            }
            else
            {
                if (m_penColor != dwPenColor)
                {
                    m_pen.Color = gdiColor;
                    m_penColor = dwPenColor;
                }
                if (m_penWidth != width)
                {
                    m_pen.Width = width;
                    m_penWidth = width;
                }
                if (m_penStyle != style)
                {
                    if (style == 0)
                    {
                        m_pen.DashStyle = DashStyle.Solid;
                    }
                    else if (style == 1)
                    {
                        m_pen.DashStyle = DashStyle.Dash;
                    }
                    else if (style == 2)
                    {
                        m_pen.DashStyle = DashStyle.Dot;
                    }
                    m_penStyle = style;
                }
            }
            return m_pen;
        }

    }
}
