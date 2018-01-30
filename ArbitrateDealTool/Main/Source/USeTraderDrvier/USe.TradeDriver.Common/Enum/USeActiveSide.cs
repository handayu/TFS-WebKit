using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 内外盘方向。
    /// </summary>
    public enum USeActiveSide
    {
        /// <summary>
        /// 无。
        /// </summary>
        None = 0,

        /// <summary>
        /// 内盘(主动性卖盘)。
        /// </summary>
        Ask = 1,

        /// <summary>
        /// 外盘(主动行买盘)。
        /// </summary>
        Bid = 2,
    }

    /// <summary>
    /// 内外盘方向扩展类。
    /// </summary>
    public static class USeActiveSideExtend
    {
        /// <summary>
        /// 内外盘方向描述。
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public static string ToDescription(this USeActiveSide side)
        {
            switch(side)
            {
                case USeActiveSide.Ask:return "内盘";
                case USeActiveSide.Bid:return "外盘";
                default:return "未知";
            }
        }
    }
}
