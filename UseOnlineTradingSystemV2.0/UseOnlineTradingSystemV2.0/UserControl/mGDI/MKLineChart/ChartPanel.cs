using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace mPaint
{
    /// <summary>
    /// 面板属性设置
    /// </summary>
    public class ChartPanel
    {
        public ChartPanel()
        {
            infoBombField = CommonClass.GetGuid();
            panelBorder_Pen.DashStyle = DashStyle.Dash;
            grid_Pen.DashStyle = DashStyle.Dash;
        }

        ~ChartPanel()
        {
            if (bgBrush != null) { bgBrush.Dispose(); }
            if (xtip_Brush != null) { xtip_Brush.Dispose(); }
            if (xTipFont_Brush != null) { xTipFont_Brush.Dispose(); }
            if (xTipFont_Pen != null) { xTipFont_Pen.Dispose(); }
            if (leftyTip_Brush != null) { leftyTip_Brush.Dispose(); }
            if (leftyTipFont_Brush != null) { leftyTipFont_Brush.Dispose(); }
            if (leftTipFont_Pen != null) { leftTipFont_Pen.Dispose(); }
            if (rightyTip_Brush != null) { rightyTip_Brush.Dispose(); }
            if (rightyTipFont_Brush != null) { rightyTipFont_Brush.Dispose(); }
            if (panelBorder_Pen != null) { panelBorder_Pen.Dispose(); }
            if (titleFont_Brush != null) { titleFont_Brush.Dispose(); }
            if (grid_Pen != null) { grid_Pen.Dispose(); }
            if (coordinateXFont_Brush != null) { coordinateXFont_Brush.Dispose(); }
            if (leftYFont_Brush != null) { leftYFont_Brush.Dispose(); }
            if (rightYFont_Brush != null) { rightYFont_Brush.Dispose(); }
            if (rightyTipFont_Pen != null) { rightyTipFont_Pen.Dispose(); }
            if (xScalePen != null) { xScalePen.Dispose(); }
            if (leftScalePen != null) { leftScalePen.Dispose(); }
            if (rightScalePen != null) { rightScalePen.Dispose(); }
            if (crossHair_Pen != null) { crossHair_Pen.Dispose(); }
        }

        /// <summary>
        /// 十字线的画笔
        /// </summary>
        private Pen crossHair_Pen = new Pen(Color.White);

        public Pen CrossHair_Pen
        {
            get { return crossHair_Pen; }
            set { crossHair_Pen = value; }
        }

        /// <summary>
        /// 网格线的幅度
        /// </summary>
        private int gridInterval = 3;

        public int GridInterval
        {
            get { return gridInterval; }
            set { gridInterval = value; }
        }

        /// <summary>
        /// 信息地雷的字段
        /// </summary>
        private string infoBombField = string.Empty;

        public string InfoBombField
        {
            get { return infoBombField; }
            set { infoBombField = value; }
        }

        /// <summary>
        /// 信息地雷的颜色
        /// </summary>
        private Color infoBombColor = Color.White;

        public Color InfoBombColor
        {
            get { return infoBombColor; }
            set { infoBombColor = value; }
        }

        /// <summary>
        /// 信息地雷的选中色
        /// </summary>
        private Color infoBombSelectedColor = Color.FromArgb(255, 255, 153);

        public Color InfoBombSelectedColor
        {
            get { return infoBombSelectedColor; }
            set { infoBombSelectedColor = value; }
        }

        /// <summary>
        /// 信息地雷提示框的背景色
        /// </summary>
        private Color infoBombTipColor = Color.FromArgb(255, 255, 153);

        public Color InfoBombTipColor
        {
            get { return infoBombTipColor; }
            set { infoBombTipColor = value; }
        }

        /// <summary>
        /// 信息地雷提示框的文字颜色
        /// </summary>
        private Color infoBombTipTextColor = Color.Black;

        public Color InfoBombTipTextColor
        {
            get { return infoBombTipTextColor; }
            set { infoBombTipTextColor = value; }
        }

        /// <summary>
        /// 保存标记的集合
        /// </summary>
        private Dictionary<string, List<SignalSeries>> signalSeriesDic = new Dictionary<string, List<SignalSeries>>();

        public Dictionary<string, List<SignalSeries>> SignalSeriesDic
        {
            get { return signalSeriesDic; }
            set { signalSeriesDic = value; }
        }

        /// <summary>
        /// 绘制标题的集合
        /// </summary>
        private List<TitleField> titleFieldList = new List<TitleField>();

        public List<TitleField> TitleFieldList
        {
            get { return titleFieldList; }
            set { titleFieldList = value; }
        }

        /// <summary>
        /// 用户自定义标题
        /// </summary>
        private bool userDefinedTitle = false;

        public bool UserDefinedTitle
        {
            get { return userDefinedTitle; }
            set { userDefinedTitle = value; }
        }

        /// <summary>
        /// 面板显示数值保留小数的位数
        /// </summary>
        private int digit = 2;

        public int Digit
        {
            get { return digit; }
            set { digit = value; }
        }

        /// <summary>
        /// 面板的ID
        /// </summary>
        private int panelID;

        public int PanelID
        {
            get { return panelID; }
            set { panelID = value; }
        }

        /// <summary>
        /// 纵向比例
        /// </summary>
        private int verticalPercent = 0;

        public int VerticalPercent
        {
            get { return verticalPercent; }
            set { verticalPercent = value; }
        }

        /// <summary>
        /// Y轴的Tick
        /// </summary>
        private double yScaleTick = 0.01;

        public double YScaleTick
        {
            get { return yScaleTick; }
            set { yScaleTick = value; }
        }

        /// <summary>
        /// 卖点对应的字段
        /// </summary>
        private string sellSignalField = string.Empty;

        [Browsable(false)]
        public string SellSignalField
        {
            get { return sellSignalField; }
            set { sellSignalField = value; }
        }

        /// <summary>
        /// 面板的标题
        /// </summary>
        private string panelTitle = string.Empty;

        public string PanelTitle
        {
            get { return panelTitle; }
            set { panelTitle = value; }
        }

        /// <summary>
        /// 是否显示网格线
        /// </summary>
        private bool showGrid = true;

        public bool ShowGrid
        {
            get { return showGrid; }
            set { showGrid = value; }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        private double maxValue;

        [Browsable(false)]
        public double MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        /// <summary>
        /// 最小值
        /// </summary>
        private double minValue;

        [Browsable(false)]
        public double MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        /// <summary>
        /// 当前显示的线条的日期类型
        /// </summary>
        private IntervalType interval = IntervalType.Day;

        [Browsable(true)]
        public IntervalType Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        /// <summary>
        /// 面板的矩形
        /// </summary>
        private Rectangle rectPanel = new Rectangle();

        public Rectangle RectPanel
        {
            get { return rectPanel; }
            set { rectPanel = value; }
        }

        /// <summary>
        /// K线柱的集合
        /// </summary>
        private List<CandleSeries> candleSeriesList = new List<CandleSeries>();

        public List<CandleSeries> CandleSeriesList
        {
            get { return candleSeriesList; }
            set { candleSeriesList = value; }
        }

        /// <summary>
        /// 柱状图的集合
        /// </summary>
        private List<HistogramSeries> historamSeriesList = new List<HistogramSeries>();

        public List<HistogramSeries> HistoramSeriesList
        {
            get { return historamSeriesList; }
            set { historamSeriesList = value; }
        }

        /// <summary>
        /// 线条的集合
        /// </summary>
        private List<TrendLineSeries> trendLineSeriesList = new List<TrendLineSeries>();

        public List<TrendLineSeries> TrendLineSeriesList
        {
            get { return trendLineSeriesList; }
            set { trendLineSeriesList = value; }
        }

        /// <summary>
        /// 计算Y轴所使用的字段
        /// </summary>
        private List<string> yScaleField = new List<string>();

        public List<string> YScaleField
        {
            get { return yScaleField; }
            set { yScaleField = value; }
        }

        /// <summary>
        /// 背景色刷
        /// </summary>
        private Brush bgBrush = new SolidBrush(Color.Black);

        public Brush BgBrush
        {
            get { return bgBrush; }
            set { bgBrush = value; }
        }

        /// <summary>
        /// X轴的线条画笔
        /// </summary>
        private Pen xScalePen = new Pen(Color.Red);

        public Pen XScalePen
        {
            get { return xScalePen; }
            set { xScalePen = value; }
        }

        /// <summary>
        /// 左侧Y轴的画笔
        /// </summary>
        private Pen leftScalePen = new Pen(Color.Red);

        public Pen LeftScalePen
        {
            get { return leftScalePen; }
            set { leftScalePen = value; }
        }

        /// <summary>
        /// 右侧Y轴的画笔
        /// </summary>
        private Pen rightScalePen = new Pen(Color.Red);

        public Pen RightScalePen
        {
            get { return rightScalePen; }
            set { rightScalePen = value; }
        }

        /// <summary>
        /// X轴提示框背景色的刷子
        /// </summary>
        private Brush xtip_Brush = new SolidBrush(Color.FromArgb(100, Color.Red));

        public Brush Xtip_Brush
        {
            get { return xtip_Brush; }
            set { xtip_Brush = value; }
        }

        /// <summary>
        /// X轴提示框文子色的画笔
        /// </summary>
        private Pen xTipFont_Pen = new Pen(Color.White);

        public Pen XTipFont_Pen
        {
            get { return xTipFont_Pen; }
            set { xTipFont_Pen = value; }
        }

        /// <summary>
        /// X轴提示框文字色的刷子
        /// </summary>
        private Brush xTipFont_Brush = new SolidBrush(Color.White);

        public Brush XTipFont_Brush
        {
            get { return xTipFont_Brush; }
            set { xTipFont_Brush = value; }
        }

        /// <summary>
        /// X轴提示框文字的字体
        /// </summary>
        private Font xTipFont = new Font("New Times Roman", 10, FontStyle.Bold);

        public Font XTipFont
        {
            get { return xTipFont; }
            set { xTipFont = value; }
        }

        /// <summary>
        /// 左侧Y轴提示框背景色的刷子
        /// </summary>
        /// </summary>
        private Brush leftyTip_Brush = new SolidBrush(Color.FromArgb(100, Color.Red));

        public Brush LeftyTip_Brush
        {
            get { return leftyTip_Brush; }
            set { leftyTip_Brush = value; }
        }

        /// <summary>
        /// 左侧Y轴提示框文字色的画笔
        /// </summary>
        private Pen leftTipFont_Pen = new Pen(Color.White);

        public Pen LeftTipFont_Pen
        {
            get { return leftTipFont_Pen; }
            set { leftTipFont_Pen = value; }
        }

        /// <summary>
        /// 左侧Y轴提示框文字色的刷子
        /// </summary>
        private Brush leftyTipFont_Brush = new SolidBrush(Color.White);

        public Brush LeftyTipFont_Brush
        {
            get { return leftyTipFont_Brush; }
            set { leftyTipFont_Brush = value; }
        }

        /// <summary>
        /// 左侧Y轴提示框文字的字体
        /// </summary>
        private Font leftyTipFont = new Font("New Times Roman", 10, FontStyle.Bold);

        public Font LeftyTipFont
        {
            get { return leftyTipFont; }
            set { leftyTipFont = value; }
        }

        /// <summary>
        /// 右侧Y轴提示框背景色的刷子
        /// </summary>
        /// </summary>
        private Brush rightyTip_Brush = new SolidBrush(Color.FromArgb(100, Color.Red));

        public Brush RightyTip_Brush
        {
            get { return rightyTip_Brush; }
            set { rightyTip_Brush = value; }
        }

        /// <summary>
        /// 右侧Y轴提示框文字色的刷子
        /// </summary>
        private Brush rightyTipFont_Brush = new SolidBrush(Color.White);

        public Brush RightyTipFont_Brush
        {
            get { return leftyTipFont_Brush; }
            set { leftyTipFont_Brush = value; }
        }

        /// <summary>
        /// 右侧Y轴提示框文字色的画笔
        /// </summary>
        private Pen rightyTipFont_Pen = new Pen(Color.White);

        public Pen RightyTipFont_Pen
        {
            get { return rightyTipFont_Pen; }
            set { rightyTipFont_Pen = value; }
        }

        /// <summary>
        /// 右侧Y轴提示框文字的字体
        /// </summary>
        private Font rightyTipFont = new Font("New Times Roman", 10, FontStyle.Bold);

        public Font RightyTipFont
        {
            get { return leftyTipFont; }
            set { leftyTipFont = value; }
        }

        /// <summary>
        /// 面板边线的画笔
        /// </summary>
        private Pen panelBorder_Pen = new Pen(Color.SkyBlue);

        public Pen PanelBorder_Pen
        {
            get { return panelBorder_Pen; }
            set { panelBorder_Pen = value; }
        }

        /// <summary>
        /// 标题的笔刷
        /// </summary>
        private Brush titleFont_Brush = new SolidBrush(Color.White);

        public Brush TitleFont_Brush
        {
            get { return titleFont_Brush; }
            set { titleFont_Brush = value; }
        }

        /// <summary>
        /// 标题的字体
        /// </summary>
        private Font titleFont = new Font("New Times Roman", 9);

        public Font TitleFont
        {
            get { return titleFont; }
            set { titleFont = value; }
        }

        /// <summary>
        /// 网格线的画笔
        /// </summary>
        private Pen grid_Pen = new Pen(Color.FromArgb(100, Color.Red));

        public Pen Grid_Pen
        {
            get { return grid_Pen; }
            set { grid_Pen = value; }
        }

        /// <summary>
        /// X轴文字的刷子
        /// </summary>
        private Brush coordinateXFont_Brush = new SolidBrush(Color.White);

        public Brush CoordinateXFont_Brush
        {
            get { return coordinateXFont_Brush; }
            set { coordinateXFont_Brush = value; }
        }

        /// <summary>
        /// X轴文字的字体
        /// </summary>
        private Font coordinateXFont = new Font("New Times Roman", 9);

        public Font CoordinateXFont
        {
            get { return coordinateXFont; }
            set { coordinateXFont = value; }
        }

        /// <summary>
        /// 左侧Y轴文字的刷子
        /// </summary>
        private Brush leftYFont_Brush = new SolidBrush(Color.White);

        public Brush LeftYFont_Brush
        {
            get { return leftYFont_Brush; }
            set { leftYFont_Brush = value; }
        }

        /// <summary>
        /// 左侧Y轴文字的字体
        /// </summary>
        private Font leftYFont = new Font("New Times Roman", 9);

        public Font LeftYFont
        {
            get { return leftYFont; }
            set { leftYFont = value; }
        }

        /// <summary>
        /// 右侧Y轴文字的刷子
        /// </summary>
        private Brush rightYFont_Brush = new SolidBrush(Color.White);

        public Brush RightYFont_Brush
        {
            get { return rightYFont_Brush; }
            set { rightYFont_Brush = value; }
        }

        /// <summary>
        /// 右侧Y轴文字的字体
        /// </summary>
        private Font rightYFont = new Font("New Times Roman", 9);

        public Font RightYFont
        {
            get { return rightYFont; }
            set { rightYFont = value; }
        }

        /// <summary>
        /// 标题的高度
        /// </summary>
        private float titleHeight = 30;

        [Browsable(true)]
        public float TitleHeight
        {
            get { return titleHeight; }
            set { titleHeight = value; }
        }


        /// <summary>
        /// X轴的间隙
        /// </summary>
        private int scaleX_Height = 25;

        public int ScaleX_Height
        {
            get { return scaleX_Height; }
            set { scaleX_Height = value; }
        }
    }
}
