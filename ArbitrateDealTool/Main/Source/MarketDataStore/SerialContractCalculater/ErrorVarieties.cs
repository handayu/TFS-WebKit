using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialContractCalculater
{
    /// <summary>
    /// 计算异常品种。
    /// </summary>
    class ErrorVarieties
    {
        /// <summary>
        /// 品种。
        /// </summary>
        public string Varieties { get; set; }

        /// <summary>
        /// 结算日。
        /// </summary>
        public DateTime SettlementDate { get; set; }

        /// <summary>
        /// 错误原因。
        /// </summary>
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return string.Format("{0}@{1:yyyy-MM-dd}计算失败, {2}", this.Varieties, this.SettlementDate, this.ErrorMessage);
        }
    }
}
