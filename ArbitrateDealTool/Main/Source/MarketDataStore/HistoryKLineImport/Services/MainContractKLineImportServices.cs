using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using USe.Common;
using USe.Common.AppLogger;
using USe.TradeDriver.Common;

namespace HistoryKLineImport
{
    /// <summary>
    /// 主力合约K线导入服务。
    /// </summary>
    class MainContractKLineImportServices : ImportServices
    {
        #region member
        private DateTime m_beginDate = DateTime.MinValue;
        private DateTime m_endDate = DateTime.MaxValue;
        #endregion

        #region construction
        public MainContractKLineImportServices(IAppLogger eventLogger)
            :base(eventLogger)
        {
        }
        #endregion

        #region methods
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <returns></returns>
        public override bool Initialize()
        {
            if (base.Initialize() == false)
            {
                return false;
            }

            try
            {
                string beginDateValue = ConfigurationManager.AppSettings["MainContractBeginDate"];
                if (string.IsNullOrEmpty(beginDateValue))
                {
                    string text = "Not found MainContractBeginDate";
                    m_eventLogger.WriteError(text);
                    USeConsole.WriteLine(text);
                    return false;
                }

                string endDateValue = ConfigurationManager.AppSettings["MainContractEndDate"];
                if (string.IsNullOrEmpty(endDateValue))
                {
                    string text = "Not found MainContractEndDate";
                    m_eventLogger.WriteError(text);
                    USeConsole.WriteLine(text);
                    return false;
                }

                m_beginDate = Convert.ToDateTime(beginDateValue);
                m_endDate = Convert.ToDateTime(endDateValue);
            }
            catch (Exception ex)
            {
                string text = "初始化主力合约K线导入服务失败," + ex.Message;
                m_eventLogger.WriteError(text);
                USeConsole.WriteLine(text);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Run()
        {
            List<MainContractEntity> mainContractList = GetMainContractList();
            if (mainContractList.Count < 0)
            {
                string text = "无可导入主力合约";
                m_eventLogger.WriteInformation(text);
                USeConsole.WriteLine(text);
                return true;
            }

            int index = 0;
            foreach (MainContractEntity mainContract in mainContractList)
            {
                index++;
                try
                {

                    string cmdDeleteText = GetMainContractMin1KLineDeleteSql(mainContract);
                    string cmdInsertText = GetMainContractMin1KLineInsertSql(mainContract);

                    string cmdText = cmdDeleteText + cmdInsertText;
                    Stopwatch stopWathch = new Stopwatch();
                    stopWathch.Start();

                    using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                    {
                        connection.Open();

                        MySqlCommand command = new MySqlCommand(cmdText, connection);
                        command.ExecuteNonQuery();
                    }
                    stopWathch.Stop();
                    long useSecondes = stopWathch.ElapsedMilliseconds / 1000;
                    USeConsole.WriteLine(string.Format("导入{0}主力合约K线完成,耗时{1}秒,Finish{2}/{3}",
                        mainContract, useSecondes, index, mainContractList.Count));
                }
                catch (Exception ex)
                {
                    string text = string.Format("导入{0}主力合约K线失败,错误:{1}", mainContract, ex.Message);
                    m_eventLogger.WriteInformation(text);
                    USeConsole.WriteLine(text);
                }
            }

            return true;
        }
        #endregion

        /// <summary>
        /// 获取主力合约列表。
        /// </summary>
        /// <returns></returns>
        private List<MainContractEntity> GetMainContractList()
        {
            string cmdText = string.Format(@"select varieties, exchange, settlement_date, main_contract from {0}.main_contract
where settlement_date >= '{1:yyyy-MM-dd}' and settlement_date <= '{2:yyyy-MM-dd}'
order by settlement_date, exchange, varieties;",
                m_alphaDBName, m_beginDate, m_endDate);

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmdText, connection);
                adapter.Fill(table);
            }

            List<MainContractEntity> mainContractList = new List<MainContractEntity>();
            foreach (DataRow row in table.Rows)
            {
                USeMarket exchange = (USeMarket)Enum.Parse(typeof(USeMarket), row["exchange"].ToString());
                MainContractEntity mainContract = new MainContractEntity() {
                    Varieties = row["varieties"].ToString(),
                    Exchange = exchange,
                    SettlementDate = Convert.ToDateTime(row["settlement_date"]),
                    BaseContract = row["main_contract"].ToString(),
                    MainContractCode = USeTraderProtocol.GetMainContractCode(row["varieties"].ToString(), exchange).InstrumentCode
                };
                mainContractList.Add(mainContract);
            }

            return mainContractList;
        }

        /// <summary>
        /// 获取主力合约K线Insert语句。
        /// </summary>
        /// <param name="mainContract"></param>
        /// <returns></returns>
        private string GetMainContractMin1KLineInsertSql(MainContractEntity mainContract)
        {
            string cmdText = string.Format(@"insert into {0}.min1_kline_{1}(contract, exchange, date_time, price_open, price_high, price_low, price_close, volumn, turnover, openinterest)
select '{2}',exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest
 from {0}.min1_kline_{1} where contract = '{3}' and DATE_FORMAT(date_time, '%Y-%m-%d') = '{4:yyyy-MM-dd}';",
            m_alphaDBName, mainContract.Exchange.ToString().ToLower(), mainContract.MainContractCode,
            mainContract.BaseContract, mainContract.SettlementDate);

            return cmdText;
        }

        /// <summary>
        /// 获取主力合约K线Delete语句。
        /// </summary>
        /// <param name="mainContract"></param>
        /// <returns></returns>
        private string GetMainContractMin1KLineDeleteSql(MainContractEntity mainContract)
        {
            string cmdText = string.Format(@"delete from {0}.min1_kline_{1}
where contract = '{2}' and DATE_FORMAT(date_time, '%Y-%m-%d') = '{3:yyyy-MM-dd}';",
             m_alphaDBName, mainContract.Exchange.ToString().ToLower(),
             mainContract.MainContractCode, mainContract.SettlementDate);

            return cmdText;
        }

        /// <summary>
        /// 主力合约。
        /// </summary>
        private class MainContractEntity
        {
            /// <summary>
            /// 品种。
            /// </summary>
            public string Varieties { get; set; }

            /// <summary>
            /// 市场。
            /// </summary>
            public USeMarket Exchange { get; set; }

            /// <summary>
            /// 结算日。
            /// </summary>
            public DateTime SettlementDate { get; set; }

            /// <summary>
            /// 基础合约。
            /// </summary>
            public string BaseContract { get; set; }

            /// <summary>
            /// 主力合约编码。
            /// </summary>
            public string MainContractCode { get; set; }

            public override string ToString()
            {
                return string.Format("{0}.{1}@{2:yyyy-MM-dd}", this.Varieties, this.Exchange, this.SettlementDate);
            }
        }
    }
}
