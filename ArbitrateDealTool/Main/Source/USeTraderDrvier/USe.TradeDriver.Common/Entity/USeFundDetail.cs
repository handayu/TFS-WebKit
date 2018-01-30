#region Copyright & Version
//==============================================================================
// 文件名称: USeFundDetail.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: USe资金账户信息。
// 
// 修 改 人: Yang Ming
// 修改日期: 2014/04/22
// 描    述: 资金账户信息由USeAccountInfo重命名为USeFund。
//==============================================================================
#endregion
namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 资金明细信息。
    /// </summary>
    public class USeFundDetail
    {
        #region property
        /// <summary>
        /// 投资者帐号。
        /// </summary>
        public string AccountID
        {
            get;
            set;
        }

        /// <summary>
        /// 可用金额。
        /// </summary>
        public decimal Available
        {
            get;
            set;
        }

        /// <summary>
        /// 今日入金。
        /// </summary>
        public decimal Deposit
        {
            get;
            set;
        }

        /// <summary>
        /// 质押金额。
        /// </summary>
        public decimal Mortgage
        {
            get;
            set;
        }

        /// <summary>
        /// 上日结存。
        /// </summary>
        public decimal PreBalance
        {
            get;
            set;
        }

        /// <summary>
        /// 上次信用额度。
        /// </summary>
        public decimal PreCredit
        {
            get;
            set;
        }

        /// <summary>
        /// 上次质押金额。
        /// </summary>
        public decimal PreMortgage
        {
            get;
            set;
        }

        /// <summary>
        /// 今日出金。
        /// </summary>
        public decimal WithDraw
        {
            get;
            set;
        }

        /// <summary>
        /// 静态权益。
        /// </summary>
        public decimal StaticBenefit
        {
            get;
            set;
        }

        /// <summary>
        /// 平仓盈亏。
        /// </summary>
        public decimal CloseProfit
        {
            get;
            set;
        }

        /// <summary>
        /// 交易手续费。
        /// </summary>
        public decimal TradeFee
        {
            get;
            set;
        }

        /// <summary>
        /// 持仓盈亏。
        /// </summary>
        public decimal HoldProfit
        {
            get;
            set;
        }

        /// <summary>
        /// 占用保证金。
        /// </summary>
        public decimal HoldMargin
        {
            get;
            set;
        }

        /// <summary>
        /// 动态权益。
        /// </summary>
        public decimal DynamicBenefit
        {
            get;
            set;
        }

        /// <summary>
        /// 冻结保证金。
        /// </summary>
        public decimal FrozonMargin
        {
            get;
            set;
        }

        /// <summary>
        /// 冻结手续费。
        /// </summary>
        public decimal FrozonFee
        {
            get;
            set;
        }

        /// <summary>
        /// 下单冻结。
        /// </summary>
        public decimal Fronzon
        {
            get;
            set;
        }

        /// <summary>
        /// 风险度。
        /// </summary>
        public decimal Risk
        {
            get;
            set;
        }

        /// <summary>
        /// 可取资金。
        /// </summary>
        public decimal PreferCash
        {
            get;
            set;
        }
        #endregion // property

        public USeFundDetail Clone()
        {
            USeFundDetail entity = new USeFundDetail();
            entity.AccountID = this.AccountID;
            entity.Available = this.Available;
            entity.Deposit = this.Deposit;
            entity.Mortgage = this.Mortgage;
            entity.PreBalance = this.PreBalance;
            entity.PreCredit = this.PreCredit;
            entity.PreMortgage = this.PreMortgage;
            entity.WithDraw = this.WithDraw;
            entity.StaticBenefit = this.StaticBenefit;
            entity.CloseProfit = this.CloseProfit;
            entity.TradeFee = this.TradeFee;
            entity.HoldProfit = this.HoldProfit;
            entity.HoldMargin = this.HoldMargin;
            entity.DynamicBenefit = this.DynamicBenefit;
            entity.FrozonMargin = this.FrozonMargin;
            entity.FrozonFee = this.FrozonFee;
            entity.Fronzon = this.Fronzon;
            entity.Risk = this.Risk;
            entity.PreferCash = this.PreferCash;

            return entity;
        }
    }
}
