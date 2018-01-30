using System;

namespace USe.Common
{
    /// <summary>
	/// USe通知事件接口。
	/// </summary>
	public interface IUSeNotifier
    {
        /// <summary>
        /// 通知事件。
        /// </summary>
        event EventHandler<USeNotifyEventArgs> Notify;
    }
}
