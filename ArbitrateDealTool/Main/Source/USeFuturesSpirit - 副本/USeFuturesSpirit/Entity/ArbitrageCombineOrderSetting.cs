using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public class ArbitrageCombineOrderSetting
    {
        //private USeProduct m_product = null;
        //private int m_openVolumn = 0;
        //private int m_openVolumnPerNum = 0;
        //private USeInstrument m_openFirstInstrument = null;
        //private USeInstrument m_closeFirstInstrument = null;
        //private USeInstrument m_stoplossFirstInstrument = null;
        //private USeMarketPriceMethod m_nearPriceStyle  = USeMarketPriceMethod.Unknown;
        //private USeMarketPriceMethod m_farPriceStyle = USeMarketPriceMethod.Unknown;

        public USeProduct Product
        {
            get;
            set;
        }
        public int OpenVolumn
        {
            get;
            set;
        }
        public int OpenVolumnPerNum
        {
            get;
            set;
        }
        public USeDirection OpenFirstDirection
        {
            get;
            set;
        }
        public USeDirection CloseFirstDirection
        {
            get;
            set;
        }
        public USeDirection StoplossFirstDirection
        {
            get;
            set;
        }

        public ArbitrageOrderPriceType NearPriceStyle
        {
            get;
            set;
        }
        public ArbitrageOrderPriceType FarPriceStyle
        {
            get;
            set;
        }

    }
}
