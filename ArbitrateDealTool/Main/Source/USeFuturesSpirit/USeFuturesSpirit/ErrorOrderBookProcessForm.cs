using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;
using System.Diagnostics;

namespace USeFuturesSpirit
{
    public partial class ErrorOrderBookProcessForm : Form
    {
        private DataTable m_dataTable = null;

        private List<ErrorUSeOrderBook> m_errorOrderBookList = new List<ErrorUSeOrderBook>();


        public ErrorOrderBookProcessForm(List<ErrorUSeOrderBook> errorOrderBookList)
        {
            m_errorOrderBookList = errorOrderBookList;
            InitializeComponent();
        }


        public List<ErrorUSeOrderBook> Result
        {
            get;
            private set;
        }



        private void ErrorOrderBookProcessForm_Load(object sender, EventArgs e)
        {
            InitializeColumn();

            m_dataTable = CreateErrorOrderBookDataTable();
            FillData(m_errorOrderBookList, m_dataTable);

            this.gridOrderBook.AutoGenerateColumns = false;
            this.gridOrderBook.DataSource = m_dataTable;
            UpdateErrorOrderCount();
        }


        private bool IsMatchRule(DataRow row, out string errorMessage)
        {
            errorMessage = string.Empty;
            USeOrderStatus orderState = (USeOrderStatus)row["SelectedOrderStatus"];

            int orderQty = Convert.ToInt32(row["OrderQty"]);
            decimal OrderPrice = Convert.ToDecimal(row["OrderPrice"]);

            int tradeQty = Convert.ToInt32(row["TradeQty"]);
            decimal tradePrice = Convert.ToDecimal(row["TradePrice"]);
            decimal tradeFee = Convert.ToDecimal(row["TradeFee"]);

            if (orderState == USeOrderStatus.Unknown)
            {
                errorMessage = "请选择委托单处理状态";
                return false;
            }

            if (orderState == USeOrderStatus.AllTraded)
            {
                if (orderQty != tradeQty)
                {
                    errorMessage = "全部成交状态，成交量应等于委托量";
                    return false;
                }
                if (tradePrice <= 0)
                {
                    errorMessage = "全部成交状态，成交价格不应该为空";
                    return false;
                }

                if(orderQty == tradeQty && tradePrice > 0)
                {
                    errorMessage = "处理完毕";
                    return true;
                }
            }
            else if (orderState == USeOrderStatus.PartCanceled)
            {
                if (tradeQty >= orderQty || tradeQty <= 0)
                {
                    errorMessage = "部分撤单状态，成交量应该大于0小于委托量";
                    return false;
                }

                if (tradePrice <= 0)
                {
                    errorMessage = "部分撤单状态，成交价格不应该为空";
                    return false;
                }

                if (tradeQty < orderQty && tradeQty > 0 && tradePrice > 0)
                {
                    errorMessage = "处理完毕";
                    return true;
                }
            }
            else if (orderState == USeOrderStatus.AllCanceled)
            {
                if (tradeQty != 0)
                {
                    errorMessage = "全部撤单状态，成交量应该等于0";
                    return false;
                }
                if (tradePrice > 0)
                {
                    errorMessage = "全部撤单状态，成交价格应该为0";
                    return false;
                }

                if (tradeQty == 0 && tradePrice == 0)
                {
                    errorMessage = "处理完毕";
                    return true;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(orderState);
                return false;
            }


            return true;
        }

        private DataTable CreateErrorOrderBookDataTable()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Alisa", typeof(string));
            table.Columns.Add("OrderTime", typeof(DateTime));
            table.Columns.Add("OrderSide", typeof(USeOrderSide));
            table.Columns.Add("OrderSideDesc", typeof(string));
            table.Columns.Add("OffsetType", typeof(USeOffsetType));
            table.Columns.Add("OffsetTypeDesc", typeof(string));
            table.Columns.Add("Instrument", typeof(USeInstrument));
            table.Columns.Add("InstrumentCode", typeof(string));

            table.Columns.Add("DoneFlag", typeof(bool));
            table.Columns.Add("IsDone", typeof(Image));
            table.Columns.Add("OrderStatus", typeof(USeOrderStatus));
            table.Columns.Add("OrderStatusDesc", typeof(string));
            table.Columns.Add("SelectedOrderStatus", typeof(int));
            table.Columns.Add("ErrorUSeOrderBook", typeof(ErrorUSeOrderBook));


            table.Columns.Add("OrderQty", typeof(string));
            table.Columns.Add("OrderPrice", typeof(decimal));
            table.Columns.Add("TradeQty", typeof(string));
            table.Columns.Add("TradePrice", typeof(decimal));
            table.Columns.Add("TradeFee", typeof(decimal));
            table.Columns.Add("OrderNum", typeof(USeOrderNum));
            table.Columns.Add("Memo", typeof(string));

            return table;
        }

