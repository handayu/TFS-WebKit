using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 保证金比例请求
    /// </summary>
    public class MaginRadioRequest
    {
        //categoryId 是int 品类ID（eq：1 cu 2 al）
        public string categoryId;
    }

    /// <summary>
    /// 保证金比例响应
    /// </summary>
    public class MaginRadioResponse : BaseResponse
    {
        //  "code": "string",
        //  "data": {
        //    "categoryId": 0,
        //    "companyId": 0,
        //    "name": "string",
        //    "unitName": "string",
        //    "unitSymbol": "string",
        //    "unitValue": 0
        //}
        //  "msg": "string",
        //  "success": true,
        //  "timeStamp": "string"
        //}
        public string timeStamp;

        public MaginRadio data;

    }

    /// <summary>
    /// 保证金比例信息
    /// </summary>
    public class MaginRadio
    {
        public string categoryId;
        public string companyId;
        public string name;
        public string unitName;
        public string unitSymbol;
        public string unitValue;
    }
}
