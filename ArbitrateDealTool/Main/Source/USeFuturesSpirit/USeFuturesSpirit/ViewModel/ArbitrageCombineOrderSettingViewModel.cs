using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public class ArbitrageCombineOrderSettingViewModel
    {
        private USeProduct m_product = null;
        private int m_openVolumn = 0;
        private int m_openVolumnPerNum = 0;
        private USeDirection m_openFirstDirection = USeDirection.Long;
        private USeDirection m_closeFirstDirection = USeDirection.Long;
        private USeDirection m_stoplossFirstDirection = USeDirection.Long;
        private string m_nearPriceStyle = string.Empty;
        private string m_farPriceStyle = string.Empty;

        public USeProduct Product
        {
            get { return m_product; }
            set { value = m_product; }
        }
        public int OpenVolumn
        {
            get { return m_openVolumn; }
            set { value = m_openVolumn; }
        }
        public int OpenVolumnPerNum
        {
            get { return m_openVolumnPerNum; }
            set { value = m_openVolumnPerNum; }
        }
        public USeDirection OpenFirstDirection
        {
            get { return m_openFirstDirection; }
            set { value = m_openFirstDirection; }
        }
        public USeDirection CloseFirstDirection
        {
            get { return m_closeFirstDirection; }
            set { value = m_closeFirstDirection; }
        }
        public USeDirection StoplossFirstDirection
        {
            get { return m_stoplossFirstDirection; }
            set { value = m_stoplossFirstDirection; }
        }

        public string NearPriceStyle
        {
            get { return m_nearPriceStyle; }
            set { value = m_nearPriceStyle; }
        }
        public string FarPriceStyle
        {
            get { return m_farPriceStyle; }
            set { value = m_farPriceStyle; }
        }


    }
}
