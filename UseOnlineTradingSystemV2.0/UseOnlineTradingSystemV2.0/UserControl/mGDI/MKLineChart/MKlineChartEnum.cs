using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mPaint
{
    /// <summary>
    /// 标记的类型
    /// </summary>
    public enum SignalType
    {
        UpArrow,
        DownArrow,
        UpArrowWithOutTail,
        DownArrowWithOutTail,
        LeftArrow,
        RightArrow
    }

    /// <summary>
    /// 周期类型
    /// </summary>
    public enum IntervalType
    {
        Second,
        Minute,
        Day,
        Week,
        Month,
        Year
    }

    /// <summary>
    /// 指标的类型
    /// </summary>
    public enum IndicatorType
    {
        SimpleMovingAverage,
        ExponentialMovingAverage,
        StochasticOscillator
    }
}
