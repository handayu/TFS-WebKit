using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Threading;
using System.Linq;
using USe.Common.AppLogger;
using CTPAPI;
using USe.CtpOrderQuerier;
using USe.CtpOrderQuerier.Configuration;
using MySql.Data.MySqlClient;
using USe.TradeDriver.Common;
using USe.Common.TradingDay;
using System.Xml.Serialization;
using USe.Common;
using System.IO;
using USe.Common.Manager;

namespace CloseMarketDataDownLoad
{
    /// <summary>
    /// 期货结算价下载服务。
    /// </summary>
    class DownloadServies
    {
        #region member
        private string m_dbConStr = string.Empty;
        private string m_alphaDBName = string.Empty;


        private int m_queryNum = 5; //查询次数
        private TimeSpan m_queryFrequence = new TimeSpan(0, 5, 0);   //查询间隔分钟

        Dictionary<string, USeInstrumentDetail> instrumentDic = new Dictionary<string, USeInstrumentDetail>();

        private QueryDay m_refrashDate = QueryDay.Unknow;

        private CtpAccountElement m_ctpAccountConfig = null;
        private CtpOrderDriverElement m_ctpDriverConfig = null;
        private IAppLogger m_eventLogger = null;
        private List<USeInstrumentDetail> m_insDetailList = null;
        private TimeSpan m_nextDayRange = new TimeSpan(18, 0, 0); // 下一交易日的时间分界点
        #endregion // member

