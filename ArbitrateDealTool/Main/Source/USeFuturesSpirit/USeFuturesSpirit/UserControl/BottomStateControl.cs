using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;
using System.Diagnostics;

namespace USeFuturesSpirit
{
    public partial class BottomStateControl : USeUserControl
    {
        public delegate void SelectedProductChangeEventHandle(USeProduct product);
        public event SelectedProductChangeEventHandle OnSelectedProduct;

        public BottomStateControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize()
        {
            this.btnChangeProduct.Text = "选择品种";
            USeOrderDriver orderDriver = USeManager.Instance.OrderDriver;
            USeQuoteDriver quoteDriver = USeManager.Instance.QuoteDriver;

            quoteDriver.OnDriverStateChanged += QuoteDriver_OnDriverStateChanged;
            orderDriver.OnDriverStateChanged += OrderDriver_OnDriverStateChanged;
            USeManager.Instance.AutoTraderManager.OnAutoTraderNotify += AutoTraderManager_OnAutoTraderNotify;

            SetQuoteDriverState(quoteDriver.DriverState);
            SetOrderDriverState(orderDriver.DriverState);
        }

        #region 事件处理
        private void OrderDriver_OnDriverStateChanged(object sender, USeOrderDriverStateChangedEventArgs e)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeOrderDriverStateChangedEventArgs>(OrderDriver_OnDriverStateChanged), sender, e);
                return;
            }

            SetOrderDriverState(e.NewState);
        }

        private void QuoteDriver_OnDriverStateChanged(object sender, USeQuoteDriverStateChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeQuoteDriverStateChangedEventArgs>(QuoteDriver_OnDriverStateChanged), sender, e);
                return;
            }
            SetQuoteDriverState(e.NewState);
        }


        private void SetOrderDriverState(USeOrderDriverState state)
        {
            if (state == USeOrderDriverState.Ready)
            {
                this.pbxOrderDriverState.Image = global::USeFuturesSpirit.Properties.Resources.green1;
                this.lblOrderDriverState.ForeColor = Color.Green;
            }
            else
            {
                this.pbxOrderDriverState.Image = global::USeFuturesSpirit.Properties.Resources.red1;
                this.lblOrderDriverState.ForeColor = Color.Red;
            }
        }

        private void SetQuoteDriverState(USeQuoteDriverState state)
        {
            if (state == USeQuoteDriverState.Ready)
            {
                this.pbxQuoteDriverState.Image = global::USeFuturesSpirit.Properties.Resources.green1;
                this.lblQuoteDriverState.ForeColor = Color.Green;
            }
            else
            {
                this.pbxQuoteDriverState.Image = global::USeFuturesSpirit.Properties.Resources.red1;
                this.lblQuoteDriverState.ForeColor = Color.Red;
            }
        }


        private void AutoTraderManager_OnAutoTraderNotify(AutoTraderNotice notice)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AutoTraderNotifyEventHandle(AutoTraderManager_OnAutoTraderNotify), notice);
                return;
            }

            string text = string.Format("套利单<{0}>{1}", notice.Alias, notice.Message);
            this.lblAutoTradeInfo.Text = text;
        }
        #endregion

        /// <summary>
        /// 设置默认产品。
        /// </summary>
        /// <param name="product">产品。</param>
        public void SetDefaultProduct(USeProduct product)
        {
            SetChangeProductButtonText(product);
        }

        private void btnChangeProduct_Click(object sender, EventArgs e)
        {
            SelectProductForm form = new SelectProductForm(this, (USeProduct)this.btnChangeProduct.Tag);

            if (DialogResult.Yes == form.ShowDialog())
            {
                SetChangeProductButtonText(form.SelectedProduct);
                SafeFireSelectedProductChanged(form.SelectedProduct);
            }
        }

        /// <summary>
        /// 设定更改产品按钮Text。
        /// </summary>
        /// <param name="product">产品。</param>
        private void SetChangeProductButtonText(USeProduct product)
        {
            if (product == null) return;

            string productName = product.ShortName;

            if (string.IsNullOrEmpty(productName)) return;
            this.btnChangeProduct.Tag = product;
            this.btnChangeProduct.Text = productName;

            if (productName.Length == 1)
            {
                this.btnChangeProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            }
            else if (productName.Length == 2)
            {
                this.btnChangeProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            }
            else if (productName.Length == 3)
            {
                this.btnChangeProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            }
            else if (productName.Length == 4)
            {
                this.btnChangeProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            }
            else
            {
                this.btnChangeProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            }
        }

        /// <summary>
        /// 触发产品变更事件。
        /// </summary>
        /// <param name="product">产品。</param>
        private void SafeFireSelectedProductChanged(USeProduct product)
        {
            SelectedProductChangeEventHandle handle = this.OnSelectedProduct;

            if (handle != null)
            {
                try
                {
                    handle(product);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }
    }
}
