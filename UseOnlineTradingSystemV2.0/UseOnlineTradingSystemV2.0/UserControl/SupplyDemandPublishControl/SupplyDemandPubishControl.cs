using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using mPaint;
using UseOnlineTradingSystem.Properties;

namespace UseOnlineTradingSystem
{
    public partial class SupplyDemandPubishControl : UserControl
    {
        private string m_nowProcessId = string.Empty;   //目前处理的商品ID
        private ContractCategoryDic m_contractVo = null;//目前处理的contractVo

        protected MBorad m_SelectBoard;//画板
        protected MSelect m_selectControl;//等级-选择框


        #region 挂牌对外事件通知

        public EventHandler OnPublishSuccessEvent;

        #endregion

        public SupplyDemandPubishControl()
        {
            InitializeComponent();
            //Initialize();
        }

        /// <summary>
        /// 组件初始化之后调用，完成业务初始化和参数初始化
        /// </summary>
        /// 
        public void Initialize()
        {
            VisualPanelChangedPriceCoice();

            #region 初始化Combox控件
            InitializeLevelComBox();
            InitializeBrandCombox();
            InitializeWareHouseCombox();
            #endregion

            RECT defult = new RECT();
            this.m_SelectBoard = new MBorad();
            this.m_SelectBoard.Dock = DockStyle.Fill;
            this.m_SelectBoard.Location = new Point(0, 0);
            this.m_SelectBoard.Name = "cBoard";
            this.m_SelectBoard.BackColor = Color.Black;
            //this.m_SelectBoard.Visible = Visible;
            //this.m_SelectBoard.Click += M_SelectBoard_Click;
            //this.m_SelectBoard.DoubleClick += Board_DoubleClick;
            //this.m_SelectBoard.MouseWheel += Board_MouseWheel;
            this.tableLayoutPanel1.Controls.Add(m_SelectBoard, 1, 0);

            #region 选择框
            m_selectControl = new MSelect();
            m_selectControl.BackgroundImage = Resources.select_normal;
            m_selectControl.MouseClickImage = Resources.select_press;
            m_selectControl.MouseEnterImage = Resources.select_normal;
            m_selectControl.Font = MCommonData.d4Font;
            m_selectControl.ForeColor = COLOR.RGB(MCommonData.fontColor4);
            m_selectControl.DropDownBoxForeColor = COLOR.RGB(MCommonData.fontColor5);
            m_selectControl.DropDownBoxBackColor = COLOR.RGB(MCommonData.fontColor4);
            m_selectControl.DropDownBoxRowMouseEnterColor = COLOR.RGB(MCommonData.fontColor13);
            m_selectControl.Text = "请选择";
            m_selectControl.TextChangeEvent += M_selectControl_TextChangeEvent;
            m_selectControl.Visible = true;
            this.m_SelectBoard.AddControl(m_selectControl);

            this.m_SelectBoard.Size = this.m_SelectBoard.Size;
            this.m_SelectBoard.Draw();

            #endregion

            //更新查询一下资金和使用率(挂单成功且为保证金模式下，再刷新一次)
            RefrashAvaliableAndRadio();
            ResetControlSettings();

            Draw();

        }

        /// <summary>
        /// 重绘
        /// </summary>
        protected void Draw()
        {
            if (m_SelectBoard == null)
            {
                return;
            }

            //m-board
            Control control = this.tableLayoutPanel1.GetControlFromPosition(1,0);
            Rectangle positionRec =  control.Parent.ClientRectangle;
       
            //修改画板大小
            m_selectControl.ImageRectangle = new RECT(positionRec.Left , positionRec.Top,positionRec.Right,positionRec.Bottom);

            this.m_SelectBoard.Draw();
        }

        /// <summary>
        /// 选择框改变
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void M_selectControl_TextChangeEvent(object arg1, string arg2)
        {
            int i = 0;
        }

        /// <summary>
        /// 等级自定义控件选择项改变
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void Select_TextChangeEvent(object arg1, string arg2)
        {
            int i = 0;
        }


