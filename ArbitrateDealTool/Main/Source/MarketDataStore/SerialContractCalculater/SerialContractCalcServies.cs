using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common.AppLogger;
using SerialContractCalculater.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using USe.TradeDriver.Common;
using System.Diagnostics;
using System.Configuration;
using USe.Common;
using System.Threading;
using USe.Common.Manager;

namespace SerialContractCalculater
{
    /// <summary>
    /// 连续合约服务类。
    /// </summary>
    public class SerialContractCalcServies
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
        /// 构造SerialContractCalcServies实例。
        /// </summary>
        /// <param name="logger">日志。</param>
        public SerialContractCalcServies(IAppLogger logger)
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
                SerialContractCalculateSection section = (ConfigurationManager.GetSection("SerialContractCalculater") as SerialContractCalculateSection);
                if (section == null)
                {
                    text = "未找到SerialContractCalculater配置节点";
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
        /// 启动计算连续合约方法。
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
                //当日连续合约计算先检查基础数据状态，基础数据就绪后在计算连续合约
                if (CheckTodayBaseDataIsReady() == false)
                {
                    text = string.Format("{0:yyyy-MM-dd}基础数据未就绪,无法计算", m_beginDate);
                    m_logger.WriteError(text);
                    USeConsole.WriteLine(text);
                    return -2;
                }

                text = string.Format("{0:yyyy-MM-dd}基础数据已就绪，开始计算连续合约", m_beginDate);
                m_logger.WriteInformation(text);
                USeConsole.WriteLine(text);
            }