        private void FillData(List<ErrorUSeOrderBook> errorOrderBooks, DataTable table)
        {
            if (errorOrderBooks == null || table == null) return;
            foreach (ErrorUSeOrderBook errorItem in errorOrderBooks)
            {
                USeOrderBook orderBook = errorItem.OrderBook;

                DataRow row = table.NewRow();
                row["Alisa"] = errorItem.Alias;
                row["OrderTime"] = orderBook.OrderTime;
                row["OrderSide"] = orderBook.OrderSide;
                row["OrderSideDesc"] = orderBook.OrderSide.ToDescription();
                row["IsDone"] = global::USeFuturesSpirit.Properties.Resources.red;
                row["OffsetType"] = orderBook.OffsetType;
                row["OffsetTypeDesc"] = orderBook.OffsetType.ToDescription();
                row["Instrument"] = orderBook.Instrument;
                row["InstrumentCode"] = orderBook.Instrument.InstrumentCode;
                row["DoneFlag"] = false;
                row["IsDone"] = global::USeFuturesSpirit.Properties.Resources.red1;

                row["OrderStatus"] = orderBook.OrderStatus;
                row["OrderStatusDesc"] = orderBook.OrderStatus.ToDescription();
                row["SelectedOrderStatus"] = (int)USeOrderStatus.Unknown;
                row["ErrorUSeOrderBook"] = errorItem;

                row["OrderQty"] = orderBook.OrderQty;
                row["OrderPrice"] = orderBook.OrderPrice;
                row["TradeQty"] = 0;
                row["TradePrice"] = 0;
                row["TradeFee"] = 0;
                row["OrderNum"] = orderBook.OrderNum;
                row["Memo"] = "请选择委托单处理状态";
                table.Rows.Add(row);
            }
        }

        private void InitializeColumn()
        {
            this.Column_SelectOrderState.Items.Clear();
            List<OrderStateusDropItem> dropList = new List<OrderStateusDropItem>();
            {
                OrderStateusDropItem empty = new OrderStateusDropItem() {
                    OrderStatus = USeOrderStatus.Unknown,
                    OrderStatusID = (int)USeOrderStatus.Unknown,
                    Description = "请选择结束状态"
                };
                dropList.Add(empty);
            }
            {
                OrderStateusDropItem dropItem = new OrderStateusDropItem() {
                    OrderStatus = USeOrderStatus.AllTraded,
                    OrderStatusID = (int)USeOrderStatus.AllTraded,
                    Description = "全部成交"
                };
                dropList.Add(dropItem);
            }

            {
                OrderStateusDropItem dropItem = new OrderStateusDropItem() {
                    OrderStatus = USeOrderStatus.AllCanceled,
                    OrderStatusID = (int)USeOrderStatus.AllCanceled,
                    Description = "撤单"
                };
                dropList.Add(dropItem);
            }

            this.Column_SelectOrderState.DataSource = dropList;
            this.Column_SelectOrderState.DisplayMember = "Description";
            this.Column_SelectOrderState.ValueMember = "OrderStatusID";
        }


        private class OrderStateusDropItem
        {
            public USeOrderStatus OrderStatus { get; set; }

            public int OrderStatusID { get; set; }

            public string Description { get; set; }

            public override string ToString()
            {
                return this.Description;
            }
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            List<ErrorUSeOrderBook> result = new List<ErrorUSeOrderBook>();

