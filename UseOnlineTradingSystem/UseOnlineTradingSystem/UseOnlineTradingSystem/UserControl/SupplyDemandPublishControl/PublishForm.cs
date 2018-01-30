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
    /// 供需发布窗口
    /// </summary>
    public partial class PublishForm : Form
    {
        public PublishForm()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            this.supplyDemandPubishControl1.Initialize();
            this.tradingInfoControl1.Initialize();
            this.supplyDemandPubishControl1.OnPublishSuccessEvent += this.tradingInfoControl1.OnPublishSuccessChangedEvent;
        }

        public void SetDefultWhenLogin(ContractCategoryDic contractVo)
        {
            //供需发布/交易列表初始化之后给一个初始的品类初始化

            this.supplyDemandPubishControl1.SetDefultListWhenLogin(contractVo);
            this.tradingInfoControl1.SetDefultListWhenLogin(contractVo);
        }

        /// <summary>
        /// 改变品类
        /// </summary>
        /// <param name="vo"></param>
        public void SetContractIDChanged(ContractCategoryDic vo)
        {
            this.supplyDemandPubishControl1.OnContractcCategoryVoChanged(vo);
            this.tradingInfoControl1.OnContractcCategoryVoChanged(vo);
        }

        public TradingInfoControl TradingInfoCtrol
        {
            get
            {
                return this.tradingInfoControl1;
            }
        }
    }
}
