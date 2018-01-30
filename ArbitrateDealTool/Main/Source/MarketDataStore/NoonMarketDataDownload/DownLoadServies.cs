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


namespace NoonMarketDataDownload
{
    /// <summary>
    /// 期货午盘价下载服务。
    /// </summary>
    class DownloadServies
    {
        #region member
        private string m_dbConStr = string.Empty;
        private string m_alphaDBName = string.Empty;

        private TimeSpan m_queryFrequence = new TimeSpan(0, 5, 0);   //查询间隔分钟

        Dictionary<string, USeInstrumentDetail> instrumentDic = new Dictionary<string, USeInstrumentDetail>();

        private TimeSpan m_noonExchangeEndTime = TimeSpan.Zero;//午盘休盘时间
        private TimeSpan m_noonExchangeBeginTime = TimeSpan.Zero;//午盘下午开盘时间

        private CtpAccountElement m_ctpAccountConfig = null;
        private CtpOrderDriverElement m_ctpDriverConfig = null;
        private IAppLogger m_eventLogger = null;
        #endregion // member

        #region methods
        /// <summary>
        /// 启动午盘休盘价下载。
        /// </summary>
        public int Start()
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
            string text = "开始下载午盘收盘价服务";
            m_eventLogger.WriteInformation(text);
            USeConsole.WriteLine(text);

            if (ReadConfig() == false) return -1;

            CtpOrderQuerier ctpApp = new CtpOrderQuerier();
            try
            {
                ctpApp.Connect(m_ctpDriverConfig.Address, m_ctpDriverConfig.Port,
                       m_ctpDriverConfig.LoginTimeOut, m_ctpDriverConfig.QueryTimeOut);
                text = "连接CTP交易服务器成功";
                USeConsole.WriteLine(text);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                text = "连接CTP交易服务器失败," + ex.Message;
                USeConsole.WriteLine(text);
                m_eventLogger.WriteError(text);
                ctpApp.Disconnect();
                return -1;
            }

            try
            {
                ctpApp.Login(m_ctpAccountConfig.ID, m_ctpAccountConfig.Password, m_ctpAccountConfig.BrokerID);
                text = "登陆CTP交易服务器成功";
                USeConsole.WriteLine(text);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                text = "登陆CTP交易服务器失败," + ex.Message;
                USeConsole.WriteLine(text);
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
                USeConsole.WriteLine(text);
                m_eventLogger.WriteInformation(text);

            }
            catch (Exception ex)
            {
                text = "查询期货合约数据失败," + ex.Message;
                USeConsole.WriteLine(text);
                m_eventLogger.WriteError(text);
                ctpApp.Disconnect();
                return -1;
            }


            List<DepthMarketDataField> depthMarketDataFieldList = new List<DepthMarketDataField>();
            depthMarketDataFieldList = ctpApp.QueryDepthMarketData();
            //while (true)
            //{
            //    depthMarketDataFieldList = ctpApp.QueryDepthMarketData();

            //    //返回大于下午开盘的行情时间
            //    if (VerfiyIsNoonBeginTime(depthMarketDataFieldList) == true)
            //    {
            //        text = string.Format("[{0}]行情已经进入下午开盘时间不在午盘时间内", DateTime.Now);
            //        USeConsole.WriteLine(text);
            //        m_eventLogger.WriteError(text);
            //        ctpApp.Disconnect();
            //        return -1;
            //    }

            //    //未找到大于11:30:00。
            //    if (VerfiyIsNoonEndTime(depthMarketDataFieldList) == false)
            //    {
            //        Thread.Sleep(m_queryFrequence);
            //        continue;
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            ctpApp.Disconnect();

            try
            {
                foreach (DepthMarketDataField marketData in depthMarketDataFieldList)
                {
                    if (instrumentDic.ContainsKey(marketData.InstrumentID))
                    {
                        USeMarket market = instrumentDic[marketData.InstrumentID].Instrument.Market;
                        //保存收盘价
                        SaveClosePriceToDB(marketData, marketData.InstrumentID, market, DateTime.Today, marketData.LastPrice);
                        instrumentDic.Remove(marketData.InstrumentID);

                        USeConsole.WriteLine(string.Format("保存{0}成功", marketData.InstrumentID));

                    }
                }

                RefrashPriceCloseDownLoad(DateTime.Today, 1);
            }
            catch (Exception ex)
            {
                text = string.Format("查询,保存当日午盘数据异常:{0}", ex.Message);
                USeConsole.WriteLine(text);
                m_eventLogger.WriteInformation(text);
                return -1;
            }


            return 0;
        }

