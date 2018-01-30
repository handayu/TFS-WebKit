#region Copyright & Version
//==============================================================================
// 文件名称: USeFundInfo.cs
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
    /// 资金账户信息。
    /// </summary>
    public class USeFund
    {
        #region property
        /// <summary>
        /// 账户。
        /// </summary>
        public string AccountID
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
        /// 上次信用额度。
        /// </summary>
        public decimal PreCredit
        {
            get;
            set;
        }

        /// <summary>
        /// 上次结算准备金(上日结存)。
        /// </summary>
        public decimal PreBalance
        {
            get;
            set;
        }

        /// <summary>
        /// 入金金额。
        /// </summary>
        public decimal Deposit
        {
            get;
            set;
        }

        /// <summary>
        /// 出金金额。
        /// </summary>
        public decimal WithDraw
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
        /// 交割保证金。
        /// </summary>
        public decimal DeliveryMargin
        {
            get;
            set;
        }
        #endregion // property

        /// <summary>
        /// 克隆USeFund对象。
        /// </summary>
        /// <returns></returns>
        public USeFund Clone()
        {
            USeFund accountInfo = new USeFund();
            accountInfo.AccountID = this.AccountID;
            accountInfo.PreMortgage = this.PreMortgage;
            accountInfo.PreCredit = this.PreCredit;
            accountInfo.PreBalance = this.PreBalance;
            accountInfo.Deposit = this.Deposit;
            accountInfo.WithDraw = this.WithDraw;
            accountInfo.Mortgage = this.Mortgage;
            accountInfo.DeliveryMargin = this.DeliveryMargin;

            return accountInfo;
        }
    }
}
