using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using UseHttpHelper;

namespace UseOnlineTradingSystem
{
    public partial class TradingInfoControl : UserControl
    {
        private string m_nowProcessId = string.Empty;   //目前处理的商品ID
        private ContractCategoryDic m_contractVo = null;//目前处理的contractVo

        #region 绑定数据源
        //挂牌
        private BindingList<SelfListed> m_commodityInfoList = new BindingList<SelfListed>();

        //摘牌
        private BindingList<Transaction> m_delistBrandInfoList = new BindingList<Transaction>();

        //成交
        private BindingList<Transaction> m_tradedBrandInfoList = new BindingList<Transaction>();

        #endregion

        public TradingInfoControl()
        {
            InitializeComponent();
            //Initialize();
        }

        private void TradingInfoControl_SizeChanged(object sender, System.EventArgs e)
        {
            int w = this.Width - 35;
            int h = this.Height - 40;
            this.panel2.Size = new System.Drawing.Size(w, h);
        }

        private void UpdateDelistData(ContractLastPrice lastPrice)
        {
            if (lastPrice == null) return;

            foreach (Transaction dalist in m_delistBrandInfoList)
            {
                if (dalist.contract != lastPrice.contractMonth) continue;
                int premium;
                int.TryParse(dalist.premium, out premium);
                if (premium != 0)
                {
                    if (dalist.transType == "0")
                    {
                        dalist.fixedPrice = (lastPrice.bidPrice + premium).ToString();
                    }
                    else
                    {
                        dalist.fixedPrice = (lastPrice.askPrice + premium).ToString();
                    }
                }
            }
        }

        private void UpdateTradedData(ContractLastPrice lastPrice)
        {
            if (lastPrice == null) return;
            foreach (Transaction ts in m_tradedBrandInfoList)
            {
                if (ts.contract != lastPrice.contractMonth) continue;
                int premium;
                int.TryParse(ts.premium, out premium);
                if (premium != 0)
                {
                    if (ts.transType == "0")
                    {
                        ts.fixedPrice = (lastPrice.bidPrice + premium).ToString();
                    }
                    else
                    {
                        ts.fixedPrice = (lastPrice.askPrice + premium).ToString();
                    }
                }
            }
        }


        /// <summary>
        /// 更新条目绝对价格
        /// </summary>
        public void UpdateData(ContractLastPrice lastPrice)
        {
            UpdateDelistData(lastPrice);
            UpdateTradedData(lastPrice);
        }

        /// <summary>
        /// 核销事件订阅-刷新挂牌-摘牌-成交
        /// </summary>
        /// <param name="obj"></param>
        private void MQTTService_NoticeWriteOffEvent1(WriteOff obj)
        {
            //查询挂牌信息拉全量
            InitializePutBrandList(m_contractVo);

            //刷新摘牌
            InitializeDelistBrandList(m_contractVo);

            //刷新成交
            InitializeTradedBrandList(m_contractVo);
        }

        private void DataGridView_PutBrand_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView gv = sender as DataGridView;
            if (gv != null)
            {
                DataGridView.HitTestInfo info = gv.HitTest(e.X, e.Y);
                if (info.ColumnIndex < 0 || info.RowIndex < 0)
                    return;

                DataGridViewCell cell = gv[info.ColumnIndex, info.RowIndex];
                if (cell.Value == null)
                    return;
                cell.Selected = true;
            }
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView gv = sender as DataGridView;
            if (gv != null)
            {
                //gv.ClearSelection();
            }
        }

