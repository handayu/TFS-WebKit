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

namespace TradeRangeManager
{
    /// <summary>
    /// 行情存储器(MySql数据库)。
    /// </summary>
    public class DataStoreForMySql : IDataStore
    {
        #region member
        private int m_sotreCount = 0;
        private bool m_hasError = false;
        private string m_dbConnStr = string.Empty;



        #endregion

        #region construction
        public DataStoreForMySql(string dbConnStr)
        {
            if (string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentNullException("dbConnStr");
            }

            m_dbConnStr = dbConnStr;
        }
        #endregion

        #region property
        /// <summary>
        /// 是否工作。
        /// </summary>
        public bool IsBusy
        {
            get { return false; }
        }

        public bool HasError
        {
            get { return m_hasError; }
        }

        /// <summary>
        /// 已存储数量。
        /// </summary>
        public int StoreCount
        {
            get { return m_sotreCount; }
        }

        /// <summary>
        /// 未存储数量。
        /// </summary>
        public int UnStoreCount
        {
            get { return 0; }
        }
        #endregion

        #region 
        /// <summary>
        /// 启动。
        /// </summary>
        public void Start()
        {

        }

        /// <summary>
        /// 停止。
        /// </summary>
        public void Stop()
        {

        }

        /// <summary>
        /// 重置。
        /// </summary>
        public void Reset()
        {
            m_sotreCount = 0;
        }
        #endregion



        /// <summary>
        /// 保存。
        /// </summary>
        /// <param name=""></param>
        public void InternalSaveKLineData(List<ProductTradeRangeInfo> rangeInfolist)
        {
            Debug.Assert(rangeInfolist != null);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
                {
                    connection.Open();

                    foreach (ProductTradeRangeInfo rangeInfo in rangeInfolist)
                    {

                        foreach (TradeRangeTimeSectionInfo sectionInfo in rangeInfo.TradeRangeTimeSectionsInfo)
                        {
                            string cmdText = CreateInsertSql("future_trade_range");

                            MySqlCommand command = new MySqlCommand(cmdText, connection);
                            command.Parameters.AddWithValue("@varieties", rangeInfo.Name);
                            command.Parameters.AddWithValue("@exchange", rangeInfo.Exchange);
                            command.Parameters.AddWithValue("@is_night", sectionInfo.IsNight);
                            command.Parameters.AddWithValue("@begin_time", sectionInfo.BeginTime);
                            command.Parameters.AddWithValue("@end_time", sectionInfo.EndTime);

                            int result = command.ExecuteNonQuery();
                            Debug.Assert(result == 1);
                            Interlocked.Increment(ref m_sotreCount);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Insert Failed :" + ex.Message);
            }
        }

        /// <summary>
        /// 保存。
        /// </summary>
        /// <param name=""></param>
        public void InternalInstrumentData(List<ProductInstrumentInfo> insInfolist)
        {
            Debug.Assert(insInfolist != null);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(m_dbConnStr))
                {
                    connection.Open();

                    foreach (ProductInstrumentInfo Info in insInfolist)
                    {

                        string cmdText = CreateInsertSqlIns("varieties");

                        MySqlCommand command = new MySqlCommand(cmdText, connection);
                        command.Parameters.AddWithValue("@code", Info.Name);
                        command.Parameters.AddWithValue("@exchange", Info.Exchange);
                        command.Parameters.AddWithValue("@short_name", Info.ShortName);
                        command.Parameters.AddWithValue("@long_name", Info.LongName);

                        int result = command.ExecuteNonQuery();
                        Debug.Assert(result == 1);
                        Interlocked.Increment(ref m_sotreCount);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Insert Failed :" + ex.Message);
            }
        }
        /// <summary>
        /// 创建Insert SQL语句。
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private string CreateInsertSql(string tableName)
        {
            string strSql = string.Format(@"INSERT INTO {0}(varieties,exchange,is_night,begin_time,end_time) 
 values (@varieties,@exchange,@is_night,@begin_time,@end_time)", tableName);
            return strSql;
        }

        /// <summary>
        /// 创建Insert SQL语句。
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private string CreateInsertSqlIns(string tableName)
        {
            string strSql = string.Format(@"INSERT INTO {0}(code,exchange,short_name,long_name) 
 values (@code,@exchange,@short_name,@long_name)", tableName);
            return strSql;
        }

        /// <summary>
        /// 创建Update SQL语句。
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private string CreateUpdateSql(string tableName)
        {
            string strSql = string.Format(@"update {0} set varieties = @varieties,exchange=@exchange,is_night=@is_night,begin_time=@begin_time,end_time=@end_time) 
where varieties=@varieties and exchange=@exchange and is_night= @is_night and begin_time= @begin_time and end_time= @end_time", tableName);
            return strSql;
        }

    }
}
