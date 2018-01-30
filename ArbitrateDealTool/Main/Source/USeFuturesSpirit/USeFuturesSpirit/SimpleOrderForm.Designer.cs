namespace USeFuturesSpirit
{
    partial class SimpleOrderForm
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
            this.simpleOrderPanelControl1 = new USeFuturesSpirit.SimpleOrderPanelControl();
            this.SuspendLayout();
            // 
            // simpleOrderPanelControl1
            // 
            this.simpleOrderPanelControl1.Location = new System.Drawing.Point(9, 10);
            this.simpleOrderPanelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleOrderPanelControl1.Name = "simpleOrderPanelControl1";
            this.simpleOrderPanelControl1.Size = new System.Drawing.Size(214, 202);
            this.simpleOrderPanelControl1.TabIndex = 0;
            // 
            // SimpleOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 226);
            this.Controls.Add(this.simpleOrderPanelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "SimpleOrderForm";
            this.Text = "下单板";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SimpleOrderForm_FormClosing);
            this.Load += new System.EventHandler(this.SimpleOrderForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SimpleOrderPanelControl simpleOrderPanelControl1;
    }
}