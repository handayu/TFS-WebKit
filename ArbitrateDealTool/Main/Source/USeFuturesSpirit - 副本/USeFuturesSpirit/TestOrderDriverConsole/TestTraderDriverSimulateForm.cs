using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using USe.TradeDriver.Test;
using USe.TradeDriver.Common;
using USeFuturesSpirit.ViewModel;


namespace USeFuturesSpirit
{
    public partial class TestTraderDriverSimulateForm : Form
    {
        private BindingList<MarketDataViewModel> m_quoteDataSource = new BindingList<MarketDataViewModel>();
        private BindingList<OrderBookViewModel> m_orderDataSource = new BindingList<OrderBookViewModel>();
        private BindingList<TestPositionDataViewModel> m_positionDataSource = new BindingList<TestPositionDataViewModel>();

        private USeTestQuoteDriver m_quoterDriver = null;
        private USeTestOrderDriver m_orderDriver = null;

        public TestTraderDriverSimulateForm()
        {
            InitializeComponent();
        }

        private void TestTraderDriverSimulateForm_Load(object sender, EventArgs e)
        {
            if (USeManager.Instance.QuoteDriver != null && USeManager.Instance.QuoteDriver is USeTestQuoteDriver)
            {
                m_quoterDriver = USeManager.Instance.QuoteDriver as USeTestQuoteDriver;
            }

            if (USeManager.Instance.OrderDriver != null && USeManager.Instance.OrderDriver is USeTestOrderDriver)
            {
                m_orderDriver = USeManager.Instance.OrderDriver as USeTestOrderDriver;
            }

            if (m_orderDriver == null || m_quoterDriver == null)
            {
                foreach (Control subControl in this.panel_MarketBtn.Controls)
                {
                    if (subControl is Button)
                    {
                        subControl.Enabled = false;
                    }
                }

                foreach (Control subControl in this.panel_OrderBtn.Controls)
                {
                    if (subControl is Button)
                    {
                        subControl.Enabled = false;
                    }
                }

                return;
            }

            this.gridQuote.AutoGenerateColumns = false;
            this.gridQuote.DataSource = m_quoteDataSource;
            m_quoterDriver.OnMarketDataChanged += QuoteDriver_OnMarketDataChanged;

            this.gridOrder.AutoGenerateColumns = false;
            this.gridOrder.DataSource = m_orderDataSource;
            m_orderDriver.OnOrderBookChanged += OrderDriver_OnOrderBookChanged;
            m_orderDriver.OnClientCancelOrder += M_orderDriver_OnClientCancelOrder;

            //持仓
            this.gridPosition.AutoGenerateColumns = false;
            this.gridPosition.DataSource = m_positionDataSource;
            //m_orderDriver.OnPositionChanged += OrderDriver_OnPositionChanged;

            this.textBox_Slip.Text = "1";


            InitializMarketData();
            InitializeOrderBookData();
            //InitializePositionData();
        }

