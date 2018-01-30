using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit.ViewModel
{
    public sealed class PositionDetailDataViewModel : USeBaseViewModel
    {
        #region member
        private int m_iD = 0;
        private USeInstrument m_instrument = null;
        private USeDirection m_direction = USeDirection.Long;
        private USePositionType m_positionType = USePositionType.Today;
        private int m_openQty = 0;
        private decimal m_openPrice = 0m;
        private DateTime m_openTime = DateTime.MinValue;
        private int m_closeQty = 0;
        private int m_remainQty = 0;
        private decimal m_closeAmount = 0m;
        private decimal m_closePrice = 0m;

        #endregion

        #region property

        /// <summary>
        /// 每笔建仓生成一个ID。
        /// </summary>
        public int ID
        {
            get { return m_iD; }
            set
            {
                m_iD = value;
                SetProperty(() => this.ID);
            }
        }

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
        /// 市场。
        /// </summary>
        public string MarketDesc
        {
            get
            {
                if (this.Instrument != null)
                {
                    return this.Instrument.Market.ToDescription();
                }
                else
                {
                    return string.Empty;
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
        /// 持仓类型。
        /// </summary>
        public USePositionType PositionType
        {
            get { return m_positionType; }
            set
            {
                m_positionType = value;
                SetProperty(() => this.PositionType);
                SetProperty(() => this.PositionTypeDesc);

            }
        }

        /// <summary>
        /// 持仓类型描述。
        /// </summary>
        public string PositionTypeDesc
        {
            get { return m_positionType.ToDescription(); }
        }


        /// <summary>
        /// 建仓数量(昨日仓为所有留仓数量)。
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
        /// 建仓价格。
        /// </summary>
        public decimal OpenPrice
        {
            get { return m_openPrice; }
            set
            {
                m_openPrice = value;
                SetProperty(() => this.OpenPrice);
            }
        }

        /// <summary>
        /// 建仓时间。
        /// </summary>
        public DateTime OpenTime
        {
            get { return m_openTime; }
            set
            {
                m_openTime = value;
                SetProperty(() => this.OpenTime);
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

        /// <summary>
        /// 剩余持仓量。
        /// </summary>
        public int RemainQty
        {
            get { return m_remainQty; }
            set
            {
                m_remainQty = value;
                SetProperty(() => this.RemainQty);
            }
        }

        /// <summary>
        /// 平仓金额。
        /// </summary>
        public decimal CloseAmount
        {
            get { return m_closeAmount; }
            set
            {
                m_closeAmount = value;
                SetProperty(() => this.CloseAmount);
            }
        }

        /// <summary>
        /// 平仓平均价。
        /// </summary>
        public decimal ClosePrice
        {
            get { return m_closePrice; }
            set
            {
                m_closePrice = value;
                SetProperty(() => this.ClosePrice);
            }
        }
        #endregion

        #region update
        public void Update(USePositionDetail entity)
        {
            this.ID = entity.ID;
            this.Instrument = entity.Instrument;
            this.Direction = entity.Direction;
            this.PositionType = entity.PositionType;
            this.OpenQty = entity.OpenQty;
            this.OpenPrice = entity.OpenPrice;
            this.OpenTime = entity.OpenTime;
            this.CloseQty = entity.CloseQty;
            this.RemainQty = entity.RemainQty;
            this.CloseAmount = entity.CloseAmount;
            this.ClosePrice = entity.ClosePrice;

        }
        #endregion

        #region Construct
        public static PositionDetailDataViewModel Creat(USePositionDetail entity)
        {
            PositionDetailDataViewModel data_model = new PositionDetailDataViewModel();
            data_model.ID = entity.ID;
            data_model.Instrument = entity.Instrument;
            data_model.Direction = entity.Direction;
            data_model.PositionType = entity.PositionType;
            data_model.OpenQty = entity.OpenQty;
            data_model.OpenPrice = entity.OpenPrice;
            data_model.OpenTime = entity.OpenTime;
            data_model.CloseQty = entity.CloseQty;
            data_model.RemainQty = entity.RemainQty;
            data_model.CloseAmount = entity.CloseAmount;
            data_model.ClosePrice = entity.ClosePrice;
            data_model.CloseQty = entity.CloseQty;

            return data_model;
        }
        #endregion
    }
}

