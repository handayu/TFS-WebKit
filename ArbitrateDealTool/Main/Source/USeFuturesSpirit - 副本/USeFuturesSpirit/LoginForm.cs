using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using USe.TradeDriver.Common;
using USe.TradeDriver.Ctp;
using USe.TradeDriver.Test;

namespace USeFuturesSpirit
{
    public partial class LoginForm : Form
    {
        private BackgroundWorker m_loginWorker = null;
        private USeOrderDriver m_orderDriver = null;
        private USeQuoteDriver m_quoteDriver = null;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            LoadFrontServer();
            this.txtAccount.Focus();
            this.txtAccount.Select();

            UserDefineSetting userSetting = USeManager.Instance.SystemConfigManager.GetUserDefineSetting();
            if (userSetting.SaveInvestorId)
            {
                this.txtAccount.Text = userSetting.LastInvestorId;
                this.cbxSaveInvestorAccount.Checked = true;
            }
# if DEBUG
            this.txtAccount.Text = "090952";
            this.txtPassword.Text = "2wsx1qaz";
#endif
        }

        private void btnSetServerConfig_Click(object sender, EventArgs e)
        {
            FrontServerConfigForm form = new FrontServerConfigForm();
            if (DialogResult.Yes == form.ShowDialog(this))
            {
                LoadFrontServer();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.lblLoginState.Text = string.Empty;

            FrontSeverConfig serverConfig = this.cbxServerConfig.SelectedItem as FrontSeverConfig;
            if (serverConfig == null)
            {
                Debug.Assert(false);
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "请配置服务信息");
                return;
            }
            if (string.IsNullOrEmpty(serverConfig.BrokerID))
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "请配置服务信息");
                return;
            }
            if (string.IsNullOrEmpty(this.txtAccount.Text))
            {
                USeFuturesSpiritUtility.ShowInformationMessageBox(this, "请输入账号");
                this.txtAccount.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtPassword.Text))
            {
                USeFuturesSpiritUtility.ShowInformationMessageBox(this, "请输入密码");
                this.txtPassword.Focus();
                return;
            }

            if (m_loginWorker != null && m_loginWorker.IsBusy)
            {
                USeFuturesSpiritUtility.ShowInformationMessageBox(this, "正在登录中,请稍后");
                return;
            }

            try
            {
                LoginAccount loginAccount = new LoginAccount()
                {
                    QuoteAddress = serverConfig.QuoteFrontAddress,
                    QuotePort = serverConfig.QuoteFrontPort,
                    TradeAddress = serverConfig.TradeFrontAddress,
                    TradePort = serverConfig.TradeFrontPort,
                    BrokerId = serverConfig.BrokerID,
                    Account = this.txtAccount.Text,
                    Password = this.txtPassword.Text
                };

                m_quoteDriver = new CtpQuoteDriver(serverConfig.QuoteFrontAddress, serverConfig.QuoteFrontPort);
                m_orderDriver = new CtpOrderDriver(USeDriverType.Simulate, serverConfig.TradeFrontAddress, serverConfig.TradeFrontPort);
                //m_quoteDriver = new USeTestQuoteDriver();// serverconfig.quotefrontaddress, serverconfig.quotefrontport);
                //m_orderDriver = new USeTestOrderDriver();// usedriverType.Simulate, serverConfig.TradeFrontAddress, serverConfig.TradeFrontPort);

                m_orderDriver.OnDriverStateChanged += OrderDriver_OnDriverStateChanged;

                m_loginWorker = new BackgroundWorker();
                m_loginWorker.DoWork += LoginWorker_DoWork;
                m_loginWorker.RunWorkerCompleted += LoginWorker_RunWorkerCompleted;
                m_loginWorker.RunWorkerAsync(loginAccount);

                this.btnLogin.Enabled = false;
                this.timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, "登录失败");
            }
        }

        private void LoginWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Debug.Assert(m_quoteDriver != null);
            Debug.Assert(m_orderDriver != null);

            LoginAccount loginAccount = e.Argument as LoginAccount;
            Debug.Assert(loginAccount != null);

            m_quoteDriver.ConnectServer();
            m_quoteDriver.Login(loginAccount.BrokerId, loginAccount.Account, loginAccount.Password);

            m_orderDriver.ConnectServer();
            m_orderDriver.Login(loginAccount.BrokerId, loginAccount.Account, loginAccount.Password);
        }

        private void OrderDriver_OnDriverStateChanged(object sender, USeOrderDriverStateChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeOrderDriverStateChangedEventArgs>(OrderDriver_OnDriverStateChanged), sender, e);
                return;
            }

            this.lblLoginState.Text = e.Reason;
        }

        private void LoginWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.timer1.Enabled = false;
            this.proBarLogin.Value = 0;
            if (e.Error != null)
            {
                this.lblLoginState.Text = e.Error.Message;
                this.btnLogin.Enabled = true;
                return;
            }

            Debug.Assert(m_quoteDriver != null);
            Debug.Assert(m_orderDriver != null);
            if (m_quoteDriver.DriverState != USeQuoteDriverState.Ready)
            {
                MessageBox.Show("行情未登录");
                DisposeTradeDriver();
                this.btnLogin.Enabled = true;
                return;
            }
            if (m_orderDriver.DriverState != USeOrderDriverState.Ready)
            {
                MessageBox.Show("交易未登录");
                DisposeTradeDriver();
                this.btnLogin.Enabled = true;
                return;
            }

            UserDefineSetting userSetting = USeManager.Instance.SystemConfigManager.GetUserDefineSetting();
            if (this.cbxSaveInvestorAccount.Checked)
            {
                userSetting.LastInvestorId = m_orderDriver.Account;
            }
            else
            {
                userSetting.LastInvestorId = string.Empty;
            }
            userSetting.SaveInvestorId = this.cbxSaveInvestorAccount.Checked;

            USeManager.Instance.SystemConfigManager.SaveUserDefineSetting(userSetting);

            if (m_orderDriver.NeedSettlementConfirm)
            {
                SettlementInfoConfirmForm confirmForm = new SettlementInfoConfirmForm(m_orderDriver);
                if (DialogResult.Yes != confirmForm.ShowDialog())
                {
                    this.DialogResult = DialogResult.No;
                    this.Close();
                    return;
                }
            }

            USeManager.Instance.PreStart(m_orderDriver, m_quoteDriver);



            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void DisposeTradeDriver()
        {
            try
            {
                if (m_quoteDriver != null)
                {
                    m_quoteDriver.DisConnectServer();
                    m_quoteDriver = null;
                }

                if (m_orderDriver != null)
                {
                    m_orderDriver.DisConnectServer();
                    m_orderDriver = null;
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        /// <summary>
        /// 加载前置服务信息。
        /// </summary>
        private void LoadFrontServer()
        {
            try
            {
                this.cbxServerConfig.Items.Clear();

                GlobalFontServerConfig globalConfig = USeManager.Instance.DataAccessor.GetGlobalFontServerConfig();
                if (globalConfig == null || globalConfig.ServerList == null || globalConfig.ServerList.Count <= 0)
                {
                    FrontSeverConfig emptyItem = new FrontSeverConfig();
                    emptyItem.BrokerName = "请配置服务信息";

                    this.cbxServerConfig.Items.Add(emptyItem);
                }
                else
                {
                    for (int i = 0; i < globalConfig.ServerList.Count; i++)
                    {
                        FrontSeverConfig item = globalConfig.ServerList[i];
                        this.cbxServerConfig.Items.Add(item);
                    }
                }

                this.cbxServerConfig.SelectedIndex = 0;

                if (globalConfig != null && string.IsNullOrEmpty(globalConfig.DefaultBrokerId) == false)
                {
                    for (int i = 0; i < globalConfig.ServerList.Count; i++)
                    {
                        if (globalConfig.ServerList[i].BrokerID == globalConfig.DefaultBrokerId)
                        {
                            this.cbxServerConfig.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                MessageBox.Show("加载服务信息失败," + ex.Message);
            }
        }


        private class LoginAccount
        {
            public string QuoteAddress { get; set; }

            public int QuotePort { get; set; }

            public string TradeAddress { get; set; }

            public int TradePort { get; set; }

            public string BrokerId { get; set; }

            public string Account { get; set; }

            public string Password { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                //YMTestUnit.TestOrderNum();
                //TempUtility.Test();
                //TempUtility.TestOrder();
                QualityAlalisyForm form = new QualityAlalisyForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int m_progressValue = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            m_progressValue += 10;
            if (m_progressValue > 100)
            {
                m_progressValue = 10;
            }
            //this.proBarLogin.Maximum = 100;
            this.proBarLogin.Value = m_progressValue;
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Enabled = false;
        }
    }
}
