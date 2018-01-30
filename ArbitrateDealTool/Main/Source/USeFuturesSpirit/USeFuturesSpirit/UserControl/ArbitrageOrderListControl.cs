using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace USeFuturesSpirit
{
    public partial class ArbitrageOrderListControl : USeUserControl
    {
        #region member
        private List<ArbitrageOrderControl> m_orderControlList = new List<ArbitrageOrderControl>();
        #endregion

        #region construction
        public ArbitrageOrderListControl()
        {
            InitializeComponent();
        }
        #endregion

        #region public methods
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize()
        {
            this.panelOrderContainer.Controls.Clear();

            AutoTraderManager traderManager = USeManager.Instance.AutoTraderManager;

            traderManager.OnAddAutoTrader += TraderManager_OnAddAutoTrader;
            traderManager.OnRemoveAutoTrader += TraderManager_OnRemoveAutoTrader;
            List<AutoTrader> autoTraderList = traderManager.GetAllAutoTrader();
            if (autoTraderList != null && autoTraderList.Count > 0)
            {
                foreach (AutoTrader autoTrader in autoTraderList)
                {
                    AddArbitrageOrderControl(autoTrader);
                }
            }
        }

        public void Stop()
        {
            if (m_orderControlList != null && m_orderControlList.Count > 0)
            {
                foreach (ArbitrageOrderControl control in m_orderControlList)
                {
                    control.Stop();
                }
            }
        }


        private void TraderManager_OnAddAutoTrader(Guid traderIdentify)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AddAutoTraderEventHandel(TraderManager_OnAddAutoTrader), traderIdentify);
                return;
            }

            AutoTrader autoTrader = USeManager.Instance.AutoTraderManager.GetAutoTrader(traderIdentify);
            AddArbitrageOrderControl(autoTrader);
        }

        private void TraderManager_OnRemoveAutoTrader(Guid traderIdentify)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new RemoveAutoTraderEventHandel(TraderManager_OnRemoveAutoTrader), traderIdentify);
                return;
            }

            RemoveArbitrageOrderControl(traderIdentify);
        }
        #endregion

        #region private methods
        /// <summary>
        /// 新增套利单控件。
        /// </summary>
        /// <param name="autoTrader"></param>
        private void AddArbitrageOrderControl(AutoTrader autoTrader)
        {
            if (ExistArbitrageOrderControl(autoTrader.TraderIdentify) == false)
            {
                ArbitrageOrderControl orderControl = CreateArbitrageOrderControl(autoTrader);

                this.panelOrderContainer.Controls.Add(orderControl);
                m_orderControlList.Add(orderControl);
                orderControl.Initialize();
            }
            else
            {
                Debug.Assert(false);
            }
        }

        /// <summary>
        /// 移除套利单控件。
        /// </summary>
        /// <param name="traderIdentity"></param>
        private void RemoveArbitrageOrderControl(Guid traderIdentity)
        {
            ArbitrageOrderControl arbitrageOrderControl = null;
            foreach (ArbitrageOrderControl orderControl in m_orderControlList)
            {
                if (orderControl.TraderIdentify == traderIdentity)
                {
                    arbitrageOrderControl = orderControl;
                    break;
                }
            }

            Debug.Assert(arbitrageOrderControl != null);
            if (arbitrageOrderControl != null)
            {
                arbitrageOrderControl.Stop();
                this.panelOrderContainer.Controls.Remove(arbitrageOrderControl);
                m_orderControlList.Remove(arbitrageOrderControl);

                arbitrageOrderControl.Dispose();
            }
        }

        /// <summary>
        /// 套利单空间是否存在。
        /// </summary>
        /// <param name="traderIdentity"></param>
        /// <returns></returns>
        private bool ExistArbitrageOrderControl(Guid traderIdentity)
        {
            foreach (ArbitrageOrderControl orderControl in m_orderControlList)
            {
                if (orderControl.TraderIdentify == traderIdentity)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 创建套利单控件。
        /// </summary>
        /// <param name="arbitrageOrder"></param>
        /// <returns></returns>
        private ArbitrageOrderControl CreateArbitrageOrderControl(AutoTrader autoTrader)
        {
            ArbitrageOrderControl orderItemControl = new ArbitrageOrderControl(autoTrader);
            orderItemControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            orderItemControl.Dock = System.Windows.Forms.DockStyle.Top;
            orderItemControl.Location = new System.Drawing.Point(0, 0);
            orderItemControl.Name = "arbitrageItemControl1";
            orderItemControl.Size = new System.Drawing.Size(836, 153);
            orderItemControl.TabIndex = 0;

            orderItemControl.Initialize();

            return orderItemControl;
        }
        #endregion
    }
}
