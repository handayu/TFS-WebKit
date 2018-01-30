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
        private bool m_initialized = false;//是否完成初始化成功
        private string mqttAddress;//mqtt地址
        private string ssoAddress;//sso地址
        private string address;//基础服务地址
        private string dataCenterAddress;//数据中心
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
        /// 数据中心地址
        /// </summary>
        public string DataCenterAddress
        {
            get { return dataCenterAddress; }
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
                Logger.LogError("USeManager已经初始化！");
                return;
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

        private void CreateMQTTService()
        {
            Logger.LogInfo("创建MQTT对象开始");
            if (Helper.GetAppConfig("Enable") == "true")
            {
                try
                {
                    Logger.LogInfo("创建预发布环境MQTT对象");
                    address = Helper.GetAppConfig("address");
                    dataCenterAddress = Helper.GetAppConfig("dataCenterAddress");
                    mqttAddress = Helper.GetAppConfig("mqttHost");
                    ssoAddress = Helper.GetAppConfig("ssoHost");
                    string port = Helper.GetAppConfig("mqttPort");
                    m_mqttService = new MQTTService(mqttAddress, port);
                    release = true;
                }
                catch (Exception ex)
                {
                    Logger.LogError("创建MQTT对象失败, " + ex.Message);
                }
            }
            else
            {
                if (CreateMQTTService("Service_test"))
                {
                    Logger.LogInfo("创建测试环境MQTT对象");
                }
                else if (CreateMQTTService("Service_deve"))
                {
                    Logger.LogInfo("创建开发环境MQTT对象");
                }
            }
            Logger.LogInfo("创建MQTT对象结束");
        }

        /// <summary>
        /// 集合Service创建MQTTBussiness服务
        /// </summary>
        private bool CreateMQTTService(string name)
        {
            try
            {
                if (Helper.GetConfig(name, "enable") == "true" && m_mqttService == null)
                {
                    string cname = Helper.GetConfig(name, "name");
                    address = mqttAddress = Helper.GetConfig(name, "addressQuote");
                    dataCenterAddress = Helper.GetConfig(name, "dataCenterAddress");
                    ssoAddress = Helper.GetConfig(name, "ssoAddressQuote");
                    string port = Helper.GetConfig(name, "MQTTPort");
                    m_mqttService = new MQTTService(mqttAddress, port);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("创建MQTT对象失败, " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start(string clientID = null, string username = "guest", string password = "guest")
        {
            Logger.LogInfo("启动MQTT对象开始");
            if (m_initialized == false)
            {
                Logger.LogError("MQTT对象尚未初始化");
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
            Logger.LogInfo("启动MQTT对象结束");
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            Logger.LogInfo("停止MQTT对象开始");
            if (m_mqttService != null)
            {
                try
                {
                    m_mqttService.Stop();
                }
                catch (Exception ex)
                {
                    Logger.LogError(String.Format("{0} 停止MQTT服务异常, Error: {1}", m_mqttService, ex.Message));
                }
            }
            Logger.LogInfo("停止MQTT对象结束");

        }
    }
}