        private void M_orderDriver_OnClientCancelOrder(TestOrderNum orderNum)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new ClientCancelOrderNoticeEventHandle(M_orderDriver_OnClientCancelOrder), orderNum);
                return;
            }
            this.listBox1.Items.Add(orderNum);
        }

        #region 初始化
        private void InitializMarketData()
        {
            m_quoteDataSource.Clear();

            try
            {

                List<USeInstrumentDetail> instrumentDetailList = m_orderDriver.QueryInstrumentDetail();
                if (instrumentDetailList == null) return;

                List<USeInstrument> instrumentList = (from e in instrumentDetailList
                                                      orderby e.Instrument.InstrumentCode
                                                      select e.Instrument).ToList();
                foreach (USeInstrument instrument in instrumentList)
                {
                    MarketDataViewModel marketModel = new MarketDataViewModel(instrument);
                    m_quoteDataSource.Add(marketModel);
                }

                m_quoterDriver.Subscribe(instrumentList);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        private void InitializeOrderBookData()
        {
            List<USeOrderBook> orderBookList = m_orderDriver.QueryOrderBooks();
            if (orderBookList != null && orderBookList.Count > 0)
            {
                foreach (USeOrderBook orderBook in orderBookList)
                {
                    UpdateOrderBook(orderBook);
                }
            }
        }
        #endregion

        /// <summary>
        /// 获取初始持仓数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_InitPosition_Click(object sender, EventArgs e)
        {
            m_positionDataSource.Clear();

            try
            {

                List<USePosition> positionList = m_orderDriver.GetDefaultPositionList();
                if (positionList == null) return;

                foreach (USePosition position in positionList)
                {
                    TestPositionDataViewModel positionModel = TestPositionDataViewModel.Creat(position);
                    m_positionDataSource.Add(positionModel);

                    ////更新到客户端
                    //USePosition positionData = ConvertModelPositon(positionModel);
                    //m_orderDriver.ReloadInitPositionData(positionData);

                    ////更新初始持仓列表
                    //m_orderDriver.AddPositionList(positionData);
                }

            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        /// <summary>
        /// 修改后的Position数据同步到前台客户端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_AsycToClient_Click(object sender, EventArgs e)
        {
            DataGridViewRowCollection rows_collection = this.gridPosition.Rows;
            if (rows_collection.Count == 0)
            {
                return;
            }

            m_orderDriver.ClearPositionList();

            foreach (DataGridViewRow row in rows_collection)
            {
                TestPositionDataViewModel positionModel = row.DataBoundItem as TestPositionDataViewModel;
                USePosition positionData = ConvertModelPositon(positionModel);
                //重新更新持仓列表
                m_orderDriver.AddPositionList(positionData);

                //同步到客户端
                m_orderDriver.ReloadInitPositionData(positionData);
            }

        }

        private void QuoteDriver_OnMarketDataChanged(object sender, USeMarketDataChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeMarketDataChangedEventArgs>(QuoteDriver_OnMarketDataChanged), sender, e);
                return;
            }

            MarketDataViewModel marketModel = m_quoteDataSource.FirstOrDefault(p => p.Instrument == e.MarketData.Instrument);
            if (marketModel != null)
            {
                marketModel.Update(e.MarketData);
            }
        }

        /// <summary>
        /// 新增，变更委托单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderDriver_OnOrderBookChanged(object sender, USeOrderBookChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeOrderBookChangedEventArgs>(OrderDriver_OnOrderBookChanged), sender, e);
                return;
            }

            UpdateOrderBook(e.OrderBook);
        }

        private void UpdateOrderBook(USeOrderBook orderBook)
        {
            OrderBookViewModel marketModel = m_orderDataSource.FirstOrDefault(p => p.OrderNum.Equals(orderBook.OrderNum));
            if (marketModel != null)
            {
                marketModel.Update(orderBook);
            }
            else
            {
                OrderBookViewModel order_data_model = OrderBookViewModel.Creat(orderBook);
                m_orderDataSource.Insert(0, order_data_model);
            }
        }
        private void TestTraderDriverSimulateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                CloseReason reason = e.CloseReason;
                e.Cancel = true;
                this.Hide();
            }
        }

        public USeMarketData ConvertModelData(MarketDataViewModel data_model)
        {
            USeMarketData market_data = new USeMarketData();
            if (data_model == null) return market_data;
       
            market_data.Instrument = data_model.Instrument;
            market_data.LastPrice = data_model.LastPrice;
            market_data.AskPrice = data_model.AskPrice;
            market_data.BidPrice = data_model.BidPrice;
            market_data.OpenPrice = data_model.OpenPrice;
            market_data.ClosePrice = data_model.ClosePrice;
            market_data.HighPrice = data_model.HighPrice;
            market_data.LowPrice = data_model.LowPrice;
            market_data.PreClosePrice = data_model.PreClosePrice;
            market_data.PreSettlementPrice = data_model.PreSettlementPrice;

            //随机一个AskSize和BidSize
            Random ran = new Random();
            int randAskSize = ran.Next(6, 23);
            int randBidSize = ran.Next(8, 20);
            market_data.AskSize = randAskSize;
            market_data.BidSize = randBidSize;

            return market_data;
        }

        public USeOrderBook ConvertModelOrder(OrderBookViewModel order_data_model)
        {
            USeOrderBook order_book = new USeOrderBook();
            if (order_data_model == null) return order_book;

            order_book.OrderNum = order_data_model.OrderNum;
            order_book.Account = order_data_model.Account;
            order_book.Instrument = order_data_model.Instrument;
            order_book.OrderQty = order_data_model.OrderQty;
            order_book.OrderPrice = order_data_model.OrderPrice;
            order_book.TradeQty = order_data_model.TradeQty;
            order_book.TradeAmount = order_data_model.TradeAmount;
            order_book.TradePrice = order_data_model.TradePrice;
            order_book.TradeFee = order_data_model.TradeFee;
            order_book.OrderStatus = order_data_model.OrderStatus;
            order_book.CancelQty = order_data_model.CancelQty;
            //order_book.BlankQty = order_data_model.BlankQty;
            order_book.OrderSide = order_data_model.OrderSide;
            order_book.OffsetType = order_data_model.OffsetType;
            order_book.Memo = order_data_model.Memo;
            order_book.OrderTime = order_data_model.OrderTime;
            //order_book.IsFinish = order_data_model.IsFinish;
            return order_book;
        }

        public USePosition ConvertModelPositon(TestPositionDataViewModel position_model)
        {
            USePosition positionData = new USePosition();
            if (position_model == null) return positionData;
           positionData.Instrument = position_model.Instrument;
           positionData.Direction = position_model.Direction;
           positionData.NewPosition = position_model.NewPosition;
           positionData.OldPosition = position_model.OldPosition;
           positionData.NewFrozonPosition = position_model.NewFrozonPosition;
           positionData.OldFrozonPosition = position_model.OldFrozonPosition;
           positionData.YesterdayPosition = position_model.YesterdayPosition;
           positionData.AvgPirce = position_model.AvgPrice;
           positionData.Amount = position_model.Amount;
           positionData.OpenQty = position_model.OpenQty;
           positionData.CloseQty = position_model.CloseQty;
            return positionData;

        }

        private void PublichMarketDataAdd(USeMarketData data, int tick)
        {
            if (data == null || m_orderDriver == null) return;

            decimal priceTick = m_orderDriver.QueryInstrumentDetail(data.Instrument).PriceTick;
            data.LastPrice = data.LastPrice + priceTick * tick;
            data.AskPrice = data.AskPrice + priceTick * tick;
            data.BidPrice = data.BidPrice + priceTick * tick;
            m_quoterDriver.changeMarketData(data);
        }

        private void PublichMarketDataLess(USeMarketData data, int tick)
        {
            if (data == null || m_orderDriver == null) return;

            decimal priceTick = m_orderDriver.QueryInstrumentDetail(data.Instrument).PriceTick;
            data.LastPrice = data.LastPrice - priceTick * tick;
            data.AskPrice = data.AskPrice - priceTick * tick;
            data.BidPrice = data.BidPrice - priceTick * tick;
            m_quoterDriver.changeMarketData(data);
        }

        private void button_ADD1_Click(object sender, EventArgs e)
        {
            MarketDataViewModel marketDataModel = this.gridQuote.Rows[this.gridQuote.CurrentCell.RowIndex].DataBoundItem as MarketDataViewModel;
            PublichMarketDataAdd(ConvertModelData(marketDataModel), 1);


        }

        private void button_ADD2_Click(object sender, EventArgs e)
        {
            MarketDataViewModel marketDataModel = this.gridQuote.Rows[this.gridQuote.CurrentCell.RowIndex].DataBoundItem as MarketDataViewModel;
            PublichMarketDataAdd(ConvertModelData(marketDataModel), 2);
        }
  
        private void button_ADD3_Click(object sender, EventArgs e)
        {
            MarketDataViewModel marketDataModel = this.gridQuote.Rows[this.gridQuote.CurrentCell.RowIndex].DataBoundItem as MarketDataViewModel;
            PublichMarketDataAdd(ConvertModelData(marketDataModel), 3);
        }

        private void button_ADD4_Click(object sender, EventArgs e)
        {
            MarketDataViewModel marketDataModel = this.gridQuote.Rows[this.gridQuote.CurrentCell.RowIndex].DataBoundItem as MarketDataViewModel;
            PublichMarketDataAdd(ConvertModelData(marketDataModel), 4);
        }

        private void button_LESS1_Click(object sender, EventArgs e)
        {
            MarketDataViewModel marketDataModel = this.gridQuote.Rows[this.gridQuote.CurrentCell.RowIndex].DataBoundItem as MarketDataViewModel;
            PublichMarketDataLess(ConvertModelData(marketDataModel), 1);

        }

        private void button_LESS2_Click(object sender, EventArgs e)
        {
            MarketDataViewModel marketDataModel = this.gridQuote.Rows[this.gridQuote.CurrentCell.RowIndex].DataBoundItem as MarketDataViewModel;
            PublichMarketDataLess(ConvertModelData(marketDataModel), 2);
        }

        private void button_LESS3_Click(object sender, EventArgs e)
        {
            MarketDataViewModel marketDataModel = this.gridQuote.Rows[this.gridQuote.CurrentCell.RowIndex].DataBoundItem as MarketDataViewModel;
            PublichMarketDataLess(ConvertModelData(marketDataModel), 3);

        }

        private void button_LESS4_Click(object sender, EventArgs e)
        {
            MarketDataViewModel marketDataModel = this.gridQuote.Rows[this.gridQuote.CurrentCell.RowIndex].DataBoundItem as MarketDataViewModel;
            PublichMarketDataLess(ConvertModelData(marketDataModel), 4);

        }

        private void button1_ADDANY_Click(object sender, EventArgs e)
        {
            if (this.textBox_ADDLESSNUM.Text == "") return;

            MarketDataViewModel marketDataModel = this.gridQuote.Rows[this.gridQuote.CurrentCell.RowIndex].DataBoundItem as MarketDataViewModel;
            PublichMarketDataAdd(ConvertModelData(marketDataModel), System.Convert.ToInt32(this.textBox_ADDLESSNUM.Text));
        }

        private void button_LESSANY_Click(object sender, EventArgs e)
        {
            if (this.textBox_ADDLESSNUM.Text == "") return;

            MarketDataViewModel marketDataModel = this.gridQuote.Rows[this.gridQuote.CurrentCell.RowIndex].DataBoundItem as MarketDataViewModel;
            PublichMarketDataLess(ConvertModelData(marketDataModel), System.Convert.ToInt32(this.textBox_ADDLESSNUM.Text));
        }

        private void button_AllTrade_Click(object sender, EventArgs e)
        {
            if (m_orderDriver == null) return;
            OrderBookViewModel orderModel = this.gridOrder.Rows[this.gridOrder.CurrentCell.RowIndex].DataBoundItem as OrderBookViewModel;
            m_orderDriver.AllTrade(orderModel.OrderNum);
        }

        private void button_PARTTRATED_Click(object sender, EventArgs e)
        {
            if (m_orderDriver == null) return;
            OrderBookViewModel orderModel = this.gridOrder.Rows[this.gridOrder.CurrentCell.RowIndex].DataBoundItem as OrderBookViewModel;
            m_orderDriver.PartTrade(orderModel.OrderNum);
        }

        private void button_CACELORDER_Click(object sender, EventArgs e)
        {
            if (m_orderDriver == null) return;
            OrderBookViewModel orderModel = this.gridOrder.Rows[this.gridOrder.CurrentCell.RowIndex].DataBoundItem as OrderBookViewModel;
            m_orderDriver.CanceledOrderActionReturn(orderModel.OrderNum, true);
        }

        private void button_ListAllTraded_Click(object sender, EventArgs e)
        {
            DataGridViewRowCollection row_collection = this.gridOrder.Rows;
            if (row_collection.Count == 0) return;
            foreach(DataGridViewRow data_row in row_collection)
            {
                OrderBookViewModel order_model = data_row.DataBoundItem as OrderBookViewModel;
                if(order_model.IsFinish == false)
                {
                    m_orderDriver.AllTrade(order_model.OrderNum);
                }
            }
        }

        private void button_SlipPointTraded_Click(object sender, EventArgs e)
        {
            //滑点不填写值默认滑点一跳，填写值按照填写滑点成交
            if (m_orderDriver == null) return;
            OrderBookViewModel orderModel = this.gridOrder.Rows[this.gridOrder.CurrentCell.RowIndex].DataBoundItem as OrderBookViewModel;
            if (this.textBox_Slip.Text == "")
            {
                m_orderDriver.AllTrade(orderModel.OrderNum,1);
            }
            else
            {
                int slip_point = System.Convert.ToInt32((this.textBox_Slip.Text));
                m_orderDriver.AllTrade(orderModel.OrderNum, slip_point);
            }
        }

        private void button_SlipPointPartTraded_Click(object sender, EventArgs e)
        {
            //滑点不填写值默认滑点一跳，填写值按照填写滑点成交
            if (m_orderDriver == null) return;
            OrderBookViewModel orderModel = this.gridOrder.Rows[this.gridOrder.CurrentCell.RowIndex].DataBoundItem as OrderBookViewModel;
            if (this.textBox_Slip.Text == "")
            {
                m_orderDriver.PartTrade(orderModel.OrderNum, 1);
            }
            else
            {
                int slip_point = System.Convert.ToInt32((this.textBox_Slip.Text));
                m_orderDriver.PartTrade(orderModel.OrderNum, slip_point);
            }
        }

        /// <summary>
        /// 主动撤单成功返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection itemsCollection =  this.listBox1.SelectedItems;
            if (itemsCollection.Count == 0) return;
            List<object> cancelOrderNumList = new List<object>();
            foreach (object o in itemsCollection)
            {
                TestOrderNum orderNum = o as TestOrderNum;
                bool iResult = m_orderDriver.CanceledOrderActionReturn(orderNum, true);
                if(iResult)
                {
                    cancelOrderNumList.Add(o);
                }
            }
            foreach(object o in cancelOrderNumList)
            {
                this.listBox1.Items.Remove(o);
            }
        }

        /// <summary>
        /// 主动撤单被拒绝
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection itemsCollection = this.listBox1.SelectedItems;
            if (itemsCollection.Count == 0) return;
            List<object> cancelOrderNumList = new List<object>();
            foreach (object o in itemsCollection)
            {
                TestOrderNum orderNum = o as TestOrderNum;
                bool iResult = m_orderDriver.CanceledOrderActionReturn(orderNum, false);
                if (iResult == false)
                {
                    cancelOrderNumList.Add(o);
                }
            }

            foreach (object o in cancelOrderNumList)
            {
                this.listBox1.Items.Remove(o);
            }
        }

        /// <summary>
        /// 全部撤单成功返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_AllCancelSuccess_Click(object sender, EventArgs e)
        {
            List<object> cancelOrderNumList = new List<object>();
            foreach (object o in this.listBox1.Items)
            {
                TestOrderNum orderNum = o as TestOrderNum;
                bool iResult = m_orderDriver.CanceledOrderActionReturn(orderNum, true);
                if (iResult)
                {
                    cancelOrderNumList.Add(o);
                }
            }
            foreach (object o in cancelOrderNumList)
            {
                this.listBox1.Items.Remove(o);
            }
        }

        /// <summary>
        /// 全部撤单被拒绝返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_AllCancelRejected_Click(object sender, EventArgs e)
        {
            List<object> cancelOrderNumList = new List<object>();
            foreach (object o in this.listBox1.Items)
            {
                TestOrderNum orderNum = o as TestOrderNum;
                bool iResult = m_orderDriver.CanceledOrderActionReturn(orderNum, false);
                if(iResult == false)
                {
                    cancelOrderNumList.Add(o);
                }
            }
            foreach (object o in cancelOrderNumList)
            {
                this.listBox1.Items.Remove(o);
            }
        }

        private void btnOrderDriverEnable_Click(object sender, EventArgs e)
        {
            m_orderDriver.ManualSetOrderState(USeOrderDriverState.Ready);
        }

        private void btnOrderDriverDisable_Click(object sender, EventArgs e)
        {
            m_orderDriver.ManualSetOrderState(USeOrderDriverState.DisConnected);
        }
    }
}
