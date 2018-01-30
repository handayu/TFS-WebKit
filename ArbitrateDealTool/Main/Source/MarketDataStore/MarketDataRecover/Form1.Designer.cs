namespace MarketDataRecover
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label_ItemNum = new System.Windows.Forms.Label();
            this.label_InstrumentDayMinType = new System.Windows.Forms.Label();
            this.listView_Info = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_Inport = new System.Windows.Forms.Button();
            this.button_OpenFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Path = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(812, 366);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.listView_Info);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(804, 337);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "K线恢复";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label_ItemNum);
            this.panel2.Controls.Add(this.label_InstrumentDayMinType);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 64);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(798, 38);
            this.panel2.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "已完成";
            // 
            // label_ItemNum
            // 
            this.label_ItemNum.AutoSize = true;
            this.label_ItemNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ItemNum.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label_ItemNum.Location = new System.Drawing.Point(97, 7);
            this.label_ItemNum.Name = "label_ItemNum";
            this.label_ItemNum.Size = new System.Drawing.Size(75, 20);
            this.label_ItemNum.TabIndex = 1;
            this.label_ItemNum.Text = "122/577";
            // 
            // label_InstrumentDayMinType
            // 
            this.label_InstrumentDayMinType.AutoSize = true;
            this.label_InstrumentDayMinType.Location = new System.Drawing.Point(215, 10);
            this.label_InstrumentDayMinType.Name = "label_InstrumentDayMinType";
            this.label_InstrumentDayMinType.Size = new System.Drawing.Size(208, 15);
            this.label_InstrumentDayMinType.TabIndex = 0;
            this.label_InstrumentDayMinType.Text = "导入cu1705 Day K线数据完成";
            // 
            // listView_Info
            // 
            this.listView_Info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_Info.Location = new System.Drawing.Point(1, 108);
            this.listView_Info.Name = "listView_Info";
            this.listView_Info.Size = new System.Drawing.Size(795, 223);
            this.listView_Info.TabIndex = 1;
            this.listView_Info.UseCompatibleStateImageBehavior = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_Inport);
            this.panel1.Controls.Add(this.button_OpenFile);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox_Path);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(798, 61);
            this.panel1.TabIndex = 0;
            // 
            // button_Inport
            // 
            this.button_Inport.Location = new System.Drawing.Point(386, 25);
            this.button_Inport.Name = "button_Inport";
            this.button_Inport.Size = new System.Drawing.Size(75, 22);
            this.button_Inport.TabIndex = 3;
            this.button_Inport.Text = "导入";
            this.button_Inport.UseVisualStyleBackColor = true;
            this.button_Inport.Click += new System.EventHandler(this.button_Inport_Click);
            // 
            // button_OpenFile
            // 
            this.button_OpenFile.Location = new System.Drawing.Point(297, 25);
            this.button_OpenFile.Name = "button_OpenFile";
            this.button_OpenFile.Size = new System.Drawing.Size(75, 22);
            this.button_OpenFile.TabIndex = 2;
            this.button_OpenFile.Text = "浏览...";
            this.button_OpenFile.UseVisualStyleBackColor = true;
            this.button_OpenFile.Click += new System.EventHandler(this.button_OpenFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "K线文件";
            // 
            // textBox_Path
            // 
            this.textBox_Path.Location = new System.Drawing.Point(91, 24);
            this.textBox_Path.Name = "textBox_Path";
            this.textBox_Path.Size = new System.Drawing.Size(200, 25);
            this.textBox_Path.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 366);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Path;
        private System.Windows.Forms.Button button_Inport;
        private System.Windows.Forms.Button button_OpenFile;
        private System.Windows.Forms.ListView listView_Info;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_ItemNum;
        private System.Windows.Forms.Label label_InstrumentDayMinType;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

