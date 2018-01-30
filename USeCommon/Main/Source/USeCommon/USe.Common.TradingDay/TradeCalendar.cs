using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.Common.TradingDay
{
    /// <summary>
    /// 交易日历。
    /// </summary>
    public class TradeCalendar
    {
        /// <summary>
        /// 交易日历。
        /// </summary>
        public TradeCalendar()
        {

        }

        /// <summary>
        /// 交易日历。
        /// </summary>
        /// <param name="today">当日。</param>
        /// <param name="nextTradeDay">前一交易日。</param>
        /// <param name="preTradeDay">下一交易日。</param>
        /// <param name="isTradeDay">是否为交易日。</param>
        public TradeCalendar(DateTime today, DateTime nextTradeDay, DateTime preTradeDay, bool isTradeDay)
        {
            this.Today = today;
            this.NextTradeDay = nextTradeDay;
            this.PreTradeDay = preTradeDay;
            this.IsTradeDay = isTradeDay;
        }

        /// <summary>
        /// 当前交易日。
        /// </summary>
        public DateTime Today { get; set; }

        /// <summary>
        /// 前一交易日。
        /// </summary>
        public DateTime PreTradeDay { get; set; }

        /// <summary>
        /// 下一交易日。
        /// </summary>
        public DateTime NextTradeDay { get; set; }

        /// <summary>
        /// 是否为交易日。
        /// </summary>
        public bool IsTradeDay { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}",
                this.Today.ToString("yyyy-MM-dd"),
                this.IsTradeDay ? "Y" : "N",
                this.PreTradeDay.ToString("yyyy-MM-dd"),
                this.NextTradeDay.ToString("yyyy-MM-dd"));
        }
    }
}
