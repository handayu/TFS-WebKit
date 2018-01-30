using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using USe.Common.DBDriver;
using USe.TradeDriver.Common;

namespace TempUtilityForm
{
    /// <summary>
    /// 回补历史的9999主力合约数据
    /// </summary>
    public class AnapleroticData
    {
        //    private string m_alphaName = string.Empty;
        //    private string m_connectStr = string.Empty;
        //    private DateTime m_dateTime = DateTime.Now;
        //    public AnapleroticData(string alaphaName,string ConnectStr,DateTime dateTime)
        //    {
        //        m_alphaName = alaphaName;
        //        m_connectStr = ConnectStr;
        //        m_dateTime = dateTime;
        //    }

        //    /// <summary>
        //    /// 开始执行
        //    /// </summary>
        //    public void Start()
        //    {
        //        //交易日所有的主力合约cu1711
        //        List<string> tradeDayMainContractList = GetTradeDayMainContractList(m_dateTime);

        //        //交易日根据主力合约表和日期拿出行情
        //        List<USeMarketData> dayContractsList = GetMainCntractStandardMarketDataList(m_dateTime, tradeDayMainContractList);

        //        //加工成主力9999合约
        //        List<USeMarketData> mainContracts = ProcessMarketDataToMainContractsData(dayContractsList);

        //        //插入
        //        bool iInsertResult = InsertMainContractsIntoStores(mainContracts);

        //    }


        //    /// <summary>
        //    /// 获取指定交易日的主力合约列表
        //    /// </summary>
        //    /// <param name="dateTime"></param>
        //    /// <returns></returns>
        //    private List<string> GetTradeDayMainContractList(DateTime dateTime)
        //    {
        //        List<string> mainContractList = new List<string>();

        //        string strSel = string.Format(@"select * from {0}.main_contract where settlement_date='{2:yyyy-MM-dd}';",
        //            m_alphaName, dateTime);

        //        DataTable table = MySQLDriver.GetTableFromDB(m_connectStr, strSel);

        //        foreach (DataRow row in table.Rows)
        //        {
        //            try
        //            {
        //                string mainContractStr = row["main_contract"].ToString();
        //                mainContractList.Add(mainContractStr);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception("GetTradeDayMainContractList:" + ex.Message);
        //            }
        //        }

        //        return mainContractList;
        //    }


        //    /// <summary>
        //    /// 获取指定交易日的指定合约的行情列表
        //    /// </summary>
        //    /// <param name="dateTime"></param>
        //    /// <returns></returns>
        //    private List<USeMarketData> GetMainCntractStandardMarketDataList(DateTime dateTime,List<string> mainContractsList)
        //    {
        //        List<USeMarketData> standardMarketDataList = new List<USeMarketData>();

        //        string strSel = string.Format(@"select * from {0}.day_kline where date_time='{2:yyyy-MM-dd}';",
        //            m_alphaName, dateTime);

        //        DataTable table = MySQLDriver.GetTableFromDB(m_connectStr, strSel);

        //        foreach (DataRow row in table.Rows)
        //        {
        //            try
        //            {
        //                foreach(string mainContract in mainContractsList)
        //                {
        //                    if (mainContract != row["contract"].ToString()) continue;

        //                    USeMarketData mainContractMarketData = new USeMarketData();

        //                    USeKLine 






        //                    standardMarketDataList.Add(mainContractMarketData);

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception("GetTradeDayMainContractList:" + ex.Message);
        //            }
        //        }

        //        return standardMarketDataList;
        //    }

        //    /// <summary>
        //    /// 标准合约转化成9999主力合约
        //    /// </summary>
        //    private List<USeMarketData>  ProcessMarketDataToMainContractsData(List<USeMarketData> marketData)
        //    {

        //    }


        //    private bool InsertMainContractsIntoStores(List<USeMarketData> marketData)
        //    {







        //    }




    }
}
