using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    public class ArbitrageOrderSettlement
    {
        public decimal BuyInstrumentProfit { get; set; }

        public decimal SellInstrumentProfit { get; set; }

        public decimal Profit { get; set; }

        public decimal AvgPrice { get; set; }
    }
}
