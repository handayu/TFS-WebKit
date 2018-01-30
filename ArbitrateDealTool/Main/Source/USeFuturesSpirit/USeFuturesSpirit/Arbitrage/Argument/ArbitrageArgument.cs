using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using System.Diagnostics;

namespace USeFuturesSpirit.Arbitrage
{
    /// <summary>
    /// 套利单参数。
    /// </summary>
    public class ArbitrageArgument
    {
        /// <summary>
        /// 品种。
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// 近月合约。
        /// </summary>
        public USeInstrument NearInstrument { get; set; }

        /// <summary>
        /// 远月合约。
        /// </summary>
        public USeInstrument FarInstrument { get; set; }

        /// <summary>
        /// 操作方向。
        /// </summary>
        public ArbitrageOperationSide OperationSide { get; set; }


        /// <summary>
        /// 开仓参数。
        /// </summary>
        public ArbitrageOpenArgument OpenArg { get; set; }

        /// <summary>
        /// 平仓参数。
        /// </summary>
        public ArbitrageCloseArgument CloseArg { get; set; }

        /// <summary>
        /// 平仓止损参数。
        /// </summary>
        public ArbitrageStopLossArgument StopLossArg { get; set; }

        /// <summary>
        /// 预警参数。
        /// </summary>
        public List<ArbitrageAlarmArgument> AlarmArgs { get; set; }

        public ArbitrageArgument Clone()
        {
            ArbitrageArgument arg = new ArbitrageArgument();
            arg.ProductID = this.ProductID;
            if (this.NearInstrument != null)
            {
                arg.NearInstrument = this.NearInstrument.Clone();
            }
            if (this.FarInstrument != null)
            {
                arg.FarInstrument = this.FarInstrument.Clone();
            }
            arg.OperationSide = this.OperationSide;

            if (this.OpenArg != null)
            {
                arg.OpenArg = this.OpenArg.Clone();
            }
            if (this.CloseArg != null)
            {
                arg.CloseArg = this.CloseArg.Clone();
            }
            if (this.StopLossArg != null)
            {
                arg.StopLossArg = this.StopLossArg.Clone();
            }
            if (this.AlarmArgs != null)
            {
                List<ArbitrageAlarmArgument> alarmArgList = new List<ArbitrageAlarmArgument>();
                foreach (ArbitrageAlarmArgument alarmArg in this.AlarmArgs)
                {
                    alarmArgList.Add(alarmArg.Clone());
                }

                arg.AlarmArgs = alarmArgList;
            }

            return arg;
        }

        /// <summary>
        /// 参数内部校验
        /// </summary>
        /// <returns></returns>
        public bool Vefify(out string errorMessage)
        {
            errorMessage = "参数内部校验无误";

            if (VefifyNullArgs(out errorMessage) == false) //参数空值检测
            {
                return false;
            }
            else if (VerfifyNullInArgs(out errorMessage) == false)//参数内部参数检测
            {
                return false;
            }
            else if (VerfifyArgsRelation(out errorMessage) == false)//空值检测完-参数相关关系检测
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 参数相关关系校验
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool VerfifyArgsRelation(out string errorMessage)
        {
            errorMessage = "";
            if (this.OperationSide == ArbitrageOperationSide.BuyNearSellFar)
            {
                Debug.Assert(OpenArg.BuyInstrument != null);
                Debug.Assert(OpenArg.SellInstrument != null);
                Debug.Assert(NearInstrument != null);
                Debug.Assert(FarInstrument != null);

                if (this.OpenArg.BuyInstrument != this.NearInstrument || this.OpenArg.SellInstrument != this.FarInstrument)
                {
                    errorMessage = "买近卖远设定下，买方合约应该为近月合约，卖方合约应该为远月合约";
                    return false;
                }
            }
            else if(this.OperationSide == ArbitrageOperationSide.SellNearBuyFar)
            {
                Debug.Assert(OpenArg.BuyInstrument != null);
                Debug.Assert(OpenArg.SellInstrument != null);
                Debug.Assert(NearInstrument != null);
                Debug.Assert(FarInstrument != null);

                if (this.OpenArg.BuyInstrument != this.FarInstrument || this.OpenArg.SellInstrument != this.NearInstrument)
                {
                    errorMessage = "卖近买远设定下，卖方合约应该为近月合约，买方合约应该为远月合约";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 参数内部参数空值校验
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool VerfifyNullInArgs(out string errorMessage)
        {
            errorMessage = "";
            if (this.NearInstrument.InstrumentCode == "" || this.NearInstrument.InstrumentCode == null
                || this.FarInstrument.InstrumentCode == "" || this.FarInstrument.InstrumentCode == null)
            {
                errorMessage = "近月和远月合约合约代码不能为空字符串和空";
                return false;
            }
            else if (OpenArg.BuyInstrument == null || OpenArg.SellInstrument == null ||
                OpenArg.BuyInstrument.InstrumentCode == "" || OpenArg.SellInstrument.InstrumentCode == "")
            {
                errorMessage = "开仓参数的买方合约和卖方合约不能为空，合约代码不能为空";
                return false;
            }
            else if(CloseArg.BuyInstrument == null || CloseArg.SellInstrument == null ||
                CloseArg.BuyInstrument.InstrumentCode == "" || CloseArg.SellInstrument.InstrumentCode == "")
            {
                errorMessage = "平仓参数的买方合约和卖方合约不能为空，合约代码不能为空";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 参数空值校验
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool VefifyNullArgs(out string errorMessage)
        {
            errorMessage = "";
            if (this.ProductID == "" || this.ProductID == null)
            {
                errorMessage = "参数品种不能为空字符串";
                return false;
            }
            else if (this.NearInstrument == null || this.FarInstrument == null)
            {
                errorMessage = "近远月合约参数不能为空";
                return false;
            }
            else if (this.OpenArg == null || this.CloseArg == null)
            {
                errorMessage = "开仓参数和平仓参数不能为空";
                return false;
            }
            return true;
        }
    }
}
