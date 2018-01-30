using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using USe.Common.AppLogger;
using USe.TradeDriver.Common;
using USeFuturesSpirit.Arbitrage;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 自动下单机。
    /// </summary>
    public partial class AutoTrader
    {
        #region event
        /// <summary>
        /// 自动下单机运行状态变更事件。
        /// </summary>
        public event AutoTraderStateChangedEventHandle OnStateChanged;

        /// <summary>
        /// 自动下单机通知事件。
        /// </summary>
        public event AutoTraderNotifyEventHandle OnNotify;

        /// <summary>
        /// 预警。
        /// </summary>
        public event AlarmNoticeEventHandel OnAlarm;

        /// <summary>
        /// 套利单信息变更事件。
        /// </summary>
        public event ArbitrageOrderChangedEventHandle OnArbitrageOrderChanged;
        #endregion

        #region member
        private static object ms_orderSyncObj = new object();

        private object m_syncObj = new object(); //同步对象
        private const int EVENT_WAIT_Time = 2000; // 信号量等待时间

        private USeOrderDriver m_orderDriver = null;
        private USeQuoteDriver m_quoteDriver = null;
        private TryOrderCondition m_tryOrderCondition = null;

        private USeArbitrageOrder m_arbitrageOrder = null;                     // 套利单信息
        private AutoTraderWorkType m_backgroundWorkerType = AutoTraderWorkType.None;  // 当前下单机后台流程任务类型
        private AutoTraderState m_state = AutoTraderState.Disable;             // 下单机状态

        private bool m_initialized = false;

        private bool m_backgroundRunFlag = false; // 是否运行标识

        private AutoResetEvent m_operatorEvent = new AutoResetEvent(false);  // 流程操作信号量

        private IAppLogger m_recordLogger = new NullLogger("AutoTrader<Null>");
        #endregion

        #region construction
        /// <summary>
        /// 构造方法。
        /// </summary>
        public AutoTrader()
        {
            m_tryOrderCondition = new TryOrderCondition() {
                MaxTryOrderCount = 3,                                           //默认最大尝试次数3次
                NextOrderInterval = new TimeSpan(0, 0, 5)                       // 默认下单间隔5秒
            };
        }
        #endregion

        #region property
        /// <summary>
        /// 自动下单机标识。
        /// </summary>
        public Guid TraderIdentify
        {
            get
            {
                if (m_arbitrageOrder != null)
                {
                    return m_arbitrageOrder.TraderIdentify;
                }
                else
                {
                    Debug.Assert(false);
                    return Guid.Empty;
                }
            }
        }

        /// <summary>
        /// 套利单别名。
        /// </summary>
        public string Alias
        {
            get
            {
                if (m_arbitrageOrder != null)
                {
                    return m_arbitrageOrder.Alias;
                }
                else
                {
                    Debug.Assert(m_arbitrageOrder != null);
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 当前下单机后台流程任务类型。
        /// </summary>
        public AutoTraderWorkType BackgroundWorkerType
        {
            get
            {
                lock (m_syncObj)
                {
                    return m_backgroundWorkerType;
                }
            }
        }

        /// <summary>
        /// 后台工作线程是否忙碌。
        /// </summary>
        public bool BackgroundWorkerIsBusy
        {
            get
            {
                lock (m_syncObj)
                {
                    if (m_worker == null)
                    {
                        return false;
                    }
                    else
                    {
                        return m_worker.IsBusy;
                    }
                }
            }
        }

        /// <summary>
        /// 开平仓流程是否在运行。
        /// </summary>
        public bool OpenCloseWorkerIsRun
        {
            get
            {
                lock (m_syncObj)
                {
                    if (m_worker != null && m_worker.IsBusy &&
                        m_backgroundWorkerType == AutoTraderWorkType.OpenOrClose)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 自动下单机状态。
        /// </summary>
        public AutoTraderState State
        {
            get
            {
                lock (m_syncObj)
                {
                    return m_state;
                }
            }
        }

        /// <summary>
        /// 套利单状态。
        /// </summary>
        public ArbitrageOrderState ArbitrageOrderState
        {
            get
            {
                Debug.Assert(m_arbitrageOrder != null);
                return m_arbitrageOrder.State;
            }
        }

        /// <summary>
        /// 是否交易完成。
        /// </summary>
        public bool IsFinish
        {
            get
            {
                lock (m_syncObj)
                {
                    return (m_arbitrageOrder.State == ArbitrageOrderState.Closed ||
                            m_arbitrageOrder.State == ArbitrageOrderState.Finish);
                }
            }
        }

        /// <summary>
        /// 是否有未完成委托单。
        /// </summary>
        public bool HasUnFinishOrderBook
        {
            get
            {
                lock (m_syncObj)
                {
                    return m_arbitrageOrder.HasUnFinishOrderBook;
                }
            }
        }

        public bool HasAlarm
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// 设定记录日志。
        /// </summary>
        /// <param name="recordLogger"></param>
        public void SetRecordLogger(IAppLogger recordLogger)
        {
            if(recordLogger != null)
            {
                m_recordLogger = recordLogger;
            }
            else
            {
                m_recordLogger = new NullLogger("AutoTrader<Null>");
            }
        }

        /// <summary>
        /// 设置尝试下单条件。
        /// </summary>
        /// <param name="maxTryOrderCount">最大尝试次数。</param>
        /// <param name="nextOrderInterval">下单间隔。</param>
        public void SetTryOrderCondition(int maxTryOrderCount,TimeSpan nextOrderInterval)
        {
            m_tryOrderCondition.MaxTryOrderCount = maxTryOrderCount;
            m_tryOrderCondition.NextOrderInterval = nextOrderInterval;
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化套利下单机。
        /// </summary>
        /// <param name="arbitrageOrder">套利单信息。</param>
        /// <param name="orderDriver">下单驱动。</param>
        /// <param name="quoteDriver">行情驱动。</param>
        /// <param name="alarmManager">预警管理类。</param>
        public void Initialize(USeArbitrageOrder arbitrageOrder,USeOrderDriver orderDriver,USeQuoteDriver quoteDriver,AlarmManager alarmManager)
        {
            if (m_initialized)
            {
                throw new ApplicationException(string.Format("{0} already initialized.", this));
            }

            if(arbitrageOrder == null)
            {
                throw new ArgumentNullException("arbitrageOrder", "arbitrageOrder is null");
            }

            if(orderDriver == null)
            {
                throw new ArgumentNullException("orderDriver", "orderDriver is null");
            }

            if(quoteDriver == null)
            {
                throw new ArgumentNullException("quoteDrvier", "quoteDrvier is null");
            }

            if(alarmManager == null)
            {
                throw new ArgumentNullException("alarmManager", "alarmManager is null");
            }

            try
            {
                lock (m_syncObj)
                {
                    m_arbitrageOrder = arbitrageOrder.Clone();

                    m_orderDriver = orderDriver;
                    m_quoteDriver = quoteDriver;
                }

                m_orderDriver.OnOrderBookChanged += OrderDriver_OnOrderBookChanged;
                m_quoteDriver.OnMarketDataChanged += QuoteDriver_OnMarketDataChanged;

                if(arbitrageOrder.OpenArgument !=null)
                {
                    m_quoteDriver.Subscribe(arbitrageOrder.OpenArgument.BuyInstrument);
                    m_quoteDriver.Subscribe(arbitrageOrder.OpenArgument.SellInstrument);
                }

                string text = string.Format("初始化套利单完成,当前状态为[{0}]", arbitrageOrder.State.ToDescription());
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
            catch(Exception ex)
            {
                string text = string.Format("初始化套利单失败,原因:{0}", ex.Message);
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);

                throw new Exception("初始化自定下单机失败");
            }
            m_initialized = true;
        }

        #endregion

        public ArbitrageArgument GetArgument()
        {
            return m_arbitrageOrder.Argument.Clone();
        }

        #region 设定开仓参数
        /// <summary>
        /// 设定开仓参数。
        /// </summary>
        /// <param name="openArg">开仓参数。</param>
        public void SetOpenArgument(ArbitrageOpenArgument openArg)
        {
            throw new NotSupportedException("不支持开仓参数设定");
        }
        #endregion

        #region 设定平仓参数
        /// <summary>
        /// 设定平仓参数。
        /// </summary>
        /// <param name="closeArg">平仓参数。</param>
        public void ModifyArbitrageArgument(ArbitrageArgument argument)
        {
            CheckBackgroundWorker("修改套利单参数");

            lock (m_syncObj)
            {
                if (m_arbitrageOrder.State >= ArbitrageOrderState.Closeing)
                {
                    throw new Exception(string.Format("{0}为{1}状态,不能修改套利单参数", this, this.ArbitrageOrderState.ToDescription()));
                }
                string errorMessage = string.Empty;
                if (VerfiyArbitrageArgument(argument, out errorMessage) == false)
                {
                    string text = string.Format("{0}修改套利单参数失败,{1}", this, errorMessage);
                    throw new ArgumentException(text);
                }

                //[yangming]更新能修改的参数
                m_arbitrageOrder.Argument = argument.Clone();
                //m_arbitrageOrder.CloseArgument = closeArg.Clone();
            }
        }
        #endregion

        #region 进入开仓流程
        /// <summary>
        /// 设定流程进入开仓流程。
        /// </summary>
        public void BeginOpen()
        {
            lock (m_syncObj)
            {
                if (m_arbitrageOrder.OpenArgument == null)
                {
                    throw new Exception(string.Format("{0}开仓参数未设定",this));
                }

                if (m_arbitrageOrder.State != ArbitrageOrderState.None)
                {
                    throw new Exception(string.Format("{0}为{1}状态,不能进入开仓状态", this, m_arbitrageOrder.State.ToDescription()));
                }

                string errorMessage = string.Empty;
                if (VerfiyOpenArgument(m_arbitrageOrder.OpenArgument, out errorMessage) == false)
                {
                    throw new Exception(string.Format("{0}开仓参数错误,{1}", this, errorMessage));
                }

                //创建开仓任务组
                ArbitrageTaskGroup openTaskGroup = CreateOpenTaskGroup(m_arbitrageOrder.Argument);

                m_arbitrageOrder.OpenTaskGroup = openTaskGroup;
                m_arbitrageOrder.State = ArbitrageOrderState.Opening;
            }

            m_operatorEvent.Set();

            SafeFireArbitrageOrderChanged();

            string text = "进入开仓流程";
            AutoTraderNotice notice = CreateTraderNotice(text);
            SafeFireAutoTraderNotice(notice);
            WriteTraderNoticeLog(notice);
        }
        #endregion

        #region 进入平仓流程
        /// <summary>
        /// 开始平仓。
        /// </summary>
        public void BeginClose()
        {
            lock (m_syncObj)
            {
                Debug.Assert(m_arbitrageOrder.Argument != null);
                Debug.Assert(m_arbitrageOrder.Argument.CloseArg != null);
                //Debug.Assert(m_arbitrageOrder.Argument.StopLossArg != null);
                
                if (m_arbitrageOrder.State != ArbitrageOrderState.Opened)
                {
                    throw new Exception(string.Format("{0}为{1}状态,不能进入平仓状态", this, m_arbitrageOrder.State.ToDescription()));
                }
                string errorMessage = string.Empty;
                if (VerfiyArbitrageArgument(m_arbitrageOrder.Argument, out errorMessage) == false)
                {
                    throw new Exception(string.Format("{0}平仓参数错误,{1}", this, errorMessage));
                }

                //创建平仓任务组
                ArbitrageTaskGroup closeTaskGroup = CreateCloseTaskGroup(m_arbitrageOrder.OpenTaskGroup, m_arbitrageOrder.Argument);

                m_arbitrageOrder.CloseTaskGroup = closeTaskGroup;
                m_arbitrageOrder.State = ArbitrageOrderState.Closeing;
            }

            m_operatorEvent.Set();

            SafeFireArbitrageOrderChanged();

            string text = "进入平仓流程";
            AutoTraderNotice notice = CreateTraderNotice(text);
            SafeFireAutoTraderNotice(notice);
            WriteTraderNoticeLog(notice);
        }
        #endregion

        #region 强制开仓完成
        /// <summary>
        /// 强制开仓完成。
        /// </summary>
        public void ForceOpenFinish()
        {
            CheckBackgroundWorker("强制开仓完成");

            lock (m_syncObj)
            {
                if (m_arbitrageOrder.HasUnFinishOrderBook)
                {
                    throw new Exception(string.Format("{0}有未完成委托单", this));
                }

                if (m_arbitrageOrder.State != ArbitrageOrderState.Opening)
                {
                    throw new Exception(string.Format("{0}为{1}状态,不能强制开仓完成", this, this.ArbitrageOrderState.ToDescription()));
                }

                m_arbitrageOrder.State = ArbitrageOrderState.Opened;
            }

            SafeFireArbitrageOrderChanged();

            {
                string text = "强制开仓完成";
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
        }
        #endregion

        #region 强制平仓完成
        /// <summary>
        /// 强制平仓完成。
        /// </summary>
        public void ForceCloseFinish()
        {
            CheckBackgroundWorker("强制平仓完成");

            lock (m_syncObj)
            {
                if (m_arbitrageOrder.HasUnFinishOrderBook)
                {
                    throw new Exception(string.Format("{0}有未完成委托单", this));
                }

                if (m_arbitrageOrder.State != ArbitrageOrderState.Closeing)
                {
                    throw new Exception(string.Format("{0}为{1}状态,不能强制设定为平仓完成", this, this.ArbitrageOrderState.ToDescription()));
                }

                m_arbitrageOrder.State = ArbitrageOrderState.Closed;
            }

            SafeFireArbitrageOrderChanged();

            {
                string text = "强制平仓完成";
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
        }
        #endregion

        #region 开平仓监控
        /// <summary>
        /// 启动自动下单机开平仓监控。
        /// </summary>
        public void StartOpenOrCloseMonitor()
        {
            CheckBackgroundWorker("开平仓监控");

            Debug.Assert(m_arbitrageOrder != null);
            Debug.Assert(m_worker == null);

            try
            {
                m_worker = new BackgroundWorker();
                m_worker.ProgressChanged += WorkerForOpenAndCloseProgressChanged;
                m_worker.RunWorkerCompleted += WorkerForOpenAndCloseCompleted;
                m_worker.DoWork += WorkerDoWorkForOpenAndClose;

                m_backgroundRunFlag = true;
                m_backgroundWorkerType = AutoTraderWorkType.OpenOrClose;

                m_worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                m_backgroundRunFlag = false;
                m_backgroundWorkerType = AutoTraderWorkType.None;

                m_worker.ProgressChanged -= WorkerForOpenAndCloseProgressChanged;
                m_worker.RunWorkerCompleted -= WorkerForOpenAndCloseCompleted;
                m_worker.DoWork -= WorkerDoWorkForOpenAndClose;

                m_worker = null;
                Debug.Assert(false, ex.Message);
                throw new Exception(string.Format("{0}启动开平仓监控失败,{1}", this, ex.Message));
            }

            //SafeFireAutoTraderStateChanged(AutoTraderWorkType.OpenOrClose, AutoTraderState.Enable);
        }

        /// <summary>
        /// 停止开平仓监控。
        /// </summary>
        public void StopOpenOrCloseMonitor()
        {
            if (m_backgroundWorkerType != AutoTraderWorkType.OpenOrClose ||
                m_backgroundRunFlag == false)
            {
                string text = "当前未进行开平仓监控";
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
                return;
            }

            {
                string text = "停止开平仓监控";
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
            lock (m_syncObj)
            {
                if (m_backgroundWorkerType == AutoTraderWorkType.OpenOrClose)
                {
                    m_backgroundRunFlag = false;
                }
            }

            m_operatorEvent.Reset();
        }
        #endregion

        #region 开仓仓位对齐
        /// <summary>
        /// 开仓对齐。
        /// </summary>
        public void OpenAlignment()
        {
            CheckBackgroundWorker("开仓对齐");

            Debug.Assert(m_arbitrageOrder != null);
            Debug.Assert(m_worker == null);

            try
            {
                m_worker = new BackgroundWorker();
                m_worker.DoWork += WorkerDoWorkForOpenAlignment;
                m_worker.ProgressChanged += WorkerForOpenAlignmentProgressChanged;
                m_worker.RunWorkerCompleted += WorkerForOpenAlignmentCompleted;

                m_backgroundRunFlag = true;
                m_backgroundWorkerType = AutoTraderWorkType.OpenAlignment;

                m_worker.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                m_backgroundRunFlag = false;
                m_backgroundWorkerType = AutoTraderWorkType.None;

                m_worker.DoWork -= WorkerDoWorkForOpenAlignment;
                m_worker.ProgressChanged -= WorkerForOpenAlignmentProgressChanged;
                m_worker.RunWorkerCompleted -= WorkerForOpenAlignmentCompleted;

                m_worker = null;
                Debug.Assert(false, ex.Message);
                throw new Exception(string.Format("{0}启动开仓对齐失败,{1}", this, ex.Message));
            }

            SafeFireAutoTraderStateChanged(AutoTraderWorkType.OpenAlignment, AutoTraderState.Enable);
        }

        /// <summary>
        /// 停止开仓对齐。
        /// </summary>
        public void StopOpenAlignment()
        {
            if (m_backgroundWorkerType != AutoTraderWorkType.OpenAlignment ||
                m_backgroundRunFlag == false)
            {
                string text = "当前未进行开仓对齐操作";
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
                return;
            }

            {
                string text = "停止开仓对齐";
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
            lock (m_syncObj)
            {
                if (m_backgroundWorkerType == AutoTraderWorkType.OpenAlignment)
                {
                    m_backgroundRunFlag = false;
                }
            }

            m_operatorEvent.Reset();
        }
        #endregion

        #region 开仓追单
        /// <summary>
        /// 开仓追单对齐。
        /// </summary>
        /// <remarks>
        /// 1.优先合约未成交单撤单
        /// 2.反手合约未成交单撤单
        /// 3.反手合约以市价委托持仓仓差。
        /// </remarks>
        public void OpenChaseOrder()
        {
            CheckBackgroundWorker("开仓追单");

            Debug.Assert(m_arbitrageOrder != null);
            Debug.Assert(m_worker == null);

            try
            {
                m_worker = new BackgroundWorker();
                m_worker.ProgressChanged += WorkerForOpenChaseOrderProgressChanged;
                m_worker.RunWorkerCompleted += WorkerForOpenChaseOrderCompleted;
                m_worker.DoWork += WorkerDoWorkForOpenChaseOrder;

                m_backgroundRunFlag = true;
                m_backgroundWorkerType = AutoTraderWorkType.OpenChaseOrder;

                m_worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                m_backgroundRunFlag = false;
                m_backgroundWorkerType = AutoTraderWorkType.None;

                m_worker.ProgressChanged -= WorkerForOpenChaseOrderProgressChanged;
                m_worker.RunWorkerCompleted -= WorkerForOpenChaseOrderCompleted;
                m_worker.DoWork -= WorkerDoWorkForOpenChaseOrder;

                m_worker = null;
                Debug.Assert(false, ex.Message);
                throw new Exception(string.Format("{0}启动开仓追单失败,{1}", this, ex.Message));
            }

            SafeFireAutoTraderStateChanged(AutoTraderWorkType.OpenChaseOrder, AutoTraderState.Enable);
        }

        /// <summary>
        /// 停止开仓追单对齐。
        /// </summary>
        public void StopOpenChaseOrder()
        {
            if (m_backgroundWorkerType != AutoTraderWorkType.OpenChaseOrder ||
                m_backgroundRunFlag == false)
            {
                string text = "当前未进行开仓追单";
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
                return;
            }

            {
                string text = "停止开仓追单";
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }

            lock (m_syncObj)
            {
                if (m_backgroundWorkerType == AutoTraderWorkType.OpenChaseOrder)
                {
                    m_backgroundRunFlag = false;
                }
            }

            m_operatorEvent.Reset();
        }
        #endregion

        #region 平仓追单
        /// <summary>
        /// 平仓追单对齐。
        /// </summary>
        /// <remarks>
        /// 1.优先合约未成交单撤单
        /// 2.反手合约未成交单撤单
        /// 3.反手合约以市价委托持仓仓差。
        /// </remarks>
        public void CloseChaseOrder()
        {
            CheckBackgroundWorker("平仓追单");

            Debug.Assert(m_arbitrageOrder != null);
            Debug.Assert(m_worker == null);

            try
            {
                m_worker = new BackgroundWorker();
                m_worker.ProgressChanged += WorkerForCloseChaseOrderProgressChanged;
                m_worker.RunWorkerCompleted += WorkerForCloseChaseOrderCompleted;
                m_worker.DoWork += WorkerDoWorkForCloseChaseOrder;

                m_backgroundRunFlag = true;
                m_backgroundWorkerType = AutoTraderWorkType.CloseChaseOrder;
                
                m_worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                m_backgroundRunFlag = false;
                m_backgroundWorkerType = AutoTraderWorkType.None;

                m_worker.ProgressChanged -= WorkerForCloseChaseOrderProgressChanged;
                m_worker.RunWorkerCompleted -= WorkerForCloseChaseOrderCompleted;
                m_worker.DoWork -= WorkerDoWorkForCloseChaseOrder;

                m_worker = null;
                Debug.Assert(false, ex.Message);
                throw new Exception(string.Format("{0}启动平仓追单失败,{1}", this, ex.Message));
            }

            SafeFireAutoTraderStateChanged(AutoTraderWorkType.CloseChaseOrder, AutoTraderState.Enable);
        }

        /// <summary>
        /// 停止平仓追单对齐。
        /// </summary>
        public void StopCloseChaseOrder()
        {
            if (m_backgroundWorkerType != AutoTraderWorkType.CloseChaseOrder ||
                m_backgroundRunFlag == false)
            {
                string text = "当前未进行平仓追单";
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
                return;
            }

            {
                string text = "停止平仓追单";
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }

            lock (m_syncObj)
            {
                if (m_backgroundWorkerType == AutoTraderWorkType.CloseChaseOrder)
                {
                    m_backgroundRunFlag = false;
                }
            }

            m_operatorEvent.Reset();
        }
        #endregion

        #region 转移为历史记录
        /// <summary>
        /// 转移为历史记录(不在更改)。
        /// </summary>
        public void TransferToHistoryArbitrage()
        {
            lock (m_syncObj)
            {
                if (m_arbitrageOrder.State != ArbitrageOrderState.Closed)
                {
                    throw new Exception(string.Format("{0}为{1}状态,未结束不能转移", this, this.ArbitrageOrderState.ToDescription()));
                }
            }

            ArbitrageOrderSettlement settlemetResult = CalculateSettlementResult();
            lock (m_syncObj)
            {
                m_arbitrageOrder.State = ArbitrageOrderState.Finish;
                m_arbitrageOrder.FinishTime = DateTime.Now;
                m_arbitrageOrder.SettlementResult = settlemetResult;
            }

            SafeFireArbitrageOrderChanged();

            string text = "交易结束,转移到历史记录";
            AutoTraderNotice notice = CreateTraderNotice(text);
            SafeFireAutoTraderNotice(notice);
            WriteTraderNoticeLog(notice);
        }

        private ArbitrageOrderSettlement CalculateSettlementResult()
        {
            USeArbitrageOrder arbitrageOrder = null;
            lock (m_syncObj)
            {
                arbitrageOrder = m_arbitrageOrder.Clone();
            }

            List<USeOrderBook> orderBookList = arbitrageOrder.GetAllOrderBooks();

            USeInstrument buyInstrument = arbitrageOrder.OpenArgument.BuyInstrument;
            USeInstrument sellInstrument = arbitrageOrder.OpenArgument.SellInstrument;

            USeMarketData buyMarketData = USeManager.Instance.QuoteDriver.Query(buyInstrument);
            USeMarketData sellMarketData = USeManager.Instance.QuoteDriver.Query(sellInstrument);
            USeInstrumentDetail buyInstrumentDetail = USeManager.Instance.OrderDriver.QueryInstrumentDetail(buyInstrument);
            USeInstrumentDetail sellInstrumentDetail = USeManager.Instance.OrderDriver.QueryInstrumentDetail(sellInstrument);

            decimal buyProfit = CalculateProfit(orderBookList, buyInstrumentDetail, buyMarketData);
            decimal sellProfit = CalculateProfit(orderBookList, sellInstrumentDetail, sellMarketData);
            decimal totalProfit = buyProfit + sellProfit;

            ArbitrageOrderSettlement settlemt = new ArbitrageOrderSettlement()
            {
                BuyInstrumentProfit = buyProfit,
                SellInstrumentProfit = sellProfit,
                Profit = totalProfit
            };

            return settlemt;
        }

        private decimal CalculateProfit(List<USeOrderBook> orderBookList, USeInstrumentDetail instrumentDetail, USeMarketData marketData)
        {
            Debug.Assert(instrumentDetail.Instrument.Equals(marketData.Instrument));
            if (orderBookList == null || orderBookList.Count < 0) return 0m;

            List<USeOrderBook> list = (orderBookList.Where(o => o.Instrument.Equals(instrumentDetail.Instrument))).ToList();

            int buyQty = 0;
            int sellQty = 0;
            decimal buyAmount = 0m;
            decimal sellAmount = 0m;
            foreach (USeOrderBook orderBook in list)
            {
                if (orderBook.OrderSide == USeOrderSide.Buy)
                {
                    buyQty += orderBook.TradeQty;
                    buyAmount += orderBook.TradeAmount;
                }
                else if (orderBook.OrderSide == USeOrderSide.Sell)
                {
                    sellQty += orderBook.TradeQty;
                    sellAmount += orderBook.TradeAmount;
                }
            }

            if (marketData.LastPrice <= 0)
            {
                //没行情暂时不计算
                return 0m;
            }
            else
            {
                decimal profit = (buyQty - sellQty) * marketData.LastPrice * instrumentDetail.VolumeMultiple + sellAmount - buyAmount;
                return profit;
            }
        }
        #endregion

        #region 套利单委托撤单
        /// <summary>
        /// 撤销挂单。
        /// </summary>
        public void CancelOpenHangingOrder()
        {
            CheckBackgroundWorker("未成交撤单");

            if (m_arbitrageOrder.HasUnFinishOrderBook == false)
            {
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, "没有未完成委托单，无需撤单");
                SafeFireAutoTraderNotice(notice);
                return;
            }
            else
            {
                string text = "正在进行撤单操作";
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }

            //未完成单先撤单
            foreach (ArbitrageTask task in m_arbitrageOrder.OpenTaskGroup.TaskList)
            {
                if (task.HasUnFinishOrderBook)
                {
                    List<USeOrderBook> unFinishOrderBooks = task.UnFinishOrderBooks;
                    foreach (USeOrderBook orderBook in unFinishOrderBooks)
                    {
                        string message = string.Empty;
                        bool cancelResult = m_orderDriver.CancelOrder(orderBook.OrderNum, orderBook.Instrument, out message);
                        if (cancelResult == false)
                        {
                            AutoTraderNotice errorNotice = CreateTraderNotice(AutoTraderNoticeType.Order, "撤单失败");
                            SafeFireAutoTraderNotice(errorNotice);
                        }
                    }
                }
            }

            {
                AutoTraderNotice notice = CreateTraderNotice("撤单完成,等待撤单结果");
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
        }

        /// <summary>
        /// 撤销挂单。
        /// </summary>
        public void CancelCloseHangingOrder()
        {
            CheckBackgroundWorker("未成交撤单");

            if (m_arbitrageOrder.HasUnFinishOrderBook == false)
            {
                AutoTraderNotice notice = CreateTraderNotice(AutoTraderNoticeType.Order, "没有未完成委托单，无需撤单");
                SafeFireAutoTraderNotice(notice);
                return;
            }
            else
            {
                string text = "正在进行撤单操作";
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }

            //未完成单先撤单
            foreach (ArbitrageTask task in m_arbitrageOrder.CloseTaskGroup.TaskList)
            {
                if (task.HasUnFinishOrderBook)
                {
                    List<USeOrderBook> unFinishOrderBooks = task.UnFinishOrderBooks;
                    foreach (USeOrderBook orderBook in unFinishOrderBooks)
                    {
                        string message = string.Empty;
                        bool cancelResult = m_orderDriver.CancelOrder(orderBook.OrderNum, orderBook.Instrument, out message);
                        if (cancelResult == false)
                        {
                            AutoTraderNotice errorNotice = CreateTraderNotice(AutoTraderNoticeType.Order, "撤单失败");
                            SafeFireAutoTraderNotice(errorNotice);
                        }
                    }
                }
            }

            {
                AutoTraderNotice notice = CreateTraderNotice("撤单完成,等待撤单结果");
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);
            }
        }
        #endregion

        /// <summary>
        /// 检查后台流程是否在运行。
        /// </summary>
        /// <returns></returns>
        private bool CheckBackgroundWorker(string action)
        {
            if (m_initialized == false)
            {
                throw new ApplicationException(string.Format("{0}未初始化", this));
            }

            if (this.BackgroundWorkerIsBusy)
            {
                string text = string.Format("{0}正在进行{1}操作,请先停止后台流程在进行{2}操作", this, this.BackgroundWorkerType.ToDescription(), action);
                AutoTraderNotice notice = CreateTraderNotice(text);
                SafeFireAutoTraderNotice(notice);
                WriteTraderNoticeLog(notice);

                throw new ApplicationException(text);
            }

            return true;
        }
        

        #region 公共查询方法
        /// <summary>
        /// 获取套利订单信息。
        /// </summary>
        /// <returns></returns>
        public USeArbitrageOrder GetArbitrageOrder()
        {
            lock (m_syncObj)
            {
                return m_arbitrageOrder.Clone();
            }
        }
        #endregion

        #region 触发事件
        /// <summary>
        /// 触发状态变更事件。
        /// </summary>
        /// <param name="workType">工作类型。</param>
        /// <param name="newState">新状态。</param>
        private void SafeFireAutoTraderStateChanged(AutoTraderWorkType workType, AutoTraderState newState)
        {
            m_state = newState;
            AutoTraderStateChangedEventHandle handle = this.OnStateChanged;

            if(handle != null)
            {
                try
                {
                    handle(workType, newState);
                }
                catch(Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 安全触发下单机通知事件。
        /// </summary>
        /// <param name="noticeType">通知类型。</param>
        /// <param name="message">通知消息。</param>
        private void SafeFireAutoTraderNotice(AutoTraderNotice notice)
        {
            AutoTraderNotifyEventHandle handle = this.OnNotify;

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
        /// 安全触发下单机通知事件。
        /// </summary>
        /// <param name="alarm">预警通知。</param>
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
        private void SafeFireArbitrageOrderChanged()
        {
            Guid tradeIdentity = this.TraderIdentify;
            ArbitrageOrderChangedEventHandle handle = this.OnArbitrageOrderChanged;

            if (handle != null)
            {
                try
                {
                    handle(tradeIdentity);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }
        #endregion

        /// <summary>
        /// 计算当前套利单占用的保证金。
        /// </summary>
        /// <returns></returns>
        public decimal CalculatUseMargin()
        {
            ArbitrageOpenArgument openArg = null;
            lock(m_syncObj)
            {
                if(m_arbitrageOrder.State == ArbitrageOrderState.Finish ||
                    m_arbitrageOrder.State == ArbitrageOrderState.Closed)
                {
                    //平仓完成套利单不占用保证金
                    return 0m;
                }
                openArg = m_arbitrageOrder.OpenArgument.Clone();
            }

            USeOrderDriver orderDriver = USeManager.Instance.OrderDriver;
            USeQuoteDriver quoteDriver = USeManager.Instance.QuoteDriver;

            USeInstrumentDetail buyInstrumentDetail = orderDriver.QueryInstrumentDetail(openArg.BuyInstrument);
            USeInstrumentDetail sellInstrumentDetail = orderDriver.QueryInstrumentDetail(openArg.SellInstrument);
            USeMarketData buyMarketData = quoteDriver.Query(openArg.BuyInstrument);
            USeMarketData sellMarketData = quoteDriver.Query(openArg.SellInstrument);
            USeMargin buyMarginRate = orderDriver.QueryInstrumentMargin(openArg.BuyInstrument);
            USeMargin sellMarginRate = orderDriver.QueryInstrumentMargin(openArg.SellInstrument);

            decimal buyMargin = (openArg.TotalOrderQty * buyMarginRate.BrokerLongMarginRatioByVolume) +
                               (buyMarketData.LastPrice * openArg.TotalOrderQty * buyInstrumentDetail.VolumeMultiple * buyMarginRate.BrokerLongMarginRatioByMoney);
            decimal sellMargin = (openArg.TotalOrderQty * sellMarginRate.BrokerShortMarginRatioByVolume) +
                               (sellMarketData.LastPrice * openArg.TotalOrderQty * sellInstrumentDetail.VolumeMultiple * sellMarginRate.BrokerShortMarginRatioByMoney);

            if (openArg.BuyInstrument.Market == USeMarket.SHFE && openArg.SellInstrument.Market == USeMarket.SHFE)
            {
                return Math.Max(buyMargin, sellMargin);
            }
            else
            {
                return (buyMargin + sellMargin);
            }
        }

        #region RecordLogger 写入方法
        /// <summary>
        /// 记录通知日志。
        /// </summary>
        /// <param name="notice"></param>
        public void WriteTraderNoticeLog(AutoTraderNotice notice)
        {
            string text = string.Format("[Notice][Alias:{1}][NoticeType:{2}][Message:{3}][TradeIdentity:{0}]",
                 notice.TradeIdentity, notice.Alias, notice.NoticeType, notice.Message);

            m_recordLogger.WriteInformation(text);
        }

        /// <summary>
        /// 记录错误信息。
        /// </summary>
        /// <param name="message">日志消息。</param>
        private void WriteTrderErrorLog(string message)
        {
            string text = string.Format("[Log][Alias:{0}][Message:{1}][TradeIdentity:{2}]",
                this.Alias, message,this.TraderIdentify);
            m_recordLogger.WriteError(text);
        }

        /// <summary>
        /// 记录提示信息。
        /// </summary>
        /// <param name="message">日志消息。</param>
        private void WriteTrderInformationLog(string message)
        {
            string text = string.Format("[Log][Alias:{0}][Message:{1}][TradeIdentity:{2}]",
                this.Alias, message, this.TraderIdentify);
            m_recordLogger.WriteInformation(text);
        }

        /// <summary>
        /// 记录警告信息。
        /// </summary>
        /// <param name="message">日志消息。</param>
        private void WriteTraderWarningLog(string message)
        {
            string text = string.Format("[Log][Alias:{0}][Message:{1}][TradeIdentity:{2}]",
               this.Alias, message, this.TraderIdentify);
            m_recordLogger.WriteWarning(text);
        }

        /// <summary>
        /// 打印Debug调试信息。
        /// </summary>
        /// <param name="message"></param>
        private void WriteTraderDebugInfo(string message)
        {
            Debug.WriteLine("==>{0:yyyy-MM-dd HH:mm:ss.fff} [{1}] {2}", DateTime.Now, this.Alias, message);
        }
        #endregion

        #region 创建通知对象
        private AutoTraderNotice CreateTraderNotice(string message)
        {
            AutoTraderNotice notice = new AutoTraderNotice(this.TraderIdentify, this.Alias, AutoTraderNoticeType.Infomation, message);
            return notice;
        }

        private AutoTraderNotice CreateTraderNotice(AutoTraderNoticeType noticeType, string message)
        {
            AutoTraderNotice notice = new AutoTraderNotice(this.TraderIdentify, this.Alias, noticeType, message);
            return notice;
        }
        #endregion

        public override string ToString()
        {
            if (m_arbitrageOrder == null || string.IsNullOrEmpty(m_arbitrageOrder.Alias))
            {
                return "套利单<Null>";
            }
            else
            {
                return string.Format("套利单<{0}>", m_arbitrageOrder.Alias);
            }
        }
    }
}
