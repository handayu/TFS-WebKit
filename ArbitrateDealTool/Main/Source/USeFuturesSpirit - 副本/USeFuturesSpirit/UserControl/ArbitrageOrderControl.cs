using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;
using USe.Common;
using System.Diagnostics;
using System.Threading;
using USeFuturesSpirit.Arbitrage;

namespace USeFuturesSpirit
{
    public partial class ArbitrageOrderControl : USeUserControl
    {
        #region member
        private System.Threading.Timer m_porfitCalcTimer = null;   // 损益计算定时器

        private AutoTrader m_autoTrader = null;             // 自动下单机
        private USeArbitrageOrder m_arbitrageOrder = null;  // 下单机信息
        private ArbitrageOrderTradeView m_chartView = new ArbitrageOrderTradeView();  // 进度绘图

        private DataTable m_taskDT = null; // 任务表

        private const string OPEN_TASK = "OPEN";
        private const string CLOSE_TASK = "CLOSE";
        #endregion

        #region construction
        public ArbitrageOrderControl()
        {
            InitializeComponent();
        }

        public ArbitrageOrderControl(AutoTrader autoTrader)
        {
            m_porfitCalcTimer = new System.Threading.Timer(CalculateProfitCallback, false, Timeout.Infinite, Timeout.Infinite);
            m_autoTrader = autoTrader;
            InitializeComponent();
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
                if (m_autoTrader == null)
                {
                    return Guid.Empty;
                }
                else
                {
                    return m_autoTrader.TraderIdentify;
                }
            }
        }
        #endregion

        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize()
        {
            Debug.Assert(m_autoTrader != null);
            m_autoTrader.OnStateChanged += M_autoTrader_OnStateChanged;
            m_autoTrader.OnArbitrageOrderChanged += M_autoTrader_OnArbitrageOrderChanged;
            m_autoTrader.OnAlarm += M_autoTrader_OnAlarm;
            m_arbitrageOrder = m_autoTrader.GetArbitrageOrder();

            ResetContextMenu();
            SetChartViewData(m_arbitrageOrder);

            ShowArbitrageOrderView();
            ShowAlarmIcon();
            ShowChartView();
            ShowTaskGrid();
            ShowRunButton();

            m_porfitCalcTimer.Change(500, Timeout.Infinite);
        }


