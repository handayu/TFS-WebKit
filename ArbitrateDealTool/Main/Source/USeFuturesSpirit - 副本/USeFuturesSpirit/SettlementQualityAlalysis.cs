using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 结算单分析器类
    /// </summary>
    public class SettlementQualityAlalysis
    {
        private Dictionary<string, decimal> m_dateQuality = null;//天-结算单资金汇总Dic

        public SettlementQualityAlalysis()
        {
            m_dateQuality = new Dictionary<string, decimal>();


        }

        public Dictionary<string, decimal> DateQuelity
        {
            get { return m_dateQuality; }
        }

        /// <summary>
        /// 获取指定交易区间的结算信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetDayRangeQuality(DateTime startDate, DateTime endDate)
        {
            DateTime dateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);

            for (DateTime dateTimeTemp = dateTime; dateTimeTemp <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0); dateTimeTemp = dateTimeTemp.AddDays(1))
            {
                try
                {
                    string tradingDateStr = dateTimeTemp.ToString("yyyyMMdd");
                    string settlementInfo = USeManager.Instance.OrderDriver.GetSettlementInfo(tradingDateStr);
                    if (settlementInfo.Equals("")) continue;

                    string[] strInfoTemp = settlementInfo.Split('\n');
                    string custormQualityInfo = strInfoTemp[15];
                    string cusSpilt = "";
                    Debug.Assert(custormQualityInfo.Count() > 0);
                    foreach (char c in custormQualityInfo)
                    {
                        if (c == ' ' || c == '\r') continue;
                        cusSpilt = cusSpilt + c;
                    }

                    string qualityStr = "";

                    for (int i = cusSpilt.Count() - 1; i >= 0; i--)
                    {
                        if ((cusSpilt[i] >= '0' && cusSpilt[i] <= '9') || cusSpilt[i] == '.')
                        {
                            qualityStr = qualityStr.Insert(0, cusSpilt[i].ToString());
                        }
                        else
                        {
                            break;
                        }
                    }

                    //弄出资金的值
                    m_dateQuality[tradingDateStr] = Convert.ToDecimal(qualityStr);
                }
                catch(Exception ex)
                {
                    throw new Exception("读取结算单信息失败," + ex.Message);
                }
            }
            return m_dateQuality;
        }
    }
}
