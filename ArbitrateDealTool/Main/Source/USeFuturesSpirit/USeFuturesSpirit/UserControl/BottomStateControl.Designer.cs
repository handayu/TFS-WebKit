namespace USeFuturesSpirit
{
    partial class BottomStateControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BottomStateControl));
            this.pbxOrderDriverState = new System.Windows.Forms.PictureBox();
            this.lblOrderDriverState = new System.Windows.Forms.Label();
            this.lblQuoteDriverState = new System.Windows.Forms.Label();
            this.pbxQuoteDriverState = new System.Windows.Forms.PictureBox();
            this.lblAutoTradeInfo = new System.Windows.Forms.Label();
            this.btnChangeProduct = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOrderDriverState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxQuoteDriverState)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxOrderDriverState
            // 
            this.pbxOrderDriverState.Image = ((System.Drawing.Image)(resources.GetObject("pbxOrderDriverState.Image")));
            this.pbxOrderDriverState.Location = new System.Drawing.Point(145, 9);
            this.pbxOrderDriverState.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbxOrderDriverState.Name = "pbxOrderDriverState";
            this.pbxOrderDriverState.Size = new System.Drawing.Size(21, 23);
            this.pbxOrderDriverState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxOrderDriverState.TabIndex = 12;
            this.pbxOrderDriverState.TabStop = false;
            // 
            // lblOrderDriverState
            // 
            this.lblOrderDriverState.AutoSize = true;
            this.lblOrderDriverState.Location = new System.Drawing.Point(170, 14);
            this.lblOrderDriverState.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOrderDriverState.Name = "lblOrderDriverState";
            this.lblOrderDriverState.Size = new System.Drawing.Size(31, 13);
            this.lblOrderDriverState.TabIndex = 11;
            this.lblOrderDriverState.Text = "交易";
            // 
            // lblQuoteDriverState
            // 
            this.lblQuoteDriverState.AutoSize = true;
            this.lblQuoteDriverState.Location = new System.Drawing.Point(110, 14);
            this.lblQuoteDriverState.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQuoteDriverState.Name = "lblQuoteDriverState";
            this.lblQuoteDriverState.Size = new System.Drawing.Size(31, 13);
            this.lblQuoteDriverState.TabIndex = 10;
            this.lblQuoteDriverState.Text = "行情";
            // 
            // pbxQuoteDriverState
            // 
            this.pbxQuoteDriverState.Image = global::USeFuturesSpirit.Properties.Resources.red1;
            this.pbxQuoteDriverState.Location = new System.Drawing.Point(88, 9);
            this.pbxQuoteDriverState.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbxQuoteDriverState.Name = "pbxQuoteDriverState";
            this.pbxQuoteDriverState.Size = new System.Drawing.Size(21, 23);
            this.pbxQuoteDriverState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxQuoteDriverState.TabIndex = 9;
            this.pbxQuoteDriverState.TabStop = false;
            // 
            // lblAutoTradeInfo
            // 
            this.lblAutoTradeInfo.Location = new System.Drawing.Point(212, 11);
            this.lblAutoTradeInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAutoTradeInfo.Name = "lblAutoTradeInfo";
            this.lblAutoTradeInfo.Size = new System.Drawing.Size(295, 19);
            this.lblAutoTradeInfo.TabIndex = 8;
            this.lblAutoTradeInfo.Text = "---";
            this.lblAutoTradeInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnChangeProduct
            // 
            this.btnChangeProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeProduct.ForeColor = System.Drawing.Color.Purple;
            this.btnChangeProduct.Location = new System.Drawing.Point(0, 2);
            this.btnChangeProduct.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnChangeProduct.Name = "btnChangeProduct";
            this.btnChangeProduct.Size = new System.Drawing.Size(66, 38);
            this.btnChangeProduct.TabIndex = 1;
            this.btnChangeProduct.Text = "品种品种";
            this.btnChangeProduct.UseVisualStyleBackColor = true;
            this.btnChangeProduct.Click += new System.EventHandler(this.btnChangeProduct_Click);
            // 
            // BottomStateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbxOrderDriverState);
            this.Controls.Add(this.lblOrderDriverState);
            this.Controls.Add(this.lblQuoteDriverState);
            this.Controls.Add(this.pbxQuoteDriverState);
            this.Controls.Add(this.lblAutoTradeInfo);
            this.Controls.Add(this.btnChangeProduct);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "BottomStateControl";
            this.Size = new System.Drawing.Size(608, 41);
            ((System.ComponentModel.ISupportInitialize)(this.pbxOrderDriverState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxQuoteDriverState)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnChangeProduct;
        private System.Windows.Forms.Label lblAutoTradeInfo;
        private System.Windows.Forms.PictureBox pbxQuoteDriverState;
        private System.Windows.Forms.Label lblQuoteDriverState;
        private System.Windows.Forms.Label lblOrderDriverState;
        private System.Windows.Forms.PictureBox pbxOrderDriverState;
    }
}
