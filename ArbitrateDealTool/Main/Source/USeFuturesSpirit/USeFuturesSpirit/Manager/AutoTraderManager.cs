using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using USe.TradeDriver.Common;
using USe.Common;
using USeFuturesSpirit.Arbitrage;

namespace USeFuturesSpirit
{
    #region delegate
    /// <summary>
    /// 新增自动下单机委托。
    /// </summary>
    /// <param name="traderIdentify">下单机标识。</param>
    public delegate void AddAutoTraderEventHandel(Guid traderIdentify);

    /// <summary>
    /// 移除自动下单机委托。
    /// </summary>
    /// <param name="traderIdentify"></param>
    public delegate void RemoveAutoTraderEventHandel(Guid traderIdentify);
    #endregion

    /// <summary>
    /// 自动下单机管理类。
    /// </summary>
    class AutoTraderManager
    {
        #region event
        /// <summary>
        /// 新增自动下单机。
        /// </summary>
        public event AddAutoTraderEventHandel OnAddAutoTrader;

        /// <summary>
        /// 移除自动下单机。
        /// </summary>
        public event RemoveAutoTraderEventHandel OnRemoveAutoTrader;

        /// <summary>
        /// 自动下单机状态变更。
        /// </summary>
        public event AutoTraderStateChangedEventHandle OnAutoTraderStateChanged;

        /// <summary>
        /// 套利单状态发生变更。
        /// </summary>
        public event ArbitrageOrderChangedEventHandle OnArbitrageOrderChanged;

        /// <summary>
        /// 自动下单机通知事件。
        /// </summary>
        public event AutoTraderNotifyEventHandle OnAutoTraderNotify;

        /// <summary>
        /// 自动下单机预警事件。
        /// </summary>
        public event AlarmNoticeEventHandel OnAlarm;

        #endregion

        #region member
        private object m_syncObj = new object(); // 同步对象

        private Dictionary<Guid, AutoTrader> m_autoTraderDic = null;

        private USeOrderDriver m_orderDriver = null;
        private USeQuoteDriver m_quoteDriver = null;
        private AlarmManager m_alarmManager = null;
        private SystemConfigManager m_systemConfigManager = null;

        private CommonIdCreator m_aliasIdCrateor = new CommonIdCreator();
        #endregion

        #region construction
        public AutoTraderManager()
        {
            m_autoTraderDic = new Dictionary<Guid, AutoTrader>();
        }
        #endregion

        #region property
        /// <summary>
        /// 自动下单机数量。
        /// </summary>
        public int TraderCount
        {
            get
            {
                lock (m_syncObj)
                {
                    return m_autoTraderDic.Count;
                }
            }
        }

