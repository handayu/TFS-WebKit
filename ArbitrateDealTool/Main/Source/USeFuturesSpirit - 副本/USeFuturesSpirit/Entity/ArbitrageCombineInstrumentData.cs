using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    public class ArbitrageCombineInstrumentData
    {
        #region member
        private ArbitrageCombineInstrument m_arbitrageCombineInstrument = null;

        private decimal m_spread = 0m;

        private decimal m_nearLastPrice = 0m;
        private decimal m_farLastPrice = 0m;

        private decimal m_nearDistanceBuyPrice = 0m;
        private decimal m_farDistanceBuyPrice = 0m;

        private decimal m_nearDistanceSellPrice = 0;
        private decimal m_farDistanceSellPrice = 0m;

        private int m_nearDistanceBuyVolumn = 0;
        private int m_farDistanceBuyVolumn = 0;

        private int m_nearDistanceSellVolumn = 0;
        private int m_farDistanceSellVolumn = 0;

        #endregion

        /// <summary>
        /// 套利组合合约。
        /// </summary>
        public ArbitrageCombineInstrument ArbitrageCombineInstrument
        {
            get;
            set;
        }

        /// <summary>
        /// 价差
        /// </summary>
        public decimal Spread
        {
            get { return NearLastPrice - FarLastPrice; }
        }

        /// <summary>
        /// 近月最新价
        /// </summary>
        public decimal NearLastPrice
        {
            get;
            set;
        }


        /// <summary>
        /// 远月最新价
        /// </summary>
        public decimal FarLastPrice
        {
            get;
            set;
        }

        ///// <summary>
        ///// 近月买价。
        ///// </summary>
        public decimal NearDistanceBuyPrice
        {
            get;
            set;
        }

        ///// <summary>
        ///// 远月买价。
        ///// </summary>
        public decimal FarDistanceBuyPrice
        {
            get;
            set;
        }

        ///// <summary>
        ///// 近月卖价
        ///// </summary>
        public decimal NearDistanceSellPrice
        {
            get;
            set;
        }

        ///// <summary>
        ///// 远月卖价
        ///// </summary>
        public decimal FarDistanceSellPrice
        {
            get;
            set;
        }

        ///// <summary>
        ///// 近月买量
        ///// </summary>
        public int NearDistanceBuyVolumn
        {
            get;
            set;
        }

        ///// <summary>
        ///// 远月买量
        ///// </summary>
        public int FarDistanceBuyVolumn
        {
            get;
            set;
        }

        ///// <summary>
        ///// 近月卖量
        ///// </summary>
        public int NearDistanceSellVolumn
        {
            get;
            set;
        }

        ///// <summary>
        ///// 远月卖量
        ///// </summary>
        public int FarDistanceSellVolumn
        {
            get;
            set;
        }

    }
}
