namespace TempUtilityForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_DicPath = new System.Windows.Forms.TextBox();
            this.textBox_DbName = new System.Windows.Forms.TextBox();
            this.textBox_DBConStr = new System.Windows.Forms.TextBox();
            this.button_Start = new System.Windows.Forms.Button();
            this.label_5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 233);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 38);
            this.button1.TabIndex = 0;
            this.button1.Text = "外盘品种SQL";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(33, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "文件跟踪：正在处理的文件数第:---- 个,共 --- 个文件，文件名: ---- ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "DictoryPath:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "DBConnectStr:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(82, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "DBName:";
            // 
            // textBox_DicPath
            // 
            this.textBox_DicPath.Location = new System.Drawing.Point(151, 26);
            this.textBox_DicPath.Name = "textBox_DicPath";
            this.textBox_DicPath.Size = new System.Drawing.Size(498, 25);
            this.textBox_DicPath.TabIndex = 5;
            // 
            // textBox_DbName
            // 
            this.textBox_DbName.Location = new System.Drawing.Point(151, 94);
            this.textBox_DbName.Name = "textBox_DbName";
            this.textBox_DbName.Size = new System.Drawing.Size(498, 25);
            this.textBox_DbName.TabIndex = 6;
            // 
            // textBox_DBConStr
            // 
            this.textBox_DBConStr.Location = new System.Drawing.Point(151, 61);
            this.textBox_DBConStr.Name = "textBox_DBConStr";
            this.textBox_DBConStr.Size = new System.Drawing.Size(498, 25);
            this.textBox_DBConStr.TabIndex = 7;
            // 
            // button_Start
            // 
            this.button_Start.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Start.Location = new System.Drawing.Point(534, 219);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(115, 35);
            this.button_Start.TabIndex = 8;
            this.button_Start.Text = "开始录入";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // label_5
            // 
            this.label_5.AutoSize = true;
            this.label_5.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_5.ForeColor = System.Drawing.Color.Red;
            this.label_5.Location = new System.Drawing.Point(33, 196);
            this.label_5.Name = "label_5";
            this.label_5.Size = new System.Drawing.Size(512, 20);
            this.label_5.TabIndex = 9;
            this.label_5.Text = "文件内部处理跟踪：正在处理的文件数第:---- 个,共 --- 个文件，文件名: ---- ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 292);
            this.Controls.Add(this.label_5);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.textBox_DBConStr);
            this.Controls.Add(this.textBox_DbName);
            this.Controls.Add(this.textBox_DicPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "数据库数据导入";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_DicPath;
        private System.Windows.Forms.TextBox textBox_DbName;
        private System.Windows.Forms.TextBox textBox_DBConStr;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Label label_5;
    }
}

