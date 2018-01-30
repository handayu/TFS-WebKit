using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 仓库信息响应
    /// </summary>
    public class WareHouseResponse : BaseResponse
    {
        public List<WareHouseInfo> Result;
    }

    /// <summary>
    /// 仓库信息
    /// </summary>
    public class WareHouseInfo
    {
        public string id;//序号
        public string warehouseName;//仓库名称
        public string imgSrc;       //图片地址
        public string description;  
        public string updatedDate;
        public string cates;

        public override string ToString()
        {
            return warehouseName;
        }

    }
}
