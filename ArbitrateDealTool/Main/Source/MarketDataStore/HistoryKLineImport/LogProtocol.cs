using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using USe.TradeDriver.Common;
using USe.Common.Manager;

namespace HistoryKLineImport
{
    public static class LogProtocol
    {
        public static int FIELD_COUNT = 9;

        public static int DATE_INDEX = 0;
        public static int TIME_INDEX = 1;
        public static int OPEN_INDEX = 2;
        public static int HIGH_INDEX = 3;
        public static int LOW_INDEX = 4;
        public static int CLOSE_INDEX = 5;
        public static int OPEN_INTEREST_INDEX = 6;
        public static int VOLUMN_INDEX = 7;
        public static int TURNOVER_INDEX = 8;
    }

    internal class LogFileInfo
    {
        public USeCycleType Cycle { get; set; }

        public string InstrumentCode { get; set; }

        public USeMarket Market { get; set; }

        public FileInfo FileInfo { get; set; }

        public static LogFileInfo ParseLogFile(FileInfo fileInfo,USeProductManager productManager)
        {
            string[] items = fileInfo.Name.Split(new char[] { '-', '.' });
            if (items.Length != 3 && items[2].ToLower() != "csv")
            {
                throw new Exception("Undistinguish fileName:" + fileInfo.Name);
            }

            USeCycleType cycle = USeCycleType.Unknown;
            string cycleValue = items[0].ToLower();
            string instrumentCode = items[1];
            if (cycleValue == "1min")
            {
                cycle = USeCycleType.Min1;
            }
            else if (cycleValue == "5min")
            {
                cycle = USeCycleType.Min5;
            }
            else if (cycleValue == "1day")
            {
                cycle = USeCycleType.Day;
            }
            else
            {
                throw new Exception("Undistinguish cycel:" + cycleValue);
            }

            string productCode = USeTraderProtocol.GetVarieties(instrumentCode);
            USeProduct product = productManager.GetPruduct(productCode);
            if (product == null)
            {
                throw new Exception("Undistinguish cycel:" + cycleValue);
            }

            LogFileInfo logInfo = new LogFileInfo();
            logInfo.InstrumentCode = GetOfficeInstrumentCode(instrumentCode, product.Market);
            logInfo.Market = product.Market;
            logInfo.Cycle = cycle;
            logInfo.FileInfo = fileInfo;

            return logInfo;
        }

        private static string GetOfficeInstrumentCode(string instrumentCode, USeMarket market)
        {
            if (market == USeMarket.CZCE)
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
