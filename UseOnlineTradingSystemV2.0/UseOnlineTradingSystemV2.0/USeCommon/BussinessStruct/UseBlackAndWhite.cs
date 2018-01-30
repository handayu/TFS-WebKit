using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 判断是否是对方黑名单
    /// </summary>
    public class BlackState : BaseResponse
    {
        /// <summary>
        /// 数据响应
        /// </summary>
        public string result;
    }

    /// <summary>
    /// 黑白名单状态
    /// </summary>
    public class BlackWhiteState : BaseResponse
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string timeStamp;
        /// <summary>
        /// 响应数据
        /// </summary>
        public string data;
    }

    /// <summary>
    /// 黑白名单列表
    /// </summary>
    public class BlackAndWhiteList : BaseResponse
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string timeStamp;

        /// <summary>
        /// 黑白名单页
        /// </summary>
        public BlackAndWhiteListPage data;
    }

    /// <summary>
    /// 黑白名单页
    /// </summary>
    public class BlackAndWhiteListPage
    {
        /// <summary>
        /// 页码
        /// </summary>
        public string pageNum;
        public string  total;
        public string  pages;
        /// <summary>
        /// 黑白名单数据集
        /// </summary>
        public List<BlackAndWhite> dataList;
    }

    /// <summary>
    /// 黑白名单单条数据
    /// </summary>
    public class BlackAndWhite
    {
        /// <summary>
        /// id
        /// </summary>
        public string id;
        /// <summary>
        /// 公司ID
        /// </summary>
        public string companyId;
        /// <summary>
        /// 公司名称
        /// </summary>
        public string companyName;
        /// <summary>
        /// 类型
        /// </summary>
        public string type;
        /// <summary>
        /// 创建日期
        /// </summary>
        public string createdDate;
    }
}
