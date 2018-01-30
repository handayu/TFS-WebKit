using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreCommon
{
    public enum DayNightType
    {
        /// <summary>
        /// 日盘
        /// </summary>
        Day = 0,

        /// <summary>
        /// 夜盘
        /// </summary>
        Night = 1,
    }

    public static class DayNightTypeExtend
    {
        public static string ToDescription(this DayNightType type)
        {
            switch(type)
            {
                case DayNightType.Day: return "日盘";
                case DayNightType.Night:return "夜盘";
                default:return "未知";
            }
        }
    }
}