        #region methods
        /// <summary>
        /// 启动结算价下载。
        /// </summary>
        public int Start()
        {
            //确定更新结算价成功之后，需要再次计算资金沉淀，盘中的时候用的是最新价计算，有结算价之后，就改为结算价重新刷新一遍计算
            //计算公式：资金沉淀=品种持仓量*结算价*合约规模*交易所多头保证金比例
            //计算资金总沉淀(盘中就用最新价计算资金沉淀，待下午结算价出来之后，按照结算价再更新一次)
            //int perSharesContract = GetInstrumentPerSharesContract(marketData.Instrument.InstrumentCode);//合约规模
            //decimal exchangeLongMarginRatio = GetExchangeLongMarginRatio(marketData.Instrument.InstrumentCode);//保证金
            //m_kLine.SendimentaryMoney = marketData.OpenInterest * marketData.LastPrice * perSharesContract * exchangeLongMarginRatio;//资金沉淀
            //取出UseinstrumentDetail品种合约信息取出合约规模和保证金额比例，再次执行SQL语句

            try
            {
                m_eventLogger = AppLogger.InitInstance();
            }
            catch
            {
                m_eventLogger = new NullLogger("DownloadProcessor_DefaultLogger");
            }

            m_eventLogger.LineFeed();
            string text = "开始下载结算价服务";
            m_eventLogger.WriteInformation(text);
            WriteConsoleLog(text);

            if (ReadConfig() == false) return -1;

            //下载所有合约详情，失败直接退出因为后面需要基础数据
            try
            {
                USeTradingInstrumentManager tradingInsManager = CreateInstrumentManager();
                m_insDetailList = tradingInsManager.GetAllInstrumentDetails();
                Debug.Assert(m_insDetailList != null);
            }
            catch (Exception ex)
            {
                text = "下载数据库所有合约详情InstrumentDetail失败," + ex.Message;
                WriteConsoleLog(text);
                m_eventLogger.WriteError(text);
                return -1;
            }

            CtpOrderQuerier ctpApp = new CtpOrderQuerier();
            try
            {
                ctpApp.Connect(m_ctpDriverConfig.Address, m_ctpDriverConfig.Port,
                       m_ctpDriverConfig.LoginTimeOut, m_ctpDriverConfig.QueryTimeOut);
                text = "连接CTP交易服务器成功";
                WriteConsoleLog(text);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                text = "连接CTP交易服务器失败," + ex.Message;
                WriteConsoleLog(text);
                m_eventLogger.WriteError(text);
                ctpApp.Disconnect();
                return -1;
            }

            try
            {
                ctpApp.Login(m_ctpAccountConfig.ID, m_ctpAccountConfig.Password, m_ctpAccountConfig.BrokerID);
                text = "登陆CTP交易服务器成功";
                WriteConsoleLog(text);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                text = "登陆CTP交易服务器失败," + ex.Message;
                WriteConsoleLog(text);
                m_eventLogger.WriteError(text);
                ctpApp.Disconnect();
                return -1;
            }


            try
            {
                List<InstrumentField> instrumentList = ctpApp.QueryInstument();

                foreach (InstrumentField item in instrumentList)
                {
                    if (item.ProductClass != ProductClass.Futures)
                    {
                        continue;
                    }
                    USeInstrumentDetail entity = InsturmentFiledToUSeInstrumentDetail(item);
                    instrumentDic.Add(entity.Instrument.InstrumentCode, entity);
                }

                text = string.Format("查询期货合约数据完成,共计{0}个合约", instrumentDic.Count);
                WriteConsoleLog(text);
                m_eventLogger.WriteInformation(text);

            }
            catch (Exception ex)
            {
                text = "查询期货合约数据失败," + ex.Message;
                WriteConsoleLog(text);
                m_eventLogger.WriteError(text);
                ctpApp.Disconnect();
                return -1;
            }


            DateTime tradingDate = GetTradingDay();

            if (m_refrashDate == QueryDay.Today)
            {
                int queryNum = m_queryNum;
                while (queryNum > 0)
                {
                    try
                    {
                        List<DepthMarketDataField> tempDepthMarketDataFieldList = ctpApp.QueryDepthMarketData();

                        Debug.Assert(tempDepthMarketDataFieldList != null);
                        foreach (DepthMarketDataField marketData in tempDepthMarketDataFieldList)
                        {
                            if (marketData.SettlementPrice <= 0d || marketData.SettlementPrice == double.MaxValue)
                            {
                                continue;
                            }
                            if (instrumentDic.ContainsKey(marketData.InstrumentID))
                            {
                                //[hanyu]未防返回的MarketData没有ExchangeID,改用查询回来的合约交易所信息
                                USeMarket market = instrumentDic[marketData.InstrumentID].Instrument.Market;
                                SaveInstumentsSettlementPriceToDB(marketData, marketData.InstrumentID, market, tradingDate, marketData.SettlementPrice);

                                //保存完一笔的同时刷新一次资金沉淀[暂时不在这里处理，改用直接在特定时间执行SQL语句，因为涉及到8888指数和9999主力合约也需要刷新资金沉淀的问题]
                                //RefreashSendimentaryMoney(marketData, marketData.InstrumentID, market, tradingDate,marketData.SettlementPrice);

                                instrumentDic.Remove(marketData.InstrumentID);

                                USeConsole.WriteLine(string.Format("保存{0}成功", marketData.InstrumentID));

                            }
                        }

                        if (instrumentDic.Count <= 0)
                        {
                            //所有合约存储完毕，退出
                            break;
                        }

                        queryNum--;
                        if (queryNum > 0)
                        {
                            Thread.Sleep(m_queryFrequence);
                        }
                    }
                    catch (Exception ex)
                    {
                        text = string.Format("查询,保存当日结算价异常:{0}", ex.Message);
                        WriteConsoleLog(text);
                        m_eventLogger.WriteInformation(text);
                        ctpApp.Disconnect();
                        return -1;
                    }
                }
            }
            else
            {
                try
                {
                    List<DepthMarketDataField> tempDepthMarketDataFieldList = ctpApp.QueryDepthMarketData();

                    Debug.Assert(tempDepthMarketDataFieldList != null);

                    foreach (DepthMarketDataField marketData in tempDepthMarketDataFieldList)
                    {
                        if (marketData.PreSettlementPrice <= 0d || marketData.PreSettlementPrice == double.MaxValue)
                        {
                            continue;
                        }
                        if (instrumentDic.ContainsKey(marketData.InstrumentID))
                        {
                            //[hanyu]未防返回的MarketData没有ExchangeID,改用查询回来的合约交易所信息
                            USeMarket market = instrumentDic[marketData.InstrumentID].Instrument.Market;

                            //保存结算价
                            SaveInstumentsSettlementPriceToDB(marketData, marketData.InstrumentID, market, tradingDate, marketData.PreSettlementPrice);

                            //RefreashSendimentaryMoney(marketData, marketData.InstrumentID, market, tradingDate,marketData.PreSettlementPrice);

                            instrumentDic.Remove(marketData.InstrumentID);

                            USeConsole.WriteLine(string.Format("保存{0}成功", marketData.InstrumentID));
                        }
                    }
                }
                catch (Exception ex)
                {
                    text = string.Format("保存昨日结算价异常:{0}", ex.Message);
                    WriteConsoleLog(text);
                    m_eventLogger.WriteInformation(text);
                    ctpApp.Disconnect();
                    return -1;
                }
            }

            if (ctpApp != null)
            {
                ctpApp.Disconnect();
            }

            if (instrumentDic.Count > 0)
            {
                //未查询到的合约写入文件
                foreach (USeInstrumentDetail field in instrumentDic.Values)
                {
                    text = string.Format("[{0}]结算价查询失败", field.Instrument.InstrumentCode);
                    WriteConsoleLog(text);
                    m_eventLogger.WriteInformation(text);
                }
            }
            else
            {
                try
                {
#if DEBUG
                    bool iDownLoadResult = VerifyDBSettlementPrice(tradingDate);
                    Debug.Assert(iDownLoadResult == true);
#endif
                    RefreshDBSettlementDownLoad(tradingDate, true);
                }
                catch (Exception ex)
                {
                    text = string.Format("更新结算状态失败,错误:{0}", ex.Message);
                    WriteConsoleLog(text);
                    m_eventLogger.WriteInformation(text);
                }
            }
            return 0;
        }

