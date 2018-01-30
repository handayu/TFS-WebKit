using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using USe.Common;
using USe.Common.AppLogger;
using USe.TradeDriver.Common;
using MySql.Data.MySqlClient;
using USe.Common.Manager;

namespace HistoryKLineImport
{
    internal class KLineImportServices : ImportServices
    {
        private string m_kLineFolder = string.Empty;
        private USeCycleType m_cycle = USeCycleType.Unknown;

        public KLineImportServices(USeCycleType cycle,IAppLogger eventLogger)
            :base(eventLogger)
        {
            m_cycle = cycle;
        }

        public override bool Initialize()
        {
            if (base.Initialize() == false)
            {
                return false;
            }

            string klinFolder = ConfigurationManager.AppSettings["KLineFolder"];
            if (string.IsNullOrEmpty(klinFolder))
            {
                string text = "Not found KLineFolder";
                m_eventLogger.WriteError(text);
                USeConsole.WriteLine(text);
                return false;
            }

            m_kLineFolder = klinFolder;

            return true;
        }

        public override bool Run()
        {
            USeProductManager varietiesManager = CreateVarietiesManager();
            List<LogFileInfo> logFileList = GetLogFiles(varietiesManager);
            if (logFileList.Count < 0)
            {
                string text = "无可导入文件";
                m_eventLogger.WriteInformation(text);
                USeConsole.WriteLine(text);
                return true;
            }

            int index = 0;
            foreach (LogFileInfo file in logFileList)
            {
                index++;
                try
                {
                    KLineImporter importer = new KLineImporter(file);
                    if (importer.Cycel != USeCycleType.Min1 &&
                        importer.Cycel != USeCycleType.Day)
                    {
                        continue;
                    }
                    DateTime beginDate;
                    DateTime endDate;
                    List<USeKLine> klineList = importer.ParseKLine(out beginDate, out endDate);
                    string text = string.Format("解析{0}.{1}@{2}数据完成,共计{3}条",
                        importer.InstrumentCode, importer.Market, importer.Cycel, klineList.Count);
                    USeConsole.WriteLine(text);
                    Stopwatch stopWathch = new Stopwatch();
                    stopWathch.Start();
                    SaveKLineData(importer.Market, importer.Cycel, importer.InstrumentCode, beginDate, endDate, klineList);
                    stopWathch.Stop();
                    long useSecondes = stopWathch.ElapsedMilliseconds / 1000;
                    USeConsole.WriteLine(string.Format("导入{0}.{1}@{2}完成,共计{3}条,耗时{4}秒,平均速度{7}/秒,Finish{5}/{6}",
                        importer.InstrumentCode, importer.Market, importer.Cycel, klineList.Count, useSecondes, index, logFileList.Count,
                        useSecondes > 0 ? klineList.Count / useSecondes : 0));
                }
                catch (Exception ex)
                {
                    string text = string.Format("导入数据失败,文件路径:{0},错误:{1}", file.FileInfo.FullName, ex.Message);
                    m_eventLogger.WriteInformation(text);
                    USeConsole.WriteLine(text);
                }
            }

            return true;
        }

        private List<LogFileInfo> GetLogFiles(USeProductManager productManager)
        {
            
            DirectoryInfo klineDir = new DirectoryInfo(m_kLineFolder);
            string searchParten = "";
            if(m_cycle == USeCycleType.Day)
            {
                searchParten = "1day-*.csv";
            }
            else if(m_cycle == USeCycleType.Min1)
            {
                searchParten = "1min-*.csv";
            }
            else
            {
                throw new NotSupportedException(string.Format("未知的周期类型文件:{0}", m_cycle));
            }
            FileInfo[] klineFiles = klineDir.GetFiles(searchParten);

            List<LogFileInfo> logFileList = new List<LogFileInfo>();
            foreach (FileInfo fileItem in klineFiles)
            {
                //if (fileItem.Name.StartsWith("1min-cu1707") || fileItem.Name.StartsWith("1day-cu1707"))
                {
                    LogFileInfo logFileInfo = LogFileInfo.ParseLogFile(fileItem, productManager);
                    logFileList.Add(logFileInfo);
                }
            }

            return logFileList;
        }

