using USe.TradeDriver.Common;

namespace DataStoreCommon
{
    /// <summary>
    /// 行情数据监听者。 
    /// </summary>
    public interface IMarketDataListener
    {
        void ReceiveMarketData(USeMarketData marketData);
    }
}
