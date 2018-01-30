using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.Common
{
    /// <summary>
    /// 格式化类。
    /// </summary>
    public static class USeFormat
    {
        /// <summary>
        /// 日期格式化(例:2017-07-10)。
        /// </summary>
        public static string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// 时间格式化(例:2017-07-10 16:30:12)。
        /// </summary>
        public static string ShortTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 时间格式化(例:2017-07-10 16:30:12.123)。
        /// </summary>
        public static string LongTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>
        /// 时间格式化(例:16:30:12)。
        /// </summary>
        public static string ShortOnlyTimeFormat = "HH:mm:ss";

        /// <summary>
        /// 时间格式化(例:16:30:12.123)。
        /// </summary>
        public static string LongOnlyTimeFormat = "HH:mm:ss.fff";

        /// <summary>
        /// TimeSpan格式化(例:16:30:12)。
        /// </summary>
        public static string ShortTimeSpanFormat = @"HH\:mm\:ss";

        /// <summary>
        /// 时间格式化(例:16:30:12.123)。
        /// </summary>
        public static string LongTimeSpanFormat = @"HH\:mm\:ss\.fff";

        /// <summary>
        /// 日期格式化(例:2017-07-10)。
        /// </summary>
        public static string ToDate(this DateTime time)
        {
            return time.ToString(DateFormat);
        }

        /// <summary>
        /// 时间格式化(例:2017-07-10 16:30:12)。
        /// </summary>
        public static string ToShortTime(this DateTime time)
        {
            return time.ToString(ShortTimeFormat);
        }

        /// <summary>
        /// 时间格式化(例:2017-07-10 16:30:12.123)。
        /// </summary>
        public static string ToLongTime(this DateTime time)
        {
            return time.ToString(LongTimeFormat);
        }

        /// <summary>
        /// 时间格式化(例:16:30:12)。
        /// </summary>
        public static string ToShortOnlyTime(this DateTime time)
        {
            return time.ToString(ShortOnlyTimeFormat);
        }

        /// <summary>
        /// 时间格式化(例:16:30:12.123)。
        /// </summary>
        public static string ToLongOnlyTime(this DateTime time)
        {
            return time.ToString(LongOnlyTimeFormat);
        }

        //[yangming] 3.5不支持
        ///// <summary>
        ///// TimeSpan格式化(例:16:30:12)。
        ///// </summary>
        //public static string ToShortTime(this TimeSpan time)
        //{
        //    return time.ToString(ShortTimeSpanFormat);
        //}

        ///// <summary>
        ///// 时间格式化(例:16:30:12.123)。
        ///// </summary>
        //public static string ToLongTime(this TimeSpan time)
        //{
        //    return time.ToString(LongTimeSpanFormat);
        //}
    }
}
