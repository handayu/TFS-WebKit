
namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 摘牌核销请求
    /// </summary>
    public class WriteOffRequest : BaseRequest
    {   
        public string confirmPrice;//确认价格
        public string orderNo;//订单号
        public string remarks;//备注
    }

    /// <summary>
    /// 核销请求的响应
    /// </summary>
    public class WriteOffResponse : BaseResponse
    {
        public WriteOff data;
        public WriteOff result;
    }

    /// <summary>
    /// 核销信息
    /// </summary>
    public class WriteOff : BaseRequest
    {
        public string orderNo;//订单号
        public int cId;//品类
        public string confirmPrice;//确认价格
        public string confirmStatus;//1-待我方确认价格 2-待对方确认价格 3-双方价格不符 4-核销成功
        public string confirmStatusInfo;//状态描述
    }
}
