#region Copyright & Version
//==============================================================================
// 文件名称: USeMargin.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2014/11/20
// 描    述: 期权交易成本信息定义。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.TradeDriver.Common
{
    public class USeOptionTradeCost
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        public USeInstrument Instrument
        {
            get;
            set;
        }

        /// <summary>
        /// 期权合约保证金不变部分
        /// </summary>
        public decimal FixedMargin
        {
            get;
            set;
        }

        /// <summary>
        /// 期权合约最小保证金
        /// </summary>
        public decimal MiniMargin
        {
            get;
            set;
        }

        public USeOptionTradeCost Clone()
        {
            USeOptionTradeCost entity = new USeOptionTradeCost();
            entity.Instrument = this.Instrument.Clone();
            entity.FixedMargin = this.FixedMargin;
            entity.MiniMargin = this.MiniMargin;

            return entity;
        }
    }
}
