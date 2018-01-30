using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using USe.Common.AppLogger;

namespace NewNoonMarketDataStore
{
    public class DownLoadService
    {
        private string m_dbConStr = string.Empty;
        private string m_alphaDBName = string.Empty;

        private TimeSpan m_queryNoonBeginTime = new TimeSpan(0, 0, 0);
        private TimeSpan m_queryNoonEndTime = new TimeSpan(0, 0, 0);

        private IAppLogger m_eventLogger = null;
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

                m_dbConStr = ConfigurationManager.ConnectionStrings["MarketDataDB"].ConnectionString;
                m_alphaDBName = ConfigurationManager.AppSettings["AlphaDBName"];

                //配置的查询参数
                QueryNoonTimeSettingsSection settingsSection = config.GetSection("QueryNoonTimeSettings") as QueryNoonTimeSettingsSection;
                m_queryNoonBeginTime = settingsSection.QuerySettings.QueryNoonBeginTime;
                m_queryNoonEndTime = settingsSection.QuerySettings.QueryNoonEndTime;
            }
            catch (Exception ex)
            {
                string error = "Not found the specific configuration file," + ex.Message;
                Console.WriteLine(error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新午盘数据
        /// </summary>
        /// <returns></returns>
        private void UpdateNoonClosePriceToDB()
        {
            using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
            {
                connection.Open();

                string cmdText = string.Empty;
                cmdText = CreateClosePrice2UpdateSql();

                MySqlCommand command = new MySqlCommand(cmdText, connection);

                //command.Parameters.AddWithValue("@contract", instrumentID);

                int result = command.ExecuteNonQuery();
                Debug.Assert(result == 1);
            }
        }

        /// <summary>
        /// 更新日线数据的ClosePrice2
        /// </summary>
        /// <returns></returns>
        private string CreateClosePrice2UpdateSql()
        {
            string cmdText = string.Format(@"update {0}.day_kline set price_close2 = price_close
WHERE date_time = {1}", m_alphaDBName,DateTime.Today);
            return cmdText;
        }


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
            string text = string.Format("本地时间:{0},开始更新午盘数据",DateTime.Now);
            m_eventLogger.WriteInformation(text);


            if (ReadConfig() == false) return -1;

            while(true)
            {
                if(DateTime.Now.TimeOfDay < m_queryNoonBeginTime || DateTime.Now.TimeOfDay > m_queryNoonEndTime)
                {
                    Console.WriteLine(string.Format("当前本地时间:{0}  不在午盘开始查询时间{1} 和午盘查询结束时间{2}之间,继续等待...", 
                        DateTime.Now, m_queryNoonBeginTime, m_queryNoonEndTime));
                    System.Threading.Thread.Sleep(5000);
                    continue;
                }

                try
                {
                    UpdateNoonClosePriceToDB();
                }
                catch(Exception ex)
                {
                    string exInfo = string.Format("本地时间:{0},更新午盘数据发生异常:",ex.Message);
                    m_eventLogger.WriteInformation(exInfo);
                    return -1;
                }

            }

        }
    }
}
