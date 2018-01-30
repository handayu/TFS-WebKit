using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace USeFuturesSpirit
{
    public partial class PictureScollControl : UserControl
    {
        public PictureScollControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载静态图初始化
        /// </summary>
        private void InitializePicture()
        {
            Image image = global::USeFuturesSpirit.Properties.Resources.scoll10;
            if (image == null) return;
            this.label1.Image = global::USeFuturesSpirit.Properties.Resources.scoll10;
        }

        /// <summary>
        /// 加载动态图
        /// </summary>
        public void LoadScollPicture()
        {
            Image image = global::USeFuturesSpirit.Properties.Resources.scoll8;
            if (image == null) return;
            this.label1.Image = global::USeFuturesSpirit.Properties.Resources.scoll8;
        }

        private void PicetureScoll_Load(object sender, EventArgs e)
        {
            InitializePicture();
        }
    }
}
