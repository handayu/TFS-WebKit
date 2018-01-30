namespace UseOnlineTradingSystem
{
    partial class PublishForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tradingInfoControl1 = new UseOnlineTradingSystem.TradingInfoControl();
            this.supplyDemandPubishControl1 = new UseOnlineTradingSystem.SupplyDemandPubishControl();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.supplyDemandPubishControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(922, 425);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.tradingInfoControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(977, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1428, 425);
            this.panel2.TabIndex = 2;
            // 
            // tradingInfoControl1
            // 
            this.tradingInfoControl1.Location = new System.Drawing.Point(-36, 0);
            this.tradingInfoControl1.Name = "tradingInfoControl1";
            this.tradingInfoControl1.Size = new System.Drawing.Size(1461, 388);
            this.tradingInfoControl1.TabIndex = 0;
            // 
            // supplyDemandPubishControl1
            // 
            this.supplyDemandPubishControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.supplyDemandPubishControl1.Location = new System.Drawing.Point(0, 0);
            this.supplyDemandPubishControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.supplyDemandPubishControl1.Name = "supplyDemandPubishControl1";
            this.supplyDemandPubishControl1.Size = new System.Drawing.Size(922, 425);
            this.supplyDemandPubishControl1.TabIndex = 0;
            // 
            // PublishForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(2405, 425);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "PublishForm";
            this.Text = "供需发布";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SupplyDemandPubishControl supplyDemandPubishControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private TradingInfoControl tradingInfoControl1;
    }
}