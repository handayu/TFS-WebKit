using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 自动下单机工作类型
    /// </summary>
    public enum AutoTraderWorkType
    {
        /// <summary>
        /// 无操作。
        /// </summary>
        None = 0,

        /// <summary>
        /// 开仓或平仓。
        /// </summary>
        OpenOrClose = 1,

        /// <summary>
        /// 开仓对齐。
        /// </summary>
        OpenAlignment = 2,

        /// <summary>
        /// 开仓追单。
        /// </summary>
        OpenChaseOrder = 3,

        /// <summary>
        /// 平仓追单。
        /// </summary>
        CloseChaseOrder = 4,
    }

    public static class AutoTraderWorkTypeExtend
    {
        public static string ToDescription(this AutoTraderWorkType workerType)
        {
            switch(workerType)
            {
                case AutoTraderWorkType.OpenOrClose:return "开平仓";
                case AutoTraderWorkType.OpenAlignment:return "开仓对齐";
                case AutoTraderWorkType.OpenChaseOrder:return "开仓追单";
                case AutoTraderWorkType.CloseChaseOrder:return "平仓追单";
                default:
                    Debug.Assert(false);
                    return "未知类型";
            }
        }
    }

    /// <summary>
    /// 自动交易器状态枚举定义。
    /// </summary>
    public enum AutoTraderState
    {
        /// <summary>
        /// 禁用。
        /// </summary>
        Disable = 0,

        /// <summary>
        /// 启用。
        /// </summary>
        Enable = 1,
    }

    /// <summary>
    /// 自动下单机通知消息类型。
    /// </summary>
    public enum AutoTraderNoticeType
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 委托。
        /// </summary>
        Order = 1,

        /// <summary>
        /// 成交。
        /// </summary>
        Trade = 2,

        /// <summary>
        /// 提示。
        /// </summary>
        Infomation = 3,

        /// <summary>
        /// 其他
        /// </summary>
        Other = 99,
    }

    /// <summary>
    /// 自动下单机通知级别枚举。
    /// </summary>
    public enum AutoTraderNoticeLevel
    {
        /// <summary>
        /// 提示。
        /// </summary>
        Information =0,

        /// <summary>
        /// 警告。
        /// </summary>
        Warning =1,

        /// <summary>
        /// 错误。
        /// </summary>
        Error = 2,
    }
}
