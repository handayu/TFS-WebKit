using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 前置服务配置。
    /// </summary>
    public class FrontSeverConfig
    {
        /// <summary>
        /// 经纪商ID。
        /// </summary>
        public string BrokerID { get; set; }

        /// <summary>
        /// 经纪商名称。
        /// </summary>
        public string BrokerName { get; set; }

        /// <summary>
        /// 行情前置地址。
        /// </summary>
        public string QuoteFrontAddress { get; set; }

        /// <summary>
        /// 行情前置端口。
        /// </summary>
        public int QuoteFrontPort { get; set; }

        /// <summary>
        /// 交易前置地址。
        /// </summary>
        public string TradeFrontAddress { get; set; }

        /// <summary>
        /// 交易前置端口。
        /// </summary>
        public int TradeFrontPort { get; set; }

        public override string ToString()
        {
            return this.BrokerName;
        }
    }
}
