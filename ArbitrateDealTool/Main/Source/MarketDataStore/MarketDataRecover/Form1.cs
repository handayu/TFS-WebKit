using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;

namespace MarketDataRecover
{
    public partial class Form1 : Form
    {

        private delegate void ImportProgressEventHandle(int finishCount, int errorCount, int totalCount, string message);
        private ImportProgressEventHandle OnImportProgress;

        private ImportServices m_importWork = null; //后台任务用于导入数据

        /// <summary>
        /// 初始化
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Load(object sender, EventArgs e)
        {
            string dbConStr = ConfigurationManager.ConnectionStrings["MarketDataDB"].ConnectionString;
            string alphaDBName = ConfigurationManager.AppSettings["AlphaDBName"];

            m_importWork = new ImportServices(dbConStr, alphaDBName);
            m_importWork.OnImportProgress += M_importWork_OnImportProgress;
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_OpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.textBox_Path.Text = this.folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 启动后台任务开始导入文件夹下的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Inport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox_Path.Text))
            {
                MessageBox.Show("请选择读取文件的路径");
                return;
            }

            try
            {
                m_importWork.Start(this.textBox_Path.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void M_importWork_OnImportProgress(int finishCount, int errorCount, int totalCount, string message)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(OnImportProgress);
                return;
            }

            //this.label_ItemNum.Text = string.Format("{0}/{1}", finishCount, totalCount);
            //this.label_InstrumentDayMinType
            //this.listView_Info.Items.Add(null);
        }
    }
}
