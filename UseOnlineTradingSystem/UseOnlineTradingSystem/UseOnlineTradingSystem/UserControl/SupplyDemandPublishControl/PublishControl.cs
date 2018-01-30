using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseOnlineTradingSystem
{
    public partial class PublishControl : UserControl
    {
        //窗口关闭事件-对外发布
        public EventHandler DisposeFormEvent;

        public PublishControl()
        {
            InitializeComponent();
            //Initialize();
        }
        private void PublishControl_SizeChanged(object sender, System.EventArgs e)
        {
            int w = this.Width - this.panel2.Width;
            this.panel3.Size = new System.Drawing.Size(w, this.panel3.Height);
        }
        public  void Initialize()
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

        private void Picture_Click(object sender, EventArgs e)
        {
            //控件下拉
            DisposeFormEvent(sender, e);
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
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        /// <summary>
        /// Picture移进移出更改样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Picture_MouseMoveIn(object sender, MouseEventArgs e)
        {
            //this.pictureBox1.BackColor = Color.Silver;
            this.pictureBox1.Cursor = Cursors.Hand;
        }

        private void Picture_MouseMoveLeave(object sender, EventArgs e)
        {
            //this.pictureBox1.BackColor = Color.DarkGray;
            this.pictureBox1.Cursor = Cursors.Default;

        }
    }
}
