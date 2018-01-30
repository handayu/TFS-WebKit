using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MySql.Data;
using MySql;
using MySql.Data.MySqlClient;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Configuration;

namespace TempUtilityForm
{
    public class ContractsReader
    {
        private string m_dbConStr = string.Empty;
        private string m_alphaDBName = string.Empty;

        private List<Varieties> m_varieties = null;

        /// <summary>
        /// 解析的文本的格式为：
        /// AG1210.SHF,沪银1210,AG1210,2012年5月10日,2012年10月15日,2012年10月22日,201210,6166,,,
        /// </summary>

        private int m_readIndex = -1;
        private List<ContractRegionData> m_contractRegionDataList = null;//原始数据
        private List<ContractResultData> m_contractResultDataList = null;//加工数据

        /// <summary>
        /// 获取Txt原始数据列表
        /// </summary>
        /// <param name="filePath"></param>
        private List<ContractRegionData> ToGetContractRegionData(string filePath)
        {
            List<ContractRegionData> contractRegionDataList = new List<ContractRegionData>();

            //读取txt字符串并合成UseMarketData，添加到List
            string[] contractRegionDataLines = File.ReadAllLines(filePath,Encoding.Default);
            int index = 0;
            while (index < contractRegionDataLines.Count())
            {
                ContractRegionData contractRegionData = new ContractRegionData();

                string[] marketDataArray = contractRegionDataLines[index].Split(',');   //截取","分割

                /// 解析的文本的格式为：
                /// AG1305.SHF,沪银1305,AG1305,2012年5月16日,2013年5月15日,2013年5月22日,201305,5835,,,

                contractRegionData.Contract = marketDataArray[0];                        //解析代码
                contractRegionData.ContractName = marketDataArray[1];                    //解析简称
                contractRegionData.ContractFutureName = marketDataArray[2];              //解析期货代码
                contractRegionData.OpenDate = Convert.ToDateTime(marketDataArray[3]);    //解析开始交易日
                contractRegionData.ExpireDate = Convert.ToDateTime(marketDataArray[4]);  //解析最后交易日
                contractRegionData.EndDelivDate = Convert.ToDateTime(marketDataArray[5]);//解析交割日

                contractRegionDataList.Add(contractRegionData);

                index++;
            }
            return m_contractRegionDataList = contractRegionDataList;
        }

        /// <summary>
        /// 原始Txt数据加工成最终需要存数据库的数据
        /// </summary>
        /// <param name="contractDataList"></param>
        /// <returns></returns>
        private List<ContractResultData> ToRegionDataProcess(List<ContractRegionData> contractDataList)
        {
            if (contractDataList == null) return null;

            List<ContractResultData> resultDataList = new List<ContractResultData>();

            foreach (ContractRegionData data in contractDataList)
            {
                ContractResultData resultData = new ContractResultData();

                //证券代码--切割出交易所result的exchange和contract
                string[] contractSpiltList = data.Contract.Split('.');
                string regionContract = contractSpiltList[0];
                string regionExchange = contractSpiltList[1];

                string varitiesCode = MatchContractCode(regionContract);

                resultData.Exchange = TransferRegionExchangeToStanderd(varitiesCode);
                resultData.Contract = TransferRegionContractToStanderd(varitiesCode,regionContract);
                resultData.ContractName = data.ContractName;
                resultData.OpenDate = data.OpenDate;
                resultData.ExpireDate = data.ExpireDate;
                resultData.StartDelivDate = data.EndDelivDate;
                resultData.EndDelivDate = data.EndDelivDate;
                resultData.VolumeMultiple = MatchVarietieMulpie(varitiesCode);
                resultData.IsTrading = false;
                resultData.varieties = varitiesCode;
                resultData.PriceTick = MatchVaritiesPriceTick(varitiesCode);
                resultData.ProductClass = "Futures";

                resultDataList.Add(resultData);
            }

            return m_contractResultDataList = resultDataList;
        }

        private decimal MatchVaritiesPriceTick(string varitiesCode)
        {
            if (varitiesCode == "") return 0;
            foreach (Varieties v in m_varieties)
            {
                if (v.Code == varitiesCode)
                {
                    return v.PriceTick;
                }
            }
            return int.MaxValue;
        }

        /// <summary>
        /// 根据品种获取标准Mulpie
        /// </summary>
        /// <param name="varitiesCode"></param>
        /// <returns></returns>
        private int MatchVarietieMulpie(string varitiesCode)
        {
            if (varitiesCode == "") return 0;
            foreach (Varieties v in m_varieties)
            {
                if (v.Code == varitiesCode)
                {
                    return v.VolumeMultiple;
                }
            }
            return int.MaxValue;
        }

        /// <summary>
        /// 获取标准品种
        /// </summary>
        /// <param name="regionContract"></param>
        /// <returns></returns>
        private string MatchContractCode(string regionContract)
        {
            if (regionContract == "") return "";
            //切割出数字剩下Code部分，余下大小写切换，在varties中匹配

            string varieties = "";
            for (int i = 0; i < regionContract.Length; i++)
            {
                if (char.IsDigit(regionContract[i]) == false)
                {
                    varieties += regionContract[i];
                }
            }

            string upperCode = varieties.ToUpper();
            string downCode = varieties.ToLower();

            foreach (Varieties v in m_varieties)
            {
                if (v.Code == upperCode || v.Code == downCode)
                {
                    return v.Code;
                }
            }

            return varieties;
        }

