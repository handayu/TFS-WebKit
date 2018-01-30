using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace USeFuturesSpirit.Configuration
{
    /// <summary>
    /// 交易时间配置节定义。
    /// </summary>
    public class TradeRangeSection : ConfigurationSection
    {
        /// <summary>
        /// 配置节集合, 只读。
        /// </summary>
        [ConfigurationProperty("exchangeItems", IsDefaultCollection = false)]
        public ExchangeTradeRangeItemElementCollection ExchangeItemsCollection
        {
            get
            {
                return (ExchangeTradeRangeItemElementCollection)base["exchangeItems"];
            }
        }

        /// <summary>
        /// 配置节集合, 只读。
        /// </summary>
        [ConfigurationProperty("productItems", IsDefaultCollection = false)]
        public ProductTradeRangeItemElementCollection ProductItemsCollection
        {
            get
            {
                return (ProductTradeRangeItemElementCollection)base["productItems"];
            }
        }

        /// <summary>
        /// 获取系统默认配置文件里面的CtpQuoteDriver配置节集合。
        /// </summary>
        /// <returns>CtpQuoteDriver配置节。</returns>
        public static TradeRangeSection GetSection()
        {
            object section = ConfigurationManager.GetSection("TradeRangeSection");
            return section as TradeRangeSection;
        }
    }
}
