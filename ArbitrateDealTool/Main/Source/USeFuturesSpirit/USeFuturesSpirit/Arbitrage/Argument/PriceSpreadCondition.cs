using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 价差条件。
    /// </summary>
    public class PriceSpreadCondition
    {
        /// <summary>
        /// 价差监控方向。
        /// </summary>
        public PriceSpreadSide PriceSpreadSide { get; set; }

        /// <summary>
        /// 价差阀值。
        /// </summary>
        public decimal PriceSpreadThreshold { get; set; }

        public PriceSpreadCondition Clone()
        {
            PriceSpreadCondition entity = new PriceSpreadCondition();
            entity.PriceSpreadSide = this.PriceSpreadSide;
            entity.PriceSpreadThreshold = this.PriceSpreadThreshold;

            return entity;
        }

        public override string ToString()
        {
            switch (this.PriceSpreadSide)
            {
                case PriceSpreadSide.LessOrEqual: return "<= " + this.PriceSpreadThreshold.ToString();
                case PriceSpreadSide.GreaterOrEqual:return ">= " + this.PriceSpreadThreshold.ToString();
                default:return this.PriceSpreadThreshold.ToString();
            }
        }
    }
}
