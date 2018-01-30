using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public partial class QualityAlalisyForm : Form
    {
        private Dictionary<string, decimal> m_dateQuality = null;
        public QualityAlalisyForm()
        {
            InitializeComponent();
            m_dateQuality = new Dictionary<string, decimal>();

        }

        private USeOrderDriver m_orderDriver = null;


        public QualityAlalisyForm(USeOrderDriver orderDriver)
        {
            m_orderDriver = orderDriver;
            m_dateQuality = new Dictionary<string, decimal>();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("开始查询日期不能超过今天请重新输入");
                return;
            }

            DateTime dateTime = new DateTime(this.dateTimePicker1.Value.Year, this.dateTimePicker1.Value.Month,this.dateTimePicker1.Value.Day, 0, 0, 0);

            for(DateTime dateTimeTemp = dateTime; dateTimeTemp <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0); dateTimeTemp = dateTimeTemp.AddDays(1))
            {
                try
                {
                    string tradingDateStr = dateTimeTemp.ToString("yyyyMMdd");
                    string settlementInfo = m_orderDriver.GetSettlementInfo(tradingDateStr);
                    if (settlementInfo.Equals("")) continue;

                    string[] strInfoTemp = settlementInfo.Split('\n');
                    string custormQualityInfo = strInfoTemp[15];
                    string cusSpilt = "";
                    Debug.Assert(custormQualityInfo.Count() > 0);
                    foreach(char c in custormQualityInfo)
                    {
                        if (c == ' ' || c == '\r') continue;
                        cusSpilt = cusSpilt + c;
                    }

                    string qualityStr = "";

                    for(int i = cusSpilt.Count()-1; i>=0;i--)
                    {
                        if ((cusSpilt[i] >= '0' && cusSpilt[i] <= '9')|| cusSpilt[i] == '.')
                        {
                            qualityStr = qualityStr.Insert(0, cusSpilt[i].ToString());
                        }
                        else
                        {
                            break;
                        }
                    }

                    //弄出资金的值
                    m_dateQuality[tradingDateStr] = Convert.ToDecimal(qualityStr);

                    this.label_settlementInfo.Text = string.Format("**正在从保证金监控中心下载结算单信息，交易日:{0}",tradingDateStr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("读取结算单信息失败," + ex.Message);
                }
            }

            //m_dateQuality["20170901"] = 11233;
            //m_dateQuality["20170902"] = 11236;
            //m_dateQuality["20170903"] = 12433;
            //m_dateQuality["20170904"] = 15677;
            //m_dateQuality["20170905"] = 13455;
            //m_dateQuality["20170906"] = 12677;


            //以上汇总完日起范围的结算信息，然后显示;
            PrintSeriesQuality(m_dateQuality);
        }

        private void PrintSeriesQuality(Dictionary<string, decimal> m_dateQuality)
        {
            Debug.Assert(m_dateQuality != null);
            if (m_dateQuality.Count <= 0) return;

            KeyValuePair<string, decimal> kvFirst = m_dateQuality.First();
            string dateTimeFirst = kvFirst.Key;
            decimal qualityFirst = kvFirst.Value;

            if(kvFirst.Value == 0m)
            {
                MessageBox.Show("起始的资金为0，请重新拉去最初的起始资金以便于计算资金之后的净值");
                return;
            }

            foreach(KeyValuePair<string,decimal> kv in m_dateQuality)
            {
                string dateTime = kv.Key;
                decimal quality = kv.Value/qualityFirst;

                //加到曲线上
                this.chart1.Series[0].Points.AddXY(dateTime, quality);
                this.chart1.Series[0].Points.AddY(quality);

            }
        }

        private void Form_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
