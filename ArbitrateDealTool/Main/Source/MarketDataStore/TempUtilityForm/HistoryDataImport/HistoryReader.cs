using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;

namespace TempUtilityForm
{
    public class HistoryReader
    {

        public delegate void ProcessHandel(string processType, int successCout, int fileList, string path);
        public event ProcessHandel OnProcessDataEvent;


        private string m_folderPath = string.Empty;
        private BackgroundWorker m_worker = null;

        private string m_dbConStr = string.Empty;
        private string m_alphaDBName = string.Empty;

        private List<USeProduct> m_productList = null;

        public HistoryReader(string dbConnnString, string alphaDbName)
        {
            m_dbConStr = dbConnnString;
            m_alphaDBName = alphaDbName;
        }

        public void Start(string folderPath)
        {
            m_folderPath = folderPath;
            if (Directory.Exists(m_folderPath) == false)
            {
                throw new Exception("文件夹路径不存在");
            }

            if (m_worker != null)
            {
                throw new Exception("正在导入，不能重复");
            }

            List<USeProduct> productList = GetAllVarieties();
            if (productList == null)
            {
                throw new Exception("产品列表为空,无法通过产品信息获得交易所信息");
            }


            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += M_importWork_DoWork;
            worker.RunWorkerCompleted += M_importWork_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }


        /// <summary>
        /// 后台任务开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_importWork_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<ImportFileEntity> fileList = GetAllImportFile();
                int successCount = 0;
                int errorCount = 0;
                foreach (ImportFileEntity entity in fileList)
                {
                    try
                    {
                        if (entity.Cycle == USeCycleType.Min1)
                        {
                            ImportMin1KLine(entity);
                        }
                        else
                        {
                            ImportDayKLine(entity);
                        }
                        successCount++;

                        Debug.WriteLine("正在处理的文件数第:{0}个,共{1}个文件，文件名:{2}", successCount, fileList.Count(), entity.FilePath);

                        OnProcessDataEvent("Outer", successCount, fileList.Count(), entity.FilePath);

                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("importWork_DoWork发生异常:" + ex.Message);
            }

        }

        /// <summary>
        /// 后台任务结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_importWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (m_worker != null)
            {
                m_worker.DoWork -= M_importWork_DoWork;
                m_worker.RunWorkerCompleted -= M_importWork_RunWorkerCompleted;
                m_worker = null;
            }
        }

