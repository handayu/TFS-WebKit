using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TradeRangeManager
{
    public class ConfigManager
    {
        /// <summary>
        /// 配置文件读取的品种交易时间区间信息
        /// </summary>
        private List<ProductTradeRangeInfo> m_productTradeRangeInfoList = null;

        private static ConfigManager ms_instance = null;

        private List<ProductInstrumentInfo> m_productInstrumentList = null;

        private ConfigManager()
        {

        }

        #region Singleton实例
        /// <summary>
        /// 单件实例。
        /// </summary>
        public static ConfigManager Instance
        {
            get
            {
                if (ms_instance == null)
                {
                    ms_instance = new ConfigManager();
                }

                return ms_instance;
            }
        }
        #endregion

        /// <summary>
        /// 获取所有品种信息
        /// </summary>
        /// <returns></returns>
        public List<ProductTradeRangeInfo> GetProductTradeRangeList()
        {
            m_productTradeRangeInfoList = new List<ProductTradeRangeInfo>();

            TradeRangeSection section = TradeRangeSection.GetSection();
            if (section == null) return m_productTradeRangeInfoList;
            ProductTradeRangeItemElementCollection itemsColl = section.ProductItemsCollection;
            if (itemsColl == null || itemsColl.Count <= 0)
            {
                throw new Exception("品种加载失败");
            }

            foreach (ProductTradeRangeItemElement item in itemsColl)
            {
                Debug.Assert(item != null);

                try
                {
                    ProductTradeRangeInfo productTradeRangeInfo = new ProductTradeRangeInfo();
                    productTradeRangeInfo.Name = item.Name;
                    productTradeRangeInfo.Exchange = item.Exchange;
                    productTradeRangeInfo.ProductrName = item.ProductName;
                    productTradeRangeInfo.Description = item.Description;

                    List<TradeRangeTimeSectionInfo> TimeSectionList = new List<TradeRangeTimeSectionInfo>();


                    TradingDayElementCollection tradingDaysColls = item.TradingDaysElementCollection;
                    if (tradingDaysColls == null || tradingDaysColls.Count <= 0)
                    {
                        throw new Exception("品种时间区间加载失败");
                    }
                    foreach (TradingDayElement dayElement in tradingDaysColls)
                    {
                        TradeRangeTimeSectionInfo timeSection = new TradeRangeTimeSectionInfo();
                        timeSection.BeginTime = dayElement.BeginTime;
                        timeSection.EndTime = dayElement.EndTime;
                        timeSection.IsNight = dayElement.IsNight;

                        TimeSectionList.Add(timeSection);
                    }

                    productTradeRangeInfo.TradeRangeTimeSectionsInfo = TimeSectionList;

                    m_productTradeRangeInfoList.Add(productTradeRangeInfo);
                }
                catch(Exception ex)
                {
                    throw new Exception("Load TimeSection failed :" + ex.Message);
                }
                
            }

            return m_productTradeRangeInfoList;

        }

        /// <summary>
        /// 获取所有品种信息
        /// </summary>
        /// <returns></returns>
        public List<ProductInstrumentInfo> GetProductInstrumentInfoList()
        {
            m_productInstrumentList = new List<ProductInstrumentInfo>();

            TradeRangeSection section = TradeRangeSection.GetSection();
            if (section == null) return m_productInstrumentList;
            ProductTradeRangeItemElementCollection itemsColl = section.ProductItemsCollection;
            if (itemsColl == null || itemsColl.Count <= 0)
            {
                throw new Exception("品种加载失败");
            }

            foreach (ProductTradeRangeItemElement item in itemsColl)
            {
                Debug.Assert(item != null);

                try
                {
                    ProductInstrumentInfo productInstrumentInfo = new ProductInstrumentInfo();
                    productInstrumentInfo.Name = item.Name;
                    productInstrumentInfo.Exchange = item.Exchange;
                    productInstrumentInfo.ShortName = item.ProductName;
                    productInstrumentInfo.LongName = item.ProductName;


                    m_productInstrumentList.Add(productInstrumentInfo);
                }
                catch (Exception ex)
                {
                    throw new Exception("Load TimeSection failed :" + ex.Message);
                }

            }

            return m_productInstrumentList;

        }


    }
}
