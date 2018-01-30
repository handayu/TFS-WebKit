using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using USe.Common.TradingDay;
using System.Diagnostics;
using USe.Common.DBDriver;
using System.Data;

namespace DataStoreCommon
{
    public class TradeRangeManager:ITradeRangeManager
    {
        private string m_dbConnStr = string.Empty;
        private string m_alphaDBName = string.Empty;

        private Dictionary<string, List<TradeRangeItem>> m_tradeRangeDic = null;

        private List<TradeCalendar> m_tradeCalendarList = null;

        public TradeRangeManager(string dbConnStr,string alphaDBName)
        {
            if(string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentNullException("dbConnStr");
            }
            if (string.IsNullOrEmpty(alphaDBName))
            {
                throw new ArgumentNullException("alphaDBName");
            }
            m_dbConnStr = dbConnStr;
            m_alphaDBName = alphaDBName;
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        public void Initialize()
        {
            LoadTradeCalendar();
            LoadTradeRange();
        }

        /// <summary>
        /// 加载交易区间。
        /// </summary>
        private void LoadTradeRange()
        {
            try
            {
                string strSel = string.Format(@"select * from {0}.future_trade_range;", m_alphaDBName);

                DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

                Dictionary<string, List<TradeRangeItem>> tradeRangeDic = new Dictionary<string, List<TradeRangeItem>>();
                foreach (DataRow row in table.Rows)
                {
                    string varieties = row["varieties"].ToString();
                    List<TradeRangeItem> rangeList = null;
                    if (tradeRangeDic.TryGetValue(varieties, out rangeList) == false)
                    {
                        rangeList = new List<TradeRangeItem>(4);
                        tradeRangeDic.Add(varieties, rangeList);
                    }

                    TimeSpan beginTime;
                    TimeSpan endTime;
                    if (TimeSpan.TryParse(row["begin_time"].ToString(), out beginTime) == false)
                    {
                        throw new ArgumentException(string.Format("Invalid begin_time {0}", row["begin_time"].ToString()));
                    }
                    if (TimeSpan.TryParse(row["end_time"].ToString(), out endTime) == false)
                    {
                        throw new ArgumentException(string.Format("Invalid end_time {0}", row["end_time"].ToString()));
                    }
                    bool isNight = Convert.ToInt32(row["is_night"]) == 1;

                    TradeRangeItem rangeItem = new TradeRangeItem(beginTime, endTime, isNight);
                    rangeList.Add(rangeItem);
                }

                m_tradeRangeDic = tradeRangeDic;
            }
            catch (Exception ex)
            {
                throw new Exception("加载交易区间失败," + ex.Message);
            }
        }

        /// <summary>
        /// 加载交易日历。
        /// </summary>
        private void LoadTradeCalendar()
        {
            try
            {
                string strSel = string.Format(@"select * from {0}.trade_calendar where today >= '{1}' 
order by today limit 0,15 ;",m_alphaDBName,DateTime.Today.ToString("yyyy-MM-dd"));

                DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

                List<TradeCalendar> calendarList = new List<TradeCalendar>();
                foreach(DataRow row in table.Rows)
                {
                    TradeCalendar calendar = new TradeCalendar() {
                        Today = Convert.ToDateTime(row["today"]),
                        IsTradeDay = Convert.ToInt32(row["is_trading_day"]) == 1,
                        PreTradeDay = Convert.ToDateTime(row["pre_trade_day"]),
                        NextTradeDay = Convert.ToDateTime(row["next_trade_day"])
                    };
                    calendarList.Add(calendar);
                }

                m_tradeCalendarList = calendarList;
            }
            catch (Exception ex)
            {
                throw new Exception("加载交易日历失败," + ex.Message);
            }
        }

        /// <summary>
        /// 创建交易区间。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <returns></returns>
        public DayTradeRange CreateTradeRange(USeInstrument instrument)
        {
            string varieties = USeTraderProtocol.GetVarieties(instrument.InstrumentCode);
            return CreateTradeRange(varieties);
        }

        /// <summary>
        /// 创建交易区间。
        /// </summary>
        /// <param name="varieties">品种。</param>
        /// <returns></returns>
        public DayTradeRange CreateTradeRange(string varieties)
        {
            List<TradeRangeItem> tradeRangeList = null;
            if (m_tradeRangeDic.TryGetValue(varieties, out tradeRangeList) == false)
            {
                Debug.Assert(false);
                tradeRangeList = new List<TradeRangeItem>();
            }

            DayTradeRange dayTradeRange = new DayTradeRange(m_tradeCalendarList, tradeRangeList);
            return dayTradeRange;
        }
    }
}
