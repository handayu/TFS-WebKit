namespace USeFuturesSpirit
{
    partial class SimpleOrderPanelControl
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
            if (disposing && (components != null)) {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Label_CanVolumn = new System.Windows.Forms.Label();
            this.buttonOrder = new System.Windows.Forms.Button();
            this.labelUpper = new System.Windows.Forms.Label();
            this.labelLower = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBoxInstrument = new System.Windows.Forms.ComboBox();
            this.numericUpDownVolume = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownPrice = new System.Windows.Forms.NumericUpDown();
            this.buttonPrice = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_Buy = new System.Windows.Forms.RadioButton();
            this.radioButton_Sell = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton_CloseYD = new System.Windows.Forms.RadioButton();
            this.radioButton_Open = new System.Windows.Forms.RadioButton();
            this.radioButton_CloseToday = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrice)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Label_CanVolumn);
            this.groupBox1.Controls.Add(this.buttonOrder);
            this.groupBox1.Controls.Add(this.labelUpper);
            this.groupBox1.Controls.Add(this.labelLower);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.comboBoxInstrument);
            this.groupBox1.Controls.Add(this.numericUpDownVolume);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDownPrice);
            this.groupBox1.Controls.Add(this.buttonPrice);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 249);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "下单板";
            // 
            // Label_CanVolumn
            // 
            this.Label_CanVolumn.AutoSize = true;
            this.Label_CanVolumn.ForeColor = System.Drawing.Color.LightCoral;
            this.Label_CanVolumn.Location = new System.Drawing.Point(213, 173);
            this.Label_CanVolumn.Name = "Label_CanVolumn";
            this.Label_CanVolumn.Size = new System.Drawing.Size(64, 17);
            this.Label_CanVolumn.TabIndex = 41;
            this.Label_CanVolumn.Text = "最大开仓";
            // 
            // buttonOrder
            // 
            this.buttonOrder.Location = new System.Drawing.Point(16, 204);
            this.buttonOrder.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOrder.Name = "buttonOrder";
            this.buttonOrder.Size = new System.Drawing.Size(254, 37);
            this.buttonOrder.TabIndex = 38;
            this.buttonOrder.Text = "下 单";
            this.buttonOrder.UseVisualStyleBackColor = true;
            this.buttonOrder.Click += new System.EventHandler(this.buttonOrder_Click);
            // 
            // labelUpper
            // 
            this.labelUpper.AutoSize = true;
            this.labelUpper.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUpper.ForeColor = System.Drawing.Color.Red;
            this.labelUpper.Location = new System.Drawing.Point(208, 144);
            this.labelUpper.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUpper.Name = "labelUpper";
            this.labelUpper.Size = new System.Drawing.Size(13, 18);
            this.labelUpper.TabIndex = 36;
            this.labelUpper.Text = "-";
            // 
            // labelLower
            // 
            this.labelLower.AutoSize = true;
            this.labelLower.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLower.ForeColor = System.Drawing.Color.Green;
            this.labelLower.Location = new System.Drawing.Point(208, 128);
            this.labelLower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLower.Name = "labelLower";
            this.labelLower.Size = new System.Drawing.Size(13, 18);
            this.labelLower.TabIndex = 35;
            this.labelLower.Text = "-";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(199, 174);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(16, 17);
            this.label15.TabIndex = 34;
            this.label15.Text = "≤";
            // 
            // comboBoxInstrument
            // 
            this.comboBoxInstrument.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxInstrument.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.comboBoxInstrument.Font = new System.Drawing.Font("NSimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxInstrument.FormattingEnabled = true;
            this.comboBoxInstrument.Location = new System.Drawing.Point(61, 24);
            this.comboBoxInstrument.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxInstrument.Name = "comboBoxInstrument";
            this.comboBoxInstrument.Size = new System.Drawing.Size(178, 23);
            this.comboBoxInstrument.TabIndex = 22;
            this.comboBoxInstrument.SelectedIndexChanged += new System.EventHandler(this.SelectIndexChanged);
            // 
            // numericUpDownVolume
            // 
            this.numericUpDownVolume.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDownVolume.Location = new System.Drawing.Point(97, 165);
            this.numericUpDownVolume.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownVolume.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownVolume.Name = "numericUpDownVolume";
            this.numericUpDownVolume.Size = new System.Drawing.Size(103, 31);
            this.numericUpDownVolume.TabIndex = 28;
            this.numericUpDownVolume.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(-1, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 40);
            this.label1.TabIndex = 21;
            this.label1.Text = "合约";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownPrice
            // 
            this.numericUpDownPrice.Enabled = false;
            this.numericUpDownPrice.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDownPrice.Location = new System.Drawing.Point(96, 129);
            this.numericUpDownPrice.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownPrice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownPrice.Name = "numericUpDownPrice";
            this.numericUpDownPrice.Size = new System.Drawing.Size(103, 31);
            this.numericUpDownPrice.TabIndex = 27;
            // 
            // buttonPrice
            // 
            this.buttonPrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.buttonPrice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPrice.Font = new System.Drawing.Font("NSimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonPrice.Location = new System.Drawing.Point(20, 128);
            this.buttonPrice.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPrice.Name = "buttonPrice";
            this.buttonPrice.Size = new System.Drawing.Size(73, 36);
            this.buttonPrice.TabIndex = 26;
            this.buttonPrice.Text = "跟盘价";
            this.buttonPrice.UseVisualStyleBackColor = false;
            this.buttonPrice.Click += new System.EventHandler(this.buttonPrice_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(4, 50);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 36);
            this.label3.TabIndex = 24;
            this.label3.Text = "买卖";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(6, 167);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 25);
            this.label5.TabIndex = 23;
            this.label5.Text = "手 数";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(-35, 80);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 43);
            this.label4.TabIndex = 25;
            this.label4.Text = "开平";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_Buy);
            this.groupBox2.Controls.Add(this.radioButton_Sell);
            this.groupBox2.Location = new System.Drawing.Point(61, 41);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(113, 41);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            // 
            // radioButton_Buy
            // 
            this.radioButton_Buy.AutoSize = true;
            this.radioButton_Buy.Location = new System.Drawing.Point(6, 15);
            this.radioButton_Buy.Name = "radioButton_Buy";
            this.radioButton_Buy.Size = new System.Drawing.Size(43, 21);
            this.radioButton_Buy.TabIndex = 29;
            this.radioButton_Buy.Text = "买";
            this.radioButton_Buy.UseVisualStyleBackColor = true;
            // 
            // radioButton_Sell
            // 
            this.radioButton_Sell.AutoSize = true;
            this.radioButton_Sell.Location = new System.Drawing.Point(61, 15);
            this.radioButton_Sell.Name = "radioButton_Sell";
            this.radioButton_Sell.Size = new System.Drawing.Size(43, 21);
            this.radioButton_Sell.TabIndex = 30;
            this.radioButton_Sell.Text = "卖";
            this.radioButton_Sell.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton_CloseYD);
            this.groupBox3.Controls.Add(this.radioButton_Open);
            this.groupBox3.Controls.Add(this.radioButton_CloseToday);
            this.groupBox3.Location = new System.Drawing.Point(62, 80);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(201, 41);
            this.groupBox3.TabIndex = 40;
            this.groupBox3.TabStop = false;
            // 
            // radioButton_CloseYD
            // 
            this.radioButton_CloseYD.AutoSize = true;
            this.radioButton_CloseYD.Location = new System.Drawing.Point(135, 15);
            this.radioButton_CloseYD.Name = "radioButton_CloseYD";
            this.radioButton_CloseYD.Size = new System.Drawing.Size(57, 21);
            this.radioButton_CloseYD.TabIndex = 33;
            this.radioButton_CloseYD.Text = "平仓";
            this.radioButton_CloseYD.UseVisualStyleBackColor = true;
            // 
            // radioButton_Open
            // 
            this.radioButton_Open.AutoSize = true;
            this.radioButton_Open.Checked = true;
            this.radioButton_Open.Location = new System.Drawing.Point(6, 15);
            this.radioButton_Open.Name = "radioButton_Open";
            this.radioButton_Open.Size = new System.Drawing.Size(57, 21);
            this.radioButton_Open.TabIndex = 31;
            this.radioButton_Open.TabStop = true;
            this.radioButton_Open.Text = "开仓";
            this.radioButton_Open.UseVisualStyleBackColor = true;
            // 
            // radioButton_CloseToday
            // 
            this.radioButton_CloseToday.AutoSize = true;
            this.radioButton_CloseToday.Location = new System.Drawing.Point(70, 15);
            this.radioButton_CloseToday.Name = "radioButton_CloseToday";
            this.radioButton_CloseToday.Size = new System.Drawing.Size(57, 21);
            this.radioButton_CloseToday.TabIndex = 32;
            this.radioButton_CloseToday.Text = "平今";
            this.radioButton_CloseToday.UseVisualStyleBackColor = true;
            // 
            // SimpleOrderPanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "SimpleOrderPanelControl";
            this.Size = new System.Drawing.Size(286, 249);
            this.Load += new System.EventHandler(this.SimpleOrderPanelControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrice)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_CloseYD;
        private System.Windows.Forms.RadioButton radioButton_CloseToday;
        private System.Windows.Forms.RadioButton radioButton_Open;
        private System.Windows.Forms.RadioButton radioButton_Sell;
        private System.Windows.Forms.RadioButton radioButton_Buy;
        private System.Windows.Forms.ComboBox comboBoxInstrument;
        private System.Windows.Forms.NumericUpDown numericUpDownVolume;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownPrice;
        private System.Windows.Forms.Button buttonPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelUpper;
        private System.Windows.Forms.Label labelLower;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button buttonOrder;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label Label_CanVolumn;
    }
}
