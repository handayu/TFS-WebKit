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
    public partial class ArbitrageRunStateControl : USeUserControl
    {
        public ArbitrageRunStateControl()
        {
            InitializeComponent();
        }

        public override void Initialize()
        {
            USeManager.Instance.AutoTraderManager.OnAddAutoTrader += AutoTraderManager_OnAddAutoTrader;
            USeManager.Instance.AutoTraderManager.OnRemoveAutoTrader += AutoTraderManager_OnRemoveAutoTrader;
            USeManager.Instance.AutoTraderManager.OnAutoTraderStateChanged += AutoTraderManager_OnAutoTraderStateChanged;
            UpdateStacInfo();
        }

        private void AutoTraderManager_OnAutoTraderStateChanged(AutoTraderWorkType workType, AutoTraderState state)
        {
            UpdateStacInfo();
        }

        private void AutoTraderManager_OnRemoveAutoTrader(Guid traderIdentify)
        {
            UpdateStacInfo();
        }

        private void AutoTraderManager_OnAddAutoTrader(Guid traderIdentify)
        {
            UpdateStacInfo();
        }

        public delegate void UpdateEventHandel();
        private void UpdateStacInfo()
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateEventHandel(UpdateStacInfo));
                return;
            }
            this.lblRunResult.Text = string.Format("{0}/{1}",
                USeManager.Instance.AutoTraderManager.RunTraderCount,
                USeManager.Instance.AutoTraderManager.TraderCount);
        }
    }
}
