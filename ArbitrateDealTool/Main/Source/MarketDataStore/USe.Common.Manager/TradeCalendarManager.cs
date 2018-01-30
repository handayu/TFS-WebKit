using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;
using MySql.Data.MySqlClient;
using USe.Common.TradingDay;
using USe.Common.DBDriver;

namespace USe.Common.Manager
{
    /// <summary>
    /// 交易日历管理类。
    /// </summary>
    public class TradeCalendarManager
    {
        #region member
        private string m_dbConnStr = string.Empty;
        private string m_alphaDBName = string.Empty;

        private Dictionary<DateTime, TradeCalendar> m_tradingDateDic = null;
        #endregion //member

        #region construction
        /// <summary>
        /// TradeCalendarManager。
        /// </summary>
        /// <param name="dbConnStr">数据库连接字符串。</param>
        /// <param name="alpahDBName">Alpha数据库名。</param>
        public TradeCalendarManager(string dbConnStr, string alpahDBName)
        {
            Debug.Assert(string.IsNullOrEmpty(dbConnStr) == false);
            Debug.Assert(string.IsNullOrEmpty(alpahDBName) == false);

            m_dbConnStr = dbConnStr;
            m_alphaDBName = alpahDBName;

            m_tradingDateDic = new Dictionary<DateTime, TradeCalendar>();
        }
        #endregion //construction

        #region method
        /// <summary>
        /// 初始化。
        /// </summary>
        public void Initialize(DateTime? beginDate = null, DateTime? endDate = null)
        {
            m_tradingDateDic.Clear();

            StringBuilder sbCmdText = new StringBuilder();
            sbCmdText.Append(string.Format("select * from {0}.trade_calendar", m_alphaDBName));
            if (beginDate.HasValue || endDate.HasValue)
            {
                sbCmdText.Append(" where");
                bool flag = false;
                if (beginDate.HasValue)
                {
                    if (flag)
                    {
                        sbCmdText.Append(" and");
                    }
                    sbCmdText.Append(string.Format(" (today >= '{0:yyyy-MM-dd}')", beginDate.Value));
                    flag = true;
                }

                if (endDate.HasValue)
                {
                    if (flag)
                    {
                        sbCmdText.Append(" and");
                    }
                    sbCmdText.Append(string.Format(" (today <= '{0:yyyy-MM-dd}')", endDate.Value));
                    flag = true;
                }
            }
            sbCmdText.Append(" order by today;");

            DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, sbCmdText.ToString());
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    TradeCalendar calendar = new TradeCalendar() {
                        IsTradeDay = Convert.ToBoolean(row["is_trading_day"]),
                        NextTradeDay = Convert.ToDateTime(row["next_trade_day"]),
                        PreTradeDay = Convert.ToDateTime(row["pre_trade_day"]),
                        Today = Convert.ToDateTime(row["today"])
                    };
                    m_tradingDateDic.Add(calendar.Today, calendar);
                }
            }
        }

        /// <summary>
        /// 验证是否为交易日。
        /// </summary>
        /// <param name="date">指定日。</param>
        /// <returns></returns>
        public bool IsTradingDate(DateTime date)
        {
            date = date.Date;
            if (m_tradingDateDic.ContainsKey(date))
            {
                return m_tradingDateDic[date].IsTradeDay;
            }
            else
            {
                throw new Exception(string.Format("未找到{0:yyyy-MM-dd}交易日历", date));
            }
        }

        /// <summary>
        /// 获取指定日前一交易日。
        /// </summary>
        /// <param name="date">指定日。</param>
        /// <returns></returns>
        public DateTime GetNextTradingDate(DateTime date)
        {
            date = date.Date;
            if (m_tradingDateDic.ContainsKey(date))
            {
                return m_tradingDateDic[date].NextTradeDay;
            }
            else
            {
                throw new Exception(string.Format("未找到{0:yyyy-MM-dd}交易日历", date));
            }
        }

        /// <summary>
        /// 获取指定日前一交易日。
        /// </summary>
        /// <param name="date">指定日。</param>
        /// <returns></returns>
        public DateTime GetPreTradingDate(DateTime date)
        {
            date = date.Date;
            if (m_tradingDateDic.ContainsKey(date))
            {
                return m_tradingDateDic[date].PreTradeDay;
            }
            else
            {
                throw new Exception(string.Format("未找到{0:yyyy-MM-dd}交易日历", date));
            }
        }

        /// <summary>
        /// 获取指定日交易日历信息。
        /// </summary>
        /// <param name="date">指定日。</param>
        /// <returns></returns>
        public TradeCalendar GetTradeCalendar(DateTime date)
        {
            date = date.Date;
            if (m_tradingDateDic.ContainsKey(date.Date))
            {
                return m_tradingDateDic[date];
            }
            else
            {
                throw new Exception(string.Format("未找到{0:yyyy-MM-dd}交易日历", date));
            }
        }
        #endregion //method
    }
}
