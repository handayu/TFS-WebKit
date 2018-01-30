using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common.AppLogger;
using MySql;
using System.Data;
using System.Configuration;
using USe.Common;
using System.Threading;
using System.Diagnostics;
using MySql.Data.MySqlClient;

using USe.TradeDriver.Common;

namespace HistoryKLineImport
{
    /// <summary>
    /// 合约指数导入服务。
    /// </summary>
    class ContractIndexImportServices : ImportServices
    {
        #region member
        private DateTime m_beginDate = DateTime.MinValue;
        private DateTime m_endDate = DateTime.MaxValue;
        #endregion

        #region construction
        public ContractIndexImportServices(IAppLogger eventLogger)
            :base(eventLogger)
        {

        }
        #endregion

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
                string beginDateValue = ConfigurationManager.AppSettings["MainContractIndexBeginDate"];
                if (string.IsNullOrEmpty(beginDateValue))
                {
                    string text = "Not found MainContractIndexBeginDate";
                    m_eventLogger.WriteError(text);
                    USeConsole.WriteLine(text);
                    return false;
                }

                string endDateValue = ConfigurationManager.AppSettings["MainContractIndexEndDate"];
                if (string.IsNullOrEmpty(endDateValue))
                {
                    string text = "Not found MainContractIndexEndDate";
                    m_eventLogger.WriteError(text);
                    USeConsole.WriteLine(text);
                    return false;
                }

                m_beginDate = Convert.ToDateTime(beginDateValue);
                m_endDate = Convert.ToDateTime(endDateValue);
            }
            catch (Exception ex)
            {
                string text = "初始化合约指数导入服务失败," + ex.Message;
                m_eventLogger.WriteError(text);
                USeConsole.WriteLine(text);
                return false;
            }

