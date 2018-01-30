using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.TradeDriver.Ctp
{
    /// <summary>
    /// Ctp查询信息。
    /// </summary>
    internal abstract class CtpQueryInfo
    {
        public CtpQueryInfo()
        {
        }

        /// <summary>
        /// 查询数据类型。
        /// </summary>
        public abstract CtpQueryType QueryType
        {
            get;
        }
    }

    /// <summary>
    /// Ctp保证金查询。
    /// </summary>
    internal class CtpQueryMarginInfo : CtpQueryInfo
    {
        public CtpQueryMarginInfo(string instrumentCode)
        {
            this.InstrumentCode = instrumentCode;
        }

        /// <summary>
        /// 合约代码。
        /// </summary>
        public string InstrumentCode
        {
            get;
            private set;
        }

        /// <summary>
        /// 查询数据类型。
        /// </summary>
        public override CtpQueryType QueryType
        {
            get { return CtpQueryType.Margin; }
        }
    }

    /// <summary>
    /// Ctp手续费查询。
    /// </summary>
    internal class CtpQueryFeeInfo : CtpQueryInfo
    {
        public CtpQueryFeeInfo(string instrumentCode)
        {
            this.InstrumentCode = instrumentCode;
        }

        /// <summary>
        /// 合约代码。
        /// </summary>
        public string InstrumentCode
        {
            get;
            private set;
        }

        /// <summary>
        /// 查询数据类型。
        /// </summary>
        public override CtpQueryType QueryType
        {
            get { return CtpQueryType.Fee; }
        }
    }

    /// <summary>
    /// Ctp查询。
    /// </summary>
    internal class CtpQueryFundInfo : CtpQueryInfo
    {
        public CtpQueryFundInfo()
        {
        }

        /// <summary>
        /// 查询数据类型。
        /// </summary>
        public override CtpQueryType QueryType
        {
            get { return CtpQueryType.Fund; }
        }
    }

    /// <summary>
    /// 查询数据类型。
    /// </summary>
    internal enum CtpQueryType
    {
        /// <summary>
        /// 手续费。
        /// </summary>
        Fee = 0,

        /// <summary>
        /// 保证金。
        /// </summary>
        Margin = 1,

        /// <summary>
        /// 账户资金。
        /// </summary>
        Fund = 2,
    }
}
