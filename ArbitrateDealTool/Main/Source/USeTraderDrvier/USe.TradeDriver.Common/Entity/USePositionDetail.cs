#region Copyright & Version
//==============================================================================
// 文件名称: USePositionDetail.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/05/07
// 描    述: USe持仓明细信息定义。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// USe持仓明细信息定义。
    /// </summary>
    public class USePositionDetail
    {
        #region property
        /// <summary>
        /// 每笔建仓生成一个ID。
        /// </summary>
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// 合约。
        /// </summary>
        public USeInstrument Instrument
        {
            get;
            set;
        }

        /// <summary>
        /// 持仓多空方向。
        /// </summary>
        public USeDirection Direction
        {
            get;
            set;
        }

        /// <summary>
        /// 持仓类型。
        /// </summary>
        public USePositionType PositionType
        {
            get;
            set;
        }

        ///// <summary>
        ///// 每点价值。
        ///// </summary>
        //public int VolumeMultiple
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 建仓数量(昨日仓为所有留仓数量)。
        /// </summary>
        public int OpenQty
        {
            get;
            set;
        }

        /// <summary>
        /// 建仓价格。
        /// </summary>
        /// <remarks>
        /// 今日开仓为开仓价格，昨日开仓为昨日结算价。
        /// </remarks>
        public decimal OpenPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 建仓时间。
        /// </summary>
        public DateTime OpenTime
        {
            get;
            set;
        }

        /// <summary>
        /// 平仓量。
        /// </summary>
        public int CloseQty
        {
            get;
            set;
        }

        /// <summary>
        /// 剩余持仓量。
        /// </summary>
        public int RemainQty
        {
            get { return this.OpenQty - this.CloseQty; }
        }

        /// <summary>
        /// 平仓金额。
        /// </summary>
        public decimal CloseAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 平仓平均价。
        /// </summary>
        public decimal ClosePrice
        {
            get;
            set;
        }
        #endregion //

        /// <summary>
        /// 按建仓时间正向排序。
        /// </summary>
        /// <param name="detail1"></param>
        /// <param name="detail2"></param>
        /// <returns></returns>
        public static int SortByOpenTimeAsc(USePositionDetail detail1, USePositionDetail detail2)
        {
            if (detail1.OpenTime < detail2.OpenTime)
            {
                return -1;
            }
            else if (detail1.OpenTime > detail2.OpenTime)
            {
                return 1;
            }
            else
            {
                return detail1.ID.CompareTo(detail2.ID);
            }
        }

        /// <summary>
        /// 克隆USePositionDetail对象。
        /// </summary>
        /// <returns></returns>
        public USePositionDetail Clone()
        {
            USePositionDetail entity = new USePositionDetail();
            entity.ID = this.ID;
            entity.Instrument = this.Instrument.Clone();
            entity.Direction = this.Direction;
            entity.PositionType = this.PositionType;
            entity.OpenQty = this.OpenQty;
            entity.OpenPrice = this.OpenPrice;
            entity.OpenTime = this.OpenTime;
            entity.CloseQty = this.CloseQty;
            entity.CloseAmount = this.CloseAmount;
            entity.ClosePrice = this.ClosePrice;

            return entity;
        }

    }
}
