#region Copyright & Version
//==============================================================================
// 文件名称: USeQuoteDriverState.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Justin Shen
// 创建日期: 2012/05/10
// 描    述: 行情驱动状态枚举。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 行情驱动状态枚举
    /// </summary>
    public enum USeQuoteDriverState
    {
        /// <summary>
        /// 无效状态。
        /// </summary>
        Inactive = 0,

        /// <summary>
        /// 已断开。
        /// </summary>
        DisConnected,

        /// <summary>
        /// 连接中。
        /// </summary>
        Connecting,

        /// <summary>
        /// 已连接。
        /// </summary>
        Connected,

        /// <summary>
        /// 登录中。
        /// </summary>
        LoggingOn,

        /// <summary>
        /// 已登录
        /// </summary>
        LoggedOn,

        /// <summary>
        /// 加载中。
        /// </summary>
        Loading,

        /// <summary>
        /// 可用。
        /// </summary>
        Ready,

        /// <summary>
        /// 登出中。
        /// </summary>
        LoggingOut,

        /// <summary>
        /// 已登出。
        /// </summary>
        LoggedOut,
    }
}
