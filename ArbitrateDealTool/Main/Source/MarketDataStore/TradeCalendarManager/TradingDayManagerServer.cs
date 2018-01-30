using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Configuration;
using USe.Common.AppLogger;
using TradeCalendarManager.Configuration;
using USe.Common.TradingDay;
using MySql.Data.MySqlClient;

namespace TradeCalendarManager
{
    /// <summary>
    /// 交易日维护程序。
    /// </summary>
    internal class TradingDayManagerServer
    {
        #region member
        private string m_dbConn = string.Empty;         // 数据库连接串
        private IAppLogger m_serverLogger = null;
        #endregion // member

        #region construction
        /// <summary>
        /// 构造TradingDayManagerServer实例。
        /// </summary>
        public TradingDayManagerServer()
        {
        }
        #endregion // construction

        /// <summary>
        /// 运行。
        /// </summary>
        public int Start()
        {
            try
            {
                m_serverLogger = AppLogger.InitInstance();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Initialize applogger failed," + ex.Message);
            }

            m_serverLogger.LineFeed();

            string message = string.Empty;
            TradingDaySection config = ConfigurationManager.GetSection("TradingDayManager") as TradingDaySection;
            if (config == null)
            {
                message = "Not found [TradingDayManager] config.";
                WriteConsoleLog(message);
                m_serverLogger.WriteError(message);
                return -1;
            }

            try
            {
                m_dbConn = ConfigurationManager.ConnectionStrings["KLineDB"].ConnectionString;
                if (string.IsNullOrEmpty(m_dbConn))
                {
                    throw new ApplicationException("KLineDB connection string is null");
                }
            }
            catch (Exception ex)
            {
                message = "Read db connection string failed," + ex.Message;
                WriteConsoleLog(message);
                m_serverLogger.WriteError(message);
                return -1;
            }


            message = "Begin process trading day.";
            WriteConsoleLog(message);
            m_serverLogger.WriteInformation(message);


            DateTime startDate;
            DateTime endDate;

            startDate = config.BeginDay;
            endDate = config.EndDay;

            List<string> commands = new List<string>();
            DateTime currDate = startDate;

            List<TradeCalendar> calendarList = new List<TradeCalendar>();
            while (currDate <= endDate)
            {
                bool isTradingDay = false;
                switch (currDate.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        isTradingDay = !IsHoliday(config, currDate);
                        break;
                    default:
                        isTradingDay = false;
                        break;
                }

                TradeCalendar calendar = new TradeCalendar() {
                    Today = currDate,
                    IsTradeDay = isTradingDay,
                    PreTradeDay = DateTime.MinValue,
                    NextTradeDay = DateTime.MinValue
                };

                calendarList.Add(calendar);

                currDate = currDate.AddDays(1);
            }


            for (int i = 0; i < calendarList.Count; i++)
            {
                TradeCalendar calendar = calendarList[i];
                //补前一交易日
                for (int j = i - 1; j >= 0; j--)
                {
                    if (calendarList[j].IsTradeDay)
                    {
                        calendar.PreTradeDay = calendarList[j].Today;
                        break;
                    }
                }
                if (calendar.PreTradeDay == DateTime.MinValue)
                {
                    calendar.PreTradeDay = config.BeginDayPreTradeDay;
                }
                //补下一交易日
                for (int j = i + 1; j < calendarList.Count; j++)
                {
                    if (calendarList[j].IsTradeDay)
                    {
                        calendar.NextTradeDay = calendarList[j].Today;
                        break;
                    }
                }
                if (calendar.NextTradeDay == DateTime.MinValue)
                {
                    calendar.NextTradeDay = config.EndDayNextTradeDay;
                }
            }

            try
            {
                SaveToMySqlDB(calendarList, config.BeginDay, config.EndDay);
            }
            catch (Exception ex)
            {
                message = string.Format("Save data to db failed,Error:{0}.", ex.Message);
                WriteConsoleLog(message);
                m_serverLogger.WriteError(message);
                return -2;
            }

            message = "End process trading day.";
            WriteConsoleLog(message);
            m_serverLogger.WriteInformation(message);
            return 0;
        }

        /// <summary>
        /// 指定日期是否为节假日。
        /// </summary>
        /// <param name="config"></param>
        /// <param name="date">指定日期。</param>
        /// <returns></returns>
        private bool IsHoliday(TradingDaySection config, DateTime date)
        {
            if (config.Holidays != null && config.Holidays.Count > 0)
            {
                foreach (HolidayItemElement element in config.Holidays)
                {
                    if (date >= element.BeginDay.Date && date <= element.EndDay.Date)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void SaveToMySqlDB(List<TradeCalendar> calendarList, DateTime beginDate, DateTime endDate)
        {

            try
            {
                using (MySqlConnection connection = new MySqlConnection(m_dbConn))
                {
                    connection.Open();
                    {
                        string strDel = string.Format(@"delete from alpha.trade_calendar 
where today >='{0:yyyy-MM-dd}' and today <= '{1:yyyy-MM-dd}'",
                            beginDate, endDate);
                        MySqlCommand command = new MySqlCommand(strDel, connection);
                        command.ExecuteNonQuery();
                    }

                    string cmdText = @"insert into alpha.trade_calendar(today,is_trading_day,pre_trade_day,next_trade_day)
values(@today,@is_trading_day,@pre_trade_day,@next_trade_day)";

                    foreach (TradeCalendar calendar in calendarList)
                    {
                        MySqlCommand command = new MySqlCommand(cmdText, connection);
                        command.Parameters.AddWithValue("@today", calendar.Today);
                        command.Parameters.AddWithValue("@is_trading_day", calendar.IsTradeDay);
                        command.Parameters.AddWithValue("@pre_trade_day", calendar.PreTradeDay);
                        command.Parameters.AddWithValue("@next_trade_day", calendar.NextTradeDay);

                        int result = command.ExecuteNonQuery();
                        Debug.Assert(result == 1);
                    }
                }
            }
            catch (Exception ex)
            {
                m_serverLogger.WriteError("保存交易日历数据失败," + ex.Message);
            }
        }

        private void WriteConsoleLog(string message)
        {
            Console.WriteLine("==>{0:HH:mm:ss} {1}", DateTime.Now, message);
        }
    }
}
