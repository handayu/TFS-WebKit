using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace mPaint
{
    /// <summary>
    /// 公共类
    /// </summary>
    public class CommonClass
    {
        /// <summary>
        /// 空值常量
        /// </summary>
        public static double NULL = double.MinValue;

        /// <summary>
        /// 获取Guid
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            return System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 获取指定索引的移动平均值
        /// </summary>
        /// <param name="r"></param>
        /// <param name="field"></param>
        /// <param name="target"></param>
        /// <param name="curValue"></param>
        /// <returns></returns>
        public static double CalcuteSimpleMovingAvg(int r, int cycle, string field, string target, double curValue, DataTable dataSource)
        {
            double sumValue = 0;
            if (r == cycle - 1)
            {
                for (int i = 0; i <= r - 1; i++)
                {
                    sumValue += Convert.ToDouble(dataSource.Rows[i][target]);
                }
                sumValue += curValue;
                return sumValue / cycle;
            }
            else if (r > cycle - 1)
            {
                sumValue = Convert.ToDouble(dataSource.Rows[r - 1][field]) * cycle;
                sumValue -= Convert.ToDouble(dataSource.Rows[r - cycle][target]);
                sumValue += curValue;
                return sumValue / cycle;
            }
            else
            {
                return NULL;
            }
        }

        /// <summary>
        /// 获取指定索引的指数移动平均值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="target"></param>
        /// <param name="cycle"></param>
        /// <param name="dataSource"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static double CalculateExponentialMovingAvg(string field, string target, int cycle, DataTable dataSource, int r)
        {
            DataRow dr = dataSource.Rows[r];
            double closeValue = Convert.ToDouble(dr[target]);//今收
            double lastEMA = 0;//昨日的EMA
            double newEmaValue = 0;//今日EMA
            if (r > 0)
            {
                lastEMA = Convert.ToDouble(dataSource.Rows[r - 1][field]);
                newEmaValue = (closeValue * 2 + lastEMA * (cycle - 1)) / (cycle + 1);
            }
            else
            {
                newEmaValue = closeValue;
            }
            return newEmaValue;
        }

        /// <summary>
        /// 获取最新的面板的ID
        /// </summary>
        private static int panelID = 0;

        public static int GetPanelID()
        {
            return panelID++;
        }

        /// <summary>
        /// 根据DateTime获取Timekey
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetTimeKey(DateTime dt)
        {
            string month = dt.Month.ToString().Length == 1 ? "0" + dt.Month.ToString() : dt.Month.ToString();
            string day = dt.Day.ToString().Length == 1 ? "0" + dt.Day.ToString() : dt.Day.ToString();
            string hour = dt.Hour.ToString().Length == 1 ? "0" + dt.Hour.ToString() : dt.Hour.ToString();
            string minute = dt.Minute.ToString().Length == 1 ? "0" + dt.Minute.ToString() : dt.Minute.ToString();
            string second = dt.Second.ToString().Length == 1 ? "0" + dt.Second.ToString() : dt.Second.ToString();
            return dt.Year + month + day + hour + minute + second;
        }

        /// <summary>
        /// 根据timeKey获取datetime
        /// </summary>
        /// <param name="timeKey"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeByTimeKey(string timeKey)
        {
            int year = timeKey.Length >= 4 ? Convert.ToInt32(timeKey.Substring(0, 4)) : 1970;
            int month = timeKey.Length >= 6 ? Convert.ToInt32(timeKey.Substring(4, 2)) : 1;
            int day = timeKey.Length >= 8 ? Convert.ToInt32(timeKey.Substring(6, 2)) : 1;
            int hr = timeKey.Length >= 10 ? Convert.ToInt32(timeKey.Substring(8, 2)) : 0;
            int mn = timeKey.Length >= 12 ? Convert.ToInt32(timeKey.Substring(10, 2)) : 0;
            int sc = timeKey.Length >= 14 ? Convert.ToInt32(timeKey.Substring(12, 2)) : 0;
            DateTime dt = new DateTime(year, month, day, hr, mn, sc);
            return dt;
        }

        /// <summary>
        /// 根据日期类型和时间获取经过处理后的TimeKey
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCalenderFormatTimeKey(string value, IntervalType interval)
        {
            string timeKey = string.Empty;
            switch (interval)
            {
                case IntervalType.Year:
                    timeKey = value.Substring(0, 4);
                    break;
                case IntervalType.Month:
                    timeKey = value.Substring(0, 4) + "/" + value.Substring(4, 2);
                    break;
                case IntervalType.Week:
                case IntervalType.Day:
                    timeKey = value.Substring(0, 4) + "/" + value.Substring(4, 2) + "/" + value.Substring(6, 2);
                    break;
                case IntervalType.Minute:
                    timeKey = value.Substring(8, 2) + ":" + value.Substring(10, 2);
                    break;
                case IntervalType.Second:
                    timeKey = value.Substring(8, 2) + ":" + value.Substring(10, 2) + ":" + value.Substring(12, 2);
                    break;
            }
            return timeKey;
        }

        /// <summary>
        /// 返回一组数据的最小值
        /// </summary>
        /// <param name="valueList"></param>
        /// <returns></returns>
        public static double GetLowValue(List<double> valueList)
        {
            double low = 0;
            for (int i = 0; i < valueList.Count; i++)
            {
                if (i == 0)
                {
                    low = valueList[i];
                }
                else
                {
                    if (low > valueList[i])
                    {
                        low = valueList[i];
                    }
                }
            }
            return low;
        }

        /// <summary>
        /// 返回一组数组的最大值
        /// </summary>
        /// <param name="valueList"></param>
        /// <returns></returns>
        public static double GetHighValue(List<double> valueList)
        {
            double high = 0;
            for (int i = 0; i < valueList.Count; i++)
            {
                if (i == 0)
                {
                    high = valueList[i];
                }
                else
                {
                    if (high < valueList[i])
                    {
                        high = valueList[i];
                    }
                }
            }
            return high;
        }

        /// <summary>
        /// 获取一个表中的数据的最小值的记录号
        /// </summary>
        /// <param name="dicValues"></param>
        /// <returns></returns>
        public static int GetLoweRecord(List<object[]> dicValues)
        {
            double low = 0;
            int index = -1;
            for (int i = 0; i < dicValues.Count; i++)
            {
                int j = Convert.ToInt32(dicValues[i][0]);
                double value = Convert.ToDouble(dicValues[i][1]);
                if (i == 0)
                {
                    index = j;
                    low = value;
                }
                else
                {
                    if (low > value)
                    {
                        index = j;
                        low = value;
                    }
                }
            }
            return index;
        }

        /// <summary>
        /// 获取一个表中的数据的最大值的记录号
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static int GetHighRecord(List<object[]> dicValues)
        {
            double high = 0;
            int index = -1;
            for (int i = 0; i < dicValues.Count; i++)
            {
                int j = Convert.ToInt32(dicValues[i][0]);
                double value = Convert.ToDouble(dicValues[i][1]);
                if (i == 0)
                {
                    index = j;
                    high = value;
                }
                else
                {
                    if (high < value)
                    {
                        high = value;
                        index = j;
                    }
                }
            }
            return index;
        }

        /// <summary>
        /// 根据保留小数的位置将double型转化为string型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static string GetValueByDigit(double value, int digit)
        {
            if (digit > 0)
            {
                StringBuilder sbFormat = new StringBuilder();
                string strValue = value.ToString();
                if (strValue.IndexOf(".") != -1)
                {
                    sbFormat.Append(strValue.Substring(0, strValue.IndexOf(".") + 1));
                    for (int i = 0; i < digit; i++)
                    {
                        int pos = strValue.IndexOf(".") + (i + 1);
                        if (pos <= strValue.Length - 1)
                        {
                            sbFormat.Append(strValue.Substring(pos, 1));
                        }
                        else
                        {
                            sbFormat.Append("0");
                        }
                    }
                }
                else
                {
                    sbFormat.Append(strValue + ".");
                    for (int i = 0; i < digit; i++)
                    {
                        sbFormat.Append("0");
                    }
                }
                return sbFormat.ToString();
            }
            return value.ToString();
        }

        /// <summary>
        /// 获取一个带圆弧角的矩形
        /// </summary>
        /// <param name="cornerRadius"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static GraphicsPath GetRoundRectangle(int cornerRadius, RectangleF rect)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }

        /// <summary>
        /// 计算坐标轴的函数
        /// </summary>
        /// <param name="XMin"></param>
        /// <param name="XMax"></param>
        /// <param name="N"></param>
        /// <param name="SMin"></param>
        /// <param name="Step"></param>
        /// <returns></returns>
        public static int GridScale(double XMin, double XMax, int N, ref double SMin, ref double Step, double m_fTick)
        {
            int iNegScl;
            int iNm1;
            double lfIniStep;
            double lfSclStep;
            double lfTmp;
            double lfSMax;
            int it;
            int i;
            int[] Steps = { 10, 12, 15, 16, 20, 25, 30, 40, 50, 60, 75, 80, 100, 120, 150 };
            int iNS = Steps.Length;
            if (XMin > XMax)
            {
                lfTmp = XMin;
                XMin = XMax;
                XMax = lfTmp;
            }
            if (XMin == XMax)
                XMax = XMin == 0.0 ? 1.0 : XMin + Math.Abs(XMin) / 10.0;
            if (XMax <= 0)
            {
                iNegScl = 1;
                lfTmp = XMin;
                XMin = -XMax;
                XMax = -lfTmp;
            }
            else
                iNegScl = 0;
            if (N < 2)
                N = 2;
            iNm1 = N - 1;
            for (it = 0; it < 3; it++)
            {
                lfIniStep = (XMax - XMin) / iNm1;
                lfSclStep = lfIniStep;

                int pow10 = 0;

                for (; lfSclStep < 10.0; lfSclStep *= 10.0) pow10--;
                for (; lfSclStep > 100.0; lfSclStep /= 10.0) pow10++;
                for (i = 0; i < iNS && lfSclStep > Steps[i]; i++) ;
                do
                {
                    Step = Steps[i] * Math.Pow(10, (double)pow10);

                    if (m_fTick != 0.0)
                    {
                        Step = Math.Floor(Step / m_fTick) * m_fTick;
                    }

                    SMin = Math.Floor(XMin / Step) * Step;
                    lfSMax = SMin + iNm1 * Step;
                    if (XMax <= lfSMax)
                    {
                        if (iNegScl == 1)
                            SMin = -lfSMax;
                        Step *= iNm1 / (N - 1);
                        return 1;
                    }
                    i++;
                }
                while (i < iNS);
                iNm1 *= 2;
            }
            return 0;
        }
    }
}
