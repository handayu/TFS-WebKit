namespace USeFuturesSpirit
{
    partial class UseSystemConfigForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOrderMargin_MaxUseRate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtTaskOrder_TryOrderMinInterval = new System.Windows.Forms.TextBox();
            this.txtTaskOrder_TaskMaxTryCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbnMarketPriceMethod_ThreePriceTick = new System.Windows.Forms.RadioButton();
            this.rbnMarketPriceMethod_TwoPriceTick = new System.Windows.Forms.RadioButton();
            this.rbnMarketPriceMethod_OnePriceTick = new System.Windows.Forms.RadioButton();
            this.rbnMarketPriceMethod_OpponentPrice = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 356);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(668, 46);
            this.panel1.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(347, 5);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 32);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(228, 5);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 32);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(668, 356);
            this.panel2.TabIndex = 3;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtOrderMargin_MaxUseRate);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(16, 128);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(637, 54);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "保证金";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(307, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(336, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "范围0~100";
            // 
            // txtOrderMargin_MaxUseRate
            // 
            this.txtOrderMargin_MaxUseRate.Location = new System.Drawing.Point(212, 21);
            this.txtOrderMargin_MaxUseRate.Name = "txtOrderMargin_MaxUseRate";
            this.txtOrderMargin_MaxUseRate.Size = new System.Drawing.Size(85, 22);
            this.txtOrderMargin_MaxUseRate.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(190, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "套利单保证金占用最大比例：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtTaskOrder_TryOrderMinInterval);
            this.groupBox3.Controls.Add(this.txtTaskOrder_TaskMaxTryCount);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(16, 66);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(637, 54);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "下单设定";
            // 
            // txtTaskOrder_TryOrderMinInterval
            // 
            this.txtTaskOrder_TryOrderMinInterval.Location = new System.Drawing.Point(421, 20);
            this.txtTaskOrder_TryOrderMinInterval.Name = "txtTaskOrder_TryOrderMinInterval";
            this.txtTaskOrder_TryOrderMinInterval.Size = new System.Drawing.Size(100, 22);
            this.txtTaskOrder_TryOrderMinInterval.TabIndex = 3;
            // 
            // txtTaskOrder_TaskMaxTryCount
            // 
            this.txtTaskOrder_TaskMaxTryCount.Location = new System.Drawing.Point(126, 20);
            this.txtTaskOrder_TaskMaxTryCount.Name = "txtTaskOrder_TaskMaxTryCount";
            this.txtTaskOrder_TaskMaxTryCount.Size = new System.Drawing.Size(100, 22);
            this.txtTaskOrder_TaskMaxTryCount.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(256, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(172, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "两次下单最小间隔(毫秒)：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "最大尝试次数：";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(16, 188);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(637, 117);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "监控预警";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(16, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(637, 54);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "市价定义";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rbnMarketPriceMethod_ThreePriceTick);
            this.panel3.Controls.Add(this.rbnMarketPriceMethod_TwoPriceTick);
            this.panel3.Controls.Add(this.rbnMarketPriceMethod_OnePriceTick);
            this.panel3.Controls.Add(this.rbnMarketPriceMethod_OpponentPrice);
            this.panel3.Location = new System.Drawing.Point(94, 20);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(523, 27);
            this.panel3.TabIndex = 1;
            // 
            // rbnMarketPriceMethod_ThreePriceTick
            // 
            this.rbnMarketPriceMethod_ThreePriceTick.AutoSize = true;
            this.rbnMarketPriceMethod_ThreePriceTick.Location = new System.Drawing.Point(316, 4);
            this.rbnMarketPriceMethod_ThreePriceTick.Name = "rbnMarketPriceMethod_ThreePriceTick";
            this.rbnMarketPriceMethod_ThreePriceTick.Size = new System.Drawing.Size(107, 21);
            this.rbnMarketPriceMethod_ThreePriceTick.TabIndex = 3;
            this.rbnMarketPriceMethod_ThreePriceTick.Text = "3个变动价位";
            this.rbnMarketPriceMethod_ThreePriceTick.UseVisualStyleBackColor = true;
            // 
            // rbnMarketPriceMethod_TwoPriceTick
            // 
            this.rbnMarketPriceMethod_TwoPriceTick.AutoSize = true;
            this.rbnMarketPriceMethod_TwoPriceTick.Location = new System.Drawing.Point(201, 4);
            this.rbnMarketPriceMethod_TwoPriceTick.Name = "rbnMarketPriceMethod_TwoPriceTick";
            this.rbnMarketPriceMethod_TwoPriceTick.Size = new System.Drawing.Size(107, 21);
            this.rbnMarketPriceMethod_TwoPriceTick.TabIndex = 2;
            this.rbnMarketPriceMethod_TwoPriceTick.Text = "2个变动价位";
            this.rbnMarketPriceMethod_TwoPriceTick.UseVisualStyleBackColor = true;
            // 
            // rbnMarketPriceMethod_OnePriceTick
            // 
            this.rbnMarketPriceMethod_OnePriceTick.AutoSize = true;
            this.rbnMarketPriceMethod_OnePriceTick.Location = new System.Drawing.Point(88, 4);
            this.rbnMarketPriceMethod_OnePriceTick.Name = "rbnMarketPriceMethod_OnePriceTick";
            this.rbnMarketPriceMethod_OnePriceTick.Size = new System.Drawing.Size(107, 21);
            this.rbnMarketPriceMethod_OnePriceTick.TabIndex = 1;
            this.rbnMarketPriceMethod_OnePriceTick.Text = "1个变动价位";
            this.rbnMarketPriceMethod_OnePriceTick.UseVisualStyleBackColor = true;
            // 
            // rbnMarketPriceMethod_OpponentPrice
            // 
            this.rbnMarketPriceMethod_OpponentPrice.AutoSize = true;
            this.rbnMarketPriceMethod_OpponentPrice.Checked = true;
            this.rbnMarketPriceMethod_OpponentPrice.Location = new System.Drawing.Point(12, 4);
            this.rbnMarketPriceMethod_OpponentPrice.Name = "rbnMarketPriceMethod_OpponentPrice";
            this.rbnMarketPriceMethod_OpponentPrice.Size = new System.Drawing.Size(71, 21);
            this.rbnMarketPriceMethod_OpponentPrice.TabIndex = 0;
            this.rbnMarketPriceMethod_OpponentPrice.TabStop = true;
            this.rbnMarketPriceMethod_OpponentPrice.Text = "对手价";
            this.rbnMarketPriceMethod_OpponentPrice.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "市价定义：";
            // 
            // UseSystemConfigForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(668, 402);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "UseSystemConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统参数配置";
            this.Load += new System.EventHandler(this.UseSystemConfigForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rbnMarketPriceMethod_ThreePriceTick;
        private System.Windows.Forms.RadioButton rbnMarketPriceMethod_TwoPriceTick;
        private System.Windows.Forms.RadioButton rbnMarketPriceMethod_OnePriceTick;
        private System.Windows.Forms.RadioButton rbnMarketPriceMethod_OpponentPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTaskOrder_TryOrderMinInterval;
        private System.Windows.Forms.TextBox txtTaskOrder_TaskMaxTryCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtOrderMargin_MaxUseRate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
    }
}