using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;

namespace TempUtilityForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('GC', 'COMEX', 'CMX金', 'COMEX金', 100, 0.1);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('HG', 'COMEX', 'CMX铜', 'COMEX铜', 25000, 0.0005);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('SI', 'COMEX', 'CMX银', 'COMEX银', 5000, 0.005);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('CAD', 'LME', 'LME铜', 'LME铜', 25, 0.5);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('ZSD', 'LME', 'LME锌', 'LME锌', 25, 0.5);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('AHD', 'LME', 'LME铝', 'LME铝', 25, 0.5);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('NID', 'LME', 'LME镍', 'LME镍', 6, 5);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('SND', 'LME', 'LME锡', 'LME锡', 5,5);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('PBD', 'LME', 'LME铅', 'LME铅', 25, 0.5);

            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('TRB', 'TOCOM', 'TOCOM橡胶', 'TOCOM橡胶', 5, 0.1);
            //insert into alpha.varieties(code, exchange, short_name, long_name, volume_multiple, price_tick) values('TF', 'SGX', '标胶20', '标胶20', 5, 0.1);


            //MaketDataPlayer player = new MaketDataPlayer();
            //player.Initialize(@"C:\Users\Administrator\Desktop\新建文件夹\1.txt");

            //while(true)
            //{
            //    USeMarketData data = player.GetNextByVarieties("cu");
            //    System.Diagnostics.Debug.WriteLine(data.Instrument+","+data.UpdateTime);
            //}

            //string path = @"C:\Users\Administrator\Desktop\20140101-20170804-kline";
            //string dbConStr = ConfigurationManager.ConnectionStrings["MarketDataDB"].ConnectionString;
            //string alphaDBName = ConfigurationManager.AppSettings["AlphaDBName"];

            //HistoryReader reader = new HistoryReader(dbConStr, alphaDBName);

            //reader.OnProcessDataEvent += Reader_OnProcessDataEvent;

            //reader.Start(path);

            //ContractsReader reader = new ContractsReader();
            //reader.StartProcess(path);
        }


        //public delegate void delegateReaderDataProcess(int successCout, int fileList, string path);

        private void Reader_OnProcessDataEvent(string processType,int successCout, int fileList, string path)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string,int, int, string>(delegateReaderDataProcess), processType, successCout, fileList, path);
                return;
            }

        }

        public void delegateReaderDataProcess(string processType,int successCout, int fileList, string path)
        {
            if(processType == "Outer")
            {
                this.label1.Text = string.Format("文件跟踪：正在处理的文件数第:{0}个,共{1}个文件，文件名:{2}", successCout, fileList, path);
            }

            this.label_5.Text = string.Format("文件内部跟踪：该文件共{0}个条目,目前:{1},正在处理的文件:{2},", fileList,successCout,path);
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            try
            {
                //C:\Users\Administrator\Desktop\20140101-20170804-kline
                //Server=211.152.46.43;Port=3306;Database=alpha;Uid=deploy;Pwd=9i[1sF&#2>nBo!*z
                // "Server=172.16.88.140;Port=20306;Database=alpha;Uid=exingcai;Pwd=uscj!@#"
                //alpha



                HistoryReader reader = new HistoryReader(this.textBox_DBConStr.Text, this.textBox_DbName.Text);

                reader.OnProcessDataEvent += Reader_OnProcessDataEvent;

                reader.Start(this.textBox_DicPath.Text);
            }
            catch (Exception ex)
            {
                throw new Exception("录入数据发生异常:" + ex.Message);
            }

        }

        private void Form_Load(object sender, EventArgs e)
        {
            this.textBox_DicPath.Text = @"C:\Users\Administrator\Desktop\20140101-20170804-kline";
            this.textBox_DBConStr.Text = "Server=172.16.88.140;Port=20306;Database=alpha;Uid=exingcai;Pwd=uscj!@#";
            this.textBox_DbName.Text = "alpha";
        }
    }
}
