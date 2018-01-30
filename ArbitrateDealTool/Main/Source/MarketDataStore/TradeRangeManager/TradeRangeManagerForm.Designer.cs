namespace TradeRangeManager
{
    partial class TradeRangeManagerForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView_ProductInfo = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新增品种ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label_ProductNum = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label_MarketNum = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView_TradeSection = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button_Load = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Product = new System.Windows.Forms.TextBox();
            this.textBox_Market = new System.Windows.Forms.TextBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ProductInfo)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TradeSection)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView_ProductInfo);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(380, 396);
            this.panel1.TabIndex = 0;
            // 
            // dataGridView_ProductInfo
            // 
            this.dataGridView_ProductInfo.AllowUserToAddRows = false;
            this.dataGridView_ProductInfo.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_ProductInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_ProductInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView_ProductInfo.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView_ProductInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_ProductInfo.Location = new System.Drawing.Point(0, 77);
            this.dataGridView_ProductInfo.Name = "dataGridView_ProductInfo";
            this.dataGridView_ProductInfo.RowTemplate.Height = 24;
            this.dataGridView_ProductInfo.Size = new System.Drawing.Size(380, 319);
            this.dataGridView_ProductInfo.TabIndex = 0;
            this.dataGridView_ProductInfo.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridProductInfoCell_Click);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Exchange";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column1.HeaderText = "交易所";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Name";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column2.HeaderText = "品种";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增品种ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 30);
            // 
            // 新增品种ToolStripMenuItem
            // 
            this.新增品种ToolStripMenuItem.Name = "新增品种ToolStripMenuItem";
            this.新增品种ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.新增品种ToolStripMenuItem.Text = "新增品种";
            this.新增品种ToolStripMenuItem.Click += new System.EventHandler(this.TradeRangeSectionAdd_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.label_ProductNum);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.label_MarketNum);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(380, 77);
            this.panel4.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(154, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 17);
            this.label8.TabIndex = 7;
            this.label8.Text = "个品种";
            // 
            // label_ProductNum
            // 
            this.label_ProductNum.AutoSize = true;
            this.label_ProductNum.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ProductNum.ForeColor = System.Drawing.Color.Tomato;
            this.label_ProductNum.Location = new System.Drawing.Point(110, 35);
            this.label_ProductNum.Name = "label_ProductNum";
            this.label_ProductNum.Size = new System.Drawing.Size(18, 19);
            this.label_ProductNum.TabIndex = 6;
            this.label_ProductNum.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(53, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "个市场";
            // 
            // label_MarketNum
            // 
            this.label_MarketNum.AutoSize = true;
            this.label_MarketNum.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_MarketNum.ForeColor = System.Drawing.Color.Tomato;
            this.label_MarketNum.Location = new System.Drawing.Point(37, 35);
            this.label_MarketNum.Name = "label_MarketNum";
            this.label_MarketNum.Size = new System.Drawing.Size(18, 19);
            this.label_MarketNum.TabIndex = 4;
            this.label_MarketNum.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "共";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "市场总览：";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView_TradeSection);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(376, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(529, 396);
            this.panel2.TabIndex = 2;
            // 
            // dataGridView_TradeSection
            // 
            this.dataGridView_TradeSection.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_TradeSection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_TradeSection.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column4,
            this.Column5});
            this.dataGridView_TradeSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_TradeSection.Location = new System.Drawing.Point(0, 77);
            this.dataGridView_TradeSection.Name = "dataGridView_TradeSection";
            this.dataGridView_TradeSection.RowTemplate.Height = 24;
            this.dataGridView_TradeSection.Size = new System.Drawing.Size(529, 319);
            this.dataGridView_TradeSection.TabIndex = 1;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "BeginTime";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column3.HeaderText = "开始时间";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "EndTime";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column4.HeaderText = "截止时间";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "IsNight";
            this.Column5.HeaderText = "夜盘";
            this.Column5.Name = "Column5";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button_Load);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.textBox_Product);
            this.panel3.Controls.Add(this.textBox_Market);
            this.panel3.Controls.Add(this.button_Save);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(529, 77);
            this.panel3.TabIndex = 0;
            // 
            // button_Load
            // 
            this.button_Load.Location = new System.Drawing.Point(306, 12);
            this.button_Load.Name = "button_Load";
            this.button_Load.Size = new System.Drawing.Size(102, 43);
            this.button_Load.TabIndex = 6;
            this.button_Load.Text = "加载配置";
            this.button_Load.UseVisualStyleBackColor = true;
            this.button_Load.Click += new System.EventHandler(this.button_Load_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "品种";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "市场";
            // 
            // textBox_Product
            // 
            this.textBox_Product.Location = new System.Drawing.Point(46, 39);
            this.textBox_Product.Name = "textBox_Product";
            this.textBox_Product.Size = new System.Drawing.Size(100, 22);
            this.textBox_Product.TabIndex = 3;
            // 
            // textBox_Market
            // 
            this.textBox_Market.Location = new System.Drawing.Point(46, 6);
            this.textBox_Market.Name = "textBox_Market";
            this.textBox_Market.Size = new System.Drawing.Size(100, 22);
            this.textBox_Market.TabIndex = 2;
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(183, 13);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(93, 43);
            this.button_Save.TabIndex = 1;
            this.button_Save.Text = "保存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // TradeRangeManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 396);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "TradeRangeManagerForm";
            this.Text = "品种交易时间区间管理";
            this.Load += new System.EventHandler(this.TradeRangeManagerForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ProductInfo)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TradeSection)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView_ProductInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView_TradeSection;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Product;
        private System.Windows.Forms.TextBox textBox_Market;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 新增品种ToolStripMenuItem;
        private System.Windows.Forms.Button button_Load;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label_ProductNum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_MarketNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column5;
    }
}

