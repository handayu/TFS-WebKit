using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;

namespace HistoryKLineImport
{
    /// <summary>
    /// ClosePrice2信息。
    /// </summary>
    class ClosePrice2Entity
    {
        /// <summary>
        /// 结算日。
        /// </summary>
        public DateTime SettlementDate { get; set; }

        /// <summary>
        /// 午盘收盘价。
        /// </summary>
        public decimal ClosePrice2 { get; set; }

        /// <summary>
        /// 合约代码。
        /// </summary>
        public string InstrumentCode { get; set; }

        /// <summary>
        /// 市场。
        /// </summary>
        public USeMarket Exchange { get; set; }

        public override string ToString()
        {
            return string.Format("{0:yyyy-MM-dd} {1}", this.SettlementDate, this.ClosePrice2);
        }
    }
}
