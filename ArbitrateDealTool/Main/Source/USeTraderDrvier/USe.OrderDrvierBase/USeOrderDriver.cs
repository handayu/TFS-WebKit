#region Copyright & Version
//==============================================================================
// 文件名称: USeOrderDriver.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: USe交易驱动抽象类。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using USe.TradeDriver.Common;
using USe.Common.AppLogger;
using USe.Common.AppLogger.Configuration;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe交易驱动抽象类。
    /// </summary>
    public abstract class USeOrderDriver
    {
        #region event
        /// <summary>
        /// 登录进度事件。
        /// </summary>
        public event EventHandler<USeLoginReportEventArgs> OnLoginReport;

        /// <summary>
        /// 状态变更事件。
        /// </summary>
        public event EventHandler<USeOrderDriverStateChangedEventArgs> OnDriverStateChanged;

        /// <summary>
        /// 委托回报新增或变更。
        /// </summary>
        public event EventHandler<USeOrderBookChangedEventArgs> OnOrderBookChanged;

        /// <summary>
        /// 成交回报新增或变更。
        /// </summary>
        public event EventHandler<USeTradeBookChangedEventArgs> OnTradeBookChanged;

        /// <summary>
        /// 持仓变更。
        /// </summary>
        public event EventHandler<USePositionChangedEventArgs> OnPositionChanged;

        /// <summary>
        /// 持仓变更。
        /// </summary>
        public event EventHandler<USePositionDetailChangedEventArgs> OnPositionDetailChanged;

        /// <summary>
        /// 账户资金变更(由于下单产生的变更,由于行情触发的变更不通知)。
        /// </summary>
        public event EventHandler<USeFundChangedEventArgs> OnFundChanged;
        #endregion // event

        #region member
        private USeOrderDriverState m_driverState = USeOrderDriverState.Inactive;
        protected USeDriverType m_driverType = USeDriverType.Unknown; // 驱动类型
        protected string m_brokerID = string.Empty;     // 经纪商ID
        protected string m_investorID = string.Empty;   // 投资者代码
        protected string m_password = string.Empty;     // 密码(为自动重连准备)
        protected bool m_needSettlementConfirm = true;  // 是否需要结算单确认
        protected IAppLogger m_logger = null;           // 日志
        #endregion // member

        #region property
        /// <summary>
        /// 交易服务状态。
        /// </summary>
        public USeOrderDriverState DriverState
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

        /// <summary>
        /// 驱动名称。
        /// </summary>
        public abstract string DriverName
        {
            get;
        }

        /// <summary>
        /// 当前登录交易帐号所属经纪商。
        /// </summary>
        public string BrokerId
        {
            get { return m_brokerID; }
        }

        /// <summary>
        /// 当前登录交易帐号。
        /// </summary>
        public string Account
        {
            get { return m_investorID; }
        }

        /// <summary>
        /// 当前登录交易帐号密码。
        /// </summary>
        public string Password
        {
            get { return m_password; }
        }
    

        /// <summary>
        /// 驱动类型。
        /// </summary>
        public virtual USeDriverType DriverType
        {
            get { return m_driverType; }
        }

        /// <summary>
        /// 是否需要结算确认。
        /// </summary>
        public virtual bool NeedSettlementConfirm
        {
            get { return m_needSettlementConfirm; }
        }
        #endregion // property

        #region protected methods
        /// <summary>
        /// 触发登录进度报告事件。
        /// </summary>
        /// <param name="args"></param>
        protected virtual void FireLoginReport(double percent,string message)
        {
            USeLoginReportEventArgs args = new USeLoginReportEventArgs(percent, message);
        
            EventHandler<USeLoginReportEventArgs> handel = this.OnLoginReport;
            if (handel != null)
            {
                try
                {
                    handel(this, args);
                }
                catch
                {
                    Debug.Assert(false);
                }
            }
        }

        /// <summary>
        /// 触发状态变更事件。
        /// </summary>
        /// <param name="args"></param>
        protected virtual void FireOrderDriverStateChanged(USeOrderDriverState newState, string reason)
        { 
            Debug.WriteLine(string.Format("[{0}] driver state changed {1} ==> {2},reason:{3}.", ToString(), this.DriverState, newState, reason));

            USeOrderDriverStateChangedEventArgs args = new USeOrderDriverStateChangedEventArgs("",
                                                                                         this.DriverState,
                                                                                         newState,
                                                                                         reason);

            this.DriverState = newState;

            EventHandler<USeOrderDriverStateChangedEventArgs> handel = this.OnDriverStateChanged;
            if (handel != null)
            {
                try
                {
                    handel(this, args);
                }
                catch
                {
                    Debug.Assert(false);
                }
            }
        }

        /// <summary>
        /// 触发委托回报变更事件。
        /// </summary>
        /// <param name="args"></param>
        protected virtual void FireOrderBookChanged(USeOrderBook orderBook)
        {
            if (orderBook == null) return;

            USeOrderBookChangedEventArgs args = new USeOrderBookChangedEventArgs(orderBook);

            Debug.WriteLine(string.Format("[{0}] order book changed,[OrderNum:{1}],[TradeQty:{2}][TradePrice:{3}][TradeAmount:{4}][OrderState:{5}]",
                        ToString(), args.OrderBook.OrderNum.ToString(), args.OrderBook.TradeQty,
                        args.OrderBook.TradePrice, args.OrderBook.TradeAmount, args.OrderBook.OrderStatus));

            EventHandler<USeOrderBookChangedEventArgs> handel = this.OnOrderBookChanged;
            if (handel != null)
            {
                handel(this, args);
            }
        }

        /// <summary>
        /// 触发成交回报变更事件。
        /// </summary>
        /// <param name="args"></param>
        protected virtual void FireTradeBookChanged(USeTradeBook tradeBook,bool isNew)
        {
            if (tradeBook == null) return;
            USeTradeBookChangedEventArgs args = new USeTradeBookChangedEventArgs(tradeBook,isNew);
            Debug.WriteLine(string.Format("[{0}] trade book changed,[TradeNum:{1}].",
                          ToString(), args.TradeBook.TradeNum));

            EventHandler<USeTradeBookChangedEventArgs> handel = this.OnTradeBookChanged;
            if (handel != null)
            {
                try
                {
                    handel(this, args);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 触发持仓变更事件。
        /// </summary>
        /// <param name="args"></param>
        protected virtual void FirePositionChanged(USePosition position)
        {
            if (position == null) return;
            USePositionChangedEventArgs args = new USePositionChangedEventArgs(position);

            Debug.WriteLine(string.Format("[{0}] positon changed,[Product:{1}][Direction:{2}].",
                         ToString(), args.Position.Instrument, args.Position.Direction));

            if (OnPositionChanged != null)
            {
                OnPositionChanged(this, args);
            }
        }

        /// <summary>
        /// 触发持仓明细变更事件。
        /// </summary>
        /// <param name="args"></param>
        protected virtual void FirePositionDetailChanged(USePositionDetail positionDetail)
        {
            if (positionDetail == null) return;
            USePositionDetailChangedEventArgs args = new USePositionDetailChangedEventArgs(positionDetail);
            Debug.WriteLine(string.Format("[{0}] positon detail changed,[Product:{1}][Direction:{2}].",
                         ToString(), args.PositionDetail.Instrument, args.PositionDetail.Direction));

            if (OnPositionDetailChanged != null)
            {
                OnPositionDetailChanged(this, args);
            }
        }

        /// <summary>
        /// 触发账户资金变更事件。
        /// </summary>
        /// <param name="args"></param>
        protected virtual void FireFundChanged(USeFund fundInfo)
        {
            if (fundInfo == null) return;

            USeFundChangedEventArgs args = new USeFundChangedEventArgs(fundInfo);
            Debug.WriteLine(string.Format("[{0}] fund changed.",
                     args.FundInfo.AccountID));

            EventHandler<USeFundChangedEventArgs> handel = this.OnFundChanged;

            if (handel != null)
            {
                handel(this, args);
            }
        }
        #endregion // protected methods

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
                m_logger = new NullLogger("USeOrderDriver_DefaultLogger");
            }
        }

        /// <summary>
        /// 连接交易服务器。
        /// </summary>
        public abstract void ConnectServer();

        /// <summary>
        /// 认证
        /// </summary>
        public abstract void AuthorT(string brokerID, string userId, string authCode, string userProductInfo);

        /// <summary>
        /// 断开交易服务器。
        /// </summary>
        public abstract void DisConnectServer();

        /// <summary>
        /// 登录交易服务器。
        /// </summary>
        /// <param name="brokerId">经纪商ID。</param>
        /// <param name="account">帐号。</param>
        /// <param name="password">密码。</param>
        public abstract void Login(string brokerId,string account, string password);

        /// <summary>
        /// 登出交易服务器。 
        /// </summary>
        public abstract void Logout();

        /// <summary>
        /// 结算单确认。
        /// </summary>
        public abstract void SettlementInfoConfirm();

        /// <summary>
        /// 获取确认结算单。
        /// </summary>
        /// <returns>确认结算单信息。</returns>
        public abstract string GetSettlementInfo(string tradingDate);

        /// <summary>
        /// 查询投资者基本信息。
        /// </summary>
        /// <returns></returns>
        public abstract USeInvestorBaseInfo QueryInvestorInfo();

        /// <summary>
        /// 查询所有期货合约详细信息。
        /// </summary>
        /// <returns>合约详细信息。</returns>
        public abstract List<USeInstrumentDetail> QueryInstrumentDetail();

        /// <summary>
        /// 查询指定合约详细信息。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <returns>合约详细信息。</returns>
        public abstract USeInstrumentDetail QueryInstrumentDetail(USeInstrument instrument);

        /// <summary>
        /// 查询指定品种的所有合约详细信息。
        /// </summary>
        /// <param name="varieties">品种类型。</param>
        /// <returns></returns>
        public abstract List<USeInstrumentDetail> QueryInstrumentDetail(string varieties);


        /// <summary>
        /// 查询所有产品信息。
        /// </summary>
        /// <returns>产品信息。</returns>
        public abstract List<USeProductDetail> QueryProductDetails();

        /// <summary>
        /// 查询产品信息。
        /// </summary>
        /// <param name="productCode">产品代码。</param>
        /// <returns>产品信息。</returns>
        public abstract USeProductDetail QueryProductDetail(string productCode);

        /// <summary>
        /// 查询所有产品信息。
        /// </summary>
        /// <returns>产品信息。</returns>
        public abstract List<USeProduct> QueryProducts();

        /// <summary>
        /// 查询产品信息。
        /// </summary>
        /// <param name="productCode">产品代码。</param>
        /// <returns>产品信息。</returns>
        public abstract USeProduct QueryProduct(string productCode);

        /// <summary>
        /// 查询合约的合约乘数。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <returns>合约乘数。</returns>
        public abstract int QueryInstrumentVolumeMultiple(USeInstrument instrument);

        /// <summary>
        /// 查询合约手续费。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <returns>合约手续费。</returns>
        public abstract USeFee QueryInstrumentFee(USeInstrument instrument);

        /// <summary>
        /// 查询合约保证金。
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        public abstract USeMargin QueryInstrumentMargin(USeInstrument instrument);

        /// <summary>
        /// 查询期权合约交易成本信息。
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        public virtual USeOptionTradeCost QueryOptionTradeCost(USeInstrument instrument)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询当日所有委托回报。
        /// </summary>
        /// <returns>委托回报。 </returns>
        public abstract List<USeOrderBook> QueryOrderBooks();

        ///// <summary>
        ///// 查询当日某一合约的委托回报。
        ///// </summary>
        ///// <param name="product">合约。</param>
        ///// <returns></returns>
        //public abstract USeOrderBook[] QueryOrderBooks(USeInstrument product);

        /// <summary>
        /// 查询指定委托单号的委托回报。
        /// </summary>
        /// <param name="orderNum"></param>
        /// <returns></returns>
        public abstract USeOrderBook QueryOrderBook(USeOrderNum orderNum);

        /// <summary>
        /// 查询当日所有成交回报。
        /// </summary>
        /// <returns></returns>
        public abstract List<USeTradeBook> QueryTradeBooks();

        ///// <summary>
        ///// 查询当日某一合约的成交回报。
        ///// </summary>
        ///// <param name="instrument">合约。</param>
        ///// <returns></returns>
        //public abstract USeTradeBook[] QueryTradeBooks(USeInstrument product);

        /// <summary>
        /// 查询当日所有持仓。
        /// </summary>
        /// <returns></returns>
        public abstract List<USePosition> QueryPositions();

        /// <summary>
        /// 查询某一合约当前持仓。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <returns></returns>
        public abstract List<USePosition> QueryPositions(USeInstrument instrument);

        /// <summary>
        /// 查询某一合约指定方向当前持仓。
        /// </summary>
        /// <param name="instrument">合约。</param>
        /// <param name="direction">持仓方向。</param>
        /// <returns></returns>
        public abstract USePosition QueryPositions(USeInstrument instrument, USeDirection direction);

        /// <summary>
        /// 查询持仓明细。
        /// </summary>
        /// <returns></returns>
        public abstract List<USePositionDetail> QueryPositionDetail();

        /// <summary>
        /// 查询账户资金信息。
        /// </summary>
        /// <returns></returns>
        public abstract USeFund QueryFundInfo();

        /// <summary>
        /// 查询帐户详细资金信息。
        /// </summary>
        /// <returns></returns>
        public abstract USeFundDetail QueryFundDetailInfo();

        /// <summary>
        /// 委托下单。
        /// </summary>
        /// <param name="product">委托产品。</param>
        /// <param name="qty">委托量。</param>
        /// <param name="price">委托价格。</param>
        /// <param name="offsetType">开平仓方向。</param>
        /// <param name="orderSide">买卖方向。</param>
        /// <param name="error">[out]委托失败原因。</param>
        /// <returns>委托单号。</returns>
        /// <remarks>返回为null代表失败,否则为委托单号。</remarks>
        public abstract USeOrderNum PlaceOrder(USeInstrument product, int qty, decimal price, USeOffsetType offsetType, USeOrderSide orderSide, out string error);

        /// <summary>
        /// 撤单。
        /// </summary>
        /// <param name="orderNum">委托单号。</param>
        /// <param name="product">委托产品。</param>
        /// <param name="error">[out]撤单失败错误信息。</param>
        /// <returns>撤单是否成功。</returns>
        public abstract bool CancelOrder(USeOrderNum orderNum, USeInstrument product, out string error);

        /// <summary>
        /// 修改密码。
        /// </summary>
        /// <param name="oldPasswrod">旧密码。</param>
        /// <param name="newPassword">新密码。</param>
        /// <returns>修改密码是否成功。</returns>
        public abstract bool ModifyPassword(string oldPasswrod, string newPassword,out string error);
        #endregion // public methods
    }
}
