#region Copyright & Version
//==============================================================================
// 文件名称: USePositionChangedEventArgs.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 持仓变更。
//==============================================================================
#endregion

using System;
using USe.TradeDriver.Common;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 持仓变更。
    /// </summary>
    public class USePositionChangedEventArgs : EventArgs
    {
        #region construction
        /// <summary>
        /// 持仓变更。
        /// </summary>
        /// <param name="position"></param>
        public USePositionChangedEventArgs(USePosition position)
        {
            this.Position = position;
        }
        #endregion // construction

        #region property
        /// <summary>
        /// 持仓。
        /// </summary>
        public USePosition Position
        {
            get;
            private set;
        }
        #endregion // property
    }
}
