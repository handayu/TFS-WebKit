namespace USeFuturesSpirit
{
    partial class ArbitrageQuoteChoiceForm
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
            this.listBox_InsOne = new System.Windows.Forms.ListBox();
            this.listBox_InsTwo = new System.Windows.Forms.ListBox();
            this.button_Add = new System.Windows.Forms.Button();
            this.button_Remove = new System.Windows.Forms.Button();
            this.listBox_ArbitrageIns = new System.Windows.Forms.ListBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.comboBox_Product = new System.Windows.Forms.ComboBox();
            this.Label_product = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_User = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_InsOne
            // 
            this.listBox_InsOne.FormattingEnabled = true;
            this.listBox_InsOne.ItemHeight = 16;
            this.listBox_InsOne.Items.AddRange(new object[] {
            "JR705",
            "JR705",
            "JR705"});
            this.listBox_InsOne.Location = new System.Drawing.Point(24, 26);
            this.listBox_InsOne.Name = "listBox_InsOne";
            this.listBox_InsOne.Size = new System.Drawing.Size(133, 244);
            this.listBox_InsOne.Sorted = true;
            this.listBox_InsOne.TabIndex = 0;
            // 
            // listBox_InsTwo
            // 
            this.listBox_InsTwo.FormattingEnabled = true;
            this.listBox_InsTwo.ItemHeight = 16;
            this.listBox_InsTwo.Items.AddRange(new object[] {
            "JR709",
            "JR709",
            "JR709"});
            this.listBox_InsTwo.Location = new System.Drawing.Point(20, 26);
            this.listBox_InsTwo.Name = "listBox_InsTwo";
            this.listBox_InsTwo.Size = new System.Drawing.Size(138, 244);
            this.listBox_InsTwo.Sorted = true;
            this.listBox_InsTwo.TabIndex = 1;
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(380, 102);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(85, 28);
            this.button_Add.TabIndex = 2;
            this.button_Add.Text = ">>添加";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // button_Remove
            // 
            this.button_Remove.Location = new System.Drawing.Point(380, 229);
            this.button_Remove.Name = "button_Remove";
            this.button_Remove.Size = new System.Drawing.Size(85, 28);
            this.button_Remove.TabIndex = 3;
            this.button_Remove.Text = "<<移除";
            this.button_Remove.UseVisualStyleBackColor = true;
            this.button_Remove.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox_ArbitrageIns
            // 
            this.listBox_ArbitrageIns.FormattingEnabled = true;
            this.listBox_ArbitrageIns.ItemHeight = 16;
            this.listBox_ArbitrageIns.Items.AddRange(new object[] {
            "JR705&JR709"});
            this.listBox_ArbitrageIns.Location = new System.Drawing.Point(494, 78);
            this.listBox_ArbitrageIns.Name = "listBox_ArbitrageIns";
            this.listBox_ArbitrageIns.Size = new System.Drawing.Size(197, 244);
            this.listBox_ArbitrageIns.Sorted = true;
            this.listBox_ArbitrageIns.TabIndex = 4;
            // 
            // button_Ok
            // 
            this.button_Ok.Location = new System.Drawing.Point(535, 354);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(84, 36);
            this.button_Ok.TabIndex = 5;
            this.button_Ok.Text = "确定";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(644, 354);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(84, 36);
            this.button_Cancel.TabIndex = 6;
            this.button_Cancel.Text = "取消";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // comboBox_Product
            // 
            this.comboBox_Product.FormattingEnabled = true;
            this.comboBox_Product.Location = new System.Drawing.Point(68, 12);
            this.comboBox_Product.Name = "comboBox_Product";
            this.comboBox_Product.Size = new System.Drawing.Size(172, 24);
            this.comboBox_Product.TabIndex = 7;
            this.comboBox_Product.SelectedIndexChanged += new System.EventHandler(this.ComBoxProduct_SelectedIndexChanged);
            // 
            // Label_product
            // 
            this.Label_product.AutoSize = true;
            this.Label_product.Location = new System.Drawing.Point(10, 15);
            this.Label_product.Name = "Label_product";
            this.Label_product.Size = new System.Drawing.Size(50, 17);
            this.Label_product.TabIndex = 8;
            this.Label_product.Text = "品种：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox_InsOne);
            this.groupBox1.Location = new System.Drawing.Point(12, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 297);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "近月合约";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBox_InsTwo);
            this.groupBox2.Location = new System.Drawing.Point(196, 51);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(178, 297);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "远月合约";
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(480, 51);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(230, 297);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "组合套利合约";
            // 
            // button_User
            // 
            this.button_User.Location = new System.Drawing.Point(423, 354);
            this.button_User.Name = "button_User";
            this.button_User.Size = new System.Drawing.Size(84, 36);
            this.button_User.TabIndex = 12;
            this.button_User.Text = "应用";
            this.button_User.UseVisualStyleBackColor = true;
            this.button_User.Click += new System.EventHandler(this.button_User_Click);
            // 
            // ArbitrageQuoteChoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 403);
            this.Controls.Add(this.button_User);
            this.Controls.Add(this.Label_product);
            this.Controls.Add(this.comboBox_Product);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.listBox_ArbitrageIns);
            this.Controls.Add(this.button_Remove);
            this.Controls.Add(this.button_Add);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Name = "ArbitrageQuoteChoiceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自选套利合约";
            this.Load += new System.EventHandler(this.ArbitrageQuoteChoiceForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_InsOne;
        private System.Windows.Forms.ListBox listBox_InsTwo;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.Button button_Remove;
        private System.Windows.Forms.ListBox listBox_ArbitrageIns;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.ComboBox comboBox_Product;
        private System.Windows.Forms.Label Label_product;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_User;
    }
}