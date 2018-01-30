using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseOnlineTradingSystem
{
    public partial class CreateContractForm : Form
    {
        /// <summary>
        /// 构造函数控件初始化
        /// </summary>
        public CreateContractForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化成交数据-合同数据
        /// </summary>
        /// <param name=""></param>
        public void InitialiazeContractInfo(Transaction tradedInfo)
        {


            //填充空控件
        }

        /// <summary>
        /// Http查询出，构建出合同信息
        /// </summary>
        /// <param name="tradedInfo"></param>
        /// <returns></returns>
        private object CreateContractInfo(Transaction tradedInfo)
        {
            return null;
        }

        /// <summary>
        /// 控件填充
        /// </summary>
        /// <param name="tradedInfo"></param>
        /// <returns></returns>
        private object FillData(Transaction tradedInfo)
        {
            return null;
        }

        /// <summary>
        /// 保存为PDF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Save_Click(object sender, EventArgs e)
        {

            Stream myStream;
            SaveFileDialog saveFileDialogContract = new SaveFileDialog();

            saveFileDialogContract.Filter = "txt files   (*.pdf)|*.pdf";
            saveFileDialogContract.FilterIndex = 2;
            saveFileDialogContract.RestoreDirectory = true;

            if (saveFileDialogContract.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialogContract.OpenFile()) != null)
                {
                    using (StreamWriter sw = new StreamWriter(myStream))
                    {
                        sw.Write("this is the text");
                    }

                    myStream.Close();
                }
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
