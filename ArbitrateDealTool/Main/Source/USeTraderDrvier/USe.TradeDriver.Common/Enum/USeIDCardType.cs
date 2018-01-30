#region Copyright & Version
//==============================================================================
// 文件名称:  USeIDCardType.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2013 USe (Shanghai) Tec. & Res. CO., Ltd.
// 
// 创 建 人: Yang Ming
// 创建日期: 2013/05/15
// 描    述: 投资者身份证件类型枚举定义。
//==============================================================================
#endregion

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 投资者身份证件类型枚举。
    /// </summary>
    public enum USeIDCardType : byte
    {
        /// <summary>
        /// 组织机构代码。
        /// </summary>
        OrganizationCode = (byte)'0',

        /// <summary>
        /// 身份证。
        /// </summary>
        IDCard = (byte)'1',

        /// <summary>
        /// 军官证。
        /// </summary>
        MilitaryOfficer = (byte)'2',

        /// <summary>
        /// 警官证。
        /// </summary>
        PoliceOfficer = (byte)'3',

        /// <summary>
        /// 士兵证。
        /// </summary>
        Soldier = (byte)'4',

        /// <summary>
        /// 户口本。
        /// </summary>
        HouseholdRegister = (byte)'5',

        /// <summary>
        /// 护照。
        /// </summary>
        Passport = (byte)'6',

        /// <summary>
        /// 台胞证。
        /// </summary>
        MTPs = (byte)'7',

        /// <summary>
        /// 回乡证。
        /// </summary>
        HVPs = (byte)'8',

        /// <summary>
        /// 营业执照。
        /// </summary>
        BusinessLicense = (byte)'9',

        /// <summary>
        /// 其它证件。
        /// </summary>
        Other = (byte)'x',
    }
}
