
namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 撤牌请求
    /// </summary>
    public class CancelOrderRequest : BaseRequest
    {
        public string securityToken;
    }
    
    /// <summary>
    /// 撤牌响应
    /// </summary>
    public class CancelOrderResponse : BaseResponse
    {
        /// <summary>
        /// 撤牌响应
        /// </summary>
        public CancelOrder result;
    }

    /// <summary>
    /// 撤牌
    /// </summary>
    public class CancelOrder
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
        /// <summary>
        /// 状态。 1：挂牌 2：摘牌 3：锁定 4：下架
        /// </summary>
        public string transStatus;
        /// <summary>
        /// 卖和买。 0：采购 1：销售
        /// </summary>
        public string transType;
        /// <summary>
        /// 品类ID
        /// </summary>
        public string cid;
    }
}