        private void RefreashSendimentaryMoney(DepthMarketDataField item, string instrumentID, USeMarket market, DateTime settlementDate, double settlementPrice)
        {
            //计算资金沉淀用结算价

            decimal sendimentaryMoney = 0m;

            foreach(USeInstrumentDetail insDetail in m_insDetailList)
            {
                if(instrumentID == insDetail.Instrument.InstrumentCode)
                {
                    int perSharesContract = insDetail.VolumeMultiple;//合约规模
                    decimal exchangeLongMarginRatio = insDetail.ExchangeLongMarginRatio;//保证金
                    sendimentaryMoney = Convert.ToDecimal(item.OpenInterest) * Convert.ToDecimal(settlementPrice) * perSharesContract * exchangeLongMarginRatio;//资金沉淀
                    break;
                }
            }

            Debug.Assert(sendimentaryMoney != 0m);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                {
                    connection.Open();

                    string cmdText = string.Format(@"update {0}.day_kline set sendimentary_money = @sendimentary_money 
where contract = @contract AND exchange = @exchange AND date_time = @date_time;", m_alphaDBName);

                    MySqlCommand command = new MySqlCommand(cmdText, connection);

                    command.Parameters.AddWithValue("@contract", instrumentID);
                    command.Parameters.AddWithValue("@exchange", market.ToString());
                    command.Parameters.AddWithValue("@date_time", settlementDate);
                    command.Parameters.AddWithValue("@sendimentary_money", sendimentaryMoney);
                    int result = command.ExecuteNonQuery();
                    Debug.Assert(result == 1);

                    string text = string.Format("用结算价更新资金沉淀成功,合约:{0} 结算价:{1}", item.InstrumentID ,settlementPrice);
                    WriteConsoleLog(text);
                    m_eventLogger.WriteInformation(text);
                }
            }
            catch (Exception ex)
            {
                string text = "用结算价更新资金沉淀失败:" + ex.Message;
                WriteConsoleLog(text);
            }

        }

