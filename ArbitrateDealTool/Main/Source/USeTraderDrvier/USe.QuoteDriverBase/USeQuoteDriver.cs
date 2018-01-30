#region
//==============================================================================
// 文件名称: USeQuoteDriver.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Justin Shen
// 创建日期: 2012/05/10
// 描    述: USe行情驱动抽象类。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;

using USe.TradeDriver.Common;
using USe.Common.AppLogger;
using System.Diagnostics;

namespace USe.TradeDriver.Common
{ 
    /// <summary>
    /// USe行情驱动抽象类。
    /// </summary>
    public abstract class USeQuoteDriver
    {
        #region event
        /// <summary>
        /// 行情变更事件。
        /// </summary>
        public event EventHandler<USeMarketDataChangedEventArgs> OnMarketDataChanged;

        /// <summary>
        /// 状态变更事件。
        /// </summary>
        public event EventHandler<USeQuoteDriverStateChangedEventArgs> OnDriverStateChanged;

        /// <summary>
        /// 发送错误信息事件。
        /// </summary>
        public event EventHandler<USeErrorMessageArgs> SendErrorMessage;
        #endregion // event

        #region member
        private USeQuoteDriverState m_driverState = USeQuoteDriverState.Inactive;
        protected IAppLogger m_logger = null;           // 日志
        protected string m_brokerID = string.Empty;     // 经纪商ID
        protected string m_investorID = string.Empty;   // 投资者代码
        #endregion // member

        #region property
        /// <summary>
        /// 行情服务状态。
        /// </summary>
        public USeQuoteDriverState DriverState
        {
            get
            {
                return m_driverState;
            }
            protected set
            {
                m_driverState = value;
            }
        }
        #endregion // property

        #region protected methods
        /// <summary>
        /// 触发行情变更事件。
        /// </summary>
        /// <param name="marketData">行情数据。</param>
        protected virtual void FireOnMarketDataChanged(USeMarketData marketData)
        {
            try
            {
                EventHandler<USeMarketDataChangedEventArgs> handel = this.OnMarketDataChanged;
                if (handel != null)
                {
                    USeMarketDataChangedEventArgs args = new USeMarketDataChangedEventArgs(marketData);
                    handel(this, args);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        /// <summary>
        /// 触发状态变更事件。
        /// </summary>
        /// <param name="newState">新状态。</param>
        /// <param name="reason">变更原因。</param>
        protected virtual void FireDriverStateChanged(USeQuoteDriverState newState, string reason)
        {
            this.DriverState = newState;
            try
            {
                EventHandler<USeQuoteDriverStateChangedEventArgs> handel = this.OnDriverStateChanged;
                if (handel != null)
                {
                    USeQuoteDriverStateChangedEventArgs args = new USeQuoteDriverStateChangedEventArgs("", DriverState, newState, reason);

                    handel(this, args);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        protected virtual void FireSendErrorMessage(string errorMessage)
        {
            USeErrorMessageArgs e = new USeErrorMessageArgs(errorMessage);
            if (SendErrorMessage != null)
            {
                SendErrorMessage(this, e);
            }
        }
        #endregion // property methods

        #region public methods
        /// <summary>
        /// 设置日志。
        /// </summary>
        /// <param name="appLogger"></param>
        public virtual void SetAppLogger(IAppLogger appLogger)
        {
            if (appLogger != null)
            {
                m_logger = appLogger;
            }
            else
            {
                m_logger = new NullLogger("USeQuoteDriver_DefaultLogger");
            }
        }

        /// <summary>
        /// 连接行情服务器。
        /// </summary>
        public abstract void ConnectServer();

        /// <summary>
        /// 断开行情服务器。
        /// </summary>
        public abstract void DisConnectServer();

        /// <summary>
        /// 登录行情服务器。
        /// </summary>
        /// <param name="brokerId">经纪商ID。</param>
        /// <param name="investorId">投资者ID。</param>
        /// <param name="password">密码</param>
        public abstract void Login(string brokerId,string investorId, string password);

        /// <summary>
        /// 登出行情服务器。
        /// </summary>
        public abstract void Logout();

        /// <summary>
        /// 订阅行情。
        /// </summary>
        /// <param name="instruments">合约列表</param>
        public abstract void Subscribe(List<USeInstrument> instruments);

        /// <summary>
        /// 订阅行情。
        /// </summary>
        /// <param name="instrument">合约。</param>
        public abstract void Subscribe(USeInstrument instrument);

        /// <summary>
        /// 退订行情。
        /// </summary>
        /// <param name="instruments">合约列表</param>
        public abstract void Unsubscribe(List<USeInstrument> instruments);

        /// <summary>
        /// 退订行情。
        /// </summary>
        /// <param name="instrument">合约。</param>
        public abstract void Unsubscribe(USeInstrument instrument);

        /// <summary>
        /// 查询行情。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <returns>行情信息</returns>
        public abstract USeMarketData Query(USeInstrument instrument);

        /// <summary>
        /// 快速查询行情(无缓存直接返回空行情)。
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        public abstract USeMarketData QuickQuery(USeInstrument instrument);
        #endregion // methods
    }
}
