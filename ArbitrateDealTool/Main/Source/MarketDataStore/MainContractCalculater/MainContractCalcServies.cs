using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common.AppLogger;
using MainContractCalculater.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using USe.TradeDriver.Common;
using System.Diagnostics;
using System.Configuration;
using USe.Common;
using System.Threading;
using USe.Common.TradingDay;
using USe.Common.Manager;

namespace MainContractCalculater
{
    /// <summary>
    /// 主力合约服务类。
    /// </summary>
    public class MainContractCalcServies
    {
        #region members
        private string m_dbConnStr = string.Empty;
        private string m_alpahDBName = string.Empty;

        private IAppLogger m_logger = null;

        private TradeCalendarManager m_tradingDateManager = null;
        private DateTime m_beginDate;   // 开始日期。
        private DateTime m_endDate;     // 结束日期。

        private SettlementDayType m_dayType;    // 日期类型枚举。
        private TimeSpan m_tryInterval = TimeSpan.Zero;  // 间隔时间。
        private int m_tryCount = 0;  // 循环次数。

        private List<USeProduct> m_varietiesList = null;
        #endregion

        #region construction
        /// <summary>
        /// 构造MainContractServer实例。
        /// </summary>
        /// <param name="logger">日志。</param>
        public MainContractCalcServies(IAppLogger logger)
        {
            m_logger = logger;
        }
        #endregion

        #region methods
        /// <summary>
        /// 初始化方法。
        /// </summary>
        public void Initialize()
        {
            ReadConfig();
        }

