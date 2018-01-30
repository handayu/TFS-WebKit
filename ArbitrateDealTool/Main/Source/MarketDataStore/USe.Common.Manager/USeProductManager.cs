using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using USe.TradeDriver.Common;
using USe.Common.DBDriver;
using System.Data;

namespace USe.Common.Manager
{
    /// <summary>
    /// 品种管理类(国内市场)。
    /// </summary>
    public class USeProductManager
    {
        #region member
        private string m_dbConnStr = string.Empty;
        private string m_alphaDBName = string.Empty;
        public Dictionary<string, USeProduct> m_varietiesDic = null;
        #endregion

        #region
        /// <summary>
        /// 构造方法。
        /// </summary>
        /// <param name="dbConnStr">MySql数据连接串。</param>
        /// <param name="alphaDBName">Alpha数据库名。</param>
        public USeProductManager(string dbConnStr, string alphaDBName)
        {
            m_dbConnStr = dbConnStr;
            m_alphaDBName = alphaDBName;
            m_varietiesDic = new Dictionary<string, USeProduct>();
        }
        #endregion

        #region methods
        /// <summary>
        /// 初始化方法。
        /// </summary>
        public void Initialize()
        {
            string strSel = string.Format(@"select * from {0}.varieties
where exchange in ({1});", 
m_alphaDBName,USeTraderProtocol.GetInternalFutureMarketSqlString());
            DataTable table = MySQLDriver.GetTableFromDB(m_dbConnStr, strSel);

            List<USeProduct> productList = new List<USeProduct>();
            foreach (DataRow row in table.Rows)
            {
                string exchange = row["exchange"].ToString();
                USeProduct product = new USeProduct() {
                    ProductCode = row["code"].ToString(),
                    Market = (USeMarket)Enum.Parse(typeof(USeMarket), exchange),
                    ShortName = row["short_name"].ToString(),
                    LongName = row["long_name"].ToString(),
                    VolumeMultiple = Convert.ToDecimal(row["volume_multiple"]),
                    PriceTick = Convert.ToDecimal(row["price_tick"])
                };
                productList.Add(product);
            }

            m_varietiesDic.Clear();
            foreach (USeProduct product in productList)
            {
                m_varietiesDic.Add(product.ProductCode, product);
            }
        }

        /// <summary>
        /// 获取产品信息。
        /// </summary>
        /// <param name="productId">品种代码。</param>
        /// <returns></returns>
        public USeProduct GetPruduct(string productId)
        {
            USeProduct product = null;
            if (m_varietiesDic.TryGetValue(productId, out product) == false)
            {
                Debug.Assert(false);
            }

            return product;
        }
        #endregion
    }
}
