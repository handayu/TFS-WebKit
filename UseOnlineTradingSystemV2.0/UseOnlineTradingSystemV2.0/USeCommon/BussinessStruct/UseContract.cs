using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 合约响应
    /// </summary>
    public class ContractResponse : BaseResponse
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string timeStamp;
        /// <summary>
        /// 合约数据
        /// </summary>
        public ContractDic data;
    }

    /// <summary>
    /// 合约数据字典
    /// </summary>
    public class ContractDic
    {
        public long today9;
        /// <summary>
        /// 合约信息字典
        /// </summary>
        public Dictionary<string, ContractCategoryDic> categoryVoMap;
        /// <summary>
        /// 最新合约字典
        /// </summary>
        public Dictionary<string, ContractLastPrice> lastPriceMap;
    }

    /// <summary>
    /// 合约信息
    /// </summary>
    public class ContractCategoryDic
    {
        /// <summary>
        /// id
        /// </summary>
        public string id;
        /// <summary>
        /// 品类代码
        /// </summary>
        public string categoryCode;
        /// <summary>
        /// 品类名称
        /// </summary>
        public string categoryName;
        /// <summary>
        /// 合约字典
        /// </summary>
        public Dictionary<string, ContractBasePrice> contractMonthMap;
    }

    /// <summary>
    /// 合约最新价格信息
    /// </summary>
    public class ContractLastPrice
    {
        /// <summary>
        /// 合约代码，如cu1801
        /// </summary>
        public string contractMonth;
        /// <summary>
        /// 品类,如cu
        /// </summary>
        public string category;
        /// <summary>
        /// 卖价
        /// </summary>
        public float askPrice;
        /// <summary>
        /// 买价
        /// </summary>
        public float bidPrice;
        /// <summary>
        /// 最新价
        /// </summary>
        public float lastPrice;
    }

    /// <summary>
    /// 合约基础价格信息
    /// </summary>
    public class ContractBasePrice
    {
        /// <summary>
        /// 品类
        /// </summary>
        public string category;
        /// <summary>
        /// 品类名称
        /// </summary>
        public string categoryName;
        public string preSettlementPrice;
        /// <summary>
        /// 最低价
        /// </summary>
        public string lowestPrice;
        /// <summary>
        /// 最高价
        /// </summary>
        public string highestPrice;

        ////实时合约价格
        //public ContractMarketData marketData;

        public override string ToString()
        {
            return this.categoryName;
        }
    }

    ///// <summary>
    ///// 合约实时价格信息
    ///// </summary>
    //public class ContractMarketData
    //{
    //    // 时间点ID
    //    public long timeId;
    //    // 实际时间戳
    //    public long truthTime;
    //    // 品类 例：cu1710 al1711
    //    public string category;
    //    // 申买价
    //    public float buyPriceDiff;
    //    // 申卖价
    //    public float sellPriceDiff;
    //    // 最新价
    //    public float lastPriceDiff;
    //}
}
