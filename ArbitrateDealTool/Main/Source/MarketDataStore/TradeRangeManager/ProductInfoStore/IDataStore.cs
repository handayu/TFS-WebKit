using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeRangeManager
{
    /// <summary>
    /// 行情数据保存器接口。
    /// </summary>
    public interface IDataStore
    {
        /// <summary>
        /// 是否有错误。
        /// </summary>
        bool HasError { get; }

        /// <summary>
        /// 已存储数量。
        /// </summary>
        int StoreCount { get; }

        /// <summary>
        /// 未存储数量。
        /// </summary>
        int UnStoreCount { get; }

        /// <summary>
        /// 保存K线数据。
        /// </summary>
        /// <param name="kLine"></param>
        //void SaveKLine(KLineData kLine);

        /// <summary>
        /// 启动存储器。
        /// </summary>
        void Start();

        /// <summary>
        /// 停止存储器。
        /// </summary>
        void Stop();

        /// <summary>
        /// 重置。
        /// </summary>
        void Reset();
    }
}
