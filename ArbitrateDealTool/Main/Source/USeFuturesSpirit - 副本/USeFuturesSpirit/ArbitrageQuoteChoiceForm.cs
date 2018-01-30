using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;
using System.Diagnostics;
using System.Collections;
using System.Collections.ObjectModel;


namespace USeFuturesSpirit
{
    public partial class ArbitrageQuoteChoiceForm : Form
    {
        #region Member
        /// <summary>
        /// 确定。发布最终结果。
        /// </summary>
        public event ArbitrageCombineInstrumentChangeEventHandle OnArbitrageCombineInstrumentListChanged;
        public delegate void ArbitrageCombineInstrumentChangeEventHandle(List<ArbitrageCombineInstrument> m_arbitrageInstrumentList);

        /// <summary>
        /// 添加事件。
        /// </summary>
        public event ArbitrageCombineInstrumentAddChangeEventHandle OnArbitrageCombineInstrumentAddChanged;
        public delegate void ArbitrageCombineInstrumentAddChangeEventHandle(ArbitrageCombineInstrument m_arbitrageInstrumentList);

        /// <summary>
        /// 移除事件。
        /// </summary>
        public event ArbitrageCombineInstrumentChangeRemoveEventHandle OnArbitrageCombineInstrumentRemoveChanged;
        public delegate void ArbitrageCombineInstrumentChangeRemoveEventHandle(ArbitrageCombineInstrument m_arbitrageInstrumentList);


        /// <summary>
        /// 品种详细信息列表
        /// </summary>
        private List<USeInstrumentDetail> m_instrumentDetailList = null;

        /// <summary>
        /// 品种列表
        /// </summary>
        private List<USeInstrument> m_instrumentList = null;

        /// <summary>
        /// 套利合约
        /// </summary>
        private List<ArbitrageCombineInstrument> m_arbitrageInstrumentList = null;

        /// <summary>
        /// 当前品种
        /// </summary>
        private USeProduct m_product = null;

        /// <summary>
        /// 经纪商和账户为了区分不同账户和经纪商下的设置
        /// </summary>
        private string m_brokerId = string.Empty;
        private string m_account = string.Empty;

        /// <summary>
        /// [hanyu绑定不能修改？]
        /// </summary>
        //private List<USeInstrument> m_dataSourceInsOne = null;
        //private List<USeInstrument> m_dataSourceInsTwo = null;
        //private List<USeProduct> m_dataSourceProduct = null;
        private List<ArbitrageCombineInstrument> m_dataSourceArbitrageCombineIns = null;

        #endregion

