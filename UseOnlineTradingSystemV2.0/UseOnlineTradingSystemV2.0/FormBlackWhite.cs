using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UseOnlineTradingSystem.Properties;

namespace UseOnlineTradingSystem
{
    public partial class FormBlackWhite : Form
    {
        Bitmap bit;
        public FormBlackWhite()
        {
            InitializeComponent();
        }

        private void Form5_MouseLeave(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            bit = new Bitmap(Resources.tips.Width / 2-8, Resources.tips.Height / 2-10);
            Graphics g = Graphics.FromImage(bit);
            g.DrawImage(Resources.tips, -4, -3, Resources.tips.Width / 2, Resources.tips.Height / 2);
            this.Size = bit.Size;
            this.BackgroundImage = bit;
        }
    }
}
