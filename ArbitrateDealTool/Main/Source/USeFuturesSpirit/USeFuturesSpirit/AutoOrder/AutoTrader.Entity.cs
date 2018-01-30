using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public partial class AutoTrader
    {
        /// <summary>
        /// 尝试下单条件。
        /// </summary>
        private class TryOrderCondition
        {
            public TryOrderCondition()
            {
            }

            /// <summary>
            /// 最大尝试下单次数。
            /// </summary>
            public int MaxTryOrderCount { get; set; }

            /// <summary>
            /// 下次下单间隔。
            /// </summary>
            public TimeSpan NextOrderInterval { get; set; }
        }

        /// <summary>
        /// 下单命令。
        /// </summary>
        private class OrderCommand
        {
            #region property
            /// <summary>
            /// 任务ID。
            /// </summary>
            public int TaskId { get; set; }

            /// <summary>
            /// 下单合约。
            /// </summary>
            public USeInstrument Instrument { get; set; }

            /// <summary>
            /// 买卖方向。
            /// </summary>
            public USeOrderSide OrderSide { get; set; }

            /// <summary>
            /// 开平方向。
            /// </summary>
            public USeOffsetType OffsetType { get; set; }

            /// <summary>
            /// 委托数量。
            /// </summary>
            public int OrderQty { get; set; }

            /// <summary>
            /// 委托价格。
            /// </summary>
            public decimal OrderPrice { get; set; }

            /// <summary>
            /// 委托原因。
            /// </summary>
            public string OrderReason { get; set; }

            /// <summary>
            /// 委托单号。
            /// </summary>
            public USeOrderNum OrderNum { get; set; }

            /// <summary>
            /// 委托错误。
            /// </summary>
            public string OrderErrorMessage { get; set; }
            #endregion

            #region methods
            /// <summary>
            /// 创建原始委托回报。
            /// </summary>
            /// <param name="orderNum"></param>
            /// <returns></returns>
            public USeOrderBook CreateOrignalOrderBook()
            {
                USeOrderBook orderBook = new USeOrderBook();
                orderBook.OrderNum = this.OrderNum;
                orderBook.Account = string.Empty;
                orderBook.Instrument = this.Instrument;
                orderBook.OrderQty = this.OrderQty;
                orderBook.OrderPrice = this.OrderPrice;
                orderBook.TradeQty = 0;
                orderBook.TradeAmount = 0m;
                orderBook.TradePrice = 0m;
                orderBook.TradeFee = 0m;
                orderBook.OrderStatus = USeOrderStatus.Unknown;
                orderBook.CancelQty = 0;
                orderBook.OrderSide = this.OrderSide;
                orderBook.OffsetType = this.OffsetType;
                orderBook.Memo = string.Empty;
                orderBook.OrderTime = DateTime.Now;

                return orderBook;
            }

            /// <summary>
            /// 描述信息。
            /// </summary>
            /// <returns></returns>
            public string ToDescription()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.Instrument.InstrumentCode);
                sb.Append(this.OffsetType.ToDescription());
                sb.Append(this.OrderSide.ToDescription());
                sb.Append(string.Format("{0}手", this.OrderQty));
                sb.Append(string.Format("@{0}", this.OrderPrice));
                if (string.IsNullOrEmpty(this.OrderErrorMessage))
                {
                    sb.Append("挂单成功");
                }
                else
                {
                    sb.Append(string.Format("挂单失败,原因:{0}", this.OrderErrorMessage));
                }

                return sb.ToString();
            }
            #endregion
        }
    }
}
