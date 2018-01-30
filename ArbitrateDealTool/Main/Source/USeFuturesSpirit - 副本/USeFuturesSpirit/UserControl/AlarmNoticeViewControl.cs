using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public partial class AlarmNoticeViewControl : USeUserControl
    {
        private BindingList<AlarmLogViewModel> m_dataSource = null; 

        public AlarmNoticeViewControl()
        {
            InitializeComponent();
        }

        public override void Initialize()
        {
            m_dataSource = new BindingList<AlarmLogViewModel>();

            this.gridLog.AutoGenerateColumns = false;
            this.gridLog.DataSource = m_dataSource;

            USeManager.Instance.AutoTraderManager.OnAlarm += AutoTraderManager_OnAlarm;
            USeManager.Instance.OrderDriver.OnDriverStateChanged += OrderDriver_OnDriverStateChanged;
            USeManager.Instance.QuoteDriver.OnDriverStateChanged += QuoteDriver_OnDriverStateChanged;
        }

        private void QuoteDriver_OnDriverStateChanged(object sender, USe.TradeDriver.Common.USeQuoteDriverStateChangedEventArgs e)
        {
            if (e.OldState == USeQuoteDriverState.Ready && e.NewState != USeQuoteDriverState.Ready)
            {
                AlarmNotice alarm = new AlarmNotice(AlarmType.TradeDriverDisconect, "行情驱动断线");
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action<AlarmNotice>(AddAlarmNotice), alarm);
                    return;
                }
                else
                {
                    AddAlarmNotice(alarm);
                }
            }
        }

        private void OrderDriver_OnDriverStateChanged(object sender, USe.TradeDriver.Common.USeOrderDriverStateChangedEventArgs e)
        {
            if(e.OldState == USeOrderDriverState.Ready && e.NewState != USeOrderDriverState.Ready)
            {
                AlarmNotice alarm = new AlarmNotice(AlarmType.TradeDriverDisconect, "交易驱动断线");
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action<AlarmNotice>(AddAlarmNotice), alarm);
                    return;
                }
                else
                {
                    AddAlarmNotice(alarm);
                }
            }
        }

        private void AutoTraderManager_OnAlarm(AlarmNotice alarm)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<AlarmNotice>(AddAlarmNotice), alarm);
                return;
            }
            else
            {
                AddAlarmNotice(alarm);
            }

        }

        private void AddAlarmNotice(AlarmNotice alarm)
        { 
            AlarmLogViewModel log = new AlarmLogViewModel() {
                AlarmTime = alarm.AlarmTime,
                Message = alarm.Message,
                AlarmType = alarm.AlarmType
            };

            m_dataSource.Insert(0, log);
        }
    }
}