            return true;
        }

        public override bool Run()
        {
            return true;
        }

        /// <summary>
        /// K先保存。
        /// </summary>
        /// <param name="kLineList"></param>
        private void InternalSaveKLineData(List<USeKLine> kLineList)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                {
                    connection.Open();

                    foreach (USeKLine kLine in kLineList)
                    {
                        if (kLine.Cycle == USeCycleType.Day)
                        {
                            //日线先进行更新，如果返回影响条数为0则插入
                            string cmdText = CreateKLineUpdateSql(kLine);

                            MySqlCommand command = new MySqlCommand(cmdText, connection);
                            command.Parameters.AddWithValue("@contract", kLine.InstrumentCode);
                            command.Parameters.AddWithValue("@exchange", kLine.Market.ToString());
                            command.Parameters.AddWithValue("@date_time", kLine.DateTime);
                            command.Parameters.AddWithValue("@price_open", kLine.Open);
                            command.Parameters.AddWithValue("@price_high", kLine.High);
                            command.Parameters.AddWithValue("@price_low", kLine.Low);
                            command.Parameters.AddWithValue("@price_close", kLine.Close);
                            command.Parameters.AddWithValue("@volumn", kLine.Volumn);
                            command.Parameters.AddWithValue("@turnover", kLine.Turnover);
                            command.Parameters.AddWithValue("@openinterest", kLine.OpenInterest);
                            command.Parameters.AddWithValue("@pre_settlement_price", kLine.PreSettlementPrice);
                            command.Parameters.AddWithValue("@settlement_price", kLine.SettlementPrice);
                            int result = command.ExecuteNonQuery();

                            if (result > 0)
                            {
                                continue;  // 已更新则返回
                            }
                        }

                        {
                            string cmdText = CreateKLineInsertSql(kLine);

                            MySqlCommand command = new MySqlCommand(cmdText, connection);
                            command.Parameters.AddWithValue("@contract", kLine.InstrumentCode);
                            command.Parameters.AddWithValue("@exchange", kLine.Market.ToString());
                            command.Parameters.AddWithValue("@date_time", kLine.DateTime);
                            command.Parameters.AddWithValue("@price_open", kLine.Open);
                            command.Parameters.AddWithValue("@price_high", kLine.High);
                            command.Parameters.AddWithValue("@price_low", kLine.Low);
                            command.Parameters.AddWithValue("@price_close", kLine.Close);
                            command.Parameters.AddWithValue("@volumn", kLine.Volumn);
                            command.Parameters.AddWithValue("@turnover", kLine.Turnover);
                            command.Parameters.AddWithValue("@openinterest", kLine.OpenInterest);

                            if (kLine.Cycle == USeCycleType.Day)
                            {
                                command.Parameters.AddWithValue("@pre_settlement_price", kLine.PreSettlementPrice);
                                command.Parameters.AddWithValue("@settlement_price", kLine.SettlementPrice);
                            }
                            int result = command.ExecuteNonQuery();
                            Debug.Assert(result == 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string text = string.Format("{0}保存K线数据失败,{1}", this, ex.Message);
                m_eventLogger.WriteError(text);
            }
        }

        /// <summary>
        /// 创建Insert SQL语句。
        /// </summary>
        /// <param name="kLine"></param>
        /// <returns></returns>
        private string CreateKLineInsertSql(USeKLine kLine)
        {
            string tableName = GetDBTableName(kLine);
            string strSql = string.Empty;
            if (kLine.Cycle == USeCycleType.Day)
            {
                strSql = string.Format(@"INSERT INTO {0}(contract,exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest,pre_settlement_price,settlement_price) 
 values (@contract,@exchange,@date_time,@price_open,@price_high,@price_low,@price_close,@volumn,@turnover,@openinterest,@pre_settlement_price,@settlement_price)", tableName);
            }
            else
            {
                strSql = string.Format(@"INSERT INTO {0}(contract,exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest) 
 values (@contract,@exchange,@date_time,@price_open,@price_high,@price_low,@price_close,@volumn,@turnover,@openinterest)", tableName);
            }
            return strSql;
        }

        /// <summary>
        /// 创建Update SQL语句。
        /// </summary>
        /// <param name="kLine"></param>
        /// <returns></returns>
        private string CreateKLineUpdateSql(USeKLine kLine)
        {
            string tableName = GetDBTableName(kLine);
            string strSql = string.Empty;
            if (kLine.Cycle == USeCycleType.Day)
            {
                strSql = string.Format(@"update {0}.{1} set price_open = @price_open,price_high=@price_high,price_low=@price_low,price_close=@price_close,volumn=@volumn,turnover=@turnover,openinterest=@openinterest, 
pre_settlement_price=@pre_settlement_price,settlement_price = @settlement_price
where contract=@contract and exchange=@exchange and date_time= @date_time", m_alphaDBName, tableName);
            }
            else
            {
                strSql = string.Format(@"update {0}.{1} set price_open = @price_open,price_high=@price_high,price_low=@price_low,price_close=@price_close,volumn=@volumn,turnover=@turnover,openinterest=@openinterest
where contract=@contract and exchange=@exchange and date_time= @date_time", m_alphaDBName, tableName);
            }
            return strSql;
        }

        /// <summary>
        /// 获取数据表名。
        /// </summary>
        /// <param name="kLine"></param>
        /// <returns></returns>
        private string GetDBTableName(USeKLine kLine)
        {
            string tableName = string.Empty;
            if (kLine.Cycle == USeCycleType.Day)
            {
                tableName = "day_kline";
            }
            else if (kLine.Cycle == USeCycleType.Min1)

            {
                Debug.Assert(kLine.Cycle == USeCycleType.Min1);
                switch (kLine.Market)
                {
                    case USeMarket.CFFEX:
                    case USeMarket.CZCE:
                    case USeMarket.DCE:
                    case USeMarket.SHFE:
                        tableName = string.Format("min1_kline_{0}", kLine.Market.ToString().ToLower());
                        break;
                    default:
                        Debug.Assert(false);
                        throw new Exception("Invalid market:" + kLine.Market.ToString());
                }
            }
            else
            {
                throw new Exception("Invalid cycel:" + kLine.Cycle.ToString());
            }
            return tableName;
        }
    }
}
