using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using System.IO;
using System.Diagnostics;

namespace HistoryKLineImport
{
    class KLineImporter
    {
        #region member
        private string m_filePath = string.Empty;
        private USeCycleType m_cycle = USeCycleType.Unknown;
        private string m_instrumentCode = string.Empty;
        private USeMarket m_market = USeMarket.Unknown;
        private LogFileInfo m_logFileInfo = null;
        
        #endregion        
                                 
        public KLineImporter(LogFileInfo fileInfo)
        {
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

        public USeCycleType Cycel
        {
            get { return m_cycle; }
        }

        public USeMarket Market
        {
            get { return m_market; }
        }

        public List<USeKLine> ParseKLine(out DateTime beginDate,out DateTime endDate)
        {
            beginDate = DateTime.MaxValue;
            endDate = DateTime.MinValue;

            string[] lines = File.ReadAllLines(m_filePath);

            List<USeKLine> klineList = new List<USeKLine>();

            int totalVolumn = 0;
            decimal totalTurnover = 0m;
            DateTime currTime = DateTime.MinValue;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] items = lines[i].Split(new char[] { ',' });
                if (items.Length != LogProtocol.FIELD_COUNT)
                {
                    string text = string.Format("Unsupport fields @{0} line", i);
                    throw new Exception(text);
                }

                int volumn = Convert.ToInt32(items[LogProtocol.VOLUMN_INDEX]);
                decimal turnover = Convert.ToDecimal(items[LogProtocol.TURNOVER_INDEX]) * 10000;
                DateTime actualTime = GetActualTime(m_cycle, items[LogProtocol.DATE_INDEX], items[LogProtocol.TIME_INDEX]);

                if(m_cycle == USeCycleType.Day)
                {
                    totalVolumn = volumn;
                    totalTurnover = turnover;
                }
                else
                {
                    if(IsSameDay(currTime,actualTime))
                    {
                        totalVolumn += volumn;
                        totalTurnover += turnover;
                    }
                    else
                    {
                        totalVolumn = volumn;
                        totalTurnover = turnover;
                    }
                }
                currTime = actualTime;

                USeKLine kLine = new USeKLine() {
                    InstrumentCode = m_instrumentCode,
                    Market = m_market,
                    Cycle = m_cycle,
                    DateTime = actualTime,
                    Open = Convert.ToDecimal(items[LogProtocol.OPEN_INDEX]),
                    High = Convert.ToDecimal(items[LogProtocol.HIGH_INDEX]),
                    Low = Convert.ToDecimal(items[LogProtocol.LOW_INDEX]),
                    Close = Convert.ToDecimal(items[LogProtocol.CLOSE_INDEX]),
                    PreSettlementPrice = 0m,
                    SettlementPrice = 0m,
                    Volumn = totalVolumn,
                    Turnover = totalTurnover,
                    OpenInterest = Convert.ToDecimal(items[LogProtocol.OPEN_INTEREST_INDEX])
                };
                klineList.Add(kLine);

                if(kLine.DateTime.Date > endDate)
                {
                    endDate = kLine.DateTime.Date;
                }
                if(kLine.DateTime.Date < beginDate)
                {
                    beginDate = kLine.DateTime.Date;
                }
            }
            return klineList;
        }

        private DateTime GetActualTime(USeCycleType cycle, string dateValue,string timeValue)
        {
            string value = dateValue.Replace(".", "-") + " " + timeValue + ":00";
            DateTime time;
            if(DateTime.TryParse(value, out time) == false)
            {
                Debug.Assert(false);
            }
            if (cycle == USeCycleType.Min1)
            {
                time = time.AddHours(-3).AddMinutes(-1);
            }
            return time;
        }

        private bool IsSameDay(DateTime time1,DateTime time2)
        {
            time1 = time1.AddHours(3).AddMinutes(1);
            time2 = time2.AddHours(3).AddMinutes(1);

            return (time1.Date == time2.Date);
        }
    
        private string GetOfficeInstrumentCode(string instrumentCode,USeMarket market)
        {
            if(market == USeMarket.CZCE)
            {
                return instrumentCode.Remove(instrumentCode.Length - 4, 1);
            }
            else
            {
                return instrumentCode;
            }
        }

    }
}
