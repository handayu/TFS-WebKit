namespace UseOnlineTradingSystem
{
    partial class PublishControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label_publish = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.supplyDemandPubishControl1 = new UseOnlineTradingSystem.SupplyDemandPubishControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tradingInfoControl1 = new UseOnlineTradingSystem.TradingInfoControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkGray;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label_publish);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1412, 49);
            this.panel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.DarkGray;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox1.Image = global::UseOnlineTradingSystem.Properties.Resources.icon_close;
            this.pictureBox1.Location = new System.Drawing.Point(1384, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 49);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.Picture_Click);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.Picture_MouseMoveLeave);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Picture_MouseMoveIn);
            // 
            // label_publish
            // 
            this.label_publish.AutoSize = true;
            this.label_publish.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_publish.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label_publish.Location = new System.Drawing.Point(9, 10);
            this.label_publish.Name = "label_publish";
            this.label_publish.Size = new System.Drawing.Size(96, 28);
            this.label_publish.TabIndex = 0;
            this.label_publish.Text = "供需发布";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.supplyDemandPubishControl1);
            this.panel2.Location = new System.Drawing.Point(0, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(900, 451);
            this.panel2.TabIndex = 1;
            // 
            // supplyDemandPubishControl1
            // 
            this.supplyDemandPubishControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.supplyDemandPubishControl1.Location = new System.Drawing.Point(0, 0);
            this.supplyDemandPubishControl1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.supplyDemandPubishControl1.Name = "supplyDemandPubishControl1";
            this.supplyDemandPubishControl1.Size = new System.Drawing.Size(900, 451);
            this.supplyDemandPubishControl1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tradingInfoControl1);
            this.panel3.Location = new System.Drawing.Point(860, 40);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1410, 555);
            this.panel3.TabIndex = 1;
            // 
            // tradingInfoControl1
            // 
            this.tradingInfoControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tradingInfoControl1.Location = new System.Drawing.Point(0, 0);
            this.tradingInfoControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tradingInfoControl1.Name = "tradingInfoControl1";
            this.tradingInfoControl1.Size = new System.Drawing.Size(1410, 555);
            this.tradingInfoControl1.TabIndex = 0;
            // 
            // PublishControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PublishControl";
            this.SizeChanged += PublishControl_SizeChanged;
            this.Size = new System.Drawing.Size(1412, 772);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_publish;
        private SupplyDemandPubishControl supplyDemandPubishControl1;
        private TradingInfoControl tradingInfoControl1;
        //private System.Windows.Forms.Splitter splitter1;
        //private System.Windows.Forms.Splitter splitter2;
        //private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}
