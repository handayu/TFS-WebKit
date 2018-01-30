namespace USeFuturesSpirit
{
    partial class TestFormPicture
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
            this.pictureScollControl1 = new USeFuturesSpirit.PictureScollControl();
            this.button_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pictureScollControl1
            // 
            this.pictureScollControl1.Location = new System.Drawing.Point(70, 78);
            this.pictureScollControl1.Name = "pictureScollControl1";
            this.pictureScollControl1.Size = new System.Drawing.Size(184, 59);
            this.pictureScollControl1.TabIndex = 0;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(108, 174);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(104, 48);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "加载";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // TestFormPicture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 310);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.pictureScollControl1);
            this.Name = "TestFormPicture";
            this.Text = "TestFormPicture";
            this.Move += new System.EventHandler(this.Form_Move);
            this.ResumeLayout(false);

        }

        #endregion

        private PictureScollControl pictureScollControl1;
        private System.Windows.Forms.Button button_ok;
    }
}