using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USeFuturesSpirit;
using System.Diagnostics;

namespace USeFuturesSpirit
{
    public partial class HistoryArbitrageOrderListForm : Form
    {
        #region construction
        public HistoryArbitrageOrderListForm()
        {
            InitializeComponent();
        }
        #endregion

        private void HistoryArbitrageOrderListForm_Load(object sender, EventArgs e)
        {
            this.gridArbitrage.AutoGenerateColumns = false;
            this.dateTimePicker1.Checked = true;
            this.dateTimePicker1.Value = DateTime.Today;
        }


        private void btnQuery_Click(object sender, EventArgs e)
        {
            
            List<ArbitrageOrderBookViewModel> modelList = CreateViewModelList();

            this.gridArbitrage.AutoGenerateColumns = false;
            this.gridArbitrage.DataSource = modelList;
            if (modelList == null || modelList.Count <= 0)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "未找到历史套利单");
            }
        }


        private List<ArbitrageOrderBookViewModel> CreateViewModelList()
        {
            List<USeArbitrageOrder> arbitrageOrderList = LoadHistoryArbitrageOrderList();

            if (arbitrageOrderList == null)
            {
                return null;
            }

            List<ArbitrageOrderBookViewModel> modelList = new List<ArbitrageOrderBookViewModel>();
            foreach (USeArbitrageOrder arbitrageOrder in arbitrageOrderList)
            {
                ArbitrageOrderBookViewModel arbitrageOrderModel = ArbitrageOrderBookViewModel.Creat(arbitrageOrder);
                modelList.Add(arbitrageOrderModel);
            }

            return modelList;
        }

        private List<USeArbitrageOrder> LoadHistoryArbitrageOrderList()
        {
            //读取所有历史套利单信息
            List<USeArbitrageOrder> USeArbitrageOrderList = new List<USeArbitrageOrder>();

            try
            {
                Debug.Assert(USeManager.Instance.LoginUser != null);
                string brokerId = USeManager.Instance.LoginUser.BrokerId;
                string account = USeManager.Instance.LoginUser.Account;

                DateTime? beginTime = null;
                if (this.dateTimePicker1.Checked)
                {
                    beginTime = this.dateTimePicker1.Value.Date;
                }
                DateTime? endTime = null;
                if (this.dateTimePicker2.Checked)
                {
                    endTime = this.dateTimePicker2.Value.Date.AddDays(1);
                }

                List<USeArbitrageOrder> HistoryArbitrageOrdersList = USeManager.Instance.DataAccessor.GetHistoryArbitrageOrders(brokerId, account, beginTime, endTime);

                if (HistoryArbitrageOrdersList == null)
                {
                    return USeArbitrageOrderList;
                }
                else
                {
                    return HistoryArbitrageOrdersList;
                }
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "LoadHistoryArbitrageOrderList error :" + ex.Message);
                return USeArbitrageOrderList;
            }
        }

        private void HistoryArbitrageView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            ArbitrageOrderBookViewModel arbitrageOrderModel = this.gridArbitrage.Rows[e.RowIndex].DataBoundItem as ArbitrageOrderBookViewModel;
            USeArbitrageOrder order = arbitrageOrderModel.ArbitrageOrder;

            ArbitrageOrderViewForm arbitrageOrderViewForm = new ArbitrageOrderViewForm(order);
            arbitrageOrderViewForm.ShowDialog();
        }
    }
}
