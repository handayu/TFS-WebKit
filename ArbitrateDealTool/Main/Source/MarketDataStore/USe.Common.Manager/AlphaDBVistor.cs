using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using USe.Common.DBDriver;
using USe.TradeDriver.Common;
using USe.Common;

namespace USe.Common.Manager
{
    /// <summary>
    /// Alpha数据库访问器。
    /// </summary>
    public class AlphaDBVistor
    {
        #region member
        private string m_dbConnStr = string.Empty;
        private string m_alphaDBName = string.Empty;
        #endregion

        #region
        /// <summary>
        /// 构造方法。
        /// </summary>
        /// <param name="dbConnStr">MySql数据连接串。</param>
        /// <param name="alphaDBName">Alpha数据库名。</param>
        public AlphaDBVistor(string dbConnStr, string alphaDBName)
        {
            m_dbConnStr = dbConnStr;
            m_alphaDBName = alphaDBName;
        }
        #endregion

        #region methods
        /// <summary>
        /// 获取合约指定交易日日K线。
        /// </summary>
        public USeKLine GetDayKLine(USeInstrument instrument, DateTime day)
        {
            string strSel = string.Format(@"select * from {0}.day_kline 
where contract = '{1}' and exchange = '{2}' and date_time = '{3:yyyy-MM-dd}';",
            m_alphaDBName, instrument.InstrumentCode, instrument.Market.ToString(), day);

            DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

            if (table != null && table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];

                USeKLine kline = new USeKLine() {
                    InstrumentCode = row["contract"].ToString(),
                    Market = USeTraderProtocol.ToUseMarket(row["exchange"].ToString()),
                    Cycle = USeCycleType.Day,
                    DateTime = Convert.ToDateTime(row["date_time"]),
                    Open = row["price_open"].ToDecimal(),
                    High = row["price_high"].ToDecimal(),
                    Low = row["price_low"].ToDecimal(),
                    Close = row["price_close"].ToDecimal(),
                    PreSettlementPrice = row["pre_settlement_price"].ToDecimal(),
                    SettlementPrice = row["settlement_price"].ToDecimal(),
                    Volumn = row["volumn"].ToInt(),
                    Turnover = row["turnover"].ToDecimal(),
                    OpenInterest = row["openinterest"].ToDecimal(),
                    AskVolumn = row["ask_volumn"].ToInt(),
                    BidVolumn = row["bid_volumn"].ToInt()
                };
                return kline;
            }

            return null;
        }

        /// <summary>
        /// 获取合约指定交易日日K线。
        /// </summary>
        public USeKLine GetMin1KLine(USeInstrument instrument, DateTime time)
        {
            string strSel = string.Format(@"select * from {0}.{1} 
where contract = '{1}' and date_time = '{3:yyyy-MM-dd HH:mm:00}';",
            m_alphaDBName,GetMin1KLineTableName(instrument.Market),
            instrument.InstrumentCode, time);

            DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

            if (table != null && table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];

                USeKLine kline = new USeKLine() {
                    InstrumentCode = row["contract"].ToString(),
                    Market = USeTraderProtocol.ToUseMarket(row["exchange"].ToString()),
                    Cycle = USeCycleType.Day,
                    DateTime = Convert.ToDateTime(row["date_time"]),
                    Open = row["price_open"].ToDecimal(),
                    High = row["price_high"].ToDecimal(),
                    Low = row["price_low"].ToDecimal(),
                    Close = row["price_close"].ToDecimal(),
                    Volumn = row["volumn"].ToInt(),
                    Turnover = row["turnover"].ToDecimal(),
                    OpenInterest = row["openinterest"].ToDecimal()
                };
                return kline;
            }

            return null;
        }
        #endregion

        /// <summary>
        /// 获取一分钟K线表名。
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        private string GetMin1KLineTableName(USeMarket market)
        {
            string tableName = string.Empty;

            switch (market)
            {
                case USeMarket.CFFEX:
                case USeMarket.CZCE:
                case USeMarket.DCE:
                case USeMarket.SHFE:
                    tableName = string.Format("min1_kline_{0}", market.ToString().ToLower());
                    break;
                default:
                    Debug.Assert(false);
                    throw new Exception("Invalid market:" + market.ToString());
            }
            return tableName;
        }
    }
}
