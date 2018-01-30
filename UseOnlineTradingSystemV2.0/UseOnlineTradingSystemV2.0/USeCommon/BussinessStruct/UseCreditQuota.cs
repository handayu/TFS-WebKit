using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 授信额度请求
    /// </summary>
    public class CreditQuotaRequest
    {
        public string compId;
    }

    /// <summary>
    /// 授信额度响应
    /// </summary>
    public class CreditQuotaResponse
    {
        public string code;
        public string msg;
        public CreditQuota data;
    }

    /// <summary>
    /// 授信额度
    /// </summary>
    public class CreditQuota
    {
        public string compId;
        public string blackId;
        public string creditCompId;
        public string creditCompName;
        public string creditLineAmt;
        public string creditUsedAmt;
    }
}
