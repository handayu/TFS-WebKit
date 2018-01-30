using USe.TradeDriver.Common;

namespace OuterMarketDataStore
{
    /// <summary>
    /// 行情数据监听者。 
    /// </summary>
    public interface IMarketDataListener
    {
        void ReceiveMarketData(USeMarketData marketData);
    }
}
