using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using USe.TradeDriver.Common;
using System.IO;
using System.Threading;
using System.Diagnostics;
using USeFuturesSpirit.Arbitrage;

namespace USeFuturesSpirit
{
    public partial class MainForm : Form
    {
        private USeProduct m_product = null;

        public MainForm()
        {
            InitializeComponent();
        }

        public ArbitrageQuoteListControl GetQuoteListControl
        {
            get { return this.arbitrageQuoteListControl1; }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            USeInvestorBaseInfo investorInfo = USeManager.Instance.OrderDriver.QueryInvestorInfo();
            this.Text = string.Format("{0}  {1}", Program.AppName, investorInfo.RealName);

            InitializeUserControl();
        }

        private void InitializeUserControl()
        {
            this.orderBookListControl1.Initialize();
            this.tradeBookListControl1.Initialize();
            this.positionListControl1.Initialize();
            this.arbitrageOrderListControl1.Initialize();
            this.arbitrageLogViewControl1.Initialize();
            this.arbitrageRunStateControl1.Initialize();
            this.bottomStateControl1.Initialize();
            this.investorFundControl1.Initialize();
            this.alarmNoticeViewControl1.Initialize();

            this.bottomStateControl1.OnSelectedProduct += BottomStateControl1_OnSelectedProduct;

            //this.arbitrageQuoteListControl1.OnSelectCombineInstrumentChanged += QuoteListControl1_OnSelectInstrumentChanged;

            UserDefineSetting userSetting = USeManager.Instance.SystemConfigManager.GetUserDefineSetting();
            Debug.Assert(userSetting != null);

            this.arbitrageQuoteListControl1.Initialize();
            this.arbitrageQuoteListControl1.OnSelectCombineInstrumentChanged += ArbitrageQuoteListControl1_OnSelectCombineInstrumentChanged;

            USeProduct defaultProduct = new USeProduct()
            {
                ProductCode = userSetting.LastSelectProduct,
                ShortName = userSetting.LastSelectProductName
            };

            this.bottomStateControl1.SetDefaultProduct(defaultProduct);
            SetDefaultProduct(defaultProduct);
        }

        private void ArbitrageQuoteListControl1_OnSelectCombineInstrumentChanged(ArbitrageCombineInstrument combineInstrument)
        {
            USeProduct product = USeManager.Instance.OrderDriver.QueryProduct(combineInstrument.ProductID);
            Debug.Assert(product != null);

            ArbitrageOrderCreateForm form = new ArbitrageOrderCreateForm(combineInstrument,product);
            form.Show(this);
        }

        private void QuoteListControl1_OnSelectInstrumentChanged(USeInstrument instrument)
        {
            try
            {
                //this.simpleOrderPanelControl1.ChangeInstrument(instrument);
            }
            catch (Exception ex)
            {

            }
        }

        private void BottomStateControl1_OnSelectedProduct(USeProduct product)
        {
            SetDefaultProduct(product);
            m_product = product;
            UserDefineSetting userSetting = USeManager.Instance.SystemConfigManager.GetUserDefineSetting();
            userSetting.LastSelectProduct = product.ProductCode;
            userSetting.LastSelectProductName = product.ShortName;

            try
            {
                USeManager.Instance.SystemConfigManager.SaveUserDefineSetting(userSetting);
            }
            catch(Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        private void SetDefaultProduct(USeProduct product)
        {
            m_product = product;
            this.arbitrageQuoteListControl1.Product = product;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch(keyData)
            {
                case Keys.F2:
                    {
                        SimpleOrderForm form = USeManager.Instance.SimpleOrderForm;
                        if(form == null)
                        {
                            form = new SimpleOrderForm();
                            //this.arbitrageQuoteListControl1.OnSelectCombineInstrumentChanged += form.GetSimpleOrderPanelControl.ChangeInstrument;
                            USeManager.Instance.SimpleOrderForm = form;
                        }
                        
                        form.Activate();
                        form.Show();
                        break;
                    }
                case Keys.F3:
                    {
                        TestTraderDriverSimulateForm form = USeManager.Instance.TestTradeDriverSimulateForm;
                        form.Activate();
                        form.Show();
                        break;
                    }
                case Keys.F8:
                    {
                        string localApplicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        string rootPath  = Path.Combine(localApplicationDataPath, @"USe\USeFuturesSpirit\Data");
                        System.Diagnostics.Process.Start("explorer.exe", rootPath);
                        break;
                    }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void tsmiHistoryArbitrage_Click(object sender, EventArgs e)
        {
            HistoryArbitrageOrderListForm form = new HistoryArbitrageOrderListForm();
            form.Show();
        }

        private void tsmSimpleOrderPanel_Click(object sender, EventArgs e)
        {
            SimpleOrderForm form = USeManager.Instance.SimpleOrderForm;
            if (form == null)
            {
                form = new SimpleOrderForm();
                //this.quoteListControl1.OnSelectInstrumentChanged += form.GetSimpleOrderPanelControl.ChangeInstrument;
                USeManager.Instance.SimpleOrderForm = form;
            }

            form.Activate();
            form.Show();
        }

        private void tsmiSystemConfig_Click(object sender, EventArgs e)
        {
            UseSystemConfigForm form = new UseSystemConfigForm();
            form.ShowDialog();
        }

        private void tsmiAboutMe_Click(object sender, EventArgs e)
        {
            AboutMeForm form = new AboutMeForm();
            form.ShowDialog(this);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(USeManager.Instance.AutoTraderManager.HasUnfinishOrderBook())
            {
                string text = "当前有挂单中的委托单,退出后可能不能正常处理套利单状态";
                if(DialogResult.Yes != USeFuturesSpiritUtility.ShowYesNoMessageBox(this,text))
                {
                    e.Cancel = true;
                    return;
                }
            }
            this.investorFundControl1.Stop();
            this.arbitrageOrderListControl1.Stop();
        }

        private void tsmiArbitrageInstrument_Click(object sender, EventArgs e)
        {
            ArbitrageQuoteChoiceForm form = new ArbitrageQuoteChoiceForm();
            form.ProductId = m_product;
            form.OnArbitrageCombineInstrumentListChanged += this.arbitrageQuoteListControl1.OnArbitrageCombineInstrumentList_Changed;
            //form.OnArbitrageCombineInstrumentAddChanged += this.arbitrageQuoteListControl1.OnArbitrageCombineInstrumentAdd_Changed;
            //form.OnArbitrageCombineInstrumentRemoveChanged += this.arbitrageQuoteListControl1.OnArbitrageCombineInstrumentRemove_Changed;
            form.ShowDialog();

        }

        private void tsmiArbitrageOrderSettings_Click(object sender, EventArgs e)
        {
            ArbitrageDefultOrderSettingsForm form = new ArbitrageDefultOrderSettingsForm();
            form.ShowDialog();
        }
    }
}
