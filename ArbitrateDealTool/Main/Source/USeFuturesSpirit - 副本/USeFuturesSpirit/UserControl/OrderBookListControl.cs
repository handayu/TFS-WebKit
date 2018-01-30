using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USeFuturesSpirit.ViewModel;
using USe.TradeDriver.Common;
using System.Diagnostics;


namespace USeFuturesSpirit
{
    public partial class OrderBookListControl : USeUserControl
    {
        private BindingList<OrderBookViewModel> m_order_data_source = new BindingList<OrderBookViewModel>();

        public OrderBookListControl()
        {
            InitializeComponent();
        }

        public override void Initialize()
        {
            this.gridOrder.AutoGenerateColumns = false;
            this.gridOrder.DataSource = m_order_data_source;
            USeManager.Instance.OrderDriver.OnOrderBookChanged += OrderDriver_OnOrderBookChanged;

            ReloadOrders();
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
            OrderBookViewModel marketModel = m_order_data_source.FirstOrDefault(p => p.OrderNum.Equals(orderBook.OrderNum));
            if (marketModel != null)
            {
                if (Filter(orderBook.OrderStatus))
                {
                    marketModel.Update(orderBook);
                }
                else
                {
                    m_order_data_source.Remove(marketModel);
                }
            }
            else
            {
                if(Filter(orderBook.OrderStatus))
                {
                    OrderBookViewModel order_data_model = OrderBookViewModel.Creat(orderBook);
                    m_order_data_source.Insert(0, order_data_model);
                }
            }
        }

        private bool Filter(USeOrderStatus orderStatus)
        {
            if (this.rbnOrderState_All.Checked)
            {
                return true;
            }
            else if (this.rbnOrderState_UnTrade.Checked)
            {
                #region
                if (orderStatus == USeOrderStatus.NoTraded ||
                    orderStatus == USeOrderStatus.PartTraded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                #endregion
            }
            else if (this.rbnOrderState_Traded.Checked)
            {
                if (orderStatus == USeOrderStatus.AllTraded ||
                     orderStatus == USeOrderStatus.PartCanceled)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (this.rbnOrderState_CancelAndError.Checked)
            {
                if (orderStatus == USeOrderStatus.BlankOrder ||
                    orderStatus == USeOrderStatus.PartCanceled ||
                    orderStatus == USeOrderStatus.AllCanceled)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// 撤单-选中的表中特定的委托单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CancelOrder_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows_collection = this.gridOrder.SelectedRows;
            if (rows_collection.Count == 0)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "请选中可撤的委托单...");
                return;
            };

            foreach (DataGridViewRow row in rows_collection)
            {
                OrderBookViewModel orderBookView = row.DataBoundItem as OrderBookViewModel;
                if (orderBookView.IsFinish)
                {
                    USeFuturesSpiritUtility.ShowWarningMessageBox(this, "选中的委托单不可撤...");
                    return;
                }
                string error_info = string.Empty;
                bool bResult = USeManager.Instance.OrderDriver.CancelOrder(orderBookView.OrderNum, orderBookView.Instrument, out error_info);
                if (bResult == false)
                {
                    USeFuturesSpiritUtility.ShowWarningMessageBox(this, error_info);
                }
            }
        }

        /// <summary>
        /// 撤单-所有委托单中的未成交单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CancelAllOrders_Click(object sender, EventArgs e)
        {
            List<USeOrderBook> orderBookList = USeManager.Instance.OrderDriver.QueryOrderBooks();

            List<USeOrderBook> allCancelOrdersList = new List<USeOrderBook>();
            foreach (USeOrderBook orderBook in orderBookList)
            {
                if (orderBook.IsFinish) continue;
                allCancelOrdersList.Add(orderBook);
            }

            if(allCancelOrdersList.Count == 0)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "未查询到委托单可撤...");
                return;
            }

            CancelOrdersForm cancelOrderForm = new CancelOrdersForm(allCancelOrdersList);
            if (DialogResult.OK == cancelOrderForm.ShowDialog())
            {
                foreach (USeOrderBook orderCancel in allCancelOrdersList)
                {
                    string error_info = string.Empty;
                    bool bResult = USeManager.Instance.OrderDriver.CancelOrder(orderCancel.OrderNum, orderCancel.Instrument, out error_info);
                    if (bResult == false)
                    {
                        USeFuturesSpiritUtility.ShowWarningMessageBox(this, error_info);
                    }
                }
            }
        }

        /// <summary>
        /// 重新加载委托数据
        /// </summary>
        private void ReloadOrders()
        {
            if (m_order_data_source != null) m_order_data_source.Clear();

            List<USeOrderBook> orderBookList = USeManager.Instance.OrderDriver.QueryOrderBooks();
            if (orderBookList != null && orderBookList.Count > 0)
            {
                foreach (USeOrderBook orderBook in orderBookList)
                {
                    UpdateOrderBook(orderBook);
                }
            }
        }

        /// <summary>
        /// 全部委托单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnOrderState_All_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbnOrderState_All.Checked) ReloadOrders();
        }

        /// <summary>
        /// 未成交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnOrderState_UnTrade_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbnOrderState_UnTrade.Checked) ReloadOrders();
        }

        /// <summary>
        /// 全部成交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnOrderState_Traded_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbnOrderState_Traded.Checked) ReloadOrders();

        }

        /// <summary>
        /// 可撤/废单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnOrderState_CancelAndError_CheckedChanged(object sender, EventArgs e)
        {
            if(this.rbnOrderState_CancelAndError.Checked)ReloadOrders();

        }
    }
}

