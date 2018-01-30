#region Copyright & Version
//==============================================================================
// 文件名称: USeOrderBook.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: USe委托回报定义。
//==============================================================================
#endregion

using System;
using System.Xml.Serialization;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 委托回报信息类。
    /// </summary>
    public class USeOrderBook
    {
        #region property
        /// <summary>
        /// 委托单号。
        /// </summary>
        public USeOrderNum OrderNum
        {
            get;
            set;
        }

        /// <summary>
        /// 下单帐号。
        /// </summary>
        public string Account
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
        [XmlIgnoreAttribute]
        public string InstrumentCode
        {
            get
            {
                return this.Instrument == null ? string.Empty : this.Instrument.InstrumentCode;
            }
        }

        /// <summary>
        /// 合约名称。
        /// </summary>
        [XmlIgnoreAttribute]
        public string InstrumentName
        {
            get
            {
                return this.Instrument == null ? string.Empty : this.Instrument.InstrumentName;
            }
        }

        /// <summary>
        /// 市场。
        /// </summary>
        [XmlIgnoreAttribute]
        public USeMarket Market
        {
            get
            {
                return this.Instrument == null ? USeMarket.Unknown : this.Instrument.Market;
            }
        }

        /// <summary>
        /// 委托手数。
        /// </summary>
        public int OrderQty
        {
            get;
            set;
        }

        /// <summary>
        /// 委托价格。
        /// </summary>
        public decimal OrderPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 成交手数。
        /// </summary>
        public int TradeQty
        {
            get;
            set;
        }

        /// <summary>
        /// 成交金额。
        /// </summary>
        public decimal TradeAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 成交平均价。
        /// </summary>
        public decimal TradePrice
        {
            get;
            set;
        }

        /// <summary>
        /// 交易手续费。
        /// </summary>
        public decimal TradeFee
        {
            get;
            set;
        }

        /// <summary>
        /// 委托单状态。
        /// </summary>
        public USeOrderStatus OrderStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 撤单量。
        /// </summary>
        public int CancelQty
        {
            get;
            set;
        }

        /// <summary>
        /// 废单量。
        /// </summary>
        [XmlIgnoreAttribute]
        public int BlankQty
        {
            get
            {
                if (this.OrderStatus == USeOrderStatus.BlankOrder)
                {
                    return this.OrderQty - this.TradeQty - this.CancelQty;
                }
                else
                {
                    return 0;
                }
            }
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
        /// 开平方向。
        /// </summary>
        public USeOffsetType OffsetType
        {
            get;
            set;
        }

        /// <summary>
        /// 备注。
        /// </summary>
        public string Memo
        {
            get;
            set;
        }

        /// <summary>
        /// 委托时间。
        /// </summary>
        public DateTime OrderTime
        {
            get;
            set;
        }

        /// <summary>
        /// 委托单是否为结束。
        /// </summary>
        /// <remarks>
        /// 全部成交,撤单,部撤,废单都标识委托单结束。
        /// </remarks>
        [XmlIgnoreAttribute]
        public bool IsFinish
        {
            get
            {
                return (this.OrderStatus == USeOrderStatus.AllTraded ||
                        this.OrderStatus == USeOrderStatus.AllCanceled ||
                        this.OrderStatus == USeOrderStatus.BlankOrder ||
                        this.OrderStatus == USeOrderStatus.PartCanceled);
            }
        }
        #endregion // property

        public USeOrderBook Clone()
        {
            USeOrderBook orderBook = new USeOrderBook();
            orderBook.OrderNum = this.OrderNum;
            orderBook.Account = this.Account;
            orderBook.Instrument = this.Instrument.Clone();
            orderBook.OrderQty = this.OrderQty;
            orderBook.OrderPrice = this.OrderPrice;
            orderBook.TradeQty = this.TradeQty;
            orderBook.TradeAmount = this.TradeAmount;
            orderBook.TradePrice = this.TradePrice;
            orderBook.TradeFee = this.TradeFee;
            orderBook.OrderStatus = this.OrderStatus;
            orderBook.CancelQty = this.CancelQty;
            orderBook.OrderSide = this.OrderSide;
            orderBook.OffsetType = this.OffsetType;
            orderBook.Memo = this.Memo;
            orderBook.OrderTime = this.OrderTime;

            return orderBook;
        }
    }
}
