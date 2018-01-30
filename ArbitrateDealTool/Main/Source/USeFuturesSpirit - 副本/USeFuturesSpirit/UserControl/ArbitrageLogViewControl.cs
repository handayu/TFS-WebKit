using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace USeFuturesSpirit
{
    public partial class ArbitrageLogViewControl : USeUserControl
    {
        private BindingList<ArbitrageLogViewModel> m_dataSource = null; 

        public ArbitrageLogViewControl()
        {
            InitializeComponent();
        }

        public override void Initialize()
        {
            m_dataSource = new BindingList<ArbitrageLogViewModel>();

            this.gridLog.AutoGenerateColumns = false;
            this.gridLog.DataSource = m_dataSource;

            USeManager.Instance.AutoTraderManager.OnAutoTraderNotify += AutoTraderManager_OnNotify;
        }

        private void AutoTraderManager_OnNotify(AutoTraderNotice notice)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new AutoTraderNotifyEventHandle(AutoTraderManager_OnNotify), notice);
                return;
            }

            ArbitrageLogViewModel log = new ArbitrageLogViewModel() {
                TraderIdentify = notice.TradeIdentity,
                Alias = notice.Alias,
                LogTime = notice.NoticeTime,
                Message = notice.Message,
                NoticeType = notice.NoticeType
            };

            m_dataSource.Insert(0, log);
        }
      
    }
}
