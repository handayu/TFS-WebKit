using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using USe.Common;

namespace TradeRangeManager
{
    public partial class TradeRangeManagerForm : Form
    {
        /// <summary>
        /// 交易时间Config
        /// </summary>
        private List<ProductTradeRangeInfo> m_productTradeRangeInfoList = null;

        private List<ProductInstrumentInfo> m_productInstrumentInfoList = null;

        /// <summary>
        /// 绑定的数据源
        /// </summary>
        private BindingList<ProductInfoViewModel> m_productInfoSource = null;
        private BindingList<ProductTradeSectionViewModel> m_productTradeSectionSource = null;

        /// <summary>
        /// 数据存储器
        /// </summary>
        private IDataStore m_dataStore = null;

        public TradeRangeManagerForm()
        {
            InitializeComponent();
        }

        private void Initialize()
        {
            m_productInfoSource = new BindingList<ProductInfoViewModel>();
            m_productTradeSectionSource = new BindingList<ProductTradeSectionViewModel>();
            //m_dataStore = new IDataStore();

            //绑定数据源
            this.dataGridView_ProductInfo.AutoGenerateColumns = false;
            this.dataGridView_ProductInfo.DataSource = m_productInfoSource;

            this.dataGridView_TradeSection.AutoGenerateColumns = false;
            this.dataGridView_TradeSection.DataSource = m_productTradeSectionSource;

            //控件设置
            this.textBox_Market.ReadOnly = true;
            this.textBox_Product.ReadOnly = true;

        }

        /// <summary>
        /// 加载窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TradeRangeManagerForm_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Load_Click(object sender, EventArgs e)
        {
            // 1.从配置文件加载数据
            // 2.清空数据库
            // 3.保存到数据库

            //获取配置交易时间节
            try
            {
                m_productTradeRangeInfoList = ConfigManager.Instance.GetProductTradeRangeList();

                m_productInstrumentInfoList = ConfigManager.Instance.GetProductInstrumentInfoList();

            }
            catch (Exception ex)
            {
                throw new Exception("Load Config Failed :" + ex.Message);
            }

            //加载配置信息
            UpdateProductInfo(m_productTradeRangeInfoList);

            //获得汇总信息
            this.label_ProductNum.Text = GetInstrumentNum().ToString();
            this.label_MarketNum.Text = GetMarketNum().ToString();

        }


        /// <summary>
        /// 加载配置文件productInfo信息
        /// </summary>
        /// <param name="tradeRngList"></param>
        private void UpdateProductInfo(List<ProductTradeRangeInfo> tradeRngList)
        {
            Debug.Assert(tradeRngList != null);

            foreach(ProductTradeRangeInfo info in tradeRngList)
            {
                ProductInfoViewModel model = ProductInfoViewModel.Creat(info);
                m_productInfoSource.Insert(0, model);
            }
        }

