using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TempUtilityForm
{
    public class ContractResultData
    {
        /// <summary>
        /// 合约名称
        /// </summary>
        public string Contract
        {
            get;
            set;
        }
        /// <summary>
        /// 品种名称
        /// </summary>
        public string ContractName
        {
            get;
            set;
        }
        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange
        {
            get;
            set;
        }
        /// <summary>
        /// 开始交易时间
        /// </summary>
        public DateTime OpenDate
        {
            get;
            set;
        }
        /// <summary>
        /// 到期日
        /// </summary>
        public DateTime ExpireDate
        {
            get;
            set;
        }
        /// <summary>
        /// 开始交割日
        /// </summary>
        public DateTime StartDelivDate
        {
            get;
            set;
        }
        /// <summary>
        /// 最后交割日
        /// </summary>
        public DateTime EndDelivDate
        {
            get;
            set;
        }
        /// <summary>
        /// 合约乘数
        /// </summary>
        public int VolumeMultiple
        {
            get;
            set;
        }
        /// <summary>
        /// 是否交易
        /// </summary>
        public bool IsTrading
        {
            get;
            set;
        }
        /// <summary>
        /// 品种
        /// </summary>
        public string varieties
        {
            get;
            set;
        }
        /// <summary>
        /// tick点值
        /// </summary>
        public decimal PriceTick
        {
            get;
            set;
        }
        /// <summary>
        /// 多头保证金率
        /// </summary>
        public decimal ExchangeLongMarginRadio
        {
            get;
            set;
        }
        /// <summary>
        /// 空头保证金率
        /// </summary>
        public decimal ExchangeShortMarginRadio
        {
            get;
            set;
        }
        /// <summary>
        /// 品种类别
        /// </summary>
        public string ProductClass
        {
            get;
            set;
        }

    }
}
