using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data;
using DataStoreCommon;
using System.Data;
using System.Threading;
using System.Diagnostics;

using System.Collections.Concurrent;
using USe.Common.AppLogger;
using USe.Common;
using USe.TradeDriver.Common;
using USe.Common.Manager;

namespace DataStoreCommon
{
    /// <summary>
    /// 行情存储器(MySql数据库)。
    /// </summary>
    public class MySqlKLineStoreage : KLineStoreage
    {
        #region member
        private string m_dbConnStr = string.Empty;
        private string m_marketDataDBName = string.Empty;
        private List<USeInstrumentDetail> m_insDetailList = null;
        private Dictionary<string, USeKLine> m_insKLineDic = null;
        #endregion

        #region construction
        public MySqlKLineStoreage(string storageName, string dbConnStr, string marketDataDBName)
            : base(storageName)
        {
            if (string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentNullException("dbConnStr");
            }
            if (string.IsNullOrEmpty(marketDataDBName))
            {
                throw new ArgumentNullException("marketDataDBName");
            }

            m_dbConnStr = dbConnStr;
            m_marketDataDBName = marketDataDBName;

            m_insKLineDic = new Dictionary<string, USeKLine>();
            m_insDetailList = GetAllInstrumentDetail();
        }
        #endregion

        #region 
        protected override bool FilterKLineData(USeKLine kLine)
        {
            return true;
        }

        #endregion

