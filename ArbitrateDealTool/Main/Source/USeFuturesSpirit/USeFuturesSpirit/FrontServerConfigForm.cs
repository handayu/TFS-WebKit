using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;


namespace USeFuturesSpirit
{
    public partial class FrontServerConfigForm : Form
    {
        private BindingList<FrontSeverConfigViewModel> m_dataSource = null;

        public FrontServerConfigForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            List<FrontSeverConfig> serverList = new List<FrontSeverConfig>();
            GlobalFontServerConfig globalConfig = new GlobalFontServerConfig();

            try
            {
                foreach (FrontSeverConfigViewModel data in m_dataSource)
                {
                    if (VerifyDataCheck(data) == false) return;

                    FrontSeverConfig itemConfig = new FrontSeverConfig();
                    itemConfig.BrokerID = data.BrokerID;                                                   //经纪商代码
                    itemConfig.BrokerName = data.BrokerName;                                        //经纪商
                    itemConfig.QuoteFrontAddress = data.QuoteFrontAddress;                   //行情地址
                    itemConfig.QuoteFrontPort = data.QuoteFrontPort;                              //行情端口
                    itemConfig.TradeFrontAddress = data.TradeFrontAddress;                     //交易地址
                    itemConfig.TradeFrontPort = data.TradeFrontPort;                                //交易端口
                    itemConfig.AuthCode = data.AuthCode;
                    itemConfig.UserProductInfo = data.UserProductInfo;
                    serverList.Add(itemConfig);
                }

                globalConfig.ServerList = serverList;

                if (serverList.Count > 0)
                {
                    globalConfig.DefaultBrokerId = serverList[serverList.Count - 1].BrokerID;
                }
            }
            catch(Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, "保存服务信息失败," + ex.Message);
                return;
            }
            
            try
            {
                USeManager.Instance.DataAccessor.SaveGlobalFontServerConfig(globalConfig);
            }
            catch(Exception ex)
            {
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this,"保存服务信息失败," + ex.Message);
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

        private void FrontServerConfigForm_Load(object sender, EventArgs e)
        {
            m_dataSource = new BindingList<FrontSeverConfigViewModel>();
            this.gridServerConfig.AutoGenerateColumns = false;
            this.gridServerConfig.DataSource = m_dataSource;

            GlobalFontServerConfig globalConfig  = USeManager.Instance.DataAccessor.GetGlobalFontServerConfig();
            if(globalConfig != null && globalConfig.ServerList != null && globalConfig.ServerList.Count >0)
            {
                foreach(FrontSeverConfig item in globalConfig.ServerList)
                {
                    m_dataSource.Add(FrontSeverConfigViewModel.Create(item));
                }
            }
        }

        private bool VerifyDataCheck(FrontSeverConfigViewModel data)
        {
            Debug.Assert(data != null);

            bool conditionStrIsNull = data.BrokerID == string.Empty ||
                data.BrokerName == string.Empty ||
                data.QuoteFrontAddress == string.Empty ||
                data.TradeFrontAddress == string.Empty;

            if (conditionStrIsNull)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "有选项为空，请填入信息......");
                return false;
            }

            if (data.QuoteFrontPort <=0 || data.QuoteFrontPort >=65535)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "请填入正确的行情端口号......");
                return false;
            }

            if (data.TradeFrontPort <= 0 || data.TradeFrontPort >= 65535)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "请填入正确的交易端口号......");
                return false;
            }

            return true;
        }
    }
}
