using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeRangeManager
{
    public class ProductInstrumentInfo

    {
        public ProductInstrumentInfo(string code,
            string exchange,
            string shortName,
            string longName
            )
        {
            this.Name = code;
            this.Exchange = exchange;
            this.ShortName = shortName;
            this.LongName = longName;
        }

        public ProductInstrumentInfo()
        {
            this.Name = string.Empty;
            this.Exchange = string.Empty;
            this.ShortName = string.Empty;
            this.LongName = string.Empty;
        }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 交易所。
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 名称简写
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string LongName { get; set; }

    }
}
