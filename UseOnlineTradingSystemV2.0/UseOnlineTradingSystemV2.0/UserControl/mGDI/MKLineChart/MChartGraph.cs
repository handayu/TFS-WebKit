using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;

namespace mPaint
{
    [ToolboxItem(true)]
    public partial class MChartGraph : ContainerControl
    {
        //1.趋势线拖动
        //2.标记可以被设置为可拖动
        //3.大菜单 技术分析以及右键菜单 添加指标
        //4.删除指标
        //5.MACD数据少时不准确
        //6.EMA可能有错误
        //7.布林带不准确(优先A)
        //8.SAR指标(优先C)
        //9.日期的格式化
        //10.要能滚动看不见的变量

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MChartGraph()
        {
            InitializeComponent();
            //加载事件
            this.HandleCreated += new EventHandler(ChartGraph_HandleCreated);
            this.SizeChanged += new EventHandler(ChartGraph_SizeChanged);
            this.Paint += new PaintEventHandler(PicGraph_Paint);
            this.PreviewKeyDown += new PreviewKeyDownEventHandler(PicGraph_PreviewKeyDown);
            this.KeyUp += new KeyEventHandler(ChartGraph_KeyUp);
            this.MouseMove += new MouseEventHandler(ChartGraph_MouseMove);
            this.MouseUp += new MouseEventHandler(ChartGraph_MouseUp);
            this.MouseDown += new MouseEventHandler(ChartGraph_MouseDown);
            this.MouseWheel += new MouseEventHandler(ChartGraph_MouseWheel);
        }
        #endregion

        #region 字段
        /// <summary>
        /// X轴显示的数据字段
        /// </summary>
        private string timekeyField;

        [Browsable(false)]
        public string TimekeyField
        {
            get { return timekeyField; }
            set { timekeyField = value; }
        }

        /// <summary>
        /// 十字线的Y坐标
        /// </summary>
        private int crossHair_y = -1;

        /// <summary>
        /// 鼠标点击时的x坐标
        /// </summary>
        private int mouse_x = -1;

        /// <summary>
        /// 鼠标点击时的y坐标
        /// </summary>
        private int mouse_y = -1;
     
        private DataTable dtAllMsg = new DataTable();

        /// <summary>
        /// 保存接收到的所有数据的DataTable.
        /// </summary>
        public DataTable DtAllMsg
        {
            get { return dtAllMsg; }
            set { dtAllMsg = value; }
        }
     
        private bool showCrossHair = false;

        /// <summary>
        /// 是否显示十字线
        /// </summary>
        [Browsable(false)]
        public bool ShowCrossHair
        {
            get { return showCrossHair; }
            set { showCrossHair = value; }
        }
        
        private int axisSpace;

        /// <summary>
        /// 每条数据所占的空间
        /// </summary>
        [Browsable(true)]
        public int AxisSpace
        {
            get { return axisSpace; }
            set { axisSpace = value; }
        }
        
        private int leftPixSpace;

        /// <summary>
        /// 左侧坐标轴的空隙
        /// </summary>
        [Browsable(true)]
        public int LeftPixSpace
        {
            get
            {
                return leftPixSpace;
            }
            set
            {
                leftPixSpace = value;
            }
        }
      
        private int rightPixSpace;

        /// <summary>
        /// 右侧左标轴的空隙
        /// </summary>
        [Browsable(true)]
        public int RightPixSpace
        {
            get
            {
                return rightPixSpace;
            }
            set
            {
                rightPixSpace = value;
            }
        }
      
        [DefaultValue(true)]
        private bool showRightScale;

        /// <summary>
        /// 是否显示右侧的坐标轴
        /// </summary>
        [Browsable(true)]
        public bool ShowRightScale
        {
            get
            {
                return showRightScale;
            }
            set
            {
                showRightScale = value;
            }
        }
        
        private bool showLeftScale;

        /// <summary>
        /// 是否显示左侧的坐标轴
        /// </summary>
        [Browsable(true)]
        public bool ShowLeftScale
        {
            get { return showLeftScale; }
            set
            {
                showLeftScale = value;
            }
        }
    
        private int firstVisibleRecord = 0;

        /// <summary>
        /// 第一条可见的记录
        /// </summary>
        [Browsable(false)]
        public int FirstVisibleRecord
        {
            get { return firstVisibleRecord; }
            set
            {
                firstVisibleRecord = value;
            }
        }
        
        private int lastVisibleRecord = 0;

        /// <summary>
        /// 最后一条可见的记录
        /// </summary>
        [Browsable(false)]
        public int LastVisibleRecord
        {
            get { return lastVisibleRecord; }
            set
            {
                lastVisibleRecord = value;
                if (this.dtAllMsg.Rows.Count > 0)
                {
                    this.lastVisibleTimeKey = dtAllMsg.Rows[lastVisibleRecord - 1][timekeyField].ToString();
                    if (LastVisibleRecord == this.dtAllMsg.Rows.Count)
                    {
                        lastRecordIsVisible = true;
                    }
                    else
                    {
                        lastRecordIsVisible = false;
                    }
                }
                else
                {
                    this.lastVisibleTimeKey = string.Empty;
                    lastRecordIsVisible = false;
                }
            }
        }

        /// <summary>
        /// 最后一条记录是否可见
        /// </summary>
        private bool lastRecordIsVisible = false;

        /// <summary>
        /// 最后一条可见记录的时间
        /// </summary>
        private string lastVisibleTimeKey = string.Empty;
   
        private int crossOverIndex;

        /// <summary>
        /// 鼠标选中的记录索引,从0开始
        /// </summary>
        [Browsable(false)]
        public int CrossOverIndex
        {
            get { return crossOverIndex; }
            set { crossOverIndex = value; }
        }
     
        private int scrollLeftStep = 1;

        /// <summary>
        /// 图像左滚的幅度
        /// </summary>
        [Browsable(true)]
        public int ScrollLeftStep
        {
            get { return scrollLeftStep; }
            set { scrollLeftStep = value; }
        }
       
        private int scrollRightStep = 1;

        /// <summary>
        /// 图像右滚的幅度
        /// </summary>
        [Browsable(true)]
        public int ScrollRightStep
        {
            get { return scrollRightStep; }
            set { scrollRightStep = value; }
        }

        private bool canDragSeries = false;

        /// <summary>
        /// 可以拖动线条
        /// </summary>
        public bool CanDragSeries
        {
            get { return canDragSeries; }
            set { canDragSeries = value; }
        }

        /// <summary>
        /// 进度条的值
        /// </summary>
        private int processBarValue = 0;

