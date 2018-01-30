using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace mPaint
{
    /// <summary>
    /// 平滑移动平均线(SMA)
    /// </summary>
    public class IndicatorSimpleMovingAverage
    {
        public IndicatorSimpleMovingAverage()
        {

        }

        /// <summary>
        /// 目标字段
        /// </summary>
        private string target = string.Empty;

        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        /// <summary>
        /// 周期
        /// </summary>
        private int cycle = 5;

        public int Cycle
        {
            get { return cycle; }
            set { cycle = value; }
        }

        /// <summary>
        /// 趋势线对象
        /// </summary>
        private TrendLineSeries trendLineSeries = null;

        public TrendLineSeries TrendLineSeries
        {
            get { return trendLineSeries; }
            set { trendLineSeries = value; }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private DataTable dataSource;

        public DataTable DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        /// <summary>
        /// 计算简单移动平均线的值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="n"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public double Calculate(int r)
        {
            if (dataSource.Columns.Contains(trendLineSeries.Field) && dataSource.Columns.Contains(target))
            {
                DataRow dr = dataSource.Rows[r];
                double curValue = Convert.ToDouble(dataSource.Rows[r][target]);
                return CommonClass.CalcuteSimpleMovingAvg(r, cycle, trendLineSeries.Field, target, curValue, dataSource);
            }
            return CommonClass.NULL;
        }
    }

    /// <summary>
    /// 指数移动平均线(EMA)
    /// </summary>
    public class IndicatorExponentialMovingAverage
    {
        public IndicatorExponentialMovingAverage()
        {

        }

        /// <summary>
        /// 目标字段
        /// </summary>
        private string target = string.Empty;

        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        /// <summary>
        /// 周期
        /// </summary>
        private int cycle = 5;

        public int Cycle
        {
            get { return cycle; }
            set { cycle = value; }
        }

        /// <summary>
        /// 趋势线对象
        /// </summary>
        private TrendLineSeries trendLineSeries = null;

        public TrendLineSeries TrendLineSeries
        {
            get { return trendLineSeries; }
            set { trendLineSeries = value; }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private DataTable dataSource;

        public DataTable DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        /// <summary>
        /// 计算指数平均数指标的值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="target"></param>
        /// <param name="n"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public double Calculate(int r)
        {
            return CommonClass.CalculateExponentialMovingAvg(trendLineSeries.Field, target, cycle, dataSource, r);
        }
    }

    /// <summary>
    /// 随机指标(KDJ)
    /// </summary>
    public class IndicatorStochasticOscillator
    {
        public IndicatorStochasticOscillator()
        {
        }

        /// <summary>
        /// K值趋势线
        /// </summary>
        private TrendLineSeries tlsK = null;

        public TrendLineSeries TlsK
        {
            get { return tlsK; }
            set { tlsK = value; }
        }

        /// <summary>
        /// D值趋势线
        /// </summary>
        private TrendLineSeries tlsD = null;

        public TrendLineSeries TlsD
        {
            get { return tlsD; }
            set { tlsD = value; }
        }

        /// <summary>
        /// J值趋势线
        /// </summary>
        private TrendLineSeries tlsJ = null;

        public TrendLineSeries TlsJ
        {
            get { return tlsJ; }
            set { tlsJ = value; }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private DataTable dataSource;

        public DataTable DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        /// <summary>
        /// K值周期
        /// </summary>
        private int kPeriods = 9;

        public int KPeriods
        {
            get { return kPeriods; }
            set { kPeriods = value; }
        }

        /// <summary>
        /// 慢速K值周期
        /// </summary>
        private int kSlowing = 3;

        public int KSlowing
        {
            get { return kSlowing; }
            set { kSlowing = value; }
        }

        /// <summary>
        /// D值周期
        /// </summary>
        private int dPeriods = 9;

        public int DPeriods
        {
            get { return dPeriods; }
            set { dPeriods = value; }
        }

        /// <summary>
        /// 收盘价字段
        /// </summary>
        private string close = string.Empty;

        public string Close
        {
            get { return close; }
            set { close = value; }
        }

        /// <summary>
        /// 最高价字段
        /// </summary>
        private string high = string.Empty;

        public string High
        {
            get { return high; }
            set { high = value; }
        }

        /// <summary>
        /// 最低价字段
        /// </summary>
        private string low = string.Empty;

        public string Low
        {
            get { return low; }
            set { low = value; }
        }

        /// <summary>
        /// 获取指定索引的K,D,J的值
        /// K=昨日K*2/3+RSV/3;
        /// D=昨日D*2/3+K/3;
        /// J=3*K-2*D
        /// </summary>
        /// <param name="panelID"></param>
        /// <returns></returns>
        public double[] Calculate(int r)
        {
            double k = 0;
            double d = 0;
            if (r > 0)
            {
                double lastK = Convert.ToDouble(dataSource.Rows[r - 1][tlsK.Field]);
                double lastD = Convert.ToDouble(dataSource.Rows[r - 1][tlsD.Field]);
                k = lastK * 2 / 3 + RSV(r) / 3;
                d = lastD * 2 / 3 + k / 3;
            }
            double j = 3 * k - 2 * d;
            return new double[] { k, d, j };
        }

        /// <summary>
        /// 获取未成熟随机指标值
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="rowIndex"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public double RSV(int r)
        {
            int endIndex = r - (kPeriods - 1);
            if (endIndex < 0)
            {
                endIndex = 0;
            }
            double currentClose = Convert.ToDouble(dataSource.Rows[r][close]);
            List<double> highList = new List<double>();
            List<double> lowList = new List<double>();
            for (int i = r; i >= endIndex; i--)
            {
                double h = Convert.ToDouble(dataSource.Rows[i][high]);
                double l = Convert.ToDouble(dataSource.Rows[i][low]);
                highList.Add(h);
                lowList.Add(l);
            }
            double nHigh = CommonClass.GetHighValue(highList);
            double nLow = CommonClass.GetLowValue(lowList);
            return (currentClose - nLow) / (nHigh - nLow) * 100;
        }
    }

    /// <summary>
    /// 指数平滑异同移动平均线(MACD)
    /// </summary>
    public class IndicatorMACD
    {
        public IndicatorMACD()
        {

        }

        /// <summary>
        /// MACD柱状图
        /// </summary>
        private HistogramSeries hsMACD = null;

        public HistogramSeries HsMACD
        {
            get { return hsMACD; }
            set { hsMACD = value; }
        }

        /// <summary>
        /// Diff线
        /// </summary>
        private TrendLineSeries tlsDiff = null;

        public TrendLineSeries TlsDiff
        {
            get { return tlsDiff; }
            set { tlsDiff = value; }
        }

        /// <summary>
        /// Dea线
        /// </summary>
        private TrendLineSeries tlsDea = null;

        public TrendLineSeries TlsDea
        {
            get { return tlsDea; }
            set { tlsDea = value; }
        }

        /// <summary>
        /// 长周期
        /// </summary>
        private int longCycle = 26;

        public int LongCycle
        {
            get { return longCycle; }
            set { longCycle = value; }
        }

        /// <summary>
        /// 短周期
        /// </summary>
        private int shortCycle = 12;

        public int ShortCycle
        {
            get { return shortCycle; }
            set { shortCycle = value; }
        }

        /// <summary>
        /// 标记周期
        /// </summary>
        private int signalPeriods = 9;

        public int SignalPeriods
        {
            get { return signalPeriods; }
            set { signalPeriods = value; }
        }

        /// <summary>
        /// 收盘价字段
        /// </summary>
        private string close = string.Empty;

        public string Close
        {
            get { return close; }
            set { close = value; }
        }

        /// <summary>
        /// 长周期EMA字段
        /// </summary>
        private string longCycleEMA = "EMA_Long";// CommonClass.GetGuid();//"EMA_Long";

        public string LongCycleEMA
        {
            get { return longCycleEMA; }
            set { longCycleEMA = value; }
        }

        /// <summary>
        /// 短周期EMA字段
        /// </summary>
        private string shortCycleEMA = "EMA_Short";//CommonClass.GetGuid();// "EMA_Short";

        public string ShortCycleEMA
        {
            get { return shortCycleEMA; }
            set { shortCycleEMA = value; }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private DataTable dataSource = null;

        public DataTable DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        /// <summary>
        /// 根据指定索引的MACD,DIFF,DEA的值
        /// DIFF=EMA(C,ShortCycle)-EMA(C,LongCycle);
        /// DEA=DIFF*0.2-昨日DEA*0.8
        /// MACD=2*(DIFF-DEA)
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public double[] Calulate(int r)
        {
            double macd = 0;
            double diff = 0;
            double dea = 0;
            if (r > 0)
            {
                DataRow dr = dataSource.Rows[r];
                double longEMA = Convert.ToDouble(dr[longCycleEMA]);
                double shortEMA = Convert.ToDouble(dr[shortCycleEMA]);
                diff = shortEMA - longEMA;
                double lastDea = Convert.ToDouble(dataSource.Rows[r - 1][tlsDea.Field]);
                dea = diff * 0.2 + lastDea * 0.8;
                macd = 2 * (diff - dea);
            }
            return new double[] { macd, diff, dea };
        }
    }

    /// <summary>
    /// 布林带
    /// </summary>
    public class IndicatorBollingerBands
    {
        public IndicatorBollingerBands()
        {

        }

        /// <summary>
        /// 中间的布林线
        /// </summary>
        private TrendLineSeries tlsM = null;

        public TrendLineSeries TlsM
        {
            get { return tlsM; }
            set { tlsM = value; }
        }

        /// <summary>
        /// 上边的布林线
        /// </summary>
        private TrendLineSeries tlsU = null;

        public TrendLineSeries TlsU
        {
            get { return tlsU; }
            set { tlsU = value; }
        }

        /// <summary>
        /// 下边的布林线
        /// </summary>
        private TrendLineSeries tlsD = null;

        public TrendLineSeries TlsD
        {
            get { return tlsD; }
            set { tlsD = value; }
        }

        /// <summary>
        /// 收盘价字段
        /// </summary>
        private string close = string.Empty;

        public string Close
        {
            get { return close; }
            set { close = value; }
        }

        /// <summary>
        /// 周期
        /// </summary>
        private int periods = 20;

        public int Periods
        {
            get { return periods; }
            set { periods = value; }
        }

        /// <summary>
        /// 标准差
        /// </summary>
        private int standardDeviations = 2;

        public int StandardDeviations
        {
            get { return standardDeviations; }
            set { standardDeviations = value; }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private DataTable dataSource = null;

        public DataTable DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        /// <summary>
        /// 计算指定索引的MID,DOWN,UP值
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public double[] Calculate(int r)
        {
            double closeValue = Convert.ToDouble(dataSource.Rows[r][this.Close]);
            double mid = CommonClass.CalcuteSimpleMovingAvg(r, periods, TlsM.Field, close, closeValue, dataSource);
            if (mid == CommonClass.NULL)
            {
                mid = 0;
            }
            double md = 0;
            if (r >= periods - 1)
            {
                double sumValue = (closeValue - mid) * (closeValue - mid);
                for (int i = r - 1; i >= r - (periods - 1); i--)
                {
                    double curClose = Convert.ToDouble(dataSource.Rows[i][this.Close]);
                    double curMA = Convert.ToDouble(dataSource.Rows[i][this.tlsM.Field]);
                    sumValue += (curClose - curMA) * (curClose - curMA);
                }
                md = standardDeviations * Math.Sqrt(sumValue / periods);
            }
            double up = mid + 2 * md;
            double down = mid - 2 * md;
            return new double[] { mid, up, down };
        }
    }
}
