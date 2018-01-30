using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace mPaint
{
    /// <summary>
    /// 标记的属性
    /// </summary>
    public class SignalSeries
    {
        public SignalSeries(double value, SignalType st, Color stColor, bool canDrag)
        {
            this.value = value;
            this.signal = st;
            this.signalColor = stColor;
            this.canDrag = canDrag;
        }

        /// <summary>
        /// 标记的类型
        /// </summary>
        private SignalType signal = SignalType.UpArrow;

        public SignalType Signal
        {
            get { return signal; }
            set { signal = value; }
        }

        /// <summary>
        /// 标记的值
        /// </summary>
        private double value;

        public double Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        private bool canDrag = false;

        public bool CanDrag
        {
            get { return canDrag; }
            set { canDrag = value; }
        }

        /// <summary>
        /// 标记的颜色
        /// </summary>
        private Color signalColor = Color.Red;

        public Color SignalColor
        {
            get { return signalColor; }
            set { signalColor = value; }
        }

        /// <summary>
        /// 根据类型获取GraphicsPath.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public GraphicsPath GetGPByType(float x, float y, int width)
        {
            if (width > 10)
            {
                width = 14;
            }
            GraphicsPath gp = new GraphicsPath();
            switch (signal)
            {
                case SignalType.UpArrow:
                    gp.AddLine(x, y, x + width / 2, y + width);
                    gp.AddLine(x + width / 2, y + width, x + width / 4, y + width);
                    gp.AddLine(x + width / 4, y + width, x + width / 4, y + width * 3 / 2);
                    gp.AddLine(x + width / 4, y + width * 3 / 2, x - width / 4, y + width * 3 / 2);
                    gp.AddLine(x - width / 4, y + width * 3 / 2, x - width / 4, y + width);
                    gp.AddLine(x - width / 4, y + width, x - width / 2, y + width);
                    gp.AddLine(x - width / 2, y + width, x, y);
                    gp.CloseFigure();
                    break;
                case SignalType.DownArrow:
                    gp.AddLine(x, y, x + width / 2, y - width);
                    gp.AddLine(x + width / 2, y - width, x + width / 4, y - width);
                    gp.AddLine(x + width / 4, y - width, x + width / 4, y - width * 3 / 2);
                    gp.AddLine(x + width / 4, y - width * 3 / 2, x - width / 4, y - width * 3 / 2);
                    gp.AddLine(x - width / 4, y - width * 3 / 2, x - width / 4, y - width);
                    gp.AddLine(x - width / 4, y - width, x - width / 2, y - width);
                    gp.AddLine(x - width / 2, y - width, x, y);
                    gp.CloseFigure();
                    break;
                case SignalType.UpArrowWithOutTail:
                    gp.AddLine(x, y, x + width / 2, y + width);
                    gp.AddLine(x + width / 2, y + width, x - width / 2, y + width);
                    gp.AddLine(x - width / 2, y + width, x, y);
                    gp.CloseFigure();
                    break;
                case SignalType.DownArrowWithOutTail:
                    gp.AddLine(x, y, x + width / 2, y - width);
                    gp.AddLine(x + width / 2, y - width, x - width / 2, y - width);
                    gp.AddLine(x - width / 2, y - width, x, y);
                    gp.CloseFigure();
                    break;
                case SignalType.LeftArrow:
                    gp.AddLine(x + width / 2, y, x - width / 2, y - width / 2);
                    gp.AddLine(x - width / 2, y - width / 2, x - width / 2, y + width / 2);
                    gp.AddLine(x - width / 2, y + width / 2, x + width / 2, y);
                    gp.CloseFigure();
                    break;
                case SignalType.RightArrow:
                    gp.AddLine(x - width / 2, y, x + width / 2, y - width / 2);
                    gp.AddLine(x + width / 2, y - width / 2, x + width / 2, y + width / 2);
                    gp.AddLine(x + width / 2, y + width / 2, x - width / 2, y);
                    gp.CloseFigure();
                    break;
            }
            return gp;
        }
    }

    /// <summary>
    /// 线条的属性
    /// </summary>
    public class TrendLineSeries
    {
        public TrendLineSeries()
        {
        }

        /// <summary>
        /// 析构函数,释放对象
        /// </summary>
        ~TrendLineSeries()
        {
            if (this.up_linePen != null)
            {
                this.up_linePen.Dispose();
            }
            if (this.up_lineBrush != null)
            {
                this.up_lineBrush.Dispose();
            }
            if (this.transParentLineBrush != null)
            {
                this.transParentLineBrush.Dispose();
            }
            if (this.up_linePen != null)
            {
                this.up_linePen.Dispose();
            }
            if (this.down_lineBrush != null)
            {
                this.down_lineBrush.Dispose();
            }
        }

        /// <summary>
        /// 是否被选中
        /// </summary>
        private bool hasSelect = false;

        public bool HasSelect
        {
            get { return hasSelect; }
            set { hasSelect = value; }
        }

        /// <summary>
        /// 字段
        /// </summary>
        private string field = string.Empty;

        public string Field
        {
            get { return field; }
            set { field = value; }
        }

        /// <summary>
        /// 如果设置了该字段,则标题显示时使用该字段
        /// </summary>
        private string displayName = string.Empty;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        /// <summary>
        /// 保存的画笔
        /// </summary>
        private Pen up_linePen;

        public Pen Up_LinePen
        {
            get { return up_linePen; }
            set { up_linePen = value; }
        }

        /// <summary>
        /// 保存的画刷
        /// </summary>
        private Brush up_lineBrush;

        public Brush Up_LineBrush
        {
            get { return up_lineBrush; }
            set { up_lineBrush = value; }
        }

        /// <summary>
        /// 线的颜色
        /// </summary>
        private Color up_lineColor;

        public Color Up_LineColor
        {
            get { return up_lineColor; }
            set
            {
                up_lineColor = value;
                if (up_linePen != null)
                {
                    up_linePen.Dispose();
                }
                up_linePen = new Pen(value);
                if (up_lineBrush != null)
                {
                    up_lineBrush.Dispose();
                }
                up_lineBrush = new SolidBrush(value);
                if (transParentLineBrush != null)
                {
                    transParentLineBrush.Dispose();
                }
                transParentLineBrush = new SolidBrush(Color.FromArgb(100, value));
            }
        }

        /// <summary>
        /// 阴线的画笔
        /// </summary>
        private Pen down_linePen = new Pen(Color.Yellow);

        public Pen Down_linePen
        {
            get { return down_linePen; }
            set { down_linePen = value; }
        }

        /// <summary>
        /// 阴线的笔刷
        /// </summary>
        private Brush down_lineBrush;

        public Brush Down_lineBrush
        {
            get { return down_lineBrush; }
            set { down_lineBrush = value; }
        }

        /// <summary>
        /// 阴线的颜色
        /// </summary>
        private Color down_LineColor;

        public Color Down_LineColor
        {
            get { return down_LineColor; }
            set
            {
                down_LineColor = value;
                if (down_linePen != null)
                {
                    down_linePen.Dispose();
                }
                down_linePen = new Pen(value);
                if (down_lineBrush != null)
                {
                    down_lineBrush.Dispose();
                }
                down_lineBrush = new SolidBrush(value);
            }
        }

        /// <summary>
        /// 透明色画刷
        /// </summary>
        private Brush transParentLineBrush;

        public Brush TransParentLineBrush
        {
            get { return transParentLineBrush; }
            set { transParentLineBrush = value; }
        }
    }

    /// <summary>
    /// 柱状图的属性
    /// </summary>
    public class HistogramSeries
    {
        public HistogramSeries()
        {
        }

        /// <summary>
        /// 析构函数,释放对象
        /// </summary>
        ~HistogramSeries()
        {
            if (up_lineBrush != null)
            {
                up_lineBrush.Dispose();
            }
            if (down_lineBrush != null)
            {
                down_lineBrush.Dispose();
            }
            if (up_TransparentBrush != null)
            {
                up_TransparentBrush.Dispose();
            }
            if (up_Pen != null)
            {
                up_Pen.Dispose();
            }
            if (down_Pen != null)
            {
                down_Pen.Dispose();
            }
        }

        /// <summary>
        /// 是否被选中
        /// </summary>
        private bool hasSelect = false;

        public bool HasSelect
        {
            get { return hasSelect; }
            set { hasSelect = value; }
        }

        /// <summary>
        /// 字段名
        /// </summary>
        private string field;

        public string Field
        {
            get { return field; }
            set { field = value; }
        }

        /// <summary>
        /// 对应的K线名,设置后可以显示阴阳线
        /// </summary>
        private string relateCandleName;

        public string RelateCandleName
        {
            get { return relateCandleName; }
            set { relateCandleName = value; }
        }

        /// <summary>
        /// 透明的阳线画刷
        /// </summary>
        private Brush up_TransparentBrush;

        public Brush Up_TransparentBrush
        {
            get { return up_TransparentBrush; }
            set { up_TransparentBrush = value; }
        }

        /// <summary>
        /// 保存的阳线画笔
        /// </summary>
        private Pen up_Pen;

        public Pen Up_Pen
        {
            get { return up_Pen; }
            set { up_Pen = value; }
        }

        /// <summary>
        /// 保存的阴线画笔
        /// </summary>
        private Pen down_Pen;

        public Pen Down_Pen
        {
            get { return down_Pen; }
            set { down_Pen = value; }
        }

        /// <summary>
        /// 保存的阳线画刷
        /// </summary>
        private Brush up_lineBrush;

        public Brush Up_LineBrush
        {
            get { return up_lineBrush; }
            set { up_lineBrush = value; }
        }

        /// <summary>
        /// 保存的阴线画刷
        /// </summary>
        private Brush down_lineBrush;

        public Brush Down_lineBrush
        {
            get { return down_lineBrush; }
            set { down_lineBrush = value; }
        }

        /// <summary>
        /// 阳线的颜色
        /// </summary>
        private Color up_lineColor;

        public Color Up_LineColor
        {
            get { return up_lineColor; }
            set
            {
                up_lineColor = value;
                if (up_lineBrush != null)
                {
                    up_lineBrush.Dispose();
                }
                up_lineBrush = new SolidBrush(value);
                if (up_TransparentBrush != null)
                {
                    up_TransparentBrush.Dispose();
                }
                up_TransparentBrush = new SolidBrush(Color.FromArgb(100, value));
                if (up_Pen != null)
                {
                    up_Pen.Dispose();
                }
                up_Pen = new Pen(value);
            }
        }

        /// <summary>
        /// 阴线的颜色
        /// </summary>
        private Color down_lineColor;

        public Color Down_lineColor
        {
            get { return down_lineColor; }
            set
            {
                down_lineColor = value;
                if (down_lineBrush != null)
                {
                    down_lineBrush.Dispose();
                }
                Down_lineBrush = new SolidBrush(value);
                if (down_Pen != null)
                {
                    down_Pen.Dispose();
                }
                down_Pen = new Pen(value);
            }
        }

        /// <summary>
        /// 显示的标题名称
        /// </summary>
        private string displayName = string.Empty;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        /// <summary>
        /// 使用线的样式
        /// </summary>
        private bool lineStyle = false;

        public bool LineStyle
        {
            get { return lineStyle; }
            set { lineStyle = value; }
        }

        /// <summary>
        /// 线的宽度
        /// </summary>
        private int lineWidth = 1;

        public int LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }
    }

    /// <summary>
    /// K线柱的属性
    /// </summary>
    public class CandleSeries
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CandleSeries()
        {
        }

        /// <summary>
        /// 析构函数,释放对象
        /// </summary>
        ~CandleSeries()
        {
            if (this.upLine_Brush != null) upLine_Brush.Dispose();
            if (this.downLine_Brush != null) downLine_Brush.Dispose();
            if (this.middleLine_Pen != null) middleLine_Pen.Dispose();
            if (this.upLine_Pen != null) upLine_Pen.Dispose();
            if (this.upLine_TransparentBrush != null) upLine_TransparentBrush.Dispose();
            if (this.downLine_Pen != null) downLine_Pen.Dispose();
        }

        /// <summary>
        /// 买卖标识的字体
        /// </summary>
        private Font bsFont = new Font("宋体", 12, FontStyle.Bold);

        public Font BsFont
        {
            get { return bsFont; }
            set { bsFont = value; }
        }

        /// <summary>
        /// 买点标识色
        /// </summary>
        private Color buyColor = Color.Red;

        public Color BuyColor
        {
            get { return buyColor; }
            set { buyColor = value; }
        }

        /// <summary>
        /// 卖点标识色
        /// </summary>
        private Color sellColor = Color.SkyBlue;

        public Color SellColor
        {
            get { return sellColor; }
            set { sellColor = value; }
        }

        /// <summary>
        /// 买点的文字
        /// </summary>
        private string buyText = "B";

        public string BuyText
        {
            get { return buyText; }
            set { buyText = value; }
        }

        /// <summary>
        /// 卖点的文字
        /// </summary>
        private string sellText = "S";

        public string SellText
        {
            get { return sellText; }
            set { sellText = value; }
        }

        /// <summary>
        /// K线的名称
        /// </summary>
        private string candleName;

        public string CandleName
        {
            get { return candleName; }
            set { candleName = value; }
        }

        /// <summary>
        /// 买点对应的字段
        /// </summary>
        private string[] indBuySellField = new string[2];

        public string[] IndBuySellField
        {
            get { return indBuySellField; }
            set { indBuySellField = value; }
        }

        /// <summary>
        /// 最高价字段
        /// </summary>
        private string highField;

        public string HighField
        {
            get { return highField; }
            set { highField = value; }
        }

        /// <summary>
        /// 开盘价字段
        /// </summary>
        private string openField;

        public string OpenField
        {
            get { return openField; }
            set { openField = value; }
        }

        /// <summary>
        /// 收盘价字段
        /// </summary>
        private string closeField;

        public string CloseField
        {
            get { return closeField; }
            set { closeField = value; }
        }

        /// <summary>
        /// 最低价字段
        /// </summary>
        private string lowField;

        public string LowField
        {
            get { return lowField; }
            set { lowField = value; }
        }

        /// <summary>
        /// 是否被选中
        /// </summary>
        private bool hasSelect = false;

        public bool HasSelect
        {
            get { return hasSelect; }
            set { hasSelect = value; }
        }

        /// <summary>
        /// K线最大值对应的记录号
        /// </summary>
        private int maxRecord = 0;

        public int MaxRecord
        {
            get { return maxRecord; }
            set { maxRecord = value; }
        }

        /// <summary>
        ///  K线最小值对应的记录号
        /// </summary>
        private int minRecord = 0;

        public int MinRecord
        {
            get { return minRecord; }
            set { minRecord = value; }
        }

        /// <summary>
        /// 阳线刷
        /// </summary>
        private Brush upLine_Brush;

        public Brush UpLine_Brush
        {
            get { return upLine_Brush; }
            set { upLine_Brush = value; }
        }

        /// <summary>
        /// 阴线刷
        /// </summary>
        private Brush downLine_Brush;

        public Brush DownLine_Brush
        {
            get { return downLine_Brush; }
            set { downLine_Brush = value; }
        }

        /// <summary>
        /// 中线的画笔
        /// </summary>
        private Pen middleLine_Pen;

        public Pen MiddleLine_Pen
        {
            get { return middleLine_Pen; }
            set { middleLine_Pen = value; }
        }

        /// <summary>
        /// 阳线笔
        /// </summary>
        private Pen upLine_Pen;

        public Pen UpLine_Pen
        {
            get { return upLine_Pen; }
            set { upLine_Pen = value; }
        }

        /// <summary>
        /// 阴线笔
        /// </summary>
        private Pen downLine_Pen;

        public Pen DownLine_Pen
        {
            get { return downLine_Pen; }
            set { downLine_Pen = value; }
        }

        /// <summary>
        /// 阳线透明刷
        /// </summary>
        private Brush upLine_TransparentBrush;

        public Brush UpLine_TransparentBrush
        {
            get { return upLine_TransparentBrush; }
            set { upLine_TransparentBrush = value; }
        }

        /// <summary>
        /// 阳线颜色
        /// </summary>
        private Color up_Color;

        public Color Up_Color
        {
            get { return up_Color; }
            set
            {
                up_Color = value;
                if (upLine_Brush != null)
                {
                    upLine_Brush.Dispose();
                }
                upLine_Brush = new SolidBrush(value);
                if (upLine_Pen != null)
                {
                    upLine_Pen.Dispose();
                }
                upLine_Pen = new Pen(value);
                if (upLine_TransparentBrush != null)
                {
                    upLine_TransparentBrush.Dispose();
                }
                upLine_TransparentBrush = new SolidBrush(Color.FromArgb(100, value));
            }
        }

        /// <summary>
        /// 中线的颜色
        /// </summary>
        private Color middle_Color = Color.White;

        public Color Middle_Color
        {
            get { return middle_Color; }
            set
            {
                middle_Color = value;
                if (middleLine_Pen != null)
                {
                    middleLine_Pen.Dispose();
                }
                middleLine_Pen = null;
                if (value != Color.Empty)
                {
                    middleLine_Pen = new Pen(value);
                }
            }
        }

        /// <summary>
        /// 阴线的颜色
        /// </summary>
        private Color down_Color;

        public Color Down_Color
        {
            get { return down_Color; }
            set
            {
                down_Color = value;
                if (downLine_Brush != null)
                {
                    downLine_Brush.Dispose();
                }
                downLine_Brush = new SolidBrush(value);
                if (downLine_Pen != null)
                {
                    downLine_Pen.Dispose();
                }
                downLine_Pen = new Pen(value);
            }
        }

        /// <summary>
        /// 开盘价的标题色
        /// </summary>
        private Color openTitleColor = Color.White;

        public Color OpenTitleColor
        {
            get { return openTitleColor; }
            set { openTitleColor = value; }
        }

        /// <summary>
        /// 最高价的标题色
        /// </summary>
        private Color highTitleColor = Color.Red;

        public Color HighTitleColor
        {
            get { return highTitleColor; }
            set { highTitleColor = value; }
        }

        /// <summary>
        /// 最低价的标题色
        /// </summary>
        private Color lowTitleColor = Color.Orange;

        public Color LowTitleColor
        {
            get { return lowTitleColor; }
            set { lowTitleColor = value; }
        }

        /// <summary>
        /// 收盘价的标题色
        /// </summary>
        private Color closeTitleColor = Color.Yellow;

        public Color CloseTitleColor
        {
            get { return closeTitleColor; }
            set { closeTitleColor = value; }
        }

        /// <summary>
        /// 是否在标题上显示字段
        /// </summary>
        private bool displayTitleField = true;

        public bool DisplayTitleField
        {
            get { return displayTitleField; }
            set { displayTitleField = value; }
        }
    }
}
