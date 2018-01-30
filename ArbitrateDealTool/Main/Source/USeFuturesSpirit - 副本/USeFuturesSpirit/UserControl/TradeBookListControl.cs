using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USeFuturesSpirit.Properties;
using USe.TradeDriver.Common;
using USe.Common;
using USeFuturesSpirit.ViewModel;


namespace USeFuturesSpirit
{
    public partial class TradeBookListControl : USeUserControl
    {
        private BindingList<TradeBookViewＭodel> m_trade_data_source = new BindingList<TradeBookViewＭodel>();

        public TradeBookListControl()
        {
            InitializeComponent();
        }
        public override void Initialize()
        {
            this.gridTrade.AutoGenerateColumns = false;
            this.gridTrade.DataSource = m_trade_data_source;
            USeManager.Instance.OrderDriver.OnTradeBookChanged += OrderDriver_OnTradeBookChanged;

            List<USeTradeBook> traderBookList = USeManager.Instance.OrderDriver.QueryTradeBooks();
            if (traderBookList != null && traderBookList.Count > 0)
            {
                foreach (USeTradeBook tradeBook in traderBookList)
                {
                    UpdateTradeBook(tradeBook);
                }
            }
        }

        /// <summary>
        /// 新增成交记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderDriver_OnTradeBookChanged(object sender, USeTradeBookChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeTradeBookChangedEventArgs>(OrderDriver_OnTradeBookChanged), sender, e);
                return;
            }

            UpdateTradeBook(e.TradeBook);
        }

        private void UpdateTradeBook(USeTradeBook tradeBook)
        {
            TradeBookViewＭodel trade_data_model = TradeBookViewＭodel.Creat(tradeBook);
            m_trade_data_source.Insert(0,trade_data_model);
        }
    }
}
