namespace USeFuturesSpirit
{
    partial class ArbitrageOrderListControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelOrderContainer = new System.Windows.Forms.Panel();
            this.arbitrageItemControl2 = new USeFuturesSpirit.ArbitrageOrderControl();
            this.arbitrageItemControl1 = new USeFuturesSpirit.ArbitrageOrderControl();
            this.panelOrderContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelOrderContainer
            // 
            this.panelOrderContainer.AutoScroll = true;
            this.panelOrderContainer.Controls.Add(this.arbitrageItemControl2);
            this.panelOrderContainer.Controls.Add(this.arbitrageItemControl1);
            this.panelOrderContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOrderContainer.Location = new System.Drawing.Point(0, 0);
            this.panelOrderContainer.Name = "panelOrderContainer";
            this.panelOrderContainer.Size = new System.Drawing.Size(836, 584);
            this.panelOrderContainer.TabIndex = 0;
            // 
            // arbitrageItemControl2
            // 
            this.arbitrageItemControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.arbitrageItemControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.arbitrageItemControl2.Location = new System.Drawing.Point(0, 153);
            this.arbitrageItemControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.arbitrageItemControl2.Name = "arbitrageItemControl2";
            this.arbitrageItemControl2.Size = new System.Drawing.Size(836, 200);
            this.arbitrageItemControl2.TabIndex = 1;
            // 
            // arbitrageItemControl1
            // 
            this.arbitrageItemControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.arbitrageItemControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.arbitrageItemControl1.Location = new System.Drawing.Point(0, 0);
            this.arbitrageItemControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.arbitrageItemControl1.Name = "arbitrageItemControl1";
            this.arbitrageItemControl1.Size = new System.Drawing.Size(836, 153);
            this.arbitrageItemControl1.TabIndex = 0;
            // 
            // ArbitrageOrderListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelOrderContainer);
            this.Name = "ArbitrageOrderListControl";
            this.Size = new System.Drawing.Size(836, 584);
            this.panelOrderContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelOrderContainer;
        private ArbitrageOrderControl arbitrageItemControl1;
        private ArbitrageOrderControl arbitrageItemControl2;
    }
}
