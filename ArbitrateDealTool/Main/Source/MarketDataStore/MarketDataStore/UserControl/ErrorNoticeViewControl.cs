using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.Common;
using System.Diagnostics;

namespace MarketDataStore
{
    public partial class ErrorNoticeViewControl : UserControl
    {
        private BindingList<ErrorNoticeViewModel> m_dataSource = null;
        private bool m_verboseFlag = false;

        public ErrorNoticeViewControl()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            m_dataSource = new BindingList<ErrorNoticeViewModel>();

            this.gridLog.AutoGenerateColumns = false;
            this.gridLog.DataSource = m_dataSource;
            USeManager.Instance.Notify += OnNotifyEventArrived;
        }

        private void OnNotifyEventArrived(object sender, USe.Common.USeNotifyEventArgs e)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeNotifyEventArgs>(OnNotifyEventArrived), sender, e);
                return;
            }

            if(m_verboseFlag == false && e.Level == USeNotifyLevel.Verbose)
            {
                return;
            }

            ErrorNoticeViewModel log = new ErrorNoticeViewModel();
            log.Level = e.Level;
            log.LogTime = e.Time;
            log.Message = e.Message;
            switch(e.Level)
            {
                case USeNotifyLevel.Critical:
                case USeNotifyLevel.Error:
                    log.LevelIcon = global::MarketDataStore.Properties.Resources.error;
                    break;
                case USeNotifyLevel.Warning:
                    log.LevelIcon = global::MarketDataStore.Properties.Resources.warning;
                    break;
                case USeNotifyLevel.Information:
                case USeNotifyLevel.Verbose:
                default:
                    log.LevelIcon = global::MarketDataStore.Properties.Resources.information;
                    break;
            }
            m_dataSource.Insert(0, log);
        }

        private void cbxv_CheckedChanged(object sender, EventArgs e)
        {
            m_verboseFlag = this.cbxVerbose.Checked;
        }

        private void tsmiClearNotice_Click(object sender, EventArgs e)
        {
            m_dataSource.Clear();
        }

        private void gridLog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            ErrorNoticeViewModel model = this.gridLog.Rows[e.RowIndex].DataBoundItem as ErrorNoticeViewModel;
            switch(model.Level)
            {
                case USeNotifyLevel.Critical:
                case USeNotifyLevel.Error:
                    e.CellStyle.ForeColor = Color.Red;
                    break;
                case USeNotifyLevel.Warning:
                    e.CellStyle.ForeColor = Color.Blue;
                    break;
                case USeNotifyLevel.Information:
                case USeNotifyLevel.Verbose:
                default:
                    e.CellStyle.ForeColor = Color.Black;
                    break;
            }
        }
    }
}


