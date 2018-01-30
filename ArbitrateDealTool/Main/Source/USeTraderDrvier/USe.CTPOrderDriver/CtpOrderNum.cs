#region Copyright & Version
//==============================================================================
// 文件名称: CtpOrderNum.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/05/07
// 描    述: Ctp委托单号定义。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using USe.TradeDriver.Common;

namespace USe.TradeDriver.Ctp
{
    /// <summary>
    /// Ctp委托单号定义。
    /// </summary>
    public class CtpOrderNum :USeOrderNum 
    {
        #region construciton
        public CtpOrderNum()
        {

        }
        /// <summary>
        /// 构造CtpOrderNum实例。
        /// </summary>
        /// <param name="frontID">前置编号。</param>
        /// <param name="sessionID">会话编号。</param>
        /// <param name="orderRef">报单引用。</param>
        public CtpOrderNum(int frontID, int sessionID,string orderRef)
            :this(frontID,sessionID,orderRef,string.Empty,string.Empty)
        {
        }

        /// <summary>
        /// 构造CtpOrderNum实例。
        /// </summary>
        /// <param name="exchangeID">交易所代码。</param>
        /// <param name="orderSysID">报单编号。</param>
        public CtpOrderNum(string exchangeID, string orderSysID)
            :this(0,0,string.Empty,exchangeID,orderSysID)
        {
        }

        /// <summary>
        /// 构造CtpOrderNum实例。
        /// </summary>
        /// <param name="frontID">前置编号。</param>
        /// <param name="sessionID">会话编号。</param>
        /// <param name="orderRef">报单引用。</param>
        /// <param name="exchangeID">交易所代码。</param>
        /// <param name="orderSysID">报单编号。</param>
        public CtpOrderNum(int frontID, int sessionID, string orderRef, string exchangeID, string orderSysID)
        {
            this.FrontID = frontID;
            this.SessionID = sessionID;
            this.OrderRef = orderRef;
            this.ExchangeID = exchangeID;
            this.OrderSysID = orderSysID;
        }
        #endregion // construciton

        #region property
        /// <summary>
        /// 前置编号。
        /// </summary>
        public int FrontID
        {
            get;
            set;
        }
        
        /// <summary>
        /// 会话编号。
        /// </summary>
        public int SessionID
        {
            get;
            set;
        }

        /// <summary>
        /// 报单引用。
        /// </summary>
        public string OrderRef
        {
            get;
            set;
        }

        /// <summary>
        /// 交易所代码。
        /// </summary>
        public string ExchangeID
        {
            get;
            set;
        }

        /// <summary>
        /// 报单编号。
        /// </summary>
        public string OrderSysID
        {
            get;
            set;
        }
       
        /// <summary>
        /// 委托单字符串。
        /// </summary>
        [XmlIgnore]
        public override string OrderString
        {
            get 
            {
                return string.Format("{0},{1},{2},{3},{4}", this.FrontID, this.SessionID, this.OrderRef, this.ExchangeID, this.OrderSysID);
            }
        }
        #endregion // property


        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;
            //(frontID+sessionID+orderRef)组合和(ExchangeID + OrderSysID)组合均能标识唯一的委托编号
            //两者使用环境不同，但在有值的前提下都可以作为识别关键字
            CtpOrderNum orderNum = obj as CtpOrderNum;
            if (this.FrontID != 0 &&
                this.SessionID != 0 &&
                string.IsNullOrEmpty(this.OrderRef) == false &&
                this.FrontID == orderNum.FrontID &&
                this.SessionID == orderNum.SessionID &&
                this.OrderRef == orderNum.OrderRef)
            {
                return true;
            }
            else if (string.IsNullOrEmpty(this.ExchangeID) == false &&
                    string.IsNullOrEmpty(this.OrderSysID) == false &&
                    this.ExchangeID == orderNum.ExchangeID &&
                    this.OrderSysID == orderNum.OrderSysID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.OrderString.GetHashCode();
        }

        public static bool operator ==(CtpOrderNum orderNum1, CtpOrderNum orderNum2)
        {
            return Object.Equals(orderNum1,orderNum2);
        }

        public static bool operator !=(CtpOrderNum orderNum1, CtpOrderNum orderNum2)
        {
            return !Object.Equals(orderNum1, orderNum2);
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.OrderSysID) == false)
            {
                return this.OrderSysID;
            }
            else if (string.IsNullOrEmpty(this.OrderRef) == false)
            {
                return this.OrderRef;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 克隆CtpOrderNum对象。
        /// </summary>
        /// <returns></returns>
        public override USeOrderNum Clone()
        {
            CtpOrderNum orderNum = new CtpOrderNum(this.FrontID,
                                                this.SessionID,
                                                this.OrderRef,
                                                this.ExchangeID,
                                                this.OrderSysID);
            return orderNum;
        }
    }

    /// <summary>
    /// Ctp委托单号比较器。
    /// </summary>
    public class CtpOrderNumComparer : IEqualityComparer<CtpOrderNum>
    {
        #region implement IEqualityComparer
        public bool Equals(CtpOrderNum orderNum1, CtpOrderNum orderNum2)
        {
            return orderNum1.Equals(orderNum2);
        }

        public int GetHashCode(CtpOrderNum orderNum)
        {
            return orderNum.OrderString.GetHashCode();
        }
        #endregion // implement IEqualityComparer
    }
}
