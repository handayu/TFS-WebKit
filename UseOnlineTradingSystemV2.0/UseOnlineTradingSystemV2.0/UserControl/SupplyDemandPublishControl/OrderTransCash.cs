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
    public partial class OrderTransCash : Form
    {
        /// <summary>
        /// 成交列表条目
        /// </summary>
        private Transaction m_transTradedOrder = null;

        public OrderTransCash(Transaction transTradedOrder)
        {
            InitializeComponent();

            this.button_Ok.BackColor = Color.FromArgb(255, 255, 255);
            this.button_Cancel.BackColor = Color.FromArgb(255, 255, 255);

            this.m_transTradedOrder = transTradedOrder;
            if (transTradedOrder == null) return;

            //品牌显示
            if(transTradedOrder.BrandName == null || transTradedOrder.BrandName == "")
            {
                this.label_brand.Text = "---";
            }
            else
            {
                this.label_brand.Text = transTradedOrder.BrandName;
            }

            //等级显示
            if (transTradedOrder.commLevelName == null || transTradedOrder.commLevelName == "")
            {
                this.label_leave.Text = "---";
            }
            else
            {
                this.label_leave.Text = transTradedOrder.commLevelName;
            }

            //数量显示
            if (transTradedOrder.commTotalQuantity == null || transTradedOrder.commTotalQuantity == "")
            {
                this.label_volumn.Text = "---";
            }
            else
            {
                this.label_volumn.Text = transTradedOrder.commTotalQuantity;
            }

            //仓库显示
            if (transTradedOrder.warehouseName == null || transTradedOrder.warehouseName == "")
            {
                this.label_warse.Text = "---";
            }
            else
            {
                this.label_warse.Text = transTradedOrder.warehouseName;
            }
        }

        /// <summary>
        /// Cancel-鼠标进入移出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_MouseMove(object sender, MouseEventArgs e)
        {
            this.button_Cancel.BackColor = Color.FromArgb(255, 128, 0);
        }

        private void Cancel_MouseLeave(object sender, EventArgs e)
        {
            this.button_Cancel.BackColor = Color.FromArgb(255, 255, 255);
        }

        /// <summary>
        /// OK-鼠标进入移出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ok_MouseMove(object sender, MouseEventArgs e)
        {
            this.button_Ok.BackColor = Color.FromArgb(255, 128, 0);
        }

        private void Ok_MouseLeave(object sender, EventArgs e)
        {
            this.button_Cancel.BackColor = Color.FromArgb(255, 255, 255);
        }

        /// <summary>
        /// 直接退出-仅展示用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