        private void SaveKLineData(USeMarket market, USeCycleType cycle, string instrumentCode,
           DateTime beginDate, DateTime endDate, List<USeKLine> klineList)
        {
            using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
            {
                connection.Open();

                string tableName = GetDBTableName(cycle, market);
                string cmdText = string.Format(@"delete from {0}.{1} where contract = '{2}' 
and date_time >= '{3:yyyy-MM-dd}' and date_time < '{4:yyyy-MM-dd}'",
                m_alphaDBName, tableName, instrumentCode, beginDate, endDate.AddDays(1));
                MySqlCommand command = new MySqlCommand(cmdText, connection);
                command.ExecuteNonQuery();

                StringBuilder sb = new StringBuilder();
                foreach (USeKLine kLine in klineList)
                {
                    cmdText = CreateKLineInsertSql2(kLine);
                    sb.Append(cmdText);
                    //command = new MySqlCommand(cmdText, connection);
                    //command.Parameters.AddWithValue("@contract", kLine.InstrumentCode);
                    //command.Parameters.AddWithValue("@exchange", kLine.Market.ToString());
                    //command.Parameters.AddWithValue("@date_time", kLine.DateTime);
                    //command.Parameters.AddWithValue("@price_open", kLine.Open);
                    //command.Parameters.AddWithValue("@price_high", kLine.High);
                    //command.Parameters.AddWithValue("@price_low", kLine.Low);
                    //command.Parameters.AddWithValue("@price_close", kLine.Close);
                    //command.Parameters.AddWithValue("@volumn", kLine.Volumn);
                    //command.Parameters.AddWithValue("@turnover", kLine.Turnover);
                    //command.Parameters.AddWithValue("@openinterest", kLine.OpenInterest);

                    //if (kLine.Cycle == USeCycleType.Day)
                    //{
                    //    command.Parameters.AddWithValue("@pre_settlement_price", kLine.PreSettlementPrice);
                    //    command.Parameters.AddWithValue("@settlement_price", kLine.SettlementPrice);
                    //}
                    if (sb.Length > 6000)
                    {
                        command = new MySqlCommand(sb.ToString(), connection);
                        int result = command.ExecuteNonQuery();
                        sb.Clear();
                    }
                    //int result = command.ExecuteNonQuery();
                }

                if (sb.Length > 0)
                {
                    command = new MySqlCommand(sb.ToString(), connection);
                    int result = command.ExecuteNonQuery();
                    sb.Clear();
                }
            }
        }

        /// <summary>
        /// 创建Insert SQL语句。
        /// </summary>
        /// <param name="kLine"></param>
        /// <returns></returns>
        private string CreateKLineInsertSql(USeKLine kLine)
        {
            string tableName = GetDBTableName(kLine.Cycle, kLine.Market);
            string strSql = string.Empty;
            if (kLine.Cycle == USeCycleType.Day)
            {
                strSql = string.Format(@"INSERT INTO {0}.{1}(contract,exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest,pre_settlement_price,settlement_price) 
 values (@contract,@exchange,@date_time,@price_open,@price_high,@price_low,@price_close,@volumn,@turnover,@openinterest,@pre_settlement_price,@settlement_price)",
                m_alphaDBName, tableName);
            }
            else
            {
                strSql = string.Format(@"INSERT INTO {0}.{1}(contract,exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest) 
 values (@contract,@exchange,@date_time,@price_open,@price_high,@price_low,@price_close,@volumn,@turnover,@openinterest)",
                m_alphaDBName, tableName);
            }
            return strSql;
        }

        /// <summary>
        /// 创建Insert SQL语句。
        /// </summary>
        /// <param name="kLine"></param>
        /// <returns></returns>
        private string CreateKLineInsertSql2(USeKLine kLine)
        {
            string tableName = GetDBTableName(kLine.Cycle, kLine.Market);
            string strSql = string.Empty;
            if (kLine.Cycle == USeCycleType.Day)
            {
                strSql = string.Format(@"INSERT INTO {0}.{1}(contract,exchange,date_time,price_open,price_high,price_low,price_close,volumn,turnover,openinterest,pre_settlement_price,settlement_price) 
 values ('{2}','{3}','{4:yyyy-MM-dd}',{5},{6},{7},{8},{9},{10},{11},{12},{13});",
                m_alphaDBName, tableName, kLine.InstrumentCode, kLine.Market.ToString(), kLine.DateTime,
                kLine.Open, kLine.High, kLine.Low, kLine.Close, kLine.Volumn, kLine.Turnover, kLine.OpenInterest,
                kLine.PreSettlementPrice, kLine.SettlementPrice);
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
        /// 获取数据表名。
        /// </summary>
        /// <param name="kLine"></param>
        /// <returns></returns>
        private string GetDBTableName(USeCycleType cycle, USeMarket market)
        {
            string tableName = string.Empty;
            if (cycle == USeCycleType.Day)
            {
                tableName = "day_kline";
            }
            else if (cycle == USeCycleType.Min1)
            {
                switch (market)
                {
                    case USeMarket.CFFEX:
                    case USeMarket.CZCE:
                    case USeMarket.DCE:
                    case USeMarket.SHFE:
                        tableName = string.Format("min1_kline_{0}", market.ToString().ToLower());
                        break;
                    default:
                        Debug.Assert(false);
                        throw new Exception("Invalid market:" + market.ToString());
                }
            }
            else
            {
                throw new Exception("Invalid cycel:" + cycle.ToString());
            }
            return tableName;
        }
    }
}
