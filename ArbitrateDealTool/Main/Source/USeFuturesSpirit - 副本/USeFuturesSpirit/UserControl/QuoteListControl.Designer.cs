namespace USeFuturesSpirit
{
    partial class QuoteListControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridQuote = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_LastPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_NetChange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_PctChange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_BidPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_AskPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_HighPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_LowPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_OpenPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.gridQuote)).BeginInit();
            this.SuspendLayout();
            // 
            // gridQuote
            // 
            this.gridQuote.AllowUserToAddRows = false;
            this.gridQuote.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridQuote.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridQuote.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridQuote.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column_LastPrice,
            this.Column_NetChange,
            this.Column_PctChange,
            this.Column_BidPrice,
            this.Column6,
            this.Column_AskPrice,
            this.Column8,
            this.Column_HighPrice,
            this.Column_LowPrice,
            this.Column_OpenPrice,
            this.Column13,
            this.Column14});
            this.gridQuote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridQuote.Location = new System.Drawing.Point(0, 0);
            this.gridQuote.Name = "gridQuote";
            this.gridQuote.RowHeadersWidth = 20;
            this.gridQuote.RowTemplate.Height = 24;
            this.gridQuote.Size = new System.Drawing.Size(1041, 245);
            this.gridQuote.TabIndex = 0;
            this.gridQuote.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellContentClickEvent);
            this.gridQuote.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridQuote_CellFormatting);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Instrument";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "合约";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 80;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "InstrumentName";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.Frozen = true;
            this.Column2.HeaderText = "合约名";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 80;
            // 
            // Column_LastPrice
            // 
            this.Column_LastPrice.DataPropertyName = "LastPrice";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column_LastPrice.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column_LastPrice.HeaderText = "最新价";
            this.Column_LastPrice.Name = "Column_LastPrice";
            this.Column_LastPrice.ReadOnly = true;
            this.Column_LastPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_LastPrice.Width = 70;
            // 
            // Column_NetChange
            // 
            this.Column_NetChange.DataPropertyName = "NetChange";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column_NetChange.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column_NetChange.HeaderText = "涨跌";
            this.Column_NetChange.Name = "Column_NetChange";
            this.Column_NetChange.ReadOnly = true;
            this.Column_NetChange.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_NetChange.Width = 70;
            // 
            // Column_PctChange
            // 
            this.Column_PctChange.DataPropertyName = "PctChange";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "P2";
            this.Column_PctChange.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column_PctChange.HeaderText = "涨跌幅";
            this.Column_PctChange.Name = "Column_PctChange";
            this.Column_PctChange.ReadOnly = true;
            this.Column_PctChange.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_PctChange.Width = 70;
            // 
            // Column_BidPrice
            // 
            this.Column_BidPrice.DataPropertyName = "BidPrice";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column_BidPrice.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column_BidPrice.HeaderText = "买价";
            this.Column_BidPrice.Name = "Column_BidPrice";
            this.Column_BidPrice.ReadOnly = true;
            this.Column_BidPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_BidPrice.Width = 70;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "BidSize";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column6.DefaultCellStyle = dataGridViewCellStyle8;
            this.Column6.HeaderText = "买量";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 70;
            // 
            // Column_AskPrice
            // 
            this.Column_AskPrice.DataPropertyName = "AskPrice";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column_AskPrice.DefaultCellStyle = dataGridViewCellStyle9;
            this.Column_AskPrice.HeaderText = "卖价";
            this.Column_AskPrice.Name = "Column_AskPrice";
            this.Column_AskPrice.ReadOnly = true;
            this.Column_AskPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_AskPrice.Width = 70;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "AskSize";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column8.DefaultCellStyle = dataGridViewCellStyle10;
            this.Column8.HeaderText = "卖量";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column8.Width = 70;
            // 
            // Column_HighPrice
            // 
            this.Column_HighPrice.DataPropertyName = "HighPrice";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column_HighPrice.DefaultCellStyle = dataGridViewCellStyle11;
            this.Column_HighPrice.HeaderText = "最高";
            this.Column_HighPrice.Name = "Column_HighPrice";
            this.Column_HighPrice.ReadOnly = true;
            this.Column_HighPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_HighPrice.Width = 70;
            // 
            // Column_LowPrice
            // 
            this.Column_LowPrice.DataPropertyName = "LowPrice";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column_LowPrice.DefaultCellStyle = dataGridViewCellStyle12;
            this.Column_LowPrice.HeaderText = "最低";
            this.Column_LowPrice.Name = "Column_LowPrice";
            this.Column_LowPrice.ReadOnly = true;
            this.Column_LowPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_LowPrice.Width = 70;
            // 
            // Column_OpenPrice
            // 
            this.Column_OpenPrice.DataPropertyName = "OpenPrice";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column_OpenPrice.DefaultCellStyle = dataGridViewCellStyle13;
            this.Column_OpenPrice.HeaderText = "开盘价";
            this.Column_OpenPrice.Name = "Column_OpenPrice";
            this.Column_OpenPrice.ReadOnly = true;
            this.Column_OpenPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_OpenPrice.Width = 70;
            // 
            // Column13
            // 
            this.Column13.DataPropertyName = "PreClosePrice";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column13.DefaultCellStyle = dataGridViewCellStyle14;
            this.Column13.HeaderText = "昨收价";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            this.Column13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column13.Width = 70;
            // 
            // Column14
            // 
            this.Column14.DataPropertyName = "PreSettlementPrice";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column14.DefaultCellStyle = dataGridViewCellStyle15;
            this.Column14.HeaderText = "昨结价";
            this.Column14.Name = "Column14";
            this.Column14.ReadOnly = true;
            this.Column14.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column14.Width = 70;
            // 
            // QuoteListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.gridQuote);
            this.Name = "QuoteListControl";
            this.Size = new System.Drawing.Size(1041, 245);
            this.Load += new System.EventHandler(this.QuoteListControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridQuote)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridQuote;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_LastPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_NetChange;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PctChange;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_BidPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_AskPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_HighPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_LowPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_OpenPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}
