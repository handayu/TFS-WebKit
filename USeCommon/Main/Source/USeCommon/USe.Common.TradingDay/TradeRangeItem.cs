using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace USe.Common.TradingDay
{
    /// <summary>
    /// 交易时间段。
    /// </summary>
    public class TradeRangeItem
    {
        #region member
        private static TimeSpan ms_BeginDay = new TimeSpan(0, 0, 0);
        private static TimeSpan ms_EndDay = new TimeSpan(24, 0, 0);
        #endregion

        #region construction
        public TradeRangeItem(TimeSpan beginTime, TimeSpan endTime, bool isNight)
        {
            this.BeginTime = beginTime;
            this.EndTime = endTime;
            this.IsNight = isNight;
        }
        #endregion

        #region property
        /// <summary>
        /// 开始时间。
        /// </summary>
        public TimeSpan BeginTime { get; private set; }

        /// <summary>
        /// 结束时间。
        /// </summary>
        public TimeSpan EndTime { get; private set; }

        /// <summary>
        /// 是否为夜盘。
        /// </summary>
        public bool IsNight { get; private set; }
        #endregion

        #region public methods
        /// <summary>
        /// 是否包含在区间内。
        /// </summary>
        /// <param name="time">指定时间。</param>
        /// <returns></returns>
        public bool Contains(TimeSpan time)
        {
            if (this.EndTime < this.BeginTime)
            {
                Debug.Assert(this.IsNight == true);
                if (time >= this.BeginTime && time <= ms_EndDay)
                {
                    return true;
                }
                else if (time >= ms_BeginDay && time <= this.EndTime)
                {
                    return true;
                }

                return false;
            }
            else
            {
                return (time >= this.BeginTime && time <= this.EndTime);
            }
        }

        /// <summary>
        /// 是否为起始时段。
        /// </summary>
        /// <param name="time">指定时间。</param>
        /// <returns></returns>
        public bool IsBeginTime(TimeSpan time)
        {
            if (Math.Floor(time.TotalSeconds) == Math.Floor(this.BeginTime.TotalSeconds))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否为截至时段。
        /// </summary>
        /// <param name="time">指定时间。</param>
        /// <returns></returns>
        public bool IsEndTime(TimeSpan time)
        {
            if (Math.Floor(time.TotalSeconds) == Math.Floor(this.EndTime.TotalSeconds))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        public override string ToString()
        {
            return string.Format("{0} {1}~{2}", this.IsNight ? "Night   " : "Daylight", this.BeginTime, this.EndTime);
        }
    }
}