        /// <summary>
        /// 外部事件
        /// </summary>
        /// <param name="contractVo"></param>
        public void OnContractcCategoryVoChanged(ContractCategoryDic contractVo)
        {
            //获得contract的ID作为Key,根据ID获取LevelID,BrandID,WareHouse
            if (contractVo == null) return;

            m_contractVo = contractVo;

            m_nowProcessId = contractVo.id;
            Debug.Assert(m_nowProcessId != "" && m_nowProcessId != null);

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<ContractCategoryDic>(OnContractcCategoryVoChanged), contractVo);
                return;
            }

            //清空三个表/查询响应品类的挂牌-摘牌-成交/添加
            m_commodityInfoList.Clear();
            m_delistBrandInfoList.Clear();
            m_tradedBrandInfoList.Clear();

            InitializePutBrandList(contractVo);
            InitializeDelistBrandList(contractVo);
            InitializeTradedBrandList(contractVo);

        }

        /// <summary>
        /// 挂牌成功之后数据刷新
        /// </summary>
        public void OnPublishSuccessChangedEvent(object sender, EventArgs e)
        {
            InitializePutBrandList(m_contractVo);
        }

        /// <summary>
        /// 摘牌成功之后跳转到摘牌Tab页面并且数据刷新
        /// </summary>
        public void OnDelistSuccessChangedEvent(object sender, EventArgs e)
        {
            button_OutBrand_Click(this.button_OutBrand, null);

            Debug.Assert(m_contractVo != null);
            InitializeDelistBrandList(m_contractVo);
        }

        /// <summary>
        /// 摘牌核销成功之后成交列表的刷新
        /// </summary>
        public void OnDelistCheckedSuccessChangeEvet(object sender, EventArgs e)
        {
            InitializeTradedBrandList(m_contractVo);
        }

        #region 切换品类加载全量挂牌-摘牌-成交列表
        /// <summary>
        /// 查询自己的挂牌列表
        /// </summary>
        public void InitializePutBrandList(ContractCategoryDic contractVo)
        {
            Debug.Assert(contractVo != null && contractVo.id != "");

            m_commodityInfoList.Clear();
            var selfListedList = HttpService.GetSelfListedList(contractVo.id);
            if (selfListedList != null)
            {
                try
                {
                    foreach (SelfListed info in selfListedList)
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
                                    if (info.transType == "0")
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
                        m_commodityInfoList.Add(info);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("获取挂牌列表发生异常请重试");
                    Logger.LogError(err.ToString());
                }
            }
            dataGridView_PutBrand.ClearSelection();
        }

        /// <summary>
        /// 查询成交列表
        /// </summary>
        public void InitializeTradedBrandList(ContractCategoryDic contractVo)
        {
            Debug.Assert(contractVo != null && contractVo.id != "");
            if (contractVo == null && contractVo.id == "")
            {
                Logger.LogError("contractVo为空");
                return;
            }
            m_tradedBrandInfoList.Clear();
            List<Transaction> list = HttpService.GetTransactionList("2", contractVo.id);
            if (list != null)
            {
                foreach (Transaction data in list)
                {
                    m_tradedBrandInfoList.Add(data);
                    UpdateTradedData(DataManager.Instance.GetContractLastPrice(data.contract));
                }
            }
            else
            {
                MessageBox.Show("获取成交列表发生异常请重试");
            }
            dataGridView_Traded.ClearSelection();
        }

        /// <summary>
        /// 查询摘牌列表
        /// </summary>
        public void InitializeDelistBrandList(ContractCategoryDic contractVo)
        {
            Debug.Assert(contractVo != null && contractVo.id != "");
            if (contractVo == null && contractVo.id == "")
            {
                Logger.LogError("contractVo为空");
                return;
            }
            m_delistBrandInfoList.Clear();
            List<Transaction> list = HttpService.GetTransactionList("1", contractVo.id);
            if (list != null)
            {
                foreach (Transaction data in list)
                {
                    m_delistBrandInfoList.Add(data);
                    UpdateDelistData(DataManager.Instance.GetContractLastPrice(data.contract));
                }
            }
            else
            {
                MessageBox.Show("获取摘牌列表发生异常请重试");
            }
            dataGridView_OutBrand.ClearSelection();
        }

        #endregion

        /// <summary>
        ///界面初始化-绑定数据源
        /// </summary>
        public void Initialize()
        {
            InitializeArguments();

            //挂牌事件数据源绑定
            //DataManager.Instance.UpdataCommodityInfoEvent += MQTTService_UpdataCommodityInfoEvent;
            this.dataGridView_PutBrand.AutoGenerateColumns = false;//不自动  
            this.dataGridView_PutBrand.DataSource = m_commodityInfoList;

            //摘牌事件数据源绑定
            DataManager.Instance.UpdataDelistingEvent += MQTTService_UpdataDelistingEvent; ;
            this.dataGridView_OutBrand.AutoGenerateColumns = false;//不自动  
            this.dataGridView_OutBrand.DataSource = m_delistBrandInfoList;

            //成交事件数据源绑定
            this.dataGridView_Traded.AutoGenerateColumns = false;//不自动  
            this.dataGridView_Traded.DataSource = m_tradedBrandInfoList;

            USeManager.Instance.MQTTService.NoticeWriteOffEvent += MQTTService_NoticeWriteOffEvent;

            //样式修改
            //挂牌列表
            dataGridView_PutBrand.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(26, 27, 27);//列头
            dataGridView_PutBrand.RowsDefaultCellStyle.BackColor = Color.FromArgb(7, 8, 8);//行 背景色
            dataGridView_PutBrand.RowsDefaultCellStyle.ForeColor = Color.White;//行 前景色
            dataGridView_PutBrand.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView_PutBrand.AlternatingRowsDefaultCellStyle = null;
            dataGridView_PutBrand.DefaultCellStyle.SelectionBackColor = Color.FromArgb(44, 47, 51);
            dataGridView_PutBrand.DefaultCellStyle.SelectionForeColor = Color.White;
            for (int i = 0; i < dataGridView_PutBrand.Columns.Count; i++)
            {
                dataGridView_PutBrand.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            dataGridView_PutBrand.MouseMove += DataGridView_PutBrand_MouseMove;

            //摘牌列表
            dataGridView_Traded.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(26, 27, 27);
            dataGridView_Traded.RowsDefaultCellStyle.BackColor = Color.FromArgb(7, 8, 8);//行 背景色
            dataGridView_Traded.RowsDefaultCellStyle.ForeColor = Color.White;//行 前景色
            dataGridView_Traded.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView_Traded.AlternatingRowsDefaultCellStyle = null;
            dataGridView_Traded.DefaultCellStyle.SelectionBackColor = Color.FromArgb(44, 47, 51);
            dataGridView_Traded.DefaultCellStyle.SelectionForeColor = Color.White;
            for (int i = 0; i < dataGridView_Traded.Columns.Count; i++)
            {
                //dataGridView_Traded.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            dataGridView_Traded.MouseMove += DataGridView_PutBrand_MouseMove;

            //成交列表
            dataGridView_OutBrand.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(26, 27, 27);
            dataGridView_OutBrand.RowsDefaultCellStyle.BackColor = Color.FromArgb(7, 8, 8);//行 背景色
            dataGridView_OutBrand.RowsDefaultCellStyle.ForeColor = Color.White;//行 前景色
            dataGridView_OutBrand.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView_OutBrand.AlternatingRowsDefaultCellStyle = null;
            dataGridView_OutBrand.DefaultCellStyle.SelectionBackColor = Color.FromArgb(44, 47, 51);
            dataGridView_OutBrand.DefaultCellStyle.SelectionForeColor = Color.White;
            for (int i = 0; i < dataGridView_OutBrand.Columns.Count; i++)
            {
                dataGridView_OutBrand.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            dataGridView_OutBrand.MouseMove += DataGridView_PutBrand_MouseMove;

            USeManager.Instance.MQTTService.NoticeWriteOffEvent += MQTTService_NoticeWriteOffEvent1;
        }

        /// <summary>
        /// 订阅摘牌核销成功的消息
        /// </summary>
        /// <param name="obj"></param>
        private void MQTTService_NoticeWriteOffEvent(WriteOff obj)
        {
            if (obj == null) return;
            //核销成功
            if (obj.confirmStatus == "4")
            {

                //刷新摘牌
                InitializeDelistBrandList(m_contractVo);
                //刷新成交列表
                InitializeTradedBrandList(m_contractVo);
            }
            else
            {
                InitializeDelistBrandList(m_contractVo);
            }

        }

        /// <summary>
        /// 登陆调用
        /// </summary>
        public void SetDefultListWhenLogin(ContractCategoryDic contractVo)
        {
            OnContractcCategoryVoChanged(contractVo);
        }

        /// <summary>
        /// 挂牌事件订阅
        /// </summary>
        /// <param name="obj"></param>
        private void MQTTService_UpdataCommodityInfoEvent(SelfListed obj, int type)
        {
            //根据返回来的状态选择绑定的表对象上展示
            if (obj == null) return;
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<SelfListed, int>(MQTTService_UpdataCommodityInfoEvent), obj);
                return;
            }

            //挂牌更新
            if (/*obj. == Helper.GetDescription(OperationType.PutBrand)*/true)
            {
                UpdataPutBrandCommodityInfo(obj);

            }
        }

        /// <summary>
        /// 摘牌事件订阅
        /// </summary>
        /// <param name="obj"></param>
        private void MQTTService_UpdataDelistingEvent(Transaction obj)
        {
            //根据返回来的状态选择绑定的表对象上展示
            if (obj == null) return;
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<Transaction>(MQTTService_UpdataDelistingEvent), obj);
                return;
            }

            //根据返回来的增量事件信息，更新表单信息(增加-删除-更新状态)
            //if (obj.transStatus == Helper.GetDescription(OperationType.PutBrand))
            //{
            //    UpdataPutBrandCommodityInfo(obj);
            //}
        }

        /// <summary>
        /// 更新挂牌信息
        /// </summary>
        /// <param name="obj"></param>
        private void UpdataPutBrandCommodityInfo(SelfListed obj)
        {
            m_commodityInfoList.Add(obj);
        }

        /// <summary>
        /// 初始化默认为挂牌
        /// </summary>
        private void InitializeArguments()
        {
            ChangeButtonBackColor(this.button_PutBrand);

            this.dataGridView_OutBrand.Visible = false;
            this.dataGridView_Traded.Visible = false;
            this.dataGridView_PutBrand.Visible = true;
        }

        #region 点击切换表事件
        /// <summary>
        /// 点击挂牌显示展现挂牌列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_PutBrand_Click(object sender, EventArgs e)
        {
            ChangeButtonBackColor((Button)sender);

            this.dataGridView_OutBrand.Visible = false;
            this.dataGridView_Traded.Visible = false;
            this.dataGridView_PutBrand.Visible = true;

            //查询挂牌信息拉全量
            InitializePutBrandList(m_contractVo);
        }

        /// <summary>
        /// 点击摘牌显示展现摘牌列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_OutBrand_Click(object sender, EventArgs e)
        {
            ChangeButtonBackColor((Button)sender);

            this.dataGridView_OutBrand.Visible = true;
            this.dataGridView_Traded.Visible = false;
            this.dataGridView_PutBrand.Visible = false;

            //刷新摘牌
            InitializeDelistBrandList(m_contractVo);

        }

        /// <summary>
        /// 点击成交显示展现成交列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Traded_Click(object sender, EventArgs e)
        {
            ChangeButtonBackColor((Button)sender);

            this.dataGridView_OutBrand.Visible = false;
            this.dataGridView_Traded.Visible = true;
            this.dataGridView_PutBrand.Visible = false;

            this.dataGridView_Traded.Dock = DockStyle.Fill;

            //刷新成交
            InitializeTradedBrandList(m_contractVo);
        }

        /// <summary>
        /// 更改按钮背景色
        /// </summary>
        /// <param name="button"></param>
        private void ChangeButtonBackColor(Button button)
        {
            if (button == this.button_PutBrand)
            {
                this.button_PutBrand.BackColor = Color.Silver;
                this.button_OutBrand.BackColor = Color.Black;
                this.button_Traded.BackColor = Color.Black;
            }
            else if (button == this.button_OutBrand)
            {
                this.button_OutBrand.BackColor = Color.Silver;
                this.button_PutBrand.BackColor = Color.Black;
                this.button_Traded.BackColor = Color.Black;
            }
            else
            {
                this.button_OutBrand.BackColor = Color.Black;
                this.button_PutBrand.BackColor = Color.Black;
                this.button_Traded.BackColor = Color.Silver;
            }
        }

        #endregion

        #region 更改背景色事件
        /// <summary>
        /// 鼠标移动变背景色事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMovePutBrand(object sender, MouseEventArgs e)
        {
            //this.button_PutBrand.BackColor = Color.Silver;
        }

        private void MouseLeavePutBrand(object sender, EventArgs e)
        {
            //this.button_PutBrand.BackColor = Color.Black;
        }

        private void MouseLeaveOutBrand(object sender, EventArgs e)
        {
            //this.button_OutBrand.BackColor = Color.Black;
        }

        private void MouseMoveOutBrand(object sender, MouseEventArgs e)
        {
            //this.button_OutBrand.BackColor = Color.Silver;
        }

        private void MouseMoveTraded(object sender, MouseEventArgs e)
        {
            //this.button_Traded.BackColor = Color.Silver;
        }

        private void MouseLeaveTraded(object sender, EventArgs e)
        {
            //this.button_Traded.BackColor = Color.Black;
        }


        #endregion

        /// <summary>
        /// 挂牌右击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellMouseDownPutBrand(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (e.Button == MouseButtons.Right)
            {
                SelfListed dataInfo = this.dataGridView_PutBrand.Rows[e.RowIndex].DataBoundItem as SelfListed;
                Debug.Assert(dataInfo != null);

                //挂牌的Cell右键Menu 
                if ((DataGridView)sender == this.dataGridView_PutBrand)
                {
                    UseMenuTrip trip = new UseMenuTrip(OperationType.PutBrand, Cursor.Position, m_contractVo, dataInfo, null, this);
                }

            }
        }

        /// <summary>
        /// 摘牌表右击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellMouseDownDelistBrand(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (e.Button == MouseButtons.Right)
            {
                Transaction dataInfo = this.dataGridView_OutBrand.Rows[e.RowIndex].DataBoundItem as Transaction;
                Debug.Assert(dataInfo != null);

                if ((DataGridView)sender == this.dataGridView_OutBrand)
                {
                    //摘牌的Cell右键Menu
                    UseMenuTrip trip = new UseMenuTrip(OperationType.DelistBrand, Cursor.Position, m_contractVo, null, dataInfo, this);
                }
            }
        }

        private void DelistDataErrorEvent(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void TradedMouseMove_Imformation(object sender, MouseEventArgs e)
        {

        }

        private ImformationTips form = null;
        /// <summary>
        /// 成交表的CellMouseMove事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TradedCellMouseMove_Imformation(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            //交易对手列
            if (e.ColumnIndex == 9)
            {
                Transaction dataInfo = this.dataGridView_Traded.Rows[e.RowIndex].DataBoundItem as Transaction;
                if (dataInfo == null) return;
                if (form != null) return;

                form = new ImformationTips();
                form.Initialize(dataInfo);
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(Cursor.Position.X - form.Width, Cursor.Position.Y);
                form.Show();
            }
        }

        private void TradedCellMouseLeave_Imformation(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            //交易对手列
            if (e.ColumnIndex == 9 && form != null)
            {
                form.Close();
                form = null;
            }
        }

        /// <summary>
        /// 成交列表的仓库点击URl-4列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TradedCellContentClick_WareHouse(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (e.ColumnIndex == 4)
            {
                Transaction dataInfo = this.dataGridView_Traded.Rows[e.RowIndex].DataBoundItem as Transaction;
                if (dataInfo == null) return;

                string url = Helper.GetURL(HTTPServiceUrlCollection.GetWareHouseInfoUrl, dataInfo.warehouseId);
                FormHouse fh = new FormHouse();
                fh.SetHouse(url);
                fh.Show();
            }
        }

        /// <summary>
        /// 摘牌仓库点击URL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OuPutCellContentClick_WareHouse(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (e.ColumnIndex == 4)
            {
                Transaction dataInfo = this.dataGridView_OutBrand.Rows[e.RowIndex].DataBoundItem as Transaction;
                if (dataInfo == null) return;

                string url = Helper.GetURL(HTTPServiceUrlCollection.GetWareHouseInfoUrl, dataInfo.warehouseId);
                FormHouse fh = new FormHouse();
                fh.SetHouse(url);
                fh.Show();
            }
        }

        /// <summary>
        /// 挂牌仓库点击URL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellContentClick_PutBrand(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (e.ColumnIndex == 4)
            {
                this.dataGridView_PutBrand.Cursor = Cursors.Hand;
                SelfListed dataInfo = this.dataGridView_PutBrand.Rows[e.RowIndex].DataBoundItem as SelfListed;
                if (dataInfo == null) return;

                string url = Helper.GetURL(HTTPServiceUrlCollection.GetWareHouseInfoUrl, dataInfo.warehouseId);
                FormHouse fh = new FormHouse();
                fh.SetHouse(url);
                fh.Show();
            }
        }

        /// <summary>
        /// 摘牌显示交易对手
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutBrandCellMouseMove_Oppo(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            //交易对手列
            if (e.ColumnIndex == 10)
            {
                object o = this.dataGridView_OutBrand.Rows[e.RowIndex].DataBoundItem;

                Transaction dataInfo = this.dataGridView_OutBrand.Rows[e.RowIndex].DataBoundItem as Transaction;
                if (dataInfo == null) return;
                if (form != null) return;

                form = new ImformationTips();
                form.Initialize(dataInfo);
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(Cursor.Position.X - form.Width, Cursor.Position.Y);
                form.Show();
            }
        }

        private void OutBrandCellMouseLeave_Oppo(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            //交易对手列
            if (e.ColumnIndex == 10 && form != null)
            {
                form.Close();
                form = null;
            }
        }
    }
}
