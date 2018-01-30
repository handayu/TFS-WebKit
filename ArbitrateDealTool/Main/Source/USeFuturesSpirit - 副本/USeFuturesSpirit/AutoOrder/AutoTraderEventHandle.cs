using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 自动下单机运行状态变更委托。
    /// </summary>
    /// <parm name="workType">工作类型。</parm>
    /// <param name="state">下单机状态。</param>
    public delegate void AutoTraderStateChangedEventHandle(AutoTraderWorkType workType,AutoTraderState state);

    /// <summary>
    /// 套利单变更委托。
    /// </summary>
    /// <param name="traderIdentify">下单机标识。</param>
    public delegate void ArbitrageOrderChangedEventHandle(Guid traderIdentify);

    /// <summary>
    /// 自动下单机通知委托。
    /// </summary>
    /// <param name="notice"></param>
    public delegate void AutoTraderNotifyEventHandle(AutoTraderNotice notice);

    /// <summary>
    /// 预警通知委托。
    /// </summary>
    /// <param name="alarm">预警。</param>
    public delegate void AlarmNoticeEventHandel(AlarmNotice alarm);
}