        /// <summary>
        /// 创建交易合约管理类。
        /// </summary>
        private USeTradingInstrumentManager CreateInstrumentManager()
        {
            string dbConnStr = ConfigurationManager.ConnectionStrings["KLineDB"].ConnectionString;
            if (string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentException("Not found KLineDB ConnectionString");
            }
            string alpahDBName = ConfigurationManager.AppSettings["AlphaDBName"];
            if (string.IsNullOrEmpty(alpahDBName))
            {
                throw new ArgumentException("Not foun AlphaDBName config");
            }

            try
            {
                USeTradingInstrumentManager manager = new USeTradingInstrumentManager(dbConnStr, alpahDBName);
                //[yangming]需要调整为交易日时间
                manager.Initialize();
                string text = String.Format("{0} Create {1} OK.", this, manager);
                m_eventLogger.WriteInformation(text);
                return manager;
            }
            catch (Exception ex)
            {
                string text = "Create InstrumentManager object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 更新SettlementDownLoad状态失败则创建
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="settlementDowmLoadFlag"></param>
        private void RefreshDBSettlementDownLoad(DateTime dateTime, bool settlementDowmLoadFlag)
        {
            string cmdText = CreateDailyWorkStateUpdateSql(dateTime, settlementDowmLoadFlag);
            using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(cmdText, connection);
                int result = command.ExecuteNonQuery();
                //更新失败重新创建
                if (result < 1)
                {
                    cmdText = CreateDailyWorkStateInsertSql(dateTime, settlementDowmLoadFlag);
                    command = new MySqlCommand(cmdText, connection);
                    result = command.ExecuteNonQuery();
                    Debug.Assert(result == 1);
                }
            }
        }

        /// <summary>
        /// CTP合约数据转换。
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private USeInstrumentDetail InsturmentFiledToUSeInstrumentDetail(InstrumentField field)
        {
            USeMarket market = CtpProtocol.CtpExchangeToUSeMarket(field.ExchangeID);

            USeInstrumentDetail entity = new USeInstrumentDetail();
            entity.Instrument = new USeInstrument(field.InstrumentID,
                                        field.InstrumentName,
                                        market);
            entity.OpenDate = DateTime.ParseExact(field.OpenDate, "yyyyMMdd", null);
            entity.ExpireDate = DateTime.ParseExact(field.ExpireDate, "yyyyMMdd", null);
            if (string.IsNullOrEmpty(field.StartDelivDate) == false)
            {
                entity.StartDelivDate = DateTime.ParseExact(field.StartDelivDate, "yyyyMMdd", null);
            }
            else
            {
                entity.StartDelivDate = entity.ExpireDate.AddDays(1);
            }
            entity.EndDelivDate = DateTime.ParseExact(field.EndDelivDate, "yyyyMMdd", null);
            entity.VolumeMultiple = Convert.ToInt32(field.VolumeMultiple);
            entity.IsTrading = field.IsTrading == IntBoolType.Yes;
            entity.Varieties = field.ProductID;
            entity.PriceTick = Convert.ToDecimal(field.PriceTick);
            entity.ProductClass = CtpProtocol.CtpProductClassToUSeProductClass(field.ProductClass);
            entity.MaxMarketOrderVolume = Convert.ToInt32(field.MaxMarketOrderVolume);
            entity.MinMarketOrderVolume = Convert.ToInt32(field.MinMarketOrderVolume);
            entity.MaxLimitOrderVolume = Convert.ToInt32(field.MaxLimitOrderVolume);
            entity.MinLimitOrderVolume = Convert.ToInt32(field.MinLimitOrderVolume);
            entity.ExchangeLongMarginRatio = Convert.ToDecimal(field.LongMarginRatio);
            entity.ExchangeShortMarginRatio = Convert.ToDecimal(field.ShortMarginRatio);

            return entity;
        }

        /// <summary>
        /// 保存合约结算价数据。
        /// </summary>
        /// <param name="DepthMarketDataFieldinstrumentList"></param>
        /// <returns></returns>
        private void SaveInstumentsSettlementPriceToDB(DepthMarketDataField item, string instrumentID, USeMarket market, DateTime settlementDate, double settlementPrice)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                {
                    connection.Open();

                    string cmdText = string.Format(@"update {0}.day_kline set settlement_price = @settlement_price 
where contract = @contract AND exchange = @exchange AND date_time = @date_time;", m_alphaDBName);

//                    string cmdText = string.Format(@"update {0}.day_kline set settlement_price ={1}
//where contract = '{2}' AND exchange = '{3}' AND date_time = '{4:yyyy-MM-dd}';", m_alphaDBName,settlementPrice,instrumentID,mar);

                    MySqlCommand command = new MySqlCommand(cmdText, connection);

                    command.Parameters.AddWithValue("@contract", instrumentID);
                    command.Parameters.AddWithValue("@exchange", market.ToString());
                    command.Parameters.AddWithValue("@date_time", settlementDate);
                    command.Parameters.AddWithValue("@settlement_price", settlementPrice);
                    int result = command.ExecuteNonQuery();
                    //Debug.Assert(result == 1);
                }
            }
            catch(Exception ex)
            {
                string text = "保存结算价失败:" + ex.Message;
                WriteConsoleLog(text);
            }
          
        }

