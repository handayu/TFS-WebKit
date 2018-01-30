using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TempUtilityForm
{
    public class ContractRegionData
    {
        /// <summary>
        /// 证券代码
        /// </summary>
        public string Contract
        {
            get;
            set;
        }
        /// <summary>
        /// 证券简称
        /// </summary>
        public string ContractName
        {
            get;
            set;
        }
        /// <summary>
        /// 期货代码
        /// </summary>
        public string ContractFutureName
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
        /// 最后交割日
        /// </summary>
        public DateTime EndDelivDate
        {
            get;
            set;
        }
    }
}
