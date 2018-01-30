using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    public class ErrorArbitrageOrder
    {
        public ErrorArbitrageOrder()
        {
            this.HasChanged = false;
        }

        public Guid TraderIdentify
        {
            get
            {
                if(this.ArbitrageOrder != null)
                {
                    return this.ArbitrageOrder.TraderIdentify;
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }
        /// <summary>
        /// 套利单。
        /// </summary>
        public USeArbitrageOrder ArbitrageOrder { get; set; }

        /// <summary>
        /// 异常委托单。
        /// </summary>
        public List<ErrorUSeOrderBook> ErrorOrderBooks { get; set; }

        /// <summary>
        /// 是否有异常委托。
        /// </summary>
        public bool HasError
        {
            get
            {
                if (this.ErrorOrderBooks == null || this.ErrorOrderBooks.Count <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 是否有变更。
        /// </summary>
        public bool HasChanged { get; set; }
    }
}