        /// <summary>
        /// 查询更新资金和资金使用率
        /// </summary>
        private void RefrashAvaliableAndRadio()
        {
            Funds accountData =  HttpService.GetAccountDataInfos();
            if (accountData == null) return;
            if (accountData.availableAmount == "" || accountData.availableAmount == null) return;
            this.label_Cash.Text = accountData.availableAmount;

            decimal freezenAmount = 0m;
            decimal.TryParse(accountData.freezeAmount, out freezenAmount);
            decimal avaliableAccountNum = 0m;
            decimal.TryParse(accountData.availableAmount, out avaliableAccountNum);

            if(avaliableAccountNum == 0m)
            {
                this.label_Cash.Text = "0.00" + "元";
                this.label_UseCash.Text = "0.00" + "%";
            }
            else
            {
                this.label_Cash.Text = string.Format("{0:C}", avaliableAccountNum) + "元";
                decimal resultCal = freezenAmount / (avaliableAccountNum + freezenAmount);
                this.label_UseCash.Text =  Math.Round(resultCal*100m,2).ToString()  + "%";
            }

        }

        /// <summary>
        /// 登陆初始化
        /// </summary>
        public void SetDefultListWhenLogin(ContractCategoryDic contractVo)
        {
            //初始化http查询各类业务基础信息-等级-品牌-仓库
            SetDefultContract(contractVo);
        }

        /// <summary>
        /// 初始化先隐藏需要切换的Panel
        /// </summary>
        private void VisualPanelChangedPriceCoice()
        {
            this.panel_DeadPrice.Visible = false;
            this.panel_SpotPrice.Visible = false;
        }

        private void InitializeLevelComBox()
        {
            this.comboBox_WareHouse.DisplayMember = "infoName";
            this.comboBox_WareHouse.ValueMember = "id";
        }

        private void InitializeWareHouseCombox()
        {
            this.comboBox_WareHouse.DisplayMember = "warehouseName";
            this.comboBox_WareHouse.ValueMember = "id";
        }

        private void InitializeBrandCombox()
        {
            this.comboBox_Brand.DisplayMember = "infoName";
            this.comboBox_Brand.ValueMember = "id";
        }

        private void ResetCombox()
        {
            this.comboBox_Brand.Items.Clear();
            this.comboBox_Level.Items.Clear();
            this.comboBox_WareHouse.Items.Clear();
            this.comboBox_ChoiceIns.Items.Clear();

            ClearComInsCaterageADdNew();
        }

        private void SetDefultContract(ContractCategoryDic contractVo)
        {
            OnContractcCategoryVoChanged(contractVo);
        }

        /// <summary>
        /// 外部事件回调
        /// </summary>
        /// <param name="contractVo"></param>
        public void OnContractcCategoryVoChanged(ContractCategoryDic contractVo)
        {
            //获得contract的ID作为Key,根据ID获取LevelID,BrandID,WareHouse
            if (contractVo == null) return;

            m_contractVo = contractVo;

            m_nowProcessId = contractVo.id;
            Debug.Assert(m_nowProcessId != "" && m_nowProcessId != null);

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<ContractCategoryDic>(OnContractcCategoryVoChanged), contractVo);
                return;
            }

            ResetCombox();

            //初始化http查询各类业务基础信息-等级-品牌-仓库
            QueryHttpBrandListToCombox(m_nowProcessId);
            QueryHttpWareHouseToCombox(m_nowProcessId);

            //默认combox全部选择第一项
            if(this.comboBox_Level.Items.Count > 0)
            {
                this.comboBox_Level.SelectedIndex = 0;
            }
            else
            {
                this.comboBox_Level.SelectedIndex = -1;
            }

            //
            if (this.comboBox_Brand.Items.Count > 0)
            {
                this.comboBox_Brand.SelectedIndex = 0;
            }
            else
            {
                this.comboBox_Brand.SelectedIndex = -1;
            }

            //
            if (this.comboBox_WareHouse.Items.Count > 0)
            {
                this.comboBox_WareHouse.SelectedIndex = 0;
            }
            else
            {
                this.comboBox_WareHouse.SelectedIndex = -1;
            }

            if (this.comboBox_ChoiceIns.Items.Count > 0)
            {
                this.comboBox_ChoiceIns.SelectedIndex = 0;
            }
            else
            {
                this.comboBox_ChoiceIns.SelectedIndex = -1;
            }