        #region 单条执行事务
        public void ImportDayKLine(ImportFileEntity entity)
        {
            int count = 0;

            List<USeKLine> kLineList = ReadDayKLineFile(entity);
            if (kLineList != null)
            {

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                    {
                        connection.Open();

                        foreach (USeKLine kLine in kLineList)
                        {
                            string cmdUpdateText = CreateKLineUpdateSql(kLine);

                            MySqlCommand updateCommand = new MySqlCommand(cmdUpdateText, connection);
                            updateCommand.Parameters.AddWithValue("@contract", kLine.InstrumentCode);
                            updateCommand.Parameters.AddWithValue("@exchange", kLine.Market.ToString());
                            updateCommand.Parameters.AddWithValue("@date_time", kLine.DateTime);
                            updateCommand.Parameters.AddWithValue("@price_open", kLine.Open);
                            updateCommand.Parameters.AddWithValue("@price_high", kLine.High);
                            updateCommand.Parameters.AddWithValue("@price_low", kLine.Low);
                            updateCommand.Parameters.AddWithValue("@price_close", kLine.Close);
                            updateCommand.Parameters.AddWithValue("@volumn", kLine.Volumn);
                            updateCommand.Parameters.AddWithValue("@turnover", kLine.Turnover);
                            updateCommand.Parameters.AddWithValue("@openinterest", kLine.OpenInterest);

                            int updateResult = updateCommand.ExecuteNonQuery();


                            if (updateResult <= 0)
                            {
                                string cmdInsertText = CreateKLineInsertSql(kLine);

                                MySqlCommand insertCommand = new MySqlCommand(cmdInsertText, connection);
                                insertCommand.Parameters.AddWithValue("@contract", kLine.InstrumentCode);
                                insertCommand.Parameters.AddWithValue("@exchange", kLine.Market.ToString());
                                insertCommand.Parameters.AddWithValue("@date_time", kLine.DateTime);
                                insertCommand.Parameters.AddWithValue("@price_open", kLine.Open);
                                insertCommand.Parameters.AddWithValue("@price_high", kLine.High);
                                insertCommand.Parameters.AddWithValue("@price_low", kLine.Low);
                                insertCommand.Parameters.AddWithValue("@price_close", kLine.Close);
                                insertCommand.Parameters.AddWithValue("@volumn", kLine.Volumn);
                                insertCommand.Parameters.AddWithValue("@turnover", kLine.Turnover);
                                updateCommand.Parameters.AddWithValue("@openinterest", kLine.OpenInterest);

                                int insertResult = insertCommand.ExecuteNonQuery();
                            }

                            count++;
                            Debug.WriteLine("该文件共{1}个条目,目前:{2},正在处理的文件:{0},", kLineList.Count(), count, entity.FilePath);
                            OnProcessDataEvent("Inner", kLineList.Count(), count, entity.FilePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("ImportDayKLine异常:" + ex.Message);
                }
            }

        }
        private void ImportMin1KLine(ImportFileEntity entity)
        {
            int count = 0;
            List<USeKLine> kLineList = ReadMin1KLineFile(entity);
            if (kLineList != null)
            {

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                    {
                        connection.Open();
                        foreach (USeKLine kLine in kLineList)
                        {
                            string cmdUpdateText = CreateKLineUpdateSql(kLine);

                            MySqlCommand updateCommand = new MySqlCommand(cmdUpdateText, connection);
                            updateCommand.Parameters.AddWithValue("@contract", kLine.InstrumentCode);
                            updateCommand.Parameters.AddWithValue("@exchange", kLine.Market.ToString());
                            updateCommand.Parameters.AddWithValue("@date_time", kLine.DateTime);
                            updateCommand.Parameters.AddWithValue("@price_open", kLine.Open);
                            updateCommand.Parameters.AddWithValue("@price_high", kLine.High);
                            updateCommand.Parameters.AddWithValue("@price_low", kLine.Low);
                            updateCommand.Parameters.AddWithValue("@price_close", kLine.Close);
                            updateCommand.Parameters.AddWithValue("@volumn", kLine.Volumn);
                            updateCommand.Parameters.AddWithValue("@turnover", kLine.Turnover);
                            updateCommand.Parameters.AddWithValue("@openinterest", kLine.OpenInterest);

                            int updateResult = updateCommand.ExecuteNonQuery();

                            if (updateResult <= 0)
                            {
                                string cmdInsertText = CreateKLineInsertSql(kLine);

                                MySqlCommand insertCommand = new MySqlCommand(cmdInsertText, connection);
                                insertCommand.Parameters.AddWithValue("@contract", kLine.InstrumentCode);
                                insertCommand.Parameters.AddWithValue("@exchange", kLine.Market.ToString());
                                insertCommand.Parameters.AddWithValue("@date_time", kLine.DateTime);
                                insertCommand.Parameters.AddWithValue("@price_open", kLine.Open);
                                insertCommand.Parameters.AddWithValue("@price_high", kLine.High);
                                insertCommand.Parameters.AddWithValue("@price_low", kLine.Low);
                                insertCommand.Parameters.AddWithValue("@price_close", kLine.Close);
                                insertCommand.Parameters.AddWithValue("@volumn", kLine.Volumn);
                                insertCommand.Parameters.AddWithValue("@turnover", kLine.Turnover);
                                insertCommand.Parameters.AddWithValue("@openinterest", kLine.OpenInterest);

                                int insertResult = insertCommand.ExecuteNonQuery();
                            }


                            count++;
                            Debug.WriteLine("该文件共{0}个条目,目前:{1},正在处理的文件:{2},", kLineList.Count(), count, entity.FilePath);
                            OnProcessDataEvent("Inner", kLineList.Count(), count, entity.FilePath);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("ImportDayKLine异常:" + ex.Message);
                }
            }
        }

        #endregion

        #region 批量执行事务
        //public void ReImportDayKLine(ImportFileEntity entity)
        //{
        //    int count = 0;

        //    List<USeKLine> kLineList = ReadDayKLineFile(entity);
        //    if (kLineList != null)
        //    {

        //        using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
        //        {
        //            connection.Open();

        //            MySqlCommand updateCommand = new MySqlCommand();
        //            updateCommand.Connection = connection;
        //            MySqlTransaction tx = connection.BeginTransaction();
        //            updateCommand.Transaction = tx;

        //            try
        //            {
        //                List<string> SQLStringList = new List<string>();
        //                foreach (USeKLine kline in kLineList)
        //                {
        //                    string cmdUpdateText = CreateKLineUpdateSql(kline);
        //                    SQLStringList.Add(cmdUpdateText);
        //                }

        //                for (int n = 0; n < SQLStringList.Count; n++)
        //                {
        //                    string strsql = SQLStringList[n].ToString();
        //                    if (strsql.Trim().Length > 1)
        //                    {
        //                        updateCommand.CommandText = strsql;

        //                        updateCommand.Parameters.AddWithValue("@contract", kLine.InstrumentCode);
        //                        updateCommand.Parameters.AddWithValue("@exchange", kLine.Market.ToString());
        //                        updateCommand.Parameters.AddWithValue("@date_time", kLine.DateTime);
        //                        updateCommand.Parameters.AddWithValue("@price_open", kLine.Open);
        //                        updateCommand.Parameters.AddWithValue("@price_high", kLine.High);
        //                        updateCommand.Parameters.AddWithValue("@price_low", kLine.Low);
        //                        updateCommand.Parameters.AddWithValue("@price_close", kLine.Close);
        //                        updateCommand.Parameters.AddWithValue("@volumn", kLine.Volumn);
        //                        updateCommand.Parameters.AddWithValue("@turnover", kLine.Turnover);
        //                        updateCommand.Parameters.AddWithValue("@openinterest", kLine.OpenInterest);

        //                        int updateResult = updateCommand.ExecuteNonQuery();

        //                        if (updateResult <= 0)
        //                        {
        //                            string cmdInsertText = CreateKLineInsertSql(kLine);

        //                            MySqlCommand insertCommand = new MySqlCommand(cmdInsertText, connection);
        //                            insertCommand.Parameters.AddWithValue("@contract", kLine.InstrumentCode);
        //                            insertCommand.Parameters.AddWithValue("@exchange", kLine.Market.ToString());
        //                            insertCommand.Parameters.AddWithValue("@date_time", kLine.DateTime);
        //                            insertCommand.Parameters.AddWithValue("@price_open", kLine.Open);
        //                            insertCommand.Parameters.AddWithValue("@price_high", kLine.High);
        //                            insertCommand.Parameters.AddWithValue("@price_low", kLine.Low);
        //                            insertCommand.Parameters.AddWithValue("@price_close", kLine.Close);
        //                            insertCommand.Parameters.AddWithValue("@volumn", kLine.Volumn);
        //                            insertCommand.Parameters.AddWithValue("@turnover", kLine.Turnover);
        //                            updateCommand.Parameters.AddWithValue("@openinterest", kLine.OpenInterest);

        //                            int insertResult = insertCommand.ExecuteNonQuery();
        //                        }
        //                    //后来加上的  
        //                    if (n > 0 && (n % 500 == 0 || n == SQLStringList.Count - 1))
        //                    {
        //                        tx.Commit();
        //                        tx = connection.BeginTransaction();
        //                    }
        //                }
        //            }





        //            }

        //            count++;
        //            Debug.WriteLine("该文件共{1}个条目,目前:{2},正在处理的文件:{0},", kLineList.Count(), count, entity.FilePath);
        //            OnProcessDataEvent("Inner", kLineList.Count(), count, entity.FilePath);

        //        }
        //    }
        //            catch (Exception ex)
        //    {
        //        throw new Exception("ImportDayKLine异常:" + ex.Message);
        //    }

        //}

        //private void ReImportMin1KLine(ImportFileEntity entity)
        //{
        //    int count = 0;
        //    List<USeKLine> kLineList = ReadMin1KLineFile(entity);
        //    if (kLineList != null)
        //    {
        //        foreach (USeKLine kLine in kLineList)
        //        {
        //            try
        //            {
        //                using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
        //                {
        //                    connection.Open();
        //                    string cmdUpdateText = CreateKLineUpdateSql(kLine);

        //                    MySqlCommand updateCommand = new MySqlCommand(cmdUpdateText, connection);
        //                    updateCommand.Parameters.AddWithValue("@contract", kLine.InstrumentCode);
        //                    updateCommand.Parameters.AddWithValue("@exchange", kLine.Market.ToString());
        //                    updateCommand.Parameters.AddWithValue("@date_time", kLine.DateTime);
        //                    updateCommand.Parameters.AddWithValue("@price_open", kLine.Open);
        //                    updateCommand.Parameters.AddWithValue("@price_high", kLine.High);
        //                    updateCommand.Parameters.AddWithValue("@price_low", kLine.Low);
        //                    updateCommand.Parameters.AddWithValue("@price_close", kLine.Close);
        //                    updateCommand.Parameters.AddWithValue("@volumn", kLine.Volumn);
        //                    updateCommand.Parameters.AddWithValue("@turnover", kLine.Turnover);
        //                    updateCommand.Parameters.AddWithValue("@openinterest", kLine.OpenInterest);

        //                    int updateResult = updateCommand.ExecuteNonQuery();

        //                    if (updateResult <= 0)
        //                    {
        //                        string cmdInsertText = CreateKLineInsertSql(kLine);

        //                        MySqlCommand insertCommand = new MySqlCommand(cmdInsertText, connection);
        //                        insertCommand.Parameters.AddWithValue("@contract", kLine.InstrumentCode);
        //                        insertCommand.Parameters.AddWithValue("@exchange", kLine.Market.ToString());
        //                        insertCommand.Parameters.AddWithValue("@date_time", kLine.DateTime);
        //                        insertCommand.Parameters.AddWithValue("@price_open", kLine.Open);
        //                        insertCommand.Parameters.AddWithValue("@price_high", kLine.High);
        //                        insertCommand.Parameters.AddWithValue("@price_low", kLine.Low);
        //                        insertCommand.Parameters.AddWithValue("@price_close", kLine.Close);
        //                        insertCommand.Parameters.AddWithValue("@volumn", kLine.Volumn);
        //                        insertCommand.Parameters.AddWithValue("@turnover", kLine.Turnover);
        //                        insertCommand.Parameters.AddWithValue("@openinterest", kLine.OpenInterest);

        //                        int insertResult = insertCommand.ExecuteNonQuery();
        //                    }


        //                    count++;
        //                    Debug.WriteLine("该文件共{0}个条目,目前:{1},正在处理的文件:{2},", kLineList.Count(), count, entity.FilePath);
        //                    OnProcessDataEvent("Inner", kLineList.Count(), count, entity.FilePath);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception("ImportDayKLine异常:" + ex.Message);
        //            }
        //        }
        //    }
        //}

        #endregion

        /// <summary>
        /// 创建Update SQL语句。
        /// </summary>
        /// <param name="kLine"></param>
        /// <returns></returns>
        private string CreateKLineUpdateSql(USeKLine kLine)
        {
            string tableName = GetDBTableName(kLine);
            string strSql = string.Empty;
            strSql = string.Format(@"update {0}.{1} set price_open = @price_open,price_high=@price_high,price_low=@price_low,price_close=@price_close,volumn=@volumn,turnover=@turnover,openinterest=@openinterest
where contract=@contract and exchange=@exchange and date_time= @date_time", m_alphaDBName, tableName);

            return strSql;
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
                strSql = string.Format(@"INSERT INTO {0}.{1}(contract,exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest) 
            values ('{2}','{3}','{4:yyyy-MM-dd}',{5},{6},{7},{8},{9},{10});",
                m_alphaDBName, tableName, kLine.InstrumentCode, kLine.Market.ToString(), kLine.DateTime,
                kLine.Open, kLine.High, kLine.Low, kLine.Close, kLine.Volumn, kLine.Turnover, kLine.OpenInterest);
            }
            else
            {
                strSql = string.Format(@"INSERT INTO {0}.{1}(contract,exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest) 
            values ('{2}','{3}','{4:yyyy-MM-dd HH:mm:ss}',{5},{6},{7},{8},{9},{10});",
                m_alphaDBName, tableName, kLine.InstrumentCode, kLine.Market.ToString(), kLine.DateTime,
                kLine.Open, kLine.High, kLine.Low, kLine.Close, kLine.Volumn, kLine.Turnover, kLine.OpenInterest);
            }
            return strSql;
        }

        /// <summary>
        /// 创建delete SQL语句。
        /// </summary>
        /// <param name="kLine"></param>
        /// <returns></returns>
        private string CreateKLineDeleteSql(USeKLine kLine)
        {
            string tableName = GetDBTableName(kLine);
            string strSql = string.Empty;
            strSql = string.Format(@"DELETE FROM {0}.{1} where contract=@contract and exchange=@exchange and date_time= @date_time", m_alphaDBName, tableName);
            return strSql;
        }

        /// <summary>
        /// 读取日线文件所有的数据List
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private List<USeKLine> ReadDayKLineFile(ImportFileEntity entity)
        {
            List<USeKLine> list = new List<USeKLine>();
            string[] lines = File.ReadAllLines(entity.FilePath);
            if (lines != null && lines.Length > 0)
            {
                foreach (string line in lines)
                {
                    list.Add(ParseKLine(line, entity));
                }
            }
            return list;
        }

        /// <summary>
        /// 读取分钟文件所有的数据List
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private List<USeKLine> ReadMin1KLineFile(ImportFileEntity entity)
        {
            List<USeKLine> list = new List<USeKLine>();
            string[] lines = File.ReadAllLines(entity.FilePath);
            if (lines != null && lines.Length > 0)
            {
                foreach (string line in lines)
                {
                    list.Add(ParseKLine(line, entity));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取所有品种信息。
        /// </summary>
        /// <returns></returns>
        private List<USeProduct> GetAllVarieties()
        {
            string cmdText = string.Format(@"select* from {0}.varieties where exchange in ({1});",
                m_alphaDBName, GetInternalExchanges());

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmdText, connection);
                adapter.Fill(table);
                adapter.Dispose();
            }

            List<USeProduct> varieties = new List<USeProduct>();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    USeProduct product = new USeProduct()
                    {
                        ProductCode = row["code"].ToString(),
                        Market = (USeMarket)Enum.Parse(typeof(USeMarket), row["exchange"].ToString())
                    };

                    varieties.Add(product);
                }
            }

            return m_productList = varieties;
        }

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
        /// 匹配交易所
        /// </summary>
        /// <param name="instrumentCode"></param>
        /// <returns></returns>
        private USeMarket FromCodeToMarket(string instrumentCode)
        {
            USeMarket market = USeMarket.Unknown;

            Debug.Assert(m_productList != null);

            if (m_productList == null) return USeMarket.Unknown;

            foreach (USeProduct product in m_productList)
            {
                if (product.ProductCode == GetVarietiesName(instrumentCode))
                {
                    return product.Market;
                }
            }

            return market;
        }

        private string GetVarietiesName(string instrumentCode)
        {
            string varieties = "";
            for (int i = 0; i < instrumentCode.Length; i++)
            {
                if (char.IsDigit(instrumentCode[i]) == false)
                {
                    varieties += instrumentCode[i];
                }
            }
            return varieties;
        }

        private USeKLine ParseKLine(string line, ImportFileEntity entity)
        {
            string[] items = line.Split(new char[] { ',' });
            USeKLine kLine = new USeKLine();
            //文件名带过来的
            kLine.InstrumentCode = entity.InstrumentCode;
            //搜索匹配过来的
            kLine.Market = FromCodeToMarket(entity.InstrumentCode);

            //entity根据文件名获取过来的
            kLine.Cycle = entity.Cycle;

            //line[0]和line[1]合成...开高低收
            string dateRegion = items[0];
            string[] dateArray = items[0].Split('.');
            string dateProcess = dateArray[0] + "-" + dateArray[1] + "-" + dateArray[2] + " " + items[1];


            kLine.DateTime = Convert.ToDateTime(dateProcess);

            kLine.Open = Convert.ToDecimal(items[2]);
            kLine.High = Convert.ToDecimal(items[3]);
            kLine.Low = Convert.ToDecimal(items[4]);
            kLine.Close = Convert.ToDecimal(items[5]);
            kLine.OpenInterest = Convert.ToDecimal(items[6]);
            kLine.Volumn = Convert.ToInt32(items[7]);
            kLine.Turnover = Convert.ToDecimal(items[8]) * 10000;
            return kLine;
        }

        /// <summary>
        /// 根据加工后的K线获取数据表名。
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

        private List<ImportFileEntity> GetAllImportFile()
        {
            List<ImportFileEntity> list = new List<ImportFileEntity>();
            DirectoryInfo directory = new DirectoryInfo(m_folderPath);
            FileInfo[] fileList = directory.GetFiles();
            foreach (FileInfo file in fileList)
            {
                string[] fileNameInfo = file.Name.Split(new char[] { '-', '.' });

                ImportFileEntity entity = new ImportFileEntity();
                entity.FilePath = file.FullName;

                if (fileNameInfo[0].Equals("1day"))
                {
                    entity.Cycle = USeCycleType.Day;
                }
                else
                {
                    entity.Cycle = USeCycleType.Min1;
                }

                entity.InstrumentCode = fileNameInfo[1];

                list.Add(entity);

            }

            List<ImportFileEntity> tempList = new List<ImportFileEntity>();
            for (int i = list.Count() - 1; i >= 0; i--)
            {
                tempList.Add(list[i]);
            }

            return tempList;
        }

        public class ImportFileEntity
        {
            public USeCycleType Cycle { get; set; }

            public string InstrumentCode { get; set; }

            public string FilePath { get; set; }
        }

    }
}