            try
            {
                //如果存在未被处理的委托单，提示处理
                foreach (DataRow row in m_dataTable.Rows)
                {
                    string errorMessage = string.Empty;
                    if (IsMatchRule(row, out errorMessage) == false)
                    {
                        USeFuturesSpiritUtility.ShowErrrorMessageBox(this, "请处理异常委托单");
                        return;
                    }
                }

                USeOrderDriver orderDriver = USeManager.Instance.OrderDriver;
                Debug.Assert(orderDriver != null);

                //根据界面生成结果保存到Result
                foreach (DataRow row in m_dataTable.Rows)
                {
                    USeInstrument instrument = row["Instrument"] as USeInstrument;
                    Debug.Assert(instrument != null);

                    USeInstrumentDetail instrumentDetail = orderDriver.QueryInstrumentDetail(instrument);
                    //[yangming]合约过期后可能查不到了，此处可以考虑用品种信息获取合约乘数
                    Debug.Assert(instrumentDetail != null);
                    int orderQty = Convert.ToInt32(row["OrderQty"]);
                    int tradeQty = Convert.ToInt32(row["TradeQty"]);
                    int cancelQty = orderQty - tradeQty;
                    Debug.Assert(cancelQty >= 0);

                    decimal tradePrice = Convert.ToDecimal(row["TradePrice"]);
                    decimal tradeFee = Convert.ToDecimal(row["TradeFee"]);
                    decimal tradeAmount = tradePrice * tradeQty * instrumentDetail.VolumeMultiple;

                    USeOrderStatus selectedOrderStatus = (USeOrderStatus)row["SelectedOrderStatus"];
                    USeOrderStatus status = USeOrderStatus.Unknown;
                    if (selectedOrderStatus == USeOrderStatus.AllTraded)
                    {
                        status = USeOrderStatus.AllTraded;
                    }
                    else if (selectedOrderStatus == USeOrderStatus.AllCanceled)
                    {
                        if (cancelQty == orderQty)
                        {
                            status = USeOrderStatus.AllCanceled;
                        }
                        else
                        {
                            status = USeOrderStatus.PartCanceled;
                        }
                    }

                    USeOrderNum orderNum = row["OrderNum"] as USeOrderNum;
                    ErrorUSeOrderBook erroOrderBook = row["ErrorUSeOrderBook"] as ErrorUSeOrderBook;
                    erroOrderBook.OrderBook.OrderStatus = status;
                    erroOrderBook.OrderBook.CancelQty = cancelQty;
                    erroOrderBook.OrderBook.TradeQty = tradeQty;
                    erroOrderBook.OrderBook.TradePrice = tradePrice;
                    erroOrderBook.OrderBook.TradeAmount = tradeAmount;
                    erroOrderBook.OrderBook.TradeFee = tradeFee;

                    result.Add(erroOrderBook);
                }

                this.Result = result;
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, ex.Message);
                return;
            }

            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void gridOrderBook_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void UpdateErrorOrderCount()
        {
            try
            {
                int count = 0;
                foreach (DataRow row in m_dataTable.Rows)
                {
                    if (Convert.ToBoolean(row["DoneFlag"]) == false)
                    {
                        count++;
                    }
                }
                this.lblErrorOrderBookCount.Text = count.ToString();
            }
            catch (Exception ex)
            {
                string s = "";
            }
        }
        private void gridOrderBook_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = -1;
                foreach (DataRow row in m_dataTable.Rows)
                {
                    index++;
                    USeOrderStatus orderState = (USeOrderStatus)row["SelectedOrderStatus"];

                    int orderQty = Convert.ToInt32(row["OrderQty"]);
                    decimal OrderPrice = Convert.ToDecimal(row["OrderPrice"]);
                    int tradeQty = Convert.ToInt32(row["TradeQty"]);
                    decimal tradePrice = Convert.ToDecimal(row["TradePrice"]);
                    decimal tradeFee = Convert.ToDecimal(row["TradeFee"]);

                    string errorMessage = string.Empty;
                    bool matchResult = IsMatchRule(row, out errorMessage);

                    row["Memo"] = errorMessage;

                    if (matchResult != Convert.ToBoolean(row["DoneFlag"]))
                    {
                        Debug.WriteLine("MatchResultChange:" + matchResult + "@" + index);
                        row["DoneFlag"] = matchResult;
                        if (matchResult)
                        {
                            row["IsDone"] = global::USeFuturesSpirit.Properties.Resources.green1;
                        }
                        else
                        {
                            row["IsDone"] = global::USeFuturesSpirit.Properties.Resources.red1;
                        }
                    }
                }
                UpdateErrorOrderCount();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
