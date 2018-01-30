using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// K线响应
    /// </summary>
    public class UseKLineResponse : BaseResponse
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string timeStamp;
        /// <summary>
        /// K线数据
        /// </summary>
        public List<UseKLine> data;
    }
    /// <summary>
    /// K线数据
    /// </summary>
    public class UseKLine
    {
        //"dateTime":1481817600000,"priceOpen":47850.0,"priceHigh":48050.0,"priceLow":47850.0,"priceClose":47870.0,"volumn":8.0};
        public string dateTime;//时间
        public string priceOpen;//开盘价
        public string priceHigh;//最高价
        public string priceLow;//最低价
        public string priceClose;//收盘价
        public string volumn;//量
        public string turnover;//交易额
}
}
