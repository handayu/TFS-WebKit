namespace USeFuturesSpirit
{
    partial class OrderPriceTypeControl
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
            this.btnLastPrice = new System.Windows.Forms.Button();
            this.btnOpponentPrice = new System.Windows.Forms.Button();
            this.btnQueuePrice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLastPrice
            // 
            this.btnLastPrice.BackColor = System.Drawing.SystemColors.Control;
            this.btnLastPrice.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLastPrice.Location = new System.Drawing.Point(1, 1);
            this.btnLastPrice.Name = "btnLastPrice";
            this.btnLastPrice.Size = new System.Drawing.Size(85, 30);
            this.btnLastPrice.TabIndex = 0;
            this.btnLastPrice.Text = "最新价";
            this.btnLastPrice.UseVisualStyleBackColor = false;
            this.btnLastPrice.Click += new System.EventHandler(this.btnLastPrice_Click);
            // 
            // btnOpponentPrice
            // 
            this.btnOpponentPrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnOpponentPrice.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOpponentPrice.Location = new System.Drawing.Point(88, 1);
            this.btnOpponentPrice.Name = "btnOpponentPrice";
            this.btnOpponentPrice.Size = new System.Drawing.Size(85, 30);
            this.btnOpponentPrice.TabIndex = 1;
            this.btnOpponentPrice.Text = "对手价";
            this.btnOpponentPrice.UseVisualStyleBackColor = false;
            this.btnOpponentPrice.Click += new System.EventHandler(this.btnOpponentPrice_Click);
            // 
            // btnQueuePrice
            // 
            this.btnQueuePrice.BackColor = System.Drawing.SystemColors.Control;
            this.btnQueuePrice.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnQueuePrice.Location = new System.Drawing.Point(175, 1);
            this.btnQueuePrice.Name = "btnQueuePrice";
            this.btnQueuePrice.Size = new System.Drawing.Size(85, 30);
            this.btnQueuePrice.TabIndex = 2;
            this.btnQueuePrice.Text = "排队价";
            this.btnQueuePrice.UseVisualStyleBackColor = false;
            this.btnQueuePrice.Click += new System.EventHandler(this.btnQueuePrice_Click);
            // 
            // OrderPriceTypeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnQueuePrice);
            this.Controls.Add(this.btnOpponentPrice);
            this.Controls.Add(this.btnLastPrice);
            this.Name = "OrderPriceTypeControl";
            this.Size = new System.Drawing.Size(270, 31);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLastPrice;
        private System.Windows.Forms.Button btnOpponentPrice;
        private System.Windows.Forms.Button btnQueuePrice;
    }
}
