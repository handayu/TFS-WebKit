namespace USeFuturesSpirit
{
    partial class ArbitrageOpenArgumentViewControl
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.rbnPriceSpreadSide_Less = new System.Windows.Forms.RadioButton();
            this.rbnPriceSpreadSide_Greater = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbnPreferentialSide_Buy = new System.Windows.Forms.RadioButton();
            this.rbnPreferentialSide_Sell = new System.Windows.Forms.RadioButton();
            this.nudDifferentialUnit = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudOrderQtyUint = new System.Windows.Forms.NumericUpDown();
            this.nudTotalOrderQty = new System.Windows.Forms.NumericUpDown();
            this.nudPriceSpreadThreshold = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbnSellOrderPriceType_OpponentPrice = new System.Windows.Forms.RadioButton();
            this.rbnSellOrderPriceType_LastPrice = new System.Windows.Forms.RadioButton();
            this.rbnSellOrderPriceType_QueuePrice = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbnBuyOrderPriceType_OpponentPrice = new System.Windows.Forms.RadioButton();
            this.rbnBuyOrderPriceType_LastPrice = new System.Windows.Forms.RadioButton();
            this.rbnBuyOrderPriceType_QueuePrice = new System.Windows.Forms.RadioButton();
            this.label59 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.cbxSellInstrument = new System.Windows.Forms.ComboBox();
            this.cbxBuyInstrument = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDifferentialUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOrderQtyUint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotalOrderQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPriceSpreadThreshold)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.nudDifferentialUnit);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nudOrderQtyUint);
            this.groupBox1.Controls.Add(this.nudTotalOrderQty);
            this.groupBox1.Controls.Add(this.nudPriceSpreadThreshold);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.label59);
            this.groupBox1.Controls.Add(this.label58);
            this.groupBox1.Controls.Add(this.cbxSellInstrument);
            this.groupBox1.Controls.Add(this.cbxBuyInstrument);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(389, 133);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "开仓参数";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rbnPriceSpreadSide_Less);
            this.panel4.Controls.Add(this.rbnPriceSpreadSide_Greater);
            this.panel4.Location = new System.Drawing.Point(72, 68);
            this.panel4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(169, 24);
            this.panel4.TabIndex = 53;
            // 
            // rbnPriceSpreadSide_Less
            // 
            this.rbnPriceSpreadSide_Less.AutoSize = true;
            this.rbnPriceSpreadSide_Less.Location = new System.Drawing.Point(85, 4);
            this.rbnPriceSpreadSide_Less.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbnPriceSpreadSide_Less.Name = "rbnPriceSpreadSide_Less";
            this.rbnPriceSpreadSide_Less.Size = new System.Drawing.Size(73, 17);
            this.rbnPriceSpreadSide_Less.TabIndex = 45;
            this.rbnPriceSpreadSide_Less.Text = "小于等于";
            this.rbnPriceSpreadSide_Less.UseVisualStyleBackColor = true;
            this.rbnPriceSpreadSide_Less.CheckedChanged += new System.EventHandler(this.rbnPriceSpreadSide_CheckedChanged);
            // 
            // rbnPriceSpreadSide_Greater
            // 
            this.rbnPriceSpreadSide_Greater.AutoSize = true;
            this.rbnPriceSpreadSide_Greater.Checked = true;
            this.rbnPriceSpreadSide_Greater.Location = new System.Drawing.Point(8, 4);
            this.rbnPriceSpreadSide_Greater.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbnPriceSpreadSide_Greater.Name = "rbnPriceSpreadSide_Greater";
            this.rbnPriceSpreadSide_Greater.Size = new System.Drawing.Size(73, 17);
            this.rbnPriceSpreadSide_Greater.TabIndex = 44;
            this.rbnPriceSpreadSide_Greater.TabStop = true;
            this.rbnPriceSpreadSide_Greater.Text = "大于等于";
            this.rbnPriceSpreadSide_Greater.UseVisualStyleBackColor = true;
            this.rbnPriceSpreadSide_Greater.CheckedChanged += new System.EventHandler(this.rbnPriceSpreadSide_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rbnPreferentialSide_Buy);
            this.panel3.Controls.Add(this.rbnPreferentialSide_Sell);
            this.panel3.Location = new System.Drawing.Point(4, 20);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(74, 45);
            this.panel3.TabIndex = 52;
            // 
            // rbnPreferentialSide_Buy
            // 
            this.rbnPreferentialSide_Buy.AutoSize = true;
            this.rbnPreferentialSide_Buy.BackColor = System.Drawing.Color.RoyalBlue;
            this.rbnPreferentialSide_Buy.Checked = true;
            this.rbnPreferentialSide_Buy.Location = new System.Drawing.Point(2, 2);
            this.rbnPreferentialSide_Buy.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbnPreferentialSide_Buy.Name = "rbnPreferentialSide_Buy";
            this.rbnPreferentialSide_Buy.Size = new System.Drawing.Size(73, 17);
            this.rbnPreferentialSide_Buy.TabIndex = 50;
            this.rbnPreferentialSide_Buy.TabStop = true;
            this.rbnPreferentialSide_Buy.Text = "优先买入";
            this.rbnPreferentialSide_Buy.UseVisualStyleBackColor = false;
            this.rbnPreferentialSide_Buy.CheckedChanged += new System.EventHandler(this.rbnPreferentialSide_CheckedChanged);
            // 
            // rbnPreferentialSide_Sell
            // 
            this.rbnPreferentialSide_Sell.AutoSize = true;
            this.rbnPreferentialSide_Sell.Location = new System.Drawing.Point(2, 24);
            this.rbnPreferentialSide_Sell.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbnPreferentialSide_Sell.Name = "rbnPreferentialSide_Sell";
            this.rbnPreferentialSide_Sell.Size = new System.Drawing.Size(73, 17);
            this.rbnPreferentialSide_Sell.TabIndex = 51;
            this.rbnPreferentialSide_Sell.Text = "优先卖出";
            this.rbnPreferentialSide_Sell.UseVisualStyleBackColor = true;
            this.rbnPreferentialSide_Sell.CheckedChanged += new System.EventHandler(this.rbnPreferentialSide_CheckedChanged);
            // 
            // nudDifferentialUnit
            // 
            this.nudDifferentialUnit.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudDifferentialUnit.Location = new System.Drawing.Point(331, 100);
            this.nudDifferentialUnit.Name = "nudDifferentialUnit";
            this.nudDifferentialUnit.Size = new System.Drawing.Size(40, 23);
            this.nudDifferentialUnit.TabIndex = 34;
            this.nudDifferentialUnit.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(256, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "最大仓差单位：";
            // 
            // nudOrderQtyUint
            // 
            this.nudOrderQtyUint.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudOrderQtyUint.Location = new System.Drawing.Point(206, 100);
            this.nudOrderQtyUint.Name = "nudOrderQtyUint";
            this.nudOrderQtyUint.Size = new System.Drawing.Size(47, 23);
            this.nudOrderQtyUint.TabIndex = 32;
            this.nudOrderQtyUint.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // nudTotalOrderQty
            // 
            this.nudTotalOrderQty.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudTotalOrderQty.Location = new System.Drawing.Point(74, 100);
            this.nudTotalOrderQty.Name = "nudTotalOrderQty";
            this.nudTotalOrderQty.Size = new System.Drawing.Size(62, 23);
            this.nudTotalOrderQty.TabIndex = 31;
            this.nudTotalOrderQty.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // nudPriceSpreadThreshold
            // 
            this.nudPriceSpreadThreshold.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudPriceSpreadThreshold.Location = new System.Drawing.Point(247, 71);
            this.nudPriceSpreadThreshold.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudPriceSpreadThreshold.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudPriceSpreadThreshold.Name = "nudPriceSpreadThreshold";
            this.nudPriceSpreadThreshold.Size = new System.Drawing.Size(89, 23);
            this.nudPriceSpreadThreshold.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "开仓价差：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbnSellOrderPriceType_OpponentPrice);
            this.panel2.Controls.Add(this.rbnSellOrderPriceType_LastPrice);
            this.panel2.Controls.Add(this.rbnSellOrderPriceType_QueuePrice);
            this.panel2.Location = new System.Drawing.Point(182, 43);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 24);
            this.panel2.TabIndex = 18;
            // 
            // rbnSellOrderPriceType_OpponentPrice
            // 
            this.rbnSellOrderPriceType_OpponentPrice.AutoSize = true;
            this.rbnSellOrderPriceType_OpponentPrice.Location = new System.Drawing.Point(76, 4);
            this.rbnSellOrderPriceType_OpponentPrice.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbnSellOrderPriceType_OpponentPrice.Name = "rbnSellOrderPriceType_OpponentPrice";
            this.rbnSellOrderPriceType_OpponentPrice.Size = new System.Drawing.Size(61, 17);
            this.rbnSellOrderPriceType_OpponentPrice.TabIndex = 2;
            this.rbnSellOrderPriceType_OpponentPrice.TabStop = true;
            this.rbnSellOrderPriceType_OpponentPrice.Text = "对手价";
            this.rbnSellOrderPriceType_OpponentPrice.UseVisualStyleBackColor = true;
            this.rbnSellOrderPriceType_OpponentPrice.CheckedChanged += new System.EventHandler(this.rbnSellOrderPriceType_CheckedChanged);
            // 
            // rbnSellOrderPriceType_LastPrice
            // 
            this.rbnSellOrderPriceType_LastPrice.AutoSize = true;
            this.rbnSellOrderPriceType_LastPrice.Checked = true;
            this.rbnSellOrderPriceType_LastPrice.Location = new System.Drawing.Point(6, 4);
            this.rbnSellOrderPriceType_LastPrice.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbnSellOrderPriceType_LastPrice.Name = "rbnSellOrderPriceType_LastPrice";
            this.rbnSellOrderPriceType_LastPrice.Size = new System.Drawing.Size(61, 17);
            this.rbnSellOrderPriceType_LastPrice.TabIndex = 0;
            this.rbnSellOrderPriceType_LastPrice.TabStop = true;
            this.rbnSellOrderPriceType_LastPrice.Text = "最新价";
            this.rbnSellOrderPriceType_LastPrice.UseVisualStyleBackColor = true;
            this.rbnSellOrderPriceType_LastPrice.CheckedChanged += new System.EventHandler(this.rbnSellOrderPriceType_CheckedChanged);
            // 
            // rbnSellOrderPriceType_QueuePrice
            // 
            this.rbnSellOrderPriceType_QueuePrice.AutoSize = true;
            this.rbnSellOrderPriceType_QueuePrice.Location = new System.Drawing.Point(142, 4);
            this.rbnSellOrderPriceType_QueuePrice.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbnSellOrderPriceType_QueuePrice.Name = "rbnSellOrderPriceType_QueuePrice";
            this.rbnSellOrderPriceType_QueuePrice.Size = new System.Drawing.Size(61, 17);
            this.rbnSellOrderPriceType_QueuePrice.TabIndex = 49;
            this.rbnSellOrderPriceType_QueuePrice.Text = "排队价";
            this.rbnSellOrderPriceType_QueuePrice.UseVisualStyleBackColor = true;
            this.rbnSellOrderPriceType_QueuePrice.CheckedChanged += new System.EventHandler(this.rbnSellOrderPriceType_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbnBuyOrderPriceType_OpponentPrice);
            this.panel1.Controls.Add(this.rbnBuyOrderPriceType_LastPrice);
            this.panel1.Controls.Add(this.rbnBuyOrderPriceType_QueuePrice);
            this.panel1.Location = new System.Drawing.Point(182, 17);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 24);
            this.panel1.TabIndex = 17;
            // 
            // rbnBuyOrderPriceType_OpponentPrice
            // 
            this.rbnBuyOrderPriceType_OpponentPrice.AutoSize = true;
            this.rbnBuyOrderPriceType_OpponentPrice.Checked = true;
            this.rbnBuyOrderPriceType_OpponentPrice.Location = new System.Drawing.Point(76, 4);
            this.rbnBuyOrderPriceType_OpponentPrice.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbnBuyOrderPriceType_OpponentPrice.Name = "rbnBuyOrderPriceType_OpponentPrice";
            this.rbnBuyOrderPriceType_OpponentPrice.Size = new System.Drawing.Size(61, 17);
            this.rbnBuyOrderPriceType_OpponentPrice.TabIndex = 4;
            this.rbnBuyOrderPriceType_OpponentPrice.TabStop = true;
            this.rbnBuyOrderPriceType_OpponentPrice.Text = "对手价";
            this.rbnBuyOrderPriceType_OpponentPrice.UseVisualStyleBackColor = true;
            this.rbnBuyOrderPriceType_OpponentPrice.CheckedChanged += new System.EventHandler(this.rbnBuyOrderPriceType_CheckedChanged);
            // 
            // rbnBuyOrderPriceType_LastPrice
            // 
            this.rbnBuyOrderPriceType_LastPrice.AutoSize = true;
            this.rbnBuyOrderPriceType_LastPrice.Location = new System.Drawing.Point(5, 4);
            this.rbnBuyOrderPriceType_LastPrice.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbnBuyOrderPriceType_LastPrice.Name = "rbnBuyOrderPriceType_LastPrice";
            this.rbnBuyOrderPriceType_LastPrice.Size = new System.Drawing.Size(61, 17);
            this.rbnBuyOrderPriceType_LastPrice.TabIndex = 3;
            this.rbnBuyOrderPriceType_LastPrice.TabStop = true;
            this.rbnBuyOrderPriceType_LastPrice.Text = "最新价";
            this.rbnBuyOrderPriceType_LastPrice.UseVisualStyleBackColor = true;
            this.rbnBuyOrderPriceType_LastPrice.CheckedChanged += new System.EventHandler(this.rbnBuyOrderPriceType_CheckedChanged);
            // 
            // rbnBuyOrderPriceType_QueuePrice
            // 
            this.rbnBuyOrderPriceType_QueuePrice.AutoSize = true;
            this.rbnBuyOrderPriceType_QueuePrice.Location = new System.Drawing.Point(141, 4);
            this.rbnBuyOrderPriceType_QueuePrice.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbnBuyOrderPriceType_QueuePrice.Name = "rbnBuyOrderPriceType_QueuePrice";
            this.rbnBuyOrderPriceType_QueuePrice.Size = new System.Drawing.Size(61, 17);
            this.rbnBuyOrderPriceType_QueuePrice.TabIndex = 48;
            this.rbnBuyOrderPriceType_QueuePrice.Text = "排队价";
            this.rbnBuyOrderPriceType_QueuePrice.UseVisualStyleBackColor = true;
            this.rbnBuyOrderPriceType_QueuePrice.CheckedChanged += new System.EventHandler(this.rbnBuyOrderPriceType_CheckedChanged);
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(138, 106);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(78, 13);
            this.label59.TabIndex = 15;
            this.label59.Text = "单位(手/次)：";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(13, 104);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(67, 13);
            this.label58.TabIndex = 13;
            this.label58.Text = "单边手数：";
            // 
            // cbxSellInstrument
            // 
            this.cbxSellInstrument.FormattingEnabled = true;
            this.cbxSellInstrument.Location = new System.Drawing.Point(80, 44);
            this.cbxSellInstrument.Name = "cbxSellInstrument";
            this.cbxSellInstrument.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbxSellInstrument.Size = new System.Drawing.Size(102, 21);
            this.cbxSellInstrument.TabIndex = 11;
            // 
            // cbxBuyInstrument
            // 
            this.cbxBuyInstrument.FormattingEnabled = true;
            this.cbxBuyInstrument.Location = new System.Drawing.Point(80, 20);
            this.cbxBuyInstrument.Name = "cbxBuyInstrument";
            this.cbxBuyInstrument.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbxBuyInstrument.Size = new System.Drawing.Size(102, 21);
            this.cbxBuyInstrument.TabIndex = 10;
            // 
            // ArbitrageOpenArgumentViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ArbitrageOpenArgumentViewControl";
            this.Size = new System.Drawing.Size(389, 133);
            this.Load += new System.EventHandler(this.CombinationOrderPanelControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDifferentialUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOrderQtyUint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotalOrderQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPriceSpreadThreshold)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbxSellInstrument;
        private System.Windows.Forms.ComboBox cbxBuyInstrument;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbnSellOrderPriceType_OpponentPrice;
        private System.Windows.Forms.RadioButton rbnSellOrderPriceType_LastPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudOrderQtyUint;
        private System.Windows.Forms.NumericUpDown nudTotalOrderQty;
        private System.Windows.Forms.NumericUpDown nudPriceSpreadThreshold;
        private System.Windows.Forms.RadioButton rbnBuyOrderPriceType_OpponentPrice;
        private System.Windows.Forms.RadioButton rbnBuyOrderPriceType_LastPrice;
        private System.Windows.Forms.NumericUpDown nudDifferentialUnit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbnPriceSpreadSide_Less;
        private System.Windows.Forms.RadioButton rbnPriceSpreadSide_Greater;
        private System.Windows.Forms.RadioButton rbnBuyOrderPriceType_QueuePrice;
        private System.Windows.Forms.RadioButton rbnSellOrderPriceType_QueuePrice;
        private System.Windows.Forms.RadioButton rbnPreferentialSide_Sell;
        private System.Windows.Forms.RadioButton rbnPreferentialSide_Buy;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
    }
}
