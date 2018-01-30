using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using USe.TradeDriver.Common;

namespace HistoryKLineImport
{
    class ClosePrice2Importer
    {
        #region member
        private string m_filePath = string.Empty;
        private USeCycleType m_cycle = USeCycleType.Unknown;
        private string m_instrumentCode = string.Empty;
        private USeMarket m_market = USeMarket.Unknown;
        private LogFileInfo m_logFileInfo = null;
        private TimeSpan m_logNoonTime = new TimeSpan(11 + 3, 32, 00);  //午盘时间 前推三个小时
        #endregion

        public ClosePrice2Importer(LogFileInfo fileInfo)
        {
            Debug.Assert(fileInfo.Cycle == USeCycleType.Min1);
            m_logFileInfo = fileInfo;

            m_filePath = fileInfo.FileInfo.FullName;
            m_cycle = fileInfo.Cycle;
            m_market = fileInfo.Market;
            m_instrumentCode = fileInfo.InstrumentCode;
        }

        public string InstrumentCode
        {
            get { return m_instrumentCode; }
        }

        public USeMarket Market
        {
            get { return m_market; }
        }

        public List<ClosePrice2Entity> ParseClosePrice2()
        {
            string[] lines = File.ReadAllLines(m_filePath);

            List<ClosePrice2Entity> closePriceList = new List<ClosePrice2Entity>();

            decimal closePrice2 = 0m;
            DateTime currTime = DateTime.MinValue;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] items = lines[i].Split(new char[] { ',' });
                if (items.Length != LogProtocol.FIELD_COUNT)
                {
                    string text = string.Format("Unsupport fields @{0} line", i);
                    throw new Exception(text);
                }

                DateTime actualTime = GetLogTime(m_cycle, items[LogProtocol.DATE_INDEX], items[LogProtocol.TIME_INDEX]);
                if (actualTime.TimeOfDay > m_logNoonTime)
                {
                    //午盘后数据忽略
                    continue;
                }

                if (IsSameDay(currTime, actualTime))
                {
                    Debug.Assert(actualTime > currTime);
                    closePrice2 = Convert.ToDecimal(items[LogProtocol.CLOSE_INDEX]);
                }
                else
                {
                    if (closePrice2 > 0)
                    {
                        ClosePrice2Entity entity = new ClosePrice2Entity() {
                            SettlementDate = currTime.Date,
                            ClosePrice2 = closePrice2,
                            InstrumentCode = m_instrumentCode,
                            Exchange = m_market
                        };
                        closePriceList.Add(entity);
                    }
                }

                currTime = actualTime;
                closePrice2 = Convert.ToDecimal(items[LogProtocol.CLOSE_INDEX]);
            }

            if (closePrice2 > 0)
            {
                Debug.Assert(currTime > DateTime.MinValue);
                ClosePrice2Entity entity = new ClosePrice2Entity() {
                    SettlementDate = currTime.Date,
                    ClosePrice2 = closePrice2,
                    InstrumentCode = m_instrumentCode,
                    Exchange = m_market
                };
                closePriceList.Add(entity);
            }

            return closePriceList;
        }

        private DateTime GetLogTime(USeCycleType cycle, string dateValue, string timeValue)
        {
            string value = dateValue.Replace(".", "-") + " " + timeValue + ":00";
            DateTime time;
            if (DateTime.TryParse(value, out time) == false)
            {
                Debug.Assert(false);
            }
            return time;
        }

        private bool IsSameDay(DateTime time1, DateTime time2)
        {
            return (time1.Date == time2.Date);
        }
    }
}
