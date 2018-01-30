using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;
using USe.Common;
using USe.TradeDriver;
using USe.TradeDriver.Ctp;
using CTPAPI;
using System.Configuration;
using System.Diagnostics;
using DataStoreCommon;

namespace MarketDataStore
{
    public partial class MainForm : Form
    {
        private BindingList<KLineStoreStateViewModel> m_klineStoreState = new BindingList<KLineStoreStateViewModel>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ClearView();
            this.errorNoticeViewControl1.Initialize();
            ShowMonitorMarket();
            InitializeKLineStoer();
            InitializeMQTTStore();
            try
            {
                USeManager.Instance.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                //this.Close();
            }

            this.Text = "KLine数据存储 @" + USeManager.Instance.DayNightType.ToDescription();
        }

        private void InitializeKLineStoer()
        {
            m_klineStoreState.Clear();
            List<KLineStoreage> storeageList = USeManager.Instance.KLineStorages; 
            if(storeageList != null)
            {
                foreach(KLineStoreage storeage in storeageList)
                {
                    KLineStoreStateViewModel model = new KLineStoreStateViewModel();
                    model.Name = storeage.StorageName;
                    m_klineStoreState.Add(model);
                }
            }
            this.gridKLineStore.AutoGenerateColumns = false;
            this.gridKLineStore.DataSource = m_klineStoreState;
        }

        /// <summary>
        /// 初始化MQTT显示
        /// </summary>
        private void InitializeMQTTStore()
        {
            List<MQTTMarketDataStoreage> storeageList = USeManager.Instance.MQTTMarketDataStoreageList;
            if (storeageList != null)
            {
                foreach (MQTTMarketDataStoreage storeage in storeageList)
                {
                    KLineStoreStateViewModel model = new KLineStoreStateViewModel();
                    model.Name = storeage.StoreageName;
                    m_klineStoreState.Add(model);
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                ShowMarketDataReceiverInfo();
                ShowMarketDataProcessorInfo();
                ShowMarketDataStoreInfo();
                ShowKLineStoaresInfo();

                //MQTT发送CTP实时数据
                showMQTTRealMarketDataStoreInfo();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void showMQTTRealMarketDataStoreInfo()
        {
            List<MQTTMarketDataStoreage> mqttMarketDataStoreageList = USeManager.Instance.MQTTMarketDataStoreageList;
            foreach (MQTTMarketDataStoreage storage in mqttMarketDataStoreageList)
            {
                KLineStoreStateViewModel model = GetMQTTViewModel(storage.StoreageName);
                if (model != null)
                {
                    model.StoreCount = storage.StoreCount;
                    model.UnstoreCount = storage.UnStoreCount;
                    model.ErrorStoreCount = storage.ErrorStoreCount;
                }
            }
        }


        private void ShowMonitorMarket()
        {
            string value = string.Join(",", USeManager.Instance.MonitorMarketes);
            this.lblMonitorExchange.Text = value;
        }

        private void ShowMarketDataReceiverInfo()
        {
            CTPMarketDataReceiver receiver = USeManager.Instance.MarketDataReceiver;
            this.lblReceive_InstrumentCount.Text = receiver.InstrumentCount.ToString();
            this.lblReceive_ReceiveCount.Text = receiver.ReceiveCount.ToString();
            this.lblReceive_LatestMarketDataTime.Text = receiver.LastMarketDataTime.HasValue? receiver.LastMarketDataTime.Value.ToString("HH:mm:ss"): "---";
        }

        private void ShowMarketDataProcessorInfo()
        {
            KLineProcessor processor = USeManager.Instance.MarketDataProcessor;
            this.lblProcessor_ProcessCount.Text = processor.ProcessCount.ToString();
        }

        private void ShowMarketDataStoreInfo()
        {
            FileMarketDataStorage store = USeManager.Instance.MarketDataFileStorage;
            this.lblMarketDataFileStorage_StoreCount.Text = store.StoreCount.ToString();
            this.lblMarketDataFileStorage_UnStoreCount.Text = store.UnStoreCount.ToString();
            this.lblMarketDataFileStorage_ErrorStoreCount.Text = store.ErrorStoreCount.ToString();
        }

        private void ShowKLineStoaresInfo()
        {
            List<KLineStoreage> sotreages = USeManager.Instance.KLineStorages;
            foreach (KLineStoreage storage in sotreages)
            {
                KLineStoreStateViewModel model = GetKLineStoreViewModel(storage.StorageName);
                if (model != null)
                {
                    model.StoreCount = storage.StoreCount;
                    model.UnstoreCount = storage.UnStoreCount;
                    model.ErrorStoreCount = storage.ErrorStoreCount;
                }
            }
        }

        private KLineStoreStateViewModel GetKLineStoreViewModel(string storageName)
        {
            foreach (KLineStoreStateViewModel model in m_klineStoreState)
            {
                if (model.Name == storageName)
                {
                    return model;
                }
            }
            return null;
        }

        private KLineStoreStateViewModel GetMQTTViewModel(string storageName)
        {
            foreach (KLineStoreStateViewModel model in m_klineStoreState)
            {
                if (model.Name == storageName)
                {
                    return model;
                }
            }
            return null;
        }


        private void ClearView()
        {
            this.lblReceive_InstrumentCount.Text = "0";
            this.lblReceive_ReceiveCount.Text = "0";
            this.lblReceive_LatestMarketDataTime.Text = "----";

            this.lblProcessor_ProcessCount.Text = "0";

            //this.lblStorer_StoreCount.Text = "0";
            //this.lblStorer_UnStoreCount.Text = "0";
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                USeKLine kline = new USeKLine();
                kline.InstrumentCode = "cu1707";
                kline.Market = USeMarket.CFFEX;
                kline.Cycle = USeCycleType.Min1;
                kline.DateTime = new DateTime(2017, 06, 26, 09, 30, 0);
                kline.Open = 43325;
                kline.High = 43325;
                kline.Low = 43325;
                kline.Close = 43325;
                kline.Volumn = 122222;
                kline.Turnover = 122222.780m;
                kline.OpenInterest = 2345;

                List<KLineStoreage> storeageList = USeManager.Instance.KLineStorages;
                foreach(KLineStoreage storeage in storeageList)
                {
                    if(storeage is FileKLineStoreage)
                    {
                        storeage.ReceiveKLineData(kline);
                    }
                }
                //IKLineDataListener store = USeManager.Instance.RabbitMQStore;
                //store.ReceiveKLineData(kline);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
