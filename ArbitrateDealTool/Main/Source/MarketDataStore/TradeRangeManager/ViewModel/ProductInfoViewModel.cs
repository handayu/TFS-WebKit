using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common;


namespace TradeRangeManager
{
    public class ProductInfoViewModel : USeBaseViewModel
    {
        #region member
        private string m_exchange = string.Empty;
        private string m_name = string.Empty;
        //private string m_productName = string.Empty;
        #endregion

        #region property
        /// <summary>
        /// 交易所。
        /// </summary>
        public string Exchange
        {
            get { return m_exchange; }
            set
            {
                m_exchange = value;
                SetProperty(() => this.Exchange);
            }
        }

        /// <summary>
        /// 品种。
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                SetProperty(() => this.Name);

            }
        }

        ///// <summary>
        ///// 品种名称。
        ///// </summary>
        //public string ProductName
        //{
        //    get { return m_productName; }
        //    set
        //    {
        //        m_productName = value;
        //        SetProperty(() => this.ProductName);

        //    }
        //}

        #endregion

        #region Construct
        public static ProductInfoViewModel Creat(ProductTradeRangeInfo sectionInfo)
        {
            ProductInfoViewModel data_model = new ProductInfoViewModel();
            data_model.Exchange = sectionInfo.Exchange;
            data_model.Name = sectionInfo.Name;
            //data_model.ProductName = sectionInfo.ProductrName;

            return data_model;
        }
        #endregion
    }
}

