#region Copyright & Version
//==============================================================================
// 文件名称: USeTradeBook.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: USe成交回报定义。
//==============================================================================
#endregion

using System;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe成交回报定义。
    /// </summary>
    public class USeTradeBook
    {
        public USeTradeBook()
        {
        }

        #region property
        /// <summary>
        /// 成交编号。
        /// </summary>
        public string TradeNum
        {
            get;
            set;
        }

        /// <summary>
        /// 合约。
        /// </summary>
        public USeInstrument Instrument
        {
            get;
            set;
        }

        /// <summary>
        /// 合约代码。
        /// </summary>
        public string InstrumentCode
        {
            get { return this.Instrument == null ? string.Empty : this.Instrument.InstrumentCode; }
        }

        /// <summary>
        /// 合约名称。
        /// </summary>
        public string InstrumentName
        {
            get { return this.Instrument == null ? string.Empty : this.Instrument.InstrumentName; }
        }

        /// <summary>
        /// 市场。
        /// </summary>
        public USeMarket Market
        {
            get { return this.Instrument == null ? USeMarket.Unknown : this.Instrument.Market; }
        }

        /// <summary>
        /// 委托单号。
        /// </summary>
        public USeOrderNum OrderNum
        {
            get;
            set;
        }

        /// <summary>
        /// 买卖方向。
        /// </summary>
        public USeOrderSide OrderSide
        {
            get;
            set;
        }

        /// <summary>
        /// 开平标志。
        /// </summary>
        public USeOffsetType OffsetType
        {
            get;
            set;
        }

        /// <summary>
        /// 成交价格。
        /// </summary>
        public decimal Price
        {
            get;
            set;
        }

        /// <summary>
        /// 成交数量。
        /// </summary>
        public int Qty
        {
            get;
            set;
        }

        /// <summary>
        /// 成交金额(成交金额 = 成交价格 * 成交量 * 每点价值。
        /// </summary>
        public decimal Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 手续费。
        /// </summary>
        public decimal Fee
        {
            get;
            set;
        }

        /// <summary>
        /// 成交时间。
        /// </summary>
        public DateTime TradeTime
        {
            get;
            set;
        }

        public string Account
        {
            get;
            set;
        }
        #endregion // property

        public USeTradeBook Clone()
        {
            USeTradeBook tradeBook = new USeTradeBook();
            tradeBook.TradeNum = this.TradeNum;
            tradeBook.Instrument = this.Instrument == null ? null : this.Instrument.Clone();
            tradeBook.OrderNum = this.OrderNum == null ? null : this.OrderNum.Clone();
            tradeBook.OrderSide = this.OrderSide;
            tradeBook.OffsetType = this.OffsetType;
            tradeBook.Price = this.Price;
            tradeBook.Qty = this.Qty;
            tradeBook.Amount = this.Amount;
            tradeBook.Fee = this.Fee;
            tradeBook.TradeTime = this.TradeTime;
            tradeBook.Account = this.Account;

            return tradeBook;
        }
    }
}
