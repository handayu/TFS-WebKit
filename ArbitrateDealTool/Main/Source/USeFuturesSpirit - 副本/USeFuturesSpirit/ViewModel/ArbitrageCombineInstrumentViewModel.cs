using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public sealed class ArbitrageCombineInstrumentViewModel : USeBaseViewModel
    {
        #region member
        private ArbitrageCombineInstrument m_arbitrageCombineInstrument = null;

        private string m_arbiCombineInstrument = string.Empty;

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
            get { return m_arbitrageCombineInstrument; }
            set
            {
                m_arbitrageCombineInstrument = value;
                SetProperty(() => this.ArbitrageCombineInstrument);
                SetProperty(() => this.ArbitrageInstrument);
            }
        }

        /// <summary>
        /// 套利组合合约别名。
        /// </summary>
        public string ArbitrageInstrument
        {
            get
            {
                if (m_arbitrageCombineInstrument == null)
                {
                    return string.Empty;
                }
                else
                {
                    return m_arbitrageCombineInstrument.ArbitrageAlisa;
                }
            }

        }

        /// <summary>
        /// 价差
        /// </summary>
        public decimal Spread
        {
            get
            {
                if (m_nearLastPrice <= 0m || m_farLastPrice <= 0)
                {
                    return 0m;
                }
                else
                {
                    return m_nearLastPrice - m_farLastPrice;
                }
            }
        }

        /// <summary>
        /// 近月最新价
        /// </summary>
        public decimal NearLastPrice
        {
            get { return m_nearLastPrice; }
            set
            {
                m_nearLastPrice = value;
                SetProperty(() => this.NearLastPrice);
                SetProperty(() => this.Spread);
            }
        }


        /// <summary>
        /// 远月最新价
        /// </summary>
        public decimal FarLastPrice
        {
            get { return m_farLastPrice; }
            set
            {
                m_farLastPrice = value;
                SetProperty(() => this.FarLastPrice);
                SetProperty(() => this.Spread);
            }
        }

        ///// <summary>
        ///// 近月买价。
        ///// </summary>
        public decimal NearDistanceBuyPrice
        {
            get { return m_nearDistanceBuyPrice; }
            set
            {
                if (value != m_nearDistanceBuyPrice)
                {
                    m_nearDistanceBuyPrice = value;
                    SetProperty(() => this.NearDistanceBuyPrice);
                }
            }
        }

        ///// <summary>
        ///// 远月买价。
        ///// </summary>
        public decimal FarDistanceBuyPrice
        {
            get { return m_farDistanceBuyPrice; }
            set
            {
                if (value != m_farDistanceBuyPrice)
                {
                    m_farDistanceBuyPrice = value;
                    SetProperty(() => this.FarDistanceBuyPrice);
                }
            }
        }

        ///// <summary>
        ///// 近月卖价
        ///// </summary>
        public decimal NearDistanceSellPrice
        {
            get { return m_nearDistanceSellPrice; }
            set
            {
                if (value != m_nearDistanceSellPrice)
                {
                    m_nearDistanceSellPrice = value;
                    SetProperty(() => this.NearDistanceSellPrice);
                }
            }
        }

        ///// <summary>
        ///// 远月卖价
        ///// </summary>
        public decimal FarDistanceSellPrice
        {
            get { return m_farDistanceSellPrice; }
            set
            {
                if (value != m_farDistanceSellPrice)
                {
                    m_farDistanceSellPrice = value;
                    SetProperty(() => this.FarDistanceSellPrice);
                }
            }
        }

        ///// <summary>
        ///// 近月买量
        ///// </summary>
        public int NearDistanceBuyVolumn
        {
            get { return m_nearDistanceBuyVolumn; }
            set
            {
                if (value != m_nearDistanceBuyVolumn)
                {
                    m_nearDistanceBuyVolumn = value;
                    SetProperty(() => this.NearDistanceBuyVolumn);
                }
            }
        }

        ///// <summary>
        ///// 远月买量
        ///// </summary>
        public int FarDistanceBuyVolumn
        {
            get { return m_farDistanceBuyVolumn; }
            set
            {
                if (value != m_farDistanceBuyVolumn)
                {
                    m_farDistanceBuyVolumn = value;
                    SetProperty(() => this.FarDistanceBuyVolumn);
                }
            }
        }

        ///// <summary>
        ///// 近月卖量
        ///// </summary>
        public int NearDistanceSellVolumn
        {
            get { return m_nearDistanceSellVolumn; }
            set
            {
                if (value != m_nearDistanceSellVolumn)
                {
                    m_nearDistanceSellVolumn = value;
                    SetProperty(() => this.NearDistanceSellVolumn);
                }
            }
        }

        ///// <summary>
        ///// 远月卖量
        ///// </summary>
        public int FarDistanceSellVolumn
        {
            get { return m_farDistanceSellVolumn; }
            set
            {
                if (value != m_farDistanceSellVolumn)
                {
                    m_farDistanceSellVolumn = value;
                    SetProperty(() => this.FarDistanceSellVolumn);
                }
            }
        }

        public bool ContainsInstrument(USeInstrument instument)
        {
            if(this.m_arbitrageCombineInstrument.FirstInstrument.Equals(instument) ||
                this.m_arbitrageCombineInstrument.SecondInstrument.Equals(instument))
            {
                return true;
            }

            return false;
        }

        public void UpdateMarketData(USeMarketData marketData)
        {

        }


        public void Update(ArbitrageCombineInstrumentData data)
        {
            this.ArbitrageCombineInstrument = data.ArbitrageCombineInstrument;
            this.NearLastPrice = data.NearLastPrice;
            this.FarLastPrice = data.FarLastPrice;

            this.NearDistanceBuyPrice = data.NearDistanceBuyPrice;
            this.FarDistanceBuyPrice = data.FarDistanceBuyPrice;

            this.NearDistanceSellPrice = data.NearDistanceSellPrice;
            this.FarDistanceSellPrice = data.FarDistanceSellPrice;

            this.NearDistanceBuyVolumn = data.NearDistanceBuyVolumn;
            this.FarDistanceBuyVolumn = data.FarDistanceBuyVolumn;

            this.NearDistanceSellVolumn = data.NearDistanceSellVolumn;
            this.FarDistanceSellVolumn = data.FarDistanceSellVolumn;
        }


        public static ArbitrageCombineInstrumentData CreatArbitrageCombineInstrumentData(ArbitrageCombineInstrumentViewModel model)
        {
            ArbitrageCombineInstrumentData data = new ArbitrageCombineInstrumentData();
            data.ArbitrageCombineInstrument = model.ArbitrageCombineInstrument;
            data.NearLastPrice = model.NearLastPrice;
            data.FarLastPrice = model.FarLastPrice;
            data.NearDistanceBuyPrice = model.NearDistanceBuyPrice;
            data.FarDistanceBuyPrice = model.FarDistanceBuyPrice;
            data.NearDistanceSellPrice = model.NearDistanceSellPrice;
            data.FarDistanceSellPrice = model.FarDistanceSellPrice;
            data.NearDistanceBuyVolumn = model.NearDistanceBuyVolumn;
            data.FarDistanceBuyVolumn = model.FarDistanceBuyVolumn;
            data.NearDistanceSellVolumn = model.NearDistanceSellVolumn;
            data.FarDistanceSellVolumn = model.FarDistanceSellVolumn;
            return data;
        }

        public static ArbitrageCombineInstrumentViewModel CreatArbitrageCombineInstrumentViewModel(ArbitrageCombineInstrumentData data)
        {
            ArbitrageCombineInstrumentViewModel model = new ArbitrageCombineInstrumentViewModel();
            model.ArbitrageCombineInstrument = data.ArbitrageCombineInstrument;
            model.NearLastPrice = data.NearLastPrice;
            model.FarLastPrice = data.FarLastPrice;
            model.NearDistanceBuyPrice = data.NearDistanceBuyPrice;
            model.FarDistanceBuyPrice = data.FarDistanceBuyPrice;
            model.NearDistanceSellPrice = data.NearDistanceSellPrice;
            model.FarDistanceSellPrice = data.FarDistanceSellPrice;
            model.NearDistanceBuyVolumn = data.NearDistanceBuyVolumn;
            model.FarDistanceBuyVolumn = data.FarDistanceBuyVolumn;
            model.NearDistanceSellVolumn = data.NearDistanceSellVolumn;
            model.FarDistanceSellVolumn = data.FarDistanceSellVolumn;
            return model;
        }
    }
}
