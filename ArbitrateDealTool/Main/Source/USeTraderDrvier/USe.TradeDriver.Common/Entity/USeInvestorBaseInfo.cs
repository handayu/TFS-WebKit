#region Copyright & Version
//==============================================================================
// 文件名称: USeInvestorBaseInfo.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/27
// 描    述: 投资者帐户基本信息。
//==============================================================================
#endregion

using System;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 投资者帐户基本信息。
    /// </summary>
    public class USeInvestorBaseInfo
    {
        /// <summary>
        /// 投资者帐号。
        /// </summary>
        public string Account
        {
            get;
            set;
        }

        /// <summary>
        /// 经纪商ID。
        /// </summary>
        public string BrokerID
        {
            get;
            set;
        }

        /// <summary>
        /// 经纪商名称。
        /// </summary>
        public string BrokerName
        {
            get;
            set;
        }

        /// <summary>
        /// 身份证件类型。
        /// </summary>
        public USeIDCardType IdCardType
        {
            get;
            set;
        }

        /// <summary>
        /// 身份证件号码。
        /// </summary>
        public string IdentifiedCardNo
        {
            get;
            set;
        }

        /// <summary>
        /// 开户日期。
        /// </summary>
        public DateTime OpenDate
        {
            get;
            set;
        }

        /// <summary>
        /// 手机。
        /// </summary>
        public string Mobile
        {
            get;
            set;
        }

        /// <summary>
        /// 真实姓名。
        /// </summary>
        public string RealName
        {
            get;
            set;
        }

        public USeInvestorBaseInfo Clone()
        {
            USeInvestorBaseInfo entity = new USeInvestorBaseInfo();
            entity.Account = this.Account;
            entity.BrokerID = this.BrokerID;
            entity.BrokerName = this.BrokerName;
            entity.IdCardType = this.IdCardType;
            entity.IdentifiedCardNo = this.IdentifiedCardNo;
            entity.OpenDate = this.OpenDate;
            entity.Mobile = this.Mobile;
            entity.RealName = this.RealName;

            return entity;
        }
    }
}
