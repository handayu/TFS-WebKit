#region Copyright & Version
//==============================================================================
// 文件名称: USeFee.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 合约手续费定义。
//
// 修 改 人: Yang Ming
// 修改日期: 2014/04/03
// 描    述: 为期权增加执行手续费。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 合约手续费。
    /// </summary>
    public class USeFee
    {
        #region property
        /// <summary>
        /// 合约。
        /// </summary>
        public USeInstrument Instrument
        {
            get;
            set;
        }

        /// <summary>
        /// 开仓手续费率。
        /// </summary>
        public decimal OpenRatioByMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 开仓手续费。
        /// </summary>
        public decimal OpenRatioByVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 平仓手续费率。
        /// </summary>
        public decimal CloseRatioByMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 平仓手续费
        /// </summary>
        public decimal CloseRatioByVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 平今仓手续费率
        /// </summary>
        public decimal CloseTodayRatioByMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 平今仓手续费。
        /// </summary>
        public decimal CloseTodayRatioByVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 执行手续费率(期权特有)。
        /// </summary>
        public decimal StrikeRatioByMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 执行手续费(期权特有)。
        /// </summary>
        public decimal StrikeRatioByVolume
        {
            get;
            set;
        }
        #endregion // property

        /// <summary>
        /// 克隆USeFee对象。
        /// </summary>
        /// <returns></returns>
        public USeFee Clone()
        {
            USeFee fee = new USeFee();
            fee.Instrument = this.Instrument == null? null: this.Instrument.Clone();
            fee.OpenRatioByMoney = this.OpenRatioByMoney;
            fee.OpenRatioByVolume = this.OpenRatioByVolume;
            fee.CloseRatioByMoney = this.CloseRatioByMoney;
            fee.CloseRatioByVolume = this.CloseRatioByVolume;
            fee.CloseTodayRatioByMoney = this.CloseTodayRatioByMoney;
            fee.CloseTodayRatioByVolume = this.CloseTodayRatioByVolume;
            fee.StrikeRatioByMoney = this.StrikeRatioByMoney;
            fee.StrikeRatioByVolume = this.StrikeRatioByVolume;

            return fee;
        }
    }
}