        /// <summary>
        /// 当前运行下单机数量。
        /// </summary>
        public int RunTraderCount
        {
            get
            {
                lock (m_syncObj)
                {
                    int count = 0;
                    foreach(AutoTrader trader in m_autoTraderDic.Values)
                    {
                        if(trader.State == AutoTraderState.Enable)
                        {
                            count++;
                        }
                    }
                    return count;
                }
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="arbitrageOrderList">套利单信息。</param>
        /// <param name="orderDriver">交易驱动。</param>
        /// <param name="quoteDriver">行情驱动。</param>
        /// <param name="alarmManager">预警管理类。</param>
        /// <param name="systemConfigManager">系统配置管理类。</param>
        public void Initialize(List<USeArbitrageOrder> arbitrageOrderList,USeOrderDriver orderDriver, USeQuoteDriver quoteDriver,AlarmManager alarmManager,SystemConfigManager systemConfigManager)
        {
            m_orderDriver = orderDriver;
            m_quoteDriver = quoteDriver;
            m_alarmManager = alarmManager;
            m_systemConfigManager = systemConfigManager;

            m_systemConfigManager.OnSystemSettingChanged += M_systemConfigManager_OnSystemSettingChanged1;

            if (arbitrageOrderList == null || arbitrageOrderList.Count <= 0) return;

            lock (m_syncObj)
            {
                int maxAliasNum = 0;  // 当日别名最大号码

                TaskOrderSetting taskOrderSetting = m_systemConfigManager.GetTaskOrderSetting();
                foreach (USeArbitrageOrder arbitrageOrder in arbitrageOrderList)
                {
                    AutoTrader autoTrader = new AutoTrader();
                    autoTrader.SetRecordLogger(USeManager.Instance.CommandLogger);
                    autoTrader.SetTryOrderCondition(taskOrderSetting.TaskMaxTryCount, taskOrderSetting.TryOrderMinInterval);
                    autoTrader.Initialize(arbitrageOrder,m_orderDriver,m_quoteDriver,m_alarmManager);
                    autoTrader.OnNotify += AutoTrader_OnNotify;
                    autoTrader.OnAlarm += AutoTrader_OnAlarm;
                    autoTrader.OnStateChanged += AutoTrader_OnStateChanged;
                    autoTrader.OnArbitrageOrderChanged += AutoTrader_OnArbitrageOrderChanged;
                    m_autoTraderDic.Add(autoTrader.TraderIdentify, autoTrader);

                    if(arbitrageOrder.CreateTime.Date == DateTime.Today && arbitrageOrder.AliasNum > maxAliasNum)
                    {
                        maxAliasNum = arbitrageOrder.AliasNum;
                    }
                }

                m_aliasIdCrateor.Set(maxAliasNum);
            }
        }

       

        private void M_systemConfigManager_OnSystemSettingChanged1()
        {
            try
            {
                List<AutoTrader> traderList = null;
                lock (m_syncObj)
                {
                    traderList = m_autoTraderDic.Values.ToList();
                }

                if (traderList != null && traderList.Count > 0)
                {
                    TaskOrderSetting taskOrderSetting = m_systemConfigManager.GetTaskOrderSetting();

                    foreach (AutoTrader trader in traderList)
                    {
                        trader.SetTryOrderCondition(taskOrderSetting.TaskMaxTryCount, taskOrderSetting.TryOrderMinInterval);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        private void M_systemConfigManager_OnSystemSettingChanged()
        {
            throw new NotImplementedException();
        }
        #endregion


        #region public methods
        /// <summary>
        /// 获取自动下单机。
        /// </summary>
        /// <returns></returns>
        public List<AutoTrader> GetAllAutoTrader()
        {
            lock (m_syncObj)
            {
                List<AutoTrader> list = new List<AutoTrader>(m_autoTraderDic.Values);
                return list;
            }
        }

        /// <summary>
        /// 获取自动下单机。
        /// </summary>
        /// <param name="traderIdentify">自动下单机标识。</param>
        /// <returns></returns>
        public AutoTrader GetAutoTrader(Guid traderIdentify)
        {
            lock (m_syncObj)
            {
                AutoTrader trader = null;
                m_autoTraderDic.TryGetValue(traderIdentify, out trader);
                return trader;
            }
        }

        public bool HasUnfinishOrderBook()
        {
            foreach(AutoTrader trader in m_autoTraderDic.Values)
            {
                if(trader.HasUnFinishOrderBook)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 计算当前逃离单占用的保证金。
        /// </summary>
        /// <returns></returns>
        public decimal CalculatUseMargin()
        {
            decimal useMargin = 0m;
            foreach (AutoTrader trader in m_autoTraderDic.Values)
            {
                useMargin += trader.CalculatUseMargin();
            }
            return useMargin;
        }

        /// <summary>
        /// 创建新的套利订单。
        /// </summary>
        /// <param name="argument">套利单参数。</param>
        /// <param name="currentInvestor">当前投资者帐号。</param>
        public AutoTrader CreateNewAutoTrader(ArbitrageArgument argument, InvestorAccount currentInvestor)
        {
            Guid traderIdentify = CreateNewAutoTraderIdentify();
            int aliasNum = CreateNewAutoTraderAliasNum();

            USeArbitrageOrder arbitrageOrder = new USeArbitrageOrder();
            arbitrageOrder.TraderIdentify = traderIdentify;
            arbitrageOrder.AliasNum = aliasNum;
            arbitrageOrder.State = ArbitrageOrderState.None;
            arbitrageOrder.BrokerId = currentInvestor.BrokerId;
            arbitrageOrder.Account = currentInvestor.Account;
            arbitrageOrder.Argument = argument.Clone();
            arbitrageOrder.CreateTime = DateTime.Now;

            TaskOrderSetting taskOrderSetting = m_systemConfigManager.GetTaskOrderSetting();
            //任务组待开仓或者平仓时在创建
            AutoTrader trader = new AutoTrader();
            trader.SetRecordLogger(USeManager.Instance.CommandLogger);
            trader.SetTryOrderCondition(taskOrderSetting.TaskMaxTryCount, taskOrderSetting.TryOrderMinInterval);
            trader.Initialize(arbitrageOrder, m_orderDriver, m_quoteDriver,m_alarmManager);
            
            trader.OnNotify += AutoTrader_OnNotify;
            trader.OnAlarm += AutoTrader_OnAlarm;
            trader.OnArbitrageOrderChanged += AutoTrader_OnArbitrageOrderChanged;
            trader.OnStateChanged += AutoTrader_OnStateChanged;

            lock (m_syncObj)
            {
                m_autoTraderDic.Add(traderIdentify, trader);
            }

            SafeFireAddAutoTrader(traderIdentify);
            string text = "创建套利单成功";
            AutoTraderNotice notice = new AutoTraderNotice(trader.TraderIdentify, trader.Alias, AutoTraderNoticeType.Infomation, text);
            SafeFireAutoTraderNotice(notice);
            trader.WriteTraderNoticeLog(notice);

            return trader;
        }

        /// <summary>
        /// 移除自动下单机。
        /// </summary>
        /// <param name="traderIdentify">自动下单机标识。</param>
        public void RemoveAutoTrader(Guid traderIdentify)
        {
            AutoTrader trader = null;
            lock (m_syncObj)
            {
                if (m_autoTraderDic.TryGetValue(traderIdentify, out trader) == false)
                {
                    Debug.Assert(false);
                    return;
                }
            }

            if(trader.State == AutoTraderState.Enable)
            {
                throw new Exception(string.Format("套利单[{0}]为运行状态，请先停止在移除", trader.Alias));
            }
            if (trader.IsFinish == false)
            {
                throw new Exception(string.Format("套利单[{0}]当前状态为 {1} ,不能移除", trader, trader.ArbitrageOrderState.ToDescription()));
            }
            if (trader.HasUnFinishOrderBook)
            {
                throw new Exception(string.Format("套利单[{0}]有未完成委托单 ,不能移除", trader));
            }

            trader.OnNotify -= AutoTrader_OnNotify;
            trader.OnAlarm -= AutoTrader_OnAlarm;
            trader.OnArbitrageOrderChanged -= AutoTrader_OnArbitrageOrderChanged;

            lock (m_syncObj)
            {
                m_autoTraderDic.Remove(traderIdentify);
            }

            string text = "移除套利单成功";
            AutoTraderNotice notice = new AutoTraderNotice(trader.TraderIdentify, trader.Alias, AutoTraderNoticeType.Infomation, text);
            SafeFireAutoTraderNotice(notice);
            trader.WriteTraderNoticeLog(notice);

            SafeFireRemoveAutoTrader(traderIdentify);
        }
        #endregion

        #region private methods
        /// <summary>
        /// 创建一个新的下单机标识。
        /// </summary>
        /// <returns></returns>
        private Guid CreateNewAutoTraderIdentify()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 创建一个新的下单机别名。
        /// </summary>
        /// <returns></returns>
        private int CreateNewAutoTraderAliasNum()
        {
            return m_aliasIdCrateor.Next();
        }
        #endregion

        #region AutoTrader事件
        private void AutoTrader_OnArbitrageOrderChanged(Guid tradeIdentify)
        {
            SafeFireArbitrageOrderChanged(tradeIdentify);
        }

        private void AutoTrader_OnStateChanged(AutoTraderWorkType workType, AutoTraderState state)
        {
            SafeFireAutoTraderStateChanged(workType, state);
        }

        private void AutoTrader_OnNotify(AutoTraderNotice notice)
        {
            SafeFireAutoTraderNotice(notice);
        }

        private void AutoTrader_OnAlarm(AlarmNotice alarm)
        {
            SafeFireAutoTraderAlarm(alarm);
        }
        #endregion

        #region fire event
        /// <summary>
        /// 触发新增自动下单机事件。
        /// </summary>
        /// <param name="traderIdentify">下单机标识。</param>
        private void SafeFireAddAutoTrader(Guid traderIdentify)
        {
            AddAutoTraderEventHandel handle = this.OnAddAutoTrader;

            if (handle != null)
            {
                try
                {
                    handle(traderIdentify);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 触发移除自动下单机事件。
        /// </summary>
        /// <param name="traderIdentify">下单机标识。</param>
        private void SafeFireRemoveAutoTrader(Guid traderIdentify)
        {
            RemoveAutoTraderEventHandel handle = this.OnRemoveAutoTrader;

            if (handle != null)
            {
                try
                {
                    handle(traderIdentify);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 安全触发报告事件。
        /// </summary>
        /// <param name="notice">通知。</param>
        private void SafeFireAutoTraderNotice(AutoTraderNotice notice)
        {
            AutoTraderNotifyEventHandle handle = this.OnAutoTraderNotify;
            if (handle != null)
            {
                try
                {
                    handle(notice);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 安全触发报告事件。
        /// </summary>
        /// <param name="notice">通知。</param>
        private void SafeFireAutoTraderAlarm(AlarmNotice alarm)
        {
            AlarmNoticeEventHandel handle = this.OnAlarm;
            if (handle != null)
            {
                try
                {
                    handle(alarm);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 安全触发套利单变更事件。
        /// </summary>
        /// <param name="state">通知。</param>
        private void SafeFireArbitrageOrderChanged(Guid tradeIdentify)
        {
            ArbitrageOrderChangedEventHandle handle = this.OnArbitrageOrderChanged;
            if (handle != null)
            {
                try
                {
                    handle(tradeIdentify);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 安全触发套利单状态变更事件。
        /// </summary>
        /// <param name="workType">工作类型。</param>
        /// <param name="state">状态。</param>
        private void SafeFireAutoTraderStateChanged(AutoTraderWorkType workType, AutoTraderState state)
        {
            AutoTraderStateChangedEventHandle handle = this.OnAutoTraderStateChanged;
            if (handle != null)
            {
                try
                {
                    handle(workType, state);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 安全触发报告事件。
        /// </summary>
        /// <param name="tradeIdentity"></param>
        /// <param name="alias"></param>
        /// <param name="noticeType">通知类型。</param>
        /// <param name="message">通知消息。</param>
        private void SafeFireAutoTraderNotice(Guid tradeIdentity, string alias, AutoTraderNoticeType noticeType, string message)
        {
            AutoTraderNotice notice = new AutoTraderNotice(tradeIdentity, alias, noticeType, message);
            SafeFireAutoTraderNotice(notice);
        }
        #endregion

        public override string ToString()
        {
            return "AutoTraderManager";
        }
    }
}