        /// <summary>
        /// 读取配置。
        /// </summary>
        private void ReadConfig()
        { 
            string text = string.Empty;
            m_dbConnStr = ConfigurationManager.ConnectionStrings["AlphaDB"].ConnectionString;
            if (string.IsNullOrEmpty(m_dbConnStr))
            {
                text = "AlphaDB数据库连接串为空";

            }

            m_alpahDBName = ConfigurationManager.AppSettings["AlphaDBName"];
            if (string.IsNullOrEmpty(m_alpahDBName))
            {
                text = "AlphaDBName为空";
                throw new ApplicationException(text);
            }
            
            try
            {
                MainContractCalculateSection section = (ConfigurationManager.GetSection("MainContractCalculater") as MainContractCalculateSection);
                if (section == null)
                {
                    text = "未找到MainContractCalculater配置节点";
                    throw new ApplicationException(text);
                }

                if (section.CalculateDate.CalculateDayType == SettlementDayType.SpecialDay)
                {
                    if (section.CalculateDate.BeginDay.HasValue == false)
                    {
                        text = "未设定beginDate节点值";
                        throw new ApplicationException(text);
                    }

                    if (section.CalculateDate.EndDay.HasValue == false)
                    {
                        text = "未设定endDate节点值";
                        throw new ApplicationException(text);
                    }

                    if (section.CalculateDate.EndDay < section.CalculateDate.BeginDay)
                    {
                        text = "beginDate节点值大于endDate";
                        throw new ApplicationException(text);
                    }

                    m_dayType = SettlementDayType.SpecialDay;
                    m_beginDate = section.CalculateDate.BeginDay.Value.Date;
                    m_endDate = section.CalculateDate.EndDay.Value.Date;
                }
                else
                {
                    Debug.Assert(section.CalculateDate.CalculateDayType == SettlementDayType.Today);
                    m_dayType = SettlementDayType.Today;
                    m_beginDate = DateTime.Today;
                    m_endDate = DateTime.Today;
                }

                if (section.QuerySetting.Interval <= TimeSpan.Zero)
                {
                    text = "轮询间隔不能为空";
                    throw new Exception(text);
                }

                if (section.QuerySetting.Count <= 0)
                {
                    text = "重试次数不能小于0";
                    throw new Exception(text);
                }

                m_tryInterval = section.QuerySetting.Interval;
                m_tryCount = section.QuerySetting.Count;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 启动计算主力合约方法。
        /// </summary>
        /// <returns></returns>
        public int Run()
        {
            string text = string.Empty;
            try
            {
                TradeCalendarManager tradeCalendarManager = new TradeCalendarManager(m_dbConnStr,m_alpahDBName);
                tradeCalendarManager.Initialize();

                m_tradingDateManager = tradeCalendarManager;

                text = "初始化交易日历成功";
                m_logger.WriteInformation(text);
                USeConsole.WriteLine(text);
            }
            catch (Exception ex)
            {
                text = string.Format("初始化交易日历失败,错误:{0}.", ex.Message);
                m_logger.WriteError(text);
                USeConsole.WriteLine(text);
                return -1;
            }

            try
            {
                m_varietiesList = GetAllVarieties();
                text = "加载品种信息成功";
                m_logger.WriteInformation(text);
                USeConsole.WriteLine(text);
            }
            catch (Exception ex)
            {
                text = "加载品种信息失败," + ex.Message;
                m_logger.WriteError(text);
                USeConsole.WriteLine(text);
                return -1;
            }

            if (m_dayType == SettlementDayType.Today)
            {
                //当日主力合约计算先检查基础数据状态，基础数据就绪后在计算主力合约
                if (CheckTodayBaseDataIsReady() == false)
                {
                    text = string.Format("{0:yyyy-MM-dd}基础数据未就绪,无法计算", m_beginDate);
                    m_logger.WriteError(text);
                    USeConsole.WriteLine(text);
                    return -2;
                }

                text = string.Format("{0:yyyy-MM-dd}基础数据已就绪，开始计算主力合约", m_beginDate);
                m_logger.WriteInformation(text);
                USeConsole.WriteLine(text);
            }

            List<ErrorVarieties> errorList = new List<ErrorVarieties>();
            CalculateMainContract(m_beginDate, m_endDate,errorList);
            if(errorList.Count >0)
            {
                foreach(ErrorVarieties errorItem in errorList)
                {
                    text = errorItem.ToString();
                    m_logger.WriteInformation(text);
                    USeConsole.WriteLine(text);
                }

                return -2;
            }

            return 0;
        }

        /// <summary>
        /// 检查当日基础数据是否准备就绪。
        /// </summary>
        /// <returns></returns>
        private bool CheckTodayBaseDataIsReady()
        {
            string text = string.Empty;

            int tryCount = 0;

            while (tryCount <= m_tryCount)
            {
                tryCount++;

                bool verifyResult = false;
                try
                {
                    verifyResult = CheckBaseDataIsReady(DateTime.Today);
                }
                catch (Exception ex)
                {
                    text = string.Format("基础数据第{0}次检查失败,错误：{1}", tryCount, ex.Message);
                    m_logger.WriteError(text);
                    USeConsole.WriteLine(text);
                }

                if (verifyResult == false)
                {
                    text = string.Format("基础数据第{0}次检查未就绪，等待{1}继续尝试", tryCount, m_tryInterval);
                    m_logger.WriteInformation(text);
                    USeConsole.WriteLine(text);

                    Thread.Sleep(m_tryInterval);
                    continue;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// 计算指定时段主力合约。
        /// </summary>
        /// <param name="beginDate">起始日。</param>
        /// <param name="endDate">结束日。</param>
        /// <returns></returns>
        private bool CalculateMainContract(DateTime beginDate, DateTime endDate, List<ErrorVarieties> errorList)
        {
            string text = string.Empty;
            DateTime settlementDate = beginDate;
            
            while (settlementDate <= endDate)
            {
                try
                {
                    CalculateMainContract(settlementDate,errorList);
                }
                catch(Exception ex)
                {
                    text = string.Format("{0:yyyy-MM-dd}计算失败,错误:{1}", settlementDate, ex.Message);
                    m_logger.WriteError(text);
                    USeConsole.WriteLine(text);
                }
                settlementDate = settlementDate.AddDays(1);
            }

            return true;
        }

        /// <summary>
        /// 计算指定日主力合约。
        /// </summary>
        /// <param name="settlementDate"></param>
        /// <returns></returns>
        private bool CalculateMainContract(DateTime settlementDate, List<ErrorVarieties> errorList)
        {
            string text = string.Empty;
            TradeCalendar tradeCalendar = m_tradingDateManager.GetTradeCalendar(settlementDate);
            if (tradeCalendar.IsTradeDay == false)
            {
                text = string.Format("{0:yyyy-MM-dd}为非交易日,无需计算", settlementDate);
                USeConsole.WriteLine(text);
                return true;
            }

            //[yangming]历史数据未维护暂不校验
            //if (CheckBaseDataIsReady(settlementDate) == false)
            //{
            //    text = string.Format("{0:yyyy-MM-dd}结算数据未就绪，无法计算", settlementDate);
            //    m_logger.WriteWarning(text);
            //    USeConsole.WriteLine(text);

            //    errorList.Add(new ErrorVarieties() {
            //        Varieties = "All",
            //        SettlementDate = settlementDate,
            //        ErrorMessage = text
            //    });
            //    return false;
            //}

            Debug.Assert(m_varietiesList != null && m_varietiesList.Count > 0);
            foreach (USeProduct varieties in m_varietiesList)
            {
                //if (varieties.ProductCode != "CF") continue;

                try
                {
                    string mainContract = GetMaxVolumeInstrument(varieties, settlementDate);
                    //mainContract = "CF709";

                    List<string> hisMainContractList = GetHistoryMainContract(varieties, settlementDate);
                    if (hisMainContractList != null && hisMainContractList.Count > 0)
                    {
                        string lastMainContract = hisMainContractList[0];
                        if (mainContract != lastMainContract)
                        {
                            //主力合约不同于昨日,且曾经为主力合约，则主力合约不切换
                            if (hisMainContractList.Contains(mainContract))
                            {
                                mainContract = lastMainContract;
                            }
                        }
                    }

                    SaveMainContract(varieties, tradeCalendar.NextTradeDay, mainContract);
                    text = string.Format("{0}@{1:yyyy-MM-dd}主力合约计算完成,主力合约:{2}", varieties.ProductCode, settlementDate,mainContract);
                    m_logger.WriteInformation(text);
                    USeConsole.WriteLine(text);
                }
                catch (Exception ex)
                {
                    text = string.Format("{0}@{1:yyyy-MM-dd}主力合约计算失败,{2}", varieties.ProductCode, settlementDate, ex.Message);
                    m_logger.WriteInformation(text);
                    USeConsole.WriteLine(text);

                    errorList.Add(new ErrorVarieties() {
                        Varieties = varieties.ProductCode,
                        SettlementDate = settlementDate,
                        ErrorMessage = ex.Message
                    });
                }
            }

            return true;
        }

        /// <summary>
        /// 重写toString()方法。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "MainContractCalcServies";
        }
        #endregion

        /// <summary>
        /// 获取国内市场。
        /// </summary>
        /// <returns></returns>
        private string GetInternalExchanges()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("'{0}',", USeMarket.SHFE.ToString()));
            sb.Append(string.Format("'{0}',", USeMarket.DCE.ToString()));
            sb.Append(string.Format("'{0}',", USeMarket.CFFEX.ToString()));
            sb.Append(string.Format("'{0}'", USeMarket.CZCE.ToString()));

            return sb.ToString();
        }

        /// <summary>
        /// 获取所有品种信息。
        /// </summary>
        /// <returns></returns>
        private List<USeProduct> GetAllVarieties()
        {
            string cmdText = string.Format(@"select* from {0}.varieties where exchange in ({1});",
                m_alpahDBName, GetInternalExchanges());

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmdText, connection);
                adapter.Fill(table);
                adapter.Dispose();
            }

            List<USeProduct> varieties = new List<USeProduct>();
            if(table != null)
            {
                foreach(DataRow row in table.Rows)
                {
                    USeProduct product = new USeProduct() {
                        ProductCode = row["code"].ToString(),
                        Market = (USeMarket)Enum.Parse(typeof(USeMarket), row["exchange"].ToString())
                    };

                    varieties.Add(product);
                }
            }

            return varieties;
        }