        /// <summary>
        /// 合成标准代码
        /// </summary>
        /// <param name="varitiesCode"></param>
        /// <param name="regionContract"></param>
        /// <returns></returns>
        private string TransferRegionContractToStanderd(string varitiesCode, string regionContract)
        {
            //根据已经生成的Code和region_contract(CF1709)合成标准的合约
            //切割出数字部分和标准Code合成标准合约编码
            if (varitiesCode == "") return "";
            string varieties = "";
            for (int i = 0; i < regionContract.Length; i++)
            {
                if (char.IsDigit(regionContract[i]) == true)
                {
                    varieties += regionContract[i];
                }
            }

            return varitiesCode + varieties;
        }

        /// <summary>
        /// 根据品种代码获取标准交易所
        /// </summary>
        /// <param name="varitiesCode"></param>
        /// <returns></returns>
        private string TransferRegionExchangeToStanderd(string varitiesCode)
        {
            //有了code去varities表中匹配交易所
            if (varitiesCode == "") return "";
            foreach (Varieties v in m_varieties)
            {
                if(v.Code == varitiesCode)
                {
                    return v.Exchange;
                }
            }
            return "";
        }

        /// <summary>
        /// 保存加工最终的数据
        /// </summary>
        /// <param name="instrumentList"></param>
        /// <returns></returns>
        private int SaveResultDataToDB(List<ContractResultData> resultDataList)
        {
            using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
            {
                connection.Open();

                foreach (ContractResultData item in resultDataList)
                {
                    try
                    {
                        if(IsExitPrimerKeyInsCode(item.Contract) == true)
                        {
                            continue;
                        }


                        string cmdText = string.Empty;

                        cmdText = CreateResultDataInsertSql();

                        MySqlCommand command = new MySqlCommand(cmdText, connection);
                        command.Parameters.AddWithValue("@contract", item.Contract);
                        command.Parameters.AddWithValue("@contract_name", item.ContractName);
                        command.Parameters.AddWithValue("@exchange", item.Exchange);
                        command.Parameters.AddWithValue("@open_date", item.OpenDate);
                        command.Parameters.AddWithValue("@expire_date", item.ExpireDate);
                        command.Parameters.AddWithValue("@start_deliv_date", item.StartDelivDate);
                        command.Parameters.AddWithValue("@end_deliv_date", item.EndDelivDate);
                        command.Parameters.AddWithValue("@volume_multiple", item.VolumeMultiple);
                        command.Parameters.AddWithValue("@is_trading", item.IsTrading ? 1 : 0);
                        command.Parameters.AddWithValue("@varieties", item.varieties);
                        command.Parameters.AddWithValue("@price_tick", item.PriceTick);
                        command.Parameters.AddWithValue("@product_class", item.ProductClass.ToString());

                        int result = command.ExecuteNonQuery();
                        Debug.Assert(result == 1);
                        Debug.WriteLine(string.Format("时间:{0}  合约:{1}",DateTime.Now,item.Contract));
                    }
                    catch (Exception ex)
                    {
                        Debug.Assert(false, ex.Message);
                        throw ex;
                    }
                }
            }

            return resultDataList.Count;
        }

        private bool IsExitPrimerKeyInsCode(string contract)
        {
            //读取Varities列表
            string strSel = string.Format(@"select *from {0}.contracts;", m_alphaDBName);

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(strSel, connection);
                adapter.Fill(table);
            }

            foreach (DataRow row in table.Rows)
            {
                if (row["contract"].ToString() == contract)
                {
                    return true;
                }        
            }
            return false;
        }

        /// <summary>
        /// 创建查询语句
        /// </summary>
        /// <returns></returns>
        private string CreateResultDataInsertSql()
        {
            string cmdText = string.Format(@"insert into {0}.contracts(contract,contract_name,exchange,open_date,expire_date,
start_deliv_date,end_deliv_date,volume_multiple,is_trading,varieties,price_tick,
product_class)
VALUES(@contract,@contract_name,@exchange,@open_date,@expire_date,
@start_deliv_date,@end_deliv_date,@volume_multiple,@is_trading,@varieties,@price_tick,
@product_class);",
                m_alphaDBName);
            return cmdText;
        }

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
        /// 从数据库读取variteis便于组装resultData
        /// </summary>
        /// <returns></returns>
        private List<Varieties> GetExistVaritiesFromDB()
        {
            //读取Varities列表
            string strSel = string.Format(@"select *from {0}.varieties;", m_alphaDBName);

            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(m_dbConStr))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(strSel, connection);
                adapter.Fill(table);
            }

            List<Varieties> varietiesList = new List<Varieties>();
            foreach (DataRow row in table.Rows)
            {
                Varieties varities = new Varieties();
                varities.Code = row["code"].ToString();
                varities.Exchange = row["exchange"].ToString();
                varities.ShortName = row["short_name"].ToString();
                varities.LongName = row["long_name"].ToString();
                varities.VolumeMultiple = Convert.ToInt32(row["volume_multiple"].ToString());
                varities.PriceTick = Convert.ToDecimal(row["price_tick"].ToString());
                varities.Symbol = row["symbol"].ToString();

                varietiesList.Add(varities);
            }
            return m_varieties = varietiesList;
        }

        public void StartProcess(string filePath)
        {
            try
            {
                //读取配置文件初始化
                bool iResult = ReadConfig();

                //读取DB品种列表初始化
                List<Varieties> vartiesList = GetExistVaritiesFromDB();

                //获取原始Excel的数据列表
                List<ContractRegionData> regionDataList = ToGetContractRegionData(filePath);

                //原始数据加工
                List<ContractResultData> resultDataList = ToRegionDataProcess(regionDataList);

                //加工成型的数据DB存储
                int saveItems = SaveResultDataToDB(resultDataList);

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

}