        /// <summary>
        /// 点击品种信息，获得时间区间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridProductInfoCell_Click(object sender, DataGridViewCellMouseEventArgs e)
        {
            m_productTradeSectionSource.Clear();
            this.textBox_Market.Clear();
            this.textBox_Product.Clear();

            ProductInfoViewModel data_model = this.dataGridView_ProductInfo.Rows[e.RowIndex].DataBoundItem as ProductInfoViewModel;

            if (data_model == null) return;

            foreach (ProductTradeRangeInfo rangeInfo in m_productTradeRangeInfoList)
            {
                try
                {
                    if (rangeInfo.Name != data_model.Name) continue;

                    this.textBox_Product.Text = rangeInfo.Name;
                    this.textBox_Market.Text = rangeInfo.Exchange;

                    foreach (TradeRangeTimeSectionInfo timeSection in rangeInfo.TradeRangeTimeSectionsInfo)
                    {
                        ProductTradeSectionViewModel tradeSectioModel = new ProductTradeSectionViewModel();
                        tradeSectioModel.BeginTime = timeSection.BeginTime;
                        tradeSectioModel.EndTime = timeSection.EndTime;
                        tradeSectioModel.IsNight = timeSection.IsNight;

                        m_productTradeSectionSource.Insert(0, tradeSectioModel);
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception("GridProductInfoCell_Click Failed :" + ex.Message);
                }
                
            }

        }

        /// <summary>
        /// 获得合约数目
        /// </summary>
        /// <returns></returns>
        private int GetInstrumentNum()
        {
            Debug.Assert(m_productTradeRangeInfoList != null);
            return m_productTradeRangeInfoList.Count;
        }

        /// <summary>
        /// 获得市场数目
        /// </summary>
        /// <returns></returns>
        private int GetMarketNum()
        {
            Debug.Assert(m_productTradeRangeInfoList != null);
            
            List<string> marketList = new List<string>();


            foreach (ProductTradeRangeInfo info in m_productTradeRangeInfoList)
            {
                if(marketList.Contains(info.Exchange) ==false)
                {
                    marketList.Add(info.Exchange);
                }
            }

            return marketList.Count;
        }

        /// <summary>
        /// 右键添加条目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TradeRangeSectionAdd_Click(object sender, EventArgs e)
        {
            //打开只读控件，开始填入信息，并保存
            this.textBox_Market.ReadOnly = false;
            this.textBox_Product.ReadOnly = false;

            this.textBox_Market.Clear();
            this.textBox_Product.Clear();

            //清空等待添加
            this.m_productTradeSectionSource.Clear();
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        private bool Verify()
        {
            if(this.textBox_Market.Text == "")
            {
                MessageBox.Show(this, "请填入市场");
                return false;
            }

            if (this.textBox_Product.Text == "")
            {
                MessageBox.Show(this, "请填入合约");
                return false;
            }

            if (this.dataGridView_TradeSection.Rows.Count == 0)
            {
                MessageBox.Show(this, "交易区间不能为空");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保存到内存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void button_Save_Click(object sender, EventArgs e)
        //{
        //    ////校验信息，新增的并写入内存-->数据库
        //    //if (Verify() == false) return;

        //    ////写入内存
        //    //ProductTradeRangeInfo tradeRangeInfo = new ProductTradeRangeInfo();
        //    //tradeRangeInfo.Exchange = this.textBox_Market.Text;
        //    //tradeRangeInfo.Name = this.textBox_Product.Text;

        //    //List<TradeRangeTimeSectionInfo> listSection = new List<TradeRangeTimeSectionInfo>();

        //    //DataGridViewRowCollection collec =  this.dataGridView_TradeSection.Rows;
        //    //foreach(DataGridViewRow row in collec)
        //    //{
        //    //    ProductTradeSectionViewModel model = row.DataBoundItem as ProductTradeSectionViewModel;
        //    //    if (model == null) continue;

        //    //    TradeRangeTimeSectionInfo sectionInfo = new TradeRangeTimeSectionInfo();
        //    //    sectionInfo.BeginTime = model.BeginTime;
        //    //    sectionInfo.EndTime = model.EndTime;
        //    //    sectionInfo.IsNight = model.IsNight;
        //    //    listSection.Add(sectionInfo);

        //    //}

        //    //tradeRangeInfo.TradeRangeTimeSectionsInfo = listSection;

        //    //m_productTradeRangeInfoList.Add(tradeRangeInfo);

        //}

        /// <summary>
        /// 数据存储
        /// </summary>
        private void StoreTradeRangeInfo()
        {

        }


        /// <summary>
        /// 测试保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Save_Click(object sender, EventArgs e)
        {
            string dbConnStr = ConfigurationManager.ConnectionStrings["KLineDB"].ConnectionString;
            if (string.IsNullOrEmpty(dbConnStr))
            {
                throw new ArgumentException("Not found KLineDB ConnectionString");
            }

            try
            {
                DataStoreForMySql store = new DataStoreForMySql(dbConnStr);
                store.InternalInstrumentData(m_productInstrumentInfoList);
            }
            catch (Exception ex)
            {
                string text = "Create DataStore object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }

        }
    }
}
