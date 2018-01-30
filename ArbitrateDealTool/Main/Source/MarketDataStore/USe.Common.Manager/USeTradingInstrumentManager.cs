using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;
using USe.TradeDriver.Common;
using USe.Common.DBDriver;
using USe.Common;

namespace USe.Common.Manager
{
    /// <summary>
    /// 合约管理类。
    /// </summary>
    public class USeTradingInstrumentManager
    {
        #region member
        private string m_dbConnStr = string.Empty;
        private string m_alphaDBName = string.Empty;
        public List<USeInstrumentDetail> m_instrumentList = null;
        #endregion

        #region
        /// <summary>
        /// 构造方法。
        /// </summary>
        /// <param name="dbConnStr">MySql数据连接串。</param>
        /// <param name="alphaDBName">Alpha数据库名。</param>
        public USeTradingInstrumentManager(string dbConnStr, string alphaDBName)
        {
            m_dbConnStr = dbConnStr;
            m_alphaDBName = alphaDBName;
            m_instrumentList = new List<USeInstrumentDetail>();
        }
        #endregion

        #region methods
        /// <summary>
        /// 初始化方法。
        /// </summary>
        public void Initialize(DateTime tradeDay)
        {
            string strSel = string.Format(@"select * from {0}.contracts
where open_date <= '{1}' and expire_date >= '{1}' and exchange in({2});",
                m_alphaDBName, tradeDay.ToString("yyyy-MM-dd"), USeTraderProtocol.GetInternalFutureMarketSqlString());

            DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

            List<USeInstrumentDetail> instrumentList = new List<USeInstrumentDetail>();
            foreach (DataRow row in table.Rows)
            {
                USeMarket exchange = (USeMarket)Enum.Parse(typeof(USeMarket), row["exchange"].ToString());

                USeInstrumentDetail instrument = new USeInstrumentDetail()
                {
                    Instrument = new USeInstrument(row["contract"].ToString(),
                                                   row["contract_name"].ToString(),
                                                   exchange),
                    OpenDate = Convert.ToDateTime(row["open_date"]),
                    ExpireDate = Convert.ToDateTime(row["expire_date"]),
                    StartDelivDate = Convert.ToDateTime(row["start_deliv_date"]),
                    EndDelivDate = Convert.ToDateTime(row["end_deliv_date"]),
                    VolumeMultiple = Convert.ToInt32(row["volume_multiple"]),
                    IsTrading = true,
                    Varieties = row["varieties"].ToString(),
                    PriceTick = Convert.ToDecimal(row["price_tick"]),
                    ExchangeLongMarginRatio = Convert.ToDecimal(row["exchange_long_margin_ratio"]),
                    ExchangeShortMarginRatio = Convert.ToDecimal(row["exchange_short_margin_ratio"]),
                    ProductClass = (USeProductClass)Enum.Parse(typeof(USeProductClass), row["product_class"].ToString()),
                    UnderlyingInstrument = row["underlying_instrument"].ToString(),
                    MaxMarketOrderVolume = Convert.ToInt16(row["max_market_order_volume"]),
                    MinMarketOrderVolume = Convert.ToInt16(row["min_market_order_volume"]),
                    MaxLimitOrderVolume = Convert.ToInt16(row["max_limit_order_volume"]),
                    MinLimitOrderVolume = Convert.ToInt16(row["min_limit_order_volume"])
                };
                instrumentList.Add(instrument);
            }

            m_instrumentList = instrumentList;
        }

