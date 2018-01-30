using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 资金响应
    /// </summary>
    public class FundsResponse :BaseResponse
    {
        public string timeStamp;
        public Funds data;
    }
    /// <summary>
    /// 资金信息
    /// </summary>
    public class Funds
    {
        public string accountName;
        public string accountNo;
        public string availableAmount;
        public string bankName;
        public string freezeAmount;
        public string subAccNo;
    }
}
