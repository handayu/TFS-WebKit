using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 套利单状态枚举定义。
    /// </summary>
    public enum ArbitrageOrderState
    {
        /// <summary>
        /// 空。
        /// </summary>
        None = 0,

        /// <summary>
        /// 开仓中。
        /// </summary>
        Opening = 1,

        /// <summary>
        /// 开仓完成。
        /// </summary>
        Opened = 2,

        /// <summary>
        /// 平仓中
        /// </summary>
        Closeing = 3,

        /// <summary>
        /// 平仓完成。
        /// </summary>
        Closed = 4,

        /// <summary>
        /// 结束。
        /// </summary>
        Finish = 5,
    }

    public static class ArbitrageOrderStateExtend
    {
        public static string ToDescription(this ArbitrageOrderState state)
        {
            switch(state)
            {
                case ArbitrageOrderState.Opening:return "开仓中";
                case ArbitrageOrderState.Opened:return "开仓完成";
                case ArbitrageOrderState.Closeing:return "平仓中";
                case ArbitrageOrderState.Closed:return "平仓完成";
                case ArbitrageOrderState.Finish:return "结束";
                default:return "未知";
            }
        }
    }
}
