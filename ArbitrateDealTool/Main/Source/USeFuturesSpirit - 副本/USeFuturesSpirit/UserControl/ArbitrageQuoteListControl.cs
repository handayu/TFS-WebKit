using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;
using USeFuturesSpirit.ViewModel;
using System.Diagnostics;

namespace USeFuturesSpirit
{
    public partial class ArbitrageQuoteListControl : USeUserControl
    {

        /// <summary>
        /// 套利组合约改变事件。
        /// </summary>
        public event SelectCombineInstrumentChangeEventHandle OnSelectCombineInstrumentChanged;
        public delegate void SelectCombineInstrumentChangeEventHandle(ArbitrageCombineInstrument combineInstrument);

        /// <summary>
        /// 当前品种
        /// </summary>
        private USeProduct m_product = null;

        private List<ArbitrageCombineInstrument> m_CombineInstrumentList = null;
        private Dictionary<USeInstrument, USeMarketData> m_marketDataDic = null; //每个合约维护一个Dic。

        public ArbitrageQuoteChoiceForm m_form = null;

        /// <summary>
        /// 数据绑定
        /// </summary>
        private BindingList<ArbitrageCombineInstrumentViewModel> m_dataSource = new BindingList<ArbitrageCombineInstrumentViewModel>();

        public ArbitrageQuoteListControl()
        {
            InitializeComponent();
        }

        public override void Initialize()
        {
            this.dataGridView_ArbiInstrument.AutoGenerateColumns = false;
            this.dataGridView_ArbiInstrument.DataSource = m_dataSource;

            //dataGridView_ArbiInstrument.Sort(dataGridView_ArbiInstrument.Columns[0], ListSortDirection.Ascending);

            try
            {
                m_CombineInstrumentList = USeManager.Instance.DataAccessor.GetCombineInstruments(USeManager.Instance.LoginUser.BrokerId,
                                                                                                 USeManager.Instance.LoginUser.Account);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            m_marketDataDic = new Dictionary<USeInstrument, USeMarketData>();

            //行情订阅
            USeManager.Instance.QuoteDriver.OnMarketDataChanged += QuoteDriver_OnMarketDataChanged;

        }

        /// <summary>
        /// 当前品种。
        /// </summary>
        public USeProduct Product
        {
            get { return m_product; }
            set
            {
                if (value != m_product)
                {
                    m_product = value;
                    ChangegProduct(value);
                }
            }
        }

        /// <summary>
        /// 更换品种,订阅品种所有合约行情。
        /// </summary>
        /// <param name="product"></param>
        private void ChangegProduct(USeProduct product)
        {
            m_dataSource.Clear();
            if (product == null) return;

            List<USeInstrument> subList = new List<USeInstrument>();
            try
            {
                foreach (ArbitrageCombineInstrument combineIns in m_CombineInstrumentList)
                {
                    if (combineIns.ProductID != product.ProductCode) continue;

                    subList.Add(combineIns.FirstInstrument);
                    subList.Add(combineIns.SecondInstrument);

                    ArbitrageCombineInstrumentData combineMarketData = new ArbitrageCombineInstrumentData();
                    combineMarketData.ArbitrageCombineInstrument = combineIns;
                    combineMarketData.FarDistanceBuyPrice = 0m;
                    combineMarketData.FarDistanceBuyVolumn = 0;
                    combineMarketData.FarDistanceSellPrice = 0m;
                    combineMarketData.FarDistanceSellVolumn = 0;

                    combineMarketData.NearDistanceBuyPrice = 0m;
                    combineMarketData.NearDistanceBuyVolumn = 0;
                    combineMarketData.NearDistanceSellPrice = 0m;
                    combineMarketData.NearDistanceSellVolumn = 0;

                    combineMarketData.NearLastPrice = 0m;
                    combineMarketData.FarLastPrice = 0m;

                    ArbitrageCombineInstrumentViewModel combineMarketDataModel = ArbitrageCombineInstrumentViewModel.CreatArbitrageCombineInstrumentViewModel(combineMarketData);
                    m_dataSource.Insert(0, combineMarketDataModel);
                }

                //更改当前品种
                m_product = product;

                USeManager.Instance.QuoteDriver.Subscribe(subList);
                //ToDO:快速查询，行情列表需要排序
                foreach (USeInstrument ins in subList)
                {
                    USeManager.Instance.QuoteDriver.QuickQuery(ins);
                }

                foreach (DataGridViewColumn c in this.dataGridView_ArbiInstrument.Columns)
                {
                    c.SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }


        public void OnArbitrageCombineInstrumentList_Changed(List<ArbitrageCombineInstrument> m_arbitrageInstrumentList)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<List<ArbitrageCombineInstrument>>(OnArbitrageCombineInstrumentList_Changed));
                return;
            }

            Debug.Assert(m_arbitrageInstrumentList != null);
            m_CombineInstrumentList = m_arbitrageInstrumentList;
        }

