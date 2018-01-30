#region Copyright & Version
//==============================================================================
// 文件名称: USeOrderDriverState.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 交易驱动类状态枚举定义。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 交易驱动类状态枚举定义。
    /// </summary>
    public enum USeOrderDriverState
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
        /// 认证中
        /// </summary>
        Authoring,

        /// <summary>
        /// 认证失败
        /// </summary>
        AuthorFieldOut,

        /// <summary>
        /// 认证成功
        /// </summary>
        AuthorSuccessOn,

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

    /// <summary>
    /// 交易驱动类状态枚举扩展类。
    /// </summary>
    public static class USeOrderDriverStateExtend
    {
        /// <summary>
        /// 交易驱动状态描述。
        /// </summary>
        /// <param name="state">交易驱动状态。</param>
        /// <returns>描述字符串。</returns>
        public static string ToDescription(this USeOrderDriverState state)
        {
            switch (state)
            {
                case USeOrderDriverState.DisConnected: return "断开";
                case USeOrderDriverState.Connecting: return "连接中";
                case USeOrderDriverState.Connected: return "已连接";
                case USeOrderDriverState.LoggingOn: return "登录中";
                case USeOrderDriverState.LoggedOn: return "已登录";
                case USeOrderDriverState.Loading: return "加载中";
                case USeOrderDriverState.Ready: return "就绪";
                case USeOrderDriverState.LoggingOut: return "登出中";
                case USeOrderDriverState.LoggedOut: return "已登出";
                default: return "";
            }
        }
    }
}
