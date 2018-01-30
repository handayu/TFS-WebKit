using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 基础请求
    /// </summary>
    public class BaseRequest
    {
        /// <summary>
        /// 请求的客户ID
        /// </summary>
        public string mqId;

        /// <summary>
        /// 客户的ClientID
        /// </summary>
        public string clientId;

        /// <summary>
        /// 商品ID
        /// </summary>
        public long commId;

        /// <summary>
        /// 请求类型-操作-1挂牌，2摘牌，3锁定，4撤单,5核销
        /// </summary>
        public int operationType;

    }

    /// <summary>
    /// 基础响应
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string Code;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg;

        /// <summary>
        /// 返回状态
        /// </summary>
        public bool Success;
    }

    #region 枚举
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        private string description;
        public string Description { get { return description; } }

        public EnumDescriptionAttribute(string description)
            : base()
        {
            this.description = description;
        }
    }
    /// <summary>
    /// 操作类型枚举
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 操作类型-1挂牌
        /// </summary>
        [EnumDescription("1")]
        PutBrand = 1,

        /// <summary>
        /// 操作类型-2摘牌
        /// </summary>
        [EnumDescription("2")]
        DelistBrand,

        /// <summary>
        /// 操作类型-3锁定
        /// </summary>
        [EnumDescription("3")]
        LockBrand,
        /// <summary>
        /// 操作类型-4撤单
        /// </summary>
        [EnumDescription("4")]
        ActionOrder,

        /// <summary>
        /// 操作类型-5未知类型
        /// </summary>
        [EnumDescription("5")]
        UnKnow
    }

    /// <summary>
    /// 买卖类型枚举
    /// </summary>
    public enum TransType
    {
        /// <summary>
        /// 采购
        /// </summary>
        ///        
        [EnumDescription("采购")]
        Buy = 0,

        /// <summary>
        /// 销售
        /// </summary>
        [EnumDescription("销售")]
        Sell = 1,

        /// <summary>
        /// 未知买卖类型
        /// </summary>
        [EnumDescription("未知")]
        Unknow = 2
    }

    /// <summary>
    /// 是否显示公司名
    /// </summary>
    public enum ISCoperationNameVisual
    {
        /// <summary>
        /// 显示
        /// </summary>
        [EnumDescription("显示")]
        Visual = 0,

        /// <summary>
        /// 不显示
        /// </summary>
        [EnumDescription("不显示")]
        UnViusal = 1,

        /// <summary>
        /// 未知
        /// </summary>
        [EnumDescription("未知")]
        Unknow = 2
    }

    /// <summary>
    /// 核销状态
    /// </summary>
    public enum ConfirmStatus
    {
        /// <summary>
        /// 等待我方确认
        /// </summary>
        [EnumDescription("等待我方确认")]
        WaitSelfComfirm = 1,

        /// <summary>
        /// 等待对方确认
        /// </summary>
        [EnumDescription("等待对方确认")]
        WaitOppositeConfirm = 2,

        /// <summary>
        /// 双方价格不符
        /// </summary>
        [EnumDescription("双方价格不符")]
        NotMatch = 3,

        /// <summary>
        /// 核销成功
        /// </summary>
        [EnumDescription("核销成功")]
        VerifySuccessed = 4,

        /// <summary>
        /// 未知
        /// </summary>
        [EnumDescription("未知")]
        UnKnow,
    }

    /// <summary>
    /// 摘牌价格类型
    /// </summary>
    public enum PricingMethod
    {
        /// <summary>
        ///点价
        /// </summary>
        [EnumDescription("点价")]
        SpotPrice = 0,

        /// <summary>
        /// 一口价
        /// </summary>
        [EnumDescription("一口价")]
        DeadPrice = 1,
    }

    /// <summary>
    /// HttpServe枚举
    /// </summary>
    public enum HTTPServiceUrlCollection
    {
        /// <summary>
        /// 获取余额和资金使用率
        /// </summary>
        GetAvaliableQualityCash,

        /// <summary>
        /// 获取授信额度
        /// </summary>
        GetDelistReditQuality,

        /// <summary>
        /// 获取保证金比例[hanyu URL需要确认一下]
        /// </summary>
        GetCommIdMarginRadio,

        /// <summary>
        /// 获取合约基础信息
        /// </summary>
        GetBaseInstrumentsInfoUrl,

        /// <summary>
        /// 获取所有挂牌列表信息
        /// </summary>
        GetList,

        /// <summary>
        /// 获取自己挂牌列表信息
        /// </summary>
        GetMyPutBrandList,

        /// <summary>
        /// 获取个人信息
        /// </summary>
        GetPersonalInformation,

        /// <summary>
        /// 获取仓库基本信息
        /// </summary>
        GetBaseWareHouseInfoUrl,
        /// <summary>
        /// 获取指定仓库URL
        /// </summary>
        GetWareHouseInfoUrl,

        /// <summary>
        /// 品牌等级信息
        /// </summary>
        GetBaseLevelBrandInfoUrl,

        /// <summary>
        /// 摘牌成交列表
        /// </summary>
        GetDelistTradedListInfoUrl,

        /// <summary>
        /// 挂牌操作
        /// </summary>
        PostBrandOrderRequireInfoUrl,

        /// <summary>
        /// 摘牌操作
        /// </summary>
        PostDelistBrandOrderRequireInfoUrl,

        /// <summary>
        /// 核销操作
        /// </summary>
        PostDelistBrandOrderVerifyRequireInfoUrl,

        /// <summary>
        /// 撤单操作
        /// </summary>
        PostActionBrandOrderRequireInfoUrl,

        /// <summary>
        /// 登录请求
        /// </summary>
        LoginHttp,

        /// <summary>
        /// 登录操作
        /// </summary>
        Login,

        /// <summary>
        /// 登出操作
        /// </summary>
        LoginOff,

        /// <summary>
        /// 历史交易查询
        /// </summary>
        History,

        /// <summary>
        /// 基础管理
        /// </summary>
        BasicManagement,

        /// <summary>
        /// 注册
        /// </summary>
        Registered,

        /// <summary>
        /// 黑白名单
        /// </summary>
        GetBlackAndWhiteList,

        /// <summary>
        /// 黑名启用状态
        /// </summary>
        GetBlackState,

        /// <summary>
        /// 白名单启用状态
        /// </summary>
        GetWhiteState,

        ///
        ///判断当前客户是否存在于挂牌方的黑名单中
        ///
        GetP2PBlackState,
    }
    #endregion
}
