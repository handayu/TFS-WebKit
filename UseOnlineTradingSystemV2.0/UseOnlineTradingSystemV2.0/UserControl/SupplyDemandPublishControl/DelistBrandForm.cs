using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 摘牌弹出框
    /// </summary>
    public partial class DelistBrandForm : Form
    {
        private OneListed m_commodityInfo;

        public DelistBrandForm()
        {
            InitializeComponent();
        }


        /// <summary>
        ///初始化窗口控件之后设置默认值 
        /// </summary>
        /// <param name="commodityInfo"></param>
        public void SetCommodityInfo(OneListed commodityInfo)
        {
            SetDefultFieldToDelistBrand(commodityInfo);
        }

        private void DelistBrandForm_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelistBrandForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        #region 摘牌成功事件通知
        public EventHandler OnDelistSuccessEvent;
        #endregion

        public Transaction m_dataResult = null;

        /// <summary>
        /// 挂牌行情表调用，获取参数，准备摘牌操作
        /// </summary>
        /// <param name="commodityInfo"></param>
        public void SetDefultFieldToDelistBrand(OneListed commodityInfo)
        {
            if (commodityInfo == null)
            {
                MessageBox.Show("摘牌参数不能为空");
                return;
            }

            m_commodityInfo = commodityInfo;

            CreditQuotaRequest args = new CreditQuotaRequest()
            {
                compId = commodityInfo.publisherId,
            };


            decimal canUseAvaliableMoney = 0m;
            //信用额度
            CreditQuotaResponse avaliableQuatityArgs = HttpService.GetDelistAditQuatity(args);
            if (avaliableQuatityArgs != null && avaliableQuatityArgs.data != null && avaliableQuatityArgs.data.creditLineAmt != null &&
                avaliableQuatityArgs.data.creditLineAmt != "")
            {
                canUseAvaliableMoney = Convert.ToDecimal(avaliableQuatityArgs.data.creditLineAmt) - Convert.ToDecimal(avaliableQuatityArgs.data.creditUsedAmt);
                this.label_AvaliableCridetCash.Text = string.Format("{0:C}",(Convert.ToDecimal(avaliableQuatityArgs.data.creditLineAmt) - Convert.ToDecimal(avaliableQuatityArgs.data.creditUsedAmt)));
            }
            else
            {
                canUseAvaliableMoney = 0m;
                this.label_AvaliableCridetCash.Text = string.Format("{0:C}", 0m);
            }

            //品牌-中文显示(原来是ID，现在切换为Name)
            this.label_Brand.Text = HttpService.QueryHttpBrandName(commodityInfo);

            //升贴水
            if (commodityInfo.premium == null)
            {
                this.label_UpDownPrice.Text = "---";
            }
            else
            {
                this.label_UpDownPrice.Text = commodityInfo.premium;
            }
            //else if (Convert.ToDecimal(commodityInfo.premium) > 0m)
            //{
            //    this.label_UpDownPrice.Text = "+" + commodityInfo.premium;
            //}
            //else
            //{
            //    this.label_UpDownPrice.Text = "-" + commodityInfo.premium;
            //}

            //成交量
            this.comboBox_VolumnChoice.Items.Clear();
            this.comboBox_VolumnChoice.Text = "";

            int min;
            int.TryParse(commodityInfo.minDealQuantity, out min);
            int all;
            int.TryParse(commodityInfo.commAvailableQuantity, out all);
            for (int i = min; i < all; i += min)
            {
                this.comboBox_VolumnChoice.Items.Add(i.ToString());
            }
            this.comboBox_VolumnChoice.Items.Add(all.ToString());
            if (comboBox_VolumnChoice.Items.Count > 0)
            {
                comboBox_VolumnChoice.SelectedIndex=0;
            }

            //查询结算价
            Dictionary<string, ContractCategoryDic> vo = DataManager.Instance.GetContractcCategoryVo();
            if (vo == null || vo.Values.Count <= 0)
            {
                this.label_StandPrice.Text = "---";
            }
            else
            {
                foreach (KeyValuePair<string, ContractCategoryDic> kv in vo)
                {
                    if (kv.Value.contractMonthMap == null || kv.Value.contractMonthMap.Count <= 0)
                    {
                        continue;
                    }
                    if (kv.Value.id != commodityInfo.cid) continue;

                    foreach (KeyValuePair<string, ContractBasePrice> kvMonth in kv.Value.contractMonthMap)
                    {
                        decimal d;
                        decimal.TryParse(kvMonth.Value.preSettlementPrice,out d);
                        this.label_StandPrice.Text = string.Format("{0:C}", d);
                        break;
                    }
                }
            }

            //需要保证金/应付保证金
            //需要保证金比例目前所有的都为10%-[hanyu]保证金比例目前查询有误
            decimal needMarginAll = Convert.ToDecimal(this.label_StandPrice.Text.Replace("¥", "").Replace(",", "")) * Convert.ToDecimal(this.comboBox_VolumnChoice.SelectedItem) * 0.1m;
            this.label_NeedMagin2.Text = string.Format("{0:C}",needMarginAll);

            //应该付 = 总需要 - 可用金额
            decimal shouldMarginLeave = needMarginAll - (Convert.ToDecimal(avaliableQuatityArgs.data.creditLineAmt) - Convert.ToDecimal(avaliableQuatityArgs.data.creditUsedAmt));

            if(shouldMarginLeave <= 0m )
            {
                this.label_NeedMargin1.Text = string.Format("{0:C}", 0m);
            }
            else
            {
                this.label_NeedMargin1.Text = string.Format("{0:C}", shouldMarginLeave);
            }

            //可用余额
            Funds accountData = HttpService.GetAccountDataInfos();
            if (accountData == null || accountData.availableAmount == "" || accountData.availableAmount == null)
            {
                this.label1_LeaveCash.Text = string.Format("{0:C}", 0m);
            }
            else
            {
                decimal avaliableAccountNum = 0m;
                decimal.TryParse(accountData.availableAmount, out avaliableAccountNum);

                if (avaliableAccountNum == 0m)
                {
                    this.label1_LeaveCash.Text = string.Format("{0:C}",0m);
                }
                else
                {
                    this.label1_LeaveCash.Text =  string.Format("{0:C}", avaliableAccountNum);
                }
            }
        }

        /// <summary>
        /// 刷新参数
        /// </summary>
        /// <param name="commodityInfo"></param>
        private void RefrashArgs(OneListed commodityInfo)
        {
            CreditQuotaRequest args = new CreditQuotaRequest()
            {
                compId = commodityInfo.publisherId,
            };


            decimal canUseAvaliableMoney = 0m;
            //信用额度
            CreditQuotaResponse avaliableQuatityArgs = HttpService.GetDelistAditQuatity(args);
            if (avaliableQuatityArgs != null && avaliableQuatityArgs.data != null && avaliableQuatityArgs.data.creditLineAmt != null &&
                avaliableQuatityArgs.data.creditLineAmt != "")
            {
                canUseAvaliableMoney = Convert.ToDecimal(avaliableQuatityArgs.data.creditLineAmt) - Convert.ToDecimal(avaliableQuatityArgs.data.creditUsedAmt);
                this.label_AvaliableCridetCash.Text = string.Format("{0:C}", (Convert.ToDecimal(avaliableQuatityArgs.data.creditLineAmt) - Convert.ToDecimal(avaliableQuatityArgs.data.creditUsedAmt)));
            }
            else
            {
                canUseAvaliableMoney = 0m;
                this.label_AvaliableCridetCash.Text = string.Format("{0:C}", 0m);
            }

            //查询结算价
            Dictionary<string, ContractCategoryDic> vo = DataManager.Instance.GetContractcCategoryVo();
            if (vo == null || vo.Values.Count <= 0)
            {
                this.label_StandPrice.Text = "---";
            }
            else
            {
                foreach (KeyValuePair<string, ContractCategoryDic> kv in vo)
                {
                    if (kv.Value.contractMonthMap == null || kv.Value.contractMonthMap.Count <= 0)
                    {
                        continue;
                    }
                    if (kv.Value.id != commodityInfo.cid) continue;

                    foreach (KeyValuePair<string, ContractBasePrice> kvMonth in kv.Value.contractMonthMap)
                    {
                        decimal d;
                        decimal.TryParse(kvMonth.Value.preSettlementPrice, out d);
                        this.label_StandPrice.Text = string.Format("{0:C}", d);
                        break;
                    }
                }
            }

            //需要保证金/应付保证金
            //需要保证金比例目前所有的都为10%-[hanyu]保证金比例目前查询有误
            decimal needMarginAll = Convert.ToDecimal(this.label_StandPrice.Text.Replace("¥", "").Replace(",", "")) * Convert.ToDecimal(this.comboBox_VolumnChoice.SelectedItem) * 0.1m;
            this.label_NeedMagin2.Text = string.Format("{0:C}", needMarginAll);

            //应该付 = 总需要 - 可用金额
            decimal shouldMarginLeave = needMarginAll - (Convert.ToDecimal(avaliableQuatityArgs.data.creditLineAmt) - Convert.ToDecimal(avaliableQuatityArgs.data.creditUsedAmt));

            if (shouldMarginLeave <= 0m)
            {
                this.label_NeedMargin1.Text = string.Format("{0:C}", 0m);
            }
            else
            {
                this.label_NeedMargin1.Text = string.Format("{0:C}", shouldMarginLeave);
            }

            //可用余额
            Funds accountData = HttpService.GetAccountDataInfos();
            if (accountData == null || accountData.availableAmount == "" || accountData.availableAmount == null)
            {
                this.label1_LeaveCash.Text = string.Format("{0:C}", 0m);
            }
            else
            {
                decimal avaliableAccountNum = 0m;
                decimal.TryParse(accountData.availableAmount, out avaliableAccountNum);

                if (avaliableAccountNum == 0m)
                {
                    this.label1_LeaveCash.Text = string.Format("{0:C}", 0m);
                }
                else
                {
                    this.label1_LeaveCash.Text = string.Format("{0:C}", avaliableAccountNum);
                }
            }
        }


        /// <summary>
        /// 订阅基价期货行情
        /// </summary>
        private void SubscribeFutureMarketData()
        {

        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// 参数检查
        /// </summary>
        /// <returns></returns>
        private bool VerifyArgumentsBeforeAction(out string strInfo)
        {
            //检查保证金等参数
            strInfo = "";
            if (this.comboBox_VolumnChoice.Text == "")
            {
                strInfo = "成交量不能为空，请重新选择或取消操作";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_OK_Click(object sender, EventArgs e)
        {
            string strInfos = "";
            if (false == VerifyArgumentsBeforeAction(out strInfos))
            {
                MessageBox.Show(strInfos);
                return;
            }

            #region 测试数据
            DelistingRequest requireDelistBrandArgs = new DelistingRequest();
            if (DataManager.Instance.LoginData == null)
            {
                MessageBox.Show("登陆状态有误，请重新登陆");
                return;
            }

            requireDelistBrandArgs.clientId = DataManager.Instance.LoginData.currentCompany.id + "_pc";
            requireDelistBrandArgs.mqId = "test";
            requireDelistBrandArgs.commId = Convert.ToInt64(m_commodityInfo.id);
            requireDelistBrandArgs.operationType = 2;
            requireDelistBrandArgs.basePrice = m_commodityInfo.fixedPrice; //绝对价格-基价
            requireDelistBrandArgs.commQuantity = this.comboBox_VolumnChoice.Text;
            requireDelistBrandArgs.cid = m_commodityInfo.cid;
            requireDelistBrandArgs.remarks = "我是韩宇在摘牌";

            DelistingResponse response = HttpService.PostDelistBrandOrder(requireDelistBrandArgs);

            if (response != null&&response.data != null )
            {
                m_dataResult = response.data;
                MessageBox.Show("摘牌成功!");
                //刷新行情列表
                DataManager.Instance.GetCommodity();
                //招牌成功对外通知
                if (OnDelistSuccessEvent != null)
                {
                    OnDelistSuccessEvent(this, null);
                }
            }
            else
            {
                if (response != null)
                {
                    MessageBox.Show("摘牌失败:" + response.msg);
                }
                else
                {
                    MessageBox.Show("摘牌失败");
                }
                return;
            }

            this.Close();
            #endregion

        }

        /// <summary>
        /// 成交量选择改变更改应付保证金额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectIndexChanged_Volumn(object sender, EventArgs e)
        {
            //if (this.comboBox_VolumnChoice.SelectedText == null || this.comboBox_VolumnChoice.SelectedText == "") return;
            //RefrashArgs(m_commodityInfo);
        }

        /// <summary>
        /// 摘牌的结果
        /// </summary>
        public Transaction DataListResult
        {
            get
            {
                return m_dataResult;
            }
        }

        private void SelectValueChanged_Volumn(object sender, EventArgs e)
        {
            //if (this.comboBox_VolumnChoice.SelectedText == null || this.comboBox_VolumnChoice.SelectedText == "") return;
            //RefrashArgs(m_commodityInfo);
        }

        private void SelcetionChanged(object sender, EventArgs e)
        {
            if (this.comboBox_VolumnChoice.SelectedItem == null) return;
            RefrashArgs(m_commodityInfo);
        }
    }
}
