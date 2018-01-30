using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common;


namespace TradeRangeManager
{
    public class ProductTradeSectionViewModel : USeBaseViewModel
    {
        #region member
        private string m_beginTime = string.Empty;
        private string m_endTime = string.Empty;
        private bool m_isNight = false;
        #endregion

        #region property
        /// <summary>
        /// 开始时间。
        /// </summary>
        public string BeginTime
        {
            get { return m_beginTime; }
            set
            {
                m_beginTime = value;
                SetProperty(() => this.BeginTime);
            }
        }

        /// <summary>
        /// 结束时间。
        /// </summary>
        public string EndTime
        {
            get { return m_endTime; }
            set
            {
                m_endTime = value;
                SetProperty(() => this.EndTime);

            }
        }

        /// <summary>
        /// 是否为夜盘。
        /// </summary>
        public bool IsNight
        {
            get { return m_isNight; }
            set
            {
                m_isNight = value;
                SetProperty(() => this.IsNight);

            }
        }
        #endregion


        //#region Construct
        //public static TradeRangeTimeSectionInfo Creat(ProductTradeRangeInfo sectionInfo)
        //{
        //    TradeRangeTimeSectionInfo data_model = new TradeRangeTimeSectionInfo();
        //    data_model.BeginTime = sectionInfo.TradeRangeTimeSectionsInfo.;
        //    data_model.Name = sectionInfo.Name;

        //    return data_model;
        //}
        //#endregion
    }
}
