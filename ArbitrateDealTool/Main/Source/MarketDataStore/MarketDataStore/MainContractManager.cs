using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using USe.Common.TradingDay;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data;
using USe.Common;

namespace MarketDataStore
{
    public class MainContractManager
    {
        private string m_dbConnStr = string.Empty;
        private string m_alphaDBName = string.Empty;

        private Dictionary<string, USeInstrument> m_mainContractDic = new Dictionary<string, USeInstrument>();

        public MainContractManager(string dbConnStr,string alphaDBName)
        {
            if(string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentNullException("dbConnStr");
            }

            m_dbConnStr = dbConnStr;
            m_alphaDBName = alphaDBName;
        }

        public Dictionary<string, USeInstrument> MainContractDictionary
        {
            get
            {
                if(m_mainContractDic == null)
                {
                    return null;
                }
                else if(m_mainContractDic != null && m_mainContractDic.Count == 0)
                {
                    Dictionary<string, USeInstrument> mainContractInsDic = new Dictionary<string, USeInstrument>();
                    return mainContractInsDic;
                }
                else
                {
                    Dictionary<string, USeInstrument> mainContractInsDic = new Dictionary<string, USeInstrument>();
                    foreach (KeyValuePair<string,USeInstrument> kv in m_mainContractDic)
                    {
                        mainContractInsDic[kv.Key] = kv.Value;
                    }
                    return mainContractInsDic;
                }
            }
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        public void Initialize()
        {
            LoadMainContract();
        }

        /// <summary>
        /// 加载交易日历。
        /// </summary>
        /// <returns></returns>
        private TradeCalendar GetTradeCalendar()
        {
            string strSel = string.Format(@"select * from {0}.trade_calendar
where today = '{1:yyyy-MM-dd}';", m_alphaDBName, DateTime.Today);

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(strSel, connection);
                adapter.Fill(table);
                adapter.Dispose();
            }

            if(table != null && table.Rows.Count >0)
            {
                Debug.Assert(table.Rows.Count == 1);
                DataRow row = table.Rows[0];
                TradeCalendar calendar = new TradeCalendar() {
                    Today = Convert.ToDateTime(row["today"]),
                    IsTradeDay = Convert.ToBoolean(row["is_trading_day"]),
                    PreTradeDay = Convert.ToDateTime(row["pre_trade_day"]),
                    NextTradeDay = Convert.ToDateTime(row["next_trade_day"])
                };
                return calendar;
            }
            else
            {
                throw new Exception(string.Format("未找到[{0}]交易日历", DateTime.Today.ToDate()));
            }
        }

        /// <summary>
        /// 加载主力合约信息。
        /// </summary>
        private void LoadMainContract()
        {
            try
            {
                TradeCalendar tradeCalendar = GetTradeCalendar();

                DateTime tradeDay = DateTime.Today;
                //[yangming]强制设定17点为一天的分隔点，后续在完善
                if (DateTime.Now.TimeOfDay > new TimeSpan(17, 0, 0))
                {
                    tradeDay = tradeCalendar.NextTradeDay;
                }

                string strSel = string.Format(@"select * from {0}.main_contract
where settlement_date = '{1:yyyy-MM-dd}' and exchange in ({2});",
                m_alphaDBName, tradeDay, USeTraderProtocol.GetInternalFutureMarketSqlString());

                DataTable table = new DataTable();
                using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(strSel, connection);
                    adapter.Fill(table);
                    adapter.Dispose();
                }

                Dictionary<string, USeInstrument> mainContractDic = new Dictionary<string, USeInstrument>();

                foreach (DataRow row in table.Rows)
                {
                    string varieties = row["varieties"].ToString();
                    string mainContractCode = row["main_contract"].ToString();
                    USeMarket exchange = (USeMarket)Enum.Parse(typeof(USeMarket), row["exchange"].ToString());

                    if (string.IsNullOrEmpty(mainContractCode) == false)
                    {
                        USeInstrument mainContract = new USeInstrument(mainContractCode, mainContractCode, exchange);
                        mainContractDic.Add(varieties, mainContract);
                    }
                }

                m_mainContractDic = mainContractDic;
            }
            catch (Exception ex)
            {
                throw new Exception("加载主力合约信息失败," + ex.Message);
            }
        }

        /// <summary>
        /// 获取主力合约。
        /// </summary>
        /// <param name="varieties"></param>
        /// <returns></returns>
        public USeInstrument GetMainContract(string varieties)
        {
            USeInstrument instrument = null;
            m_mainContractDic.TryGetValue(varieties, out instrument);
            return instrument;
        }

        public bool IsMainContract(string instrumentCode)
        {
            string varieties = USeTraderProtocol.GetVarieties(instrumentCode);
            USeInstrument mainContract = null;
            m_mainContractDic.TryGetValue(varieties, out mainContract);
            if(mainContract != null && mainContract.InstrumentCode == instrumentCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
