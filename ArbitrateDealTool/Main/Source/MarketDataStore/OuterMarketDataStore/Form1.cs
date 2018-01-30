using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.Common;
using USe.TradeDriver.Common;

namespace OuterMarketDataStore
{
    public partial class Form1 : Form
    {
        private BindingList<MQTTStoreageViewModel> m_MqttStoreStateList = new BindingList<MQTTStoreageViewModel>();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            InitilizeComboxInstrument();

            this.gridMQTTSend.AutoGenerateColumns = false;
            this.gridMQTTSend.DataSource = m_MqttStoreStateList;

            try
            {
                USeManager.Instance.Initialize();
                USeManager.Instance.Start();
                InitializeMQTTStore();
                InitializeMySqlStore();

            }
            catch (Exception ex)
            {
                throw new Exception("Form_Load 发生异常:" + ex.Message);
            }

            USeManager.Instance.OutLMEMarketDataReceiveEvent += HttpDataReceiver_OutLMEMarketDataReceiveEvent;
            USeManager.Instance.Notify += Instance_Notify;

        }


        /// <summary>
        /// 初始化MQTT显示
        /// </summary>
        private void InitializeMQTTStore()
        {
            List<MQTTMarketDataStoreage> storeageList = USeManager.Instance.MQTTMArketDataStoreageList;
            if (storeageList != null)
            {
                foreach (MQTTMarketDataStoreage storeage in storeageList)
                {
                    MQTTStoreageViewModel model = new MQTTStoreageViewModel();
                    model.Name = storeage.StoreageName;
                    m_MqttStoreStateList.Add(model);
                }
            }

        }

        /// <summary>
        /// 初始化数据库mySql显示
        /// </summary>
        private void InitializeMySqlStore()
        {
            List<MySqlKLineStoreage> storeageList = USeManager.Instance.MySqlStoreageList;
            if (storeageList != null)
            {
                foreach (MySqlKLineStoreage storeage in storeageList)
                {
                    MQTTStoreageViewModel model = new MQTTStoreageViewModel();
                    model.Name = storeage.StoreageName;
                    m_MqttStoreStateList.Add(model);
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                showMQTTRealMarketDataStoreInfo();
                showMySqlRealStoreInfo();
                showOutMarketDataReceiveInfo();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void showMySqlRealStoreInfo()
        {
            List<MySqlKLineStoreage>  mySqlMarketDataStoreageList = USeManager.Instance.MySqlStoreageList;
            foreach (MySqlKLineStoreage storage in mySqlMarketDataStoreageList)
            {
                MQTTStoreageViewModel model = GetMQTTViewModel(storage.StoreageName);
                if (model != null)
                {
                    model.StoreCount = storage.StoreCount;
                    model.UnstoreCount = storage.UnStoreCount;
                    model.ErrorStoreCount = storage.ErrorStoreCount;
                }
            }
        }

        private void showOutMarketDataReceiveInfo()
        {
            this.textBox_ReceiveItems.Text = USeManager.Instance.HttpDataReceiver.ReceiveCount.ToString();
        }

        private void showMQTTRealMarketDataStoreInfo()
        {
            List<MQTTMarketDataStoreage> mqttMarketDataStoreageList = USeManager.Instance.MQTTMArketDataStoreageList;
            foreach (MQTTMarketDataStoreage storage in mqttMarketDataStoreageList)
            {
                MQTTStoreageViewModel model = GetMQTTViewModel(storage.StoreageName);
                if (model != null)
                {
                    model.StoreCount = storage.StoreCount;
                    model.UnstoreCount = storage.UnStoreCount;
                    model.ErrorStoreCount = storage.ErrorStoreCount;
                }
            }
        }

        private MQTTStoreageViewModel GetMQTTViewModel(string storageName)
        {
            foreach (MQTTStoreageViewModel model in m_MqttStoreStateList)
            {
                if (model.Name == storageName)
                {
                    return model;
                }
            }
            return null;
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Instance_Notify(object sender, USe.Common.USeNotifyEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new EventHandler<USeNotifyEventArgs>(Instance_Notify), sender, e);
                    return;
                }

                this.richTextBox_Log.AppendText(e.Message + '\n');
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 行情接收显示
        /// </summary>
        /// <param name="marketData"></param>
        private void HttpDataReceiver_OutLMEMarketDataReceiveEvent(USeMarketData marketData)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action<USeMarketData>(HttpDataReceiver_OutLMEMarketDataReceiveEvent),marketData);
                    return;
                }

                string text = string.Format("{0} {1} {2}", marketData.UpdateTime, marketData.Instrument.InstrumentCode, marketData.ClosePrice);

                this.richTextBox_RealData.AppendText(text + '\n');
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //private void richTextBox1_TextChanged(object sender, EventArgs e)
        //{

        //}

        private void InitilizeComboxInstrument()
        {
            this.comboBox_Market.Items.Add("LME");
            this.comboBox_Market.SelectedIndex = 0;

            this.textBox_ReceiveItems.ReadOnly = true;
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            USeManager.Instance.HttpDataReceiver.Stop();
        }
    }
}
