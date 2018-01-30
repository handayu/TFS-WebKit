using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common.AppLogger;
using USe.Common;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using USe.TradeDriver.Common;
using USe.Common.Manager;

namespace HistoryKLineImport
{
    class ClosePrice2ImportServices : ImportServices
    {
        private string m_kLineFolder = string.Empty;
        public ClosePrice2ImportServices(IAppLogger eventLogger)
            :base(eventLogger)
        {

        }

        public override bool Initialize()
        {
            if(base.Initialize() == false)
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

            DirectoryInfo klineDir = new DirectoryInfo(m_kLineFolder);
            FileInfo[] klineFiles = klineDir.GetFiles("1min-*.csv");

            List<LogFileInfo> logFileList = new List<LogFileInfo>();
            foreach (FileInfo fileItem in klineFiles)
            {
                //if (fileItem.Name.StartsWith("1min") || fileItem.Name.StartsWith("1day"))
                //if (fileItem.Name.StartsWith("1min-cu1707"))//  || fileItem.Name.StartsWith("1day"))
                {
                    LogFileInfo logFileInfo = LogFileInfo.ParseLogFile(fileItem, varietiesManager);
                    logFileList.Add(logFileInfo);
                }
            }

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
                    ClosePrice2Importer importer = new ClosePrice2Importer(file);

                    List<ClosePrice2Entity> closePriceList = importer.ParseClosePrice2();
                    string text = string.Format("解析{0}.{1}午盘数据完成,共计{2}条",
                        importer.InstrumentCode, importer.Market, closePriceList.Count);
                    USeConsole.WriteLine(text);
                    Stopwatch stopWathch = new Stopwatch();
                    stopWathch.Start();
                    SaveClosePrice2Data(importer.Market, closePriceList);
                    stopWathch.Stop();
                    long useSecondes = stopWathch.ElapsedMilliseconds / 1000;
                    USeConsole.WriteLine(string.Format("导入{0}.{1}午盘数据完成,共计{2}条,耗时{3}秒,平均速度{4}/秒,Finish{5}/{6}",
                        importer.InstrumentCode, importer.Market, closePriceList.Count, useSecondes,
                        useSecondes > 0 ? closePriceList.Count / useSecondes : 0, index, logFileList.Count));
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

        private void SaveClosePrice2Data(USeMarket market, List<ClosePrice2Entity> closePriceList)
        {
            using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
            {
                connection.Open();


                StringBuilder sb = new StringBuilder();
                foreach (ClosePrice2Entity closePrice in closePriceList)
                {
                    string cmdText = CreateClosePrice2UpdateSql(closePrice);
                    sb.Append(cmdText);

                    if (sb.Length > 6000)
                    {
                        MySqlCommand command = new MySqlCommand(sb.ToString(), connection);
                        int result = command.ExecuteNonQuery();
                        sb.Clear();
                    }
                    //int result = command.ExecuteNonQuery();
                }

                if (sb.Length > 0)
                {
                    MySqlCommand command = new MySqlCommand(sb.ToString(), connection);
                    int result = command.ExecuteNonQuery();
                    sb.Clear();
                }
            }
        }

        /// <summary>
        /// 创建ClosePrice2 Update SQL语句。
        /// </summary>
        /// <param name="kLine"></param>
        /// <returns></returns>
        private string CreateClosePrice2UpdateSql(ClosePrice2Entity closePrice)
        {
            string strSql = string.Format(@"update {0}.day_kline set price_close2 = {1}
where contract = '{2}' and exchange = '{3}' and date_time = '{4:yyyy-MM-dd}';",
                m_alphaDBName, closePrice.ClosePrice2, closePrice.InstrumentCode, closePrice.Exchange.ToString(), closePrice.SettlementDate);
            return strSql;
        }
    }
}
