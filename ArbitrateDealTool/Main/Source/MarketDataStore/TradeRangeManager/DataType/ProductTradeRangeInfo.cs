using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeRangeManager
{
    public class ProductTradeRangeInfo
    {
        public ProductTradeRangeInfo(string name,
            string productName,
            string exchange,
            string description,
            List<TradeRangeTimeSectionInfo> TradeRangeTimeSectionsInfo
            )
        {
            this.Name = name;
            this.Exchange = exchange;
            this.Description = description;
            this.TradeRangeTimeSectionsInfo = TradeRangeTimeSectionsInfo;
        }

        public ProductTradeRangeInfo()
        {
            this.Name = string.Empty;
            this.Exchange = string.Empty;
            this.Description = string.Empty;
            this.TradeRangeTimeSectionsInfo = null;
        }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ProductrName
        /// </summary>
        public string ProductrName { get; set; }

        /// <summary>
        /// 交易所。
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 时间描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 交易时间段
        /// </summary>
        public List<TradeRangeTimeSectionInfo> TradeRangeTimeSectionsInfo { get; set; }

    }
}
