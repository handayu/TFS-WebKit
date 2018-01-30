using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public partial class SelectProductControl : USeUserControl
    {
        private const int VARIETIES_ITEM_COUNT = 4; // 每行按钮个数
        public event EventHandler DoubleClickEvent;
        private DateTime m_clickTime = DateTime.Now;

        public SelectProductControl()
        {
            InitializeComponent();
            ClearView();
        }

        private USeProduct m_selectedProduct = null;

        public override void Initialize()
        {
            ClearView();

            List<USeProduct> productList = USeManager.Instance.OrderDriver.QueryProducts();

            InitializeVarietiesPanel(this.panelSFE, productList, USeMarket.SHFE);
            InitializeVarietiesPanel(this.panelCZCE, productList, USeMarket.CZCE);
            InitializeVarietiesPanel(this.panelDCE, productList, USeMarket.DCE);
            InitializeVarietiesPanel(this.panelCFFE, productList, USeMarket.CFFEX);
        }

        public USeProduct SelectedProduct
        {
            get
            {
                return m_selectedProduct;
            }
            set
            {
                m_selectedProduct = value;
                SetSelectVarieties(value);
            }
        }

       
        private void ClearView()
        {
            this.panelDCE.Controls.Clear();
            this.panelSFE.Controls.Clear();
            this.panelCZCE.Controls.Clear();
            this.panelCFFE.Controls.Clear();
        }

        private void InitializeVarietiesPanel(Panel panel, List<USeProduct> products, USeMarket market)
        {
            panel.Controls.Clear();
            List<USeProduct> productList = (from i in products
                                            where i.Market == market
                                            select i).ToList();
                                            
            if (productList == null || productList.Count <= 0) return;

            int x = 19;
            int y = 8;
            int xOffset = 103;
            int yOffset = 40;

            for (int i = 0; i < productList.Count; i++)
            {
                USeProduct product = productList[i];

                int xPos = x + ((i % VARIETIES_ITEM_COUNT) * xOffset);
                int yPos = y + ((i / VARIETIES_ITEM_COUNT) * yOffset);

                Button button = new System.Windows.Forms.Button();
                button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                button.ForeColor = System.Drawing.SystemColors.ControlText;
                button.Location = new System.Drawing.Point(xPos, yPos);
                button.Name = "button2";
                button.Size = new System.Drawing.Size(95, 35);
                button.TabIndex = 3;
                button.Text = product.ShortName;
                button.Tag = product;
                button.UseVisualStyleBackColor = true;
                button.Click += ProductButton_Click;
                panel.Controls.Add(button);
            }
        }

        private void ProductButton_Click(object sender, EventArgs e)
        {
            TimeSpan span = DateTime.Now - m_clickTime;
            if (span.TotalMilliseconds < SystemInformation.DoubleClickTime)
            {
                ProductButton_DoubleClick(sender, e);
                return;
            }

            m_clickTime = DateTime.Now;

            USeProduct product = (sender as Button).Tag as USeProduct;
            m_selectedProduct = product;
            SetSelectVarieties(product);
        }

        private void ProductButton_DoubleClick(object sender, EventArgs e)
        {
            USeProduct product = (sender as Button).Tag as USeProduct;
            m_selectedProduct = product;
            SetSelectVarieties(product);
            if (DoubleClickEvent != null)
            {
                DoubleClickEvent(sender, e);
            }
        }

        private void SetSelectVarieties(USeProduct product)
        {
            SetMarketPanelButton(this.panelSFE, product);
            SetMarketPanelButton(this.panelDCE, product);
            SetMarketPanelButton(this.panelCFFE, product);
            SetMarketPanelButton(this.panelCZCE, product);
        }

        /// <summary>
        /// 选中相应的Button
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="product"></param>
        private void GetMarketPanelButton(Panel panel, USeProduct product)
        {
            foreach (Control subControl in panel.Controls)
            {
                if ((subControl is Button) == false) continue;

                Button btn = subControl as Button;
                Debug.Assert(btn != null);
                if (btn.Tag == null) continue;
                Debug.Assert(btn.Tag != null && (btn.Tag is USeProduct));

                if ((btn.Tag as USeProduct).ProductCode == product.ProductCode)
                {
                    this.tabControl1.SelectedTab = (TabPage)panel.Parent;
                    SetMarketPanelButton(panel, product);
                }
            }
        }

        /// <summary>
        /// 寻找并选中
        /// </summary>
        /// <param name="product"></param>
        private void FindButtonProduct(USeProduct product)
        {
            GetMarketPanelButton(this.panelSFE, product);
            GetMarketPanelButton(this.panelDCE, product);
            GetMarketPanelButton(this.panelCFFE, product);
            GetMarketPanelButton(this.panelCZCE, product);
        }
        public void SetDefultBottomButtonText(USeProduct product)
        {
            FindButtonProduct(product);
        }

        private void SetMarketPanelButton(Panel panel, USeProduct product)
        {
            foreach (Control subControl in panel.Controls)
            {
                if ((subControl is Button) == false) continue;

                Button btn = subControl as Button;
                Debug.Assert(btn != null);
                if (btn.Tag == null) continue;
                Debug.Assert(btn.Tag != null && (btn.Tag is USeProduct));

                if ((btn.Tag as USeProduct).ProductCode == product.ProductCode)
                {
                    btn.BackColor = System.Drawing.Color.LawnGreen;
                    btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    btn.ForeColor = System.Drawing.Color.Purple;
                    btn.UseVisualStyleBackColor = false;
                }
                else
                {
                    btn.BackColor = SystemColors.Control;
                    btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    btn.ForeColor = System.Drawing.SystemColors.ControlText;
                    btn.UseVisualStyleBackColor = false;
                }
            }
        }
    }
}
