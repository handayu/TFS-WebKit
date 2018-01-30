using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit.ViewModel
{
    class TradeBookViewＭodel : USeBaseViewModel
    {

        #region member
        private string m_tradeNum = null;
        private USeInstrument m_instrument = null;
        private USeOrderNum m_orderNum = null;
        private USeOrderSide m_orderSide = USeOrderSide.Buy;
        private USeOffsetType m_offsetType = USeOffsetType.Open;
        private decimal m_price = 0m;
        private int m_qty = 0;
        private decimal m_amount = 0m;
        private decimal m_fee = 0m;
        private DateTime m_tradeTime = DateTime.MinValue;
        private string m_account = null;

        #endregion

        #region property

        /// <summary>
        /// 成交编号。
        /// </summary>
        public string TradeNum
        {
            get { return m_tradeNum; }
            set
            {
                m_tradeNum = value;
                SetProperty(() => this.TradeNum);
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
        /// 合约名称。
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
        /// 委托单号。
        /// </summary>
        public USeOrderNum OrderNum
        {
            get { return m_orderNum; }
            set
            {
                m_orderNum = value;
                SetProperty(() => this.OrderNum);
            }
        }

        /// <summary>
        /// 买卖方向。
        /// </summary>
        public USeOrderSide OrderSide
        {
            get { return m_orderSide; }
            set
            {
                m_orderSide = value;
                SetProperty(() => this.OrderSide);
                SetProperty(() => this.OrderSideDesc);
            }
        }

        /// <summary>
        /// 买卖方向描述。
        /// </summary>
        public string OrderSideDesc
        {
            get { return m_orderSide.ToDescription(); }
        }

        /// <summary>
        /// 开平标志。
        /// </summary>
        public USeOffsetType OffsetType
        {
            get { return m_offsetType; }
            set
            {
                m_offsetType = value;
                SetProperty(() => this.OffsetType);
                SetProperty(() => this.OffsetTypeDesc);
            }
        }

        /// <summary>
        /// 开平方向描述。
        /// </summary>
        public string OffsetTypeDesc
        {
            get { return m_offsetType.ToDescription(); }
        }

        /// <summary>
        /// 成交价格。
        /// </summary>
        public decimal Price
        {
            get { return m_price; }
            set
            {
                m_price = value;
                SetProperty(() => this.Price);
            }
        }

        /// <summary>
        /// 成交数量。
        /// </summary>
        public int Qty
        {
            get { return m_qty; }
            set
            {
                m_qty = value;
                SetProperty(() => this.Qty);
            }
        }

        /// <summary>
        /// 成交金额(成交金额 = 成交价格 * 成交量 * 每点价值。
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
        /// 手续费。
        /// </summary>
        public decimal Fee
        {
            get { return m_fee; }
            set
            {
                m_fee = value;
                SetProperty(() => this.Fee);
            }
        }

        /// <summary>
        /// 成交时间。
        /// </summary>
        public DateTime TradeTime
        {
            get { return m_tradeTime; }
            set
            {
                m_tradeTime = value;
                SetProperty(() => this.TradeTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Account
        {
            get { return m_account; }
            set
            {
                m_account = value;
                SetProperty(() => this.Account);
            }
        }
        #endregion

        #region update
        public void Update(USeTradeBook entity)
        {
            this.TradeNum = entity.TradeNum;
            this.Instrument = entity.Instrument;
            this.OrderNum = entity.OrderNum;
            this.OrderSide = entity.OrderSide;
            this.OffsetType = entity.OffsetType;
            this.Price = entity.Price;
            this.Qty = entity.Qty;
            this.Amount = entity.Amount;
            this.Fee = entity.Fee;
            this.TradeTime = entity.TradeTime;
            this.Account = entity.Account;

        }
        #endregion

        #region Construct
        public static TradeBookViewＭodel Creat(USeTradeBook trade_book)
        {
            TradeBookViewＭodel data_model = new TradeBookViewＭodel();
            data_model.TradeNum = trade_book.TradeNum;
            data_model.Instrument = trade_book.Instrument;
            data_model.OrderNum = trade_book.OrderNum;
            data_model.OrderSide = trade_book.OrderSide;
            data_model.OffsetType = trade_book.OffsetType;
            data_model.Price = trade_book.Price;
            data_model.Qty = trade_book.Qty;
            data_model.Amount = trade_book.Amount;
            data_model.Fee = trade_book.Fee;
            data_model.TradeTime = trade_book.TradeTime;
            data_model.m_account = trade_book.Account;

            return data_model;
        }
        #endregion
    }

}

