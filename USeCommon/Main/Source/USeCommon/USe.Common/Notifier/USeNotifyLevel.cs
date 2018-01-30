using System;
using System.Diagnostics;

namespace USe.Common
{
    /// <summary>
    /// USe通知事件级别枚举类型定义。
    /// </summary>
    [Flags]
    public enum USeNotifyLevel
    {
        /// <summary>
        /// 关键性错误或应用程序崩溃信息。
        /// </summary>
        Critical = 0X0001,

        /// <summary>
        /// 可恢复的错误信息。
        /// </summary>
        Error = 0X0002,

        /// <summary>
        /// 警告(非关键性问题)信息。
        /// </summary>
        Warning = 0X0004,

        /// <summary>
        /// 一般性信息。
        /// </summary>
        Information = 0X0008,

        /// <summary>
        /// 调试跟踪信息。
        /// </summary>
        Verbose = 0X0010,
    }
}
