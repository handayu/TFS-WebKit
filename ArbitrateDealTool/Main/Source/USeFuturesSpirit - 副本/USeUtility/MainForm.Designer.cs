namespace USeUtility
{
    partial class MainForm
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
            this.btnGenerate = new System.Windows.Forms.Button();
            this.txtAssemblePath = new System.Windows.Forms.TextBox();
            this.txtCodeResult = new System.Windows.Forms.RichTextBox();
            this.cbxClassType = new System.Windows.Forms.ComboBox();
            this.btnLoadAssemble = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(793, 40);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(86, 24);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "创建";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // txtAssemblePath
            // 
            this.txtAssemblePath.Location = new System.Drawing.Point(95, 12);
            this.txtAssemblePath.Name = "txtAssemblePath";
            this.txtAssemblePath.Size = new System.Drawing.Size(692, 22);
            this.txtAssemblePath.TabIndex = 1;
            // 
            // txtCodeResult
            // 
            this.txtCodeResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCodeResult.Location = new System.Drawing.Point(12, 85);
            this.txtCodeResult.Name = "txtCodeResult";
            this.txtCodeResult.Size = new System.Drawing.Size(1082, 306);
            this.txtCodeResult.TabIndex = 2;
            this.txtCodeResult.Text = "";
            // 
            // cbxClassType
            // 
            this.cbxClassType.FormattingEnabled = true;
            this.cbxClassType.Location = new System.Drawing.Point(95, 41);
            this.cbxClassType.Name = "cbxClassType";
            this.cbxClassType.Size = new System.Drawing.Size(692, 24);
            this.cbxClassType.TabIndex = 3;
            // 
            // btnLoadAssemble
            // 
            this.btnLoadAssemble.Location = new System.Drawing.Point(793, 12);
            this.btnLoadAssemble.Name = "btnLoadAssemble";
            this.btnLoadAssemble.Size = new System.Drawing.Size(86, 24);
            this.btnLoadAssemble.TabIndex = 5;
            this.btnLoadAssemble.Text = "加载";
            this.btnLoadAssemble.UseVisualStyleBackColor = true;
            this.btnLoadAssemble.Click += new System.EventHandler(this.btnLoadAssemble_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "程序集文件";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "生成类";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 403);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoadAssemble);
            this.Controls.Add(this.cbxClassType);
            this.Controls.Add(this.txtCodeResult);
            this.Controls.Add(this.txtAssemblePath);
            this.Controls.Add(this.btnGenerate);
            this.Name = "MainForm";
            this.Text = "DataViewModel代码生成";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox txtAssemblePath;
        private System.Windows.Forms.RichTextBox txtCodeResult;
        private System.Windows.Forms.ComboBox cbxClassType;
        private System.Windows.Forms.Button btnLoadAssemble;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

