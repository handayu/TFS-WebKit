using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialContractCalculater
{

    public class ContractSeriesData
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        public string Varities;

        /// <summary>
        /// 连续数据表示
        /// </summary>
        public string SeriesData;

    }

    public class MainContractsData
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        public string Varities;

        /// <summary>
        /// 连续数据表示
        /// </summary>
        public string SettlementDate;

        /// <summary>
        /// 连续数据表示Series1
        /// </summary>
        public string Series01;

        /// <summary>
        /// 连续数据表示Series2
        /// </summary>
        public string Series02;

        /// <summary>
        /// 连续数据表示Series3
        /// </summary>
        public string Series03;

    }


    /// <summary>
    /// 数据库中只写123到期后下一个最近的主力合约，合约到varities中找
    /// </summary>
    public class Series123ContractCal
    {

        public Series123ContractCal()
        {

        }

        /// <summary>
        /// 获取Varieties-Series列表
        /// </summary>
        /// <returns></returns>
        private List<ContractSeriesData> GetContractSeriesData()
        {
            List<ContractSeriesData> seriesData = new List<ContractSeriesData>();



            return seriesData;
        } 

        /// <summary>
        /// 检查合约是否过期
        /// </summary>
        private void CheckIfIntrumentIsInspire()
        {

        }

        /// <summary>
        /// 检查所有合约，确定Series1-2-3合约
        /// </summary>
        private void CheckEverySeriesContract()
        {

        }


    }

}
