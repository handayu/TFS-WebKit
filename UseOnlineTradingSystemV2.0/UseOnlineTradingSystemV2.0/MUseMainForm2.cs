using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;
using UseOnlineTradingSystem.Properties;

namespace UseOnlineTradingSystem
{
    public partial class MUseMainForm2 : DocumentWindow
    {
        private FormBlackWhite fm=new FormBlackWhite();
        private Bitmap bmp;
        private Pen pen = new Pen(Color.FromArgb(255, 135, 0));
        private DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //public RadControl SelectedControl;
        private MockIntegerDataSource dataSource1 = null;
        private MockIntegerDataSource dataSource2 = null;
        private FormHouse fh;//仓库信息
        private DelistBrandForm m_form;//摘牌
        public void SetColor()
        {
            if (infoCountLabel != null)
            {
                infoCountLabel.ForeColor = Color.Red;
            }
            if (infoAverageLabel != null)
            {
                infoAverageLabel.ForeColor = Color.Green;
            }
            if (radLblRefreshCount != null)
            {
                radLblRefreshCount.ForeColor = Color.White;
            }
            if (radLblAverage != null)
            {
                radLblAverage.ForeColor = Color.White;
            }
    }

        public MUseMainForm2():this("")
        {
        }

        public MUseMainForm2(string text):base(text)
        {
            InitializeComponent();
            int after = 40;
            bmp = new Bitmap((Resources.mark.Width*4)/5 + after, (Resources.mark.Height * 4) / 5);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(Resources.mark, after, 0,( Resources.mark.Width * 4) / 5, (Resources.mark.Height * 4) / 5);

            this.Text = text;

            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView1.MasterTemplate.AllowDeleteRow = false;
            this.radGridView1.MasterTemplate.AllowEditRow = false;
            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            this.radGridView1.AllowColumnHeaderContextMenu = false;
            this.radGridView1.AllowCellContextMenu = false;
            this.radGridView1.EnableSorting = false;
            this.radGridView1.EnableFiltering = false;
            this.radGridView1.EnableGrouping = false;
            this.radGridView1.AllowRowResize = false;

            this.radGridView2.MasterTemplate.AllowAddNewRow = false;
            this.radGridView2.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView2.MasterTemplate.AllowDeleteRow = false;
            this.radGridView2.MasterTemplate.AllowEditRow = false;
            this.radGridView2.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            this.radGridView2.AllowColumnHeaderContextMenu = false;
            this.radGridView2.AllowCellContextMenu = false;
            this.radGridView2.EnableSorting = false;
            this.radGridView2.EnableFiltering = false;
            this.radGridView2.EnableGrouping = false;
            this.radGridView2.AllowRowResize = false;

            // this.SelectedControl = this.radGridView1;
            SetColor();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            this.dataSource1 = new MockIntegerDataSource();
            this.radGridView1.CellValueNeeded += new GridViewCellValueEventHandler(radGridView1_CellValueNeeded);
            this.radGridView1.MouseDown +=new MouseEventHandler( RadGridView1_MouseDown);
            this.radGridView1.CellClick +=new GridViewCellEventHandler( RadGridView1_CellClick);
            this.radGridView1.CurrentRowChanging += RadGridView_CurrentRowChanging;
            radGridView1.VirtualMode = true;
            radGridView1.ColumnCount = this.dataSource1.Columns;
            this.radGridView1.RowCount = this.dataSource1.Rows;

            this.radGridView1.Columns[0] = new GridViewImageColumn();
            this.radGridView1.Columns[0].HeaderText = "标记";
            this.radGridView1.Columns[0].HeaderImage = bmp;
            this.radGridView1.Columns[1].HeaderText = "采购/销售";
            this.radGridView1.Columns[2].HeaderText = "等级";
            this.radGridView1.Columns[3].HeaderText = "参考合约";
            this.radGridView1.Columns[4].HeaderText = "升贴水(元/吨)";
            this.radGridView1.Columns[5].HeaderText = "绝对价格(元/吨)";
            this.radGridView1.Columns[6].HeaderText = "数量(吨)";
            this.radGridView1.Columns[7].HeaderText = "仓库";
            this.radGridView1.Columns[8].HeaderText = "公司";
            this.radGridView1.Columns[9].HeaderText = "备注";
            this.radGridView1.Columns[10].HeaderText = "发布时间";
            for (int i = 0; i < radGridView1.ColumnCount; i++)
            {
                this.radGridView1.Columns[i].TextAlignment = ContentAlignment.MiddleCenter;
            }

            this.dataSource2 = new MockIntegerDataSource();
            this.radGridView2.CellValueNeeded += new GridViewCellValueEventHandler(radGridView2_CellValueNeeded);
            radGridView2.VirtualMode = true;
            radGridView2.ColumnCount = this.dataSource2.Columns;
            this.radGridView2.RowCount = this.dataSource2.Rows;
            this.radGridView2.CurrentRowChanging += RadGridView_CurrentRowChanging;
            this.radGridView2.ShowColumnHeaders = false;
            this.radGridView2.Columns[0] = new GridViewImageColumn();
            this.radGridView2.Columns[0].HeaderText = "标记";
            this.radGridView2.Columns[1].HeaderText = "采购/销售";
            this.radGridView2.Columns[2].HeaderText = "等级";
            this.radGridView2.Columns[3].HeaderText = "参考合约";
            this.radGridView2.Columns[4].HeaderText = "升贴水(元/吨)";
            this.radGridView2.Columns[5].HeaderText = "绝对价格(元/吨)";
            this.radGridView2.Columns[6].HeaderText = "数量(吨)";
            this.radGridView2.Columns[7].HeaderText = "仓库";
            this.radGridView2.Columns[8].HeaderText = "公司";
            this.radGridView2.Columns[9].HeaderText = "备注";
            this.radGridView2.Columns[10].HeaderText = "发布时间";
            for (int i = 0; i < radGridView2.ColumnCount; i++)
            {
                this.radGridView2.Columns[i].TextAlignment = ContentAlignment.MiddleCenter;
            }
            UpdateTable();
        }