        private bool VerifyDBSettlementPrice(DateTime dateTime)
        {
            bool iVerifyResult = false;

            try
            {
                string strSel = string.Format(@"select * from alpha.day_kline where date_time = '{0}' And ( settlement_price <= 0)",
                    dateTime.ToString("yyyy-MM-dd"));

                DataTable table = new DataTable();
                using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(strSel, connection);
                    adapter.Fill(table);
                    adapter.Dispose();
                }

                if (table.Rows.Count == 0) return true;
            }
            catch (Exception ex)
            {
                string text = "检查结算价是否准备完毕失败:" + ex.Message;
                WriteConsoleLog(text);
                m_eventLogger.WriteError(text);
                return false;
            }

            return iVerifyResult;
        }


        /// <summary>
        /// 获取交易日/交易日历
        /// </summary>
        /// <returns></returns>
        private DateTime GetTradingDay()
        {
            if (m_refrashDate == QueryDay.Yesterday)
            {
                DateTime tradeDay = DateTime.Today;
                if(DateTime.Now.TimeOfDay > m_nextDayRange)
                {
                    tradeDay = DateTime.Today.AddDays(1);
                }
                string strSel = string.Format(@"select * from alpha.trade_calendar where today = '{0}';",
                    tradeDay.ToString("yyyy-MM-dd"));
                DataTable table = new DataTable();
                using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(strSel, connection);
                    adapter.Fill(table);
                    adapter.Dispose();
                }

                if (table != null && table.Rows.Count > 0)
                {
                    Debug.Assert(table.Rows.Count == 1);
                    return Convert.ToDateTime(table.Rows[0]["pre_trade_day"]);
                }
                else
                {
                    throw new Exception(string.Format("未找到{0:yyyy-MM-dd}交易日历", DateTime.Today));
                }
            }
            else if (m_refrashDate == QueryDay.Today)
            {
                return DateTime.Today;
            }
            else
            {
                throw new Exception(string.Format("不支持的RefrashDate:" + m_refrashDate.ToString()));
            }
        }

        /// <summary>
        /// 更新指定日期的结算任务状态
        /// </summary>
        /// <returns></returns>
        private string CreateDailyWorkStateUpdateSql(DateTime dateTime, bool settlementDowmLoadFlag)
        {
            string cmdText = string.Format(@"update {0}.daily_work_state set settlement_price_download = {1}
where working_day = '{2:yyyy-MM-dd}';", m_alphaDBName, settlementDowmLoadFlag ? 1 : 0, dateTime);
            return cmdText;
        }

        /// <summary>
        /// 创建一条新的工作状态
        /// </summary>
        /// <returns></returns>
        private string CreateDailyWorkStateInsertSql(DateTime dateTime, bool settlementDowmLoadFlag)
        {
            string cmdText = string.Format(@"insert into {0}.daily_work_state(working_day,settlement_price_download) 
values('{1:yyyy-MM-dd}',{2});", m_alphaDBName, dateTime, settlementDowmLoadFlag ? 1 : 0);
            return cmdText;
        }

        /// <summary>
        /// 读取配置信息。
        /// </summary>
        /// <returns></returns>
        private bool ReadConfig()
        {
            try
            {
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                Debug.Assert(!string.IsNullOrEmpty(exePath));

                Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
                if (!config.HasFile)
                {
                    throw new ApplicationException("Not found the specific configuration file.");
                }
                CtpOrderDriverSection ctpSection = config.GetSection("CtpOrderDriver") as CtpOrderDriverSection;
                m_ctpDriverConfig = ctpSection.CtpOrderDriver;
                m_ctpAccountConfig = ctpSection.CtpAccount;

                m_dbConStr = ConfigurationManager.ConnectionStrings["MarketDataDB"].ConnectionString;

                m_alphaDBName = ConfigurationManager.AppSettings["AlphaDBName"];

                //配置的查询参数
                QuerySettingsSection settingsSection = config.GetSection("QuerySettlementSettings") as QuerySettingsSection;
                m_queryNum = settingsSection.QuerySettings.QueryNum;
                m_queryFrequence = settingsSection.QuerySettings.QueryFrequence;
                m_refrashDate = settingsSection.QuerySettings.QueryDay;

            }
            catch (Exception ex)
            {
                string error = "Not found the specific configuration file," + ex.Message;
                Console.WriteLine(error);
                m_eventLogger.WriteError(error);
                return false;
            }

            return true;
        }

        private void WriteConsoleLog(string message)
        {
            Console.WriteLine("==>{0:HH:mm:ss} {1}", DateTime.Now, message);
        }
        #endregion // methods


    }
}
