namespace USeFuturesSpirit
{
    partial class SelectProductForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.selectProductControl1 = new USeFuturesSpirit.SelectProductControl();
            this.SuspendLayout();
            // 
            // selectProductControl1
            // 
            this.selectProductControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectProductControl1.Location = new System.Drawing.Point(0, 0);
            this.selectProductControl1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.selectProductControl1.Name = "selectProductControl1";
            this.selectProductControl1.SelectedProduct = null;
            this.selectProductControl1.Size = new System.Drawing.Size(440, 253);
            this.selectProductControl1.TabIndex = 0;
            // 
            // SelectProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 253);
            this.Controls.Add(this.selectProductControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectProductForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "产品选择";
            this.Load += new System.EventHandler(this.SelectProductForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SelectProductControl selectProductControl1;
    }
}