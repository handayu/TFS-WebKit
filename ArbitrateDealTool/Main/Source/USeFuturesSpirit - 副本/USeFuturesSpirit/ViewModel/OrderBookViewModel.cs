using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common;
using USeFuturesSpirit.ViewModel;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public class OrderBookViewModel : USeBaseViewModel
    {
        #region member
        private USeOrderNum m_orderNum = null;
        private string m_account = null;
        private USeInstrument m_instrument = null;
        private int m_orderQty = 0;
        private decimal m_orderPrice = 0m;
        private int m_tradeQty = 0;
        private decimal m_tradeAmount = 0m;
        private decimal m_tradePrice = 0m;
        private decimal m_tradeFee = 0m;
        private USeOrderStatus m_orderStatus = USeOrderStatus.Unknown;
        private int m_cancelQty = 0;
        private int m_blankQty = 0;
        private USeOrderSide m_orderSide = USeOrderSide.Buy;
        private USeOffsetType m_offsetType = USeOffsetType.Open;
        private string m_memo = null;
        private DateTime m_orderTime = DateTime.MinValue;
        private Boolean m_isFinish = false;

        #endregion

        #region property

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
        /// 下单帐号。
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

        /// <summary>
        /// 合约代码。
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
        /// 委托手数。
        /// </summary>
        public int OrderQty
        {
            get { return m_orderQty; }
            set
            {
                m_orderQty = value;
                SetProperty(() => this.OrderQty);
            }
        }

        /// <summary>
        /// 委托价格。
        /// </summary>
        public decimal OrderPrice
        {
            get { return m_orderPrice; }
            set
            {
                m_orderPrice = value;
                SetProperty(() => this.OrderPrice);
            }
        }

        /// <summary>
        /// 成交手数。
        /// </summary>
        public int TradeQty
        {
            get { return m_tradeQty; }
            set
            {
                m_tradeQty = value;
                SetProperty(() => this.TradeQty);
            }
        }

        /// <summary>
        /// 成交金额。
        /// </summary>
        public decimal TradeAmount
        {
            get { return m_tradeAmount; }
            set
            {
                m_tradeAmount = value;
                SetProperty(() => this.TradeAmount);
            }
        }

        /// <summary>
        /// 成交平均价。
        /// </summary>
        public decimal TradePrice
        {
            get { return m_tradePrice; }
            set
            {
                m_tradePrice = value;
                SetProperty(() => this.TradePrice);
            }
        }

        /// <summary>
        /// 交易手续费。
        /// </summary>
        public decimal TradeFee
        {
            get { return m_tradeFee; }
            set
            {
                m_tradeFee = value;
                SetProperty(() => this.TradeFee);
            }
        }

        /// <summary>
        /// 委托单状态。
        /// </summary>
        public USeOrderStatus OrderStatus
        {
            get { return m_orderStatus; }
            set
            {
                m_orderStatus = value;
                SetProperty(() => this.OrderStatus);
                SetProperty(() => this.OrderStatusDesc);
            }
        }

        public string OrderStatusDesc
        {
            get { return m_orderStatus.ToDescription(); }
        }

        /// <summary>
        /// 撤单量。
        /// </summary>
        public int CancelQty
        {
            get { return m_cancelQty; }
            set
            {
                m_cancelQty = value;
                SetProperty(() => this.CancelQty);
            }
        }

        /// <summary>
        /// 废单量。
        /// </summary>
        public int BlankQty
        {
            get { return m_blankQty; }
            set
            {
                m_blankQty = value;
                SetProperty(() => this.BlankQty);
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

        public string OrderSideDesc
        {
            get { return m_orderSide.ToDescription(); }
        }

        /// <summary>
        /// 开平方向。
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
        /// 开平方向。
        /// </summary>
        public string OffsetTypeDesc
        {
            get { return m_offsetType.ToDescription(); }
        }

        /// <summary>
        /// 备注。
        /// </summary>
        public string Memo
        {
            get { return m_memo; }
            set
            {
                m_memo = value;
                SetProperty(() => this.Memo);
            }
        }

        /// <summary>
        /// 委托时间。
        /// </summary>
        public DateTime OrderTime
        {
            get { return m_orderTime; }
            set
            {
                m_orderTime = value;
                SetProperty(() => this.OrderTime);
            }
        }

        /// <summary>
        /// 委托单是否为结束。
        /// </summary>
        public Boolean IsFinish
        {
            get { return m_isFinish; }
            set
            {
                m_isFinish = value;
                SetProperty(() => this.IsFinish);
            }
        }
        #endregion

        #region update
        public void Update(USeOrderBook entity)
        {
            this.OrderNum = entity.OrderNum;
            this.Account = entity.Account;
            this.Instrument = entity.Instrument;
            this.OrderQty = entity.OrderQty;
            this.OrderPrice = entity.OrderPrice;
            this.TradeQty = entity.TradeQty;
            this.TradeAmount = entity.TradeAmount;
            this.TradePrice = entity.TradePrice;
            this.TradeFee = entity.TradeFee;
            this.OrderStatus = entity.OrderStatus;
            this.CancelQty = entity.CancelQty;
            this.BlankQty = entity.BlankQty;
            this.OrderSide = entity.OrderSide;
            this.OffsetType = entity.OffsetType;
            this.Memo = entity.Memo;
            this.OrderTime = entity.OrderTime;
            this.IsFinish = entity.IsFinish;

        }
        #endregion

        #region Construct
        public static OrderBookViewModel Creat(USeOrderBook orderBook)
        {
            OrderBookViewModel viewModel = new OrderBookViewModel();
            viewModel.OrderNum = orderBook.OrderNum;
            viewModel.Account = orderBook.Account;
            viewModel.Instrument = orderBook.Instrument;
            viewModel.OrderQty = orderBook.OrderQty;
            viewModel.OrderPrice = orderBook.OrderPrice;
            viewModel.TradeQty = orderBook.TradeQty;
            viewModel.TradeAmount = orderBook.TradeAmount;
            viewModel.TradePrice = orderBook.TradePrice;
            viewModel.TradeFee = orderBook.TradeFee;
            viewModel.OrderStatus = orderBook.OrderStatus;
            viewModel.CancelQty = orderBook.CancelQty;
            viewModel.BlankQty = orderBook.BlankQty;
            viewModel.OrderSide = orderBook.OrderSide;
            viewModel.OffsetType = orderBook.OffsetType;
            viewModel.Memo = orderBook.Memo;
            viewModel.OrderTime = orderBook.OrderTime;
            viewModel.IsFinish = orderBook.IsFinish;
            return viewModel;
        }

        #endregion
    }

}



