using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI.Docking;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 供需发布窗口
    /// </summary>
    public partial class PublishForm :  ToolWindow
    {
        public PublishForm(string name)
        {
            InitializeComponent();

            //双缓冲设置模式
            SetStyle(
                     ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.Selectable
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.SupportsTransparentBackColor,
                     true);

            this.Name = name;
        }

        public void Initialize()
        {
            this.supplyDemandPubishControl2.Initialize();
            this.tradingInfoControl2.Initialize();
            this.supplyDemandPubishControl2.OnPublishSuccessEvent += this.tradingInfoControl2.OnPublishSuccessChangedEvent;
        }

        public void SetDefultWhenLogin(ContractCategoryDic contractVo)
        {
            //供需发布/交易列表初始化之后给一个初始的品类初始化

            this.supplyDemandPubishControl2.SetDefultListWhenLogin(contractVo);
            this.tradingInfoControl2.SetDefultListWhenLogin(contractVo);
        }

        /// <summary>
        /// 改变品类
        /// </summary>
        /// <param name="vo"></param>
        public void SetContractIDChanged(ContractCategoryDic vo)
        {
            this.supplyDemandPubishControl2.OnContractcCategoryVoChanged(vo);
            this.tradingInfoControl2.OnContractcCategoryVoChanged(vo);
        }

        public TradingInfoControl TradingInfoCtrol
        {
            get
            {
                return this.tradingInfoControl2;
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            this.supplyDemandPubishControl2.Width = 625;
            this.tradingInfoControl2.Size = new Size(this.Width - this.supplyDemandPubishControl2.Width,this.supplyDemandPubishControl2.Height);
        }
    }
}
