using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit.ViewModel
{
    public sealed class PositionDataViewModel : USeBaseViewModel
    {
        #region member
        private USeInstrument m_instrument = null;
        private USeDirection m_direction = USeDirection.Long;
        private int m_newPosition = 0;
        private int m_oldPosition = 0;
        private int m_newFrozonPosition = 0;
        private int m_newAvaliablePosition = 0;
        private int m_oldFrozonPosition = 0;
        private int m_oldAvaliablePosition = 0;
        private int m_yesterdayPosition = 0;
        private int m_totalPosition = 0;
        private int m_avaliablePosition = 0;
        private decimal m_avgPrice = 0m;
        private decimal m_amount = 0m;
        private int m_openQty = 0;
        private int m_closeQty = 0;
        private decimal m_marketToMarketPL = 0m;
        private decimal m_marketToPresettlementPL = 0m;

        #endregion

        #region property

        /// <summary>
        /// 合约。
        /// </summary>
        public USeInstrument Instrument
        {
            get { return m_instrument; }
            set
            {
                m_instrument = value;
                SetProperty(() => this.Instrument);
                SetProperty(() => this.InstrumentCode);
                SetProperty(() => this.InstrumentName);
                SetProperty(() => this.Market);

            }
        }

        /// <summary>
        /// 合约代码。
        /// </summary>
        public string InstrumentCode
        {
            get
            {
                if (this.Instrument != null)
                {
                    return this.Instrument.InstrumentCode;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 合约代码。
        /// </summary>
        public string InstrumentName
        {
            get
            {
                if (this.Instrument != null)
                {
                    return this.Instrument.InstrumentName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 市场。
        /// </summary>
        public USeMarket Market
        {
            get
            {
                if (this.Instrument != null)
                {
                    return this.Instrument.Market;
                }
                else
                {
                    return USeMarket.Unknown;
                }
            }
        }

        /// <summary>
        /// 持仓多空方向。
        /// </summary>
        public USeDirection Direction
        {
            get { return m_direction; }
            set
            {
                m_direction = value;
                SetProperty(() => this.Direction);
                SetProperty(() => this.DirectionDesc);
            }
        }

        public string DirectionDesc
        {
            get { return m_direction.ToDescription(); }
        }


        /// <summary>
        /// 今日新仓量(实际持仓)。
        /// </summary>
        public int NewPosition
        {
            get { return m_newPosition; }
            set
            {
                m_newPosition = value;
                SetProperty(() => this.NewPosition);
            }
        }

        /// <summary>
        /// 昨仓量(实际持仓)。
        /// </summary>
        public int OldPosition
        {
            get { return m_oldPosition; }
            set
            {
                m_oldPosition = value;
                SetProperty(() => this.OldPosition);
            }
        }

        /// <summary>
        /// 今日新仓冻结量(平仓指令成交前)。
        /// </summary>
        public int NewFrozonPosition
        {
            get { return m_newFrozonPosition; }
            set
            {
                m_newFrozonPosition = value;
                SetProperty(() => this.NewFrozonPosition);
            }
        }

        /// <summary>
        /// 今日新仓可平量。
        /// </summary>
        public int NewAvaliablePosition
        {
            get { return m_newAvaliablePosition; }
            set
            {
                m_newAvaliablePosition = value;
                SetProperty(() => this.NewAvaliablePosition);
            }
        }

        /// <summary>
        /// 昨仓冻结量(平仓指令成交前)。
        /// </summary>
        public int OldFrozonPosition
        {
            get { return m_oldFrozonPosition; }
            set
            {
                m_oldFrozonPosition = value;
                SetProperty(() => this.OldFrozonPosition);
            }
        }

        /// <summary>
        /// 昨仓可平量。
        /// </summary>
        public int OldAvaliablePosition
        {
            get { return m_oldAvaliablePosition; }
            set
            {
                m_oldAvaliablePosition = value;
                SetProperty(() => this.OldAvaliablePosition);
            }
        }

        /// <summary>
        /// 昨日持仓量(昨日结算后的持仓,当日不变)。
        /// </summary>
        public int YesterdayPosition
        {
            get { return m_yesterdayPosition; }
            set
            {
                m_yesterdayPosition = value;
                SetProperty(() => this.YesterdayPosition);
            }
        }

        /// <summary>
        /// 当前总持仓量。
        /// </summary>
        public int TotalPosition
        {
            get { return m_totalPosition; }
            set
            {
                m_totalPosition = value;
                SetProperty(() => this.TotalPosition);
            }
        }

        /// <summary>
        /// 当前可平持仓量。
        /// </summary>
        public int AvaliablePosition
        {
            get { return m_avaliablePosition; }
            set
            {
                m_avaliablePosition = value;
                SetProperty(() => this.AvaliablePosition);
            }
        }

        /// <summary>
        /// 持仓均价。
        /// </summary>
        public decimal AvgPrice
        {
            get { return m_avgPrice; }
            set
            {
                m_avgPrice = value;
                SetProperty(() => this.AvgPrice);
            }
        }

        /// <summary>
        /// 持仓金额。
        /// </summary>
        public decimal Amount
        {
            get { return m_amount; }
            set
            {
                m_amount = value;
                SetProperty(() => this.Amount);
            }
        }

        /// <summary>
        /// 开仓量。
        /// </summary>
        public int OpenQty
        {
            get { return m_openQty; }
            set
            {
                m_openQty = value;
                SetProperty(() => this.OpenQty);
            }
        }

        /// <summary>
        /// 平仓量。
        /// </summary>
        public int CloseQty
        {
            get { return m_closeQty; }
            set
            {
                m_closeQty = value;
                SetProperty(() => this.CloseQty);
            }
        }

        public decimal MarketToMarketPL
        {
            get { return m_marketToMarketPL; }
            set
            {
                m_marketToMarketPL = value;
                SetProperty(() => this.MarketToMarketPL);
            }
        }

        public decimal MarketToPresettlementPL
        {
            get { return m_marketToPresettlementPL; }
            set
            {
                m_marketToPresettlementPL = value;
                SetProperty(() => this.MarketToPresettlementPL);
            }
        }

        #endregion

        #region update
        public void Update(USePosition entity)
        {
            this.Instrument = entity.Instrument;
            this.Direction = entity.Direction;
            this.NewPosition = entity.NewPosition;
            this.OldPosition = entity.OldPosition;
            this.NewFrozonPosition = entity.NewFrozonPosition;
            this.NewAvaliablePosition = entity.NewAvaliablePosition;
            this.OldFrozonPosition = entity.OldFrozonPosition;
            this.OldAvaliablePosition = entity.OldAvaliablePosition;
            this.YesterdayPosition = entity.YesterdayPosition;
            this.TotalPosition = entity.TotalPosition;
            this.AvaliablePosition = entity.AvaliablePosition;
            this.AvgPrice = entity.AvgPirce;
            this.Amount = entity.Amount;
            this.OpenQty = entity.OpenQty;
            this.CloseQty = entity.CloseQty;
        

        }
        #endregion

        #region Construct
        public static PositionDataViewModel Creat(USePosition entity)
        {
            PositionDataViewModel data_model = new PositionDataViewModel();
            data_model.Instrument = entity.Instrument;
            data_model.Direction = entity.Direction;
            data_model.NewPosition = entity.NewPosition;
            data_model.OldPosition = entity.OldPosition;
            data_model.NewFrozonPosition = entity.NewFrozonPosition;
            data_model.NewAvaliablePosition = entity.NewAvaliablePosition;
            data_model.OldFrozonPosition = entity.OldFrozonPosition;
            data_model.OldAvaliablePosition = entity.OldAvaliablePosition;
            data_model.YesterdayPosition = entity.YesterdayPosition;
            data_model.TotalPosition = entity.TotalPosition;
            data_model.AvaliablePosition = entity.AvaliablePosition;
            data_model.AvgPrice = entity.AvgPirce;
            data_model.Amount = entity.Amount;
            data_model.OpenQty = entity.OpenQty;
            data_model.CloseQty = entity.CloseQty;

            return data_model;

        }
        #endregion


    }
}
