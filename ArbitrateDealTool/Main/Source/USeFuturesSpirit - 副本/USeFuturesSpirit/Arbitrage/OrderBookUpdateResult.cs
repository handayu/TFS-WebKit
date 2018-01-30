using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 委托回报更新结果(相对上次镜像)。
    /// </summary>
    public class OrderBookUpdateResult
    {
        /// <summary>
        /// 合约。
        /// </summary>
        public USeInstrument Instrument { get; set; }

        /// <summary>
        /// 成交量。
        /// </summary>
        public int TradeQty { get; set; }

        /// <summary>
        /// 撤单量。
        /// </summary>
        public int CancelQty { get; set; }

        /// <summary>
        /// 废单量。
        /// </summary>
        public int BlankQty { get; set; }

        /// <summary>
        /// 所属任务。
        /// </summary>
        public ArbitrageTask Task { get; set; }

        private string TaskDesc
        {
            get { return this.Task == null? "任务<null>" : "任务"+this.Task.TaskId.ToString(); }
        }

        public AutoTraderNotice CreateNotice(Guid traderIdentify,string alias)
        {
            AutoTraderNotice notice = null;
            if (this.CancelQty > 0)
            {
                string text = string.Format("{0} {1}撤单{2}手", this.TaskDesc, this.Instrument, this.CancelQty);
                notice = new AutoTraderNotice(traderIdentify, alias, AutoTraderNoticeType.Trade, text);
            }
            else if(this.BlankQty >0)
            {
                string text = string.Format("{0} {1}废单{2}手", this.TaskDesc, this.Instrument, this.BlankQty);
                notice = new AutoTraderNotice(traderIdentify, alias, AutoTraderNoticeType.Trade, text);
            }
            else if(this.TradeQty >0)
            {
                string text = string.Format("{0} {1}成交{2}手", this.TaskDesc, this.Instrument, this.TradeQty);
                notice = new AutoTraderNotice(traderIdentify, alias, AutoTraderNoticeType.Trade, text);
            }

            return notice;
        }
    }
}
