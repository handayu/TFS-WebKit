using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using System.Xml.Serialization;


namespace USeFuturesSpirit
{
    public class ArbitrageCombineInstrument
    {

        /// <summary>
        /// 品种
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// 套利合约第一腿合约详情
        /// </summary>
        public USeInstrument FirstInstrument { get; set; }

        /// <summary>
        /// 套利合约第二腿合约详情
        /// </summary>
        public USeInstrument SecondInstrument { get; set; }


        /// <summary>
        /// 套利合约第一腿合约代码
        /// </summary>
        public string ArbitrageInstrumentOneCode
        {
            get
            {
                if (FirstInstrument == null)
                {
                    return string.Empty;
                }
                else
                {
                    return FirstInstrument.InstrumentCode;
                }
            }
        }


        /// <summary>
        /// 套利合约第二腿合约代码
        /// </summary>
        public string ArbitrageInstrumentTwoCode
        {
            get
            {
                if (SecondInstrument == null)
                {
                    return string.Empty;
                }
                else
                {
                    return SecondInstrument.InstrumentCode;
                }
            }
        }

        /// <summary>
        /// 套利合约组合别名。
        /// </summary>
        public string ArbitrageAlisa
        {
            get
            {
                if (FirstInstrument == null || SecondInstrument == null)
                {
                    return string.Empty;
                }
                else
                {
                    return FirstInstrument.InstrumentCode + " & " + SecondInstrument.InstrumentCode;
                }

            }
        }


        public override string ToString()
        {
            return this.ArbitrageAlisa;
        }

        public static ArbitrageCombineInstrument Clone(ArbitrageCombineInstrument tempIns)
        {
            ArbitrageCombineInstrument ins = new ArbitrageCombineInstrument();
            ins.FirstInstrument = tempIns.FirstInstrument;
            ins.SecondInstrument = tempIns.SecondInstrument;
            ins.ProductID = tempIns.ProductID;
            return ins;
        }
    }
}
