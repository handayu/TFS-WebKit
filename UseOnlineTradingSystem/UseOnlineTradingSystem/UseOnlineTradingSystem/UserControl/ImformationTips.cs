using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 交易对手描述
    /// </summary>
    public partial class ImformationTips : Form
    {
        public ImformationTips()
        {
            InitializeComponent();
        }

        public void Initialize(Transaction tradedInfo)
        {
            if(tradedInfo == null)
            {
                this.label_CommP.Text = "----";
                this.label_Phone.Text = "----";
            }
            else
            {
                if(tradedInfo.OppoCompName == null)
                {
                    this.label_CommP.Text = "----";
                }
                else
                {
                    this.label_CommP.Text = tradedInfo.OppoCompName;
                }

                if (tradedInfo.oppoPhone == null)
                {
                    this.label_Phone.Text = "----";
                }
                else
                {
                    this.label_Phone.Text = tradedInfo.oppoPhone;
                }
            }

        }
    }
}
