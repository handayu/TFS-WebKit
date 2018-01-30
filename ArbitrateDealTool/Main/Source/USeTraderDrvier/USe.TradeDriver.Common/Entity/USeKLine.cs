using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 市场K线数据定义
    /// </summary>
    public sealed class USeKLine
    {
        #region
        /// <summary>
        /// 合约。
        /// </summary>
        public string InstrumentCode { get; set; }

        /// <summary>
        /// 市场。
        /// </summary>
        public USeMarket Market { get; set; }

        /// <summary>
        /// 周期。
        /// </summary>
        public USeCycleType Cycle { get; set; }

        /// <summary>
        /// 时间。
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 开盘价。
        /// </summary>
        public decimal Open { get; set; }

        /// <summary>
        /// 最高价。
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// 最低价。
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// 收盘价。
        /// </summary>
        public decimal Close { get; set; }

        /// <summary>
        /// 累计成交量。
        /// </summary>
        public int Volumn { get; set; }

        /// <summary>
        /// 累计成交金额。
        /// </summary>
        public decimal Turnover { get; set; }

        /// <summary>
        /// 累计持仓量。
        /// </summary>
        public decimal OpenInterest { get; set; }

        /// <summary>
        /// 结算价。
        /// </summary>
        public decimal SettlementPrice { get; set; }

        /// <summary>
        /// 昨日结算价。
        /// </summary>
        public decimal PreSettlementPrice { get; set; }

        /// <summary>
        /// 内盘(主动性卖盘)。
        /// </summary>
        public int AskVolumn { get; set; }

        /// <summary>
        /// 外盘(主动行买盘)。
        /// </summary>
        public int BidVolumn { get; set; }

        /// <summary>
        /// 沉淀资金
        /// </summary>
        public decimal SendimentaryMoney { get; set; }

        /// <summary>
        /// 资金流入流出
        /// </summary>
        public decimal FlowFund { get; set; }

        /// <summary>
        /// 平均价
        /// </summary>
        public double AvgPrice { get; set; }

        /// <summary>
        /// 投机度
        /// </summary>
        public decimal SpeculateRadio { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("[{0}]", this.InstrumentCode));
            if(this.Cycle == USeCycleType.Day)
            {
                sb.Append(string.Format("[{0:yyyyMMdd}]", this.DateTime));
            }
            else
            {
                sb.Append(string.Format("[{0:yyyyMMdd HHmm}]", this.DateTime));
            }
            sb.Append(string.Format("[Open:{0}]", this.Open));
            sb.Append(string.Format("[High:{0}]", this.High));
            sb.Append(string.Format("[Low:{0}]", this.Low));
            sb.Append(string.Format("[Close:{0}]", this.Close));
            sb.Append(string.Format("[Volumn:{0}]", this.Volumn));
            sb.Append(string.Format("[Turnover:{0}]", this.Turnover));
            sb.Append(string.Format("[OpenInterest:{0}]", this.OpenInterest));
            sb.Append(string.Format("[AskVolumn:{0}", this.AskVolumn));
            sb.Append(string.Format("[BidVolumn:{0}", this.BidVolumn));
            sb.Append(string.Format("[AvgPrice:{0}", this.AvgPrice));
            sb.Append(string.Format("[SendimentaryMoney:{0}", this.SendimentaryMoney));
            sb.Append(string.Format("[FlowFund:{0}", this.FlowFund));
            sb.Append(string.Format("[SpeculateRadio:{0}", this.SpeculateRadio));

            return sb.ToString();
        }
        /// <summary>
        /// 克隆。
        /// </summary>
        /// <returns></returns>
        public USeKLine Clone()
        {
            USeKLine data = new USeKLine();
            data.InstrumentCode = this.InstrumentCode;
            data.Market = this.Market;
            data.Cycle = this.Cycle;
            data.DateTime = this.DateTime;
            data.Open = this.Open;
            data.High = this.High;
            data.Low = this.Low;
            data.Close = this.Close;
            data.PreSettlementPrice = this.PreSettlementPrice;
            data.SettlementPrice = this.SettlementPrice;
            data.Volumn = this.Volumn;
            data.Turnover = this.Turnover;
            data.OpenInterest = this.OpenInterest;
            data.AskVolumn = this.AskVolumn;
            data.BidVolumn = this.BidVolumn;
            data.SendimentaryMoney = this.SendimentaryMoney;
            data.FlowFund = this.FlowFund;
            data.AvgPrice = this.AvgPrice;
            data.SpeculateRadio = this.SpeculateRadio;
            return data;
        }
    }
}
