using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using USe.TradeDriver.Common;
using USeFuturesSpirit.ViewModel;

namespace USeFuturesSpirit
{
    public partial class QuoteListControl : USeUserControl
    {
        public event SelectInstrumentChangeEventHandle OnSelectInstrumentChanged;
        public delegate void SelectInstrumentChangeEventHandle(USeInstrument market_data);

        private string m_productId = string.Empty; // 当前品种

        private BindingList<MarketDataViewModel> m_dataSource = new BindingList<MarketDataViewModel>();

        public QuoteListControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当前品种。
        /// </summary>
        public string ProductId
        {
            get { return m_productId; }
            set
            {
                if (value != m_productId)
                {
                    m_productId = value;
                    ChangegProduct(value);
                }
            }
        }

        public override void Initialize()
        {
            this.gridQuote.AutoGenerateColumns = false;
            this.gridQuote.DataSource = m_dataSource;
            USeManager.Instance.QuoteDriver.OnMarketDataChanged += QuoteDriver_OnMarketDataChanged;
        }

        private void QuoteDriver_OnMarketDataChanged(object sender, USeMarketDataChangedEventArgs e)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeMarketDataChangedEventArgs>(QuoteDriver_OnMarketDataChanged), sender, e);
                return;
            }

            MarketDataViewModel marketModel = m_dataSource.FirstOrDefault(p => p.Instrument == e.MarketData.Instrument);
            if(marketModel != null)
            {
                marketModel.Update(e.MarketData);
            }
        }

        /// <summary>
        /// 更换品种。
        /// </summary>
        /// <param name="product"></param>
        private void ChangegProduct(string product)
        {
            m_dataSource.Clear();
            if (string.IsNullOrEmpty(product)) return;

            try
            {
                
                List<USeInstrumentDetail> instrumentDetailList = USeManager.Instance.OrderDriver.QueryInstrumentDetail(product);
                if (instrumentDetailList == null) return;

                List<USeInstrument> instrumentList = (from e in instrumentDetailList
                                                      orderby e.Instrument.InstrumentCode
                                                      select e.Instrument).ToList();
                foreach (USeInstrument instrument in instrumentList)
                {
                    MarketDataViewModel marketModel = new MarketDataViewModel(instrument);
                    m_dataSource.Add(marketModel);
                }

                USeManager.Instance.QuoteDriver.Subscribe(instrumentList);
            }
            catch(Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        private void QuoteListControl_Load(object sender, EventArgs e)
        {
        }

        private void SetPriceCellForeColor(DataGridViewCellStyle cellStyle,decimal diff)
        {
            if (diff> 0)
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

        private void SetPriceCellForeColor(DataGridViewCellStyle cellStyle, decimal value,decimal refValue)
        {
            if(value <=0 || refValue<=0)
            {
                cellStyle.BackColor = Color.White;
            }
            else
            {
                SetPriceCellForeColor(cellStyle, (value - refValue));
            }
        }

        private void gridQuote_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            MarketDataViewModel dataModel = this.gridQuote.Rows[e.RowIndex].DataBoundItem as MarketDataViewModel;
            Debug.Assert(dataModel != null);
            if(e.ColumnIndex == this.Column_LastPrice.Index)
            {
                SetPriceCellForeColor(e.CellStyle, dataModel.LastPrice, dataModel.PreSettlementPrice);
            }
            else if(e.ColumnIndex == this.Column_NetChange.Index)
            {
                SetPriceCellForeColor(e.CellStyle, dataModel.NetChange);
            }
            else if (e.ColumnIndex == this.Column_PctChange.Index)
            {
                SetPriceCellForeColor(e.CellStyle, dataModel.PctChange);
            }
            else if(e.ColumnIndex == this.Column_BidPrice.Index)
            {
                SetPriceCellForeColor(e.CellStyle, dataModel.BidPrice, dataModel.PreSettlementPrice);
            }
            else if(e.ColumnIndex == this.Column_AskPrice.Index)
            {
                SetPriceCellForeColor(e.CellStyle, dataModel.AskPrice, dataModel.PreSettlementPrice);
            }
            else if (e.ColumnIndex == this.Column_HighPrice.Index)
            {
                SetPriceCellForeColor(e.CellStyle, dataModel.HighPrice, dataModel.PreSettlementPrice);
            }
            else if (e.ColumnIndex == this.Column_LowPrice.Index)
            {
                SetPriceCellForeColor(e.CellStyle, dataModel.LowPrice, dataModel.PreSettlementPrice);
            }
            else if (e.ColumnIndex == this.Column_OpenPrice.Index)
            {
                SetPriceCellForeColor(e.CellStyle, dataModel.OpenPrice, dataModel.PreSettlementPrice);
            }
        }

        private void CellContentClickEvent(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            MarketDataViewModel market_data_model = this.gridQuote.Rows[e.RowIndex].DataBoundItem as MarketDataViewModel;

            if (OnSelectInstrumentChanged != null)
            {
                try
                {
                    OnSelectInstrumentChanged(market_data_model.Instrument);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }
      
    }
}
