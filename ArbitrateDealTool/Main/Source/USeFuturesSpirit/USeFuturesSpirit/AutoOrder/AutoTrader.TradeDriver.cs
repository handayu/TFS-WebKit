using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 自动下单机-交易驱动事件处理。
    /// </summary>
    public partial class AutoTrader
    {
        /// <summary>
        /// 委托回报变更。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderDriver_OnOrderBookChanged(object sender, USeOrderBookChangedEventArgs e)
        {
            try
            {
                OrderBookUpdateResult result = null;
                lock (m_syncObj)
                {
                    result = m_arbitrageOrder.UpdateOrderBook(e.OrderBook);
                    if (result != null)
                    {
                        result.Task.UpdateTaskState();

                        m_arbitrageOrder.UpdataArbitrageOrderState();
                        m_operatorEvent.Set();
                    }
                }

                if (result != null)
                {
                    //委托更新触发流程监控运行
                    m_operatorEvent.Set();
                    SafeFireArbitrageOrderChanged();

                    AutoTraderNotice tradeNotice = result.CreateNotice(this.TraderIdentify, this.Alias);
                    if (tradeNotice != null)
                    {
                        SafeFireAutoTraderNotice(tradeNotice);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        /// <summary>
        /// 行情变更通知事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuoteDriver_OnMarketDataChanged(object sender, USeMarketDataChangedEventArgs e)
        {
            if (m_backgroundRunFlag == false)
            {
                //监控流程未启动
                return;
            }
            if (IsMyCareInstrument(e.MarketData.Instrument) == false)
            {
                //不是自己关心的行情
                return;
            }

            //行情触发流程监控运行
            m_operatorEvent.Set();
        }

        /// <summary>
        /// 是否下单机关心合约。
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        private bool IsMyCareInstrument(USeInstrument instrument)
        {
            lock (m_syncObj)
            {
                if (m_arbitrageOrder.OpenArgument == null) return false;

                if (m_arbitrageOrder.OpenArgument.BuyInstrument == instrument)
                {
                    return true;
                }
                if (m_arbitrageOrder.OpenArgument.SellInstrument == instrument)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
