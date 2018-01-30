using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using USe.Common.AppLogger;
using USe.Common.DBDriver;
using USe.Common.Manager;
using USe.TradeDriver.Common;

namespace RefreashSendimentaryMoney
{
    public class SendimentaryMoneyCalService
    {
        private string m_dbConnStr = string.Empty;
        private string m_alphaDBName = string.Empty;
        private IAppLogger m_eventLogger = null;
        private List<USeInstrumentDetail> m_insDetailList = null;

        private TradeCalendarManager m_tradeCalendarManager = null;

        /// <summary>
        /// 开始启动
        /// </summary>
        public int Start()
        {
            //获取配置文件
            {
                try
                {
                    if (false == ReadConfig()) return -1;
                }
                catch (Exception ex)
                {
                    string info = "基础数据：配置文件加载失败" + ex.Message;
                    m_eventLogger.WriteInformation(info);
                    WriteConsoleLog(info);
                    return -1;
                }
            }

            //加载交易日历
            try
            {
                TradeCalendarManager tradeCalendarManager = new TradeCalendarManager(m_dbConnStr, m_alphaDBName);
                tradeCalendarManager.Initialize(DateTime.Today.AddDays(-150), DateTime.Today);
                m_tradeCalendarManager = tradeCalendarManager;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("初始化交易日历失败," + ex.Message);
            }

            //加载log日志
            {
                try
                {
                    m_eventLogger = AppLogger.InitInstance();
                }
                catch
                {
                    m_eventLogger = new NullLogger("DownloadProcessor_DefaultLogger");
                }

                m_eventLogger.LineFeed();
                string text = "开始用结算价更新资金沉淀任务服务";
                m_eventLogger.WriteInformation(text);
                WriteConsoleLog(text);
            }

            //获取合约列表详情
            {
                try
                {
                    m_insDetailList = GetInsDetailList();
                }
                catch (Exception ex)
                {
                    string info = "基础数据：获取合约详情列表失败" + ex.Message;
                    m_eventLogger.WriteInformation(info);
                    WriteConsoleLog(info);
                    return -1;
                }
            }

            {
                try
                {
                    List<USeKLine> klineList = GetTodayDayKlineList(DateTime.Today.AddDays(-20));
                    RefreashSendimentaryMoney(klineList);
                }
                catch (Exception ex)
                {
                    string info = "服务：获取当天交易日K线数据,更新资金沉淀失败" + ex.Message;
                    m_eventLogger.WriteInformation(info);
                    WriteConsoleLog(info);
                    return -1;
                }

            }

            return 0;
        }

        //输出console日志
        private void WriteConsoleLog(string message)
        {
            Console.WriteLine("==>{0:HH:mm:ss} {1}", DateTime.Now, message);
        }

        /// <summary>
        /// 获取合约详情列表
        /// </summary>
        /// <returns></returns>
        private List<USeInstrumentDetail> GetInsDetailList()
        {
            USeTradingInstrumentManager tradingInsManager = CreateInstrumentManager();
            List<USeInstrumentDetail> insDetailList = tradingInsManager.GetAllInstrumentDetails();
            Debug.Assert(insDetailList != null);
            return insDetailList;
        }

