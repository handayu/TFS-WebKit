using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using CTPAPI;

using USe.CtpOrderQuerier;
using USe.CtpOrderQuerier.Configuration;

using USe.TradeDriver.Ctp;
using USe.TradeDriver.Common;

namespace ContractIndexTestForm
{
    public partial class Form1 : Form
    {
        private CtpQuoteDriver m_quoteDriver = null;
        private BindingList<MarketDataViewModel> m_dataSource = new BindingList<MarketDataViewModel>();
        private MarketDataViewModel m_indexModel = null;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 读取配置信息。
        /// </summary>
        /// <returns></returns>
        private CtpOrderDriverSection ReadConfig()
        {
            try
            {
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                Debug.Assert(!string.IsNullOrEmpty(exePath));

                Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
                if (!config.HasFile)
                {
                    throw new ApplicationException("Not found the specific configuration file.");
                }
                CtpOrderDriverSection ctpSection = config.GetSection("CtpOrderDriver") as CtpOrderDriverSection;
                return ctpSection;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private Dictionary<string, List<USeInstrument>> QueryAllInstrument()
        {
            string text = string.Empty;

            CtpOrderQuerier ctpApp = new CtpOrderQuerier();
            try
            {
                CtpOrderDriverSection section = ReadConfig();
                ctpApp.Connect(section.CtpOrderDriver.Address, section.CtpOrderDriver.Port,
                       section.CtpOrderDriver.LoginTimeOut, section.CtpOrderDriver.QueryTimeOut);

                ctpApp.Login(section.CtpAccount.ID, section.CtpAccount.Password, section.CtpAccount.BrokerID);

                List<InstrumentField> fileldList = ctpApp.QueryInstument();

                Dictionary<string, List<USeInstrument>> dic = new Dictionary<string, List<USeInstrument>>();
                foreach(InstrumentField field in fileldList)
                {
                    if (field.ProductClass != ProductClass.Futures)
                    {
                        continue;
                    }

                    List<USeInstrument> instrumentList = null;
                    if(dic.TryGetValue(field.ProductID,out instrumentList) == false)
                    {
                        instrumentList = new List<USeInstrument>();
                        dic.Add(field.ProductID, instrumentList);
                    }
                    USeInstrument instrument = new USeInstrument(field.InstrumentID, field.InstrumentID, USeMarket.SHFE);
                    instrumentList.Add(instrument);
                }

                ctpApp.Disconnect();

                return dic;


            }
            catch (Exception ex)
            {
                ctpApp.Disconnect();
                throw ex;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = m_dataSource;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<USeInstrument>> productDic = QueryAllInstrument();
            string varieties = this.txtVarieties.Text;
            if(productDic.ContainsKey(varieties) == false)
            {
                MessageBox.Show(string.Format("品种{0}不存在", varieties));
                return;
            }

            m_dataSource.Clear();
            List<USeInstrument> subList = productDic[varieties];
            foreach (USeInstrument instrument in subList)
            {
                MarketDataViewModel marketModel = new MarketDataViewModel(instrument, false);
                m_dataSource.Add(marketModel);
            }

            USeInstrument indexInstrument = new USeInstrument(varieties + "88888", "指数", USeMarket.SHFE);
            MarketDataViewModel indexModel = new MarketDataViewModel(indexInstrument, true);
            m_dataSource.Add(indexModel);
            m_indexModel = indexModel;

            string address = "180.166.11.33";
            int port = 41213;
            string brokerId = "4200";
            string account = "13100110";
            string password = "135246";

            CtpQuoteDriver quoteDriver = new CtpQuoteDriver(address,port);
            quoteDriver.OnMarketDataChanged += QuoteDriver_OnMarketDataChanged;
            quoteDriver.ConnectServer();
            quoteDriver.Login(brokerId, account, password);
            quoteDriver.Subscribe(subList);

            m_quoteDriver = quoteDriver;
        }

        private void QuoteDriver_OnMarketDataChanged(object sender, USe.TradeDriver.Common.USeMarketDataChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeMarketDataChangedEventArgs>(QuoteDriver_OnMarketDataChanged), sender, e);
                return;
            }

            MarketDataViewModel marketModel = m_dataSource.FirstOrDefault(p => p.Instrument == e.MarketData.Instrument);
            if (marketModel != null)
            {
                marketModel.Update(e.MarketData);
                CalculateIndex();
            }
        }

        private void CalculateIndex()
        {
            string indexCode = this.txtVarieties.Text + "8888";

            decimal totalPreOpenInterest = 0;
            int count = 0;
            foreach (MarketDataViewModel model in m_dataSource)
            {
                if (model.IsIndex) continue;

                //totalPreOpenInterest += model.PreOpenInterest;
                totalPreOpenInterest += model.OpenInterest;
                count++;
            }

            Dictionary<string, decimal> weightDic = GetWeight();


           decimal openPrice = 0m;
            decimal highPrice = 0m;
            decimal lowPrice = 0m;
            decimal lastPrice = 0m;
            decimal openInterest = 0m;
            decimal preSettlementPrice = 0m;
            int volumn = 0;
            foreach (MarketDataViewModel model in m_dataSource)
            {
                if (model.IsIndex) continue;

                decimal weight = 0m;
                //weightDic.TryGetValue(model.Instrument.InstrumentCode, out weight);
                //weight = model.PreOpenInterest / totalPreOpenInterest;
                weight = model.OpenInterest / totalPreOpenInterest;
                if (totalPreOpenInterest > 0)
                {
                    openPrice += model.OpenPrice * weight;
                    highPrice += model.HighPrice * weight;
                    lowPrice += model.LowPrice * weight;
                    lastPrice += model.LastPrice * weight;
                    preSettlementPrice += model.PreSettlementPrice * weight;
                }

                openInterest += model.OpenInterest;
                volumn += model.Volume;
            }

            if(m_indexModel != null)
            {
                m_indexModel.PreOpenInterest = totalPreOpenInterest;
                m_indexModel.OpenInterest = openInterest;
                m_indexModel.OpenPrice = Convert.ToInt32(openPrice);
                m_indexModel.HighPrice = Convert.ToInt32(highPrice);
                m_indexModel.LowPrice = Convert.ToInt32(lowPrice);
                m_indexModel.LastPrice = Convert.ToInt32(lastPrice);
                m_indexModel.PreSettlementPrice = Convert.ToInt32(preSettlementPrice);
                m_indexModel.Volume = volumn;
            }
        }

        private Dictionary<string, decimal> GetWeight()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>(); // 成交量
            dic.Add("cu1707", 17180);
            dic.Add("cu1708", 58216);
            dic.Add("cu1709", 123504);
            dic.Add("cu1710", 23062);
            dic.Add("cu1711", 2452);
            dic.Add("cu1712", 494);
            dic.Add("cu1801", 292);
            dic.Add("cu1802", 164);
            dic.Add("cu1803", 318);
            dic.Add("cu1804", 468);
            dic.Add("cu1805", 402);
            dic.Add("cu1806", 602);

            Dictionary<string, int> dic2 = new Dictionary<string, int>();  // 持仓量
            dic2.Add("cu1707", 35860);
            dic2.Add("cu1708", 130588);
            dic2.Add("cu1709", 202196);
            dic2.Add("cu1710", 81910);
            dic2.Add("cu1711", 30172);
            dic2.Add("cu1712", 26292);
            dic2.Add("cu1801", 27030);
            dic2.Add("cu1802", 16302);
            dic2.Add("cu1803", 12174);
            dic2.Add("cu1804", 11598);
            dic2.Add("cu1805", 11562);
            dic2.Add("cu1806", 12254);

            Dictionary<string, int> dic3 = new Dictionary<string, int>(); // 成交+持仓
            dic3.Add("cu1707", 17180 + 35860);
            dic3.Add("cu1708", 58216 + 130588);
            dic3.Add("cu1709", 123504 + 202196);
            dic3.Add("cu1710", 23062 + 81910);
            dic3.Add("cu1711", 2452 + 30172);
            dic3.Add("cu1712", 494 + 26292);
            dic3.Add("cu1801", 292 + 27030);
            dic3.Add("cu1802", 164 + 16302);
            dic3.Add("cu1803", 318 + 12174);
            dic3.Add("cu1804", 468 + 11598);
            dic3.Add("cu1805", 402 + 11562);
            dic3.Add("cu1806", 602 + 12254);

            Dictionary<string, decimal> weightDic1 = null;
            {
                Dictionary<string, int> calcDic = dic;
                decimal totoalWeight = 0m;
                foreach (KeyValuePair<string, int> item in calcDic)
                {
                    totoalWeight += item.Value;
                }

                Dictionary<string, decimal> weightDic = new Dictionary<string, decimal>();
                foreach (KeyValuePair<string, int> item in calcDic)
                {
                    weightDic.Add(item.Key, item.Value / totoalWeight);
                }
                weightDic1 = weightDic;
            }
            Dictionary<string, decimal> weightDic2 = null;
            {
                Dictionary<string, int> calcDic = dic2;
                decimal totoalWeight = 0m;
                foreach (KeyValuePair<string, int> item in calcDic)
                {
                    totoalWeight += item.Value;
                }

                Dictionary<string, decimal> weightDic = new Dictionary<string, decimal>();
                foreach (KeyValuePair<string, int> item in calcDic)
                {
                    weightDic.Add(item.Key, item.Value / totoalWeight);
                }
                weightDic2 = weightDic;
            }

            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            foreach (KeyValuePair<string, decimal> item in weightDic1)
            {
                result.Add(item.Key, (item.Value + weightDic2[item.Key]) / 2);

            }
            return result;

        }
    }
}