        /// <summary>
        /// 设置的套利合约字典。
        /// </summary>
        /// 
        public List<ArbitrageCombineInstrument> IResult
        {
            get { return new List<ArbitrageCombineInstrument>(m_arbitrageInstrumentList.ToArray()); }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="null"></param>
        public ArbitrageQuoteChoiceForm()
        {
            InitializeComponent();

            m_instrumentList = new List<USeInstrument>();
            m_instrumentDetailList = new List<USeInstrumentDetail>();

        }

        public void SetDefultProduct()
        {
            ProductViewAlisa defultProduct = new ProductViewAlisa();
            defultProduct.Product = m_product;
            //[hanyu]默认选中defult选项
            foreach (object o in this.comboBox_Product.Items)
            {
                USeProduct product = o as USeProduct;
                if (product == m_product)
                {
                    this.comboBox_Product.SelectedItem = defultProduct.Product;
                }
            }
        }

        private void ArbitrageQuoteChoiceForm_Load(object sender, EventArgs e)
        {
            try
            {
                Initialize();

                InitializeAccountInfo();

                InitializeProductCombox();

                //先到文件中读取所有之前存在的品种套利组合
                m_arbitrageInstrumentList = GetArbitrageCombineInsList();

                //默认品种选择
                SetDefultProduct();

            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                USeFuturesSpiritUtility.ShowErrrorMessageBox(this, ex.Message);
                this.Close();
            }
        }


        private void Initialize()
        {
            this.comboBox_Product.Items.Clear();
            this.listBox_InsOne.Items.Clear();
            this.listBox_InsTwo.Items.Clear();
            this.listBox_ArbitrageIns.Items.Clear();

            this.listBox_InsOne.SelectionMode = SelectionMode.One;
            this.listBox_InsOne.SelectionMode = SelectionMode.One;
            this.listBox_ArbitrageIns.SelectionMode = SelectionMode.One;

            //m_dataSourceInsOne = new List<USeInstrument>();
            //m_dataSourceInsTwo = new List<USeInstrument>();
            //m_dataSourceProduct = new List<USeProduct>();
            m_dataSourceArbitrageCombineIns = new List<ArbitrageCombineInstrument>();

            this.comboBox_Product.DisplayMember = "ProductAlisa";
            this.comboBox_Product.ValueMember = "ProductCode";


            //this.listBox_InsOne.DataSource = m_dataSourceInsOne;
            this.listBox_InsOne.DisplayMember = "InstrumentCode";

            //this.listBox_InsTwo.DataSource = m_dataSourceInsTwo;
            this.listBox_InsTwo.DisplayMember = "InstrumentCode";

            //this.listBox_ArbitrageIns.DataSource = m_dataSourceArbitrageCombineIns;
            this.listBox_InsTwo.DisplayMember = "ArbitrageAlisa";

        }

        /// <summary>
        /// 初始化账户和经纪商
        /// </summary>
        private void InitializeAccountInfo()
        {
            m_brokerId = USeManager.Instance.LoginUser.BrokerId;
            m_account = USeManager.Instance.LoginUser.Account;
            Debug.Assert(string.IsNullOrEmpty(m_brokerId) == false);
            Debug.Assert(string.IsNullOrEmpty(m_account) == false);
        }


        private void InitializeProductCombox()
        {
            this.comboBox_Product.Items.Clear();

            List<USeProduct> useProductList = null;
            try
            {
                useProductList = USeManager.Instance.OrderDriver.QueryProducts();
            }
            catch (Exception ex)
            {
                throw new Exception("查询产品信息异常:" + ex.Message);
            }

            foreach (USeProduct product in useProductList)
            {
                ProductViewAlisa productAlisa = new ProductViewAlisa();
                productAlisa.Product = product;
                this.comboBox_Product.Items.Add(productAlisa);
            }


        }

        /// <summary>
        /// 当前品种属性(外部调用)。
        /// </summary>
        public USeProduct ProductId
        {
            get { return m_product; }
            set
            {
                if (value != m_product)
                {
                    m_product = value;
                    ChangegProduct(value);
                }
            }
        }

        /// <summary>
        /// 更换品种。
        /// </summary>
        /// <param name="product"></param>
        private void ChangegProduct(USeProduct product)
        {
            if (string.IsNullOrEmpty(product.ProductCode)) return;
            try
            {
                m_instrumentDetailList.Clear();
                m_instrumentList.Clear();

                List<USeInstrumentDetail> instrumentDetailList = USeManager.Instance.OrderDriver.QueryInstrumentDetail(product.ProductCode);
                if (instrumentDetailList == null) return;

                foreach (USeInstrumentDetail instrumentDetail in instrumentDetailList)
                {
                    m_instrumentDetailList.Add(instrumentDetail);

                    USeInstrument instrument = instrumentDetail.Instrument;

                    m_instrumentList.Add(instrument);
                }

                m_product = product;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        /// <summary>
        /// 清空两腿ListBox并填充合约
        /// </summary>
        private void FillInstrumentListBox()
        {
            Debug.Assert(m_instrumentDetailList.Count != 0);

            this.listBox_InsOne.Items.Clear();
            this.listBox_InsTwo.Items.Clear();

            foreach (USeInstrument ins in m_instrumentList)
            {
                this.listBox_InsOne.Items.Add(ins);
                this.listBox_InsTwo.Items.Add(ins);
            }
        }

        /// <summary>
        /// 清空套利组合并填充合约
        /// </summary>
        private void FillCombineInstrumentListBox()
        {
            if (this.listBox_ArbitrageIns.Items.Count > 0)
            {
                this.listBox_ArbitrageIns.Items.Clear();

            }

            //更换品种的时候先去查一遍，有之前保存的品种则显示
            foreach (ArbitrageCombineInstrument combineIns in m_arbitrageInstrumentList)
            {
                if (combineIns.ProductID == m_product.ProductCode)
                {
                    this.listBox_ArbitrageIns.Items.Add(combineIns);
                }
            }
        }

        /// <summary>
        /// 确定按键。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Ok_Click(object sender, EventArgs e)
        {
            //存文件，成功之后发布事件;存文件失败继续修改
            Debug.Assert(m_arbitrageInstrumentList != null);

            bool iDataAccesserStore = ArbitrageInstrumentsDataAccesserStore(m_arbitrageInstrumentList);
            if (iDataAccesserStore == false)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "保存套利合约单设置失败，请重试");
                return;
            }

            SafeFireOnArbitrageCombineInsChanged(m_arbitrageInstrumentList);

            this.DialogResult = DialogResult.Yes;
            this.Close();

        }

        /// <summary>
        /// 取消按键。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Add_Click(object sender, EventArgs e)
        {
            Debug.Assert(this.listBox_InsOne.Items.Count != 0);
            Debug.Assert(this.listBox_InsTwo.Items.Count != 0);

            USeInstrument firstIns = (USeInstrument)this.listBox_InsOne.SelectedItem;
            USeInstrument SecIns = (USeInstrument)this.listBox_InsTwo.SelectedItem;


            if (firstIns == null)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "请选择近月合约");
                return;
            }
            if (SecIns == null)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "请选择远月合约");
                return;
            }
            if (firstIns.InstrumentCode == SecIns.InstrumentCode)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "不能选择相同的合约");
                return;
            }

            if (firstIns.InstrumentCode.CompareTo(SecIns.InstrumentCode) > 0)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "请选择较近月的远月合约");
                return;
            }
            else
            {

                ArbitrageCombineInstrument arbitrageCombineInstrument = new ArbitrageCombineInstrument();
                arbitrageCombineInstrument.FirstInstrument = firstIns;
                arbitrageCombineInstrument.SecondInstrument = SecIns;
                arbitrageCombineInstrument.ProductID = m_product.ProductCode;

                foreach (object o in this.listBox_ArbitrageIns.Items)
                {
                    ArbitrageCombineInstrument combineInstrument = o as ArbitrageCombineInstrument;
                    if (combineInstrument.FirstInstrument.Equals(arbitrageCombineInstrument.FirstInstrument) &&
                        combineInstrument.SecondInstrument.Equals(arbitrageCombineInstrument.SecondInstrument))
                    {
                        USeFuturesSpiritUtility.ShowWarningMessageBox(this, "已经存在该套利组合合约，不能重复添加");
                        return;
                    }
                }

                //添加到界面
                this.listBox_ArbitrageIns.Items.Add(arbitrageCombineInstrument);

                //添加到内存
                m_arbitrageInstrumentList.Add(arbitrageCombineInstrument);

                //发布添加事件通知
                SafeFireOnArbitrageCombineInsAddChanged(arbitrageCombineInstrument);

            }

        }

        /// <summary>
        /// 存储套利组合合约设置。
        /// </summary>
        /// <param name="arbiInstrumentList"></param>
        /// <returns></returns>
        private bool ArbitrageInstrumentsDataAccesserStore(List<ArbitrageCombineInstrument> arbiInstrumentList)
        {
            Debug.Assert(arbiInstrumentList != null);

            try
            {
                USeManager.Instance.DataAccessor.SaveCombineInstruments(m_brokerId, m_account, arbiInstrumentList);
            }
            catch (Exception ex)
            {
                throw new Exception("保存套利组合合约异常:" + ex.Message);
            }

            return true;
        }


        /// <summary>
        /// List更改通知事件。
        /// </summary>
        private void SafeFireOnArbitrageCombineInsChanged(List<ArbitrageCombineInstrument> arbiInstrumentList)
        {
            if (OnArbitrageCombineInstrumentListChanged != null)
            {
                try
                {
                    OnArbitrageCombineInstrumentListChanged(arbiInstrumentList);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }


        /// <summary>
        /// List更改通知事件。
        /// </summary>
        private void SafeFireOnArbitrageCombineInsAddChanged(ArbitrageCombineInstrument arbiInstrumentList)
        {
            if (OnArbitrageCombineInstrumentAddChanged != null)
            {
                try
                {
                    OnArbitrageCombineInstrumentAddChanged(arbiInstrumentList);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }


        /// <summary>
        /// List更改通知事件。
        /// </summary>
        private void SafeFireOnArbitrageCombineInsRemoveChanged(ArbitrageCombineInstrument arbiInstrumentList)
        {
            if (OnArbitrageCombineInstrumentRemoveChanged != null)
            {
                try
                {
                    OnArbitrageCombineInstrumentRemoveChanged(arbiInstrumentList);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 文件中获取ArbitrageCombineIns合约表
        /// </summary>
        /// <returns></returns>
        private List<ArbitrageCombineInstrument> GetArbitrageCombineInsList()
        {
            List<ArbitrageCombineInstrument> arbitrageCombineInstrumentList = new List<ArbitrageCombineInstrument>();

            try
            {
                arbitrageCombineInstrumentList = USeManager.Instance.DataAccessor.GetCombineInstruments(m_brokerId, m_account);
            }
            catch (Exception ex)
            {
                throw new Exception("从文件获取套利组合合约异常：" + ex.Message);
            }

            return arbitrageCombineInstrumentList;
        }


        /// <summary>
        /// 点击移除。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            int count = this.listBox_ArbitrageIns.Items.Count;

            if (count <= 0)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "套利组合合约列表为空");
                return;
            }
            else if (this.listBox_ArbitrageIns.SelectedItems.Count == 0)
            {
                USeFuturesSpiritUtility.ShowWarningMessageBox(this, "请选择需要从套利组合列表中移除的组合合约");
                return;
            }

            ArbitrageCombineInstrument combineIns = this.listBox_ArbitrageIns.SelectedItem as ArbitrageCombineInstrument;

            //控件删除
            this.listBox_ArbitrageIns.Items.Remove(combineIns);

            //内存删除
            Debug.Assert(m_arbitrageInstrumentList.Contains(combineIns));
            m_arbitrageInstrumentList.Remove(combineIns);

            //发布移除事件
            SafeFireOnArbitrageCombineInsRemoveChanged(combineIns);

        }

        /// <summary>
        /// 产品Combox更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            USeProduct product = ((ProductViewAlisa)this.comboBox_Product.SelectedItem).Product;
            //更改产品
            ChangegProduct(product);
            //填充两个腿列表
            FillInstrumentListBox();
            //填充套利组合约列表
            FillCombineInstrumentListBox();
        }

        private void button_User_Click(object sender, EventArgs e)
        {
            //对外发布更改事件
            SafeFireOnArbitrageCombineInsChanged(m_arbitrageInstrumentList);

            USeFuturesSpiritUtility.ShowWarningMessageBox(this, "已保存设置，可继续操作");
            return;
        }

    }

    public class ProductViewAlisa
    {
        public USeProduct Product
        {
            get;
            set;
        }

        public string ProductAlisa
        {
            get { return "(" + Product.ProductCode + ")" + Product.ShortName; }
        }

        public override string ToString()
        {
            return "(" + Product.ProductCode + ")" + Product.ShortName; 
        }

    }
}
