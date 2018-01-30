namespace UseOnlineTradingSystem
{
    partial class TestForm
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.aquaTheme1 = new Telerik.WinControls.Themes.AquaTheme();
            this.radGridView_TradeInfo = new Telerik.WinControls.UI.RadGridView();
            this.visualStudio2012DarkTheme1 = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView_TradeInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView_TradeInfo.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // radGridView_TradeInfo
            // 
            this.radGridView_TradeInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView_TradeInfo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radGridView_TradeInfo.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            gridViewTextBoxColumn1.HeaderText = "column1";
            gridViewTextBoxColumn1.Name = "column1";
            gridViewTextBoxColumn2.HeaderText = "column2";
            gridViewTextBoxColumn2.Name = "column2";
            gridViewTextBoxColumn3.HeaderText = "column3";
            gridViewTextBoxColumn3.Name = "column3";
            gridViewTextBoxColumn4.HeaderText = "column4";
            gridViewTextBoxColumn4.Name = "column4";
            gridViewTextBoxColumn5.HeaderText = "column5";
            gridViewTextBoxColumn5.Name = "column5";
            gridViewTextBoxColumn6.HeaderText = "column6";
            gridViewTextBoxColumn6.Name = "column6";
            gridViewTextBoxColumn7.HeaderText = "column7";
            gridViewTextBoxColumn7.Name = "column7";
            gridViewTextBoxColumn8.HeaderText = "column8";
            gridViewTextBoxColumn8.Name = "column8";
            this.radGridView_TradeInfo.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8});
            this.radGridView_TradeInfo.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView_TradeInfo.Name = "radGridView_TradeInfo";
            this.radGridView_TradeInfo.Size = new System.Drawing.Size(1167, 557);
            this.radGridView_TradeInfo.TabIndex = 0;
            this.radGridView_TradeInfo.Text = "radGridView1";
            this.radGridView_TradeInfo.ThemeName = "VisualStudio2012Dark";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 557);
            this.Controls.Add(this.radGridView_TradeInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView_TradeInfo.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView_TradeInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.Themes.AquaTheme aquaTheme1;
        private Telerik.WinControls.UI.RadGridView radGridView_TradeInfo;
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme1;
    }
}