namespace USeFuturesSpirit
{
    partial class ArbitrageOrderViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArbitrageOrderViewForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpOpen = new System.Windows.Forms.TabPage();
            this.openTaskGroupView = new USeFuturesSpirit.TaskGroupViewControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.arbitrageOpenArgumentView2Control1 = new USeFuturesSpirit.ArbitrageOpenArgumentView2Control();
            this.tpClose = new System.Windows.Forms.TabPage();
            this.closeTaskGroupView = new USeFuturesSpirit.TaskGroupViewControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.arbitrageCloseArgumentView2Control1 = new USeFuturesSpirit.ArbitrageCloseArgumentView2Control();
            this.tabControl1.SuspendLayout();
            this.tpOpen.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tpClose.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpOpen);
            this.tabControl1.Controls.Add(this.tpClose);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1305, 524);
            this.tabControl1.TabIndex = 2;
            // 
            // tpOpen
            // 
            this.tpOpen.Controls.Add(this.openTaskGroupView);
            this.tpOpen.Controls.Add(this.panel2);
            this.tpOpen.Location = new System.Drawing.Point(4, 25);
            this.tpOpen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tpOpen.Name = "tpOpen";
            this.tpOpen.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tpOpen.Size = new System.Drawing.Size(1297, 495);
            this.tpOpen.TabIndex = 0;
            this.tpOpen.Text = "开仓";
            this.tpOpen.UseVisualStyleBackColor = true;
            // 
            // openTaskGroupView
            // 
            this.openTaskGroupView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openTaskGroupView.Location = new System.Drawing.Point(3, 94);
            this.openTaskGroupView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.openTaskGroupView.Name = "openTaskGroupView";
            this.openTaskGroupView.Size = new System.Drawing.Size(1291, 399);
            this.openTaskGroupView.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.arbitrageOpenArgumentView2Control1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1291, 92);
            this.panel2.TabIndex = 1;
            // 
            // arbitrageOpenArgumentView2Control1
            // 
            this.arbitrageOpenArgumentView2Control1.BackColor = System.Drawing.SystemColors.Control;
            this.arbitrageOpenArgumentView2Control1.Location = new System.Drawing.Point(6, 2);
            this.arbitrageOpenArgumentView2Control1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.arbitrageOpenArgumentView2Control1.Name = "arbitrageOpenArgumentView2Control1";
            this.arbitrageOpenArgumentView2Control1.Size = new System.Drawing.Size(818, 90);
            this.arbitrageOpenArgumentView2Control1.TabIndex = 0;
            // 
            // tpClose
            // 
            this.tpClose.Controls.Add(this.closeTaskGroupView);
            this.tpClose.Controls.Add(this.panel3);
            this.tpClose.Location = new System.Drawing.Point(4, 25);
            this.tpClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tpClose.Name = "tpClose";
            this.tpClose.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tpClose.Size = new System.Drawing.Size(1297, 495);
            this.tpClose.TabIndex = 1;
            this.tpClose.Text = "平仓";
            this.tpClose.UseVisualStyleBackColor = true;
            // 
            // closeTaskGroupView
            // 
            this.closeTaskGroupView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.closeTaskGroupView.Location = new System.Drawing.Point(3, 92);
            this.closeTaskGroupView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.closeTaskGroupView.Name = "closeTaskGroupView";
            this.closeTaskGroupView.Size = new System.Drawing.Size(1291, 401);
            this.closeTaskGroupView.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.arbitrageCloseArgumentView2Control1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 2);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1291, 90);
            this.panel3.TabIndex = 2;
            // 
            // arbitrageCloseArgumentView2Control1
            // 
            this.arbitrageCloseArgumentView2Control1.BackColor = System.Drawing.SystemColors.Control;
            this.arbitrageCloseArgumentView2Control1.Location = new System.Drawing.Point(7, 1);
            this.arbitrageCloseArgumentView2Control1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.arbitrageCloseArgumentView2Control1.Name = "arbitrageCloseArgumentView2Control1";
            this.arbitrageCloseArgumentView2Control1.Size = new System.Drawing.Size(658, 85);
            this.arbitrageCloseArgumentView2Control1.TabIndex = 0;
            // 
            // ArbitrageOrderViewForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1305, 524);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ArbitrageOrderViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "套利单详情";
            this.Load += new System.EventHandler(this.ArbitrageOrderViewForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpOpen.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tpClose.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpOpen;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabPage tpClose;
        private System.Windows.Forms.Panel panel3;
        private TaskGroupViewControl openTaskGroupView;
        private TaskGroupViewControl closeTaskGroupView;
        private ArbitrageOpenArgumentView2Control arbitrageOpenArgumentView2Control1;
        private ArbitrageCloseArgumentView2Control arbitrageCloseArgumentView2Control1;
    }
}