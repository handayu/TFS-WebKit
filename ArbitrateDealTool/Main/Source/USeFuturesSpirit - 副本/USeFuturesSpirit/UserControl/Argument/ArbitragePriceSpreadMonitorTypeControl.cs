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
    public partial class ArbitragePriceSpreadMonitorTypeControl : UserControl
    {
        private ArbitragePriceSpreadAlarmType m_monitorType = ArbitragePriceSpreadAlarmType.Close;

        private Color m_selectedColor = Color.FromArgb(255, 128, 0);
        private Color m_unselectedColor = SystemColors.Control;

        public ArbitragePriceSpreadMonitorTypeControl()
        {
            InitializeComponent();
            SetButtonStyle();
        }

        public ArbitragePriceSpreadAlarmType MonitorType
        {
            get { return m_monitorType; }
            set
            {
                m_monitorType = value;
                SetButtonStyle();
            }
        }

        private void SetButtonStyle()
        {
            this.btnOpen.BackColor = GetButtonBackColor(m_monitorType == ArbitragePriceSpreadAlarmType.Open);
            this.btnClose.BackColor = GetButtonBackColor(m_monitorType == ArbitragePriceSpreadAlarmType.Close);
        }

        private Color GetButtonBackColor(bool isSelected)
        {
            return isSelected ? m_selectedColor : m_unselectedColor;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            this.MonitorType = ArbitragePriceSpreadAlarmType.Open;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.MonitorType = ArbitragePriceSpreadAlarmType.Close;
        }
    }
}
