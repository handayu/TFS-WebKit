﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit.Arbitrage
{
    /// <summary>
    /// 套利单开仓参数。
    /// </summary>
    public class ArbitrageOpenArgument
    {
        #region member
        /// <summary>
        /// 买入合约。
        /// </summary>
        public USeInstrument BuyInstrument { get; set; }

        /// <summary>
        /// 买入合约下单类型。
        /// </summary>
        public ArbitrageOrderPriceType BuyInstrumentOrderPriceType { get; set; }

        public ArbitrageOrderPriceType NearOrderPriceType { get; set; }

        public ArbitrageOrderPriceType FarOrderPriceType { get; set; }

        /// <summary>
        /// 卖出合约。
        /// </summary>
        public USeInstrument SellInstrument { get; set; }

        /// <summary>
        /// 卖出合约下单类型。
        /// </summary>
        public ArbitrageOrderPriceType SellInstrumentOrderPriceType { get; set; }

        /// <summary>
        /// 优先买入方向。
        /// </summary>
        public USeOrderSide PreferentialSide { get; set; }

        /// <summary>
        /// 开仓价差条件。
        /// </summary>
        public PriceSpreadCondition OpenCondition { get; set; }

        /// <summary>
        /// 单边下单量。
        /// </summary>
        public int TotalOrderQty { get; set; }

        /// <summary>
        /// 下单单位（每次下单几口)。
        /// </summary>
        public int OrderQtyUint { get; set; }

        /// <summary>
        /// 买卖合约开仓口数单元差。
        /// </summary>
        public int DifferentialUnit { get; set; }
        #endregion

        /// <summary>
        /// 克隆。
        /// </summary>
        /// <returns></returns>
        public ArbitrageOpenArgument Clone()
        {
            ArbitrageOpenArgument arg = new ArbitrageOpenArgument();
            arg.BuyInstrument = this.BuyInstrument == null ? null : this.BuyInstrument;
            arg.BuyInstrumentOrderPriceType = this.BuyInstrumentOrderPriceType;
            arg.SellInstrument = this.SellInstrument == null ? null : this.SellInstrument;
            arg.SellInstrumentOrderPriceType = this.SellInstrumentOrderPriceType;
            arg.NearOrderPriceType = this.NearOrderPriceType;
            arg.FarOrderPriceType = this.FarOrderPriceType;
            arg.PreferentialSide = this.PreferentialSide;
            arg.OpenCondition = this.OpenCondition == null? null: this.OpenCondition.Clone();
            arg.TotalOrderQty = this.TotalOrderQty;
            arg.OrderQtyUint = this.OrderQtyUint;
            arg.DifferentialUnit = this.DifferentialUnit;

            return arg;
        }
    }
}
