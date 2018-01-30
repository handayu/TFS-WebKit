using System.Windows.Forms;
using System;

namespace USeTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //品种数据库更新SQL语句生成
            //try
            //{
            //    List<USeProduct> list = USeManager.Instance.OrderDriver.QueryProducts();

            //    StringBuilder sb = new StringBuilder();
            //    foreach (USeProduct item in list)
            //    {
            //        string sql = string.Format("update alpha.varieties set long_name = '{0}'," +
            //            "volume_multiple= {1},price_tick={2}" +
            //            " where code = '{3}' and exchange='{4}';",
            //            item.LongName, item.VolumeMultiple, item.PriceTick,
            //            item.ProductCode, item.Market.ToString());
            //        sb.AppendLine(sql);
            //    }

            //    string str = sb.ToString();
            //    string s = "";

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            DateTime dateTime = dateTimePicker1.Value;
            string str = dateTime.ToString("yyyyMMdd");
        }
    }
}