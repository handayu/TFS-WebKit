using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Threading;

using USe.Common.AppLogger;
using CTPAPI;
using USe.CtpOrderQuerier;
using USe.CtpOrderQuerier.Configuration;
using MySql.Data.MySqlClient;
using USe.TradeDriver.Common;

namespace CtpInstrumentDownLoad
{
    /// <summary>
    /// 期货合约信息下载处理类。
    /// </summary>
    class DownloadServies
    {
        #region member
        private string m_dbConStr = string.Empty;
        private string m_alphaDBName = string.Empty;

        private CtpAccountElement m_ctpAccountConfig = null;
        private CtpOrderDriverElement m_ctpDriverConfig = null;
        private IAppLogger m_eventLogger = null;
        #endregion // member

        #region methods
        /// <summary>
        /// 启动结算价下载。
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
            string text = "启动下载合约服务";
            m_eventLogger.WriteInformation(text);
            WriteConsoleLog(text);

            if (ReadConfig() == false) return -1;

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
                return -1;
            }


            List<USeInstrumentDetail> entityList = new List<USeInstrumentDetail>();
            try
            {
                List<InstrumentField> instrumentList = ctpApp.QueryInstument();
                

                foreach (InstrumentField item in instrumentList)
                {
                    if (item.ProductClass != ProductClass.Futures)
                    {
                        continue;
                    }
                    try
                    {
                        USeInstrumentDetail entity = InsturmentFiledToUSeInstrumentDetail(item);

                        entityList.Add(entity);
                    }
                    catch(Exception eee)
                    {
                        Debug.Assert(false, eee.Message);
                    }
                }

                text = string.Format("查询期货合约数据完成,共计{0}个合约", entityList.Count);
                WriteConsoleLog(text);
                m_eventLogger.WriteInformation(text);

                ctpApp.Disconnect();
            }
            catch (Exception ex)
            {
                text = "查询期货合约数据失败," + ex.Message;
                WriteConsoleLog(text);
                m_eventLogger.WriteError(text);
                ctpApp.Disconnect();
                return -1;
            }

            try
            {
                SaveInstumentsToDB(entityList);
                text = string.Format("保存期货合约信息完成，共计{0}个合约", entityList.Count);
                WriteConsoleLog(text);
                m_eventLogger.WriteInformation(text);
            }
            catch (Exception ex)
            {
                text = "保存期货合约信息失败," + ex.Message;
                WriteConsoleLog(text);
                m_eventLogger.WriteError(text);
                return -1;
            }
            return 0;
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
        /// 保存合约数据。
        /// </summary>
        /// <param name="instrumentList"></param>
        /// <returns></returns>
        private int SaveInstumentsToDB(List<USeInstrumentDetail> instrumentList)
        {
            string text = string.Empty;
            List<string> existList = null;
            try
            {
                existList = GetExistInstrumentFromDB();
            }
            catch (Exception ex)
            {
                text = "查询已存在合约失败," + ex.Message;
                WriteConsoleLog(text);
                m_eventLogger.WriteError(text);
                return -1;
            }


            using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
            {
                connection.Open();

                foreach (USeInstrumentDetail item in instrumentList)
                {
                    try
                    {
                        string cmdText = string.Empty;
                        if (existList.Contains(item.Instrument.InstrumentCode))
                        {
                            cmdText = CreateInstumentUpdateSql();
                        }
                        else
                        {
                            cmdText = CreateInstrumentInsertSql();
                        }

                        MySqlCommand command = new MySqlCommand(cmdText, connection);
                        command.Parameters.AddWithValue("@contract", item.Instrument.InstrumentCode);
                        command.Parameters.AddWithValue("@contract_name", item.Instrument.InstrumentName);
                        command.Parameters.AddWithValue("@exchange", item.Instrument.Market.ToString());
                        command.Parameters.AddWithValue("@open_date", item.OpenDate.Date);
                        command.Parameters.AddWithValue("@expire_date", item.ExpireDate.Date);
                        command.Parameters.AddWithValue("@start_deliv_date", item.StartDelivDate.Date);
                        command.Parameters.AddWithValue("@end_deliv_date", item.EndDelivDate.Date);
                        command.Parameters.AddWithValue("@volume_multiple", item.VolumeMultiple);
                        command.Parameters.AddWithValue("@is_trading", item.IsTrading ? 1 : 0);
                        command.Parameters.AddWithValue("@varieties", item.Varieties);
                        command.Parameters.AddWithValue("@price_tick", item.PriceTick);
                        command.Parameters.AddWithValue("@exchange_long_margin_ratio", item.ExchangeLongMarginRatio);
                        command.Parameters.AddWithValue("@exchange_short_margin_ratio", item.ExchangeShortMarginRatio);
                        command.Parameters.AddWithValue("@product_class", item.ProductClass.ToString());
                        command.Parameters.AddWithValue("@underlying_instrument", item.UnderlyingInstrument);
                        command.Parameters.AddWithValue("@max_market_order_volume", item.MaxMarketOrderVolume);
                        command.Parameters.AddWithValue("@min_market_order_volume", item.MinMarketOrderVolume);
                        command.Parameters.AddWithValue("@max_limit_order_volume", item.MaxLimitOrderVolume);
                        command.Parameters.AddWithValue("@min_limit_order_volume", item.MinLimitOrderVolume);

                        int result = command.ExecuteNonQuery();
                        Debug.Assert(result == 1);

                        string str = string.Format("保存合约信息成功,合约:{0}", item.Instrument.InstrumentCode);
                        WriteConsoleLog(str);
                    }
                    catch(Exception ex)
                    {
                        Debug.Assert(false, ex.Message);
                        throw ex;
                    }
                }

                {
                    string cmdText = string.Format(@"update {0}.contracts set is_trading = 0 
                                            where expire_date < curdate();", m_alphaDBName);
                    MySqlCommand command = new MySqlCommand(cmdText, connection);
                    int result = command.ExecuteNonQuery();
                }

                {
                    string cmdText = CreateDailyWorkStateUpdateSql();
                    MySqlCommand command = new MySqlCommand(cmdText, connection);
                    int result = command.ExecuteNonQuery();
                    if (result < 1)
                    {
                        cmdText = CreateDailyWorkStateInsertSql();
                        command = new MySqlCommand(cmdText, connection);
                        result = command.ExecuteNonQuery();
                        Debug.Assert(result == 1);
                    }
                }
            }

            return instrumentList.Count;
        }

        private string CreateInstrumentInsertSql()
        {
            string cmdText = string.Format(@"insert into {0}.contracts(contract,contract_name,exchange,open_date,expire_date,
start_deliv_date,end_deliv_date,volume_multiple,is_trading,varieties,price_tick,
exchange_long_margin_ratio,exchange_short_margin_ratio,product_class,underlying_instrument,max_market_order_volume,
min_market_order_volume,max_limit_order_volume,min_limit_order_volume)
VALUES(@contract,@contract_name,@exchange,@open_date,@expire_date,
@start_deliv_date,@end_deliv_date,@volume_multiple,@is_trading,@varieties,@price_tick,
@exchange_long_margin_ratio,@exchange_short_margin_ratio,@product_class,@underlying_instrument,@max_market_order_volume,
@min_market_order_volume,@max_limit_order_volume,@min_limit_order_volume);",
                m_alphaDBName);
            return cmdText;
        }

