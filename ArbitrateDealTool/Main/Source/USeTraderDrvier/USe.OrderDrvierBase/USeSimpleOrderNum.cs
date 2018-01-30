#region Copyright & Version
//==============================================================================
// 文件名称: USeSimpleOrderNum.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/05/28
// 描    述: USe简易委托单号定义。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using USe.TradeDriver.Common;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe简易委托单号定义。
    /// </summary>
    public class USeSimpleOrderNum : USeOrderNum
    {
        #region construction
        /// <summary>
        /// 构造USeSimpleOrderNum实例。
        /// </summary>
        /// <param name="orderNum">委托单号。</param>
        public USeSimpleOrderNum(string orderNum)
        {
            this.OrderNum = orderNum;
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 委托单号。
        /// </summary>
        public string OrderNum
        {
            get;
            set;
        }

        /// <summary>
        /// 委托单字符串。
        /// </summary>
        public override string OrderString
        {
            get
            {
                return this.OrderNum;
            }
        }
        #endregion // property

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;

            USeSimpleOrderNum orderNum = obj as USeSimpleOrderNum;
            if (string.IsNullOrEmpty(this.OrderNum) == false &&
                this.OrderNum == orderNum.OrderNum)
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

        public static bool operator ==(USeSimpleOrderNum orderNum1, USeSimpleOrderNum orderNum2)
        {
            return Object.Equals(orderNum1, orderNum2);
        }

        public static bool operator !=(USeSimpleOrderNum orderNum1, USeSimpleOrderNum orderNum2)
        {
            return !Object.Equals(orderNum1, orderNum2);
        }

        public override string ToString()
        {
            return this.OrderNum;
        }

        /// <summary>
        /// 克隆USeSimpleOrderNum。
        /// </summary>
        /// <returns></returns>
        public override USeOrderNum Clone()
        {
            USeSimpleOrderNum orderNum = new USeSimpleOrderNum(this.OrderNum);
            return orderNum;
        }
    }
}
