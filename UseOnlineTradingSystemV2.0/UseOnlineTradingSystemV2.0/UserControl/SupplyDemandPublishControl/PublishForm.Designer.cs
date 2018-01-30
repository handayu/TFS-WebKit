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
            this.tradingInfoControl2 = new UseOnlineTradingSystem.TradingInfoControl();
            this.supplyDemandPubishControl2 = new UseOnlineTradingSystem.SupplyDemandPubishControl();
            this.SuspendLayout();
            // 
            // tradingInfoControl2
            // 
            this.tradingInfoControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.tradingInfoControl2.Location = new System.Drawing.Point(923, 0);
            this.tradingInfoControl2.Name = "tradingInfoControl2";
            this.tradingInfoControl2.Size = new System.Drawing.Size(865, 534);
            this.tradingInfoControl2.TabIndex = 0;
            // 
            // supplyDemandPubishControl2
            // 
            this.supplyDemandPubishControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.supplyDemandPubishControl2.Location = new System.Drawing.Point(0, 0);
            this.supplyDemandPubishControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.supplyDemandPubishControl2.Name = "supplyDemandPubishControl2";
            this.supplyDemandPubishControl2.Size = new System.Drawing.Size(923, 534);
            this.supplyDemandPubishControl2.TabIndex = 0;
            this.supplyDemandPubishControl2.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            // 
            // PublishForm
            // 
            this.ClientSize = new System.Drawing.Size(1788, 534);
            this.Controls.Add(this.tradingInfoControl2);
            this.Controls.Add(this.supplyDemandPubishControl2);
            this.Name = "PublishForm";
            this.Text = "供需发布";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private TradingInfoControl tradingInfoControl2;
        private SupplyDemandPubishControl supplyDemandPubishControl2;
    }
}