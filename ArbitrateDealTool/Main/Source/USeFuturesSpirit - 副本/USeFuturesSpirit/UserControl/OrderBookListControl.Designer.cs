namespace USeFuturesSpirit
{
    partial class OrderBookListControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridOrder = new System.Windows.Forms.DataGridView();
            this.Column17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbnOrderState_CancelAndError = new System.Windows.Forms.RadioButton();
            this.rbnOrderState_Traded = new System.Windows.Forms.RadioButton();
            this.rbnOrderState_UnTrade = new System.Windows.Forms.RadioButton();
            this.rbnOrderState_All = new System.Windows.Forms.RadioButton();
            this.button_CancelAllOrders = new System.Windows.Forms.Button();
            this.button_CancelOrder = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridOrder)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridOrder
            // 
            this.gridOrder.AllowUserToAddRows = false;
            this.gridOrder.AllowUserToDeleteRows = false;
            this.gridOrder.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.gridOrder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridOrder.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOrder.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column17,
            this.Column2,
            this.Column14,
            this.Column15,
            this.Column5,
            this.Column6,
            this.Column11,
            this.Column12,
            this.Column13,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column19,
            this.Column16,
            this.Column3,
            this.Column18});
            this.gridOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridOrder.Location = new System.Drawing.Point(0, 48);
            this.gridOrder.Name = "gridOrder";
            this.gridOrder.ReadOnly = true;
            this.gridOrder.RowHeadersWidth = 20;
            this.gridOrder.RowTemplate.Height = 24;
            this.gridOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridOrder.Size = new System.Drawing.Size(1098, 259);
            this.gridOrder.TabIndex = 0;
            // 
            // Column17
            // 
            this.Column17.DataPropertyName = "OrderTime";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Format = "HH:mm:ss";
            this.Column17.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column17.HeaderText = "委托时间";
            this.Column17.Name = "Column17";
            this.Column17.ReadOnly = true;
            this.Column17.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column17.Width = 80;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "InstrumentCode";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "合约代码";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 70;
            // 
            // Column14
            // 
            this.Column14.DataPropertyName = "OrderSideDesc";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column14.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column14.HeaderText = "多空";
            this.Column14.Name = "Column14";
            this.Column14.ReadOnly = true;
            this.Column14.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column14.Width = 55;
            // 
            // Column15
            // 
            this.Column15.DataPropertyName = "OffsetTypeDesc";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column15.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column15.HeaderText = "开平";
            this.Column15.Name = "Column15";
            this.Column15.ReadOnly = true;
            this.Column15.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column15.Width = 55;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "OrderQty";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column5.HeaderText = "委托量";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 65;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "OrderPrice";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "#,0.00";
            this.Column6.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column6.HeaderText = "委托价格";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 80;
            // 
            // Column11
            // 
            this.Column11.DataPropertyName = "OrderStatusDesc";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column11.DefaultCellStyle = dataGridViewCellStyle8;
            this.Column11.HeaderText = "委托状态";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            this.Column11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column11.Width = 80;
            // 
            // Column12
            // 
            this.Column12.DataPropertyName = "CancelQty";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column12.DefaultCellStyle = dataGridViewCellStyle9;
            this.Column12.HeaderText = "撤单量";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            this.Column12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column12.Width = 65;
            // 
            // Column13
            // 
            this.Column13.DataPropertyName = "BlankQty";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column13.DefaultCellStyle = dataGridViewCellStyle10;
            this.Column13.HeaderText = "废单量";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            this.Column13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column13.Width = 65;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "TradeQty";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column7.DefaultCellStyle = dataGridViewCellStyle11;
            this.Column7.HeaderText = "成交量";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column7.Width = 65;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "TradeAmount";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "#,0.00";
            this.Column8.DefaultCellStyle = dataGridViewCellStyle12;
            this.Column8.HeaderText = "成交金额";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "TradePrice";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.Format = "#,0.00";
            this.Column9.DefaultCellStyle = dataGridViewCellStyle13;
            this.Column9.HeaderText = "成交价格";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column10
            // 
            this.Column10.DataPropertyName = "TradeFee";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.Format = "#,0.00";
            this.Column10.DefaultCellStyle = dataGridViewCellStyle14;
            this.Column10.HeaderText = "手续费";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column10.Width = 80;
            // 
            // Column19
            // 
            this.Column19.DataPropertyName = "OrderNum";
            this.Column19.HeaderText = "委托单号";
            this.Column19.Name = "Column19";
            this.Column19.ReadOnly = true;
            this.Column19.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column19.Visible = false;
            // 
            // Column16
            // 
            this.Column16.DataPropertyName = "Memo";
            this.Column16.HeaderText = "备注";
            this.Column16.Name = "Column16";
            this.Column16.ReadOnly = true;
            this.Column16.Width = 120;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "InstrumentName";
            this.Column3.HeaderText = "合约名称";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Visible = false;
            this.Column3.Width = 50;
            // 
            // Column18
            // 
            this.Column18.DataPropertyName = "IsFinish";
            this.Column18.HeaderText = "委托是否结束";
            this.Column18.Name = "Column18";
            this.Column18.ReadOnly = true;
            this.Column18.Visible = false;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel2);
            this.panelTop.Controls.Add(this.button_CancelAllOrders);
            this.panelTop.Controls.Add(this.button_CancelOrder);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1098, 48);
            this.panelTop.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbnOrderState_CancelAndError);
            this.panel2.Controls.Add(this.rbnOrderState_Traded);
            this.panel2.Controls.Add(this.rbnOrderState_UnTrade);
            this.panel2.Controls.Add(this.rbnOrderState_All);
            this.panel2.Location = new System.Drawing.Point(4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(412, 40);
            this.panel2.TabIndex = 2;
            // 
            // rbnOrderState_CancelAndError
            // 
            this.rbnOrderState_CancelAndError.AutoSize = true;
            this.rbnOrderState_CancelAndError.Location = new System.Drawing.Point(256, 8);
            this.rbnOrderState_CancelAndError.Name = "rbnOrderState_CancelAndError";
            this.rbnOrderState_CancelAndError.Size = new System.Drawing.Size(132, 22);
            this.rbnOrderState_CancelAndError.TabIndex = 3;
            this.rbnOrderState_CancelAndError.Text = "已撤单/错单";
            this.rbnOrderState_CancelAndError.UseVisualStyleBackColor = true;
            this.rbnOrderState_CancelAndError.CheckedChanged += new System.EventHandler(this.rbnOrderState_CancelAndError_CheckedChanged);
            // 
            // rbnOrderState_Traded
            // 
            this.rbnOrderState_Traded.AutoSize = true;
            this.rbnOrderState_Traded.Location = new System.Drawing.Point(169, 8);
            this.rbnOrderState_Traded.Name = "rbnOrderState_Traded";
            this.rbnOrderState_Traded.Size = new System.Drawing.Size(87, 22);
            this.rbnOrderState_Traded.TabIndex = 2;
            this.rbnOrderState_Traded.Text = "已成交";
            this.rbnOrderState_Traded.UseVisualStyleBackColor = true;
            this.rbnOrderState_Traded.CheckedChanged += new System.EventHandler(this.rbnOrderState_Traded_CheckedChanged);
            // 
            // rbnOrderState_UnTrade
            // 
            this.rbnOrderState_UnTrade.AutoSize = true;
            this.rbnOrderState_UnTrade.Location = new System.Drawing.Point(96, 8);
            this.rbnOrderState_UnTrade.Name = "rbnOrderState_UnTrade";
            this.rbnOrderState_UnTrade.Size = new System.Drawing.Size(69, 22);
            this.rbnOrderState_UnTrade.TabIndex = 1;
            this.rbnOrderState_UnTrade.Text = "挂单";
            this.rbnOrderState_UnTrade.UseVisualStyleBackColor = true;
            this.rbnOrderState_UnTrade.CheckedChanged += new System.EventHandler(this.rbnOrderState_UnTrade_CheckedChanged);
            // 
            // rbnOrderState_All
            // 
            this.rbnOrderState_All.AutoSize = true;
            this.rbnOrderState_All.Checked = true;
            this.rbnOrderState_All.Location = new System.Drawing.Point(9, 8);
            this.rbnOrderState_All.Name = "rbnOrderState_All";
            this.rbnOrderState_All.Size = new System.Drawing.Size(87, 22);
            this.rbnOrderState_All.TabIndex = 0;
            this.rbnOrderState_All.TabStop = true;
            this.rbnOrderState_All.Text = "全部单";
            this.rbnOrderState_All.UseVisualStyleBackColor = true;
            this.rbnOrderState_All.CheckedChanged += new System.EventHandler(this.rbnOrderState_All_CheckedChanged);
            // 
            // button_CancelAllOrders
            // 
            this.button_CancelAllOrders.Location = new System.Drawing.Point(600, 4);
            this.button_CancelAllOrders.Name = "button_CancelAllOrders";
            this.button_CancelAllOrders.Size = new System.Drawing.Size(84, 40);
            this.button_CancelAllOrders.TabIndex = 1;
            this.button_CancelAllOrders.Text = "全撤";
            this.button_CancelAllOrders.UseVisualStyleBackColor = true;
            this.button_CancelAllOrders.Click += new System.EventHandler(this.button_CancelAllOrders_Click);
            // 
            // button_CancelOrder
            // 
            this.button_CancelOrder.Location = new System.Drawing.Point(508, 4);
            this.button_CancelOrder.Name = "button_CancelOrder";
            this.button_CancelOrder.Size = new System.Drawing.Size(84, 40);
            this.button_CancelOrder.TabIndex = 0;
            this.button_CancelOrder.Text = "撤单";
            this.button_CancelOrder.UseVisualStyleBackColor = true;
            this.button_CancelOrder.Click += new System.EventHandler(this.button_CancelOrder_Click);
            // 
            // OrderBookListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridOrder);
            this.Controls.Add(this.panelTop);
            this.Name = "OrderBookListControl";
            this.Size = new System.Drawing.Size(1098, 307);
            ((System.ComponentModel.ISupportInitialize)(this.gridOrder)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column17;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column15;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column19;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column16;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column18;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbnOrderState_CancelAndError;
        private System.Windows.Forms.RadioButton rbnOrderState_Traded;
        private System.Windows.Forms.RadioButton rbnOrderState_UnTrade;
        private System.Windows.Forms.RadioButton rbnOrderState_All;
        private System.Windows.Forms.Button button_CancelAllOrders;
        private System.Windows.Forms.Button button_CancelOrder;
    }
}
