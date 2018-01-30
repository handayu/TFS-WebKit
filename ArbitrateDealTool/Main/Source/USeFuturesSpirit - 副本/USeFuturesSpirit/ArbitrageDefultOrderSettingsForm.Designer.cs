namespace USeFuturesSpirit
{
    partial class ArbitrageDefultOrderSettingsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView_ArbitrageOrderSettings = new System.Windows.Forms.DataGridView();
            this.button_OK = new System.Windows.Forms.Button();
            this.button2_Cancel = new System.Windows.Forms.Button();
            this.Column_Product = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column_Volumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_PerNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_OpenDirection = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column_CloseDirection = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column_StoplossDirection = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column_NearOpenStyle = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column_FarOpenStyle = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ArbitrageOrderSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_ArbitrageOrderSettings
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_ArbitrageOrderSettings.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_ArbitrageOrderSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_ArbitrageOrderSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_Product,
            this.Column_Volumn,
            this.Column_PerNum,
            this.Column_OpenDirection,
            this.Column_CloseDirection,
            this.Column_StoplossDirection,
            this.Column_NearOpenStyle,
            this.Column_FarOpenStyle});
            this.dataGridView_ArbitrageOrderSettings.Location = new System.Drawing.Point(9, 12);
            this.dataGridView_ArbitrageOrderSettings.Name = "dataGridView_ArbitrageOrderSettings";
            this.dataGridView_ArbitrageOrderSettings.Size = new System.Drawing.Size(1392, 251);
            this.dataGridView_ArbitrageOrderSettings.TabIndex = 1;
            this.dataGridView_ArbitrageOrderSettings.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataGridView_DataError);
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(1186, 269);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(84, 28);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "确认";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button2_Cancel
            // 
            this.button2_Cancel.Location = new System.Drawing.Point(1306, 269);
            this.button2_Cancel.Name = "button2_Cancel";
            this.button2_Cancel.Size = new System.Drawing.Size(84, 28);
            this.button2_Cancel.TabIndex = 3;
            this.button2_Cancel.Text = "取消";
            this.button2_Cancel.UseVisualStyleBackColor = true;
            this.button2_Cancel.Click += new System.EventHandler(this.button2_Cancel_Click);
            // 
            // Column_Product
            // 
            this.Column_Product.DataPropertyName = "ProductName";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_Product.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column_Product.HeaderText = "品种";
            this.Column_Product.Name = "Column_Product";
            this.Column_Product.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_Product.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column_Product.Width = 130;
            // 
            // Column_Volumn
            // 
            this.Column_Volumn.DataPropertyName = "OpenVolumn";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_Volumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column_Volumn.HeaderText = "开仓手数";
            this.Column_Volumn.Name = "Column_Volumn";
            // 
            // Column_PerNum
            // 
            this.Column_PerNum.DataPropertyName = "PerOpenVolumn";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_PerNum.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column_PerNum.HeaderText = "手/次";
            this.Column_PerNum.Name = "Column_PerNum";
            // 
            // Column_OpenDirection
            // 
            this.Column_OpenDirection.DataPropertyName = "OpenFirstDirectionID";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_OpenDirection.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column_OpenDirection.HeaderText = "开仓优先方向";
            this.Column_OpenDirection.Name = "Column_OpenDirection";
            this.Column_OpenDirection.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_OpenDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column_OpenDirection.Width = 130;
            // 
            // Column_CloseDirection
            // 
            this.Column_CloseDirection.DataPropertyName = "CloseFirstDirectionID";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_CloseDirection.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column_CloseDirection.HeaderText = "平仓优先方向";
            this.Column_CloseDirection.Name = "Column_CloseDirection";
            this.Column_CloseDirection.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_CloseDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column_CloseDirection.Width = 130;
            // 
            // Column_StoplossDirection
            // 
            this.Column_StoplossDirection.DataPropertyName = "StopLossFirstDirectionID";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_StoplossDirection.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column_StoplossDirection.HeaderText = "止损优先方向";
            this.Column_StoplossDirection.Name = "Column_StoplossDirection";
            this.Column_StoplossDirection.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_StoplossDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column_StoplossDirection.Width = 130;
            // 
            // Column_NearOpenStyle
            // 
            this.Column_NearOpenStyle.DataPropertyName = "NearOpenPriceStyleID";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_NearOpenStyle.DefaultCellStyle = dataGridViewCellStyle8;
            this.Column_NearOpenStyle.HeaderText = "近月开仓价格类型";
            this.Column_NearOpenStyle.Name = "Column_NearOpenStyle";
            this.Column_NearOpenStyle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_NearOpenStyle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column_NearOpenStyle.Width = 160;
            // 
            // Column_FarOpenStyle
            // 
            this.Column_FarOpenStyle.DataPropertyName = "FarOpenPriceStyleID";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_FarOpenStyle.DefaultCellStyle = dataGridViewCellStyle9;
            this.Column_FarOpenStyle.HeaderText = "远月开仓价格类型";
            this.Column_FarOpenStyle.Name = "Column_FarOpenStyle";
            this.Column_FarOpenStyle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_FarOpenStyle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column_FarOpenStyle.Width = 160;
            // 
            // ArbitrageDefultOrderSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1413, 309);
            this.Controls.Add(this.button2_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.dataGridView_ArbitrageOrderSettings);
            this.Name = "ArbitrageDefultOrderSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自选合约默认下单参数设定";
            this.Load += new System.EventHandler(this.Load_Form);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ArbitrageOrderSettings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_ArbitrageOrderSettings;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button2_Cancel;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column_Product;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Volumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PerNum;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column_OpenDirection;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column_CloseDirection;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column_StoplossDirection;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column_NearOpenStyle;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column_FarOpenStyle;
    }
}