        private void RadGridView_CurrentRowChanging(object sender, CurrentRowChangingEventArgs e)
        {
            e.Cancel=true;
        }

        private void RadGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0&&e.RowIndex==-1)
            {
                fm.Location = new Point(MousePosition.X-1, MousePosition.Y-1); 
                fm.Show();
            }
        }
        private void RadGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            //鼠标左键点击
            if (e.Button == MouseButtons.Left)
            {
                //GridHitInfo gridHitInfo = radGridView1 .CalcHitInfo(e.X, e.Y);
                ////在列标题栏内且列标题name是"colName"
                //if (gridHitInfo.InColumnPanel && gridHitInfo.Column.Name == "colName")
                //{
                //    //获取该列右边线的x坐标
                //    GridViewInfo gridViewInfo = (GridViewInfo)this.gridView.GetViewInfo();
                //    int x = gridViewInfo.GetColumnLeftCoord(gridHitInfo.Column) + gridHitInfo.Column.Width;
                //    //右边线向左移动3个像素位置不弹出对话框（实验证明3个像素是正好的）
                //    if (e.X < x - 3)
                //    {
                //        MessageBox.Show("点击Name列标题！");
                //    }
                //}
            }
        }

        void radGridView1_CellValueNeeded(object sender, GridViewCellValueEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.ColumnIndex < dataSource1.Columns)
            {
                OneListed info = this.dataSource1.Source[e.RowIndex];
                SetData(e, info);
            }
        }
        void radGridView2_CellValueNeeded(object sender, GridViewCellValueEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.ColumnIndex < dataSource2.Columns)
            {
                OneListed info = this.dataSource2.Source[e.RowIndex];
                SetData(e, info);
                //Type type = one.GetType(); //获取类型
                //System.Reflection.PropertyInfo propertyInfo = type.GetProperty(e.Column.Name); //获取指定名称的属性
                //e.Value = propertyInfo.GetValue(one, null); //获取属性
            }
        }
        private void SetData(GridViewCellValueEventArgs e, OneListed info)
        {
            switch (e.ColumnIndex)
            {
                case 0:
                    {
                        if (DataManager.Instance.LoginData != null)
                        {
                            if (info.createdBy == DataManager.Instance.LoginData.id)
                            {
                                e.Value = mPaint.MCommonData.Publish;
                            }
                            else if (DataManager.Instance.WhiteEnable && DataManager.Instance.WhiteDY != null && info.publisher != null && DataManager.Instance.WhiteDY.ContainsKey(info.publisher))
                            {
                                e.Value = mPaint.MCommonData.White;
                            }
                            else if (DataManager.Instance.BlackEnable && DataManager.Instance.BlackDY != null && info.publisher != null && DataManager.Instance.BlackDY.ContainsKey(info.publisher))
                            {
                                e.Value = mPaint.MCommonData.Black;
                            }
                            else
                            {
                                e.Value = null;
                            }
                        }

                    }
                     break;
                case 1: e.Value = info.transTypeName; break;
                case 2: e.Value = info.commLevelName; break;
                case 3:
                    {
                        if (string.IsNullOrWhiteSpace(info.contract))
                        {
                            e.Value = "--";
                        }
                        else
                        {
                            e.Value = info.contract;
                        }
                    }
                    break;
                case 4:
                    {
                        if (string.IsNullOrWhiteSpace(info.premium))
                        {
                            e.Value = "--";
                        }
                        else
                        {
                            e.Value = info.premium;
                        }
                    }
                    break;
                case 5:
                    {
                        if (info.pricingMethod == "0" && info.premium != null)
                        {
                            var vvv = DataManager.Instance.GetContractLastPrice(info.contract);
                            if (vvv != null)
                            {
                                int premium;
                                int.TryParse(info.premium, out premium);
                                if (premium != 0)
                                {
                                    if (info.transType == 0)
                                    {
                                        info.fixedPrice = (vvv.bidPrice + premium).ToString();
                                    }
                                    else
                                    {
                                        info.fixedPrice = (vvv.askPrice + premium).ToString();
                                    }
                                }
                            }
                        }
                        decimal money;
                        decimal.TryParse(info.fixedPrice, out money);
                        if (money > 0)
                        {
                            e.Value = string.Format("{0:C}", money);
                        }
                        else
                        {
                            e.Value = "";
                        }
                    }
                    break;
                case 6: e.Value = info.commAvailableQuantity; break;
                case 7:
                    {
                        string text = info.warehouseName;
                        if (text != null && text.Length >= 6)
                        {
                            text = text.Substring(0, 6) + "...";
                        }
                        e.Value = text;
                    }
                    break;
                case 8: e.Value = info.publisher; break;
                case 9: e.Value = info.remarks; break;
                case 10:
                    {
                        long l;
                        long.TryParse(info.publisherDate, out l);
                        e.Value = start.AddMilliseconds(l).ToLocalTime().ToString("HH:mm:ss");
                    }
                    break;
                default: break;
            }
        }

        private void Refresh(object sender, EventArgs args)
        {
            if (!this.radGridView1.IsDisposed)
            {
                this.dataSource1.Refresh();
                this.radGridView1.MasterTemplate.Refresh();

                this.dataSource2.Refresh();
                this.radGridView2.MasterTemplate.Refresh();
            }
        }
        private void Rb_Click(object sender, System.EventArgs e)
        {
            Refresh(sender,e);
        }

        private void AdjustmentGrid()
        {
            if (dataSource1 != null && dataSource2 != null)
            {
                int h1 = (dataSource1.Rows+1) * 26;
                int w1 = Width;
                bool gd1 = false;
                int h2 = dataSource2.Rows * 26;
                int w2 = Width;
                bool gd2 = false;
                if (h1 == 0)
                {
                    h1 =26;
                }
                if (h2 == 0)
                {
                    h2 = (Height * 2) / 3;
                }
                if (h1 > Height / 3)
                {
                    //出现滚动条
                    h1 = Height / 3;
                    gd1 = true;
                }
                if (h2 > (Height * 2) / 3)
                {
                    //出现滚动条
                    h2 = (Height * 2) / 3;
                    gd2 = true;
                }
                if (gd1 && !gd2)
                {
                    w2 -= 15;
                }
                else if (!gd1 && gd2)
                {
                    w1 -= 15;
                }

                radPanel1.Size = new Size(Width, 40);

                radPanel2.Location = new Point(0, radPanel1.Height);
                radPanel2.Size = new Size(w1, h1);

                radPanel3.Location = new Point(0, radPanel2.Location.Y + radPanel2.Height + 1);
                radPanel3.Size = new Size(w2, h2);
            }
        }

        private void Form2_SizeChanged(object sender, System.EventArgs e)
        {
            if (dataSource1 != null && dataSource2 != null)
            {
                AdjustmentGrid();
                int avg = 5;
                int avg2 = 10;
                int beginX = radPanel1.Width - infoCountLabel.Width - radLblRefreshCount.Width - infoAverageLabel.Width - radLblAverage.Width - avg * 2 - avg2 - 10;
                infoCountLabel.Location = new Point(beginX, 10);
                beginX += this.infoCountLabel.Width + avg;
                radLblRefreshCount.Location = new Point(beginX, 2);
                beginX += this.radLblRefreshCount.Width + avg2;
                infoAverageLabel.Location = new Point(beginX, 10);
                beginX += this.infoAverageLabel.Width + avg;
                radLblAverage.Location = new Point(beginX, 2);

                radDropDownButton1.Location = new Point(infoCountLabel.Location.X - radDropDownButton1.Width - 10, 0);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int lastx1 = radPanel3.Location.X;
            int lasty1 = radPanel3.Location.Y - 1;
            int lastx2 = radPanel3.Width;
            int lasty2 = radPanel3.Location.Y - 1;
            e.Graphics.DrawLine(pen, lastx1, lasty1, lastx2, lasty2);
        }

        public void UpdateTable()
        {
            if (DataManager.Instance.CurrentCategory != null&& dataSource1!=null&& dataSource2!=null)
            {
                var current = DataManager.Instance.CurrentCategory;
                List<OneListed> list = DataManager.Instance.SortCommodityData();
                dataSource1.ClearData();
                dataSource2.ClearData();
                foreach (var v in list)
                {
                    if (current.id == v.cid)
                    {
                        if (v.transStatus == "1" || v.transStatus == "2")
                        {
                            //int transType;//卖和买。 0：采购 1：销售
                            if (v.transType == 1)
                            {
                                //1：销售
                                dataSource1.AddData(v);
                            }
                            else if (v.transType == 0)
                            {
                                // 0：采购
                                dataSource2.AddData(v);
                            }
                        }
                    }
                }
                this.radGridView1.RowCount = this.dataSource1.Rows;
                this.radGridView1.MasterTemplate.Refresh();
                this.radGridView2.RowCount = this.dataSource2.Rows;
                this.radGridView2.MasterTemplate.Refresh();
                AdjustmentGrid();
            }
        }

        public void UpdateSelect(ContractCategoryDic cv)
        {
            radDropDownButton1.Items.Clear();
            if (cv != null)
            {
                foreach (var v in cv.contractMonthMap)
                {
                    RadMenuHeaderItem item = new RadMenuHeaderItem("       " +v.Value.categoryName+ "                 " );
                    item.Tag = v.Value;
                    item.Click +=new EventHandler(Item_Click);
                    radDropDownButton1.Items.Add(item);
                }
            }
            if (radDropDownButton1.Items.Count > 0)
            {
                Item_Click(radDropDownButton1.Items[0], null);
                radDropDownButton1.Text = radDropDownButton1.Items[0].Text;
            }
            else
            {
                radDropDownButton1.Text = "请选择";
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            RadMenuHeaderItem ri = sender as RadMenuHeaderItem;
            if (ri != null)
            {
                radDropDownButton1.Text = ri.Text;
                ContractBasePrice cp = ri.Tag as ContractBasePrice;
                if (cp != null)
                {
                    DataManager.Instance.CurrentContractCode = cp.category;
                    //立刻取一次更新
                    if (radDropDownButton1.Text != "请选择")
                    {
                        var d = DataManager.Instance.GetContractLastPrice(cp.category);
                        UpdateMarketData(d);
                    }
                    else
                    {
                        UpdateMarketData(null);
                    }
                }
            }
        }

        public void UpdateMarketData(ContractLastPrice obj)
        {
            if (DataManager.Instance.CurrentCode != null && obj != null)
            {
                if (DataManager.Instance.CurrentCode == obj.category && DataManager.Instance.CurrentContractCode == obj.contractMonth)
                {
                    if (obj != null)
                    {
                        if (radLblRefreshCount != null && radLblAverage != null)
                        {
                            if (this.InvokeRequired)
                            {
                                this.BeginInvoke((MethodInvoker)delegate
                                {
                                    radLblRefreshCount.Text = obj.bidPrice.ToString();
                                    radLblAverage.Text = obj.askPrice.ToString();
                                });
                            }
                            else
                            {
                                radLblRefreshCount.Text = obj.bidPrice.ToString();
                                radLblAverage.Text = obj.askPrice.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke((MethodInvoker)delegate
                            {
                                radLblRefreshCount.Text = "0";
                                radLblAverage.Text = "0";
                            });
                        }
                        else
                        {
                            radLblRefreshCount.Text = "0";
                            radLblAverage.Text = "0";
                        }
                        Updata("", 0, 0);
                    }
                }
                //if (this.PublishControl1 != null)
                //{
                //    this.PublishControl1.TradingInfoCtrol.UpdateData(obj);
                //}
                Updata(obj.category, obj.bidPrice, obj.askPrice);
            }
        }

        public void UpdateCommodity(OneListed obj, int type)
        {
            if (dataSource1 == null && dataSource2 == null) return;
            var current = DataManager.Instance.CurrentCategory;
            if (obj != null && current != null && current.id == obj.cid)
            {
                //新增
                if (type == 0)
                {
                    //买
                    dataSource2.AddData(obj);
                    this.radGridView2.RowCount = this.dataSource2.Rows;
                    this.radGridView2.MasterTemplate.Refresh();
                }
                else if (type == 1)
                {
                    //卖
                    dataSource1.AddData(obj);
                    this.radGridView1.RowCount = this.dataSource1.Rows;
                    this.radGridView1.MasterTemplate.Refresh();
                }
                else if (type == 2)
                {
                    //更新
                    if (dataSource1.Update(obj))
                    {
                        this.radGridView1.MasterTemplate.Refresh();
                    }
                    else if (dataSource2.Update(obj))
                    {
                        this.radGridView2.MasterTemplate.Refresh();
                    }
                }
                AdjustmentGrid();
            }
        }

        public void Updata(string category, float buyPrice, float sellPrice)
        {
            if (sellPrice != 0&& dataSource1!=null)
            {
                bool flag = false;
                for (int i = 0; i < dataSource1.Rows; i++)
                {
                    OneListed ci = this.dataSource1.Source[i];
                    if (ci != null)
                    {
                        if (ci.pricingMethod == "0" && ci.premium != null && ci.contract != null && category.EndsWith(ci.contract))
                        {
                            int premium;
                            int.TryParse(ci.premium, out premium);
                            if (premium != 0)
                            {
                                ci.fixedPrice = (sellPrice + premium).ToString();
                                flag = true;
                            }
                        }
                    }
                }
                if (flag)
                {
                    this.radGridView1.MasterTemplate.Refresh();
                }
            }
            if (buyPrice != 0&& dataSource2 != null)
            {
                bool flag = false;
                for (int i = 0; i < dataSource2.Rows; i++)
                {
                    OneListed ci = this.dataSource2.Source[i];
                    if (ci != null)
                    {
                        if (ci.pricingMethod == "0" && ci.premium != null && ci.contract != null && category.EndsWith(ci.contract))
                        {
                            int premium;
                            int.TryParse(ci.premium, out premium);
                            if (premium != 0)
                            {
                                ci.fixedPrice = (buyPrice + premium).ToString();
                                flag = true;
                            }
                        }
                    }
                }
                if (flag)
                {
                    this.radGridView2.MasterTemplate.Refresh();
                }
            }
        }
    }

    interface IMockDataSource<T>
    {
        List<T> Source { get; }
        // void Refresh();
        bool Update(OneListed one);
        void ClearData();
        void AddData(OneListed one);
        int Rows { get; }
        int Columns { get; }
    }

    public class MockIntegerDataSource : IMockDataSource<OneListed>
    {
        private List<OneListed> data=new List<OneListed>();

        public MockIntegerDataSource()
        {
            //this.rows = rows;
            //this.columns = columns;
            //this.Refresh();
        }
        public bool Update(OneListed one)
        {
            if (one != null)
            {
                foreach (var v in data)
                {
                    if (v.id == one.id)
                    {
                        v.Update(one);
                        return true;
                    }
                }
            }
            return false;
        }

        public void ClearData()
        {
            data.Clear();
        }

        public void AddData(OneListed one)
        {
            data.Add(one);
        }
        public void Refresh()
        {
            //data.Clear();
            //for (int i = 0; i < rows; i++)
            //{
            //    WorkItem temp = new WorkItem(columns);
            //    for (int j = 0; j < columns; j++)
            //    {
            //        temp.Random();
            //    }
            //    this.data.Add(temp);
            //}
        }

        public List<OneListed> Source
        {
            get
            {
                return data;
            }
        }

        public int Columns
        {
            get
            {
                return 11;
            }
        }

        public int Rows
        {
            get
            {
                return this.data.Count;
            }
        }
    }
}
