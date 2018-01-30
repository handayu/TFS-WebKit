#region Copyright & Version
//==============================================================================
// 文件名称: USePosition.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: USe持仓信息定义。
//==============================================================================
#endregion

using System;
using System.Diagnostics;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe持仓信息。
    /// </summary>
    public class USePosition
    {
        #region property
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
            get
            {
                return this.Instrument == null ? string.Empty : this.Instrument.InstrumentCode;
            }
        }

        /// <summary>
        /// 合约代码。
        /// </summary>
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
        public USeMarket Market
        {
            get
            {
                return this.Instrument == null ? USeMarket.Unknown : this.Instrument.Market;
            }
        }
        /// <summary>
        /// 持仓多空方向。
        /// </summary>
        public USeDirection Direction
        {
            get;
            set;
        }

        /// <summary>
        /// 今日新仓量(实际持仓)。
        /// </summary>
        public int NewPosition
        {
            get;
            set;
        }

        /// <summary>
        /// 昨仓量(实际持仓)。
        /// </summary>
        public int OldPosition
        {
            get;
            set;
        }

        /// <summary>
        /// 今日新仓冻结量(平仓指令成交前)。
        /// </summary>
        public int NewFrozonPosition
        {
            get;
            set;
        }

        /// <summary>
        /// 今日新仓可平量。
        /// </summary>
        public int NewAvaliablePosition
        {
            get 
            {
                //Debug.Assert(this.NewPosition >= this.NewFrozonPosition);
                return this.NewPosition - this.NewFrozonPosition;
            }
        }

        /// <summary>
        /// 昨仓冻结量(平仓指令成交前)。
        /// </summary>
        public int OldFrozonPosition
        {
            get;
            set;
        }

        /// <summary>
        /// 昨仓可平量。
        /// </summary>
        public int OldAvaliablePosition
        {
            get
            {
                //Debug.Assert(this.OldPosition >= this.OldFrozonPosition);
                return this.OldPosition - this.OldFrozonPosition;
            }
        }

        /// <summary>
        /// 昨日持仓量(昨日结算后的持仓,当日不变)。
        /// </summary>
        public int YesterdayPosition
        {
            get;
            set;
        }

        /// <summary>
        /// 当前总持仓量。
        /// </summary>
        public int TotalPosition
        {
            get { return this.NewPosition + this.OldPosition; }
        }

        /// <summary>
        /// 当前可平持仓量。
        /// </summary>
        public int AvaliablePosition
        {
            get { return this.NewAvaliablePosition + this.OldAvaliablePosition; }
        }

        /// <summary>
        /// 持仓均价。
        /// </summary>
        public decimal AvgPirce
        {
            get;
            set;
        }

        /// <summary>
        /// 持仓金额。
        /// </summary>
        public decimal Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 开仓量。
        /// </summary>
        public int OpenQty
        {
            get;
            set;
        }

        /// <summary>
        /// 平仓量。
        /// </summary>
        public int CloseQty
        {
            get;
            set;
        }
        #endregion // property

        /// <summary>
        /// 克隆USePosition对象。
        /// </summary>
        /// <returns></returns>
        public USePosition Clone()
        {
            USePosition position = new USePosition();
            position.Instrument = this.Instrument.Clone();
            position.Direction = this.Direction;
            position.NewPosition = this.NewPosition;
            position.OldPosition = this.OldPosition;
            position.YesterdayPosition = this.YesterdayPosition;
            position.OpenQty = this.OpenQty;
            position.AvgPirce = this.AvgPirce;
            position.Amount = this.Amount;
            position.CloseQty = this.CloseQty;
            position.NewFrozonPosition = this.NewFrozonPosition;
            position.OldFrozonPosition = this.OldFrozonPosition;
            
            return position;
        }
    }
}