        /// <summary>
        /// 中午收盘
        /// </summary>
        /// <param name="tempDepthMarketDataFieldList"></param>
        /// <returns></returns>
        private bool VerfiyIsNoonEndTime(List<DepthMarketDataField> tempDepthMarketDataFieldList)
        {
            Debug.Assert(tempDepthMarketDataFieldList != null);

            foreach (DepthMarketDataField field in tempDepthMarketDataFieldList)
            {
                DateTime updateDateTime = Convert.ToDateTime(field.UpdateTime);
                DateTime noonEndTime = Convert.ToDateTime(m_noonExchangeEndTime);

                //大于中午收盘时间
                if (updateDateTime >= noonEndTime)
                {
                    return true;
                }
            }
            return false;
        }

        private TimeSpan m_morningTimeSpan = new TimeSpan(8, 50, 0);
        private TimeSpan m_afterNoontimeSpan = new TimeSpan(15, 30, 0);


        /// <summary>
        /// 下午开盘
        /// </summary>
        /// <param name="tempDepthMarketDataFieldList"></param>
        /// <returns></returns>
        private bool VerfiyIsNoonBeginTime(List<DepthMarketDataField> tempDepthMarketDataFieldList)
        {
            Debug.Assert(tempDepthMarketDataFieldList != null);

            foreach (DepthMarketDataField field in tempDepthMarketDataFieldList)
            {
                TimeSpan updateDateTime = Convert.ToDateTime(field.UpdateTime).TimeOfDay;

                //大于下午开盘时间（且行情是在白盘）
                if (updateDateTime >= m_noonExchangeBeginTime)
                {
                    DepthMarketDataField fieldTemp = field;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 更新状态失败则创建
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="settlementDowmLoadFlag"></param>
        private void RefrashPriceCloseDownLoad(DateTime dateTime, int priceCloseDowmLoadFlag)
        {
            try
            {
                string cmdText = CreateDailyWorkStateUpdateSql(dateTime, priceCloseDowmLoadFlag);
                using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(cmdText, connection);
                    int result = command.ExecuteNonQuery();
                    //更新失败重新创建
                    if (result < 1)
                    {
                        cmdText = CreateDailyWorkStateInsertSql(dateTime, priceCloseDowmLoadFlag);
                        command = new MySqlCommand(cmdText, connection);
                        result = command.ExecuteNonQuery();
                        Debug.Assert(result == 1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("更新午盘数据下载任务失败：" + ex.Message);
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
                //[yangming] 有的合约查询不到开始交割日，暂时用到期日下一天
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
        private void SaveClosePriceToDB(DepthMarketDataField item, string instrumentID, USeMarket market, DateTime dateTime, double priceClose)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                {
                    connection.Open();

                    string cmdText = string.Empty;
                    cmdText = CreateClosePrice2UpdateSql();

                    MySqlCommand command = new MySqlCommand(cmdText, connection);

                    command.Parameters.AddWithValue("@contract", instrumentID);
                    command.Parameters.AddWithValue("@exchange", market.ToString());
                    command.Parameters.AddWithValue("@date_time", dateTime);
                    command.Parameters.AddWithValue("@price_close2", (decimal)priceClose);
                    int result = command.ExecuteNonQuery();
                    //Debug.Assert(result == 1);
                }
            
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新日线数据的ClosePrice2
        /// </summary>
        /// <returns></returns>
        private string CreateClosePrice2UpdateSql()
        {
            string cmdText = string.Format(@"update {0}.day_kline set price_close2 = @price_close2 
WHERE contract = @contract AND exchange = @exchange AND date_time = @date_time;", m_alphaDBName);
            return cmdText;
        }

        /// <summary>
        /// 更新指定日期午盘任务状态
        /// </summary>
        /// <returns></returns>
        private string CreateDailyWorkStateUpdateSql(DateTime dateTime, int stateFlag)
        {
            string cmdText = string.Format(@"update {0}.daily_work_state set closePrice2_download = {1}
where working_day = '{2:yyyy-MM-dd}';", m_alphaDBName, stateFlag, dateTime);
            return cmdText;
        }

        /// <summary>
        /// 创建一条新的工作状态
        /// </summary>
        /// <returns></returns>
        private string CreateDailyWorkStateInsertSql(DateTime dateTime, int stateFlag)
        {
            string cmdText = string.Format(@"insert into {0}.daily_work_state(working_day,closePrice2_download) 
values('{1:yyyy-MM-dd}',{2});", m_alphaDBName, dateTime, stateFlag);
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
                m_queryFrequence = settingsSection.QuerySettings.QueryFrequence;
                m_noonExchangeEndTime = settingsSection.QuerySettings.ExchangeNoonEndTime;
                m_noonExchangeBeginTime = settingsSection.QuerySettings.ExchangeNoonBeginTime;

            }
            catch (Exception ex)
            {
                string error = "Not found the specific configuration file," + ex.Message;
                USeConsole.WriteLine(error);
                m_eventLogger.WriteError(error);
                return false;
            }

            return true;
        }

        //private void WriteConsoleLog(string message)
        //{
        //    Console.WriteLine("==>{0:HH:mm:ss} {1}", DateTime.Now, message);
        //}
        #endregion // methods
    }
}

