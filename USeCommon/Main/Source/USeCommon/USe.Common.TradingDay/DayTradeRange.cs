using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace USe.Common.TradingDay
{
    /// <summary>
    /// 日交易区间。
    /// </summary>
    public class DayTradeRange
    {
        private static TimeSpan ms_nightBeginTime = new TimeSpan(20, 30, 0); // 夜盘开始时间 21:00前推30分钟
        private static TimeSpan ms_daylightBeginTime = new TimeSpan(08, 30, 0); // 日盘开始时间 9:00前推30分钟
        private static TimeSpan ms_dayBeginTime = new TimeSpan(0, 0, 0);  // 一天开始时间
        private static TimeSpan ms_dayEndTime = new TimeSpan(24, 0, 0); // 一天截至事件
        #region member

        private Dictionary<DateTime, TradeCalendar> m_tradeCalendarDic = null;
        private List<TradeRangeItem> m_rangeList = null;
        #endregion

        /// <summary>
        /// 交易区间。
        /// </summary>
        /// <param name="tradeCalendars">交易日历。</param>
        /// <param name="rangeItems">交易区间。</param>
        public DayTradeRange(List<TradeCalendar> tradeCalendars, List<TradeRangeItem> rangeItems)
        {
            m_tradeCalendarDic = new Dictionary<DateTime, TradeCalendar>();

            if (tradeCalendars != null && tradeCalendars.Count > 0)
            {
                foreach (TradeCalendar calendar in tradeCalendars)
                {
                    m_tradeCalendarDic.Add(calendar.Today.Date, calendar);
                }
            }

            m_rangeList = new List<TradeRangeItem>();
            if (rangeItems != null && rangeItems.Count > 0)
            {
                m_rangeList.AddRange(rangeItems);
            }
        }

        /// <summary>
        /// 是否为交易时间。
        /// </summary>
        /// <param name="time">指定时间。</param>
        /// <returns></returns>
        public bool IsTradeTime(TimeSpan time)
        {
            foreach (TradeRangeItem item in m_rangeList)
            {
                if (item.Contains(time))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否为交易时段开始时间。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsBeginTime(TimeSpan time)
        {
            foreach (TradeRangeItem item in m_rangeList)
            {
                if (item.IsBeginTime(time))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否为交易时段截至时间。
        /// </summary>
        /// <param name="time">指定时间。</param>
        /// <returns></returns>
        public bool IsEndTime(TimeSpan time)
        {
            foreach (TradeRangeItem item in m_rangeList)
            {
                if (item.IsEndTime(time))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取所属交易日(夜盘属于下一交易日)。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DateTime GetTradeDay(DateTime time)
        {
            TimeSpan timeOfDay = time.TimeOfDay;

            if (timeOfDay >= ms_daylightBeginTime && timeOfDay < ms_nightBeginTime)
            {
                return time.Date;
            }
            else if (timeOfDay >= ms_nightBeginTime && timeOfDay <= ms_dayEndTime)
            {
                return GetNextTradeDay(time.Date);
            }
            else
            {
                Debug.Assert(timeOfDay >= ms_dayBeginTime && timeOfDay < ms_daylightBeginTime);
                return GetNextTradeDay(time.Date.AddDays(-1));
            }
        }

        /// <summary>
        /// 后去指定日下一交易日。
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private DateTime GetNextTradeDay(DateTime date)
        {
            TradeCalendar calendar = null;
            if (m_tradeCalendarDic.TryGetValue(date, out calendar) == false)
            {
                throw new Exception(string.Format("Can't calculat {0:yyyy-MM-dd} next tradeDay",date));
            }
            return calendar.NextTradeDay;
        }
    }
}

