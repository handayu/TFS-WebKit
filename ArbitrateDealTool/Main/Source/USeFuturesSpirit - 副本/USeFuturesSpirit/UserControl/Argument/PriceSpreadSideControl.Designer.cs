namespace USeFuturesSpirit
{
    partial class PriceSpreadSideControl
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
            this.btnGreaterOrEqual = new System.Windows.Forms.Button();
            this.btnLessOrEqual = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGreaterOrEqual
            // 
            this.btnGreaterOrEqual.BackColor = System.Drawing.SystemColors.Control;
            this.btnGreaterOrEqual.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGreaterOrEqual.Location = new System.Drawing.Point(1, 1);
            this.btnGreaterOrEqual.Name = "btnGreaterOrEqual";
            this.btnGreaterOrEqual.Size = new System.Drawing.Size(85, 30);
            this.btnGreaterOrEqual.TabIndex = 0;
            this.btnGreaterOrEqual.Text = "大于等于";
            this.btnGreaterOrEqual.UseVisualStyleBackColor = false;
            this.btnGreaterOrEqual.Click += new System.EventHandler(this.btnGreaterOrEqual_Click);
            // 
            // btnLessOrEqual
            // 
            this.btnLessOrEqual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnLessOrEqual.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLessOrEqual.Location = new System.Drawing.Point(88, 1);
            this.btnLessOrEqual.Name = "btnLessOrEqual";
            this.btnLessOrEqual.Size = new System.Drawing.Size(85, 30);
            this.btnLessOrEqual.TabIndex = 1;
            this.btnLessOrEqual.Text = "小于等于";
            this.btnLessOrEqual.UseVisualStyleBackColor = false;
            this.btnLessOrEqual.Click += new System.EventHandler(this.btnLessOrEqual_Click);
            // 
            // PriceSpreadSideControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLessOrEqual);
            this.Controls.Add(this.btnGreaterOrEqual);
            this.Name = "PriceSpreadSideControl";
            this.Size = new System.Drawing.Size(178, 31);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGreaterOrEqual;
        private System.Windows.Forms.Button btnLessOrEqual;
    }
}
