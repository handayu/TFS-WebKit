namespace USeFuturesSpirit
{
    partial class ArbitrageOperationSideControl
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
            this.btnSellNearBuyFar = new System.Windows.Forms.Button();
            this.btnBuyNearSellFar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSellNearBuyFar
            // 
            this.btnSellNearBuyFar.BackColor = System.Drawing.SystemColors.Control;
            this.btnSellNearBuyFar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSellNearBuyFar.Location = new System.Drawing.Point(1, 1);
            this.btnSellNearBuyFar.Margin = new System.Windows.Forms.Padding(2);
            this.btnSellNearBuyFar.Name = "btnSellNearBuyFar";
            this.btnSellNearBuyFar.Size = new System.Drawing.Size(64, 25);
            this.btnSellNearBuyFar.TabIndex = 0;
            this.btnSellNearBuyFar.Text = "卖近买远";
            this.btnSellNearBuyFar.UseVisualStyleBackColor = false;
            this.btnSellNearBuyFar.Click += new System.EventHandler(this.btnSellNearBuyFar_Click);
            // 
            // btnBuyNearSellFar
            // 
            this.btnBuyNearSellFar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnBuyNearSellFar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBuyNearSellFar.Location = new System.Drawing.Point(66, 1);
            this.btnBuyNearSellFar.Margin = new System.Windows.Forms.Padding(2);
            this.btnBuyNearSellFar.Name = "btnBuyNearSellFar";
            this.btnBuyNearSellFar.Size = new System.Drawing.Size(64, 25);
            this.btnBuyNearSellFar.TabIndex = 1;
            this.btnBuyNearSellFar.Text = "买近卖远";
            this.btnBuyNearSellFar.UseVisualStyleBackColor = false;
            this.btnBuyNearSellFar.Click += new System.EventHandler(this.btnBuyNearSellFar_Click);
            // 
            // ArbitrageOperationSideControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBuyNearSellFar);
            this.Controls.Add(this.btnSellNearBuyFar);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ArbitrageOperationSideControl";
            this.Size = new System.Drawing.Size(168, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSellNearBuyFar;
        private System.Windows.Forms.Button btnBuyNearSellFar;
    }
}
