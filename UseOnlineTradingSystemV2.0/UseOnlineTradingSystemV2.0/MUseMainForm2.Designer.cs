namespace UseOnlineTradingSystem
{
    partial class MUseMainForm2 
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();

            this.infoCountLabel = new Telerik.WinControls.UI.RadLabel();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.radGridView2 = new Telerik.WinControls.UI.RadGridView();
            this.infoAverageLabel = new Telerik.WinControls.UI.RadLabel();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.radPanel2 = new Telerik.WinControls.UI.RadPanel();
            this.radPanel3 = new Telerik.WinControls.UI.RadPanel();
            this.radLblAverage = new Telerik.WinControls.UI.RadLabel();
            this.radLblRefreshCount = new Telerik.WinControls.UI.RadLabel();
            this.rb = new Telerik.WinControls.UI.RadButton();
            this.radDropDownButton1 = new Telerik.WinControls.UI.RadDropDownButton();
            ((System.ComponentModel.ISupportInitialize)(this.infoCountLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoAverageLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).BeginInit();
            this.radPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel3)).BeginInit();
            this.radPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLblAverage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblRefreshCount)).BeginInit();
            this.SuspendLayout();

            // 
            // radDropDownButton1
            // 
            this.radDropDownButton1.Location = new System.Drawing.Point(1208,0);
            this.radDropDownButton1.Name = "radDropDownButton1";
            this.radDropDownButton1.Size = new System.Drawing.Size(150, 36);
            this.radDropDownButton1.TabIndex = 1;
            this.radDropDownButton1.Text = "请选择";

            // 
            // radGridView1
            // 
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.ForeColor = System.Drawing.Color.Black;
            this.radGridView1.Location = new System.Drawing.Point(0, 66);
            this.radGridView1.Margin = new System.Windows.Forms.Padding(4);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radGridView1.Size = new System.Drawing.Size(1200, 927);
            this.radGridView1.Text = "radGridView1";

            // 
            // radGridView2
            // 
            this.radGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView2.ForeColor = System.Drawing.Color.Black;
            this.radGridView2.Location = new System.Drawing.Point(0, 66);
            this.radGridView2.Margin = new System.Windows.Forms.Padding(4);
            // 
            // 
            // 
            this.radGridView2.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.radGridView2.Name = "radGridView2";
            this.radGridView2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radGridView2.Size = new System.Drawing.Size(1200, 927);
            this.radGridView2.Text = "radGridView2";

            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.radDropDownButton1);
            this.radPanel1.Controls.Add(this.rb);
            this.radPanel1.Controls.Add(this.infoAverageLabel);
            this.radPanel1.Controls.Add(this.infoCountLabel);
            this.radPanel1.Controls.Add(this.radLblAverage);
            this.radPanel1.Controls.Add(this.radLblRefreshCount);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.None;
            this.radPanel1.ForeColor = System.Drawing.Color.Black;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(1200, 40);
            this.radPanel1.TabIndex = 4;

            // 
            // radPanel2
            // 
            this.radPanel2.Controls.Add(this.radGridView1);
            this.radPanel2.Dock = System.Windows.Forms.DockStyle.None;
            this.radPanel2.ForeColor = System.Drawing.Color.Black;
            this.radPanel2.Location = new System.Drawing.Point(0, 0);
            this.radPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.radPanel2.Name = "radPanel2";
            this.radPanel2.Size = new System.Drawing.Size(1200, 400);
            //this.radPanel1.SizeChanged += new System.EventHandler(RadPanel1_SizeChanged);

            // 
            // radPanel3
            // 
            this.radPanel3.Controls.Add(this.radGridView2);
            this.radPanel3.Dock = System.Windows.Forms.DockStyle.None;
            this.radPanel3.ForeColor = System.Drawing.Color.Black;
            this.radPanel3.Location = new System.Drawing.Point(0, 0);
            this.radPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.radPanel3.Name = "radPanel3";
            this.radPanel3.Size = new System.Drawing.Size(1200, 400);
            this.radPanel3.TabIndex = 4;
            //this.radPanel3.SizeChanged += new System.EventHandler(RadPanel1_SizeChanged);

            // 
            // rb
            // 
            this.rb.Location = new System.Drawing.Point(10, 10);
            this.rb.Name = "rb";
            this.rb.Size = new System.Drawing.Size(170, 30);
            this.rb.AutoSize = true;
            this.rb.Text = "刷新数据";
            this.rb.Click +=new System.EventHandler(Rb_Click);

            int beginX = 200;
            int avg = -80;
            // 
            // infoCountLabel
            // 
            this.infoCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif",
                11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.infoCountLabel.ForeColor = System.Drawing.Color.Red;
            this.infoCountLabel.Location = new System.Drawing.Point(beginX, 10);
            this.infoCountLabel.Name = "infoCountLabel";
            this.infoCountLabel.Size = new System.Drawing.Size(170, 29);
            this.infoCountLabel.AutoSize = true;
            this.infoCountLabel.TabIndex = 1;
            this.infoCountLabel.Text = "买入基价: ";
            this.infoCountLabel.AutoSize = true;
            this.infoCountLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;

            beginX += this.infoCountLabel.Width + avg;
            // 
            // radLblRefreshCount
            // 
            this.radLblRefreshCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 
                18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radLblRefreshCount.Location = new System.Drawing.Point(beginX, 2);
            this.radLblRefreshCount.Margin = new System.Windows.Forms.Padding(4);
            this.radLblRefreshCount.Name = "radLblRefreshCount";
            this.radLblRefreshCount.Size = new System.Drawing.Size(118, 26);
            this.radLblRefreshCount.AutoSize = true;
            this.radLblRefreshCount.TabIndex = 0;
            this.radLblRefreshCount.Text = "50000";

            beginX += this.radLblRefreshCount.Width - 30;
            // 
            // infoAverageLabel
            // 
            this.infoAverageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif",
                11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.infoAverageLabel.ForeColor = System.Drawing.Color.Red;
            this.infoAverageLabel.Location = new System.Drawing.Point(beginX, 10);
            this.infoAverageLabel.Margin = new System.Windows.Forms.Padding(4);
            this.infoAverageLabel.Name = "infoAverageLabel";
            this.infoAverageLabel.Size = new System.Drawing.Size(170, 29);
            this.infoAverageLabel.AutoSize = true;
            this.infoAverageLabel.TabIndex = 2;
            this.infoAverageLabel.Text = "卖出基价: ";
            this.infoAverageLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;

            beginX += this.infoAverageLabel.Width +avg;
            // 
            // radLblAverage
            // 
            this.radLblAverage.Font = new System.Drawing.Font("Microsoft Sans Serif", 
                18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radLblAverage.Location = new System.Drawing.Point(beginX, 2);
            this.radLblAverage.Margin = new System.Windows.Forms.Padding(4);
            this.radLblAverage.Name = "radLblAverage";
            this.radLblAverage.AutoSize = true;
            this.radLblAverage.Size = new System.Drawing.Size(118, 26);
            this.radLblAverage.TabIndex = 1;
            this.radLblAverage.Text = "50000";

            // 
            // Form2
            // 
            //this.Controls.Add(this.radGridView1);
            //this.Controls.Add(this.radGridView2);
            this.Controls.Add(this.radPanel1);
            this.Controls.Add(this.radPanel2);
            this.Controls.Add(this.radPanel3);
            this.SizeChanged +=new System.EventHandler( Form2_SizeChanged);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1200, 993);
            ((System.ComponentModel.ISupportInitialize)(this.infoCountLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoAverageLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).EndInit();
            this.radPanel2.ResumeLayout(false);
            this.radPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel3)).EndInit();
            this.radPanel3.ResumeLayout(false);
            this.radPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLblAverage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLblRefreshCount)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadPanel radPanel2;
        private Telerik.WinControls.UI.RadPanel radPanel3;
        private Telerik.WinControls.UI.RadLabel infoCountLabel;
        private Telerik.WinControls.UI.RadLabel infoAverageLabel;
        private Telerik.WinControls.UI.RadLabel radLblRefreshCount;
        private Telerik.WinControls.UI.RadLabel radLblAverage;

        private Telerik.WinControls.UI.RadButton rb;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadGridView radGridView2;
        private Telerik.WinControls.UI.RadDropDownButton radDropDownButton1;
    }
}