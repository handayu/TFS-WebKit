using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;
using USe.Common.AppLogger;
using USe.Common;
using USe.TradeDriver.Common;
using USe.Common.Manager;
using USe.Common.DBDriver;
using System.Timers;

namespace OuterMarketDataStore
{
    /// <summary>
    /// 行情存储器(MySql数据库)。
    /// </summary>
    public class MySqlKLineStoreage : IMarketDataListener, IUSeNotifier
    {
        #region member
        private string m_dbConnStr = string.Empty;
        private string m_marketDataDBName = string.Empty;
        private Dictionary<string, USeKLine> m_insDayKLineDic = null;
        protected ConcurrentQueue<USeMarketData> m_marketDataQueue = null;

        public event EventHandler<USeNotifyEventArgs> Notify;

        private string m_storeageName = string.Empty;
        private System.Windows.Forms.Timer m_timerOutMarketData = null;

        private int m_sotreCount = 0;
        private int m_errorStoreCount = 0;

        private DateTime m_tradingDayflag = DateTime.Now;
        #endregion

        /// <summary>
        /// 存储器名称
        /// </summary>
        public string StoreageName
        {
            get { return m_storeageName; }
        }

        /// <summary>
        /// 存储失败数。
        /// </summary>
        public int ErrorStoreCount
        {
            get { return m_errorStoreCount; }
        }

        /// <summary>
        /// 已更新的数据量
        /// </summary>
        public int StoreCount
        {
            get { return m_sotreCount; }
        }

        /// <summary>
        /// 未更新的K线数量
        /// </summary>
        public int UnStoreCount
        {
            get { return m_marketDataQueue.Count; }
        }

        public void Stop()
        {
            m_timerOutMarketData.Enabled = false;
            m_timerOutMarketData.Stop();
        }

        #region construction
        public MySqlKLineStoreage(string storageName, string dbConnStr, string marketDataDBName)
        {
            if (string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentNullException("dbConnStr");
            }
            if (string.IsNullOrEmpty(marketDataDBName))
            {
                throw new ArgumentNullException("marketDataDBName");
            }

            m_storeageName = storageName;

            try
            {
                m_dbConnStr = dbConnStr;
                m_marketDataDBName = marketDataDBName;

                m_insDayKLineDic = new Dictionary<string, USeKLine>();
            }
            catch (Exception ex)
            {
                throw new Exception("MySqlKLineStoreage Created error:" + ex.Message);
            }

            //打开交易进程h之后，确定打开的交易日的23：00：00之后的标志交易日
            m_tradingDayflag = new DateTime(DateTime.Now.Year,DateTime.Now.Month, DateTime.Now.Day, 23, 0, 0);

            m_marketDataQueue = new ConcurrentQueue<USeMarketData>();

            //启动定时器刷新合成K线
            m_timerOutMarketData = new System.Windows.Forms.Timer();
            m_timerOutMarketData.Interval = 1000; //每一分钟发布一次
            m_timerOutMarketData.Tick += M_timerOutMarketData_Tick;
            m_timerOutMarketData.Enabled = true;
            m_timerOutMarketData.Start();
        }

        /// <summary>
        /// 定时器合成K线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_timerOutMarketData_Tick(object sender, EventArgs e)
        {
            if (m_marketDataQueue.Count <= 0) return;
            USeMarketData marketData;
            m_marketDataQueue.TryDequeue(out marketData);
            Debug.Assert(marketData != null);

            //去字典中找是否有一条K线记录，如果没有创建一条新的发布存储，如果找到了，开始update
            USeKLine kline = new USeKLine();
            kline.InstrumentCode = marketData.Instrument.InstrumentCode;
            kline.Market = USeMarket.LME;

            //如果K线的时间现在在夜盘时间，而且小于第二天的凌晨时间，那么交易日应该归为前一交易日的行情
            Debug.Assert(kline.DateTime != null);
            if (kline.DateTime >= m_tradingDayflag)
            {
                kline.DateTime = new DateTime(m_tradingDayflag.Year, m_tradingDayflag.Month, m_tradingDayflag.Day, 0, 0, 0);
            }
            else
            {
                kline.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            }

            kline.Open = marketData.OpenPrice;
            kline.High = marketData.HighPrice;
            kline.Low = marketData.LowPrice;
            kline.Close = marketData.ClosePrice;
            kline.Volumn = marketData.Volume;
            kline.Turnover = marketData.Turnover;
            kline.OpenInterest = marketData.OpenInterest;
            kline.PreSettlementPrice = marketData.PreSettlementPrice;
            kline.SettlementPrice = marketData.SettlementPrice;
            kline.AskVolumn = 0;
            kline.BidVolumn = 0;
            kline.SendimentaryMoney = 0m;
            kline.FlowFund = 0m;

            //发布

            List<USeKLine> klineList = new List<USeKLine>();
            klineList.Add(kline);

            InternalSaveKLineData(klineList);

        }

        #endregion

        /// <summary>
        /// K线保存。
        /// </summary>
        /// <param name="kLineList"></param>
        private void InternalSaveKLineData(List<USeKLine> kLineList)
        {
            try
            {

                using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
                {
                    connection.Open();

                    foreach (USeKLine kLine in kLineList)
                    {
                        //if (kLine.Cycle == USeCycleType.Day)
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


                            command.Parameters.AddWithValue("@pre_settlement_price", kLine.PreSettlementPrice);
                            command.Parameters.AddWithValue("@settlement_price", kLine.SettlementPrice);
                            command.Parameters.AddWithValue("@ask_volumn", kLine.AskVolumn);
                            command.Parameters.AddWithValue("@bid_volumn", kLine.BidVolumn);

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
                USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
                //SafeRaiseNotifyEvent(this, notify);
            }
        }

        /// <summary>
        /// 创建Insert SQL语句。
        /// </summary>
        /// <param name="kLine"></param>
        /// <returns></returns>
        private string CreateKLineInsertSql(USeKLine kLine)
        {
            string tableName = "day_kline";

            string strSql = string.Format(@"INSERT INTO {0}(contract,exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest,pre_settlement_price,settlement_price,ask_volumn,bid_volumn,update_time,sendimentary_money,flow_fund) 
 values (@contract,@exchange,@date_time,@price_open,@price_high,@price_low,@price_close,@volumn,@turnover,@openinterest,@pre_settlement_price,@settlement_price,@ask_volumn,@bid_volumn,now(),@sendimentary_money,@flow_fund)", tableName);

            return strSql;
        }

        /// <summary>
        /// 创建Update SQL语句。
        /// </summary>
        /// <param name="kLine"></param>
        /// <returns></returns>
        private string CreateKLineUpdateSql(USeKLine kLine)
        {
            string tableName = "day_kline";

            string strSql = string.Format(@"update {0}.{1} set price_open = @price_open,price_high=@price_high,price_low=@price_low,price_close=@price_close,volumn=@volumn,turnover=@turnover,openinterest=@openinterest, 
pre_settlement_price=@pre_settlement_price,settlement_price = @settlement_price,ask_volumn = @ask_volumn,bid_volumn = @bid_volumn,update_time = now(),sendimentary_money=@sendimentary_money ,flow_fund=@flow_fund
where contract=@contract and exchange=@exchange and date_time= @date_time", m_marketDataDBName, tableName);


            return strSql;
        }

        public override string ToString()
        {
            return this.m_storeageName;
        }

        public void ReceiveMarketData(USeMarketData marketData)
        {
            //首先获取到存到Tick的队列数据，为每一个合约Insert创建第一根当天交易日的日线数据，然后刷新;
            m_marketDataQueue.Enqueue(marketData);
        }
    }
}