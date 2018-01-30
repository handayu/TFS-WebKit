using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USe.Common.Manager;
using USe.TradeDriver.Common;
namespace MarketDataStore
{
    public class StoreContext
    {
        public TradeCalendarManager TradeCalendarManager { get; set; }

        public TradeRangeManager TradeRangeManager { get; set; }

        public USeProductManager ProductManager { get; set; }
    }
}
