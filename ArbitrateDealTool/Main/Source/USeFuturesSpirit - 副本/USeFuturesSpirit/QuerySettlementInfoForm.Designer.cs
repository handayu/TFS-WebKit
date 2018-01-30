namespace USeFuturesSpirit
{
    partial class QuerySettlementInfoForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker_Start = new System.Windows.Forms.DateTimePicker();
            this.rtxtSettlementInfo = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "日期:";
            // 
            // dateTimePicker_Start
            // 
            this.dateTimePicker_Start.Location = new System.Drawing.Point(94, 15);
            this.dateTimePicker_Start.Name = "dateTimePicker_Start";
            this.dateTimePicker_Start.Size = new System.Drawing.Size(200, 25);
            this.dateTimePicker_Start.TabIndex = 2;
            // 
            // rtxtSettlementInfo
            // 
            this.rtxtSettlementInfo.Location = new System.Drawing.Point(16, 59);
            this.rtxtSettlementInfo.Name = "rtxtSettlementInfo";
            this.rtxtSettlementInfo.Size = new System.Drawing.Size(1057, 361);
            this.rtxtSettlementInfo.TabIndex = 4;
            this.rtxtSettlementInfo.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(346, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "查询";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // QuerySettlementInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 432);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rtxtSettlementInfo);
            this.Controls.Add(this.dateTimePicker_Start);
            this.Controls.Add(this.label1);
            this.Name = "QuerySettlementInfoForm";
            this.Text = "查询结算单";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Start;
        private System.Windows.Forms.RichTextBox rtxtSettlementInfo;
        private System.Windows.Forms.Button button1;
    }
}