        public void Stop()
        {
            m_porfitCalcTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        #region autoTrader事件
        private void M_autoTrader_OnStateChanged(AutoTraderWorkType workType, AutoTraderState state)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AutoTraderStateChangedEventHandle(M_autoTrader_OnStateChanged), workType, state);
                return;
            }

            ShowRunButton();
        }

        private void M_autoTrader_OnArbitrageOrderChanged(Guid orderIdentify)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ArbitrageOrderChangedEventHandle(M_autoTrader_OnArbitrageOrderChanged), orderIdentify);
                return;
            }

            Debug.Assert(orderIdentify == m_autoTrader.TraderIdentify);
            m_arbitrageOrder = m_autoTrader.GetArbitrageOrder();
            ResetContextMenu();
            SetChartViewData(m_arbitrageOrder);

            ShowArbitrageOrderView();
            ShowChartView();
            ShowTaskGrid();
            ShowRunButton();
        }


        private void M_autoTrader_OnAlarm(AlarmNotice alarm)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new AlarmNoticeEventHandel(M_autoTrader_OnAlarm), alarm);
                return;
            }

            ShowAlarmIcon();
        }
        #endregion

        private void ResetContextMenu()
        {
            if (m_arbitrageOrder.State <= ArbitrageOrderState.Opened)
            {
                ContextMenuStrip menu = this.pbxTaskChart.ContextMenuStrip;
                if (menu == null || menu != this.cmsOpenMenu)
                {
                    this.gridTask.ContextMenuStrip = this.cmsOpenMenu;
                    this.pbxTaskChart.ContextMenuStrip = this.cmsOpenMenu;
                    this.panelTop.ContextMenuStrip = this.cmsOpenMenu;
                }
            }
            else
            {
                ContextMenuStrip menu = this.pbxTaskChart.ContextMenuStrip;
                if (menu == null || menu != this.cmsCloseMenu)
                {
                    this.gridTask.ContextMenuStrip = this.cmsCloseMenu;
                    this.pbxTaskChart.ContextMenuStrip = this.cmsCloseMenu;
                    this.panelTop.ContextMenuStrip = this.cmsCloseMenu;
                }
            }
        }

        /// <summary>
        /// 设置图形区域数据源。
        /// </summary>
        /// <param name="arbitrageOrder"></param>
        private void SetChartViewData(USeArbitrageOrder arbitrageOrder)
        {
            switch (arbitrageOrder.State)
            {
                case ArbitrageOrderState.None:
                case ArbitrageOrderState.Opening:
                case ArbitrageOrderState.Opened:
                    m_chartView.SetData(arbitrageOrder.OpenTaskGroup);
                    break;
                case ArbitrageOrderState.Closeing:
                case ArbitrageOrderState.Closed:
                case ArbitrageOrderState.Finish:
                    m_chartView.SetData(arbitrageOrder.CloseTaskGroup);
                    break;
            }
        }

        public void UpdateUI()
        {
            try
            {
                ShowArbitrageOrderView();
                ShowAlarmIcon();
                ShowChartView();
                ShowTaskGrid();
                ShowRunButton();
            }
            catch (Exception ex)
            {
                //Debug.Assert(false, ex.Message);
            }
        }

        /// <summary>
        /// 清空界面。
        /// </summary>
        private void ClearView()
        {
            this.lblAlias.Text = "MMDD-XXX";
            this.lblBuyInstrument.Text = string.Empty;
            this.lblBuyTradeInfo.Text = "0/0";
            this.lblBuyInstrumentProfit.Text = "0";
            this.lblBuyInstrmentOpenAvgPrice.Text = "0";
            this.lblBuyInstrumentProfit.Text = "0";

            this.lblSellInstrument.Text = string.Empty;
            this.lblSellTradeInfo.Text = "0/0";
            this.lblSellInstrumentProfit.Text = "0";
            this.lblSellInstrmentOpenAvgPrice.Text = "0";
            this.lblSellInstrumentProfit.Text = "0";

            this.lblTotalProfit.Text = "0";
        }

        

        #region 损益计算
        /// <summary>
        /// 定时计算损益。
        /// </summary>
        /// <param name="state"></param>
        private void CalculateProfitCallback(object state)
        {
            try
            {
                ArbitrageOrderState orderState = m_arbitrageOrder.State;

                ProfitResult result = null;
                if (orderState <= ArbitrageOrderState.Opened)
                {
                    result = CalculatOpenProfit(m_arbitrageOrder);
                }
                else
                {
                    result = CalculatCloseProfit(m_arbitrageOrder);
                }

                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action<ProfitResult>(ShowProfit), result);
                    return;
                }
            }
            catch(Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            m_porfitCalcTimer.Change(500, Timeout.Infinite);
        }

        /// <summary>
        /// 计算开仓损益。
        /// </summary>
        /// <param name="arbitrageOrder"></param>
        /// <returns></returns>
        private ProfitResult CalculatOpenProfit(USeArbitrageOrder arbitrageOrder)
        {
            USeOrderDriver orderDriver = USeManager.Instance.OrderDriver;
            USeQuoteDriver quoteDriver = USeManager.Instance.QuoteDriver;
            Debug.Assert(orderDriver != null);
            Debug.Assert(quoteDriver != null);

            ArbitrageTaskGroup taskGroup = arbitrageOrder.OpenTaskGroup;
            List<USeOrderBook> orderBookList = arbitrageOrder.OpenTaskGroup.GetAllOrderBooks();

            USeMarketData buyMarketData = quoteDriver.Query(taskGroup.BuyInstrument);
            USeMarketData sellMarketData = quoteDriver.Query(taskGroup.SellInstrument);
            USeInstrumentDetail buyInstrumentDetail = orderDriver.QueryInstrumentDetail(taskGroup.BuyInstrument);
            USeInstrumentDetail sellInstrumentDetail = orderDriver.QueryInstrumentDetail(taskGroup.SellInstrument);

            decimal buyProfit = CalculateProfitByOrderBook(orderBookList, buyInstrumentDetail, buyMarketData);
            decimal sellProfit = CalculateProfitByOrderBook(orderBookList, sellInstrumentDetail, sellMarketData);
            decimal totalProfit = buyProfit + sellProfit;

            ProfitResult result = new ProfitResult() {
                BuyProfit = buyProfit,
                SellProfit = sellProfit
            };
            return result;
        }

        /// <summary>
        /// 计算平仓损益。
        /// </summary>
        /// <param name="arbitrageOrder"></param>
        /// <returns></returns>
        private ProfitResult CalculatCloseProfit(USeArbitrageOrder arbitrageOrder)
        {
            USeOrderDriver orderDriver = USeManager.Instance.OrderDriver;
            USeQuoteDriver quoteDriver = USeManager.Instance.QuoteDriver;
            Debug.Assert(orderDriver != null);
            Debug.Assert(quoteDriver != null);

            ArbitrageTaskGroup closeTaskGroup = arbitrageOrder.CloseTaskGroup;
            List<USeOrderBook> orderBookList = arbitrageOrder.GetAllOrderBooks();


            USeMarketData buyMarketData = USeManager.Instance.QuoteDriver.Query(closeTaskGroup.BuyInstrument);
            USeMarketData sellMarketData = USeManager.Instance.QuoteDriver.Query(closeTaskGroup.SellInstrument);
            USeInstrumentDetail buyInstrumentDetail = USeManager.Instance.OrderDriver.QueryInstrumentDetail(closeTaskGroup.BuyInstrument);
            USeInstrumentDetail sellInstrumentDetail = USeManager.Instance.OrderDriver.QueryInstrumentDetail(closeTaskGroup.SellInstrument);

            decimal buyProfit = CalculateProfitByOrderBook(orderBookList, buyInstrumentDetail, buyMarketData);
            decimal sellProfit = CalculateProfitByOrderBook(orderBookList, sellInstrumentDetail, sellMarketData);
            decimal totalProfit = buyProfit + sellProfit;

            ProfitResult result = new ProfitResult() {
                BuyProfit = buyProfit,
                SellProfit = sellProfit
            };
            return result;
        }

        /// <summary>
        /// 计算损益。
        /// </summary>
        /// <param name="orderBookList"></param>
        /// <param name="instrumentDetail"></param>
        /// <param name="marketData"></param>
        /// <returns></returns>
        private decimal CalculateProfitByOrderBook(List<USeOrderBook> orderBookList, USeInstrumentDetail instrumentDetail, USeMarketData marketData)
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

        #region 界面刷新
        /// <summary>
        /// 更新启动运行按钮。
        /// </summary>
        private void ShowRunButton()
        {
            if (m_autoTrader.BackgroundWorkerType == AutoTraderWorkType.OpenOrClose &&
                m_autoTrader.State == AutoTraderState.Enable)
            {
                this.btnStartOrStop.Text = "停止";
                this.btnStartOrStop.BackColor = Color.FromArgb(0, 192, 0);
            }
            else
            {
                this.btnStartOrStop.Text = "启动";
                this.btnStartOrStop.BackColor = Color.FromArgb(255, 192, 192);
            }

            if (m_autoTrader.State == AutoTraderState.Enable)
            {
                this.pbxRunState.Image = global::USeFuturesSpirit.Properties.Resources.green1;
            }
            else
            {
                this.pbxRunState.Image = null;
            }
            this.btnStartOrStop.Enabled = true;
        }

        private void ShowAlarmIcon()
        {
            if(m_autoTrader.HasAlarm)
            {
                this.pbAlarmNotice.Image = global::USeFuturesSpirit.Properties.Resources.alarm_enable;
            }
            else
            {
                this.pbAlarmNotice.Image = global::USeFuturesSpirit.Properties.Resources.alarm_unable;
            }
        }

        /// <summary>
        /// 更新套利单信息。
        /// </summary>
        private void ShowArbitrageOrderView()
        {
            if (m_arbitrageOrder == null)
            {
                ClearView();
                return;
            }

            ArbitrageArgument argument = m_arbitrageOrder.Argument;
            if (argument != null && argument.OpenArg != null)
            {
                this.lblOpenPriceSpread.Text = argument.OpenArg.OpenCondition.ToString();
            }
            else
            {
                this.lblOpenPriceSpread.Text = "----";
            }
            if (argument != null && argument.CloseArg != null)
            {
                this.lblClosePriceSpread.Text = argument.CloseArg.CloseCondition.ToString();
            }
            else
            {
                this.lblClosePriceSpread.Text = "----";
            }
            if (argument != null && argument.StopLossArg != null)
            {
                this.lblStopLossPriceSpread.Text = argument.StopLossArg.StopLossCondition.ToString();
            }
            else
            {
                this.lblStopLossPriceSpread.Text = "----";
            }


            switch (m_arbitrageOrder.State)
            {
                case ArbitrageOrderState.None:
                case ArbitrageOrderState.Opening:
                case ArbitrageOrderState.Opened:
                    ShowOpenInfo(m_arbitrageOrder);
                    break;
                case ArbitrageOrderState.Closeing:
                case ArbitrageOrderState.Closed:
                case ArbitrageOrderState.Finish:
                    ShowCloseInfo(m_arbitrageOrder);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            this.lblArbitrageOrderState.Text = m_arbitrageOrder.State.ToDescription();
        }
        /// <summary>
        /// 展示开仓信息。
        /// </summary>
        /// <param name="arbitrageOrder"></param>
        private void ShowOpenInfo(USeArbitrageOrder arbitrageOrder)
        {
            Debug.Assert(arbitrageOrder.OpenArgument != null);
            Debug.Assert(arbitrageOrder.OpenTaskGroup != null);

            this.panelTop.BackColor = Color.FromArgb(255, 255, 192);
            this.panelPriceSpread.BackColor = Color.FromArgb(255, 255, 192);

            ArbitrageTaskGroup taskGroup = arbitrageOrder.OpenTaskGroup;

            this.lblAlias.Text = arbitrageOrder.Alias;
            if (taskGroup.BuyInstrument != null)
            {
                this.lblBuyInstrument.Text = taskGroup.BuyInstrument.InstrumentCode;
            }
            else
            {
                this.lblBuyInstrument.Text = "---";
            }
            if (taskGroup.SellInstrument != null)
            {
                this.lblSellInstrument.Text = taskGroup.SellInstrument.InstrumentCode;
            }
            else
            {
                this.lblSellInstrument.Text = "---";
            }

            this.lblBuyTradeInfo.Text = string.Format("{0}/{1}", taskGroup.BuySubTaskTradeQty, taskGroup.BuySubTaskPlanOrderQty);
            this.lblBuyInstrmentOpenAvgPrice.Text = taskGroup.BuySubTaskAvgTradePrice.ToString();
            this.lblSellTradeInfo.Text = string.Format("{0}/{1}", taskGroup.SellSubTaskTradeQty, taskGroup.SellSubTaskPlanOrderQty);
            this.lblSellInstrmentOpenAvgPrice.Text = taskGroup.SellSubTaskAvgTradePrice.ToString();
        }

        /// <summary>
        /// 展示平仓信息。
        /// </summary>
        /// <param name="arbitrageOrder"></param>
        private void ShowCloseInfo(USeArbitrageOrder arbitrageOrder)
        {
            Debug.Assert(arbitrageOrder.CloseArgument != null);
            Debug.Assert(arbitrageOrder.CloseTaskGroup != null);
            this.panelTop.BackColor = System.Drawing.Color.AliceBlue;
            this.panelPriceSpread.BackColor = System.Drawing.Color.AliceBlue;

            ArbitrageTaskGroup closeTaskGroup = arbitrageOrder.CloseTaskGroup;

            this.lblAlias.Text = arbitrageOrder.Alias;
            this.lblBuyInstrument.Text = closeTaskGroup.BuyInstrument.InstrumentCode;
            this.lblSellInstrument.Text = closeTaskGroup.SellInstrument.InstrumentCode;

            this.lblBuyTradeInfo.Text = string.Format("{0}/{1}", closeTaskGroup.BuySubTaskTradeQty, closeTaskGroup.BuySubTaskPlanOrderQty);
            this.lblBuyInstrmentOpenAvgPrice.Text = closeTaskGroup.BuySubTaskAvgTradePrice.ToString();
            this.lblSellTradeInfo.Text = string.Format("{0}/{1}", closeTaskGroup.SellSubTaskTradeQty, closeTaskGroup.SellSubTaskPlanOrderQty);
            this.lblSellInstrmentOpenAvgPrice.Text = closeTaskGroup.SellSubTaskAvgTradePrice.ToString();
        }

        /// <summary>
        /// 呈现损益信息。
        /// </summary>
        /// <param name="profitResult"></param>
        private void ShowProfit(ProfitResult profitResult)
        {
            if (profitResult == null) return;

            this.lblBuyInstrumentProfit.Text = profitResult.BuyProfit.ToString("#,0");
            this.lblBuyInstrumentProfit.ForeColor = profitResult.BuyProfit >= 0 ? Color.Red : Color.Green;
            this.lblSellInstrumentProfit.Text = profitResult.SellProfit.ToString("#,0");
            this.lblSellInstrumentProfit.ForeColor = profitResult.SellProfit >= 0 ? Color.Red : Color.Green;
            this.lblTotalProfit.Text = profitResult.TotalProfit.ToString("#,0");
            this.lblTotalProfit.ForeColor = profitResult.TotalProfit >= 0 ? Color.Red : Color.Green;

            
            if(profitResult.CurrentPriceSpread.HasValue)
            {
                this.lblCurrPriceSpread.Text = profitResult.CurrentPriceSpread.Value.ToString();
                this.lblCurrPriceSpread.ForeColor = profitResult.CurrentPriceSpread.Value >= 0 ? Color.Red : Color.Green;
            }
            else
            {
                this.lblCurrPriceSpread.Text = "----";
                this.lblCurrPriceSpread.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// 更新进度绘图界面。
        /// </summary>
        private void ShowChartView()
        {
            this.pbxTaskChart.Refresh();
        }

        /// <summary>
        /// 更新列表控件。
        /// </summary>
        private void ShowTaskGrid()
        {
            if (m_arbitrageOrder.State <= ArbitrageOrderState.Opened)
            {
                //建仓
                if (m_taskDT == null || m_taskDT.TableName != OPEN_TASK ||
                    m_arbitrageOrder.OpenTaskGroup.TaskCount != m_taskDT.Columns.Count - 2)
                {
                    m_taskDT = CreateTaskTable(OPEN_TASK, m_arbitrageOrder.OpenTaskGroup);
                    this.gridTask.DataSource = m_taskDT;
                    SetTaskGridCloumnStyle();
                }

                FillTaskTable(m_taskDT, m_arbitrageOrder.OpenTaskGroup);
            }
            else
            {
                // 平仓
                if (m_taskDT == null || m_taskDT.TableName != CLOSE_TASK ||
                    m_arbitrageOrder.CloseTaskGroup.TaskCount != m_taskDT.Columns.Count - 2)
                {
                    m_taskDT = CreateTaskTable(CLOSE_TASK, m_arbitrageOrder.CloseTaskGroup);
                    this.gridTask.DataSource = m_taskDT;
                    SetTaskGridCloumnStyle();
                }

                FillTaskTable(m_taskDT, m_arbitrageOrder.CloseTaskGroup);
            }
        }
        #endregion

        private void SetTaskGridCloumnStyle()
        {
            foreach(DataGridViewColumn column in this.gridTask.Columns)
            {
                if(column.Name.StartsWith("任务"))
                {
                    column.Width = 50;
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    column.ReadOnly = true;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            m_chartView.Paint(g);
            base.OnPaint(e);
        }

        #region TaskGrid Data
        /// <summary>
        /// 创建任务列表。
        /// </summary>
        /// <returns></returns>
        private DataTable CreateTaskTable(string tableName, ArbitrageTaskGroup taskGroup)
        {
            DataTable table = new DataTable();
            table.TableName = tableName;
            table.Columns.Add("Direction", typeof(string));
            table.Columns.Add("Instrument", typeof(USeInstrument));

            if (taskGroup != null && taskGroup.TaskCount > 0)
            {
                for (int i = 0; i < taskGroup.TaskCount; i++)
                {
                    table.Columns.Add(string.Format("任务{0}", (i + 1)),typeof(OrderTaskResult));
                }
            }

            DataRow buyRow = table.NewRow();
            buyRow["Direction"] = "买入";
            buyRow["Instrument"] = taskGroup.BuyInstrument;
            
            DataRow sellRow = table.NewRow();
            sellRow["Direction"] = "卖出";
            sellRow["Instrument"] = taskGroup.SellInstrument;

            foreach (DataColumn column in table.Columns)
            {
                if (column.ColumnName.StartsWith("任务") == false) continue;

                buyRow[column] = new OrderTaskResult();
                sellRow[column] = new OrderTaskResult();
            }

            if (taskGroup.PreferentialSide == USeOrderSide.Buy)
            {
                table.Rows.Add(buyRow);
                table.Rows.Add(sellRow);
            }
            else if (taskGroup.PreferentialSide == USeOrderSide.Sell)
            {
                table.Rows.Add(sellRow);
                table.Rows.Add(buyRow);
            }
            else
            {
                Debug.Assert(false);
            }
           
            return table;
        }

        private void FillTaskTable(DataTable table ,ArbitrageTaskGroup taskGroup)
        {
            if(taskGroup == null )
            {
                //Debug.Assert(false);
                return;
            }

            Debug.Assert(taskGroup.TaskCount == table.Columns.Count - 2);
            Debug.Assert(table.Rows.Count == 2);
            DataRow firstRow = table.Rows[0];
            DataRow secondRow = table.Rows[1];

            foreach(ArbitrageTask task in taskGroup.TaskList)
            {
                string cloumnName = string.Format("任务{0}", task.TaskId);
                {
                    OrderTaskResult firstResult = new OrderTaskResult();
                    firstResult.PlanOrderQty = task.FirstSubTask.PlanOrderQty;
                    firstResult.OrderQty = task.FirstSubTask.OrderQty;
                    firstResult.TradeQty = task.FirstSubTask.TradeQty;
                    firstResult.AvgTradePrice = task.FirstSubTask.TradeAvgPrice;

                    firstRow[cloumnName] = firstResult;
                }

                {
                    OrderTaskResult secondResult = new OrderTaskResult();
                    secondResult.PlanOrderQty = task.SecondSubTask.PlanOrderQty;
                    secondResult.OrderQty = task.SecondSubTask.OrderQty;
                    secondResult.TradeQty = task.SecondSubTask.TradeQty;
                    secondResult.AvgTradePrice = task.SecondSubTask.TradeAvgPrice;

                    secondRow[cloumnName] = secondResult;
                }
            }
        }
        #endregion

        #region TaskGrid
        /// <summary>
        /// 下单任务结果。
        /// </summary>
        private class OrderTaskResult : USeBaseViewModel
        {
            #region member
            private int m_planOrderQty = 0;
            private int m_orderQty = 0;
            private int m_tradeQty = 0;
            private decimal m_avgTradePrice = 0;
            #endregion

            #region property
            /// <summary>
            /// 计划委托量
            /// </summary>
            public int PlanOrderQty
            {
                get { return m_planOrderQty; }
                set
                {
                    if(value != m_planOrderQty)
                    {
                        m_planOrderQty = value;
                        SetProperty(() => this.PlanOrderQty);
                        SetProperty(() => this.IsFinish);
                    }
                }
            }

            /// <summary>
            /// 委托量。
            /// </summary>
            public int OrderQty
            {
                get { return m_orderQty; }
                set
                {
                    if (value != m_orderQty)
                    {
                        m_orderQty = value;
                        SetProperty(() => this.OrderQty);
                        SetProperty(() => this.IsOrder);
                    }
                }
            }

            /// <summary>
            /// 成交量。
            /// </summary>
            public int TradeQty
            {
                get { return m_tradeQty; }
                set
                {
                    if (value != m_tradeQty)
                    {
                        m_tradeQty = value;
                        SetProperty(() => this.TradeQty);
                        SetProperty(() => this.IsFinish);
                    }
                }
            }

            /// <summary>
            /// 平均成交价格。
            /// </summary>
            public decimal AvgTradePrice
            {
                get { return m_avgTradePrice; }
                set
                {
                    if (value != m_avgTradePrice)
                    {
                        m_avgTradePrice = value;
                        SetProperty(() => this.AvgTradePrice);
                    }
                }
            }

            /// <summary>
            /// 是否完成。
            /// </summary>
            public bool IsFinish
            {
                get
                {
                    if(this.PlanOrderQty >0 && this.PlanOrderQty == this.TradeQty)
                    {
                        return true;
                    }

                    return false;
                }
            }

            public bool IsOrder
            {
                get
                {
                    if(this.OrderQty >0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            #endregion

            public override string ToString()
            {
                return string.Format("{0}/{1}", this.TradeQty, this.PlanOrderQty);
            }
        }


        private void gridTask_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if(this.gridTask.Columns[e.ColumnIndex].Name.StartsWith("任务"))
            {
                OrderTaskResult result = this.gridTask.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as OrderTaskResult;
                if(result != null)
                {
                    if(result.IsFinish)
                    {
                        e.CellStyle.BackColor = Color.Green;
                    }
                    else if(result.IsOrder)
                    {
                        e.CellStyle.BackColor = Color.Yellow;
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.White;
                    }
                }
            }
        }

        private void gridTask_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            DataGridViewCell cell = this.gridTask.Rows[e.RowIndex].Cells[e.ColumnIndex];
            OrderTaskResult result = cell.Value as OrderTaskResult;
            if (result != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("计划委托:  {0}", result.PlanOrderQty));
                sb.AppendLine(string.Format("   委托量:  {0}", result.OrderQty));
                sb.AppendLine(string.Format("  成交量:  {0}", result.TradeQty));
                sb.Append(string.Format("成交均价:  {0}", result.AvgTradePrice));
                cell.ToolTipText = sb.ToString();
            }
        }
        #endregion

    }
}
