using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using USe.TradeDriver.Common;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace MarketDataRecover
{
    class ImportServices
    {
        public delegate void ImportProgressEventHandle(int finishCount, int errorCount, int totalCount, string message);

        public event ImportProgressEventHandle OnImportProgress;

        private string m_folderPath = string.Empty;
        private BackgroundWorker m_worker = null;

        private string m_dbConStr = string.Empty;
        private string m_alphaDBName = string.Empty;

        public ImportServices(string dbConnnString, string alphaDbName)
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
                        if (entity.Cycle == USeCycleType.Day)
                        {
                            ImportDayKLine(entity);
                        }
                        else
                        {
                            ImportMin1KLine(entity);
                        }
                        successCount++;
                        string text = "";
                        SafeFireReportProgress(successCount, errorCount, fileList.Count, text);
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        string text = "";
                        SafeFireReportProgress(successCount, errorCount, fileList.Count, text);
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


        public void ImportDayKLine(ImportFileEntity entity)
        {
            USeKLine kLine = ReadDayKLineFile(entity.FilePath);
            if (kLine != null)
            {
                //日线先更新，影响条目为0，则Insert
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                    {
                        connection.Open();
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
                        updateCommand.Parameters.AddWithValue("@pre_settlement_price", kLine.PreSettlementPrice);
                        updateCommand.Parameters.AddWithValue("@settlement_price", kLine.SettlementPrice);
                        updateCommand.Parameters.AddWithValue("@ask_volumn", kLine.AskVolumn);
                        updateCommand.Parameters.AddWithValue("@bid_volumn", kLine.BidVolumn);
                        int updateResult = updateCommand.ExecuteNonQuery();

                        if (updateResult <= 0)
                        {
                            string cmdInsertText = CreateKLineUpdateSql(kLine);

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
                            insertCommand.Parameters.AddWithValue("@pre_settlement_price", kLine.PreSettlementPrice);
                            insertCommand.Parameters.AddWithValue("@settlement_price", kLine.SettlementPrice);
                            insertCommand.Parameters.AddWithValue("@ask_volumn", kLine.AskVolumn);
                            insertCommand.Parameters.AddWithValue("@bid_volumn", kLine.BidVolumn);
                            int insertResult = insertCommand.ExecuteNonQuery();
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("ImportDayKLine异常:" + ex.Message);
                }
            }
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
            strSql = string.Format(@"update {0}.{1} set price_open = @price_open,price_high=@price_high,price_low=@price_low,price_close=@price_close,volumn=@volumn,turnover=@turnover,openinterest=@openinterest, 
pre_settlement_price=@pre_settlement_price,settlement_price = @settlement_price,ask_volumn = @ask_volumn,bid_volumn = @bid_volumn,update_time = now()
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
                strSql = string.Format(@"INSERT INTO {0}.{1}(contract,exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest,pre_settlement_price,settlement_price,ask_volumn,bid_volumn) 
            values ('{2}','{3}','{4:yyyy-MM-dd}',{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15});",
                m_alphaDBName, tableName, kLine.InstrumentCode, kLine.Market.ToString(), kLine.DateTime,
                kLine.Open, kLine.High, kLine.Low, kLine.Close, kLine.Volumn, kLine.Turnover, kLine.OpenInterest,
                kLine.PreSettlementPrice, kLine.SettlementPrice,kLine.AskVolumn,kLine.BidVolumn);
            }
            else
            {
                strSql = string.Format(@"INSERT INTO {0}.{1}(contract,exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest) 
            values ('{2}','{3}','{4:yyyy-MM-dd HH:mm:ss}',{5},{6},{7},{8},{9},{10},{11});",
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

        private void ImportMin1KLine(ImportFileEntity entity)
        {
            List<USeKLine> klineList = ReadMin1KLineFile(entity.FilePath);
            if (klineList != null)
            {
                DateTime maxTime = klineList[klineList.Count - 1].DateTime;
                DateTime minTime = klineList[0].DateTime;

                //数据库delete这个范围
                foreach (USeKLine kLine in klineList)
                {
                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                        {
                            connection.Open();

                            string cmdDeleteText = CreateKLineDeleteSql(kLine);
                            MySqlCommand deleteCommand = new MySqlCommand(cmdDeleteText, connection);
                            deleteCommand.Parameters.AddWithValue("@contract", kLine.InstrumentCode);
                            deleteCommand.Parameters.AddWithValue("@exchange", kLine.Market.ToString());
                            deleteCommand.Parameters.AddWithValue("@date_time", kLine.DateTime);

                            int deleteResult = deleteCommand.ExecuteNonQuery();
                        }
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("ImportMinKLine删除分钟数据异常:" + ex.Message);
                    }
                }

                //分钟线删除完毕开始Insert
                foreach (USeKLine kLine in klineList)
                {
                    try
                    {
                        using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
                        {
                            connection.Open();

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
                            insertCommand.Parameters.AddWithValue("@pre_settlement_price", kLine.PreSettlementPrice);
                            insertCommand.Parameters.AddWithValue("@settlement_price", kLine.SettlementPrice);
                            insertCommand.Parameters.AddWithValue("@ask_volumn", kLine.AskVolumn);
                            insertCommand.Parameters.AddWithValue("@bid_volumn", kLine.BidVolumn);
                            int insertResult = insertCommand.ExecuteNonQuery();
                            Debug.Assert(insertResult == 1);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ImportMinKLine添加分钟数据异常:" + ex.Message);
                    }
                }

            }
        }


        /// <summary>
        /// 读取日线文件最后一条更新的数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private USeKLine ReadDayKLineFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            if (lines != null && lines.Length > 0)
            {
                string line = lines[lines.Length - 1];
                return ParseKLine(line);
            }
            return null;
        }

        /// <summary>
        /// 读取分钟文件所有的数据List
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private List<USeKLine> ReadMin1KLineFile(string fileName)
        {
            List<USeKLine> list = new List<USeKLine>();
            string[] lines = File.ReadAllLines(fileName);
            if (lines != null && lines.Length > 0)
            {
                foreach (string line in lines)
                {
                    list.Add(ParseKLine(line));
                }
            }
            return list;
        }

        private USeKLine ParseKLine(string line)
        {
            line = line.Substring(19);

            string[] items = line.Split(new char[] { ',' });


            USeKLine kLine = new USeKLine();
            kLine.InstrumentCode = items[0];
            kLine.Market = (USeMarket)(Enum.Parse(typeof(USeMarket), items[1]));
            kLine.Cycle = (USeCycleType)(Enum.Parse(typeof(USeCycleType), items[2]));
            kLine.DateTime = Convert.ToDateTime(items[3]);
            kLine.Open = Convert.ToDecimal(items[4]);
            kLine.High = Convert.ToDecimal(items[5]);
            kLine.Low = Convert.ToDecimal(items[6]);
            kLine.Close = Convert.ToDecimal(items[7]);
            kLine.Volumn = Convert.ToInt32(items[8]);
            kLine.Turnover = Convert.ToDecimal(items[9]);
            kLine.OpenInterest = Convert.ToDecimal(items[10]);

            kLine.SettlementPrice = Convert.ToDecimal(items[11]);
            kLine.PreSettlementPrice = Convert.ToDecimal(items[12]);
            kLine.AskVolumn = Convert.ToInt32(items[13]);
            kLine.BidVolumn = Convert.ToInt32(items[14]);

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

        private void SafeFireReportProgress(int finishCount, int errorCount, int totalCount, string message)
        {
            ImportProgressEventHandle handle = this.OnImportProgress;
            if (handle != null)
            {
                try
                {
                    handle(finishCount, errorCount, totalCount, message);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        private List<ImportFileEntity> GetAllImportFile()
        {
            List<ImportFileEntity> list = new List<ImportFileEntity>();
            DirectoryInfo directory = new DirectoryInfo(m_folderPath);
            FileInfo[] fileList = directory.GetFiles();
            foreach (FileInfo file in fileList)
            {
                string[] fileNameInfo = file.Name.Split(new char[] { '_', '.' });

                ImportFileEntity entity = new ImportFileEntity();
                entity.FilePath = file.FullName;
                entity.Cycle = (USeCycleType)(Enum.Parse(typeof(USeCycleType),fileNameInfo[0]));
                entity.InstrumentCode = fileNameInfo[1];
            }

            return list;
        }

        public class ImportFileEntity
        {
            public USeCycleType Cycle { get; set; }

            public string InstrumentCode { get; set; }

            public string FilePath { get; set; }
        }

    }
}
