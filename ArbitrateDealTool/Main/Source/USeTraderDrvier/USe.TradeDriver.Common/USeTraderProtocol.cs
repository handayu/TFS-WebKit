using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe交易协议。
    /// </summary>
    public static class USeTraderProtocol
    {
        /// <summary>
        /// 根据合约名称获取品种代码。
        /// </summary>
        /// <param name="instrumentCode">合约名称</param>
        /// <returns>品种代码。</returns>
        public static string GetVarieties(string instrumentCode)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < instrumentCode.Length; i++)
            {
                char item = instrumentCode[i];
                if (item >= '0' && item <= '9')
                {
                    break;
                }
                else
                {
                    sb.Append(item);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取USe主力合约编码。
        /// </summary>
        /// <param name="product">品种。</param>
        /// <returns>主力合约编码。</returns>
        public static USeInstrument GetMainContractCode(USeProduct product)
        {
            return GetMainContractCode(product.ProductCode, product.Market);
        }

        /// <summary>
        /// 获取USe主力合约编码。
        /// </summary>
        /// <param name="varieties">品种。</param>
        /// <param name="exchange">市场。</param>
        /// <returns>主力合约编码。</returns>
        public static USeInstrument GetMainContractCode(string varieties, USeMarket exchange)
        {
            string contractCode = string.Format("{0}9999", varieties);
            return new USeInstrument(contractCode, varieties + "主力", exchange);

            //string contractCode = string.Empty;
            //switch (exchange)
            //{
            //    case USeMarket.CZCE:
            //        contractCode = string.Format("{0}999", varieties);
            //        break;
            //    case USeMarket.CFFEX:
            //    case USeMarket.SHFE:
            //    case USeMarket.DCE:
            //        contractCode = string.Format("{0}9999", varieties);
            //        break;
            //    default:
            //        throw new NotSupportedException(string.Format("不支持{0}市场主力合约编码", exchange));
            //}

            //return new USeInstrument(contractCode, varieties+"主力", exchange);
        }

        /// <summary>
        /// 获取USe品种指数编码。
        /// </summary>
        /// <param name="product">品种。</param>
        /// <returns>品种指数编码。</returns>
        public static USeInstrument GetVarietiesIndexCode(USeProduct product)
        {
            return GetVarietiesIndexCode(product.ProductCode, product.Market);
        }

        /// <summary>
        /// 获取USe品种指数编码。
        /// </summary>
        /// <param name="varieties">品种。</param>
        /// <param name="exchange">市场。</param>
        /// <returns>品种指数编码。</returns>
        public static USeInstrument GetVarietiesIndexCode(string varieties, USeMarket exchange)
        {
            string indexCode = string.Format("{0}8888", varieties);
            return new USeInstrument(indexCode, varieties + "指数", exchange);

            //string indexCode = string.Empty;
            //switch (exchange)
            //{
            //    case USeMarket.CZCE:
            //        indexCode = string.Format("{0}888", varieties);
            //        break;
            //    case USeMarket.CFFEX:
            //    case USeMarket.SHFE:
            //    case USeMarket.DCE:
            //        indexCode = string.Format("{0}8888", varieties);
            //        break;
            //    default:
            //        throw new NotSupportedException(string.Format("不支持{0}市场品种指数编码", exchange));
            //}

            //return new USeInstrument(indexCode, varieties + "指数", exchange);
        }

        public static USeMarket ToUseMarket(string exchange)
        {
            return (USeMarket)Enum.Parse(typeof(USeMarket), exchange);
        }

        /// <summary>
        /// 获取国内交易市场。
        /// </summary>
        /// <returns></returns>
        public static List<USeMarket> GetInternalFutureMarket() 
        {
            List<USeMarket> marketList = new List<USeMarket>();
            marketList.Add(USeMarket.SHFE);
            marketList.Add(USeMarket.CZCE);
            marketList.Add(USeMarket.CFFEX);
            marketList.Add(USeMarket.DCE);

            return marketList;
        }

        /// <summary>
        /// 获取国内交易市场Sql字符串。
        /// </summary>
        /// <returns></returns>
        public static string GetInternalFutureMarketSqlString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("'{0}',", USeMarket.SHFE.ToString()));
            sb.Append(string.Format("'{0}',", USeMarket.CZCE.ToString()));
            sb.Append(string.Format("'{0}',", USeMarket.CFFEX.ToString()));
            sb.Append(string.Format("'{0}'", USeMarket.DCE.ToString()));

            return sb.ToString();
        }
    }
}
