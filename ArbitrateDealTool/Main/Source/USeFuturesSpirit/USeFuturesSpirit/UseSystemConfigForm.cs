using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

namespace USeFuturesSpirit
{
    public partial class UseSystemConfigForm : Form
    {
        public UseSystemConfigForm()
        {
            InitializeComponent();
        }

        private void UseSystemConfigForm_Load(object sender, EventArgs e)
        {
            try
            {
                USeSystemSetting setting = USeManager.Instance.SystemConfigManager.GetSystemSetting();
                Debug.Assert(setting != null);

                switch(setting.MarketPriceMethods)
                {
                    case USeMarketPriceMethod.OpponentPrice:this.rbnMarketPriceMethod_OpponentPrice.Checked = true;break;
                    case USeMarketPriceMethod.OnePriceTick:this.rbnMarketPriceMethod_OnePriceTick.Checked = true;break;
                    case USeMarketPriceMethod.TwoPriceTick:this.rbnMarketPriceMethod_TwoPriceTick.Checked = true;break;
                    case USeMarketPriceMethod.ThreePriceTick:this.rbnMarketPriceMethod_ThreePriceTick.Checked = true;break;
                }

                this.txtTaskOrder_TaskMaxTryCount.Text = setting.TaskOrder.TaskMaxTryCount.ToString();
                this.txtTaskOrder_TryOrderMinInterval.Text = ((int)setting.TaskOrder.TryOrderMinInterval.TotalMilliseconds).ToString();

                this.txtOrderMargin_MaxUseRate.Text = (setting.OrderMargin.MaxUseRate * 100).ToString();
            }
            catch(Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "加载系统配置信息失败," + ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            USeMarketPriceMethod marketPriceMethod = USeMarketPriceMethod.Unknown;
            if (this.rbnMarketPriceMethod_OpponentPrice.Checked)
            {
                marketPriceMethod = USeMarketPriceMethod.OpponentPrice;
            }
            else if (this.rbnMarketPriceMethod_OnePriceTick.Checked)
            {
                marketPriceMethod = USeMarketPriceMethod.OnePriceTick;
            }
            else if (this.rbnMarketPriceMethod_TwoPriceTick.Checked)
            {
                marketPriceMethod = USeMarketPriceMethod.TwoPriceTick;
            }
            else if (this.rbnMarketPriceMethod_ThreePriceTick.Checked)
            {
                marketPriceMethod = USeMarketPriceMethod.ThreePriceTick;
            }
            else
            {
                USeFuturesSpiritUtility.ShowInformationMessageBox(this, "请选择市价定义");
                return;
            }

            int taskMaxTryCount = 0;
            if (int.TryParse(this.txtTaskOrder_TaskMaxTryCount.Text, out taskMaxTryCount) == false)
            {
                USeFuturesSpiritUtility.ShowInformationMessageBox(this, "请输入正确的下单设定-最大尝试次数");
                this.txtTaskOrder_TaskMaxTryCount.Focus();
                return;
            }
            if (taskMaxTryCount <= 0 || taskMaxTryCount > 20)
            {
                USeFuturesSpiritUtility.ShowInformationMessageBox(this, "请输入正确的下单设定-最大尝试次数,合法范围[1~20]");
                this.txtTaskOrder_TaskMaxTryCount.Focus();
                return;
            }

            int tryOrderMinInterval = 0;
            if (int.TryParse(this.txtTaskOrder_TryOrderMinInterval.Text, out tryOrderMinInterval) == false)
            {
                USeFuturesSpiritUtility.ShowInformationMessageBox(this, "请输入正确的下单设定-两次下单最小间隔");
                this.txtTaskOrder_TaskMaxTryCount.Focus();
                return;
            }
            if (tryOrderMinInterval < 0)
            {
                USeFuturesSpiritUtility.ShowInformationMessageBox(this, "请输入正确的下单设定-两次下单最小间隔");
                this.txtTaskOrder_TaskMaxTryCount.Focus();
                return;
            }

            decimal maxMarginUseRate = 0;
            if (decimal.TryParse(this.txtOrderMargin_MaxUseRate.Text, out maxMarginUseRate) == false)
            {
                USeFuturesSpiritUtility.ShowInformationMessageBox(this, "请输入正确的套利单保证金占用最大比例,合法范围(0~100]");
                return;
            }
            if (maxMarginUseRate <= 0 || maxMarginUseRate > 100)
            {
                USeFuturesSpiritUtility.ShowInformationMessageBox(this, "套利单保证金占用最大比例");
                return;
            }
            maxMarginUseRate = maxMarginUseRate / 100m;

            try
            {
                USeSystemSetting setting = new USeSystemSetting();
                setting.MarketPriceMethods = marketPriceMethod;

                setting.TaskOrder = new TaskOrderSetting() {
                    TaskMaxTryCount = taskMaxTryCount,
                    TryOrderMinInterval = new TimeSpan(0, 0, 0, 0, tryOrderMinInterval)
                };

                setting.OrderMargin = new OrderMarginSetting() {
                    MaxUseRate = maxMarginUseRate
                };

                USeManager.Instance.SystemConfigManager.SaveSystemSetting(setting);
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, ex.Message);
                return;
            }

            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
