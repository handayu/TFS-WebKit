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
    public partial class ArbitrageDefultOrderSettingsForm : Form
    {
        private DataTable m_dataTable = null;

        private List<ArbitrageCombineOrderSetting> m_arbitrageSettingsList = new List<ArbitrageCombineOrderSetting>();

        public ArbitrageDefultOrderSettingsForm()
        {
            InitializeComponent();
        }

        public ArbitrageDefultOrderSettingsForm(List<ArbitrageCombineOrderSetting> arbitrageCombineOrderSettingsList)
        {
            m_arbitrageSettingsList = arbitrageCombineOrderSettingsList;
            InitializeComponent();
        }

        private bool VerifyDataTable(DataRow row, out string errorMessage)
        {
            errorMessage = string.Empty;

            string product = row["ProductName"] as string;

            int openVolumn = Convert.ToInt32(row["OpenVolumn"]);
            int perOpenVolumn = Convert.ToInt32(row["PerOpenVolumn"]);

            int openDirection = (int)row["OpenFirstDirectionID"];
            int closeDirection = (int)row["CloseFirstDirectionID"];
            int stoplossDirection = (int)row["StopLossFirstDirectionID"];

            ArbitrageOrderPriceType nearOpenPriceStyle = (ArbitrageOrderPriceType)row["NearOpenPriceStyleID"];
            ArbitrageOrderPriceType farOpenPriceStyle = (ArbitrageOrderPriceType)row["FarOpenPriceStyleID"];

            if (product == string.Empty)
            {
                errorMessage = "请选择产品";
                return false;
            }
            if (openVolumn <= 0 || perOpenVolumn <= 0)
            {
                errorMessage = "开仓手数和每次开仓手数不能小于0";
                return false;
            }
            if (openVolumn < perOpenVolumn)
            {
                errorMessage = "每次开仓手数不能大于总开仓手数";
                return false;
            }
            if (openDirection == -1 || closeDirection == -1 || stoplossDirection == -1)
            {
                errorMessage = "优先开仓方向，优先平仓方向，优先止损方向请选择";
                return false;
            }
            if (nearOpenPriceStyle == ArbitrageOrderPriceType.Unknown || farOpenPriceStyle == ArbitrageOrderPriceType.Unknown)
            {
                errorMessage = "请选择近月开仓价格类型，远月开仓价格类型";
                return false;
            }

            return true;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {

            List<ArbitrageCombineOrderSetting> arbitrageCombineOrderList = new List<ArbitrageCombineOrderSetting>();

            try
            {
                //保存之前数据校验
                foreach (DataRow row in m_dataTable.Rows)
                {
                    string errorMessage = string.Empty;
                    if (VerifyDataTable(row, out errorMessage) == false)
                    {
                        USeFuturesSpiritUtility.ShowErrrorMessageBox(this, "请填写完整的默认信息:" + errorMessage);
                        return;
                    }
                }

                //根据界面生成结果保存
                foreach (DataRow row in m_dataTable.Rows)
                {
                    string productCode = row["ProductName"] as string;
                    Debug.Assert(productCode != string.Empty);

                    int openVolumn = Convert.ToInt32(row["OpenVolumn"]);
                    int perOpenVolumn = Convert.ToInt32(row["PerOpenVolumn"]);

                    USeDirection openDirection = (USeDirection)row["OpenFirstDirectionID"];

                    USeDirection closeDirection = (USeDirection)row["CloseFirstDirectionID"];
                    USeDirection stoplossDirection = (USeDirection)row["StopLossFirstDirectionID"];

                    ArbitrageOrderPriceType nearOpenPriceStyle = (ArbitrageOrderPriceType)row["NearOpenPriceStyleID"];
                    ArbitrageOrderPriceType farOpenPriceStyle = (ArbitrageOrderPriceType)row["FarOpenPriceStyleID"];

                    ArbitrageCombineOrderSetting order = new ArbitrageCombineOrderSetting();
                    USeProduct product = new USeProduct()
                    {
                        ProductCode = productCode
                    };
                    order.Product = product;

                    order.OpenVolumn = openVolumn;
                    order.OpenVolumnPerNum = perOpenVolumn;
                    order.OpenFirstDirection = openDirection;
                    order.CloseFirstDirection = closeDirection;
                    order.StoplossFirstDirection = stoplossDirection;
                    order.NearPriceStyle = nearOpenPriceStyle;
                    order.FarPriceStyle = farOpenPriceStyle;

                    arbitrageCombineOrderList.Add(order);
                }
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, ex.Message);
                return;
            }

            string brokerId = USeManager.Instance.LoginUser.BrokerId;
            string account = USeManager.Instance.LoginUser.Account;

            USeManager.Instance.DataAccessor.SaveCombineOrderSettings(brokerId, account, arbitrageCombineOrderList);

            this.DialogResult = DialogResult.Yes;
            this.Close();

        }

        private void button2_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private DataTable CreatArbitrageCombineInstrumentDataTable()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Product", typeof(USeProduct));
            table.Columns.Add("ProductDesc", typeof(string));
            table.Columns.Add("ProductName", typeof(string));

            table.Columns.Add("OpenVolumn", typeof(int));
            table.Columns.Add("PerOpenVolumn", typeof(int));

            table.Columns.Add("OpenFirstDirection", typeof(USeDirection));
            table.Columns.Add("OpenFirstDirectionDesc", typeof(string));
            table.Columns.Add("OpenFirstDirectionID", typeof(int));


            table.Columns.Add("CloseFirstDirection", typeof(USeDirection));
            table.Columns.Add("CloseFirstDirectionDesc", typeof(string));
            table.Columns.Add("CloseFirstDirectionID", typeof(int));

            table.Columns.Add("StopLossFirstDirection", typeof(USeDirection));
            table.Columns.Add("StopLossFirstDirectionDesc", typeof(string));
            table.Columns.Add("StopLossFirstDirectionID", typeof(int));


            table.Columns.Add("NearOpenPriceStyle", typeof(ArbitrageOrderPriceType));
            table.Columns.Add("NearOpenPriceStyleDesc", typeof(string));
            table.Columns.Add("NearOpenPriceStyleID", typeof(int));

            table.Columns.Add("FarOpenPriceStyle", typeof(ArbitrageOrderPriceType));
            table.Columns.Add("FarOpenPriceStyleDesc", typeof(string));
            table.Columns.Add("FarOpenPriceStyleID", typeof(int));

            return table;
        }

        private void FillData(List<ArbitrageCombineOrderSetting> arbitrageSettingsList, DataTable table)
        {
            if (arbitrageSettingsList == null || table == null) return;
            foreach (ArbitrageCombineOrderSetting combineOrder in arbitrageSettingsList)
            {
                ArbitrageCombineOrderSetting order = combineOrder;
                DataRow row = table.NewRow();
                row["Product"] = order.Product;
                row["ProductDesc"] = order.Product.ShortName;
                row["ProductName"] = order.Product.ProductCode;

                row["OpenVolumn"] = order.OpenVolumn;
                row["PerOpenVolumn"] = order.OpenVolumnPerNum;

                row["OpenFirstDirection"] = order.OpenFirstDirection;
                row["OpenFirstDirectionID"] = (int)order.OpenFirstDirection;
                row["OpenFirstDirectionDesc"] = order.OpenFirstDirection.ToDescription();

                row["CloseFirstDirection"] = order.CloseFirstDirection;
                row["CloseFirstDirectionDesc"] = order.CloseFirstDirection.ToDescription();
                row["CloseFirstDirectionID"] = (int)order.CloseFirstDirection;

                row["StopLossFirstDirection"] = order.StoplossFirstDirection;
                row["StopLossFirstDirectionDesc"] = order.StoplossFirstDirection.ToDescription();
                row["StopLossFirstDirectionID"] = (int)order.StoplossFirstDirection;


                row["NearOpenPriceStyle"] = order.NearPriceStyle;
                row["NearOpenPriceStyleDesc"] = order.NearPriceStyle.ToDescription();
                row["NearOpenPriceStyleID"] = (int)order.NearPriceStyle;

                row["FarOpenPriceStyle"] = order.FarPriceStyle;
                row["FarOpenPriceStyleDesc"] = order.FarPriceStyle.ToDescription();
                row["FarOpenPriceStyleID"] = (int)order.FarPriceStyle;


                table.Rows.Add(row);
            }
        }


        private void Load_Form(object sender, EventArgs e)
        {
            string brokerId = USeManager.Instance.LoginUser.BrokerId;
            string account = USeManager.Instance.LoginUser.Account;

            try
            {
                m_arbitrageSettingsList = USeManager.Instance.DataAccessor.GetCombineOrderSettings(brokerId, account);
            }
            catch (Exception ex)
            {
                MessageBox.Show("套利下单默认参数设定窗口：" + ex.Message);
            }

            try
            {
                List<USeProduct> productList = USeManager.Instance.OrderDriver.QueryProducts();

                InitializeColumn(productList);

                m_dataTable = CreatArbitrageCombineInstrumentDataTable();
                FillData(m_arbitrageSettingsList, m_dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("套利下单默认参数设定窗口：" + ex.Message);
            }


            this.dataGridView_ArbitrageOrderSettings.AutoGenerateColumns = false;
            this.dataGridView_ArbitrageOrderSettings.DataSource = m_dataTable;


        }

        private void InitializeColumn(List<USeProduct> productList)
        {
            if (productList == null)
            {
                throw new Exception("InitializeColumn:产品列表为空");
            }

            this.Column_Product.Items.Clear();
            List<ProductDropItem> productDropList = new List<ProductDropItem>();
            {
                ProductDropItem empty = new ProductDropItem()
                {
                    Product = null,
                    ProductName = string.Empty,
                    Description = "请选择品种"
                };
                productDropList.Add(empty);
            }
            //TODO //查询品种全部添加
            foreach (USeProduct p in productList)
            {

                ProductDropItem empty = new ProductDropItem()
                {
                    Product = p,
                    ProductName = p.ProductCode,
                    Description = p.ShortName
                };
                productDropList.Add(empty);

            }

            this.Column_Product.DataSource = productDropList;
            this.Column_Product.DisplayMember = "Description";
            this.Column_Product.ValueMember = "ProductName";

            //开仓优先方向
            this.Column_OpenDirection.Items.Clear();
            List<FistDirectionDropItem> firstOpenDirectionDropList = new List<FistDirectionDropItem>();
            {
                FistDirectionDropItem empty = new FistDirectionDropItem()
                {
                    Direction = USeDirection.Long,
                    DirectionID = -1,
                    Description = "请选择方向"
                };
                firstOpenDirectionDropList.Add(empty);
            }

            {
                FistDirectionDropItem dropItem = new FistDirectionDropItem()
                {
                    Direction = USeDirection.Long,
                    DirectionID = (int)USeDirection.Long,
                    Description = "买入"
                };
                firstOpenDirectionDropList.Add(dropItem);
            }
            {
                FistDirectionDropItem dropItem = new FistDirectionDropItem()
                {
                    Direction = USeDirection.Short,
                    DirectionID = (int)USeDirection.Short,
                    Description = "卖出"
                };
                firstOpenDirectionDropList.Add(dropItem);
            }


            this.Column_OpenDirection.DataSource = firstOpenDirectionDropList;
            this.Column_OpenDirection.DisplayMember = "Description";
            this.Column_OpenDirection.ValueMember = "DirectionID";

            //平仓优先方向
            this.Column_CloseDirection.Items.Clear();
            List<FistDirectionDropItem> firstCloseDirectionDropList = new List<FistDirectionDropItem>();
            {
                FistDirectionDropItem empty = new FistDirectionDropItem()
                {
                    Direction = USeDirection.Long,
                    DirectionID = -1,
                    Description = "请选择方向"
                };
                firstCloseDirectionDropList.Add(empty);
            }

            {
                FistDirectionDropItem dropItem = new FistDirectionDropItem()
                {
                    Direction = USeDirection.Long,
                    DirectionID = (int)USeDirection.Long,
                    Description = "买入"
                };
                firstCloseDirectionDropList.Add(dropItem);
            }
            {
                FistDirectionDropItem dropItem = new FistDirectionDropItem()
                {
                    Direction = USeDirection.Short,
                    DirectionID = (int)USeDirection.Short,
                    Description = "卖出"
                };
                firstCloseDirectionDropList.Add(dropItem);
            }


            this.Column_CloseDirection.DataSource = firstCloseDirectionDropList;
            this.Column_CloseDirection.DisplayMember = "Description";
            this.Column_CloseDirection.ValueMember = "DirectionID";

            //止损优先方向
            this.Column_StoplossDirection.Items.Clear();
            List<FistDirectionDropItem> firstStoplossDirectionDropList = new List<FistDirectionDropItem>();
            {
                FistDirectionDropItem empty = new FistDirectionDropItem()
                {
                    Direction = USeDirection.Long,
                    DirectionID = -1,
                    Description = "请选择方向"
                };
                firstStoplossDirectionDropList.Add(empty);
            }

            {
                FistDirectionDropItem dropItem = new FistDirectionDropItem()
                {
                    Direction = USeDirection.Long,
                    DirectionID = (int)USeDirection.Long,
                    Description = "买入"
                };
                firstStoplossDirectionDropList.Add(dropItem);
            }
            {
                FistDirectionDropItem dropItem = new FistDirectionDropItem()
                {
                    Direction = USeDirection.Short,
                    DirectionID = (int)USeDirection.Short,
                    Description = "卖出"
                };
                firstStoplossDirectionDropList.Add(dropItem);
            }


            this.Column_StoplossDirection.DataSource = firstStoplossDirectionDropList;
            this.Column_StoplossDirection.DisplayMember = "Description";
            this.Column_StoplossDirection.ValueMember = "DirectionID";

            //近月开仓价格类型
            this.Column_NearOpenStyle.Items.Clear();
            List<OpenPriceTypeDropItem> openNearPriceTypeDropList = new List<OpenPriceTypeDropItem>();
            {
                OpenPriceTypeDropItem empty = new OpenPriceTypeDropItem()
                {
                    MarketPriceType = ArbitrageOrderPriceType.Unknown,
                    MarketPriceTypeID = (int)ArbitrageOrderPriceType.Unknown,
                    Description = "请选择近月开仓价格类型"
                };
                openNearPriceTypeDropList.Add(empty);
            }

            {
                OpenPriceTypeDropItem dropItem = new OpenPriceTypeDropItem()
                {
                    MarketPriceType = ArbitrageOrderPriceType.LastPrice,
                    MarketPriceTypeID = (int)ArbitrageOrderPriceType.LastPrice,
                    Description = "最新价"
                };
                openNearPriceTypeDropList.Add(dropItem);
            }
            {
                OpenPriceTypeDropItem dropItem = new OpenPriceTypeDropItem()
                {
                    MarketPriceType = ArbitrageOrderPriceType.OpponentPrice,
                    MarketPriceTypeID = (int)ArbitrageOrderPriceType.OpponentPrice,
                    Description = "对手价"
                };
                openNearPriceTypeDropList.Add(dropItem);
            }
            {
                OpenPriceTypeDropItem dropItem = new OpenPriceTypeDropItem()
                {
                    MarketPriceType = ArbitrageOrderPriceType.QueuePrice,
                    MarketPriceTypeID = (int)ArbitrageOrderPriceType.QueuePrice,
                    Description = "排队价"
                };
                openNearPriceTypeDropList.Add(dropItem);
            }

            this.Column_NearOpenStyle.DataSource = openNearPriceTypeDropList;
            this.Column_NearOpenStyle.DisplayMember = "Description";
            this.Column_NearOpenStyle.ValueMember = "MarketPriceTypeID";

            //近月开仓价格类型
            this.Column_FarOpenStyle.Items.Clear();
            List<OpenPriceTypeDropItem> openFarPriceTypeDropList = new List<OpenPriceTypeDropItem>();
            {
                OpenPriceTypeDropItem empty = new OpenPriceTypeDropItem()
                {
                    MarketPriceType = ArbitrageOrderPriceType.Unknown,
                    MarketPriceTypeID = (int)ArbitrageOrderPriceType.Unknown,
                    Description = "请选择近月开仓价格类型"
                };
                openFarPriceTypeDropList.Add(empty);
            }

            {
                OpenPriceTypeDropItem dropItem = new OpenPriceTypeDropItem()
                {
                    MarketPriceType = ArbitrageOrderPriceType.LastPrice,
                    MarketPriceTypeID = (int)ArbitrageOrderPriceType.LastPrice,
                    Description = "最新价"
                };
                openFarPriceTypeDropList.Add(dropItem);
            }
            {
                OpenPriceTypeDropItem dropItem = new OpenPriceTypeDropItem()
                {
                    MarketPriceType = ArbitrageOrderPriceType.OpponentPrice,
                    MarketPriceTypeID = (int)ArbitrageOrderPriceType.OpponentPrice,
                    Description = "对手价"
                };
                openFarPriceTypeDropList.Add(dropItem);
            }
            {
                OpenPriceTypeDropItem dropItem = new OpenPriceTypeDropItem()
                {
                    MarketPriceType = ArbitrageOrderPriceType.QueuePrice,
                    MarketPriceTypeID = (int)ArbitrageOrderPriceType.QueuePrice,
                    Description = "排队价"
                };
                openFarPriceTypeDropList.Add(dropItem);
            }
            this.Column_FarOpenStyle.DataSource = openFarPriceTypeDropList;
            this.Column_FarOpenStyle.DisplayMember = "Description";
            this.Column_FarOpenStyle.ValueMember = "MarketPriceTypeID";

        }


        //private List<ArbitrageCombineOrderSetting> TestCreatArbitrageCombineOrderSetting()
        //{
        //    List<ArbitrageCombineOrderSetting> list = new List<ArbitrageCombineOrderSetting>();

        //    ArbitrageCombineOrderSetting firOrderSetting = new USeFuturesSpirit.ArbitrageCombineOrderSetting();
        //    USeProduct product = new USeProduct();
        //    product.ProductCode = "cu";
        //    product.Market = USe.TradeDriver.Common.USeMarket.SHFE;

        //    firOrderSetting.Product = product;
        //    firOrderSetting.OpenVolumn = 10;
        //    firOrderSetting.OpenVolumnPerNum = 2;
        //    firOrderSetting.OpenFirstDirection = USeDirection.Long;
        //    firOrderSetting.CloseFirstDirection = USeDirection.Long;
        //    firOrderSetting.StoplossFirstDirection = USeDirection.Long;
        //    firOrderSetting.NearPriceStyle = ArbitrageOrderPriceType.OpponentPrice;
        //    firOrderSetting.FarPriceStyle = ArbitrageOrderPriceType.OpponentPrice;

        //    list.Add(firOrderSetting);

        //    return list;
        //}

        #region 绑定的对象集合
        private class ProductDropItem
        {
            public USeProduct Product { get; set; }

            public string ProductName { get; set; }
            public string Description { get; set; }

            public override string ToString()
            {
                return this.Description;
            }
        }

        private class FistDirectionDropItem
        {
            public USeDirection Direction { get; set; }

            public int DirectionID { get; set; }
            public string Description
            {
                get;
                set;
            }

            public override string ToString()
            {
                return this.Description;
            }
        }

        private class OpenPriceTypeDropItem
        {
            public ArbitrageOrderPriceType MarketPriceType { get; set; }

            public int MarketPriceTypeID { get; set; }

            public string Description
            {
                get;
                set;
            }

            public override string ToString()
            {
                return this.Description;
            }
        }
        #endregion

        /// <summary>
        /// 格式错误处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
        }
    }
}
