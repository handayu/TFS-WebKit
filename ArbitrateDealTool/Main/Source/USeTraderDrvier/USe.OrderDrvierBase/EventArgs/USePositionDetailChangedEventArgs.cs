#region Copyright & Version
//==============================================================================
// 文件名称: USePositionDetailChangedEventArgs.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/05/10
// 描    述: 持仓变更。
//==============================================================================
#endregion

using System;
using USe.TradeDriver.Common;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 持仓明细变更。
    /// </summary>
    public class USePositionDetailChangedEventArgs : EventArgs
    {
        #region construction
        /// <summary>
        /// 持仓变更。
        /// </summary>
        /// <param name="position"></param>
        public USePositionDetailChangedEventArgs(USePositionDetail positionDetail)
        {
            this.PositionDetail = positionDetail;
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 持仓明细。
        /// </summary>
        public USePositionDetail PositionDetail
        {
            get;
            private set;
        }
        #endregion // property
    }
}