            List<ErrorVarieties> errorList = new List<ErrorVarieties>();
            CalculateSerialContract(m_dayType, m_beginDate, m_endDate,errorList);
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
        /// 计算指定时段连续合约。
        /// </summary>
        /// <param name="beginDate">起始日。</param>
        /// <param name="endDate">结束日。</param>
        /// <returns></returns>
        private bool CalculateSerialContract(SettlementDayType settlementDayType, DateTime beginDate, DateTime endDate, List<ErrorVarieties> errorList)
        {
            string text = string.Empty;
            DateTime settlementDate = beginDate;
            
            while (settlementDate <= endDate)
            {
                try
                {
                    CalculateSerialContract(settlementDayType,settlementDate, errorList);
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
        private bool CalculateSerialContract(SettlementDayType settlementDayType, DateTime settlementDate, List<ErrorVarieties> errorList)
        {
            string text = string.Empty;
            if (!m_tradingDateManager.IsTradingDate(settlementDate))
            {
                text = string.Format("{0:yyyy-MM-dd}为非交易日,无需计算", settlementDate);
                USeConsole.WriteLine(text);
                return true;
            }

            Debug.Assert(m_varietiesList != null && m_varietiesList.Count > 0);
            foreach (USeProduct varieties in m_varietiesList)
            {
                try
                {
                    List<string> serialContractList = null;
                    if (settlementDayType == SettlementDayType.Today)
                    {
                        serialContractList = GetTodaySerialInstrument(varieties, settlementDate);
                    }
                    else
                    {
                        Debug.Assert(settlementDayType == SettlementDayType.SpecialDay);
                        serialContractList = GetHistorySerialInstrument(varieties, settlementDate); //连续合约
                    }

                    SaveSerialContract(varieties, settlementDate, serialContractList);
                    text = string.Format("{0}@{1:yyyy-MM-dd}连续合约计算完成,共{2}个合约", varieties.ProductCode, settlementDate,serialContractList.Count);
                    m_logger.WriteInformation(text);
                    USeConsole.WriteLine(text);
                }
                catch (Exception ex)
                {
                    text = string.Format("{0}@{1:yyyy-MM-dd}连续合约计算失败,{2}", varieties.ProductCode, settlementDate, ex.Message);
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
            return "SerialContractCalcServies";
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

            if (table == null || table.Rows.Count <= 0)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(table.Rows[0]["instrument_download"]);
            }
        }

        /// <summary>
        /// 获取指定品种指定日期合约列表(按日期排序)
        /// </summary>
        /// <param name="varieties"></param>
        /// <param name="settlementDate"></param>
        /// <returns></returns>
        private List<string> GetHistorySerialInstrument(USeProduct varieties, DateTime settlementDate)
        {
            string cmdText = string.Format(@"select * from {0}.day_kline 
where date_time = '{1:yyyy-MM-dd}' and exchange= '{2}' and contract like '{3}%' order by contract;",
                m_alpahDBName, settlementDate, varieties.Market.ToString(), varieties.ProductCode);

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmdText, connection);
                adapter.Fill(table);
                adapter.Dispose();
            }

            List<string> serialList = new List<string>();
            if (table != null && table.Rows.Count > 0)
            {
                string mainContractCode = USeTraderProtocol.GetMainContractCode(varieties.ProductCode, varieties.Market).InstrumentCode;
                string indexCode = USeTraderProtocol.GetVarietiesIndexCode(varieties.ProductCode, varieties.Market).InstrumentCode;

                foreach (DataRow row in table.Rows)
                {
                    string contract = row["contract"].ToString();
                    string constractVarieties = USeTraderProtocol.GetVarieties(contract);

                    if (constractVarieties == varieties.ProductCode) // 同品种
                    {
                        if (contract == mainContractCode || contract == indexCode)
                        {
                            //过滤掉指数编码和主力合约编码
                            continue;
                        }
                        else
                        {
                            serialList.Add(contract);
                        }
                    }
                }
            }
            return serialList;
        }

        /// <summary>
        /// 获取指定品种指定日期合约列表(按日期排序)
        /// </summary>
        /// <param name="varieties"></param>
        /// <param name="settlementDate"></param>
        /// <returns></returns>
        private List<string> GetTodaySerialInstrument(USeProduct varieties, DateTime settlementDate)
        {
            string cmdText = string.Format(@"select contract from {0}.contracts 
where varieties = '{1}' and exchange='{2}' and open_date <= '{3:yyyy-MM-dd}' and expire_date >= '{3:yyyy-MM-dd}'
order by contract;",
            m_alpahDBName, varieties.ProductCode, varieties.Market.ToString(), settlementDate);

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmdText, connection);
                adapter.Fill(table);
                adapter.Dispose();
            }

            List<string> serialList = new List<string>();
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    serialList.Add(row["contract"].ToString());
                }
            }
            return serialList;
        }

        /// <summary>
        /// 获取连续合约Insert SQL语句。
        /// </summary>
        /// <returns></returns>
        private string GetSerialContractInsertSql()
        {
            string cmdText = string.Format(@"INSERT INTO {0}.main_contract
(varieties,exchange,settlement_date,
serial_contract1,serial_contract2,serial_contract3,serial_contract4,serial_contract5,serial_contract6,
serial_contract7,serial_contract8,serial_contract9,serial_contract10,serial_contract11,serial_contract12)
VALUES(@varieties,@exchange,@settlement_date,
@serial_contract1,@serial_contract2,@serial_contract3,@serial_contract4,@serial_contract5,@serial_contract6,
@serial_contract7,@serial_contract8,@serial_contract9,@serial_contract10,@serial_contract11,@serial_contract12);",
            m_alpahDBName);
            return cmdText;
        }

        /// <summary>
        /// 获取连续合约Update SQL语句。
        /// </summary>
        /// <returns></returns>
        private string GetSerialContractUpdateSql()
        {
            string cmdText = string.Format(@"UPDATE {0}.main_contract SET
serial_contract1 = @serial_contract1,serial_contract2 = @serial_contract2,serial_contract3 = @serial_contract3,
serial_contract4 = @serial_contract4,serial_contract5 = @serial_contract5,serial_contract6 = @serial_contract6,
serial_contract7 = @serial_contract7,serial_contract8 = @serial_contract8,serial_contract9 = @serial_contract9,
serial_contract10 = @serial_contract10,serial_contract11 = @serial_contract11,serial_contract12 = @serial_contract12
WHERE varieties = @varieties AND exchange = @exchange AND settlement_date = @settlement_date;",m_alpahDBName);
            return cmdText;
        }

        /// <summary>
        /// 保存连续合约。
        /// </summary>
        /// <param name="varieties">品种。</param>
        /// <param name="settlementDate">结算日。</param>
        /// <param name="serialContractList">连续合约。</param>
        private void SaveSerialContract(USeProduct varieties,DateTime settlementDate,List<string> serialContractList)
        {
            using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
            {
                connection.Open();
                string cmdText = GetSerialContractUpdateSql();
                MySqlCommand command = new MySqlCommand(cmdText, connection);
                command.Parameters.AddWithValue("@varieties", varieties.ProductCode);
                command.Parameters.AddWithValue("@exchange", varieties.Market.ToString());
                command.Parameters.AddWithValue("@settlement_date", settlementDate.Date);

                for (int i = 1; i <= 12; i++)
                {
                    string key = string.Format("@serial_contract{0}", i);
                    string value = i <= serialContractList.Count ? serialContractList[i - 1] : "";
                    command.Parameters.AddWithValue(key, value);
                }

                int result = command.ExecuteNonQuery();
                if(result <1)
                {
                    command.CommandText = GetSerialContractInsertSql();
                    result = command.ExecuteNonQuery();
                    Debug.Assert(result == 1);
                }
            }
        }
    }
}