        /// <summary>
        /// 创建交易合约管理类。
        /// </summary>
        private USeTradingInstrumentManager CreateInstrumentManager()
        {
            string dbConnStr = ConfigurationManager.ConnectionStrings["MarketDataDB"].ConnectionString;
            if (string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentException("Not found MarketDataDB ConnectionString");
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
                manager.Initialize(DateTime.Today);
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
        /// 获取当天交易日的K线
        /// </summary>
        public List<USeKLine> GetTodayDayKlineList(DateTime dateTime)
        {
            string strSel = string.Format(@"select * from {0}.day_kline where date_time>='{1:yyyy-MM-dd 00:00:00}';",
                m_alphaDBName, dateTime);

            DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

            List<USeKLine> klineList = new List<USeKLine>();
            foreach (DataRow row in table.Rows)
            {
                try
                {
                    USeKLine kline = new USeKLine();

                    kline.InstrumentCode = row["contract"].ToString();
                    kline.Market = (USeMarket)Enum.Parse(typeof(USeMarket), row["exchange"].ToString());

                    kline.Close = Convert.ToDecimal(row["price_close"]);
                    kline.DateTime = Convert.ToDateTime(row["date_time"]);
                    kline.OpenInterest = Convert.ToDecimal(row["openinterest"]);
                    kline.SettlementPrice = (row["settlement_price"] != DBNull.Value) ? Convert.ToDecimal(row["settlement_price"]) : 0m;

                    klineList.Add(kline);
                }
                catch (Exception ex)
                {
                    throw new Exception("初始化当天交易日的K线:" + ex.Message);
                }

            }

            return klineList;
        }

        /// <summary>
        /// 获取当前K线前一个交易日的资金沉淀去用于计算资金流入流出
        /// </summary>
        /// <param name="kline"></param>
        /// <returns></returns>
        private decimal GetLastTradingDayInsSendimentaryMoney(USeKLine kline, USeInstrumentDetail insDetail)
        {
            decimal lastTradingDaySendimentaryMoney = 0m;

            DateTime preTradingDay = m_tradeCalendarManager.GetPreTradingDate(kline.DateTime);

            string strSel = string.Format(@"select * from {0}.day_kline where contract='{1}' AND date_time='{2:yyyy-MM-dd 00:00:00}';",
                m_alphaDBName, kline.InstrumentCode, preTradingDay);

            DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

            foreach (DataRow row in table.Rows)
            {
                try
                {
                    USeKLine tempKline = new USeKLine();

                    tempKline.InstrumentCode = row["contract"].ToString();
                    tempKline.Market = (USeMarket)Enum.Parse(typeof(USeMarket), row["exchange"].ToString());

                    tempKline.Close = Convert.ToDecimal(row["price_close"]);
                    tempKline.DateTime = Convert.ToDateTime(row["date_time"]);
                    tempKline.OpenInterest = Convert.ToDecimal(row["openinterest"]);
                    tempKline.SettlementPrice = (row["settlement_price"] != DBNull.Value) ? Convert.ToDecimal(row["settlement_price"]) : 0m;
                    tempKline.SendimentaryMoney = tempKline.OpenInterest * tempKline.SettlementPrice * insDetail.VolumeMultiple * insDetail.ExchangeLongMarginRatio;
                    return lastTradingDaySendimentaryMoney = tempKline.SendimentaryMoney;

                }
                catch (Exception ex)
                {
                    throw new Exception("初始化当天交易日的K线:" + ex.Message);
                }
            }

            return lastTradingDaySendimentaryMoney;
        }

        private int VerifyISIndexOrSeries(USeKLine kline)
        {
            Debug.Assert(kline != null);

            string seriesContracts = USeTraderProtocol.GetVarieties(kline.InstrumentCode) + "9999";
            string indexContracts = USeTraderProtocol.GetVarieties(kline.InstrumentCode) + "8888";

            if (kline.InstrumentCode == seriesContracts)
            {
                return 1;
            }
            else if (kline.InstrumentCode == indexContracts)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取某一个交易日某一个品种的所有合约K线用于计算Index
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        private List<USeKLine> GetTradingDayIndexKlineContracts(DateTime dateTime, string productID)
        {

            string strSel = string.Format(@"select * from {0}.day_kline where contract like '{1}%' AND date_time='{2:yyyy-MM-dd 00:00:00}';",
                m_alphaDBName, productID, dateTime);

            DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

            List<USeKLine> klineList = new List<USeKLine>();
            foreach (DataRow row in table.Rows)
            {
                try
                {
                    USeKLine tempKline = new USeKLine();

                    tempKline.InstrumentCode = row["contract"].ToString();
                    tempKline.Market = (USeMarket)Enum.Parse(typeof(USeMarket), row["exchange"].ToString());

                    tempKline.Close = Convert.ToDecimal(row["price_close"]);
                    tempKline.DateTime = Convert.ToDateTime(row["date_time"]);
                    tempKline.OpenInterest = Convert.ToDecimal(row["openinterest"]);
                    tempKline.SettlementPrice = (row["settlement_price"] != DBNull.Value) ? Convert.ToDecimal(row["settlement_price"]) : 0m;
                    tempKline.SendimentaryMoney = (row["sendimentary_money"] != DBNull.Value) ? Convert.ToDecimal(row["sendimentary_money"]) : 0m;

                    if (VerifyISIndexOrSeries(tempKline) == 0 && USeTraderProtocol.GetVarieties(tempKline.InstrumentCode) == productID)
                    {
                        klineList.Add(tempKline);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("初始化当天交易日的K线:" + ex.Message);
                }
            }

            return klineList;
        }

        /// <summary>
        /// 获取当前K线前一个交易日指数的的资金沉淀去用于计算资金流入流出
        /// </summary>
        /// <param name="kline"></param>
        /// <returns></returns>
        private decimal GetLastTradingDayIndexInsSendimentaryMoney(USeKLine kline)
        {
            decimal lastTradingDaySendimentaryMoney = 0m;

            DateTime preTradingDay = m_tradeCalendarManager.GetPreTradingDate(kline.DateTime);

            string strSel = string.Format(@"select * from {0}.day_kline where contract='{1}' AND date_time='{2:yyyy-MM-dd 00:00:00}';",
                m_alphaDBName, kline.InstrumentCode, preTradingDay);

            DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

            foreach (DataRow row in table.Rows)
            {
                try
                {
                    USeKLine tempKline = new USeKLine();

                    tempKline.InstrumentCode = row["contract"].ToString();
                    tempKline.Market = (USeMarket)Enum.Parse(typeof(USeMarket), row["exchange"].ToString());

                    tempKline.Close = Convert.ToDecimal(row["price_close"]);
                    tempKline.DateTime = Convert.ToDateTime(row["date_time"]);
                    tempKline.OpenInterest = Convert.ToDecimal(row["openinterest"]);
                    tempKline.SettlementPrice = (row["settlement_price"] != DBNull.Value) ? Convert.ToDecimal(row["settlement_price"]) : 0m;
                    tempKline.SendimentaryMoney = (row["sendimentary_money"] != DBNull.Value) ? Convert.ToDecimal(row["sendimentary_money"]) : 0m;
                    return lastTradingDaySendimentaryMoney = tempKline.SendimentaryMoney;

                }
                catch (Exception ex)
                {
                    throw new Exception("GetLastTradingDayIndexInsSendimentaryMoney:" + ex.Message);
                }
            }

            return lastTradingDaySendimentaryMoney;
        }


        /// <summary>
        /// 更新资金沉淀
        /// </summary>
        /// <param name="klineList"></param>
        private void RefreashSendimentaryMoney(List<USeKLine> klineList)
        {

            Debug.Assert(klineList != null);
            RefreashStandartContractsSendimentaryMoney(klineList);
            RefreashSeriesContractsSendimentaryMoney(klineList);
            RefreashIndexSendimentaryMoney(klineList);
        }

        private void RefreashIndexSendimentaryMoney(List<USeKLine> klineList)
        {
            Debug.Assert(klineList != null);
            foreach (USeKLine kline in klineList)
            {
                #region Index更新
                //非标准合约（自定义）如果是主力和指数合约的话,取品种相同，如果是其他非主力，非指数，非四大交易所合约
                if (VerifyISIndexOrSeries(kline) == -1)//指数更新
                {
                    decimal sendimentaryMoney = 0m;
                    decimal flowMoney = 0m;

                    if (kline.SettlementPrice == 0m)
                    {
                        List<USeKLine> tradingDayIndexKlineContracts = GetTradingDayIndexKlineContracts(kline.DateTime, USeTraderProtocol.GetVarieties(kline.InstrumentCode));
                        foreach (USeKLine k in tradingDayIndexKlineContracts)
                        {
                            sendimentaryMoney = sendimentaryMoney + k.SendimentaryMoney;
                        }
                        flowMoney = sendimentaryMoney - GetLastTradingDayIndexInsSendimentaryMoney(kline);
                    }
                    else
                    {
                        foreach (USeKLine k in GetTradingDayIndexKlineContracts(kline.DateTime, USeTraderProtocol.GetVarieties(kline.InstrumentCode)))
                        {
                            sendimentaryMoney = sendimentaryMoney + k.SendimentaryMoney;
                        }
                        flowMoney = sendimentaryMoney - GetLastTradingDayIndexInsSendimentaryMoney(kline);
                    }

                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
                        {
                            connection.Open();

                            string cmdText = string.Format(@"update {0}.day_kline set sendimentary_money=@sendimentary_money,flow_fund=@flow_fund where contract=@contract AND date_time=@date_time;", m_alphaDBName);

                            MySqlCommand command = new MySqlCommand(cmdText, connection);

                            command.Parameters.AddWithValue("@contract", kline.InstrumentCode);
                            command.Parameters.AddWithValue("@date_time", kline.DateTime);
                            command.Parameters.AddWithValue("@sendimentary_money", sendimentaryMoney);
                            command.Parameters.AddWithValue("@flow_fund", flowMoney);

                            int result = command.ExecuteNonQuery();
                            Debug.Assert(result == 1);

                            if (kline.SettlementPrice == 0m)
                            {
                                string text = string.Format("用当前收盘价更新指数的资金沉淀-资金流向成功,合约:{0} 交易所:{1} 当前收盘价:{2} 结算价:{3} 沉淀资金:{4} 资金流向:{5} K线时间:{6}", kline.InstrumentCode, kline.Market.ToString(), kline.Close, kline.SettlementPrice, sendimentaryMoney, flowMoney, kline.DateTime);
                                WriteConsoleLog(text);
                                m_eventLogger.WriteInformation(text);
                            }
                            else
                            {
                                string text = string.Format("用结算价更新指数的资金沉淀-资金流向成功,合约:{0} 交易所:{1} 当前收盘价:{2} 结算价:{3}  沉淀资金:{4} 资金流向:{5} K线时间:{6}", kline.InstrumentCode, kline.Market.ToString(), kline.Close, kline.SettlementPrice, sendimentaryMoney, flowMoney, kline.DateTime);
                                WriteConsoleLog(text);
                                m_eventLogger.WriteInformation(text);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        string text = "用结算价更新指数资金沉淀失败:" + ex.Message;
                        WriteConsoleLog(text);
                    }
                }
                #endregion
            }
        }

        private void RefreashSeriesContractsSendimentaryMoney(List<USeKLine> klineList)
        {
            Debug.Assert(klineList != null);
            foreach (USeKLine kline in klineList)
            {
                #region 主力合约更新
                if (VerifyISIndexOrSeries(kline) == 1) //主力合约更新
                {
                    //找到当天的主力合约表，确定当天哪一个合约为主力合约，然后把day_kline中的主力合约的数据更新到相应的9999上去
                    string seriesContract = GetTradingDaySeriesKline(kline.DateTime, USeTraderProtocol.GetVarieties(kline.InstrumentCode));
                    USeKLine seriesKline = GetSeriesTradingKline(kline.DateTime, seriesContract);

                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
                        {
                            connection.Open();

                            string cmdText = string.Format(@"update {0}.day_kline set sendimentary_money=@sendimentary_money,flow_fund=@flow_fund where contract=@contract AND date_time=@date_time;", m_alphaDBName);

                            MySqlCommand command = new MySqlCommand(cmdText, connection);

                            command.Parameters.AddWithValue("@contract", kline.InstrumentCode);
                            command.Parameters.AddWithValue("@date_time", kline.DateTime);
                            command.Parameters.AddWithValue("@sendimentary_money", seriesKline.SendimentaryMoney);
                            command.Parameters.AddWithValue("@flow_fund", seriesKline.FlowFund);

                            int result = command.ExecuteNonQuery();
                            Debug.Assert(result == 1);

                            if (kline.SettlementPrice == 0m)
                            {
                                string text = string.Format("用当前收盘价更新主力合约资金沉淀-资金流向成功,合约:{0} 交易所:{1} 当前收盘价:{2} 结算价:{3} 沉淀资金:{4} 资金流向:{5} K线时间:{6}", kline.InstrumentCode, kline.Market.ToString(), kline.Close, kline.SettlementPrice, seriesKline.SendimentaryMoney, seriesKline.FlowFund, kline.DateTime);
                                WriteConsoleLog(text);
                                m_eventLogger.WriteInformation(text);
                            }
                            else
                            {
                                string text = string.Format("用结算价更新主力合约资金沉淀-资金流向成功,合约:{0} 交易所:{1} 当前收盘价:{2} 结算价:{3}  沉淀资金:{4} 资金流向:{5} K线时间:{6}", kline.InstrumentCode, kline.Market.ToString(), kline.Close, kline.SettlementPrice, seriesKline.SendimentaryMoney, seriesKline.FlowFund, kline.DateTime);
                                WriteConsoleLog(text);
                                m_eventLogger.WriteInformation(text);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        string text = "用结算价更新主力合约资金沉淀失败:" + ex.Message;
                        WriteConsoleLog(text);
                    }
                }

                #endregion
            }
        }

        private void RefreashStandartContractsSendimentaryMoney(List<USeKLine> klineList)
        {
            Debug.Assert(klineList != null);
            foreach (USeKLine kline in klineList)
            {
                #region 标准交易所合约更新
                //标准合约去详细遍历
                foreach (USeInstrumentDetail insDetail in m_insDetailList)
                {
                    if (kline.InstrumentCode != insDetail.Instrument.InstrumentCode) continue;

                    int perSharesContract = insDetail.VolumeMultiple;//合约规模
                    decimal exchangeLongMarginRatio = insDetail.ExchangeLongMarginRatio;//保证金
                    decimal sendimentaryMoney = 0m;
                    decimal flowMoney = 0m;

                    if (kline.SettlementPrice == 0m)
                    {
                        sendimentaryMoney = kline.OpenInterest * kline.Close * perSharesContract * exchangeLongMarginRatio;//资金沉淀
                        flowMoney = sendimentaryMoney - GetLastTradingDayInsSendimentaryMoney(kline, insDetail);
                    }
                    else
                    {
                        #region 测试数据
                        //if (kline.InstrumentCode == "zn1711" && kline.DateTime == new DateTime(2017, 10, 30, 0, 0, 0))
                        //{
                        //    sendimentaryMoney = kline.OpenInterest * kline.SettlementPrice * perSharesContract * exchangeLongMarginRatio;//资金沉淀
                        //    flowMoney = sendimentaryMoney - GetLastTradingDayInsSendimentaryMoney(kline, insDetail);
                        //}
                        #endregion

                        sendimentaryMoney = kline.OpenInterest * kline.SettlementPrice * perSharesContract * exchangeLongMarginRatio;//资金沉淀
                        flowMoney = sendimentaryMoney - GetLastTradingDayInsSendimentaryMoney(kline, insDetail);
                    }

                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
                        {
                            connection.Open();

                            string cmdText = string.Format(@"update {0}.day_kline set sendimentary_money=@sendimentary_money,flow_fund=@flow_fund where contract=@contract AND date_time=@date_time;", m_alphaDBName);

                            MySqlCommand command = new MySqlCommand(cmdText, connection);

                            command.Parameters.AddWithValue("@contract", kline.InstrumentCode);
                            command.Parameters.AddWithValue("@date_time", kline.DateTime);
                            command.Parameters.AddWithValue("@sendimentary_money", sendimentaryMoney);
                            command.Parameters.AddWithValue("@flow_fund", flowMoney);

                            int result = command.ExecuteNonQuery();
                            Debug.Assert(result == 1);

                            if (kline.SettlementPrice == 0m)
                            {
                                string text = string.Format("用当前收盘价更新标准合约资金沉淀-资金流向成功,合约:{0} 交易所:{1} 当前收盘价:{2} 结算价:{3} 沉淀资金:{4} 资金流向:{5} K线时间:{6}", kline.InstrumentCode, kline.Market.ToString(), kline.Close, kline.SettlementPrice, sendimentaryMoney, flowMoney, kline.DateTime);
                                WriteConsoleLog(text);
                                m_eventLogger.WriteInformation(text);
                            }
                            else
                            {
                                string text = string.Format("用结算价更新标准合约资金沉淀-资金流向成功,合约:{0} 交易所:{1} 当前收盘价:{2} 结算价:{3}  沉淀资金:{4} 资金流向:{5} K线时间:{6}", kline.InstrumentCode, kline.Market.ToString(), kline.Close, kline.SettlementPrice, sendimentaryMoney, flowMoney, kline.DateTime);
                                WriteConsoleLog(text);
                                m_eventLogger.WriteInformation(text);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        string text = "用结算价更新资金沉淀失败:" + ex.Message;
                        WriteConsoleLog(text);
                    }
                }
                #endregion
            }
        }


        /// <summary>
        /// 获取某个交易日，某个品种的主力合约
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        private string GetTradingDaySeriesKline(DateTime dateTime, string productID)
        {
            string strSel = string.Format(@"select * from {0}.main_contract where varieties='{1}' AND settlement_date='{2:yyyy-MM-dd}';",
                m_alphaDBName, productID, dateTime);

            DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

            foreach (DataRow row in table.Rows)
            {
                try
                {
                    return row["main_contract"].ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetTradingDaySeriesKline:" + ex.Message);
                }
            }

            return "";
        }

        private USeKLine GetSeriesTradingKline(DateTime dateTime, string seriesInstrument)
        {
            string strSel = string.Format(@"select * from {0}.day_kline where contract='{1}' AND date_time='{2:yyyy-MM-dd 00:00:00}';",
    m_alphaDBName, seriesInstrument, dateTime);

            DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

            foreach (DataRow row in table.Rows)
            {
                try
                {
                    USeKLine tempKline = new USeKLine();

                    tempKline.InstrumentCode = row["contract"].ToString();
                    tempKline.Market = (USeMarket)Enum.Parse(typeof(USeMarket), row["exchange"].ToString());

                    tempKline.Close = Convert.ToDecimal(row["price_close"]);
                    tempKline.DateTime = Convert.ToDateTime(row["date_time"]);
                    tempKline.OpenInterest = Convert.ToDecimal(row["openinterest"]);
                    tempKline.SettlementPrice = (row["settlement_price"] != DBNull.Value) ? Convert.ToDecimal(row["settlement_price"]) : 0m;
                    tempKline.SendimentaryMoney = (row["sendimentary_money"] != DBNull.Value) ? Convert.ToDecimal(row["sendimentary_money"]) : 0m;
                    tempKline.FlowFund = (row["flow_fund"] != DBNull.Value) ? Convert.ToDecimal(row["flow_fund"]) : 0m;

                    return tempKline;

                }
                catch (Exception ex)
                {
                    throw new Exception("GetTradingDaySeriesKline:" + ex.Message);
                }
            }

            return null;
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

                m_dbConnStr = ConfigurationManager.ConnectionStrings["MarketDataDB"].ConnectionString;
                m_alphaDBName = ConfigurationManager.AppSettings["AlphaDBName"];

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
    }
}
