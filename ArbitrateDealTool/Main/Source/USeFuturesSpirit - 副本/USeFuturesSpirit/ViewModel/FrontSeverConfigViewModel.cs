using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using USe.Common;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 前置服务配置。
    /// </summary>
    public class FrontSeverConfigViewModel : USeBaseViewModel
    {
        private string m_BrokerID;
        private string m_BrokerName;
        private string m_QuoteFrontAddress;
        private int m_QuoteFrontPort;
        private string m_TradeFrontAddress;
        private int m_TradeFrontPort;

        /// <summary>
        /// 经纪商ID。
        /// </summary>
        public string BrokerID
        {
            get { return m_BrokerID; }
            set
            {
                if (value != m_BrokerID)
                {
                    m_BrokerID = value;
                    SetProperty(() => this.BrokerID);
                }
            }
        }

        /// <summary>
        /// 经纪商名称。
        /// </summary>
        public string BrokerName
        {
            get { return m_BrokerName; }
            set
            {
                if (value != m_BrokerName)
                {
                    m_BrokerName = value;
                    SetProperty(() => this.BrokerName);
                }
            }
        }
        /// <summary>
        /// 行情前置地址。
        /// </summary>
        public string QuoteFrontAddress
        {
            get { return m_QuoteFrontAddress; }
            set
            {
                if (value != m_QuoteFrontAddress)
                {
                    m_QuoteFrontAddress = value;
                    SetProperty(() => this.QuoteFrontAddress);
                }
            }
        }

        /// <summary>
        /// 行情前置端口。
        /// </summary>
        public int QuoteFrontPort
        {
            get { return m_QuoteFrontPort; }
            set
            {
                if (value != m_QuoteFrontPort)
                {
                    m_QuoteFrontPort = value;
                    SetProperty(() => this.QuoteFrontPort);
                }
            }      
        }

        /// <summary>
        /// 交易前置地址。
        /// </summary>
        public string TradeFrontAddress
        {
            get { return m_TradeFrontAddress; }
            set
            {
                if (value != m_TradeFrontAddress)
                {
                    m_TradeFrontAddress = value;
                    SetProperty(() => this.TradeFrontAddress);
                }
            }      
        }

        /// <summary>
        /// 交易前置端口。
        /// </summary>
        public int TradeFrontPort
        {
            get { return m_TradeFrontPort; }
            set
            {
                if (value != m_TradeFrontPort)
                {
                    m_TradeFrontPort = value;
                    SetProperty(() => this.TradeFrontPort);
                }
            }      
        }

        public override string ToString()
        {
            return this.BrokerName;
        }

        public static FrontSeverConfigViewModel Create(FrontSeverConfig data)
        {
            FrontSeverConfigViewModel model = new FrontSeverConfigViewModel();
            model.BrokerID = data.BrokerID;
            model.BrokerName = data.BrokerName;
            model.QuoteFrontAddress = data.QuoteFrontAddress;
            model.QuoteFrontPort = data.QuoteFrontPort;
            model.TradeFrontAddress = data.TradeFrontAddress;
            model.TradeFrontPort = data.TradeFrontPort;
            return model;
        }
    }
}