        //public void OnArbitrageCombineInstrumentAdd_Changed(ArbitrageCombineInstrument arbitrageInstrument)
        //{
        //    Debug.Assert(arbitrageInstrument != null);

        //    //ToDo::
        //    if (arbitrageInstrument.ProductID != m_product.ProductCode) return;

        //    ArbitrageCombineInstrumentData combineMarketData = new ArbitrageCombineInstrumentData();
        //    combineMarketData.ArbitrageCombineInstrument = arbitrageInstrument;
        //    combineMarketData.FarDistanceBuyPrice = 0m;
        //    combineMarketData.FarDistanceBuyVolumn = 0;
        //    combineMarketData.FarDistanceSellPrice = 0m;
        //    combineMarketData.FarDistanceSellVolumn = 0;

        //    combineMarketData.NearDistanceBuyPrice = 0m;
        //    combineMarketData.NearDistanceBuyVolumn = 0;
        //    combineMarketData.NearDistanceSellPrice = 0m;
        //    combineMarketData.NearDistanceSellVolumn = 0;

        //    combineMarketData.NearLastPrice = 0m;
        //    combineMarketData.FarLastPrice = 0m;

        //    ArbitrageCombineInstrumentViewModel combineMarketDataModel = ArbitrageCombineInstrumentViewModel.CreatArbitrageCombineInstrumentViewModel(combineMarketData);

        //    m_CombineInstrumentList.Add(combineMarketDataModel.ArbitrageCombineInstrument);
        //    m_dataSource.Insert(0, combineMarketDataModel);

        //    USeManager.Instance.QuoteDriver.Subscribe(arbitrageInstrument.FirstInstrument);
        //    USeManager.Instance.QuoteDriver.Subscribe(arbitrageInstrument.SecondInstrument);
        //}


        //public void OnArbitrageCombineInstrumentRemove_Changed(ArbitrageCombineInstrument arbitrageInstrument)
        //{
        //    Debug.Assert(arbitrageInstrument != null);

        //    //Todo::
        //    if (arbitrageInstrument.ProductID != m_product.ProductCode) return;
        //    ArbitrageCombineInstrumentViewModel combinIns = (from ins in m_dataSource
        //                                                     where ins.ArbitrageCombineInstrument == arbitrageInstrument
        //                                                     select ins).FirstOrDefault();
        //    if (combinIns != null)
        //    {
        //        m_dataSource.Remove(combinIns);
        //        m_CombineInstrumentList.Remove(combinIns.ArbitrageCombineInstrument);
        //    }
        //}


        /// <summary>
        /// 订阅的市场价格更改。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuoteDriver_OnMarketDataChanged(object sender, USeMarketDataChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeMarketDataChangedEventArgs>(QuoteDriver_OnMarketDataChanged), sender, e);
                return;
            }

            if (USeTraderProtocol.GetVarieties(e.MarketData.Instrument.InstrumentCode) != m_product.ProductCode) return;


            m_marketDataDic[e.MarketData.Instrument] = e.MarketData;