        /// <summary>
        /// 检查基础数据是否准备就绪。
        /// </summary>
        /// <returns></returns>
        private bool CheckBaseDataIsReady(DateTime settelementDate)
        {
            string cmdText = string.Format(@"select * from {0}.daily_work_state 
where working_day = '{1:yyyy-MM-dd}';", m_alpahDBName, settelementDate);

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmdText, connection);
                adapter.Fill(table);
                adapter.Dispose();
            }

            if(table == null || table.Rows.Count <=0)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(table.Rows[0]["settlement_price_download"]);
            }
        }

        /// <summary>
        /// 获取指定品种指定日期成交量最大的合约。[hanyu]2017//11/03修正为持仓量最大
        /// </summary>
        /// <param name="varieties"></param>
        /// <param name="settlementDate"></param>
        /// <returns></returns>
        private string GetMaxVolumeInstrument(USeProduct varieties, DateTime settlementDate)
        {
            string cmdText = string.Format(@"select dk.contract,dk.date_time,dk.openinterest from {0}.day_kline dk
where dk.date_time = '{1:yyyy-MM-dd}' and dk.contract like '{2}%' and dk.exchange = '{3}'
order by dk.openinterest desc,dk.contract asc;",
            m_alpahDBName, settlementDate, varieties.ProductCode, varieties.Market.ToString());

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmdText, connection);
                adapter.Fill(table);
                adapter.Dispose();
            }

            if (table != null && table.Rows.Count > 0)
            {
                USeInstrument mainContractCode = USeTraderProtocol.GetMainContractCode(varieties.ProductCode, varieties.Market);
                USeInstrument indexCode = USeTraderProtocol.GetVarietiesIndexCode(varieties.ProductCode, varieties.Market);

                int maxOpeninterest = 0;
                string mainContract = string.Empty;
                foreach(DataRow row in table.Rows)
                {
                    string contract = row["contract"].ToString();
                    string constractVarieties = USeTraderProtocol.GetVarieties(contract);
                    if(constractVarieties == varieties.ProductCode)
                    {
                        if (contract == mainContractCode.InstrumentCode || contract == indexCode.InstrumentCode)
                        {
                            //过滤掉指数编码和主力合约编码
                            string text = string.Format("{0}@{1:yyyy-MM-dd}计算主力合约被过滤的指数编码和主力合约,{2}", varieties.ProductCode, settlementDate,contract);
                            Debug.WriteLine(text);
                            m_logger.WriteInformation(text);

                            continue;
                        }
                        else
                        {
                            if (Convert.ToInt32(row["openinterest"]) > maxOpeninterest)
                            {
                                maxOpeninterest = Convert.ToInt32(row["openinterest"]);
                                mainContract = row["contract"].ToString();
                            }
                        }
                    }
                }

                return mainContract;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取指定品种指定日期成交量最大的合约。
        /// </summary>
        /// <param name="varities"></param>
        /// <param name="settlementDate"></param>
        /// <returns></returns>
        private string GetMaxVolumeInstrument2(USeProduct varities, DateTime settlementDate)
        {
            string cmdText = string.Format(@"select dk.contract,dk.date_time,dk.openinterest from {0}.day_kline dk
left join {0}.contracts  c on dk.contract = c.contract
where dk.date_time = '{1:yyyy-MM-dd}' and c.varieties = '{2}' and c.exchange = '{3}'
order by dk.openinterest desc,dk.contract asc
limit 1;",
            m_alpahDBName, settlementDate, varities.ProductCode, varities.Market.ToString());

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmdText, connection);
                adapter.Fill(table);
                adapter.Dispose();
            }

            if (table != null && table.Rows.Count > 0)
            {
                Debug.Assert(table.Rows.Count == 1);
                return table.Rows[0]["contract"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取指定品种指定日期前历史主力合约。
        /// </summary>
        /// <param name="varieties"></param>
        /// <param name="settlementDate"></param>
        /// <returns></returns>
        private List<string> GetHistoryMainContract(USeProduct varieties, DateTime settlementDate)
        {
            string cmdText = string.Format(@"select m.main_contract from {0}.main_contract m
left join {0}.contracts c on m.main_contract = c.contract
where m.varieties = '{1}' and m.exchange = '{2}' and m.settlement_date < '{3:yyyy-MM-dd}' and c.is_trading = 1
order by settlement_date desc;",
            m_alpahDBName, varieties.ProductCode, varieties.Market.ToString(), settlementDate);

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmdText, connection);
                adapter.Fill(table);
                adapter.Dispose();
            }

            List<string> histroyMainContractList = new List<string>();
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    histroyMainContractList.Add(row["main_contract"].ToString());
                }
            }
            return histroyMainContractList;
        }

        /// <summary>
        /// 获取主力合约Insert SQL语句。
        /// </summary>
        /// <returns></returns>
        private string GetMainContractInsertSql()
        {
            string cmdText = string.Format(@"INSERT INTO {0}.main_contract
(varieties,exchange,settlement_date,main_contract,update_time)
VALUES(@varieties,@exchange,@settlement_date,@main_contract,@update_time);",
            m_alpahDBName);
            return cmdText;
        }

        /// <summary>
        /// 获取主力合约Update SQL语句。
        /// </summary>
        /// <returns></returns>
        private string GetMainContractUpdateSql()
        {
            string cmdText = string.Format(@"UPDATE {0}.main_contract SET
main_contract = @main_contract,update_time = @update_time
WHERE varieties = @varieties AND exchange = @exchange AND settlement_date = @settlement_date;",m_alpahDBName);
            return cmdText;
        }

        /// <summary>
        /// 保存主力及连续合约。
        /// </summary>
        /// <param name="varieties">品种。</param>
        /// <param name="settlementDate">结算日。</param>
        /// <param name="mainContract">主力合约。</param>
        private void SaveMainContract(USeProduct varieties,DateTime settlementDate,string mainContract)
        {
            using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
            {
                connection.Open();
                string cmdText = GetMainContractUpdateSql();
                MySqlCommand command = new MySqlCommand(cmdText, connection);
                command.Parameters.AddWithValue("@varieties", varieties.ProductCode);
                command.Parameters.AddWithValue("@exchange", varieties.Market.ToString());
                command.Parameters.AddWithValue("@settlement_date", settlementDate.Date);

                command.Parameters.AddWithValue("@main_contract", mainContract);
                command.Parameters.AddWithValue("@update_time", DateTime.Now);//更新的时间


                int result = command.ExecuteNonQuery();
                if(result <1)
                {
                    command.CommandText = GetMainContractInsertSql();
                    result = command.ExecuteNonQuery();
                    Debug.Assert(result == 1);
                }
            }
        }
    }
}
