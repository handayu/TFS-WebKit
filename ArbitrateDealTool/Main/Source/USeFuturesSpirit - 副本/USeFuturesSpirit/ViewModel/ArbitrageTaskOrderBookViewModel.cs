using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using USe.Common;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    class ArbitrageTaskOrderBookViewModel : OrderBookViewModel
    {
        #region member
        private int m_taskId = 0;
        #endregion

        #region property
        /// <summary>
        /// 任务ID。
        /// </summary>
        public int TaskId
        {
            get { return m_taskId; }
            set
            {
                m_taskId = value;
                SetProperty(() => this.TaskId);
            }
        }
        #endregion 
  
      
    }
}