            foreach (ArbitrageCombineInstrument combineIns in m_CombineInstrumentList)
            {
                USeInstrument firIns = combineIns.FirstInstrument;
                USeInstrument secIns = combineIns.SecondInstrument;

                if (m_marketDataDic.ContainsKey(firIns) && m_marketDataDic.ContainsKey(secIns))
                {
                    ArbitrageCombineInstrumentData combineMarketData = new ArbitrageCombineInstrumentData();
                    combineMarketData.ArbitrageCombineInstrument = combineIns;
                    combineMarketData.FarDistanceBuyPrice = m_marketDataDic[secIns].BidPrice;
                    combineMarketData.FarDistanceBuyVolumn = m_marketDataDic[secIns].BidSize;
                    combineMarketData.FarDistanceSellPrice = m_marketDataDic[secIns].AskPrice;
                    combineMarketData.FarDistanceSellVolumn = m_marketDataDic[secIns].AskSize;

                    combineMarketData.NearDistanceBuyPrice = m_marketDataDic[firIns].BidPrice;
                    combineMarketData.NearDistanceBuyVolumn = m_marketDataDic[firIns].BidSize;
                    combineMarketData.NearDistanceSellPrice = m_marketDataDic[firIns].AskPrice;
                    combineMarketData.NearDistanceSellVolumn = m_marketDataDic[firIns].AskSize;

                    combineMarketData.NearLastPrice = m_marketDataDic[firIns].LastPrice;
                    combineMarketData.FarLastPrice = m_marketDataDic[secIns].LastPrice;

                    if (m_dataSource.Count == 0) continue;
                    Debug.WriteLine(DateTime.Now.ToString() + "--" + m_dataSource.Count() + combineIns.ArbitrageInstrumentOneCode + combineIns.ArbitrageInstrumentTwoCode);

                    ArbitrageCombineInstrumentViewModel combineMarketModel = (from p in m_dataSource
                                                                              where p.ArbitrageCombineInstrument == combineIns
                                                                              select p).FirstOrDefault();


                    if (combineMarketModel != null)
                    {
                        combineMarketModel.Update(combineMarketData);
                    }
                    else
                    {
                        m_dataSource.Add(combineMarketModel);
                    }

                }
                else
                {
                    continue;
                }
            }
        }

        private void SetPriceCellForeColor(DataGridViewCellStyle cellStyle, decimal diff)
        {
            if (diff > 0)
            {
                cellStyle.ForeColor = Color.Red;
            }
            else if (diff < 0)
            {
                cellStyle.ForeColor = Color.Green;
            }
            else
            {
                cellStyle.ForeColor = Color.Black;
            }
        }

        private void gridQuote_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            ArbitrageCombineInstrumentViewModel dataModel = this.dataGridView_ArbiInstrument.Rows[e.RowIndex].DataBoundItem as ArbitrageCombineInstrumentViewModel;
            Debug.Assert(dataModel != null);
            if (e.ColumnIndex == this.Column_Spread.Index)
            {
                SetPriceCellForeColor(e.CellStyle, dataModel.Spread);
            }
        }

       

        /// <summary>
        /// 右键组合套利合约设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArbitrageToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            m_form = new ArbitrageQuoteChoiceForm();
            m_form.ProductId = m_product;
            m_form.OnArbitrageCombineInstrumentListChanged += OnArbitrageCombineInstrumentList_Changed;
            //m_form.OnArbitrageCombineInstrumentAddChanged += OnArbitrageCombineInstrumentAdd_Changed;
            //m_form.OnArbitrageCombineInstrumentRemoveChanged += OnArbitrageCombineInstrumentRemove_Changed;

            m_form.ShowDialog();

        }

        /// <summary>
        /// 套利合约排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if(e.Column == this.Column_ArbitrageIns)
            {
                string cell_arbi = e.CellValue1 as string;
            }
        }

        private void dataGridView_ArbiInstrument_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            ArbitrageCombineInstrumentViewModel model = this.dataGridView_ArbiInstrument.Rows[e.RowIndex].DataBoundItem as ArbitrageCombineInstrumentViewModel;
            Debug.Assert(model != null);

            FireCombineInstrumentSelected(model);
        }


        private void FireCombineInstrumentSelected(ArbitrageCombineInstrumentViewModel model)
        {
            SelectCombineInstrumentChangeEventHandle handle = this.OnSelectCombineInstrumentChanged;
            if (handle != null)
            {
                try
                {
                    handle(model.ArbitrageCombineInstrument);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }
    }
}
