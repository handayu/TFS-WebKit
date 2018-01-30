using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using USeFuturesSpirit.Arbitrage;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public partial class ArbitrageOrderControl
    {
        #region 开仓菜单
        private void tsmiOpen_ArbitrageDetailView_Click(object sender, EventArgs e)
        {
            ArbitrageOrderViewForm form = new ArbitrageOrderViewForm(m_arbitrageOrder);
            form.Show(this);
        }

        private void tsmiOpen_OpenAlignment_Click(object sender, EventArgs e)
        {
            try
            {
                m_autoTrader.OpenAlignment();
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, ex.Message);
            }
        }

        private void tsmiOpen_StopOpenAlignment_Click(object sender, EventArgs e)
        {
            try
            {
                m_autoTrader.StopOpenAlignment();
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, ex.Message);
            }
        }

        private void tsmiOpen_OpenChaseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                m_autoTrader.OpenChaseOrder();
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, ex.Message);
            }
        }

        private void tsmiOpen_StopOpenChaseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                m_autoTrader.StopOpenChaseOrder();
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, ex.Message);
            }
        }

        private void tsmiOpen_ForceOpenFinish_Click(object sender, EventArgs e)
        {
            try
            {
                m_autoTrader.ForceOpenFinish();
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, ex.Message);
            }
        }

        private void tsmiOpen_CancelOrder_Click(object sender, EventArgs e)
        {
            try
            {
                m_autoTrader.CancelOpenHangingOrder();
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, ex.Message);
            }
        }
        #endregion

        #region 平仓菜单
        private void tsmiClose_ArbitrageOrderView_Click(object sender, EventArgs e)
        {
            ArbitrageOrderViewForm form = new ArbitrageOrderViewForm(m_arbitrageOrder);
            form.Show(this);
        }

        private void tsmiClose_ChaseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                m_autoTrader.CloseChaseOrder();
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, ex.Message);
            }
        }

        private void tsmiClose_StopChaseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                m_autoTrader.StopCloseChaseOrder();
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, ex.Message);
            }
        }

        private void tsmiClose_ForceCloseFinish_Click(object sender, EventArgs e)
        {
            try
            {
                m_autoTrader.ForceCloseFinish();
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, ex.Message);
            }
        }

        private void tsmiClose_CancelAllOrder_Click(object sender, EventArgs e)
        {
            try
            {
                m_autoTrader.CancelCloseHangingOrder();
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, ex.Message);
            }
        }

        private void tsmClose_TransferToHistory_Click(object sender, EventArgs e)
        {
            try
            {
                m_autoTrader.TransferToHistoryArbitrage();
                System.Threading.Thread.Sleep(200);  // [yangming]加个延时，防止还未保存文件
                USeManager.Instance.AutoTraderManager.RemoveAutoTrader(m_autoTrader.TraderIdentify);
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, ex.Message);
            }
        }
        #endregion

        #region 按钮事件

        private void btnStartOrStop_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnStartOrStop.Enabled = false;

                if (m_autoTrader.State == AutoTraderState.Disable)
                {
                    Debug.Assert(this.btnStartOrStop.Text == "启动");
                    m_autoTrader.StartOpenOrCloseMonitor();
                    //启动
                }
                else if (m_autoTrader.State == AutoTraderState.Enable)
                {
                    Debug.Assert(this.btnStartOrStop.Text == "停止");
                    m_autoTrader.StopOpenOrCloseMonitor();
                }
            }
            catch(Exception ex)
            {
                this.btnStartOrStop.Enabled = true;
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, ex.Message);
            }
        }

        private void btnArgumentSet_Click(object sender, EventArgs e)
        {
            SetArbitrageArgument();
        }
        #endregion

        private void SetArbitrageArgument()
        {
            ArbitrageArgument argument = m_autoTrader.GetArgument();
            Debug.Assert(argument != null);

            USeProduct product = USeManager.Instance.OrderDriver.QueryProduct(argument.ProductID);
            Debug.Assert(product != null);

            ArbitrageOrderCreateForm form = new ArbitrageOrderCreateForm(argument, product);
            form.ShowDialog();
        }

    }
}
