using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;

namespace UseOnlineTradingSystem
{
    public class USeManager
    {
        private static USeManager ms_instance = null;
        private MQTTService m_mqttService = null;
        //private InterProcessService m_callBackService =new InterProcessService();
        private bool m_initialized = false;//是否完成初始化成功
        private string mqttAddress;
        private string ssoAddress;
        private string address;
        private bool release = false;
        private static readonly object m_lockHelper = new object();

        /// <summary>
        /// 是否发布版本
        /// </summary>
        public bool Release
        {
            get { return release; }
        }

        /// <summary>
        /// 环境地址
        /// </summary>
        public string MqttAddress
        {
            get { return mqttAddress; }
        }
        /// <summary>
        /// SSO地址
        /// </summary>
        public string SSOAddress
        {
            get { return ssoAddress; }
        }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Address
        {
            get { return address; }
        }
        /// <summary>
        /// 私有构造
        /// </summary>
        private USeManager()
        {
            if (m_initialized)
            {
                throw new ApplicationException(string.Format("USeManager already initialized", this));
            }
            CreateMQTTService();
            m_initialized = true;
        }

        /// <summary>
        /// 单件实例。
        /// </summary>
        public static USeManager Instance
        {
            get
            {
                if (ms_instance == null)
                {
                    lock(m_lockHelper)
                    {
                        ms_instance = new USeManager();
                    }
                }
                return ms_instance;
            }
        }

        /// <summary>
        /// MQTTService服务
        /// </summary>
        public MQTTService MQTTService
        {
            get { return m_mqttService; }
        }

        //public void SetMCallBack(string name)
        //{
        //    m_callBackService.ClientName = name;
        //    m_callBackService.MessageReceived += new MessageReceivedHandler(cb_MessageReceived);
        //}

        ///// <summary>
        ///// 进程间通信服务
        ///// </summary>
        //public InterProcessService MCallBack
        //{
        //    get { return m_callBackService; }
        //}

        private void CreateMQTTService()
        {
            if (Helper.GetAppConfig("Enable") == "true")
            {
                try
                {
                    address = Helper.GetAppConfig("address");
                    mqttAddress = Helper.GetAppConfig("mqttHost");
                    ssoAddress = Helper.GetAppConfig("ssoHost");
                    string port = Helper.GetAppConfig("mqttPort");
                    m_mqttService = new MQTTService(mqttAddress, port);
                    release = true;
                }
                catch (Exception ex)
                {
                    string text = "Create MQTTService object failed, " + ex.Message;
                    throw new ApplicationException(text, ex);
                }
            }
            else
            {
                CreateMQTTService("Service_test");
                CreateMQTTService("Service_deve");
            }
        }

        /// <summary>
        /// 集合Service创建MQTTBussiness服务
        /// </summary>
        private void CreateMQTTService(string name)
        {
            try
            {
                if (Helper.GetConfig(name, "enable") == "true" && m_mqttService == null)
                {
                    string cname = Helper.GetConfig(name, "name");
                    mqttAddress = Helper.GetConfig(name, "addressQuote");
                    ssoAddress = Helper.GetConfig(name, "ssoAddressQuote");
                    string port = Helper.GetConfig(name, "MQTTPort");
                    m_mqttService = new MQTTService(mqttAddress, port);
                }
            }
            catch (Exception ex)
            {
                string text = "Create MQTTService object failed, " + ex.Message;
                throw new ApplicationException(text, ex);
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start(string clientID = null, string username = "guest", string password = "guest")
        {
            if (m_initialized == false)
            {
                throw new ApplicationException(string.Format("USeManager is not initialized ,can not start", this));
            }
            if (m_mqttService != null)
            {
                try
                {
                    m_mqttService.Start(clientID , username, password );
                }
                catch (Exception ex)
                {
                    Logger.LogError(String.Format("{0} 启动MQTT服务异常, Error: {1}", m_mqttService, ex.Message));
                }
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (m_mqttService != null)
            {
                try
                {
                    m_mqttService.Stop();
                }
                catch (Exception ex)
                {
                    Logger.LogError(String.Format("{0} 关闭MQTT服务异常, Error: {1}", m_mqttService, ex.Message));
                }
            }
        }

        /// <summary>
        /// 进程间消息
        /// </summary>
        /// <param name="requestMessage"></param>
        private void cb_MessageReceived(ProcessMessage requestMessage)
        {
            if (requestMessage.RequestType == "AccountAndPassword")
            {
                Logger.LogInfo("收到帐号密码消息,值为：" + requestMessage.RequestString);
                string[] arr = requestMessage.RequestString.Split(',');
                if (arr.Length == 2)
                {
                    DataManager.Instance.Account = arr[0];
                    DataManager.Instance.Password = arr[1];
                }
            }
            else if (requestMessage.RequestType == "Cookies")
            {
                Logger.LogInfo("收到Cookies消息,值为：" + requestMessage.RequestString);

                if (requestMessage.RequestString != null)
                {
                    string code = null;
                    var arr = requestMessage.RequestString.Split(';');
                    if (arr.Length > 1)
                    {
                        var temp = arr[1].Split('=');
                        if (temp.Length > 1)
                        {
                            code = temp[1];
                        }
                    }
                    if (code != null && code.Contains("%"))
                    {
                        code = HttpUtility.UrlDecode(code);
                    }
                    DataManager.Instance.SetCookies(code);
                }
            }
            else if (requestMessage.RequestType == "IsLogin")
            {
                Logger.LogInfo("收到登录成功消息,值为：" + requestMessage.RequestString);
                if (!DataManager.Instance.IsLogin)
                {
                    DataManager.Instance.Login();
                }
            }
        }
    }
}
