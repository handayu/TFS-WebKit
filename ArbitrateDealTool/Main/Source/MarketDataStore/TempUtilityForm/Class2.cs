using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using System.IO;

namespace TempUtilityForm
{

    /// <summary>
    /// PS解析的文本的格式为：
    /// I 20170713 171017375 pb1804,17:10:17.374,15:28:50,0,17510.00,52,17465.00,0.00,0.00,0.00,17510.00,52,20170713,0,0,
    /// </summary>
    public class MaketDataPlayer
    {
        private int m_readIndex = -1;
        private List<USeMarketData> m_marketDataList = null;

        public void Initialize(string filePath)
        {
            m_marketDataList = GetUseMarketData(filePath);
        }


        private List<USeMarketData> GetUseMarketData(string filePath)
        {
            List<USeMarketData> marketDataList = new List<USeMarketData>();

            ////获取文件夹
            //DirectoryInfo fileDicPath = new DirectoryInfo(System.IO.Path.GetDirectoryName(filePath));
            //if (fileDicPath.Exists == false)
            //{
            //    return marketDataList;
            //}

            ////获取文件
            //string fileName = Path.GetFileName(filePath);
            //FileInfo[] fileMarketData = fileDicPath.GetFiles(fileName);
            //if (fileMarketData == null || fileMarketData.Length <= 0)
            //{
            //    return marketDataList;
            //}

            //读取txt字符串并合成UseMarketData，添加到List
            string[] marketDataLines = File.ReadAllLines(filePath);
            int index = 0;
            while (index < marketDataLines.Count())
            {
                USeMarketData marketData = new USeMarketData();

                //string line = marketDataLines[index].Substring(21);
                string[] marketDataArray = marketDataLines[index].Split(',');   //截取","分割
                string[] marketDataIns = marketDataArray[0].Split(' ');//从第一个,分割中截取品种

                /// PS解析的文本的格式为[不包含结算价和前结算价]：
                /// I 20170713 171017375 pb1804,17:10:17.374,15:28:50,0,17510.00,52,17465.00,0.00,0.00,0.00,17510.00,52,20170713,0,0,
                /// 
                /// PS解析的文本的格式为[包含结算价和前结算价]：
                /// I 20170713 171017375 pb1804,17:10:17.374,15:28:50,0,17510.00,52,17465.00,0.00,0.00,0.00,17510.00,52,20170713,0,0,17500，17500，
                //不包含结算价，前结算共15个字段

                    marketData.Instrument = new USeInstrument(marketDataIns[3], null, USeMarket.Unknown);  //解析合约
                    marketData.UpdateTime = Convert.ToDateTime(marketDataArray[2]);                        //解析更新时间
                    marketData.LastPrice = Convert.ToDecimal(marketDataArray[4]);                          //解析最新价
                    marketData.OpenInterest = Convert.ToDecimal(marketDataArray[5]);                       //解析持仓
                    marketData.ClosePrice = Convert.ToDecimal(marketDataArray[6]);                         //解析收盘价
                    marketData.HighPrice = Convert.ToDecimal(marketDataArray[7]);                          //解析最高价
                    marketData.LowPrice = Convert.ToDecimal(marketDataArray[8]);                           //解析最低价
                    marketData.OpenPrice = Convert.ToDecimal(marketDataArray[9]);                          //解析开盘价
                    marketData.PreClosePrice = Convert.ToDecimal(marketDataArray[10]);                     //解析前收盘价
                    marketData.PreOpenInterest = Convert.ToDecimal(marketDataArray[11]);                   //解析前收盘价
                    marketData.Turnover = Convert.ToDecimal(marketDataArray[13]);                          //解析成交金额
                    marketData.Volume = Convert.ToInt32(marketDataArray[14]);                              //解析成交量

                if(marketDataArray.Count() == 17)
                {
                    marketData.SettlementPrice = Convert.ToDecimal(marketDataArray[15]);                          //解析结算价
                    marketData.PreSettlementPrice = Convert.ToInt32(marketDataArray[16]);                              //解析前结算价
                }

                marketDataList.Add(marketData);

                index++;

            }

            return marketDataList;
        }





        public int ReadIndex
        {
            get { return m_readIndex; }
        }

        public void Reset()
        {
            m_readIndex = -1;
        }

        public USeMarketData GetNext()
        {
            m_readIndex++;
            if (m_readIndex < m_marketDataList.Count())
            {
                return m_marketDataList[m_readIndex];
            }
            return null;

        }


        public USeMarketData GetNextByInstrument(string instrumentCode)
        {

            m_readIndex++;
            while (m_readIndex < m_marketDataList.Count())
            {
                if (m_marketDataList[m_readIndex].Instrument.InstrumentCode == instrumentCode)
                {
                    return m_marketDataList[m_readIndex];
                }
                m_readIndex++;
            }

            return null;
        }


        public USeMarketData GetNextByVarieties(string varieties)
        {
            m_readIndex++;
            while (m_readIndex < m_marketDataList.Count())
            {
                if (GetVarietiesName(m_marketDataList[m_readIndex].Instrument.InstrumentCode) == varieties)
                {
                    return m_marketDataList[m_readIndex];
                }
                m_readIndex++;
            }

            return null;
        }

        private string GetVarietiesName(string instrumentName)
        {
            string varieties = "";
            for (int i = 0; i < instrumentName.Length; i++)
            {
                if (char.IsDigit(instrumentName[i]) == false)
                {
                    varieties += instrumentName[i];
                }
            }
            return varieties;
        }

    }
}
