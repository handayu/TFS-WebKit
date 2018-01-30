using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseOnlineTradingSystem
{
    public partial class InputTradingPriceForm : Form
    {
        private Transaction m_delistDataInfo;
        private bool m_delistCheckResult = false;

        public InputTradingPriceForm(Transaction dl)
        {
            InitializeComponent();
            SetInputTradingPriceControl(dl);
        }

        private void delistBrandRightControl1_Load(object sender, EventArgs e)
        {
        }

        private void PictureClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 退出点击事件
        /// </summary>
        public EventHandler DisposeEvent;

        public void SetInputTradingPriceControl(Transaction delistInfo)
        {
            m_delistDataInfo = delistInfo;

            Debug.Assert(m_delistDataInfo != null);
            this.label_UpDownPrice.Text = m_delistDataInfo.premium;

        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定-处理业务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_OK_Click(object sender, EventArgs e)
        {
            if (m_delistDataInfo == null) return;
            m_delistDataInfo.confirmPrice = this.textBox_Volumn.Text;
            //校验业务逻辑-并处理-摘牌核销业务

            #region 测试数据
            WriteOffRequest requireDelistBrandVerifyArgs = new WriteOffRequest();

            if (DataManager.Instance.LoginData == null)
            {
                MessageBox.Show("登陆状态有误，请重新登陆");
                return;
            }

            requireDelistBrandVerifyArgs.clientId = DataManager.Instance.LoginData.currentCompany.id + "_pc";
            requireDelistBrandVerifyArgs.mqId = "test";
            requireDelistBrandVerifyArgs.commId = Convert.ToInt64(m_delistDataInfo.commId);//唯一的commID挂牌标示
            requireDelistBrandVerifyArgs.operationType = 5; //核销操作
            requireDelistBrandVerifyArgs.confirmPrice = m_delistDataInfo.confirmPrice;
            requireDelistBrandVerifyArgs.orderNo = m_delistDataInfo.orderNo;
            requireDelistBrandVerifyArgs.remarks = "我是韩宇在摘牌核销";
            WriteOffResponse response = HttpService.PostDelistBrandOrderVerify(requireDelistBrandVerifyArgs);

            if (response != null && response.data != null)
            {
                if (response.data.confirmStatus == "2")
                {
                    MessageBox.Show("待对方确认价格 !");
                }
                else if (response.data.confirmStatus == "1")
                {
                    MessageBox.Show("待我方确认价格!");

                }
                else if (response.data.confirmStatus == "3")
                {
                    MessageBox.Show("双方价格不符!");

                }
                else if (response.data.confirmStatus == "4")
                {
                    MessageBox.Show("核销成功!");
                    m_delistCheckResult = true;
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("摘牌核销失败，请检查参数并重新摘牌核销!");
                return;
            }

            #endregion

        }

        public bool Result
        {
            get
            {
                return m_delistCheckResult;
            }
        }

    }
}
