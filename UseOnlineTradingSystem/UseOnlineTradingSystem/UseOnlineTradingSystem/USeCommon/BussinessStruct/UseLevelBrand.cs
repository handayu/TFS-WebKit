using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 等级和品牌响应
    /// </summary>
    public class LevelBrandResponse : BaseResponse
    {
        public List<LevelBrandList> Result;
    }

    /// <summary>
    /// 等级和品牌列表
    /// </summary>
    public class LevelBrandList
    {
        public int id;
        public string infoName;
        public int infoType;
        public List<LevelBrand> infoList;

        public override string ToString()
        {
            return this.infoName;
        }

    }

    /// <summary>
    /// 等级和品牌信息
    /// </summary>
    public class LevelBrand
    {
        public int id;
        public string infoName;
        public string parentId;
        public string infoType;
        public string sort;
        public int cid;
        public string createdDate;
        public string createdBy;
        public string updatedDate;
        public string updatedBy;
        public string deleteFlag;

        public override string ToString()
        {
            return this.infoName;
        }
    }
}
