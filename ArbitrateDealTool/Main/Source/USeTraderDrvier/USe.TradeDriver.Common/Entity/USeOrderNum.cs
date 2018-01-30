#region Copyright & Version
//==============================================================================
// 文件名称: USeOrderNum.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: USe委托单号定义。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe委托单号定义。
    /// </summary>
    public abstract class USeOrderNum
    {
        /// <summary>
        /// 委托单号字符串描述。
        /// </summary>
        public abstract string OrderString
        {
            get;
        }

        /// <summary>
        /// 克隆USeOrderNum。
        /// </summary>
        /// <returns></returns>
        public abstract USeOrderNum Clone();
    }

    /// <summary>
    /// USe委托单号比较器。
    /// </summary>
    public class USeOrderNumComparer : IEqualityComparer<USeOrderNum>
    {
        #region implement IEqualityComparer
        public bool Equals(USeOrderNum orderNum1, USeOrderNum orderNum2)
        {
            return orderNum1.Equals(orderNum2);
        }

        public int GetHashCode(USeOrderNum orderNum)
        {
            return orderNum.OrderString.GetHashCode();
        }
        #endregion // implement IEqualityComparer
    }
}