        /// <summary>
        /// 初始化方法
        /// </summary>
        public void Initialize()
        {
            string strSel = string.Format(@"select * from {0}.contracts;",
                m_alphaDBName, USeTraderProtocol.GetInternalFutureMarketSqlString());

            DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

            List<USeInstrumentDetail> instrumentList = new List<USeInstrumentDetail>();
            foreach (DataRow row in table.Rows)
            {
                try
                {
                    if(row["exchange_long_margin_ratio"] == DBNull.Value
                        || row["exchange_short_margin_ratio"] == DBNull.Value)
                    {
                        continue;
                    }

                    USeMarket exchange = (USeMarket)Enum.Parse(typeof(USeMarket), row["exchange"].ToString());

                    USeInstrumentDetail instrument = new USeInstrumentDetail();

                    instrument.Instrument = new USeInstrument(row["contract"].ToString(),
                                                 row["contract_name"].ToString(),
                                                   exchange);
                    instrument.OpenDate = Convert.ToDateTime(row["open_date"]);
                    instrument.ExpireDate = Convert.ToDateTime(row["expire_date"]);
                    instrument.StartDelivDate = Convert.ToDateTime(row["start_deliv_date"]);
                    instrument.EndDelivDate = Convert.ToDateTime(row["end_deliv_date"]);
                    instrument.VolumeMultiple = Convert.ToInt32(row["volume_multiple"]);
                    instrument.IsTrading = true;
                    instrument.Varieties = row["varieties"].ToString();
                    instrument.PriceTick = Convert.ToDecimal(row["price_tick"]);
                    instrument.ExchangeLongMarginRatio = (row["exchange_long_margin_ratio"] != DBNull.Value) ? Convert.ToDecimal(row["exchange_long_margin_ratio"]) : 0m;
                    instrument.ExchangeShortMarginRatio = (row["exchange_short_margin_ratio"] != DBNull.Value) ? Convert.ToDecimal(row["exchange_short_margin_ratio"]) : 0m;
                    instrument.ProductClass = (USeProductClass)Enum.Parse(typeof(USeProductClass), row["product_class"].ToString());
                    instrument.UnderlyingInstrument = (row["underlying_instrument"] != DBNull.Value)? row["underlying_instrument"].ToString():"";
                    instrument.MaxMarketOrderVolume = (row["max_market_order_volume"] != DBNull.Value) ? Convert.ToInt16(row["max_market_order_volume"]) : 0;
                    instrument.MinMarketOrderVolume = (row["min_market_order_volume"] != DBNull.Value) ? Convert.ToInt16(row["min_market_order_volume"]) : 0;
                    instrument.MaxLimitOrderVolume = (row["max_limit_order_volume"] != DBNull.Value) ? Convert.ToInt16(row["max_limit_order_volume"]) : 0;
                    instrument.MinLimitOrderVolume = (row["min_limit_order_volume"] != DBNull.Value) ? Convert.ToInt16(row["min_limit_order_volume"]) : 0;

                    instrumentList.Add(instrument);
                }
                catch(Exception ex)
                {
                    throw new Exception("初始化合约信息异常:" + ex.Message);
                }
                
            }

            m_instrumentList = instrumentList;
        }


        /// <summary>
        /// 获取全部合约明细
        /// </summary>
        /// <returns></returns>
        public List<USeInstrumentDetail> GetAllInstrumentDetails()
        {
            List<USeInstrumentDetail> list = (from i in m_instrumentList
                                              select i.Clone()).ToList();
            return list;
        }

        /// <summary>
        /// 获取品种所有合约明细。
        /// </summary>
        /// <returns></returns>
        public List<USeInstrumentDetail> GetAllInstrumentDetails(string productId, USeMarket exchange)
        {
            List<USeInstrumentDetail> list = (from i in m_instrumentList
                                              where i.Varieties == productId && i.Instrument.Market == exchange
                                              select i.Clone()).ToList();
            return list;
        }

        /// <summary>
        /// 获取产品明细。
        /// </summary>
        /// <param name="instrumentCode">合约代码。</param>
        /// <param name="exchange">市场。</param>
        /// <returns></returns>
        public USeInstrumentDetail GetInstrumentDetail(string instrumentCode, USeMarket exchange)
        {
            USeInstrumentDetail detail = (from i in m_instrumentList
                                          where i.Instrument.InstrumentCode == instrumentCode && i.Instrument.Market == exchange
                                          select i).FirstOrDefault();
            return detail;
        }

        /// <summary>
        /// 获取全部合约明细
        /// </summary>
        /// <returns></returns>
        public List<USeInstrument> GetAllInstruments()
        {
            List<USeInstrument> list = (from i in m_instrumentList
                                        select i.Instrument.Clone()).ToList();
            return list;
        }

        /// <summary>
        /// 获取品种所有合约明细。
        /// </summary>
        /// <returns></returns>
        public List<USeInstrument> GetAllInstruments(string productId, USeMarket exchange)
        {
            List<USeInstrument> list = (from i in m_instrumentList
                                        where i.Varieties == productId && i.Instrument.Market == exchange
                                        select i.Instrument.Clone()).ToList();
            return list;
        }

        /// <summary>
        /// 获取产品。
        /// </summary>
        /// <param name="instrumentCode">合约代码。</param>
        /// <param name="exchange">市场。</param>
        /// <returns></returns>
        public USeInstrument GetInstrument(string instrumentCode, USeMarket exchange)
        {
            USeInstrumentDetail detail = (from i in m_instrumentList
                                          where i.Instrument.InstrumentCode == instrumentCode && i.Instrument.Market == exchange
                                          select i).FirstOrDefault();
            if (detail != null)
            {
                return detail.Instrument.Clone();
            }
            else
            {
                return null;
            }
        }
        #endregion
    }

}