            ResetControlSettings();

        }

        /// <summary>
        /// 查询所有品牌信息
        /// </summary>
        private void QueryHttpBrandListToCombox(string nowProcessId)
        {
            try
            {
                LevelBrandResponse levelBrandInfo =HttpService.GetBaseLevelBrandInfo(nowProcessId);
                if (levelBrandInfo == null || levelBrandInfo.Result == null) return;
                foreach (LevelBrandList infos in levelBrandInfo.Result)
                {
                    this.comboBox_Level.Items.Add(infos);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("供需发布初始化品牌信息异常:" + ex.Message);
            }

        }

        /// <summary>
        /// 查询所有仓库列表
        /// </summary>
        private void QueryHttpWareHouseToCombox(string nowProcessId)
        {
            try
            {
                WareHouseResponse wareHouseInfo = HttpService.GetWareHouseInfo(nowProcessId);
                if (wareHouseInfo == null || wareHouseInfo.Result == null) return;
                foreach (WareHouseInfo info in wareHouseInfo.Result)
                {
                    this.comboBox_WareHouse.Items.Add(info);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("供需发布初始化仓库列表信息异常:" + ex.Message);
            }

        }

        /// <summary>
        /// 重绘外侧的深度边框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaint(object sender, PaintEventArgs e)
        {
            //建立画笔
            Pen pen = new Pen(Brushes.White);
            pen.Width = 2;
            pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;

            ////左侧边框坐标
            Point leftStartPoint = new Point(this.panel6.Location.X, this.panel6.Location.Y);
            Point leftEndPoint = new Point(this.panel6.Location.X, this.panel5.Location.Y + this.panel5.Height);
            this.panel3.CreateGraphics().DrawLine(pen, leftStartPoint.X, leftStartPoint.Y, leftEndPoint.X, leftEndPoint.Y);

            ////上方边框坐标
            Point topStartPoint = new Point(this.panel6.Location.X, this.panel6.Location.Y);
            Point topEndPoint = new Point(this.panel6.Location.X + this.panel6.Width, this.panel6.Location.Y);
            this.panel3.CreateGraphics().DrawLine(pen, topStartPoint.X, topStartPoint.Y, topEndPoint.X, topEndPoint.Y);

            ////右侧边框坐标
            Point rightStartPoint = new Point(this.panel6.Location.X + this.panel6.Width, this.panel6.Location.Y);
            Point rightEndPoint = new Point(this.panel6.Location.X + this.panel6.Width, this.panel5.Location.Y + this.panel5.Height);
            this.panel3.CreateGraphics().DrawLine(pen, rightStartPoint.X, rightStartPoint.Y, rightEndPoint.X, rightEndPoint.Y);

            //下方边框坐标
            Point bottomStartPoint = new Point(this.panel5.Location.X, this.panel5.Location.Y + this.panel5.Height);
            Point bottomEndPoint = new Point(this.panel5.Location.X + this.panel5.Width, this.panel5.Location.Y + this.panel5.Height);
            this.panel3.CreateGraphics().DrawLine(pen, bottomStartPoint.X, bottomStartPoint.Y, bottomEndPoint.X, bottomEndPoint.Y);

            //释放资源
            pen.Dispose();

        }

        /// <summary>
        /// 检查挂单前是否正常登陆状态
        /// </summary>
        /// <returns></returns>
        private bool CheckIsLonginOK()
        {
            return true;
            //return DataManager.Instance.IsLogin;
        }

        /// <summary>
        /// 校验输入参数
        /// </summary>
        /// <returns></returns>
        private bool VerifyArguments(out string strInfo)
        {
            strInfo = "";

            int tradeVolumn;
            int.TryParse(textBox_Volumn.Text, out tradeVolumn);
            if(tradeVolumn <= 0)
            {
                strInfo = "数量不能为0或负数,请重新输入";
                return false;
            }

            int miniTradedVolumn;
            int.TryParse(textBox_MinTradeVolumn.Text, out miniTradedVolumn);
            if(miniTradedVolumn <= 0)
            {
                strInfo = "最小成交数量不能为0或负数,请重新输入";
                return false;
            }

            //如果一口价Check,检查固定价格校验
            if(radioButton_DeadPrice.Checked)
            {
                int DeadPrice;
                int.TryParse(textBox_SpreadPrice.Text, out DeadPrice);
                if (DeadPrice <= 0)
                {
                    strInfo = "固定价不能为0或负数,请重新输入";
                    return false;
                }
            }
            else
            {
                //int preminumPrice;
                //int.TryParse(textBox_UpDownPrice.Text, out preminumPrice);
                if (textBox_UpDownPrice.Text == "")
                {
                    strInfo = "升贴水不能为空，请填写升贴水的值";
                    return false;
                }

                if(this.comboBox_ChoiceIns.Text == "")
                {
                    strInfo = "参考合约不能为空,请重新输入";
                    return false;
                }
            }

            //校验conbox不能为空值
            if(this.comboBox_Level.Text == "" )
            {
                strInfo = "等级选择不能为空，请重新选择";
                return false;
            }

            if(this.comboBox_Brand.Text == "")
            {
                strInfo = "品牌选择不能为空，请重新选择";
                return false;
            }

            if (this.comboBox_WareHouse.Text == "")
            {
                strInfo = "仓库选择不能为空，请重新选择";
                return false;
            }            

            Debug.Assert(tradeVolumn > 0);
            Debug.Assert(miniTradedVolumn > 0);
            //输入数量必须大于最小成交数量的20倍
            if(tradeVolumn / miniTradedVolumn > 20 )
            {
                strInfo = "数量必须小于等于最小成交数量的20倍，请重新输入";
                return false;
            }

            //实货凭证为选择的时候，必须有仓单号
            if(radioButton_TrueVerify.Checked)
            {
                if(this.textBox_MarginCaret.Text == "")
                {
                    strInfo = "选择为实货凭证时，仓单必填";
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 参数重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Reset_Click(object sender, EventArgs e)
        {
            ResetControlSettings();
        }

        private void ResetControlSettings()
        {
            //全部清空重置
            this.radioButton_Buy.Checked = true;
            this.checkBox_IsCorDisVisual.Checked = false;
            this.textBox_Volumn.Clear();
            this.textBox_MinTradeVolumn.Clear();
            this.radioButton_DeadPrice.Checked = true;
            //界面切换并更改值
            this.radioButton_Margin.Checked = true;
            this.textBox_MarginCaret.ForeColor = Color.White;
            this.textBox_MarginCaret.Enabled = false;

            this.textBox_SpreadPrice.Clear();
            this.textBox_Remarks.Clear();

            this.panel_DeadSpotPriceChanged.Controls.Clear();
            this.panel_DeadSpotPriceChanged.Controls.Add(this.panel_DeadPrice);
            Control.ControlCollection collectionControls = this.panel_DeadPrice.Controls;
            this.panel_DeadPrice.Dock = DockStyle.Fill;
            this.panel_DeadPrice.Visible = true;
            foreach (Control ct in collectionControls)
            {
                if (ct.Name == "textBox_SpreadPrice")
                {
                    TextBox tBtn = (TextBox)ct;
                    tBtn.Clear();
                }
            }
        }

        /// <summary>
        /// 开始挂单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //TestGDIForm form = new TestGDIForm();
            //form.Initialize();
            //form.ShowDialog();


            if (false == CheckIsLonginOK())
            {
                MessageBox.Show("请检查是否正常登录状态！");
                return;
            }
 
            string strInfo;
            if (false == VerifyArguments(out strInfo))
            {
                MessageBox.Show("请检查参数挂牌参数是否填写正确:" + strInfo);
                return;
            }

            ListedRequest args = new ListedRequest();

            if(DataManager.Instance.LoginData == null)
            {
                MessageBox.Show("登陆状态有误，请重新登陆");
                return;
            }

            args.clientId = DataManager.Instance.LoginData.currentCompany.id + "_pc";
            args.mqId = "test";
            args.cid = m_contractVo.id;
            args.operationType = 1;
            args.commId = null;

            string cookies = DataManager.Instance.Cookies;
            args.securityToken = cookies;

            args.transType = this.radioButton_Buy.Checked ? TransType.Buy : TransType.Sell;//买卖方向
            LevelBrand lb = this.comboBox_Brand.SelectedItem as LevelBrand;//品牌
            if (lb != null)
            {
                args.commBrandId = lb.id;     //品牌combox获取
            }
            else
            {
                MessageBox.Show("请选择品牌!");
                return;
            }
            LevelBrandList lbl = this.comboBox_Level.SelectedItem as LevelBrandList;//等级
            if (lbl != null)
            {
                args.commLevel = lbl.id;       //等级combox获取
            }
            else
            {
                MessageBox.Show("请选择等级!");
                return;
            }
            WareHouseInfo wi = this.comboBox_WareHouse.SelectedItem as WareHouseInfo;//仓库
            if (wi != null)
            {
                args.warehouseId = wi.id;     //仓库combox获取
            }
            else
            {
                MessageBox.Show("请选择仓库!");
                return;
            }
            args.commTotalQuantity = this.textBox_Volumn.Text;//成交数
            args.minDealQuantity = this.textBox_MinTradeVolumn.Text; //最小成交数量
            args.showCompany = this.checkBox_IsCorDisVisual.Checked ? "1" : "0";  //是否显示公司名check获取 0显示 1不显示
            args.pricingMethod = this.radioButton_DeadPrice.Checked ? 1 : 0;  //固定价：点价
            if (args.pricingMethod == 1)
            {
                args.contract = "";
                args.contractName = "";
                args.premium = "";
                args.fixedPrice = this.textBox_SpreadPrice.Text;
            }
            else
            {
                //"clups"用于传给Web-必填字段
                if(this.comboBox_ChoiceIns.SelectedItem != null && (this.comboBox_ChoiceIns.SelectedItem as ContractBasePrice) != null)
                {
                    args.contract = (this.comboBox_ChoiceIns.SelectedItem as ContractBasePrice).category;

                }
                //"外盘铜"用于显示-必填字段
                args.contractName = this.comboBox_ChoiceIns.Text;

                args.fixedPrice = "";
                args.premium = this.textBox_UpDownPrice.Text;
            }

            args.ensureMethod = this.radioButton_Margin.Checked ? 0 : 1;//保证金：实物凭证
            if (args.ensureMethod == 1)
            {
                args.warehouseReceiptNum = this.textBox_MarginCaret.Text;
            }

            args.publisher = "Text_Hanyu";
            args.remarks = this.textBox_Remarks.Text;

            var response=  HttpService.PostBrandOrder(args);

            if (response != null && response.Success)
            {
                //重置
                ResetControlSettings();

                //且在保证金模式下，如果挂单成功还需要刷新资金和使用率
                RefrashAvaliableAndRadio();

                DataManager.Instance.GetCommodity();
                MessageBox.Show("挂单成功!");

                //挂单成功对外通知
                if (OnPublishSuccessEvent != null)
                {
                    OnPublishSuccessEvent(this, null);
                }
            }
            else
            {
                MessageBox.Show("挂单失败,"+ response.Msg);
                return;
            }
        }

        /// <summary>
        /// 等级Combox切换事件
        /// 等级切换-更换品牌ComBox的显示条目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combox_LevelChanged(object sender, EventArgs e)
        {
            this.comboBox_Brand.Items.Clear();

            LevelBrandList levelBrandInfos = (LevelBrandList)this.comboBox_Level.SelectedItem;
            foreach (LevelBrand info in levelBrandInfos.infoList)
            {
                this.comboBox_Brand.Items.Add(info);
            }

            this.comboBox_Brand.SelectedIndex = 0;
        }

        /// <summary>
        /// 保证金方式切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioBtn_MarginChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;
            if (!radioBtn.Checked)
            {
                this.label_MarginCaret.Text = "  仓单号:";
                this.textBox_MarginCaret.Clear();
                this.textBox_MarginCaret.Enabled = true;
            }
            else
            {
                this.label_MarginCaret.Text = "应付保证金:";
                this.textBox_MarginCaret.Clear();
                this.textBox_MarginCaret.ForeColor = Color.White;
                this.textBox_MarginCaret.Enabled = false;

                SetCalclateMargin(m_contractVo);
            }
        }

        /// <summary>
        /// 点价方式RadioBtn方式切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioBtn_DeadPriceChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;
            if (radioBtn.Checked)
            {
                this.label_DeadSpotPrice.Text = " 固定价:";
                this.panel_DeadSpotPriceChanged.Controls.Clear();
                this.panel_DeadSpotPriceChanged.Controls.Add(this.panel_DeadPrice);
                Control.ControlCollection collectionControls = this.panel_DeadPrice.Controls;
                this.panel_DeadPrice.Dock = DockStyle.Fill;
                this.panel_DeadPrice.Visible = true;
                foreach (Control ct in collectionControls)
                {
                    if (ct.Name == "textBox_SpreadPrice")
                    {
                        TextBox tBtn = (TextBox)ct;
                        tBtn.Clear();
                    }
                }
            }
            else
            {
                this.label_DeadSpotPrice.Text = "参考合约:";
                this.panel_DeadSpotPriceChanged.Controls.Clear();
                this.panel_DeadSpotPriceChanged.Controls.Add(this.panel_SpotPrice);
                this.panel_SpotPrice.Dock = DockStyle.Fill;
                this.panel_SpotPrice.Visible = true;
                //填充参考合约，清空textBox价格
                ClearComInsCaterageADdNew();

                Control.ControlCollection collectionControls = this.panel_SpotPrice.Controls;
                foreach (Control ct in collectionControls)
                {
                    if (ct.Name == "textBox_UpDownPrice")
                    {
                        TextBox tBtn = (TextBox)ct;
                        tBtn.Clear();
                    }
                }

            }


        }

        /// <summary>
        /// 清空点价模式下下的参考合约并重新添加
        /// </summary>
        private void ClearComInsCaterageADdNew()
        {
            this.comboBox_ChoiceIns.Text = "";
            this.comboBox_ChoiceIns.Items.Clear();
            Dictionary<string, ContractBasePrice> levelDic = m_contractVo.contractMonthMap;
            foreach (KeyValuePair<string, ContractBasePrice> kv in levelDic)
            {
                this.comboBox_ChoiceIns.Items.Add(kv.Value);
            }
        }

        /// <summary>
        /// 数量的变更事件动态计算保证金
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVolumnTradeChanged(object sender, EventArgs e)
        {
            //保证金的计算只和保证金模式有关系(contract的preSettlementPrice*volumn)
            //根据所选择的ID--品牌和响应的等级获取presettleMentPrice
            string str = this.textBox_Volumn.Text;
            foreach (char c in str)
            {
                if (c >= '0' && c <= '9')
                {
                    continue;
                }
                else
                {
                    this.textBox_Volumn.Clear();
                    MessageBox.Show("数量请输入标准的数字，不能包含其他字符");
                    break;
                }
            }

            if (!this.radioButton_Margin.Checked) return;
            SetCalclateMargin(m_contractVo);

        }

        /// <summary>
        /// 最小成交校验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMiniTradedVolumnChanged(object sender, EventArgs e)
        {
            string str = this.textBox_MinTradeVolumn.Text;
            foreach (char c in str)
            {
                if (c >= '0' && c <= '9')
                {
                    continue;
                }
                else
                {
                    this.textBox_MinTradeVolumn.Clear();
                    MessageBox.Show("最小成交数请输入标准的数字，不能包含其他字符");
                    break;
                }
            }
            return;
        }

        /// <summary>
        /// 保证金计算设置
        /// </summary>
        /// <param name="vo"></param>
        private void SetCalclateMargin(ContractCategoryDic vo)
        {
            if (vo == null) return;
            decimal marginRadio = 0.1m;

            MaginRadio marginRadioData= HttpService.GetCommpMarginRaioQuatity(new MaginRadioRequest() { categoryId = vo.id});
            if(marginRadioData != null && marginRadioData.unitValue != null && marginRadioData.unitValue != "")
            {
                marginRadio = Convert.ToDecimal(marginRadioData.unitValue);
            }

            Dictionary<string, ContractBasePrice> levelDic = vo.contractMonthMap;
            foreach (KeyValuePair<string, ContractBasePrice> kv in levelDic)
            {
                //[hanyu]算法问题修正-*保证金比例
                //if (kv.Key != (this.comboBox_Level.SelectedItem as LevelBrandInfos).infoName) continue;
                decimal margin = (Convert.ToDecimal(kv.Value.preSettlementPrice)) * (this.textBox_Volumn.Text == "" ? 0m : Convert.ToDecimal(this.textBox_Volumn.Text))* marginRadio;
                this.textBox_MarginCaret.Text = string.Format("{0:C}", margin);
                break;
            }
        }

        /// <summary>
        /// 升贴水的值发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextUpDownPrice_Changed(object sender, EventArgs e)
        {
            string str = this.textBox_UpDownPrice.Text;
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || ( c == '-'))
                {
                    continue;
                }
                else
                {
                    this.textBox_UpDownPrice.Clear();
                    MessageBox.Show("升贴水请输入标准的数字，不能包含其他字符");
                    break;
                }
            }
            return;
        }
    }
}