        private string CreateInstumentUpdateSql()
        {
            string cmdText = string.Format(@"update {0}.contracts set contract_name = @contract_name,
open_date = @open_date,expire_date = @expire_date,start_deliv_date = @start_deliv_date,end_deliv_date = @end_deliv_date,
volume_multiple = @volume_multiple,is_trading = @is_trading,varieties = @varieties,price_tick = @price_tick,
exchange_long_margin_ratio = @exchange_long_margin_ratio,exchange_short_margin_ratio = @exchange_short_margin_ratio,
product_class = @product_class,underlying_instrument = @underlying_instrument,
max_market_order_volume = @max_market_order_volume,min_market_order_volume = @min_market_order_volume,
max_limit_order_volume = @max_limit_order_volume,min_limit_order_volume = @min_limit_order_volume
WHERE contract = @contract AND exchange = @exchange;", m_alphaDBName);
            return cmdText;
        }

        private string CreateDailyWorkStateInsertSql()
        {
            string cmdText = string.Format(@"insert into {0}.daily_work_state(working_day,instrument_download) 
values(CURDATE(),1);", m_alphaDBName);
            return cmdText;
        }

        private string CreateDailyWorkStateUpdateSql()
        {
            string cmdText = string.Format(@"update {0}.daily_work_state set instrument_download = 1
where working_day = CURDATE();", m_alphaDBName);
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

        /// <summary>
        /// 从数据库读取已存在的Instrument。
        /// </summary>
        /// <returns></returns>
        private List<string> GetExistInstrumentFromDB()
        {
            //只读取期货信息。
            string strSel = string.Format(@"select contract,exchange from {0}.contracts 
                              where product_class in('Futures');", m_alphaDBName);

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(strSel, connection);
                adapter.Fill(table);
            }

            List<string> instrmentList = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                instrmentList.Add(row["contract"].ToString());
            }
            return instrmentList;
        }

        private void WriteConsoleLog(string message)
        {
            Console.WriteLine("==>{0:HH:mm:ss} {1}", DateTime.Now, message);
        }
        #endregion // methods
    }
}
