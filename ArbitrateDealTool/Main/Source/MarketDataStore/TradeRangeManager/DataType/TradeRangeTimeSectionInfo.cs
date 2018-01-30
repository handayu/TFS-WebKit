using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeRangeManager
{
    public class TradeRangeTimeSectionInfo
    {
        private string m_beginTime;
        private string m_endTime;
        private bool m_isNight = false;

        public TradeRangeTimeSectionInfo(string beginTime,
            string endTime,
            bool isNight)
        {
            this.BeginTime = beginTime;
            this.EndTime = endTime;
            this.IsNight = isNight;
        }

        public TradeRangeTimeSectionInfo()
        {

        }
        /// <summary>
        /// beginTime
        /// </summary>
        public string BeginTime
        {
            get { return m_beginTime; }
            set { m_beginTime = value; }
        }

        /// <summary>
        /// EndTime
        /// </summary>
        public string EndTime
        {
            get { return m_endTime; }
            set { m_endTime = value; }
        }

        /// <summary>
        /// IsNight是否为夜盘时间。
        /// </summary>
        public bool IsNight
        {
            get { return m_isNight; }
            set { m_isNight = value; }
        }

    }
}