        #region 工作线程
        /// <summary>
        /// 读数据线程
        /// </summary>
        protected override void DoWork()
        {
            try
            {
                while (m_runFlag)
                {
                    List<USeKLine> kLineList = new List<USeKLine>();

                    while (m_kLineQueue.Count > 0)
                    {
                        USeKLine kLine = null;
                        m_kLineQueue.TryDequeue(out kLine);
                        Debug.Assert(kLine != null);
                        kLineList.Add(kLine);
                        if (kLineList.Count >= 10)
                        {
                            //大于10条先存储一次
                            break;
                        }
                    }

                    if (kLineList.Count > 0)
                    {
                        InternalSaveKLineData(kLineList);
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// K线保存。
        /// </summary>
        /// <param name="kLineList"></param>
        private void InternalSaveKLineData(List<USeKLine> kLineList)
        {
            //[hanyu]新增字段资金沉淀，资金流入流出在存入数据库前加工

            //资金沉淀 = 合约持仓量*结算价(盘中为最新价)*每手的数量*保证金比例
            //资金流入流出 = 合约持仓变动*结算价(盘中为平均价) * 每手数量*保证金比例
            foreach (USeKLine kline in kLineList)
            {
                int perSharesContract = GetInstrumentPerSharesContract(kline.InstrumentCode);
                decimal exchangeLongMarginRatio = GetExchangeLongMarginRatio(kline.InstrumentCode);
                decimal diffOpenInterest = 0m;

                kline.SendimentaryMoney = kline.OpenInterest * kline.Close * perSharesContract * exchangeLongMarginRatio;
                
                foreach(KeyValuePair<string,USeKLine> kv in m_insKLineDic)
                {
                    if(kv.Key == kline.InstrumentCode)
                    {
                        diffOpenInterest = kline.OpenInterest - kv.Value.OpenInterest;
                        break;
                    }
                }

                kline.FlowFund = diffOpenInterest * Convert.ToDecimal(kline.AvgPrice) * perSharesContract * exchangeLongMarginRatio;
                m_insKLineDic[kline.InstrumentCode] = kline;
            }

            try
            {

                using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
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
                            command.Parameters.AddWithValue("@ask_volumn", kLine.AskVolumn);
                            command.Parameters.AddWithValue("@bid_volumn", kLine.BidVolumn);
                            command.Parameters.AddWithValue("@sendimentary_money", kLine.SendimentaryMoney);
                            command.Parameters.AddWithValue("@flow_fund", kLine.FlowFund);

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
                            command.Parameters.AddWithValue("@sendimentary_money", kLine.SendimentaryMoney);
                            command.Parameters.AddWithValue("@flow_fund", kLine.FlowFund);

                            if (kLine.Cycle == USeCycleType.Day)
                            {
                                command.Parameters.AddWithValue("@pre_settlement_price", kLine.PreSettlementPrice);
                                command.Parameters.AddWithValue("@settlement_price", kLine.SettlementPrice);
                                command.Parameters.AddWithValue("@ask_volumn", kLine.AskVolumn);
                                command.Parameters.AddWithValue("@bid_volumn", kLine.BidVolumn);
                                command.Parameters.AddWithValue("@sendimentary_money", kLine.SendimentaryMoney);
                                command.Parameters.AddWithValue("@flow_fund", kLine.FlowFund);
                            }
                            int result = command.ExecuteNonQuery();
                            Debug.Assert(result == 1);
                        }
                    }
                }

                m_sotreCount += kLineList.Count;
            }
            catch (Exception ex)
            {
                m_errorStoreCount += kLineList.Count;
                string text = string.Format("{0}保存K线数据失败,{1}", this, ex.Message);
                m_eventLogger.WriteError(text);
                USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
                SafeRaiseNotifyEvent(this, notify);
            }
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
                strSql = string.Format(@"INSERT INTO {0}(contract,exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest,pre_settlement_price,settlement_price,ask_volumn,bid_volumn,update_time) 
 values (@contract,@exchange,@date_time,@price_open,@price_high,@price_low,@price_close,@volumn,@turnover,@openinterest,@pre_settlement_price,@settlement_price,@ask_volumn,@bid_volumn,now(),@sendimentary_money,@flow_fund)", tableName);
            }
            else
            {
                strSql = string.Format(@"INSERT INTO {0}(contract,exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest) 
 values (@contract,@exchange,@date_time,@price_open,@price_high,@price_low,@price_close,@volumn,@turnover,@openinterest,@sendimentary_money,@flow_fund)", tableName);
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
pre_settlement_price=@pre_settlement_price,settlement_price = @settlement_price,ask_volumn = @ask_volumn,bid_volumn = @bid_volumn,update_time = now(),@sendimentary_money,@flow_fund
where contract=@contract and exchange=@exchange and date_time= @date_time", m_marketDataDBName, tableName);
            }
            else
            {
                strSql = string.Format(@"update {0}.{1} set price_open = @price_open,price_high=@price_high,price_low=@price_low,price_close=@price_close,volumn=@volumn,turnover=@turnover,openinterest=@openinterest,@sendimentary_money,@flow_fund
where contract=@contract and exchange=@exchange and date_time= @date_time", m_marketDataDBName, tableName);
            }
            return strSql;
        }
        #endregion

        public override string ToString()
        {
            return this.StorageName;
        }

        /// <summary>
        /// 获取所有合约详细信息
        /// </summary>
        /// <returns></returns>
        private List<USeInstrumentDetail> GetAllInstrumentDetail()
        {
            try
            {
                USeTradingInstrumentManager insManager = new USeTradingInstrumentManager(m_dbConnStr, m_marketDataDBName);
                insManager.Initialize();
                return insManager.GetAllInstrumentDetails();
            }
            catch (Exception ex)
            {
               throw new Exception("获取全部合约详细信息异常:" + ex.Message);
            }

        }

        /// <summary>
        /// 获取合约乘数
        /// </summary>
        /// <param name="instrumentCode"></param>
        /// <returns></returns>
        private int GetInstrumentPerSharesContract(string instrumentCode)
        {
            foreach (USeInstrumentDetail detail in m_insDetailList)
            {
                if (detail.Instrument.InstrumentCode == instrumentCode)
                {
                    return detail.VolumeMultiple;
                }
            }

            return 0;
        }

        private decimal GetExchangeLongMarginRatio(string instrumentCode)
        {
            foreach (USeInstrumentDetail detail in m_insDetailList)
            {
                if (detail.Instrument.InstrumentCode == instrumentCode)
                {
                    return detail.ExchangeLongMarginRatio;
                }
            }

            return 0m;
        }

    }
}
