using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using USe.TradeDriver.Common;
using CTPAPI;

namespace USe.TradeDriver.Ctp
{
    public partial class CtpOrderDriver
    {
        /// <summary>
        /// 数据缓冲区域。
        /// </summary>
        private partial class CtpDataBuff
        {
            /// <summary>
            /// 计算手续费。
            /// </summary>
            /// <param name="instrument"></param>
            /// <param name="offsetType"></param>
            /// <param name="qty"></param>
            /// <param name="price"></param>
            /// <returns></returns>
            public decimal CalculateFee(USeInstrument instrument, USeOffsetType offsetType, int qty, decimal price)
            {
                USeFee fee = GetFee(instrument);
                if (fee == null)
                {
                    return 0m;
                }

                int volumeMultiple = GetVolumeMultiple(instrument);

                switch (offsetType)
                {
                    case USeOffsetType.Open: return (fee.OpenRatioByMoney * qty * price * volumeMultiple + fee.OpenRatioByVolume * qty);
                    case USeOffsetType.CloseHistory:
                    case USeOffsetType.Close: return (fee.CloseRatioByMoney * qty * price * volumeMultiple + fee.CloseRatioByVolume * qty);
                    case USeOffsetType.CloseToday: return (fee.CloseTodayRatioByMoney * qty * price * volumeMultiple + fee.CloseTodayRatioByVolume * qty);
                    default:
                        Debug.Assert(false);
                        return 0m;
                }
            }

            private decimal InternalCalculateFee(USeInstrument instrument, USeOffsetType offsetType, int qty, decimal price)
            {
                USeFee fee = InternalGetFee(instrument);
                if (fee == null)
                {
                    return 0m;
                }

                int volumeMultiple = InternalGetVolumeMultiple(instrument);

                switch (offsetType)
                {
                    case USeOffsetType.Open: return (fee.OpenRatioByMoney * qty * price * volumeMultiple + fee.OpenRatioByVolume * qty);
                    case USeOffsetType.CloseHistory:
                    case USeOffsetType.Close: return (fee.CloseRatioByMoney * qty * price * volumeMultiple + fee.CloseRatioByVolume * qty);
                    case USeOffsetType.CloseToday: return (fee.CloseTodayRatioByMoney * qty * price * volumeMultiple + fee.CloseTodayRatioByVolume * qty);
                    default:
                        Debug.Assert(false);
                        return 0m;
                }
            }


            /// <summary>
            /// 获取单品种中文名称。
            /// </summary>
            /// <param name="instrumentName"></param>
            /// <returns></returns>
            private string GetVarietiesName(string instrumentName)
            {
                int index = -1;
                for (int i = instrumentName.Length-1;i>=0;i--)
                {
                    if (char.IsDigit(instrumentName[i]) == false)
                    {
                        index = i;
                        break;
                    }
                }
                Debug.Assert(index >= 0);
                return instrumentName.Substring(0, index + 1);
            }
        }
    }
}
