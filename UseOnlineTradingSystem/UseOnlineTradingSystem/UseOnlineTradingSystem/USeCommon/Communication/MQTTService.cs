using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace UseOnlineTradingSystem
{
    public class MQTTService
    {
        protected MqttClient m_mqttClient = null;        //主机为IP时
        protected string m_mqttServerAddress = string.Empty; //服务器地址
        protected int m_mqttPort = int.MinValue;             //端口
        protected string m_clientId = string.Empty;          //客户端ID
        protected bool m_runFlag = false;
        protected string m_topic;
        public event Action<string> UpdataEvent;
        public event Action<ContractLastPrice> UpdataMarketDataEvent;
        public event Action<WriteOff> NoticeWriteOffEvent;//核销消息

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start(string clientID,string username,string password)
        {
            try
            {
                if (clientID == null)
                {
                    clientID = Guid.NewGuid().ToString();
                }
                m_clientId = clientID;
                //连接订阅
                m_mqttClient = new MqttClient(m_mqttServerAddress, m_mqttPort,false,null);
                m_mqttClient.MqttMsgPublishReceived += M_mqttClient_MqttMsgPublishReceived;
                byte iConnectResult = m_mqttClient.Connect(clientID,username,password);
                if (iConnectResult == 0)
                {
                    m_runFlag = true;
                    if (m_topic != null && m_topic.Length > 0)
                    {
                        m_mqttClient.Subscribe(new string[] { "market" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                        m_mqttClient.Subscribe(new string[] { "trans_list_topic" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                        m_mqttClient.Subscribe(new string[] { "command" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                        m_mqttClient.Subscribe(new string[] { "$client/" + m_clientId }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                    }
                }
            }
            catch (Exception ex)
            {
                m_runFlag = false;
                throw new Exception("启动"+ MQTTServiceName + "发生异常:" + ex.Message);
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            try
            {
                if (m_mqttClient != null || m_mqttClient.IsConnected)
                {
                    m_mqttClient.Disconnect();
                    m_mqttClient.MqttMsgPublishReceived -= M_mqttClient_MqttMsgPublishReceived;
                    m_mqttClient = null;
                    m_runFlag = false;
                }
            }
            catch (Exception ex)
            {
                string text = "关闭MQTTChannel失败," + ex.Message; ;
            }
        }

        /// <summary>
        /// 本MQTT服务名称
        /// </summary>
        /// <returns></returns>
        public  string MQTTServiceName
        {
            get { return m_clientId; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddressPort"></param>
        /// <param name="clientID"></param>
        /// <param name="topicArray"></param>
        public MQTTService(string ipAddress,string port)
        {
            try
            {
                if (string.IsNullOrEmpty(ipAddress))
                {
                    throw new ArgumentNullException("初始化MQTT服务器地址mqttServerAddress为空");
                }
                m_mqttServerAddress = ipAddress;
                int.TryParse(port, out m_mqttPort);
                m_topic = "$client/" + m_clientId;
                //market_topic = Helper.GetConfig(clientID, "QuotesTopic");
                //list_topic = Helper.GetConfig(clientID, "ListTopic");
            }
            catch (Exception ex)
            {
                throw new Exception("初始化MQTT发生异常:" + ex.Message);
            }

        }

        /// <summary>
        /// 订阅主题消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void M_mqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (e.Topic == "market")
            {
                //if (DataManager.Instance.ContractData != null)
                {
                    byte[] bs = e.Message;
                    // 计算出时时间点ID
                    long timeId = byteArrayToLong(bs, 3, 0);
                    // 计算出实际时间戳
                    //long truthTime = DataManager.Instance.ContractData.today9 + timeId * 500;
                    // 品类 转成字符 例：cu1710 al1711
                    string code = Encoding.Default.GetString(bs, 3, 6);
                    var d2 = DataManager.Instance.GetContractLastPrice(code);
                    if (d2 != null)
                    {
                        var d1 = DataManager.Instance.GetContractBasePrice(d2.category,code);
                        if (d1 != null)
                        {
                            //md.timeId = timeId;
                            //md.truthTime = truthTime;
                            //md.category = category;
                            float f;
                            float.TryParse(d1.lowestPrice,out f);
                            // 申买价
                            d2.bidPrice = byteArrayToLong(bs, 3, 9) / 100 + f;
                            // 申卖价
                            d2.askPrice = byteArrayToLong(bs, 3, 12) / 100 + f;
                            // 最新价
                            d2.lastPrice = byteArrayToLong(bs, 3, 15) / 100 + f;
                            UpdataMarketDataEvent?.Invoke(d2);
                        }
                        //else
                        //{
                        //    ContractLastPrice md = new ContractLastPrice();
                        //    //md.timeId = timeId;
                        //    //md.truthTime = truthTime;
                        //    float f;
                        //    float.TryParse(d1.preSettlementPrice, out f);
                        //    md.category = category;
                        //    // 申买价
                        //    md.bidPrice = byteArrayToLong(bs, 3, 9) / 100 + f;
                        //    // 申卖价
                        //    md.askPrice = byteArrayToLong(bs, 3, 12) / 100 +f;
                        //    // 最新价
                        //    md.lastPrice = byteArrayToLong(bs, 3, 15) / 100 + f;
                        //    DataManager.Instance.AddContractLastPrice(md);
                        //    UpdataMarketDataEvent?.Invoke(md);
                        //}
                    }
                }
            }
            else if (e.Topic == "trans_list_topic")
            {
                byte[] bs = e.Message;
                string data = Encoding.UTF8.GetString(bs);
                try
                {
                    UpdataEvent?.Invoke(data);
                }
                catch (Exception err)
                {
                    Logger.LogError(err.ToString());
                }
            }
            else if (e.Topic == "command")
            {
                byte[] bs = e.Message;
                string data = Encoding.UTF8.GetString(bs);
                try
                {
                    UpdataEvent?.Invoke(data);
                }
                catch (Exception err)
                {
                    Logger.LogError(err.ToString());
                }
            }
            else if (e.Topic == m_topic)
            {
                byte[] bs = e.Message;
                string data = Encoding.UTF8.GetString(bs);
                try
                {
                    //核销
                    var b3 = Helper.Deserialize<WriteOffResponse>(data);
                    if (b3.Success)
                    {
                        if (b3.data != null)
                        {
                            NoticeWriteOffEvent?.Invoke(b3.data);
                        }
                        else if (b3.result != null)
                        {
                            NoticeWriteOffEvent?.Invoke(b3.result);

                        }
                    }
                }
                catch (Exception err)
                {
                    Logger.LogError(err.ToString());
                }
            }
        }

        private long byteArrayToLong(byte[] b, int size, int startIndex)
        {
            long ret = 0L;
            for (int i = size - 1; i >= 0; i--)
            {
                ret |= (b[startIndex + i] & 0xFF) << i * 8;
            }
            //int s = 0;
            //int s0 = b[0] & 0xff;// 最低位  
            //int s1 = b[1] & 0xff;
            //int s2 = b[2] & 0xff;
            //int s3 = b[3] & 0xff;
            //s3 <<= 24;
            //s2 <<= 16;
            //s1 <<= 8;
            //s = s0 | s1 | s2 | s3;
            return ret;
        }
    }
}