        [Browsable(false)]
        public int ProcessBarValue
        {
            get { return processBarValue; }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    processBarValue = value;
                }
                int pieR = 100;
                if (this.IsHandleCreated)
                {
                    Rectangle ellipseRect = new Rectangle(this.Width / 2 - pieR, this.Height / 2 - pieR, pieR * 2, pieR * 2);
                    DrawGraph(ellipseRect);
                }
            }
        }

        /// <summary>
        /// 股指提示的记录索引
        /// </summary>
        private int vp_index = -1;

        /// <summary>
        /// 保存面板的表
        /// </summary>
        private Dictionary<int, ChartPanel> dicChartPanel = new Dictionary<int, ChartPanel>();

        /// <summary>
        /// 刷新图像的锁
        /// </summary>
        private object refresh_lock = new object();

        /// <summary>
        /// 被选中的线条对象
        /// </summary>
        private object selectedObject;

        /// <summary>
        /// 鼠标最后一次移动的事件
        /// </summary>
        private DateTime lastMouseMoveTime = DateTime.Now;

        /// <summary>
        /// 标识是否准备绘制股指提示框
        /// </summary>
        private bool drawValuePanelFlag = false;

        /// <summary>
        /// 绘制显示股指提示框的委托
        /// </summary>
        private delegate void ShowValuePanelDelegate();

        /// <summary>
        /// 绘制图像的委托
        /// </summary>
        private delegate void DrawGraphDelegate();

        /// <summary>
        /// 控件标题占据的矩形作为键,控件本身作为值的表
        /// </summary>
        private Dictionary<RectangleF, object> objectRectDic = new Dictionary<RectangleF, object>();

        /// <summary>
        /// 按键滚动的幅度
        /// </summary>
        private int currentScrollStep = 1;

        private bool useScrollAddSpeed = false;

        /// <summary>
        /// 启用滚动加速效果
        /// </summary>
        public bool UseScrollAddSpeed
        {
            get { return useScrollAddSpeed; }
            set { useScrollAddSpeed = value; }
        }

        /// <summary>
        /// 正在改变大小的图层
        /// </summary>
        private ChartPanel userResizePanel = null;

        /// <summary>
        /// 简单移动平均线的集合
        /// </summary>
        private List<IndicatorSimpleMovingAverage> indSimpleMovingAverageList = new List<IndicatorSimpleMovingAverage>();

        /// <summary>
        /// 指数移动平均线的集合
        /// </summary>
        private List<IndicatorExponentialMovingAverage> indExponentialMovingAverageList = new List<IndicatorExponentialMovingAverage>();

        /// <summary>
        /// 随机指标的集合
        /// </summary>
        private List<IndicatorStochasticOscillator> indStochasticOscillatorList = new List<IndicatorStochasticOscillator>();

        /// <summary>
        /// 指数平滑异同移动平均线的集合
        /// </summary>
        private List<IndicatorMACD> indMacdList = new List<IndicatorMACD>();

        /// <summary>
        /// 布林带的集合
        /// </summary>
        private List<IndicatorBollingerBands> indBollList = new List<IndicatorBollingerBands>();
        #endregion

        #region 事件
        /// <summary>
        /// 鼠标滚动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ZoomIn(2);
            }
            else
            {
                ZoomOut(2);
            }
            this.setVisibleExtremeValue();
            ResetCrossOverRecord();
            DrawGraph();
            this.Focus();
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_MouseMove(object sender, MouseEventArgs e)
        {
            vp_index = -1;
            if (showCrossHair)
            {
                selectedObject = null;
            }
            if (selectedObject != null)
            {
                if (lastMouseMoveTime.AddTicks(1000000) < DateTime.Now)
                {
                    DrawGraph();
                }
                lastMouseMoveTime = DateTime.Now;
                drawValuePanelFlag = true;
            }
            if (this.userResizePanel == null)
            {
                if (!showCrossHair && !(e.Button==MouseButtons.Left))
                {
                    int pIndex = 0;
                    //当鼠标到纵向下边线上时，认为是需要调整大小
                    foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                    {
                        pIndex++;
                        if (pIndex == dicChartPanel.Count)
                        {
                            break;
                        }
                        Rectangle resizeRect = new Rectangle(0, chartPanel.RectPanel.Bottom - 2, chartPanel.RectPanel.Width, 4);
                        if (resizeRect.Contains(e.Location))
                        {
                            this.Cursor = Cursors.SizeNS;
                            goto OutLoop;
                        }
                    }
                    if (this.Cursor == Cursors.SizeNS)
                    {
                        this.Cursor = Cursors.Default;
                    }
                OutLoop: ;
                }
            }
        }

        /// <summary>
        /// 鼠标弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
            Point mp = GetCrossHairPoint();
            //实现线条的拖放
            if (canDragSeries)
            {
                if (!showCrossHair && selectedObject != null)
                {
                    foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                    {
                        if (mp.Y >= chartPanel.RectPanel.Y && mp.Y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height)
                        {
                            if (selectedObject is TrendLineSeries)
                            {
                                TrendLineSeries tls = selectedObject as TrendLineSeries;
                                if (!chartPanel.TrendLineSeriesList.Contains(tls))
                                {
                                    TitleField tfWaitToDrag = null;
                                    foreach (ChartPanel cp in this.dicChartPanel.Values)
                                    {
                                        if (cp.TrendLineSeriesList.Contains(tls))
                                        {
                                            cp.TrendLineSeriesList.Remove(tls);
                                            if (cp.YScaleField.Contains(tls.Field))
                                            {
                                                cp.YScaleField.Remove(tls.Field);
                                            }
                                            foreach (TitleField tf in cp.TitleFieldList)
                                            {
                                                if (!tf.MainFlag && tf.RelateSeriesField == tls.Field)
                                                {
                                                    tfWaitToDrag = tf;
                                                }
                                            }
                                            if (tfWaitToDrag != null)
                                            {
                                                cp.TitleFieldList.Remove(tfWaitToDrag); 
                                            }
                                            break;
                                        }
                                    }
                                    chartPanel.TrendLineSeriesList.Add(tls);
                                    chartPanel.YScaleField.Add(tls.Field);
                                    if (tfWaitToDrag != null)
                                    {
                                        chartPanel.TitleFieldList.Add(tfWaitToDrag);
                                    }
                                    selectedObject = null;
                                    RefreshGraph();
                                }
                            }
                            else if (selectedObject is HistogramSeries)
                            {
                                HistogramSeries hs = selectedObject as HistogramSeries;
                                if (!chartPanel.HistoramSeriesList.Contains(hs))
                                {
                                    TitleField tfWaitToDrag = null;
                                    foreach (ChartPanel cp in this.dicChartPanel.Values)
                                    {
                                        if (cp.HistoramSeriesList.Contains(hs))
                                        {
                                            if (cp.YScaleField.Contains(hs.Field))
                                            {
                                                cp.YScaleField.Remove(hs.Field);
                                            }
                                            cp.HistoramSeriesList.Remove(hs);
                                            foreach (TitleField tf in cp.TitleFieldList)
                                            {
                                                if (!tf.MainFlag && tf.RelateSeriesField == hs.Field)
                                                {
                                                    tfWaitToDrag = tf;
                                                }
                                            }
                                            if (tfWaitToDrag != null)
                                            {
                                                cp.TitleFieldList.Remove(tfWaitToDrag);
                                                
                                            }
                                            break;
                                        }
                                    }
                                    if (tfWaitToDrag != null)
                                    {
                                        chartPanel.TitleFieldList.Add(tfWaitToDrag);
                                    }
                                    chartPanel.HistoramSeriesList.Add(hs);
                                    chartPanel.YScaleField.Add(hs.Field);
                                    selectedObject = null;
                                    RefreshGraph();
                                }
                            }
                            else if (selectedObject is CandleSeries)
                            {
                                CandleSeries cs = selectedObject as CandleSeries;
                                if (!chartPanel.CandleSeriesList.Contains(cs))
                                {
                                    List<TitleField> waitToRemoveTfList = new List<TitleField>();
                                    foreach (ChartPanel cp in this.dicChartPanel.Values)
                                    {
                                        if (cp.CandleSeriesList.Contains(cs))
                                        {
                                            if (cp.YScaleField.Contains(cs.CloseField))
                                            {
                                                cp.YScaleField.Remove(cs.CloseField);
                                            }
                                            if (cp.YScaleField.Contains(cs.HighField))
                                            {
                                                cp.YScaleField.Remove(cs.HighField);
                                            }
                                            if (cp.YScaleField.Contains(cs.LowField))
                                            {
                                                cp.YScaleField.Remove(cs.LowField);
                                            }
                                            if (cp.YScaleField.Contains(cs.OpenField))
                                            {
                                                cp.YScaleField.Remove(cs.OpenField);
                                            }
                                            cp.CandleSeriesList.Remove(cs);
                                            foreach (TitleField tf in cp.TitleFieldList)
                                            {
                                                if (!tf.MainFlag)
                                                {
                                                    if (tf.RelateSeriesField == cs.OpenField || tf.RelateSeriesField == cs.HighField
                                                        || tf.RelateSeriesField == cs.LowField || tf.RelateSeriesField == cs.CloseField)
                                                    {
                                                        waitToRemoveTfList.Add(tf);
                                                    }
                                                }
                                            }
                                            foreach (TitleField tf in waitToRemoveTfList)
                                            {
                                                cp.TitleFieldList.Remove(tf);
                                            }
                                        }
                                    }
                                    chartPanel.CandleSeriesList.Add(cs);
                                    chartPanel.YScaleField.Add(cs.OpenField);
                                    chartPanel.YScaleField.Add(cs.HighField);
                                    chartPanel.YScaleField.Add(cs.CloseField);
                                    chartPanel.YScaleField.Add(cs.LowField);
                                    foreach (TitleField tf in waitToRemoveTfList)
                                    {
                                        chartPanel.TitleFieldList.Add(tf);
                                    }
                                    selectedObject = null;
                                    RefreshGraph();
                                }
                            }
                        }
                    }
                }
            }
            DragChartPanel();
        }

        /// <summary>
        /// 鼠标单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_MouseDown(object sender, MouseEventArgs e)
        {
            Point mp = GetCrossHairPoint();
            mouse_x = mp.X;
            mouse_y = mp.Y;
            if (e.Button == MouseButtons.Left && processBarValue == 0)
            {
                crossHair_y = mp.Y;
                if (e.Clicks == 1)
                {
                    //单击改变十字线准星位置
                    this.crossOverIndex = this.GetCrossOverIndex();
                    object obj = JudgeSelectedSeries(crossOverIndex, crossHair_y, true);
                    if (obj != null && !showCrossHair && canDragSeries)
                    {
                        this.Cursor = Cursors.Cross;
                    }
                    DrawGraph();
                }
                else if (e.Clicks == 2)
                {
                    //双击显示或隐藏十字线
                    this.ShowCrossHair = !this.ShowCrossHair;
                    this.crossOverIndex = this.GetCrossOverIndex();
                    selectedObject = null;
                    DrawGraph();
                }
            }
            //判断是否要进行resize
            if (!showCrossHair)
            {
                int pIndex = 0;
                //当鼠标到纵向下边线上时，认为是需要调整大小
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    pIndex++;
                    if (pIndex == dicChartPanel.Count)
                    {
                        break;
                    }
                    Rectangle resizeRect = new Rectangle(0, chartPanel.RectPanel.Bottom - 2, chartPanel.RectPanel.Width, 4);
                    if (resizeRect.Contains(mp))
                    {
                        this.Cursor = Cursors.SizeNS;
                        userResizePanel = chartPanel;
                        DrawGraph();
                        goto OutLoop;
                    }
                }
            OutLoop: ;
            }
            else
            {
                userResizePanel = null;
            }
            this.Focus();
        }

        /// <summary>
        /// 重绘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicGraph_Paint(object sender, PaintEventArgs e)
        {
            DrawGraph();
        }

        /// <summary>
        /// 图形大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_SizeChanged(object sender, EventArgs e)
        {
            if (this.Size.Width != 0 && this.Size.Height != 0)
            {
                ResizeGraph();
                RefreshGraph();
            }
        }

        /// <summary>
        /// 键盘弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_KeyUp(object sender, KeyEventArgs e)
        {
            currentScrollStep = 1;
        }

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicGraph_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            vp_index = -1;
            if (processBarValue == 0)
            {
                bool flag = false;
                bool locateCrossHairFlag = false;
                switch (e.KeyData)
                {
                    case Keys.Left:
                        flag = true;
                        if (showCrossHair)
                        {
                            CrossHairScrollLeft();
                            locateCrossHairFlag = true;
                        }
                        else
                        {
                            this.ScrollLeft(currentScrollStep);
                        }
                        break;
                    case Keys.Right:
                        flag = true;
                        if (showCrossHair)
                        {
                            CrossHairScrollRight();
                            locateCrossHairFlag = true;
                        }
                        else
                        {
                            this.ScrollRight(currentScrollStep);
                        }
                        break;
                    case Keys.Up:
                        flag = true;
                        this.ZoomIn(currentScrollStep);
                        break;
                    case Keys.Down:
                        flag = true;
                        this.ZoomOut(currentScrollStep);

                        break;
                }
                if (flag)
                {
                    this.setVisibleExtremeValue();
                    ResetCrossOverRecord();
                    if (locateCrossHairFlag)
                    {
                        LocateCrossHair();
                    }
                    DrawGraph();
                }
            }
            if (useScrollAddSpeed)
            {
                if (currentScrollStep < 40)
                {
                    currentScrollStep += 5;
                }
            }
            else
            {
                currentScrollStep = 1;
            }
            this.Focus();
        }

        /// <summary>
        /// 初始化界面属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_HandleCreated(object sender, EventArgs e)
        {
            Thread checkMouseMoveThread = new Thread(new ThreadStart(checkMouseMoveLoop));
            checkMouseMoveThread.IsBackground = true;
            checkMouseMoveThread.Start();
        }
        #endregion

        #region 界面设置
        /// <summary>
        /// 清除选中
        /// </summary>
        public void ClearSelectObj()
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                //K线
                foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                {
                    cs.HasSelect = false;
                }
                //柱状图
                foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                {
                    hs.HasSelect = false;
                }
                //线条
                foreach (TrendLineSeries tls in chartPanel.TrendLineSeriesList)
                {
                    tls.HasSelect = false;
                }
            }
            selectedObject = null;
        }

        /// <summary>
        /// 添加标记
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="timeKey"></param>
        /// <param name="st"></param>
        public SignalSeries AddSignal(int panelID, string timeKey, SignalType st, Color stColor, double value, bool canDrag)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                SignalSeries ss = new SignalSeries(value, st, stColor,canDrag);
                if (!chartPanel.SignalSeriesDic.ContainsKey(timeKey))
                {
                    chartPanel.SignalSeriesDic[timeKey] = new List<SignalSeries>();
                }
                chartPanel.SignalSeriesDic[timeKey].Add(ss);
                return ss;
            }
            return null;
        }

        /// <summary>
        /// 移除标记
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="timeKey"></param>
        public void RemoveSignal(int panelID, string timeKey)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                if (chartPanel.SignalSeriesDic.ContainsKey(timeKey))
                {
                    chartPanel.SignalSeriesDic.Remove(timeKey);
                }
            }
        }

        /// <summary>
        /// 判断是否选中了线条
        /// </summary>
        public object JudgeSelectedSeries(int curIndex, int mpY, bool setSelect)
        {
            bool hasSelect = false;
            object obj = null;
            if (setSelect)
            {
                Point mp = GetCrossHairPoint();
                foreach (RectangleF titleRect in this.objectRectDic.Keys)
                {
                    if (titleRect.Contains(mp))
                    {
                        hasSelect = true;
                        obj = objectRectDic[titleRect];
                        this.selectedObject = obj;
                        break;
                    }
                }
            }
            if (firstVisibleRecord != 0 && LastVisibleRecord != 0 && processBarValue == 0)
            {
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    if (chartPanel.TrendLineSeriesList.Count > 0)
                    {
                        foreach (TrendLineSeries tls in chartPanel.TrendLineSeriesList)
                        {
                            if (hasSelect)
                            {
                                if (setSelect)
                                {
                                    if (obj != null && obj == tls)
                                    {
                                        tls.HasSelect = true;
                                    }
                                    else
                                    {
                                        tls.HasSelect = false;
                                    }
                                }
                            }
                            else
                            {
                                if (curIndex > LastVisibleRecord - 1 || this.dtAllMsg.Rows[curIndex][tls.Field].ToString() == "")
                                {
                                    if (setSelect)
                                    {
                                        tls.HasSelect = false;
                                    }
                                    continue;
                                }
                                double lineValue = Convert.ToDouble(this.dtAllMsg.Rows[curIndex][tls.Field].ToString());
                                float scaleX = this.leftPixSpace + (curIndex + 2 - firstVisibleRecord) * axisSpace - axisSpace / 2;
                                int topY = Convert.ToInt32(GetValueYPixel(chartPanel, lineValue));
                                Point crossHairP = GetCrossHairPoint();
                                int judgeTop = 0;
                                float judgeScaleX = scaleX;
                                if (crossHairP.X >= scaleX)
                                {
                                    if (curIndex < this.LastVisibleRecord - 1 && this.dtAllMsg.Rows[curIndex + 1][tls.Field].ToString() != "")
                                    {
                                        double rightValue = Convert.ToDouble(this.dtAllMsg.Rows[curIndex + 1][tls.Field].ToString());
                                        judgeTop = Convert.ToInt32(GetValueYPixel(chartPanel, rightValue));
                                        if (judgeTop > chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height || judgeTop < chartPanel.RectPanel.Y + chartPanel.TitleHeight)
                                        {
                                            if (setSelect)
                                            {
                                                tls.HasSelect = false;
                                            }
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        judgeTop = topY;
                                    }
                                }
                                else
                                {
                                    judgeScaleX = scaleX - axisSpace;
                                    if (curIndex > 0 && this.dtAllMsg.Rows[curIndex - 1][tls.Field].ToString() != "")
                                    {
                                        double leftValue = Convert.ToDouble(this.dtAllMsg.Rows[curIndex - 1][tls.Field].ToString());
                                        judgeTop = Convert.ToInt32(GetValueYPixel(chartPanel, leftValue));
                                        if (judgeTop > chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height || judgeTop < chartPanel.RectPanel.Y + chartPanel.TitleHeight)
                                        {
                                            if (setSelect)
                                            {
                                                tls.HasSelect = false;
                                            }
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        judgeTop = topY;
                                    }
                                }
                                Rectangle judgeRect = new Rectangle();
                                if (judgeTop >= topY)
                                {
                                    judgeRect = new Rectangle((int)judgeScaleX, topY, axisSpace, judgeTop - topY < 1 ? 1 : judgeTop - topY);
                                }
                                else
                                {
                                    judgeRect = new Rectangle((int)judgeScaleX, judgeTop, axisSpace, topY - judgeTop < 1 ? 1 : topY - judgeTop);
                                }
                                if (judgeRect.Contains(crossHairP))
                                {
                                    if (setSelect)
                                    {
                                        selectedObject = tls;
                                        tls.HasSelect = true;
                                    }
                                    obj = tls;
                                    hasSelect = true;
                                }
                                else
                                {
                                    if (setSelect)
                                    {
                                        tls.HasSelect = false;
                                    }
                                }
                            }
                        }
                    }
                    if (chartPanel.HistoramSeriesList.Count > 0)
                    {
                        foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                        {
                            if (hasSelect)
                            {
                                if (setSelect)
                                {
                                    if (obj != null && obj == hs)
                                    {
                                        hs.HasSelect = true;
                                    }
                                    else
                                    {
                                        hs.HasSelect = false;
                                    }
                                }
                            }
                            else
                            {
                                if (curIndex > LastVisibleRecord - 1 || this.dtAllMsg.Rows[curIndex][hs.Field].ToString() == "")
                                {
                                    hs.HasSelect = false;
                                    continue;
                                }
                                double volumn = Convert.ToDouble(this.dtAllMsg.Rows[curIndex][hs.Field].ToString());
                                int topY = Convert.ToInt32(GetValueYPixel(chartPanel, volumn));
                                int bottomY = Convert.ToInt32(GetValueYPixel(chartPanel, 0));
                                if (volumn < 0)
                                {
                                    topY = Convert.ToInt32(GetValueYPixel(chartPanel, 0));
                                    bottomY = Convert.ToInt32(GetValueYPixel(chartPanel, volumn));
                                }
                                if (topY >= chartPanel.RectPanel.Y && bottomY <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height
                                    && mpY >= topY && mpY <= bottomY)
                                {
                                    if (setSelect)
                                    {
                                        selectedObject = hs;
                                        hs.HasSelect = true;
                                    }
                                    obj = hs;
                                    hasSelect = true;
                                }
                                else
                                {
                                    if (setSelect)
                                    {
                                        hs.HasSelect = false;
                                    }
                                }
                            }
                        }
                    }
                    if (chartPanel.CandleSeriesList.Count > 0)
                    {
                        foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                        {
                            if (hasSelect)
                            {
                                if (setSelect)
                                {
                                    if (obj != null && obj == cs)
                                    {
                                        cs.HasSelect = true;
                                    }
                                    else
                                    {
                                        cs.HasSelect = false;
                                    }
                                }
                            }
                            else
                            {
                                if (curIndex > LastVisibleRecord - 1
                                    || this.dtAllMsg.Rows[curIndex][cs.HighField].ToString() == ""
                                    || this.dtAllMsg.Rows[curIndex][cs.LowField].ToString() == "")
                                {
                                    if (setSelect)
                                    {
                                        cs.HasSelect = false;
                                    }
                                    continue;
                                }
                                double highValue = Convert.ToDouble(this.dtAllMsg.Rows[curIndex][cs.HighField].ToString());
                                double lowValue = Convert.ToDouble(this.dtAllMsg.Rows[curIndex][cs.LowField].ToString());
                                int topY = Convert.ToInt32(GetValueYPixel(chartPanel, highValue));
                                int bottomY = Convert.ToInt32(GetValueYPixel(chartPanel, lowValue));
                                if (topY >= chartPanel.RectPanel.Y && bottomY <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height
                                    && mpY >= topY && mpY <= bottomY)
                                {
                                    if (setSelect)
                                    {
                                        cs.HasSelect = true;
                                        selectedObject = cs;
                                    }
                                    obj = cs;
                                    hasSelect = true;
                                }
                                else
                                {
                                    if (setSelect)
                                    {
                                        cs.HasSelect = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (obj == null)
            {
                selectedObject = null;
            }
            return obj;
        }

        /// <summary>
        /// 重新调整图像的大小
        /// </summary>
        public void ResizeGraph()
        {
            int locationY = 0;
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                chartPanel.RectPanel = new Rectangle(0, locationY, this.Width, Convert.ToInt32((double)chartPanel.VerticalPercent / 100 * this.Height));
                locationY += Convert.ToInt32((double)chartPanel.VerticalPercent / 100 * this.Height);
            }
        }

        /// <summary>
        /// 用户自己拖动图像改变大小
        /// </summary>
        public void DragChartPanel()
        {
            if (userResizePanel != null)
            {
                Point mp = GetCrossHairPoint();
                ChartPanel nextCP = null;
                bool rightP = false;
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    if (rightP)
                    {
                        nextCP = chartPanel;
                        break;
                    }
                    if (chartPanel == userResizePanel)
                    {
                        rightP = true;
                    }
                }
                int originalVP = userResizePanel.VerticalPercent;
                if (userResizePanel.RectPanel.Contains(mp))
                {
                    userResizePanel.VerticalPercent = Convert.ToInt32(((double)mp.Y - (double)userResizePanel.RectPanel.Top) / (double)this.Height * 100);
                    if (userResizePanel.VerticalPercent < 1)
                    {
                        userResizePanel.VerticalPercent = 1;
                    }
                    if (nextCP != null)
                    {
                        nextCP.VerticalPercent += originalVP - userResizePanel.VerticalPercent;
                    }
                }
                else
                {
                    if (nextCP != null && nextCP.RectPanel.Contains(mp))
                    {
                        userResizePanel.VerticalPercent = Convert.ToInt32(((double)mp.Y - (double)userResizePanel.RectPanel.Top) / (double)this.Height * 100);
                        if (userResizePanel.VerticalPercent >= originalVP + nextCP.VerticalPercent)
                        {
                            userResizePanel.VerticalPercent -= 1;
                        }
                        nextCP.VerticalPercent = originalVP + nextCP.VerticalPercent - userResizePanel.VerticalPercent;
                    }
                }
                userResizePanel = null;
                ResizeGraph();
                DrawGraph();
            }
        }

        /// <summary>
        /// 定位十字线
        /// </summary>
        public void LocateCrossHair()
        {
            if (this.dtAllMsg.Rows.Count > 0)
            {
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    if (crossHair_y >= chartPanel.RectPanel.Y && crossHair_y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height)
                    {
                        if (GetWorkSpaceY(chartPanel.PanelID) > 0)
                        {
                            if (this.crossOverIndex >= 0 && this.crossOverIndex < this.dtAllMsg.Rows.Count)
                            {
                                if (chartPanel.CandleSeriesList.Count > 0)
                                {
                                    double closeValue = Convert.ToDouble(this.dtAllMsg.Rows[this.crossOverIndex][chartPanel.CandleSeriesList[0].CloseField]);
                                    crossHair_y = Convert.ToInt32(GetValueYPixel(chartPanel, closeValue));
                                    return;
                                }
                                if (chartPanel.HistoramSeriesList.Count > 0)
                                {
                                    double volumn = Convert.ToDouble(this.dtAllMsg.Rows[this.crossOverIndex][chartPanel.HistoramSeriesList[0].Field]);
                                    crossHair_y = Convert.ToInt32(GetValueYPixel(chartPanel, volumn));
                                    return;
                                }
                                if (chartPanel.TrendLineSeriesList.Count > 0)
                                {
                                    double lineValue = Convert.ToDouble(this.dtAllMsg.Rows[this.crossOverIndex][chartPanel.TrendLineSeriesList[0].Field]);
                                    crossHair_y = Convert.ToInt32(GetValueYPixel(chartPanel, lineValue));
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 十字线左滚
        /// </summary>
        public void CrossHairScrollLeft()
        {
            int currentRecord = this.crossOverIndex;
            this.crossOverIndex = currentRecord - 1;
            if (crossOverIndex < 0)
            {
                crossOverIndex = 0;
            }
            if (currentRecord < this.firstVisibleRecord)
            {
                ScrollLeft(1);
            }
        }

        /// <summary>
        /// 十字线右滚
        /// </summary>
        public void CrossHairScrollRight()
        {
            int currentRecord = this.crossOverIndex;
            this.crossOverIndex = currentRecord + 1;
            int maxRecord = GetMaxVisibleRecord();
            if (this.dtAllMsg.Rows.Count < maxRecord)
            {
                if (crossOverIndex >= maxRecord-1)
                {
                    crossOverIndex = maxRecord - 1;
                }
            }
            if (currentRecord >= this.LastVisibleRecord - 1)
            {
                ScrollRight(1);
            }
        }

        /// <summary>
        /// 设置指定面板的买卖线字段
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="buyField"></param>
        /// <param name="sellField"></param>
        public void SetCandleBuySellField(string candleName, string buyField, string sellField)
        {
            foreach (ChartPanel cp in this.dicChartPanel.Values)
            {
                foreach (CandleSeries cs in cp.CandleSeriesList)
                {
                    if (cs.CandleName == candleName)
                    {
                        cs.IndBuySellField[0] = buyField;
                        cs.IndBuySellField[1] = sellField;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 设置信息地雷
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="info"></param>
        /// <param name="dt"></param>
        public void SetInfoBombText(int panelID,string info,DateTime dt)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                string field = chartPanel.InfoBombField;
                if (!this.dtAllMsg.Columns.Contains(field))
                {
                    this.dtAllMsg.Columns.Add(field);
                }
                SetValue(field, info, dt);
            }
        }

        /// <summary>
        /// 设置面板的时间类型
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="intervalType"></param>
        public void SetIntervalType(int panelID, IntervalType intervalType)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].Interval = intervalType;
            }
        }

        /// <summary>
        /// 设置信息地雷的样式
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="bgColor"></param>
        /// <param name="selectedColor"></param>
        public void SetInfoBombStyle(int panelID, Color bgColor, Color selectedColor,Color tipBgColor,Color tipTextColor)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                chartPanel.InfoBombColor = bgColor;
                chartPanel.InfoBombSelectedColor = selectedColor;
                chartPanel.InfoBombTipColor = tipBgColor;
                chartPanel.InfoBombTipTextColor = tipTextColor;
            }
        }

        /// <summary>
        /// 设置买卖文字的样式
        /// </summary>
        /// <param name="candleName"></param>
        /// <param name="buyText"></param>
        /// <param name="sellText"></param>
        /// <param name="buyColor"></param>
        /// <param name="sellColor"></param>
        /// <param name="bsFont"></param>
        public void SetCandleBuySellStyle(string candleName, string buyText, string sellText, Color buyColor, Color sellColor, Font bsFont)
        {
            foreach (ChartPanel cp in this.dicChartPanel.Values)
            {
                foreach (CandleSeries cs in cp.CandleSeriesList)
                {
                    if (cs.CandleName == candleName)
                    {
                        cs.BuyText = buyText;
                        cs.SellText = sellText;
                        cs.BuyColor = buyColor;
                        cs.SellColor = sellColor;
                        cs.BsFont = bsFont;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 设置指定面板的最小变动值
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="tick"></param>
        public void SetTick(int panelID, double tick)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].YScaleTick = tick;
            }
        }

        /// <summary>
        /// 设置面板的数值保留小数位数
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="digit"></param>
        public void SetDigit(int panelID, int digit)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].Digit = digit;
            }
        }

        /// <summary>
        /// 设置滚动的幅度
        /// </summary>
        /// <param name="leftStep"></param>
        /// <param name="rightStep"></param>
        public void SetSrollStep(int leftStep, int rightStep)
        {
            this.scrollLeftStep = leftStep;
            this.scrollRightStep = rightStep;
        }

        /// <summary>
        /// 设置指定面板的标题
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="title"></param>
        public void SetTitle(int panelID, string title)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].PanelTitle = title;
            }
        }

        /// <summary>
        /// 设置面板的网格线的间隔
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="gridInterval"></param>
        public void SetGridInterval(int panelID, int gridInterval)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].GridInterval = gridInterval;
            }
        }

        /// <summary>
        /// 设置十字线的样式
        /// </summary>
        /// <param name="lineColor"></param>
        /// <param name="weight"></param>
        public void SetCrossHairStyle(int panelID,Color crossHairColor, int weight)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                chartPanel.CrossHair_Pen.Dispose();
                chartPanel.CrossHair_Pen = new Pen(crossHairColor, weight);
            }
        }

        /// <summary>
        /// 设置X轴提示框的样式
        /// </summary>
        /// <param name="backColor"></param>
        /// <param name="fontColor"></param>
        public void SetXTipStyle(int panelID, Color backColor, Color fontColor, Font xTipFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].Xtip_Brush.Dispose();
                this.dicChartPanel[panelID].Xtip_Brush = new SolidBrush(backColor);
                this.dicChartPanel[panelID].XTipFont_Brush.Dispose();
                this.dicChartPanel[panelID].XTipFont_Brush = new SolidBrush(fontColor);
                this.dicChartPanel[panelID].XTipFont = xTipFont;
                this.dicChartPanel[panelID].XTipFont_Pen.Color = fontColor;
            }
        }

        /// <summary>
        /// 设置左侧Y轴提示框的样式
        /// </summary>
        /// <param name="backColor"></param>
        /// <param name="fontColor"></param>
        public void SetLeftYTipStyle(int panelID, Color backColor, Color fontColor, Font xTipFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].LeftyTip_Brush.Dispose();
                this.dicChartPanel[panelID].LeftyTip_Brush = new SolidBrush(backColor);
                this.dicChartPanel[panelID].LeftyTipFont_Brush.Dispose();
                this.dicChartPanel[panelID].LeftyTipFont_Brush = new SolidBrush(fontColor);
                this.dicChartPanel[panelID].LeftyTipFont = xTipFont;
                this.dicChartPanel[panelID].LeftTipFont_Pen.Color = fontColor;
            }
        }

        /// <summary>
        /// 设置右侧Y轴提示框的样式
        /// </summary>
        /// <param name="backColor"></param>
        /// <param name="fontColor"></param>
        public void SetRightYTipStyle(int panelID, Color backColor, Color fontColor, Font xTipFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].RightyTip_Brush.Dispose();
                this.dicChartPanel[panelID].RightyTip_Brush = new SolidBrush(backColor);
                this.dicChartPanel[panelID].RightyTipFont_Brush.Dispose();
                this.dicChartPanel[panelID].RightyTipFont_Brush = new SolidBrush(fontColor);
                this.dicChartPanel[panelID].RightyTipFont = xTipFont;
                this.dicChartPanel[panelID].RightyTipFont_Pen.Color = fontColor;
            }
        }

        /// <summary>
        /// 设置面板的背景色
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="bgColor"></param>
        public void SetBackColor(int panelID, Color bgColor)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].BgBrush.Dispose();
                this.dicChartPanel[panelID].BgBrush = new SolidBrush(bgColor);
            }
        }

        /// <summary>
        /// 设置面板的边线
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="borderColor"></param>
        /// <param name="dashStyle"></param>
        /// <param name="width"></param>
        public void SetBorderStyle(int panelID, Color borderColor, DashStyle dashStyle, int width)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].PanelBorder_Pen.Dispose();
                this.dicChartPanel[panelID].PanelBorder_Pen = new Pen(borderColor, width);
                if (dashStyle != DashStyle.Custom)
                {
                    this.dicChartPanel[panelID].PanelBorder_Pen.DashStyle = dashStyle;
                }
            }
        }

        /// <summary>
        /// 设置标题的样式
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="titleColor"></param>
        /// <param name="titleFont"></param>
        public void SetTitleStyle(int panelID, Color titleColor, Font titleFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].TitleFont_Brush.Dispose();
                this.dicChartPanel[panelID].TitleFont_Brush = new SolidBrush(titleColor);
                this.dicChartPanel[panelID].TitleFont = titleFont;
                this.dicChartPanel[panelID].TitleHeight = this.CreateGraphics().MeasureString(" ", titleFont).Height + 5f;
            }
        }

        /// <summary>
        /// 设置网格线的样式
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="gridColor"></param>
        /// <param name="dashStyle"></param>
        /// <param name="width"></param>
        public void SetGridStyle(int panelID, Color gridColor, DashStyle dashStyle, bool showGrid, int width)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].Grid_Pen.Color = gridColor;
                this.dicChartPanel[panelID].Grid_Pen.DashStyle = dashStyle;
                this.dicChartPanel[panelID].ShowGrid = showGrid;
            }
        }

        /// <summary>
        /// 设置X轴的样式
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="fontColor"></param>
        /// <param name="scaleFont"></param>
        public void SetXScaleStyle(int panelID,Color scaleColor, Color fontColor, Font scaleFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].XScalePen.Dispose();
                this.dicChartPanel[panelID].XScalePen = new Pen(scaleColor);
                this.dicChartPanel[panelID].CoordinateXFont_Brush.Dispose();
                this.dicChartPanel[panelID].CoordinateXFont_Brush = new SolidBrush(fontColor);
                this.dicChartPanel[panelID].CoordinateXFont = scaleFont;
                this.dicChartPanel[panelID].ScaleX_Height = (int)(this.CreateGraphics().MeasureString(" ", scaleFont).Height * 1.2) + 2;
            }
        }

        /// <summary>
        /// 设置左侧Y轴的样式
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="fontColor"></param>
        /// <param name="scaleFont"></param>
        public void SetLeftYScaleStyle(int panelID,Color scaleColor, Color fontColor, Font scaleFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].LeftScalePen.Dispose();
                this.dicChartPanel[panelID].LeftScalePen = new Pen(scaleColor);
                this.dicChartPanel[panelID].LeftYFont_Brush.Dispose();
                this.dicChartPanel[panelID].LeftYFont_Brush = new SolidBrush(fontColor);
                this.dicChartPanel[panelID].LeftYFont = scaleFont;
            }
        }

        /// <summary>
        /// 设置右侧Y轴的样式
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="fontColor"></param>
        /// <param name="scaleFont"></param>
        public void SetRightYScaleStyle(int panelID,Color scaleColor, Color fontColor, Font scaleFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].RightScalePen.Dispose();
                this.dicChartPanel[panelID].RightScalePen = new Pen(scaleColor);
                this.dicChartPanel[panelID].RightYFont_Brush.Dispose();
                this.dicChartPanel[panelID].RightYFont_Brush = new SolidBrush(fontColor);
                this.dicChartPanel[panelID].RightYFont = scaleFont;
            }
        }

        /// <summary>
        /// 添加新的面板
        /// </summary>
        /// <returns></returns>
        public int AddChartPanel(int verticalPercent)
        {
            int locationY = 0;
            foreach (ChartPanel cp in this.dicChartPanel.Values)
            {
                locationY += Convert.ToInt32((double)cp.VerticalPercent / 100 * this.Height);
            }
            int panelHeight = Convert.ToInt32((double)verticalPercent / 100 * this.Height);
            ChartPanel chartPanel = new ChartPanel();
            chartPanel.VerticalPercent = verticalPercent;
            chartPanel.PanelID = CommonClass.GetPanelID(); 
            chartPanel.RectPanel = new Rectangle(0, locationY, this.Width, panelHeight);
            this.dicChartPanel[chartPanel.PanelID] = chartPanel;
            return chartPanel.PanelID;
        }

        /// <summary>
        /// 重置为空的图像
        /// </summary>
        public void ResetNullGraph()
        {
            ClearGraph();
            this.dicChartPanel.Clear();
            this.indExponentialMovingAverageList.Clear();
            this.indSimpleMovingAverageList.Clear();
            this.indMacdList.Clear();
            this.indBollList.Clear();
            this.indStochasticOscillatorList.Clear();
            this.dtAllMsg.Clear();
            this.dtAllMsg.Dispose();
            this.dtAllMsg = new DataTable();
            this.timekeyField = string.Empty;
            this.crossHair_y = -1;
            this.axisSpace = 8;
            this.leftPixSpace = 0;
            this.rightPixSpace = 0;
            this.showLeftScale = false;
            this.showRightScale = false;
            scrollLeftStep = 1;
            canDragSeries = false;
            vp_index = -1;
            drawValuePanelFlag = false;
            DrawGraph();
        }
         
        /// <summary>
        /// 设置K线的字段标题色
        /// </summary>
        /// <param name="openColor"></param>
        /// <param name="highColor"></param>
        /// <param name="lowColor"></param>
        /// <param name="closeColor"></param>
        public void SetCandleTitleColor(string candleName, Color openColor, Color highColor, Color lowColor, Color closeColor, int panelID)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                foreach (CandleSeries candleSeries in chartPanel.CandleSeriesList)
                {
                    if (candleSeries.CandleName == candleName)
                    {
                        candleSeries.OpenTitleColor = openColor;
                        candleSeries.HighTitleColor = highColor;
                        candleSeries.LowTitleColor = lowColor;
                        candleSeries.CloseTitleColor = closeColor;
                    }
                }
            }
        }

        /// <summary>
        /// 显示股票相关的值的窗体
        /// </summary>
        public void ShowValuePanel()
        {
            if (selectedObject != null)
            {
                int curRecord = GetCrossOverIndex();
                if (JudgeSelectedSeries(curRecord, GetCrossHairPoint().Y, false) == selectedObject)
                {
                    vp_index = curRecord;
                    DrawGraph();
                }
            }
        }

        /// <summary>
        /// 监测鼠标移动的循环
        /// </summary>
        public void checkMouseMoveLoop()
        {
            int tick_dely = 5000000;
            while (true)
            {
                if (lastMouseMoveTime.AddTicks(tick_dely) <= DateTime.Now)
                {
                    if (drawValuePanelFlag)
                    {
                        drawValuePanelFlag = !drawValuePanelFlag;
                        if (selectedObject != null && this.IsHandleCreated)
                        {
                            if (this.IsHandleCreated)
                            {
                                this.BeginInvoke(new ShowValuePanelDelegate(ShowValuePanel));
                            }
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 追加K线
        /// </summary>
        /// <returns></returns>
        public CandleSeries AddCandle(string candleName, string openfield, string highfield, string lowfield, string closefield, int panelID, bool displayTitleField)
        {
            if (openfield == null || highfield == null || lowfield == null || closefield == null)
            {
                return null;
            }
            if (this.dtAllMsg.Columns.Contains(openfield) ||
                this.dtAllMsg.Columns.Contains(highfield) ||
                this.dtAllMsg.Columns.Contains(lowfield) ||
                this.dtAllMsg.Columns.Contains(closefield))
            {
                return null;
            }
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                CandleSeries candleSeries = new CandleSeries();
                candleSeries = new CandleSeries();
                candleSeries.CandleName = candleName;
                candleSeries.OpenField = openfield;
                candleSeries.HighField = highfield;
                candleSeries.LowField = lowfield;
                candleSeries.CloseField = closefield;
                chartPanel.YScaleField.AddRange(new string[] { openfield, highfield, lowfield, closefield });
                candleSeries.Down_Color = Color.SkyBlue;
                candleSeries.Up_Color = Color.Red;
                candleSeries.DisplayTitleField = displayTitleField;
                DataColumn dcOpen = new DataColumn(openfield);
                DataColumn dcHigh = new DataColumn(highfield);
                DataColumn dcLow = new DataColumn(lowfield);
                DataColumn dcClose = new DataColumn(closefield);
                this.dtAllMsg.Columns.Add(dcOpen);
                this.dtAllMsg.Columns.Add(dcHigh);
                this.dtAllMsg.Columns.Add(dcLow);
                this.dtAllMsg.Columns.Add(dcClose);
                chartPanel.CandleSeriesList.Add(candleSeries);
                return candleSeries;
            }
            return null;
        }

        /// <summary>
        /// 设置K线柱的样式
        /// </summary>
        /// <param name="candleName"></param>
        /// <param name="upColor"></param>
        /// <param name="downColor"></param>
        /// <param name="middleColor"></param>
        public void SetCandleStyle(string candleName, Color upColor, Color downColor, Color middleColor)
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                {
                    if (cs.CandleName == candleName)
                    {
                        cs.Up_Color = upColor;
                        cs.Down_Color = downColor;
                        cs.Middle_Color = middleColor;
                    }
                }
            }
        }

        /// <summary>
        /// 设置柱状图的样式
        /// </summary>
        /// <param name="field"></param>
        /// <param name="upColor"></param>
        /// <param name="downColor"></param>
        /// <param name="width"></param>
        public void SetHistogramStyle(string field, Color upColor, Color downColor, int width, bool lineStyle)
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                {
                    if (hs.Field == field)
                    {
                        hs.LineStyle = lineStyle;
                        hs.LineWidth = width;
                        hs.Up_LineColor = upColor;
                        hs.Down_lineColor = downColor;
                        hs.LineStyle = lineStyle;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 设置趋势线的样式
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lineColor"></param>
        /// <param name="width"></param>
        /// <param name="dashStyle"></param>
        public void SetTrendLineStyle(string field, Color upLineColor, Color downLineColor, int width, DashStyle dashStyle)
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                foreach (TrendLineSeries tls in chartPanel.TrendLineSeriesList)
                {
                    if (tls.Field == field)
                    {
                        tls.Up_LineColor = upLineColor;
                        tls.Up_LinePen.Width = width;
                        tls.Down_LineColor = downLineColor;
                        tls.Down_linePen.Width = width;
                        if (dashStyle != DashStyle.Custom)
                        {
                            tls.Up_LinePen.DashStyle = dashStyle;
                            tls.Down_linePen.DashStyle = dashStyle;
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 追加成交量
        /// </summary>
        /// <param name="field"></param>
        /// <param name="hisColor"></param>
        /// <returns></returns>
        public HistogramSeries AddHistogram(string field, string displayName, string relateCandleName, int panelID)
        {
            if (field == null || this.dtAllMsg.Columns.Contains(field))
            {
                return null;
            }
            else
            {
                if (this.dicChartPanel.ContainsKey(panelID))
                {
                    ChartPanel chartPanel = this.dicChartPanel[panelID];
                    HistogramSeries histogramSeries = new HistogramSeries();
                    histogramSeries.Up_LineColor = Color.Lime;
                    histogramSeries.RelateCandleName = relateCandleName;
                    histogramSeries.Down_lineColor = Color.Lime;
                    chartPanel.YScaleField.Add(field);
                    histogramSeries.Field = field;
                    histogramSeries.DisplayName = displayName;
                    chartPanel.HistoramSeriesList.Add(histogramSeries);
                    DataColumn dc = new DataColumn(field);
                    this.dtAllMsg.Columns.Add(dc);
                    return histogramSeries;
                }
                return null;
            }
        }

        /// <summary>
        /// 追加趋势线
        /// </summary>
        /// <param name="field"></param>
        public TrendLineSeries AddTrendLine(string field, string displayName, int panelID)
        {
            if (field == null || this.dtAllMsg.Columns.Contains(field))
            {
                return null;
            }
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                TrendLineSeries lineSeries = new TrendLineSeries();
                lineSeries.Up_LineColor = Color.Yellow;
                lineSeries.Down_LineColor = Color.Yellow;
                lineSeries.Field = field;
                chartPanel.YScaleField.Add(field);
                if (displayName != null && displayName.Length > 0)
                {
                    lineSeries.DisplayName = displayName;
                }
                chartPanel.TrendLineSeriesList.Add(lineSeries);
                DataColumn dc = new DataColumn(field);
                this.dtAllMsg.Columns.Add(dc);
                return lineSeries;
            }
            return null;
        }

        /// <summary>
        /// 追加SMA曲线
        /// </summary>
        /// <returns></returns>
        public IndicatorSimpleMovingAverage AddSimpleMovingAverage(string field, string displayName, string target, int cycle, int panelID)
        {
            if (field == null || this.dtAllMsg.Columns.Contains(field))
            {
                return null;
            }
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                IndicatorSimpleMovingAverage indSMA = new IndicatorSimpleMovingAverage();
                indSMA.Target = target;
                indSMA.Cycle = cycle;
                indSMA.DataSource = this.dtAllMsg;
                indSMA.TrendLineSeries = AddTrendLine(field, displayName, panelID);
                this.indSimpleMovingAverageList.Add(indSMA);
                return indSMA;
            }
            return null;
        }

        /// <summary>
        /// 追加KDJ指标
        /// </summary>
        public IndicatorStochasticOscillator AddStochasticOscillator(string k, string d, string j, int kPeriod,
            string close, string high, string low, int panelID)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                IndicatorStochasticOscillator indSO = new IndicatorStochasticOscillator();
                this.indStochasticOscillatorList.Add(indSO);
                indSO.DataSource = dtAllMsg;
                indSO.Close = close;
                indSO.High = high;
                indSO.Low = low;
                indSO.KPeriods = kPeriod;
                //K
                indSO.TlsK = AddTrendLine(k, k, panelID);
                SetTrendLineStyle(k, Color.White, Color.White, 1, DashStyle.Solid);
                //D
                indSO.TlsD = AddTrendLine(d, d, panelID);
                SetTrendLineStyle(d, Color.Yellow, Color.Yellow, 1, DashStyle.Solid);
                //J
                indSO.TlsJ = AddTrendLine(j, j, panelID);
                SetTrendLineStyle(j, Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 255), 1, DashStyle.Solid);
                return indSO;
            }
            return null;
        }

        /// <summary>
        /// 添加EMA曲线
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lineColor"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public IndicatorExponentialMovingAverage AddExponentialMovingAverage(string field, string displayName, int cycle, string target, int panelID)
        {
            if (field == null || this.dtAllMsg.Columns.Contains(field))
            {
                return null;
            }
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                IndicatorExponentialMovingAverage indEMA = new IndicatorExponentialMovingAverage();
                indEMA.Cycle = cycle;
                indEMA.Target = target;
                indEMA.DataSource = this.dtAllMsg;
                indEMA.TrendLineSeries = AddTrendLine(field, displayName, panelID);
                this.indExponentialMovingAverageList.Add(indEMA);
                return indEMA;
            }
            return null;
        }

        /// <summary>
        /// 追加MACD指标
        /// </summary>
        /// <returns></returns>
        public IndicatorMACD AddMacd(string macd,string diff,string dea,string close,
            int longCycle,int shortCycle,int signalPeriods,int panelID)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                IndicatorMACD indicatorMACD = new IndicatorMACD();
                indicatorMACD.LongCycle = longCycle;
                indicatorMACD.ShortCycle = shortCycle;
                indicatorMACD.SignalPeriods = signalPeriods;
                string zeroLine = CommonClass.GetGuid();
                indicatorMACD.HsMACD = AddHistogram(macd, macd, null, panelID);
                SetHistogramStyle(macd, Color.Red, Color.SkyBlue, 1, true);
                indicatorMACD.TlsDiff = AddTrendLine(diff, diff, panelID);
                SetTrendLineStyle(diff, Color.White, Color.White, 1, DashStyle.Solid);
                indicatorMACD.TlsDea = AddTrendLine(dea, dea, panelID);
                SetTrendLineStyle(dea, Color.Yellow, Color.Yellow, 1, DashStyle.Solid);
                indicatorMACD.Close = close;
                this.dtAllMsg.Columns.Add(indicatorMACD.LongCycleEMA);
                this.dtAllMsg.Columns.Add(indicatorMACD.ShortCycleEMA);
                indicatorMACD.DataSource = this.dtAllMsg;
                this.indMacdList.Add(indicatorMACD);
                return indicatorMACD;
            }
            return null;
        }

        /// <summary>
        /// 追加BOLL线
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <param name="close"></param>
        /// <param name="periods"></param>
        /// <param name="standardDeviations"></param>
        /// <param name="panelID"></param>
        /// <returns></returns>
        public IndicatorBollingerBands AddBollingerBands(string mid, string up, string down, string close,
            int periods, int standardDeviations, int panelID)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                IndicatorBollingerBands indBoll = new IndicatorBollingerBands();
                indBoll.TlsM = AddTrendLine(mid, mid, panelID);
                SetTrendLineStyle(mid, Color.White, Color.White, 1, DashStyle.Solid);
                indBoll.TlsU = AddTrendLine(up, up, panelID);
                SetTrendLineStyle(up, Color.Yellow, Color.Yellow, 1, DashStyle.Solid);
                indBoll.TlsD = AddTrendLine(down, down, panelID);
                SetTrendLineStyle(down, Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 255), 1, DashStyle.Solid);
                indBoll.Close = close;
                indBoll.DataSource = this.dtAllMsg;
                indBoll.Periods = periods;
                indBoll.StandardDeviations = standardDeviations;
                //this.indBollList.Add(indBoll);
                return indBoll;
            }
            return null;
        }

        /// <summary>
        /// 重置十字线穿越的字段
        /// </summary>
        public void ResetCrossOverRecord()
        {
            if (this.dtAllMsg.Rows.Count >= this.GetMaxVisibleRecord())
            {
                if (this.dtAllMsg.Rows.Count > 0 && showCrossHair)
                {
                    if (this.crossOverIndex < firstVisibleRecord - 1)
                    {
                        this.crossOverIndex = firstVisibleRecord - 1;
                    }
                    if (this.crossOverIndex > LastVisibleRecord - 1)
                    {
                        this.crossOverIndex = LastVisibleRecord - 1;
                    }
                }
            }
        }

        /// <summary>
        /// 向左滚动
        /// </summary>
        /// <param name="step"></param>
        private void ScrollLeft(int step)
        {
            if (this.dtAllMsg.Rows.Count > 1 && firstVisibleRecord > 1)
            {
                if (this.dtAllMsg.Rows.Count > GetMaxVisibleRecord())
                {
                    if (firstVisibleRecord - step >= 1)
                    {
                        firstVisibleRecord = firstVisibleRecord - step;
                        LastVisibleRecord = LastVisibleRecord - step;
                    }
                    else
                    {
                        LastVisibleRecord = LastVisibleRecord - firstVisibleRecord;
                        firstVisibleRecord = 1;
                    }
                }
            }
        }

        /// <summary>
        /// 向右滚动
        /// </summary>
        /// <param name="step"></param>
        private void ScrollRight(int step)
        {
            if (this.dtAllMsg.Rows.Count > 1 && LastVisibleRecord < this.dtAllMsg.Rows.Count)
            {
                if (this.dtAllMsg.Rows.Count > GetMaxVisibleRecord())
                {
                    if (LastVisibleRecord + step > this.dtAllMsg.Rows.Count)
                    {
                        firstVisibleRecord = firstVisibleRecord + (this.dtAllMsg.Rows.Count - LastVisibleRecord);
                        LastVisibleRecord = this.dtAllMsg.Rows.Count;
                    }
                    else
                    {
                        firstVisibleRecord = firstVisibleRecord + step;
                        LastVisibleRecord = LastVisibleRecord + step;
                    }
                }
            }
        }

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="step"></param>
        private void ZoomIn(int step)
        {
            if (this.axisSpace < 50)
            {
                int oriMax = GetMaxVisibleRecord();
                bool dealWith = false;
                if (this.dtAllMsg.Rows.Count < oriMax)
                {
                    dealWith = true;
                }
                this.axisSpace = this.axisSpace + 1;
                int nowMax = GetMaxVisibleRecord();
                int subRecord = oriMax - nowMax;
                if (this.dtAllMsg.Rows.Count >= nowMax)
                {
                    if (dealWith)
                    {
                        firstVisibleRecord = 1;
                        LastVisibleRecord = nowMax;
                    }
                    else
                    {
                        this.firstVisibleRecord = this.firstVisibleRecord + subRecord / 2;
                        this.LastVisibleRecord = this.firstVisibleRecord + nowMax - 1;
                    }
                }
            }
        }

        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="step"></param>
        private void ZoomOut(int step)
        {
            if (this.axisSpace > 1)
            {
                int oriMax = GetMaxVisibleRecord();
                this.axisSpace = this.axisSpace - 1;
                int nowMax = GetMaxVisibleRecord();
                int subRecord = nowMax - oriMax;
                int f = this.firstVisibleRecord - subRecord / 2;
                int l = this.LastVisibleRecord + (subRecord - subRecord / 2);
                if (f < 1 && l > this.dtAllMsg.Rows.Count)
                {
                    firstVisibleRecord = 1;
                    LastVisibleRecord = this.dtAllMsg.Rows.Count;
                }
                else if (f < 1)
                {
                    LastVisibleRecord = LastVisibleRecord + subRecord - (firstVisibleRecord - 1);
                    firstVisibleRecord = 1;
                    if (LastVisibleRecord > this.dtAllMsg.Rows.Count)
                    {
                        LastVisibleRecord = this.dtAllMsg.Rows.Count;
                    }
                }
                else if (l > this.dtAllMsg.Rows.Count)
                {
                    firstVisibleRecord = firstVisibleRecord - (subRecord - (this.dtAllMsg.Rows.Count - this.LastVisibleRecord));
                    LastVisibleRecord = this.dtAllMsg.Rows.Count;
                    if (firstVisibleRecord < 1)
                    {
                        firstVisibleRecord = 1;
                    }
                }
                else
                {
                    this.firstVisibleRecord = f;
                    this.LastVisibleRecord = l;
                }
            }
        }

        /// <summary>
        /// 设置X轴所使用的字段
        /// </summary>
        /// <returns></returns>
        public void SetXScaleField(string field)
        {
            if (this.timekeyField == null || this.timekeyField.Length == 0)
            {
                this.timekeyField = field;
                DataColumn dcTimekey = new DataColumn(timekeyField);
                this.dtAllMsg.Columns.Add(dcTimekey);
                this.dtAllMsg.PrimaryKey = new DataColumn[] { dcTimekey };
            }
        }

        /// <summary>
        /// 设置Y轴所使用的字段
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="field"></param>
        public void SetYScaleField(int panelID, string[] field)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                chartPanel.YScaleField.Clear();
                foreach (string ch in field)
                {
                    chartPanel.YScaleField.Add(ch);
                }
            }
        }

        /// <summary>
        /// 刷新图像
        /// </summary>
        public void RefreshGraph()
        {
            InitFirstAndLastVisibleRecord();
            setVisibleExtremeValue();
            ResetCrossOverRecord();
            DrawGraph();
        }

        /// <summary>
        /// 清除图像
        /// </summary>
        public void ClearGraph()
        {
            this.dtAllMsg.Clear();
            this.firstVisibleRecord = 0;
            this.LastVisibleRecord = 0;
            showCrossHair = false;
            RefreshGraph();
        }

        /// <summary>
        /// 自动设置首先可见和最后可见的记录号
        /// </summary>
        public void InitFirstAndLastVisibleRecord()
        {
            if (this.dtAllMsg.Rows.Count == 0)
            {
                this.firstVisibleRecord = 0;
                this.LastVisibleRecord = 0;
            }
            else
            {
                int maxVisibleRecord = GetMaxVisibleRecord();
                if (this.dtAllMsg.Rows.Count < maxVisibleRecord)
                {
                    firstVisibleRecord = 1;
                    this.LastVisibleRecord = this.dtAllMsg.Rows.Count;
                }
                else
                {
                    if (firstVisibleRecord != 0 && LastVisibleRecord != 0 && !lastRecordIsVisible)
                    {
                        DataRow dr = this.dtAllMsg.Rows.Find(lastVisibleTimeKey);
                        if (dr != null)
                        {
                            int index = this.dtAllMsg.Rows.IndexOf(dr);
                            LastVisibleRecord = index + 1;
                            firstVisibleRecord = LastVisibleRecord - maxVisibleRecord + 1;
                            if (firstVisibleRecord < 1)
                            {
                                firstVisibleRecord = 1;
                            }
                        }
                    }
                    else
                    {
                        LastVisibleRecord = this.dtAllMsg.Rows.Count;
                        firstVisibleRecord = LastVisibleRecord - maxVisibleRecord + 1;
                        if (firstVisibleRecord < 1)
                        {
                            firstVisibleRecord = 1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置可见部分的最大值和最小值
        /// </summary>
        private void setVisibleExtremeValue()
        {
            if (GetWorkSpaceX() > 0)
            {
                int firstR = firstVisibleRecord - 1;
                int lastR = LastVisibleRecord;
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    Dictionary<CandleSeries, List<object[]>> kValueList = new Dictionary<CandleSeries, List<object[]>>();
                    List<double> valueList = new List<double>();
                    if (this.dtAllMsg.Rows.Count > 0)
                    {
                        for (int i = firstR; i < lastR; i++)
                        {
                            string timeKey = this.dtAllMsg.Rows[i][timekeyField].ToString();
                            //获取数据
                            DataRow dr = dtAllMsg.Rows[i];
                            foreach (string field in chartPanel.YScaleField)
                            {
                                double fieldValue = 0;
                                Double.TryParse(dr[field].ToString(), out fieldValue);
                                valueList.Add(fieldValue);
                            }
                            //K线柱的最大和最小值对应的记录号
                            foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                            {
                                if (!kValueList.ContainsKey(cs))
                                {
                                    kValueList.Add(cs, new List<object[]>());
                                }
                                double open = 0;
                                Double.TryParse(dr[cs.OpenField].ToString(), out open);
                                double high = 0;
                                Double.TryParse(dr[cs.HighField].ToString(), out high);
                                double low = 0;
                                Double.TryParse(dr[cs.LowField].ToString(), out low);
                                double close = 0;
                                Double.TryParse(dr[cs.CloseField].ToString(), out close);
                                kValueList[cs].Add(new object[] { i, open });
                                kValueList[cs].Add(new object[] { i, high });
                                kValueList[cs].Add(new object[] { i, low });
                                kValueList[cs].Add(new object[] { i, close });
                                if (chartPanel.YScaleField.Count == 0)
                                {
                                    valueList.Add(open);
                                    valueList.Add(high);
                                    valueList.Add(low);
                                    valueList.Add(close);
                                }
                                else
                                {
                                    if (chartPanel.YScaleField.Contains(cs.OpenField))
                                    {
                                        valueList.Add(open);
                                    }
                                    if (chartPanel.YScaleField.Contains(cs.HighField))
                                    {
                                        valueList.Add(high);
                                    }
                                    if (chartPanel.YScaleField.Contains(cs.LowField))
                                    {
                                        valueList.Add(low);
                                    }
                                    if (chartPanel.YScaleField.Contains(cs.CloseField))
                                    {
                                        valueList.Add(close);
                                    }
                                }
                            }
                            foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                            {
                                double volume = 0;
                                Double.TryParse(dr[hs.Field].ToString(), out volume);
                                if (chartPanel.YScaleField.Count == 0)
                                {
                                    valueList.Add(0);
                                    valueList.Add(volume);
                                }
                                else
                                {
                                    if (chartPanel.YScaleField.Contains(hs.Field))
                                    {
                                        valueList.Add(0);
                                        valueList.Add(volume);
                                    }
                                }
                            }
                            foreach (TrendLineSeries tls in chartPanel.TrendLineSeriesList)
                            {
                                double lineValue = 0;
                                Double.TryParse(dr[tls.Field].ToString(), out lineValue);
                                if (chartPanel.YScaleField.Count == 0)
                                {
                                    valueList.Add(lineValue);
                                }
                                else
                                {

                                    if (chartPanel.YScaleField.Contains(tls.Field))
                                    {
                                        valueList.Add(lineValue);
                                    }
                                }
                            }
                        }
                    }
                    chartPanel.MaxValue = CommonClass.GetHighValue(valueList);
                    chartPanel.MinValue = CommonClass.GetLowValue(valueList);
                    foreach (CandleSeries cs in kValueList.Keys)
                    {
                        cs.MaxRecord = CommonClass.GetHighRecord(kValueList[cs]);
                        cs.MinRecord = CommonClass.GetLoweRecord(kValueList[cs]);
                    }
                }
            }
        }

        /// <summary>
        /// 对指定指标进行计算
        /// </summary>
        /// <param name="indicator"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        public void CalcutaIndicator(object indicator,int startIndex,int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                DataRow dr = this.dtAllMsg.Rows[i];
                DateTime dateTime = CommonClass.GetDateTimeByTimeKey(dr[timekeyField].ToString());
                //平滑移动平均线
                if (indicator is IndicatorSimpleMovingAverage)
                {
                    IndicatorSimpleMovingAverage indSMA = indicator as IndicatorSimpleMovingAverage;
                    double smaValue = indSMA.Calculate(i);
                    if (smaValue != CommonClass.NULL)
                    {
                        SetValue(indSMA.TrendLineSeries.Field, smaValue, dateTime);
                    }
                }
                //指数移动平均线
                else if (indicator is IndicatorExponentialMovingAverage)
                {
                    IndicatorExponentialMovingAverage indEMA = indicator as IndicatorExponentialMovingAverage;
                    double emaValue = indEMA.Calculate(i);
                    if (emaValue != CommonClass.NULL)
                    {
                        SetValue(indEMA.TrendLineSeries.Field, emaValue, dateTime);
                    }
                }
                //随机指标
                else if (indicator is IndicatorStochasticOscillator)
                {
                    IndicatorStochasticOscillator indSO = indicator as IndicatorStochasticOscillator;
                    if (!(dr[indSO.High] is DBNull) && !(dr[indSO.Low] is DBNull) && !(dr[indSO.Close] is DBNull))
                    {
                        double[] kdj = indSO.Calculate(i);
                        SetValue(indSO.TlsK.Field, kdj[0], dateTime);
                        SetValue(indSO.TlsD.Field, kdj[1], dateTime);
                        SetValue(indSO.TlsJ.Field, kdj[2], dateTime);
                    }
                }
                //指数平滑异同移动平均线
                else if (indicator is IndicatorMACD)
                {
                    IndicatorMACD indMacd = indicator as IndicatorMACD;
                    if (!(dr[indMacd.Close] is DBNull))
                    {
                        double shortEMA = CommonClass.CalculateExponentialMovingAvg(indMacd.ShortCycleEMA, indMacd.Close, indMacd.ShortCycle, indMacd.DataSource, i);
                        double longEMA = CommonClass.CalculateExponentialMovingAvg(indMacd.LongCycleEMA, indMacd.Close, indMacd.LongCycle, indMacd.DataSource, i);
                        SetValue(indMacd.ShortCycleEMA, shortEMA, dateTime);
                        SetValue(indMacd.LongCycleEMA, longEMA, dateTime);
                        double[] macdValue = indMacd.Calulate(i);
                        SetValue(indMacd.HsMACD.Field, macdValue[0], dateTime);
                        SetValue(indMacd.TlsDiff.Field, macdValue[1], dateTime);
                        SetValue(indMacd.TlsDea.Field, macdValue[2], dateTime);
                    }
                }
                //布林带
                else if (indicator is IndicatorBollingerBands)
                {
                    IndicatorBollingerBands indBoll = indicator as IndicatorBollingerBands;
                    double[] bollValue = indBoll.Calculate(i);
                    SetValue(indBoll.TlsM.Field, bollValue[0], dateTime);
                    SetValue(indBoll.TlsU.Field, bollValue[1], dateTime);
                    SetValue(indBoll.TlsD.Field, bollValue[2], dateTime);
                }
            }
        }

        /// <summary>
        /// 设置值，自动排序
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="dateTime"></param>
        public void SetValue(string fieldName, object value, DateTime dateTime)
        {
            if (!dtAllMsg.Columns.Contains(fieldName))
            {
                dtAllMsg.Columns.Add(fieldName);
            }
            string timeKey = CommonClass.GetTimeKey(dateTime);
            DataRow dr = dtAllMsg.Rows.Find(timeKey);
            if (dr != null)
            {
                dr[fieldName] = value;
            }
            else
            {
                DateTime dtEnd = DateTime.Now;
                bool flag = false;
                if (this.dtAllMsg.Rows.Count > 0)
                {
                    flag = true;
                    dtEnd = CommonClass.GetDateTimeByTimeKey(this.dtAllMsg.Rows[this.dtAllMsg.Rows.Count - 1][timekeyField].ToString());
                }
                dr = dtAllMsg.NewRow();
                dr[timekeyField] = timeKey;
                dr[fieldName] = value;
                this.dtAllMsg.Rows.Add(dr);
                if (flag && dateTime < dtEnd)
                {
                    DataView dv = dtAllMsg.DefaultView;
                    dv.Sort = timekeyField + " ASC";
                    this.dtAllMsg = dv.ToTable();
                    this.dtAllMsg.PrimaryKey = new DataColumn[] { this.dtAllMsg.Columns[timekeyField] };
                }
            }
            //指标计算
            int r = this.dtAllMsg.Rows.IndexOf(dr);
            //简单移动平均线
            bool ind_flag = false;
            for (int m = r; m < this.dtAllMsg.Rows.Count; m++)
            {
                if (this.indSimpleMovingAverageList.Count > 0)
                {
                    foreach (IndicatorSimpleMovingAverage indSMA in this.indSimpleMovingAverageList)
                    {
                        if (indSMA.Target == fieldName)
                        {
                            ind_flag = true;
                            CalcutaIndicator(indSMA, m, m);
                        }
                    }
                }
                //指数移动平均线
                if (this.indExponentialMovingAverageList.Count > 0)
                {
                    foreach (IndicatorExponentialMovingAverage indEMA in this.indExponentialMovingAverageList)
                    {
                        if (indEMA.Target == fieldName)
                        {
                            ind_flag = true;
                            CalcutaIndicator(indEMA, m, m);
                        }
                    }
                }
                //随机指标
                if (this.indStochasticOscillatorList.Count > 0)
                {
                    foreach (IndicatorStochasticOscillator indSO in this.indStochasticOscillatorList)
                    {
                        if (fieldName == indSO.High || fieldName == indSO.Low || fieldName == indSO.Close)
                        {
                            ind_flag = true;
                            CalcutaIndicator(indSO, m, m);
                        }
                    }
                }
                //MACD指标
                if (this.indMacdList.Count > 0)
                {
                    foreach (IndicatorMACD indMacd in this.indMacdList)
                    {
                        if (fieldName == indMacd.Close)
                        {
                            ind_flag = true;
                            CalcutaIndicator(indMacd, m, m);
                        }
                    }
                }
                //BOLL线
                if (this.indBollList.Count > 0)
                {
                    foreach (IndicatorBollingerBands indBOLL in this.indBollList)
                    {
                        if (fieldName == indBOLL.Close)
                        {
                            ind_flag = true;
                            CalcutaIndicator(indBOLL, m, m);
                        }
                    }
                }
                if (!ind_flag)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 获取当前鼠标所在Panel的ID
        /// </summary>
        /// <returns></returns>
        public int GetCurrentPanel()
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                if (crossHair_y >= chartPanel.RectPanel.Y && crossHair_y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height)
                {
                    return chartPanel.PanelID;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取某一值的纵坐标
        /// </summary>
        /// <param name="chartPanel"></param>
        /// <param name="value"></param>
        public float GetValueYPixel(ChartPanel chartPanel, double chartValue)
        {
            return Convert.ToSingle((chartPanel.MaxValue - chartValue) / (chartPanel.MaxValue - chartPanel.MinValue) * GetWorkSpaceY(chartPanel.PanelID)
                + chartPanel.TitleHeight + chartPanel.RectPanel.Y);
        }

        /// <summary>
        /// 获取屏幕内显示的记录数
        /// </summary>
        /// <returns></returns>
        public int GetMaxVisibleRecord()
        {
            if (this.axisSpace == 0)
            {
                return this.GetWorkSpaceX();
            }
            else
            {
                return this.GetWorkSpaceX() / this.AxisSpace;
            }
        }

        /// <summary>
        /// 获取十字线的坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Point GetCrossHairPoint()
        {
            int x = this.PointToClient(MousePosition).X;
            int y = this.PointToClient(MousePosition).Y;
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                if (y >= chartPanel.RectPanel.Y && y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height)
                {
                    if (y > this.Height - chartPanel.ScaleX_Height)
                    {
                        y = this.Height - chartPanel.ScaleX_Height;
                    }
                    if (showLeftScale)
                    {
                        if (x < leftPixSpace)
                        {
                            x = leftPixSpace;
                        }
                    }
                    if (showRightScale)
                    {
                        if (x > this.Width - rightPixSpace)
                        {
                            x = this.Width - rightPixSpace;
                        }
                    }
                    break;
                }
            }
            return new Point(x, y);
        }

        /// <summary>
        /// 获取鼠标选中的记录索引,从0开始
        /// </summary>
        /// <returns></returns>
        public int GetCrossOverIndex()
        {
            Point mousePoint = GetCrossHairPoint();
            return (mousePoint.X - this.LeftPixSpace) / this.AxisSpace + this.firstVisibleRecord - 1;
        }

        /// <summary>
        /// 获取鼠标对应的值
        /// </summary>
        /// <returns></returns>
        public double GetCurrentValue()
        {
            Point mouseP = GetCrossHairPoint();
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                if (crossHair_y >= chartPanel.RectPanel.Y && crossHair_y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height)
                {
                    if (GetWorkSpaceY(chartPanel.PanelID) > 0)
                    {
                        double everyPointValue = (chartPanel.MaxValue - chartPanel.MinValue) / GetWorkSpaceY(chartPanel.PanelID);
                        return chartPanel.MaxValue - (crossHair_y - chartPanel.TitleHeight - chartPanel.RectPanel.Y) * everyPointValue;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取工作区的横向长度
        /// </summary>
        /// <returns></returns>
        public int GetWorkSpaceX()
        {
            return this.Width - this.LeftPixSpace - this.RightPixSpace;
        }

        /// <summary>
        /// 获取工作区的纵向长度
        /// </summary>
        /// <returns></returns>
        public int GetWorkSpaceY(int panelID)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                return dicChartPanel[panelID].RectPanel.Height - dicChartPanel[panelID].ScaleX_Height - (int)dicChartPanel[panelID].TitleHeight;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 绘图
        /// <summary>
        /// 画边线
        /// </summary>
        /// <param name="g"></param>
        private void DrawBackGround(Graphics g)
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                Rectangle drawRect = new Rectangle(0, chartPanel.RectPanel.Y, this.Width - 2, chartPanel.RectPanel.Height);
                g.FillRectangle(chartPanel.BgBrush, drawRect);
                g.DrawRectangle(chartPanel.PanelBorder_Pen, drawRect);
            }
        }

        /// <summary>
        /// 画十字线
        /// </summary>
        /// <param name="g"></param>
        public void DrawCrossHair(Graphics g)
        {
            Point mousePoint = GetCrossHairPoint();
            if (crossHair_y != -1)
            {
                mousePoint.Y = crossHair_y;
            }
            if (showCrossHair)
            {
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    if (mousePoint.Y >= chartPanel.RectPanel.Y +chartPanel.TitleHeight && mousePoint.Y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height)
                    {
                        //横向的线
                        g.DrawLine(chartPanel.CrossHair_Pen, LeftPixSpace, mousePoint.Y, this.Width - RightPixSpace, mousePoint.Y);
                    }
                    int verticalX = leftPixSpace + axisSpace * (crossOverIndex - firstVisibleRecord + 1) + axisSpace / 2;
                    //纵向的线
                    SizeF titleHeight=g.MeasureString(" ",chartPanel.TitleFont);
                    if (this.crossOverIndex == -1 || this.crossOverIndex < firstVisibleRecord - 1 || this.crossOverIndex > LastVisibleRecord - 1)
                    {
                        g.DrawLine(chartPanel.CrossHair_Pen, verticalX, chartPanel.RectPanel.Y + 5 + titleHeight.Height, verticalX, chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height);
                        continue;
                    }
                    else
                    {
                        float y = chartPanel.RectPanel.Y + 5 + titleHeight.Height;
                        if (this.dtAllMsg.Columns.Contains(chartPanel.InfoBombField))
                        {

                            if (!(this.dtAllMsg.Rows[crossOverIndex][chartPanel.InfoBombField] is DBNull)
                                && this.dtAllMsg.Rows[crossOverIndex][chartPanel.InfoBombField].ToString() != string.Empty)
                            {
                                y = chartPanel.RectPanel.Y + 10 + titleHeight.Height;
                            }
                        }
                        g.DrawLine(chartPanel.CrossHair_Pen, verticalX, y, verticalX, chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height);
                    }
                    SizeF xTipFontSize = g.MeasureString(CommonClass.GetCalenderFormatTimeKey(this.dtAllMsg.Rows[this.crossOverIndex][timekeyField].ToString(), chartPanel.Interval), chartPanel.XTipFont);
                    //X轴提示框及文字
                    RectangleF xRt = new RectangleF(verticalX - xTipFontSize.Width / 2 - 2, chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height + 1, xTipFontSize.Width + 4, xTipFontSize.Height + 2);
                    GraphicsPath gpXRT = CommonClass.GetRoundRectangle(1, xRt);
                    g.FillPath(chartPanel.Xtip_Brush, gpXRT);
                    g.DrawPath(chartPanel.XTipFont_Pen, gpXRT);
                    gpXRT.Dispose();
                    g.DrawString(CommonClass.GetCalenderFormatTimeKey(this.dtAllMsg.Rows[this.crossOverIndex][timekeyField].ToString(), chartPanel.Interval), chartPanel.XTipFont,
                       chartPanel.XTipFont_Brush, xRt);
                    if (mousePoint.Y >= chartPanel.RectPanel.Y+chartPanel.TitleHeight && mousePoint.Y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height)
                    {
                        double value = GetCurrentValue();
                        //显示左侧Y轴的提示框及文字
                        if (showLeftScale)
                        {
                            string leftValue = CommonClass.GetValueByDigit(value, chartPanel.Digit);
                            SizeF leftYTipFontSize = g.MeasureString(leftValue, chartPanel.LeftyTipFont);
                            RectangleF lRt = new RectangleF(this.LeftPixSpace - leftYTipFontSize.Width - 4, mousePoint.Y - leftYTipFontSize.Height / 2, leftYTipFontSize.Width + 4, leftYTipFontSize.Height + 1);
                            GraphicsPath gpLRT = CommonClass.GetRoundRectangle(1, lRt);
                            g.FillPath(chartPanel.LeftyTip_Brush, gpLRT);
                            g.DrawPath(chartPanel.LeftTipFont_Pen, gpLRT);
                            gpLRT.Dispose();
                            g.DrawString(leftValue, chartPanel.LeftyTipFont, chartPanel.LeftyTipFont_Brush, lRt);
                        }
                        //显示右侧Y轴的提示框及文字
                        if (ShowRightScale)
                        {
                            string rightValue = CommonClass.GetValueByDigit(value, chartPanel.Digit);
                            SizeF rightYTipFontSize = g.MeasureString(rightValue, chartPanel.RightyTipFont);
                            RectangleF rRt = new RectangleF(this.Width - RightPixSpace + 1, mousePoint.Y - rightYTipFontSize.Height / 2, rightYTipFontSize.Width + 4, rightYTipFontSize.Height + 1);
                            GraphicsPath gpRRT = CommonClass.GetRoundRectangle(2, rRt);
                            g.FillPath(chartPanel.RightyTip_Brush, gpRRT);
                            g.DrawPath(chartPanel.RightyTipFont_Pen, gpRRT);
                            gpRRT.Dispose();
                            g.DrawString(rightValue, chartPanel.RightyTipFont, chartPanel.RightyTipFont_Brush, rRt);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 画标题
        /// </summary>
        /// <param name="g"></param>
        private void DrawTitle(Graphics g)
        {
            objectRectDic.Clear();
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                float titleLeftPadding = this.LeftPixSpace;
                //创建字符串
                Font titleFont = chartPanel.TitleFont;
                int rightPadding = this.Width - this.rightPixSpace - 2;
                if (canDragSeries && this.dtAllMsg.Rows.Count > 0)
                {
                    //画拖动标记
                    foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                    {
                        SizeF sizeK = new SizeF(15f, 16f);
                        RectangleF rectCs = new RectangleF(rightPadding - sizeK.Width, chartPanel.RectPanel.Y + 2, sizeK.Width, sizeK.Height);
                        if (!showCrossHair && cs.HasSelect)
                        {
                            g.FillRectangle(cs.UpLine_TransparentBrush, rectCs);
                            g.DrawRectangle(cs.UpLine_Pen, rectCs.X, rectCs.Y, rectCs.Width, rectCs.Height);
                        }
                        g.DrawLine(cs.DownLine_Pen, rectCs.X + 4, rectCs.Y + 6, rectCs.X + 4, rectCs.Bottom - 2);
                        g.DrawLine(cs.UpLine_Pen, rectCs.X + 9, rectCs.Y + 2, rectCs.X + 9, rectCs.Bottom - 4);
                        g.FillRectangle(cs.DownLine_Brush, new RectangleF(rectCs.X + 3, rectCs.Y + 8, 3, 5));
                        g.FillRectangle(cs.UpLine_Brush, new RectangleF(rectCs.X + 8, rectCs.Y + 4, 3, 5));
                        rightPadding -= (int)sizeK.Width + 2;
                        objectRectDic[rectCs] = cs;
                    }
                    foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                    {
                        SizeF sizeK = new SizeF(15f, 16f);
                        RectangleF rectCs = new RectangleF(rightPadding - sizeK.Width, chartPanel.RectPanel.Y + 2, sizeK.Width, sizeK.Height);
                        float lineWidth = hs.Up_Pen.Width;
                        hs.Up_Pen.Width = 1f;
                        if (!showCrossHair && hs.HasSelect)
                        {
                            g.FillRectangle(hs.Up_TransparentBrush, rectCs);
                            g.DrawRectangle(hs.Up_Pen, rectCs.X, rectCs.Y, rectCs.Width, rectCs.Height);
                        }
                        g.FillRectangle(hs.Up_LineBrush, rectCs.X + 1, rectCs.Y + 10, 3, rectCs.Bottom - rectCs.Y - 11);
                        g.FillRectangle(hs.Up_LineBrush, rectCs.X + 6, rectCs.Y + 3, 3, rectCs.Bottom - rectCs.Y - 4);
                        g.FillRectangle(hs.Up_LineBrush, rectCs.X + 11, rectCs.Y + 8, 3, rectCs.Bottom - rectCs.Y - 9);
                        rightPadding -= (int)sizeK.Width + 2;
                        objectRectDic[rectCs] = hs;
                        hs.Up_Pen.Width = lineWidth;
                    }
                    foreach (TrendLineSeries tls in chartPanel.TrendLineSeriesList)
                    {
                        SizeF sizeK = new SizeF(15f, 16f);
                        float lineWidth = tls.Up_LinePen.Width;
                        tls.Up_LinePen.Width = 1f;
                        RectangleF rectCs = new RectangleF(rightPadding - sizeK.Width, chartPanel.RectPanel.Y + 2, sizeK.Width, sizeK.Height);
                        if (!showCrossHair && tls.HasSelect)
                        {
                            g.FillRectangle(tls.TransParentLineBrush, rectCs);
                            g.DrawRectangle(tls.Up_LinePen, rectCs.X, rectCs.Y, rectCs.Width, rectCs.Height);
                        }
                        g.DrawLine(tls.Up_LinePen, rectCs.X + 2, rectCs.Y + 5, rectCs.X + 12, rectCs.Y + 1);
                        g.DrawLine(tls.Up_LinePen, rectCs.X + 2, rectCs.Y + 10, rectCs.X + 12, rectCs.Y + 6);
                        g.DrawLine(tls.Up_LinePen, rectCs.X + 2, rectCs.Y + 15, rectCs.X + 12, rectCs.Y + 11);
                        rightPadding -= (int)sizeK.Width + 2;
                        objectRectDic[rectCs] = tls;
                        tls.Up_LinePen.Width = lineWidth;
                    }
                }
                //画标题下方的线
                SizeF sizeTitle = g.MeasureString(" ", titleFont);
                g.DrawLine(chartPanel.Grid_Pen, this.leftPixSpace, chartPanel.RectPanel.Y + 5 + sizeTitle.Height,
                    this.Width - this.rightPixSpace, chartPanel.RectPanel.Y + 5 + sizeTitle.Height);
                //画标题
                if (chartPanel.UserDefinedTitle)
                {
                    StringFormat sf = new StringFormat();
                    foreach (TitleField titleField in chartPanel.TitleFieldList)
                    {
                        string field = titleField.RelateSeriesField;
                        Color fieldColor = titleField.FieldColor;
                        string drawTitle = titleField.DisplayTitle;
                        SizeF sizeF = g.MeasureString(drawTitle, titleFont, 1000, sf);
                        Rectangle titleRect = new Rectangle((int)titleLeftPadding, chartPanel.RectPanel.Y + 2, (int)sizeF.Width, (int)sizeF.Height);
                        RectangleF drawRect = new RectangleF(titleLeftPadding, chartPanel.RectPanel.Y + 2, sizeF.Width, sizeF.Height);
                        if (titleLeftPadding + sizeF.Width <= rightPadding)
                        {
                            g.DrawString(drawTitle, titleFont, titleField.FieldBrush, drawRect);
                            if (canDragSeries)
                            {
                                foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                                {
                                    if (cs.OpenField == field || cs.HighField == field
                                        || cs.LowField == field || cs.CloseField == field)
                                    {
                                        objectRectDic[titleRect] = cs;
                                    }
                                }
                                foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                                {
                                    if (hs.Field == field)
                                    {
                                        objectRectDic[titleRect] = hs;
                                    }
                                }
                                foreach (TrendLineSeries tls in chartPanel.TrendLineSeriesList)
                                {
                                    if (tls.Field == field)
                                    {
                                        objectRectDic[titleRect] = tls;
                                    }
                                }
                            }
                        }
                        titleLeftPadding += sizeF.Width;
                    }
                    sf.Dispose();
                }
                else
                {
                    SizeF layNameSize = g.MeasureString(chartPanel.PanelTitle, titleFont);
                    if (titleLeftPadding + layNameSize.Width <= this.Width - RightPixSpace)
                    {
                        g.DrawString(chartPanel.PanelTitle, titleFont, chartPanel.TitleFont_Brush, new PointF(titleLeftPadding, chartPanel.RectPanel.Y + 2));
                    }
                    titleLeftPadding += layNameSize.Width;
                    DataRow dr = null;
                    if (this.dtAllMsg.Rows.Count > 0 && LastVisibleRecord > 0 & processBarValue == 0)
                    {
                        int displayIndex = LastVisibleRecord - 1;
                        if (showCrossHair)
                        {
                            if (crossOverIndex <= LastVisibleRecord)
                            {
                                displayIndex = crossOverIndex;
                            }
                        }
                        if (displayIndex >= 0 && displayIndex < this.dtAllMsg.Rows.Count)
                        {
                            dr = this.dtAllMsg.Rows[displayIndex];
                        }
                        foreach (DataColumn dataColumn in this.dtAllMsg.Columns)
                        {
                            string condition = dataColumn.ColumnName;
                            string displayName = null;
                            bool drawFlag = false;
                            Color titleColor = Color.White;
                            object selectedObj = null;
                            foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                            {
                                if (cs.DisplayTitleField)
                                {
                                    if (condition == cs.OpenField)
                                    {
                                        titleColor = cs.OpenTitleColor;
                                        drawFlag = true;
                                        selectedObj = cs;
                                        goto CompleteSet;
                                    }
                                    else if (condition == cs.HighField)
                                    {
                                        titleColor = cs.HighTitleColor;
                                        drawFlag = true;
                                        selectedObj = cs;
                                        goto CompleteSet;
                                    }
                                    else if (condition == cs.LowField)
                                    {
                                        titleColor = cs.LowTitleColor;
                                        drawFlag = true;
                                        selectedObj = cs;
                                        goto CompleteSet;
                                    }
                                    else if (condition == cs.CloseField)
                                    {
                                        titleColor = cs.CloseTitleColor;
                                        drawFlag = true;
                                        selectedObj = cs;
                                        goto CompleteSet;
                                    }
                                }
                            }
                            if (chartPanel.TrendLineSeriesList != null && chartPanel.TrendLineSeriesList.Count > 0)
                            {
                                foreach (TrendLineSeries ls in chartPanel.TrendLineSeriesList)
                                {
                                    if (ls.Field == condition && ls.DisplayName != null)
                                    {
                                        titleColor = ls.Up_LineColor;
                                        drawFlag = true;
                                        displayName = ls.DisplayName;
                                        selectedObj = ls;
                                        goto CompleteSet;
                                    }
                                }
                            }
                            if (chartPanel.HistoramSeriesList != null && chartPanel.HistoramSeriesList.Count > 0)
                            {
                                foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                                {
                                    if (hs.Field == condition && hs.DisplayName != null)
                                    {
                                        titleColor = hs.Down_lineColor;
                                        drawFlag = true;
                                        displayName = hs.DisplayName;
                                        selectedObj = hs;
                                        goto CompleteSet;
                                    }
                                }
                            }
                        CompleteSet: ;
                            if (drawFlag)
                            {
                                string showTitle = condition + " ";
                                if (displayName != null)
                                {
                                    showTitle = displayName + " ";
                                    if (displayName == string.Empty)
                                    {
                                        showTitle = displayName;
                                    }
                                }
                                if (dr != null)
                                {
                                    if (dtAllMsg.Columns.Contains(condition))
                                    {
                                        double value = 0;
                                        double.TryParse(dr[condition].ToString(), out value);
                                        showTitle += CommonClass.GetValueByDigit(value, chartPanel.Digit);
                                    }
                                }
                                SizeF conditionSize = g.MeasureString(showTitle, titleFont);
                                if (titleLeftPadding + conditionSize.Width <= rightPadding)
                                {
                                    Brush titleBrush = new SolidBrush(titleColor);
                                    g.DrawString(showTitle, titleFont, titleBrush, new PointF(titleLeftPadding, chartPanel.RectPanel.Y + 2));
                                    titleBrush.Dispose();
                                    if (selectedObj != null)
                                    {
                                        RectangleF titleRect = new RectangleF(titleLeftPadding, chartPanel.RectPanel.Y + 2, conditionSize.Width, conditionSize.Height);
                                        objectRectDic[titleRect] = selectedObj;
                                    }
                                }
                                titleLeftPadding += conditionSize.Width;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 画坐标轴
        /// </summary>
        /// <param name="g"></param>
        public void DrawScale(Graphics g)
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                double sMin = 0;
                double step = 0;
                int gN = 0;
                int panelBottm = chartPanel.RectPanel.Y + chartPanel.RectPanel.Height;
                int workSpaceY = GetWorkSpaceY(chartPanel.PanelID);
                //画X轴
                if (this.Height >= chartPanel.ScaleX_Height)
                {
                    g.DrawLine(chartPanel.XScalePen, 0, panelBottm - chartPanel.ScaleX_Height, this.Width, panelBottm - chartPanel.ScaleX_Height);
                }
                //画左侧Y轴
                bool drawingGridFlag = false;
                if (showLeftScale && this.leftPixSpace <= this.Width)
                {
                    if (this.LeftPixSpace <= this.Width)
                    {
                        g.DrawLine(chartPanel.LeftScalePen, leftPixSpace, chartPanel.RectPanel.Y + 1, leftPixSpace, panelBottm - chartPanel.ScaleX_Height);
                    }
                    if (processBarValue == 0)
                    {
                        Font leftYFont = chartPanel.LeftYFont;
                        SizeF leftYSize = g.MeasureString(" ", leftYFont);
                        gN = (int)(workSpaceY / leftYSize.Height) * 40 / 30;
                        CommonClass.GridScale(chartPanel.MinValue, chartPanel.MaxValue, gN, ref sMin, ref step, chartPanel.YScaleTick);
                        int interval = 0;
                        int gridInterval = chartPanel.GridInterval;
                        while (sMin <= chartPanel.MaxValue)
                        {
                            if (sMin > chartPanel.MinValue)
                            {
                                if (interval != 0 && interval % gridInterval == 0)
                                {
                                    leftYSize = g.MeasureString(CommonClass.GetValueByDigit(sMin, chartPanel.Digit), leftYFont);
                                    g.DrawLine(chartPanel.LeftScalePen, this.LeftPixSpace - 10,
                                        GetValueYPixel(chartPanel, sMin), this.LeftPixSpace, GetValueYPixel(chartPanel, sMin));
                                    g.DrawString(CommonClass.GetValueByDigit(sMin, chartPanel.Digit),
                                        leftYFont, chartPanel.LeftYFont_Brush,
                                        new RectangleF(this.LeftPixSpace - 10 - leftYSize.Width,
                                        GetValueYPixel(chartPanel, sMin) - leftYSize.Height / 2, leftYSize.Width, leftYSize.Height));
                                    drawingGridFlag = true;
                                    if (chartPanel.ShowGrid)
                                    {
                                        g.DrawLine(chartPanel.Grid_Pen, LeftPixSpace,
                                        GetValueYPixel(chartPanel, sMin), this.Width - RightPixSpace, GetValueYPixel(chartPanel, sMin));
                                    }
                                }
                                else
                                {
                                    g.DrawLine(chartPanel.LeftScalePen, this.LeftPixSpace - 5, GetValueYPixel(chartPanel, sMin), this.LeftPixSpace, GetValueYPixel(chartPanel, sMin));
                                }
                            }
                            sMin += step;
                            interval++;
                        }
                    }
                }
                //画右侧Y轴
                if (showRightScale && this.rightPixSpace <= this.Width)
                {
                    if (this.Width - RightPixSpace >= LeftPixSpace)
                    {
                        g.DrawLine(chartPanel.RightScalePen, this.Width - rightPixSpace, chartPanel.RectPanel.Y + 1, this.Width - rightPixSpace, panelBottm - chartPanel.ScaleX_Height);
                    }
                    if (processBarValue == 0)
                    {
                        Font rightYFont = chartPanel.RightYFont;
                        SizeF rightYSize = g.MeasureString(" ", rightYFont);
                        gN = (int)(workSpaceY / rightYSize.Height) * 40 / 30;
                        CommonClass.GridScale(chartPanel.MinValue, chartPanel.MaxValue, gN, ref sMin, ref step, chartPanel.YScaleTick);
                        int interval = 0;
                        int gridInterval = chartPanel.GridInterval;
                        while (sMin <= chartPanel.MaxValue)
                        {
                            if (sMin > chartPanel.MinValue)
                            {
                                if (interval != 0 && interval % gridInterval == 0)
                                {
                                    g.DrawLine(chartPanel.RightScalePen, this.Width - RightPixSpace,
                                        GetValueYPixel(chartPanel, sMin),
                                        this.Width - RightPixSpace + 10, GetValueYPixel(chartPanel, sMin));
                                    g.DrawString(CommonClass.GetValueByDigit(sMin, chartPanel.Digit),
                                        rightYFont, chartPanel.RightYFont_Brush,
                                        new RectangleF(this.Width - RightPixSpace + 10,
                                        GetValueYPixel(chartPanel, sMin) - rightYSize.Height / 2, this.RightPixSpace, rightYSize.Height));
                                    if (!drawingGridFlag)
                                    {
                                        drawingGridFlag = true;
                                        if (chartPanel.ShowGrid)
                                        {
                                            g.DrawLine(chartPanel.Grid_Pen, LeftPixSpace,
                                            GetValueYPixel(chartPanel, sMin),
                                            this.Width - RightPixSpace,
                                            GetValueYPixel(chartPanel, sMin));
                                        }
                                    }
                                }
                                else
                                {
                                    g.DrawLine(chartPanel.RightScalePen, this.Width - RightPixSpace, GetValueYPixel(chartPanel, sMin), this.Width - RightPixSpace + 5, GetValueYPixel(chartPanel, sMin));
                                }
                            }
                            sMin += step;
                            interval++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制图形
        /// </summary>
        /// <param name="g"></param>
        public void DrawSeries(Graphics g)
        {
            //设置最大值和最小值
            int minIndex = 0;
            int fRecord = firstVisibleRecord - 1;
            int lRecord = LastVisibleRecord;
            if (fRecord < 0 || lRecord < 1)
            {
                return;
            }
            List<object[]> signalList = new List<object[]>();
            Dictionary<int, string> infoBombDic = new Dictionary<int, string>();
            for (int i = fRecord; i < lRecord; i++)
            {
                string timeKeyShow = this.dtAllMsg.Rows[i][timekeyField].ToString();
                DataRow dr = dtAllMsg.Rows[i];
                if (dr == null)
                {
                    continue;
                }
                //画X轴的刻度
                float scaleX = this.leftPixSpace + (i + 2 - firstVisibleRecord) * axisSpace - axisSpace / 2;
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    string timeKey = CommonClass.GetCalenderFormatTimeKey(timeKeyShow, chartPanel.Interval);
                    SizeF timeKeySize = g.MeasureString(timeKey, chartPanel.CoordinateXFont);
                    int panelBottom = chartPanel.RectPanel.Y + chartPanel.RectPanel.Height;
                    if (i == firstVisibleRecord - 1)
                    {
                        g.DrawLine(chartPanel.XScalePen, scaleX, panelBottom - chartPanel.ScaleX_Height,
                            scaleX, panelBottom - chartPanel.ScaleX_Height + 6);
                        g.DrawString(timeKey, chartPanel.CoordinateXFont, chartPanel.CoordinateXFont_Brush,
                            new PointF(scaleX - timeKeySize.Width / 2, panelBottom - chartPanel.ScaleX_Height + 6));
                    }
                    if (scaleX - LeftPixSpace > timeKeySize.Width * 2 && minIndex == 0)
                    {
                        minIndex = i - (firstVisibleRecord - 1);
                    }
                    if (minIndex != 0 && (i - (firstVisibleRecord - 1)) % minIndex == 0)
                    {
                        g.DrawString(timeKey, chartPanel.CoordinateXFont, chartPanel.CoordinateXFont_Brush,
                            new PointF(scaleX - timeKeySize.Width / 2, panelBottom - chartPanel.ScaleX_Height + 6));
                        g.DrawLine(chartPanel.XScalePen, scaleX, panelBottom - chartPanel.ScaleX_Height,
                            scaleX, panelBottom - chartPanel.ScaleX_Height + 6);
                    }
                    else
                    {
                        g.DrawLine(chartPanel.XScalePen, scaleX, panelBottom - chartPanel.ScaleX_Height, scaleX,
                            panelBottom - chartPanel.ScaleX_Height + 3);
                    }
                    //画K线
                    foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                    {
                        double open = Convert.ToDouble(dr[cs.OpenField]);
                        double high = Convert.ToDouble(dr[cs.HighField]);
                        double low = Convert.ToDouble(dr[cs.LowField]);
                        double close = Convert.ToDouble(dr[cs.CloseField]);
                        if (GetWorkSpaceY(chartPanel.PanelID) > 0)
                        {
                            int buySellSignal = 0;
                            //画买卖点
                            if (cs.IndBuySellField[0] != null && cs.IndBuySellField[1] != null)
                            {
                                double buySignalValue = Convert.ToDouble(dr[cs.IndBuySellField[0]]);
                                double sellSignalValue = Convert.ToDouble(dr[cs.IndBuySellField[1]]);
                                if (buySignalValue >= sellSignalValue)
                                {
                                    buySellSignal = 1;
                                }
                                else
                                {
                                    buySellSignal = 2;
                                }
                                int flag = 0;
                                if (i <dtAllMsg.Rows.Count-1)
                                {

                                    double lastBuy = Convert.ToDouble(dtAllMsg.Rows[i+1][cs.IndBuySellField[0]]);
                                    double lastSell = Convert.ToDouble(dtAllMsg.Rows[i+1][cs.IndBuySellField[1]]);
                                    if (lastBuy >= lastSell)
                                    {
                                        if (buySellSignal != 1)
                                        {
                                            flag = 2;
                                            buySellSignal = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (buySellSignal != 2)
                                        {
                                            flag = 1;
                                            buySellSignal = 2;
                                        }
                                    }
                                }
                                StringFormat sf = new StringFormat();
                                if (flag == 1)
                                {
                                    Font bsFont = cs.BsFont;
                                    SizeF bsFontSize = g.MeasureString(cs.SellText, bsFont, 1000, sf);
                                    Brush sBrush = new SolidBrush(cs.SellColor);
                                    g.DrawString(cs.SellText, bsFont, sBrush, new PointF((int)scaleX - bsFontSize.Width / 2,
                                        GetValueYPixel(chartPanel, high) - bsFontSize.Height));
                                    sBrush.Dispose();
                                }
                                else if (flag == 2)
                                {
                                    Font bsFont = cs.BsFont;
                                    SizeF bsFontSize = g.MeasureString(cs.BuyText, bsFont, 1000, sf);
                                    Brush bBrush = new SolidBrush(cs.BuyColor);
                                    g.DrawString(cs.BuyText, bsFont, bBrush, new PointF((int)scaleX - bsFontSize.Width / 2,
                                    GetValueYPixel(chartPanel, low) + 2));
                                    bBrush.Dispose();
                                }
                                sf.Dispose();
                            }
                            //阳线
                            if (open <= close)
                            {
                                float recth = close - open != 0 ? (float)((close - open) / (chartPanel.MaxValue - chartPanel.MinValue) * GetWorkSpaceY(chartPanel.PanelID)) : 1;
                                if (recth < 1)
                                {
                                    recth = 1;
                                }
                                RectangleF rcUp = new RectangleF((int)scaleX - (int)(axisSpace / 4), GetValueYPixel(chartPanel, close), (int)(axisSpace / 4) * 2 + 1, recth);
                                Pen upPen = null;
                                switch (buySellSignal)
                                {
                                    case 0:
                                        upPen = cs.UpLine_Pen;
                                        break;
                                    case 1:
                                        upPen = Pens.Red;
                                        break;
                                    case 2:
                                        upPen = Pens.SkyBlue;
                                        break;
                                }
                                //先画竖线
                                if (cs.MiddleLine_Pen != null)
                                {
                                    g.DrawLine(cs.MiddleLine_Pen, scaleX, GetValueYPixel(chartPanel, high), scaleX, GetValueYPixel(chartPanel, low));
                                }
                                else
                                {
                                    g.DrawLine(upPen, scaleX, GetValueYPixel(chartPanel, high), scaleX, GetValueYPixel(chartPanel, low));
                                }
                                g.FillRectangle(chartPanel.BgBrush, new Rectangle((int)rcUp.X + 1, (int)rcUp.Y + 1, (int)rcUp.Width - 2, (int)rcUp.Height - 1));
                                g.DrawRectangle(upPen, new Rectangle((int)rcUp.X, (int)rcUp.Y, (int)rcUp.Width, (int)rcUp.Height));
                            }
                            //阴线
                            else
                            {
                                float recth = open - close != 0 ? (float)((open - close) / (chartPanel.MaxValue - chartPanel.MinValue) * GetWorkSpaceY(chartPanel.PanelID)) : 1;
                                if (recth < 1)
                                {
                                    recth = 1;
                                }
                                RectangleF rcDown = new RectangleF((int)scaleX - (int)(axisSpace / 4), GetValueYPixel(chartPanel, open), (int)(axisSpace / 4) * 2 + 1, recth);
                                Brush downBrush = null;
                                Pen downPen = null;
                                switch (buySellSignal)
                                {
                                    case 0:
                                        downBrush = cs.DownLine_Brush;
                                        downPen = cs.DownLine_Pen;
                                        break;
                                    case 1:
                                        downBrush = Brushes.Red;
                                        downPen = Pens.Red;
                                        break;
                                    case 2:
                                        downBrush = Brushes.SkyBlue;
                                        downPen = Pens.SkyBlue;
                                        break;
                                }
                                if (cs.MiddleLine_Pen != null)
                                {
                                    g.DrawLine(cs.MiddleLine_Pen, scaleX, GetValueYPixel(chartPanel, high), scaleX, GetValueYPixel(chartPanel, low));
                                }
                                else
                                {
                                    g.DrawLine(downPen, scaleX, GetValueYPixel(chartPanel, high), scaleX, GetValueYPixel(chartPanel, low));
                                }
                                g.FillRectangle(downBrush, rcDown);
                            }
                            //显示选中
                            if (cs.HasSelect)
                            {
                                if (showCrossHair)
                                {
                                    cs.HasSelect = false;
                                }
                                else
                                {
                                    int kPInterval = GetMaxVisibleRecord() / 30;
                                    if (kPInterval < 2)
                                    {
                                        kPInterval = 3;
                                    }
                                    if (i % kPInterval == 0)
                                    {
                                        RectangleF rect = new RectangleF((int)scaleX - 3, GetValueYPixel(chartPanel, close), 6, 6);
                                        g.FillRectangle(Brushes.White, rect);
                                    }
                                }
                            }
                        }
                    }
                    //画柱状图
                    foreach (HistogramSeries his in chartPanel.HistoramSeriesList)
                    {
                        if (dr[his.Field].ToString() != "")
                        {
                            double value = Convert.ToDouble(dr[his.Field]);
                            RectangleF rcHis = new RectangleF();
                            if (value >= 0)
                            {
                                rcHis = new RectangleF((int)scaleX - (int)(axisSpace / 4),
                                 GetValueYPixel(chartPanel, value), (int)(axisSpace / 5) * 2 + 1,
                                 GetValueYPixel(chartPanel, 0) - GetValueYPixel(chartPanel, value));
                            }
                            else
                            {
                                rcHis = new RectangleF((int)scaleX - (int)(axisSpace / 4),
                                 GetValueYPixel(chartPanel, 0), (int)(axisSpace / 5) * 2 + 1,
                                 GetValueYPixel(chartPanel, value) - GetValueYPixel(chartPanel, 0));
                            } 
                            if (his.RelateCandleName != null && his.RelateCandleName != string.Empty)
                            {
                                foreach (ChartPanel cp in this.dicChartPanel.Values)
                                {
                                    foreach (CandleSeries candleSeries in cp.CandleSeriesList)
                                    {
                                        if (candleSeries.CandleName == his.RelateCandleName)
                                        {
                                            double targetOpen = Convert.ToDouble(dr[candleSeries.OpenField]);
                                            double targetClose = Convert.ToDouble(dr[candleSeries.CloseField]);
                                            if (his.LineStyle)
                                            {
                                                PointF startP = new PointF((int)scaleX, GetValueYPixel(chartPanel, 0));
                                                PointF endP = new PointF((int)scaleX, GetValueYPixel(chartPanel, value));
                                                Pen linePen = null;
                                                int lineWidth = his.LineWidth;
                                                if (lineWidth > this.axisSpace)
                                                {
                                                    lineWidth = this.axisSpace;
                                                }
                                                if (targetOpen >= targetClose)
                                                {
                                                    linePen = his.Up_Pen;
                                                }
                                                else
                                                {
                                                    linePen = his.Down_Pen;
                                                }
                                                linePen.Width = lineWidth;
                                                if (startP.Y <= chartPanel.RectPanel.Y)
                                                {
                                                    startP.Y = chartPanel.RectPanel.Y;
                                                }
                                                if (startP.Y >= chartPanel.RectPanel.Bottom)
                                                {
                                                    startP.Y = chartPanel.RectPanel.Bottom;
                                                }
                                                if (endP.Y <= chartPanel.RectPanel.Y)
                                                {
                                                    endP.Y = chartPanel.RectPanel.Y;
                                                }
                                                if (endP.Y >= chartPanel.RectPanel.Bottom)
                                                {
                                                    endP.Y = chartPanel.RectPanel.Bottom;
                                                }
                                                g.DrawLine(linePen, startP, endP);
                                                linePen.Width = 1;
                                            }
                                            else
                                            {
                                                if (targetOpen >= targetClose)
                                                {
                                                    g.FillRectangle(chartPanel.BgBrush, rcHis);
                                                    g.DrawRectangle(his.Up_Pen, new Rectangle((int)rcHis.X,
                                                        (int)rcHis.Y, (int)rcHis.Width, (int)rcHis.Height + 1));
                                                }
                                                else
                                                {
                                                    g.FillRectangle(his.Down_lineBrush, rcHis);
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (his.LineStyle)
                                {
                                    PointF startP = new PointF((int)scaleX, GetValueYPixel(chartPanel, 0));
                                    PointF endP = new PointF((int)scaleX, GetValueYPixel(chartPanel, value));
                                    Pen linePen = null;
                                    int lineWidth = his.LineWidth;
                                    if (lineWidth > this.axisSpace)
                                    {
                                        lineWidth = this.axisSpace;
                                    }
                                    if (value >= 0)
                                    {
                                        linePen = his.Up_Pen;
                                    }
                                    else
                                    {
                                        linePen = his.Down_Pen;
                                    }
                                    linePen.Width = lineWidth;
                                    if (startP.Y <= chartPanel.RectPanel.Y)
                                    {
                                        startP.Y = chartPanel.RectPanel.Y;
                                    }
                                    if (startP.Y >= chartPanel.RectPanel.Bottom)
                                    {
                                        startP.Y = chartPanel.RectPanel.Bottom;
                                    }
                                    if (endP.Y <= chartPanel.RectPanel.Y)
                                    {
                                        endP.Y = chartPanel.RectPanel.Y;
                                    }
                                    if (endP.Y >= chartPanel.RectPanel.Bottom)
                                    {
                                        endP.Y = chartPanel.RectPanel.Bottom;
                                    }
                                    g.DrawLine(linePen, startP, endP);
                                    linePen.Width = 1;
                                }
                                else
                                {
                                    if (value >= 0)
                                    {
                                        g.FillRectangle(chartPanel.BgBrush, rcHis);
                                        g.DrawRectangle(his.Up_Pen, new Rectangle((int)rcHis.X, (int)rcHis.Y, (int)rcHis.Width, (int)rcHis.Height + 1));
                                    }
                                    else
                                    {
                                        g.FillRectangle(his.Down_lineBrush, rcHis);
                                    }
                                }
                            }
                            if (his.HasSelect == true)
                            {
                                if (showCrossHair)
                                {
                                    his.HasSelect = false;
                                }
                                else
                                {
                                    int kPInterval = GetMaxVisibleRecord() / 30;
                                    if (kPInterval < 2)
                                    {
                                        kPInterval = 2;
                                    }
                                    if (i % kPInterval == 0)
                                    {
                                        RectangleF rect = new RectangleF((int)scaleX - 3, GetValueYPixel(chartPanel, value) - 3, 6, 6);
                                        g.FillRectangle(Brushes.Yellow, rect);
                                    }
                                }
                            }
                        }
                        //画零线
                        if (chartPanel.MinValue < 0)
                        {
                            g.DrawLine(his.Down_Pen, leftPixSpace, GetValueYPixel(chartPanel, 0), this.Width - rightPixSpace, GetValueYPixel(chartPanel, 0));
                        }
                    }
                    //画趋势线
                    for (int lsJ = 0; lsJ < chartPanel.TrendLineSeriesList.Count; lsJ++)
                    {
                        TrendLineSeries ls = chartPanel.TrendLineSeriesList[lsJ];
                        PointF pStart = new PointF();
                        PointF pEnd = new PointF();
                        if (!(dr[ls.Field] is DBNull) && dr[ls.Field].ToString() != "")
                        {
                            double value = Convert.ToDouble(dr[ls.Field]);
                            if (dtAllMsg.Rows.Count == 1)
                            {
                                pStart = new PointF((int)scaleX - (int)(axisSpace / 4), GetValueYPixel(chartPanel, value));
                                pEnd = new PointF((int)scaleX - (int)(axisSpace / 4) + (int)(axisSpace / 4) * 2 + 1, GetValueYPixel(chartPanel, value));
                            }
                            else
                            {
                                DataRow drLast = null;
                                double lastValue = 0;
                                for (int j = i - 1; j >= fRecord; j--)
                                {
                                    string tk = this.dtAllMsg.Rows[j][timekeyField].ToString();
                                    DataRow drOld = dtAllMsg.Rows[j];
                                    if (!(drOld[ls.Field] is DBNull) && drOld[ls.Field].ToString() != "")
                                    {
                                        int left = this.leftPixSpace + (j + 2 - firstVisibleRecord) * axisSpace - axisSpace / 2;
                                        lastValue = Convert.ToDouble(drOld[ls.Field]);
                                        pStart = new PointF((int)left, GetValueYPixel(chartPanel, lastValue));
                                        if (j != i - 1)
                                        {
                                            int right = this.leftPixSpace + (i + 1 - firstVisibleRecord) * axisSpace - axisSpace / 2;
                                            pEnd = new PointF((int)right, GetValueYPixel(chartPanel, lastValue));
                                            if (pStart.Y <= panelBottom - chartPanel.ScaleX_Height + 1
                                                && pStart.Y >= chartPanel.RectPanel.Y + chartPanel.TitleHeight - 1
                                                && pEnd.Y < panelBottom - chartPanel.ScaleX_Height + 1
                                                && pEnd.Y >= chartPanel.RectPanel.Y + chartPanel.TitleHeight - 1)
                                            {
                                                if (lastValue >= value)
                                                {
                                                    g.DrawLine(ls.Up_LinePen, pStart, pEnd);
                                                }
                                                else
                                                {
                                                    g.DrawLine(ls.Down_linePen, pStart, pEnd);
                                                }
                                            }
                                            pStart = new PointF((int)right, GetValueYPixel(chartPanel, lastValue));
                                        }
                                        drLast = drOld;
                                        break;
                                    }
                                }
                                pEnd = new PointF((int)scaleX, GetValueYPixel(chartPanel, value));
                                if (drLast != null)
                                {
                                    if (pStart.Y <= panelBottom - chartPanel.ScaleX_Height + 1
                                        && pStart.Y >= chartPanel.RectPanel.Y + chartPanel.TitleHeight - 1
                                        && pEnd.Y < panelBottom - chartPanel.ScaleX_Height + 1
                                        && pEnd.Y >= chartPanel.RectPanel.Y + chartPanel.TitleHeight - 1)
                                    {
                                        if (lastValue >= value)
                                        {
                                            g.DrawLine(ls.Up_LinePen, pStart, pEnd);
                                        }
                                        else
                                        {
                                            g.DrawLine(ls.Down_linePen, pStart, pEnd);
                                        }
                                    }
                                }
                            }
                            //显示选中
                            if (ls.HasSelect)
                            {
                                if (showCrossHair)
                                {
                                    ls.HasSelect = false;
                                }
                                else
                                {
                                    int kPInterval = GetMaxVisibleRecord() / 30;
                                    if (kPInterval < 1)
                                    {
                                        kPInterval = 1;
                                    }
                                    if (i % kPInterval == 0)
                                    {
                                        RectangleF rect = new RectangleF((int)scaleX - 3, GetValueYPixel(chartPanel, value) - 3, 6, 6);
                                        if (rect.Y < panelBottom - chartPanel.ScaleX_Height)
                                        {
                                            g.FillRectangle(ls.Up_LineBrush, rect);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //画信息地雷
                    if (this.dtAllMsg.Columns.Contains(chartPanel.InfoBombField))
                    {
                        if (!(dr[chartPanel.InfoBombField] is DBNull) && dr[chartPanel.InfoBombField].ToString() != string.Empty)
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            RectangleF rectBomb = new RectangleF(scaleX - 3, chartPanel.RectPanel.Y + chartPanel.TitleHeight - 6, 6, 6);
                            Color bombColor = chartPanel.InfoBombColor;
                            if (showCrossHair && i == crossOverIndex)
                            {
                                if (i == crossOverIndex)
                                {
                                    bombColor = chartPanel.InfoBombSelectedColor;
                                    infoBombDic[chartPanel.PanelID] = dr[chartPanel.InfoBombField].ToString();
                                }
                            }
                            Pen bombPen = new Pen(bombColor);
                            Brush bombBrush = new SolidBrush(bombColor);
                            g.FillEllipse(bombBrush, rectBomb);
                            g.DrawLine(bombPen, scaleX, rectBomb.Top - 1, scaleX, rectBomb.Bottom + 1);
                            g.DrawLine(bombPen, scaleX - 4, rectBomb.Top + 3, scaleX + 4, rectBomb.Top + 3);
                            g.DrawLine(bombPen, scaleX - 4, rectBomb.Top - 1, scaleX + 4, rectBomb.Bottom + 1);
                            g.DrawLine(bombPen, scaleX - 4, rectBomb.Bottom + 1, scaleX + 4, rectBomb.Top - 1);
                            g.SmoothingMode = SmoothingMode.Default;
                            bombPen.Dispose();
                            bombBrush.Dispose();
                        }
                    }
                    //保存要绘画的标记
                    if (chartPanel.SignalSeriesDic.ContainsKey(timeKeyShow))
                    {
                        foreach (SignalSeries uds in chartPanel.SignalSeriesDic[timeKeyShow])
                        {
                            if (uds.Value >= chartPanel.MinValue && uds.Value <= chartPanel.MaxValue)
                            {
                                GraphicsPath gp = uds.GetGPByType(scaleX, GetValueYPixel(chartPanel, uds.Value), this.axisSpace);
                                Brush signalBrush = new SolidBrush(Color.FromArgb(100, uds.SignalColor));
                                Pen signalPen = new Pen(uds.SignalColor);
                                signalList.Add(new object[] { gp, signalBrush, signalPen });
                            }
                        }
                    }
                }
            }
            //绘制标记
            foreach (object[] obj in signalList)
            {
                GraphicsPath gp = obj[0] as GraphicsPath;
                Brush signalBrush = obj[1] as Brush;
                Pen signalPen = obj[2] as Pen;
                g.FillPath(signalBrush, gp);
                g.DrawPath(signalPen, gp);
                signalPen.Dispose();
                signalBrush.Dispose();
                gp.Dispose();
            }
            //显示K线的最大和最小值
            if (this.dtAllMsg.Rows.Count > 0)
            {
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                    {
                        if (cs.MaxRecord != -1 && cs.MinRecord != -1)
                        {
                            //画K线的最大值
                            DataRow drMax = this.dtAllMsg.Rows[cs.MaxRecord];
                            double maxValue = Convert.ToDouble(drMax[cs.HighField]);
                            float scaleXMax = this.leftPixSpace + (cs.MaxRecord + 2 - firstVisibleRecord) * axisSpace - axisSpace / 2;
                            float scaleYMax = GetValueYPixel(chartPanel, maxValue);
                            SizeF maxSize = g.MeasureString(maxValue.ToString("0.00"), this.Font);
                            PointF maxP = new PointF();
                            if (scaleXMax < this.leftPixSpace + maxSize.Width)
                            {
                                maxP = new PointF(scaleXMax, scaleYMax + maxSize.Height / 2);
                            }
                            else if (scaleXMax > this.Width - this.rightPixSpace - maxSize.Width)
                            {
                                maxP = new PointF(scaleXMax - maxSize.Width, scaleYMax + maxSize.Height / 2);
                            }
                            else
                            {
                                if (scaleXMax < this.Width / 2)
                                {
                                    maxP = new PointF(scaleXMax - maxSize.Width, scaleYMax + maxSize.Height / 2);
                                }
                                else
                                {
                                    maxP = new PointF(scaleXMax, scaleYMax + maxSize.Height / 2);
                                }
                            }
                            g.DrawString(maxValue.ToString("0.00"), this.Font, Brushes.White, maxP);
                            g.DrawLine(Pens.White, scaleXMax, scaleYMax, maxP.X + maxSize.Width / 2, maxP.Y);
                            //画K线的最小值
                            DataRow drMin = this.dtAllMsg.Rows[cs.MinRecord];
                            double minValue = Convert.ToDouble(drMin[cs.LowField]);
                            SizeF minSize = g.MeasureString(minValue.ToString("0.00"), this.Font);
                            float scaleXMin = this.leftPixSpace + (cs.MinRecord + 2 - firstVisibleRecord) * axisSpace - axisSpace / 2;
                            float scaleYMin = GetValueYPixel(chartPanel, minValue);
                            PointF minP = new PointF();
                            if (scaleXMin < this.leftPixSpace + minSize.Width)
                            {
                                minP = new PointF(scaleXMin, scaleYMin - minSize.Height * 3 / 2);
                            }
                            else if (scaleXMin > this.Width - this.rightPixSpace - minSize.Width)
                            {
                                minP = new PointF(scaleXMin - minSize.Width, scaleYMin - minSize.Height * 3 / 2);
                            }
                            else
                            {
                                if (scaleXMin < this.Width / 2)
                                {
                                    minP = new PointF(scaleXMin - minSize.Width, scaleYMin - minSize.Height * 3 / 2);
                                }
                                else
                                {
                                    minP = new PointF(scaleXMin, scaleYMin - minSize.Height * 3 / 2);
                                }
                            }
                            g.DrawString(minValue.ToString("0.00"), this.Font, Brushes.White, minP);
                            g.DrawLine(Pens.White, scaleXMin, scaleYMin, minP.X + minSize.Width / 2, minP.Y + minSize.Height);
                        }
                    }
                }
                //画信息地雷的信息提示
                foreach (int panelID in infoBombDic.Keys)
                {
                    ChartPanel chartPanel = this.dicChartPanel[panelID];
                    string bombInfoText = infoBombDic[panelID];
                    Color bgColor = chartPanel.InfoBombTipColor;
                    Color strColor = chartPanel.InfoBombTipTextColor;
                    Pen tipBorderPen = new Pen(bgColor);
                    tipBorderPen.DashStyle = DashStyle.Dot;
                    Brush bgBrush = new SolidBrush(bgColor);
                    Brush strBrush = new SolidBrush(strColor);
                    SizeF sizeF = g.MeasureString(bombInfoText, new Font("宋体",10));
                    g.DrawRectangle(tipBorderPen, this.leftPixSpace + 1, (int)chartPanel.RectPanel.Y + chartPanel.TitleHeight + 5, (int)sizeF.Width, (int)sizeF.Height);
                    g.FillRectangle(bgBrush, new Rectangle(this.leftPixSpace + 2, 
                        (int)chartPanel.RectPanel.Y + (int)chartPanel.TitleHeight + 6, (int)sizeF.Width-1, (int)sizeF.Height - 1));
                    g.DrawString(bombInfoText, new Font("宋体", 10), strBrush, this.leftPixSpace + 2, chartPanel.RectPanel.Y + chartPanel.TitleHeight + 7);
                    tipBorderPen.Dispose();
                    bgBrush.Dispose();
                    strBrush.Dispose();
                }
            }
        }

        /// <summary>
        /// 绘制进度条
        /// </summary>
        public void DrawProcessBar(Graphics g)
        {
            int pieR = 100;
            g.SmoothingMode = SmoothingMode.HighQuality;
            Rectangle ellipseRect = new Rectangle(this.Width / 2 - pieR, this.Height / 2 - pieR, pieR * 2, pieR * 2);
            if (processBarValue > 0 && processBarValue <= 100)
            {
                Color processColor = Color.SkyBlue;
                StringBuilder sbProcess = new StringBuilder();
                if (processBarValue < 50)
                {
                    sbProcess.Append("加载中...\r\n");
                }
                else if (processBarValue >= 50 && processBarValue < 100)
                {
                    sbProcess.Append("加载中...\r\n");
                    processColor = Color.Red;
                }
                else
                {
                    sbProcess.Append("加载完成\r\n");
                    processColor = Color.Teal;
                }
                sbProcess.Append(processBarValue.ToString() + "%");
                Brush brush = new SolidBrush(Color.FromArgb(90, processColor));
                Pen processPen = new Pen(processColor, 2);
                int startAngle = 270;
                int endAngle = Convert.ToInt32((double)processBarValue / 100 * 360);
                g.FillPie(brush, ellipseRect, startAngle, endAngle);
                Rectangle drawRectangle = new Rectangle(ellipseRect.X, ellipseRect.Y, ellipseRect.Width - 2, ellipseRect.Height - 2);
                if (processBarValue == 100)
                {
                    g.DrawEllipse(processPen, drawRectangle);
                    processBarValue = 0;
                }
                else
                {
                    g.DrawPie(processPen, drawRectangle, startAngle, endAngle + 1);
                }
                Font strFont = new Font("宋体", 12, FontStyle.Bold | FontStyle.Italic);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                SizeF fontSize = g.MeasureString(sbProcess.ToString(), strFont);
                g.DrawString(sbProcess.ToString(), strFont, Brushes.White, new PointF(this.Width / 2, this.Height / 2 - fontSize.Height / 2), sf);
                brush.Dispose();
                processPen.Dispose();
                sf.Dispose();
            }
            g.SmoothingMode = SmoothingMode.Default;
        }

        /// <summary>
        /// 绘股指提示框
        /// </summary>
        /// <param name="g"></param>
        public void DrawValuePanel(Graphics g)
        {
            if (selectedObject != null)
            {
                if (vp_index > 0 && vp_index <= LastVisibleRecord - 1)
                {
                    Point mouseP = GetCrossHairPoint();
                    //获取鼠标位置面板的digit值
                    int digit = 2;
                    IntervalType curIntervalType = IntervalType.Day;
                    foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                    {
                        if (mouseP.Y >= chartPanel.RectPanel.Y && mouseP.Y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height)
                        {
                            if (GetWorkSpaceY(chartPanel.PanelID) > 0)
                            {
                                digit = chartPanel.Digit;
                                curIntervalType = chartPanel.Interval;
                            }
                        }
                    }
                    DataRow dr = this.dtAllMsg.Rows[vp_index];
                    Point mp = new Point(GetCrossHairPoint().X + 10, GetCrossHairPoint().Y);
                    string timeKey = timekeyField + ":" + CommonClass.GetCalenderFormatTimeKey(dr[timekeyField].ToString(), curIntervalType);
                    Font tipFont = new Font("New Times Roman", 10, FontStyle.Bold);
                    SizeF timeKeySize = g.MeasureString(timeKey, tipFont);
                    double pWidth = 0;
                    double pHeight = 0;
                    List<double> wList = new List<double>();
                    StringBuilder sbValue = new StringBuilder();
                    sbValue.Append(timeKey + "\r\n");
                    Color pColor = Color.Turquoise;
                    //根据显示的字符获取框体的大小
                    if (selectedObject is CandleSeries)
                    {
                        CandleSeries cs = selectedObject as CandleSeries;
                        double open = 0;
                        Double.TryParse(dr[cs.OpenField].ToString(), out open);
                        double high = 0;
                        Double.TryParse(dr[cs.HighField].ToString(), out high);
                        double low = 0;
                        Double.TryParse(dr[cs.LowField].ToString(), out low);
                        double close = 0;
                        Double.TryParse(dr[cs.CloseField].ToString(), out close);
                        string strOpen = cs.OpenField + ":" + CommonClass.GetValueByDigit(open, digit);
                        sbValue.Append(strOpen + "\r\n");
                        SizeF openSize = g.MeasureString(strOpen, tipFont);
                        string strHigh = cs.HighField + ":" + CommonClass.GetValueByDigit(high, digit);
                        sbValue.Append(strHigh + "\r\n");
                        SizeF highSize = g.MeasureString(strHigh, tipFont);
                        string strLow = cs.LowField + ":" + CommonClass.GetValueByDigit(low, digit);
                        sbValue.Append(strLow + "\r\n");
                        SizeF lowSize = g.MeasureString(strLow, tipFont);
                        string strClose = cs.CloseField + ":" + CommonClass.GetValueByDigit(close, digit);
                        sbValue.Append(strClose);
                        SizeF closeSize = g.MeasureString(strClose, tipFont);
                        wList.AddRange(new double[] { timeKeySize.Width, openSize.Width, highSize.Width, lowSize.Width, closeSize.Width });
                        pWidth = CommonClass.GetHighValue(wList);
                        pHeight = timeKeySize.Height + openSize.Height + highSize.Height + lowSize.Height + closeSize.Height;
                    }
                    else if (selectedObject is HistogramSeries)
                    {
                        HistogramSeries hs = selectedObject as HistogramSeries;
                        double volumn = 0;
                        Double.TryParse(dr[hs.Field].ToString(), out volumn);
                        string strVolume = hs.Field + ":" + CommonClass.GetValueByDigit(volumn, digit);
                        sbValue.Append(strVolume);
                        SizeF volumeSize = g.MeasureString(strVolume, tipFont);
                        wList.AddRange(new double[] { timeKeySize.Width, volumeSize.Width });
                        pWidth = CommonClass.GetHighValue(wList);
                        pHeight = timeKeySize.Height + volumeSize.Height;
                        pColor = Color.Yellow;
                    }
                    else if (selectedObject is TrendLineSeries)
                    {
                        TrendLineSeries tls = selectedObject as TrendLineSeries;
                        double lineValue = 0;
                        Double.TryParse(dr[tls.Field].ToString(), out lineValue);
                        string strLine = tls.DisplayName != null ? tls.DisplayName + ":" + CommonClass.GetValueByDigit(lineValue, digit) : tls.Field + ":" + CommonClass.GetValueByDigit(lineValue, digit);
                        sbValue.Append(strLine);
                        SizeF lineSize = g.MeasureString(strLine, tipFont);
                        wList.AddRange(new double[] { timeKeySize.Width, lineSize.Width });
                        pWidth = CommonClass.GetHighValue(wList);
                        pHeight = timeKeySize.Height + lineSize.Height;
                        pColor = tls.Up_LineColor;
                    }
                    pWidth += 4;
                    pHeight += 1;
                    Rectangle rectP = new Rectangle(GetCrossHairPoint().X + 10, GetCrossHairPoint().Y, (int)pWidth, (int)pHeight);
                    Brush pbgBrush = new SolidBrush(Color.FromArgb(100, Color.Black));
                    Pen pPen = new Pen(pColor);
                    Brush pBrush = new SolidBrush(pColor);
                    g.FillRectangle(pbgBrush, rectP);
                    g.DrawRectangle(pPen, rectP);
                    g.DrawString(sbValue.ToString(), tipFont, pBrush, new PointF(GetCrossHairPoint().X + 10, GetCrossHairPoint().Y + 2));
                    pbgBrush.Dispose();
                    pPen.Dispose();
                    pBrush.Dispose();
                }
            }
        }

        /// <summary>
        /// 绘制整个图像
        /// </summary>
        public void DrawGraph()
        {
            PaintGraph(this.DisplayRectangle);
        }

        /// <summary>
        /// 绘制图像的一部分
        /// </summary>
        /// <param name="drawRectangle"></param>
        public void DrawGraph(Rectangle drawRectangle)
        {
            PaintGraph(drawRectangle);
        }

        /// <summary>
        /// 绘制图像到Image
        /// </summary>
        /// <returns></returns>
        public Image DrawToBitmap()
        {
            lock (refresh_lock)
            {
                Image image = new Bitmap(this.Width, this.Height);
                Graphics g = Graphics.FromImage(image);
                DrawBackGround(g);
                DrawTitle(g);
                DrawScale(g);
                if (processBarValue == 0)
                {
                    DrawSeries(g);
                    if (showCrossHair)
                    {
                        DrawCrossHair(g);
                    }
                    DrawValuePanel(g);
                }
                DrawProcessBar(g);
                return image;
            }
        }

        /// <summary>
        /// 绘制图象
        /// </summary>
        public void PaintGraph(Rectangle drawRectangle)
        {
            lock (refresh_lock)
            {
                BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
                BufferedGraphics myBuffer = currentContext.Allocate(this.CreateGraphics(), drawRectangle);
                Graphics g = myBuffer.Graphics;
                //画背景
                DrawBackGround(g);
                //画标题
                DrawTitle(g);
                //画坐标轴
                DrawScale(g);
                if (processBarValue == 0)
                {
                    //画线条
                    DrawSeries(g);
                    //画十字线
                    if (showCrossHair)
                    {
                        DrawCrossHair(g);
                    }
                    //画股指提示
                    DrawValuePanel(g);
                }
                //画进度条
                DrawProcessBar(g);
                myBuffer.Render();
                myBuffer.Dispose();
            }
        }
        #endregion

        #region 指标计算
        /// <summary>
        /// 操盘手买卖信号
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="candleName"></param>
        /// <param name="buyEMA"></param>
        /// <param name="buyTarget"></param>
        /// <param name="sellEMA"></param>
        /// <param name="sellTarget"></param>
        public void YMBuySellSignal(int panelID, string candleName, string buyEMA, string buyTarget, string sellEMA, string sellTarget)
        {
            this.AddExponentialMovingAverage(buyEMA, buyEMA, 6, buyTarget, panelID);
            this.SetTrendLineStyle(buyEMA, Color.SkyBlue, Color.Red, 1, DashStyle.Solid);
            this.AddExponentialMovingAverage(sellEMA, sellEMA, 5, sellTarget, panelID);
            this.SetTrendLineStyle(sellEMA, Color.Yellow, Color.Yellow, 1, DashStyle.Solid);
            this.SetCandleBuySellField(candleName, buyEMA, sellEMA);
        }

        /// <summary>
        /// 选择显示指标
        /// </summary>
        /// <param name="indicatorType"></param>
        public void ChoseIndicator(IndicatorType indicatorType)
        {
            //TODO:
        }
        #endregion
    }
}

