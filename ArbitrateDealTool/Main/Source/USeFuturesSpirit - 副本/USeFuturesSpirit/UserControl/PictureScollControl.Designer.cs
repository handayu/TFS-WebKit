namespace USeFuturesSpirit
{
    partial class PictureScollControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Image = global::USeFuturesSpirit.Properties.Resources.scoll11;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 62);
            this.label1.TabIndex = 0;
            // 
            // PictureScollControl
            // 
            this.Controls.Add(this.label1);
            this.Name = "PictureScollControl";
            this.Size = new System.Drawing.Size(194, 67);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label_pic;
        private System.Windows.Forms.Label label1;
    }
}
