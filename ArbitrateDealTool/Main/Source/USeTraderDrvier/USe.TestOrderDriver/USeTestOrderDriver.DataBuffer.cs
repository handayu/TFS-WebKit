using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using USe.TradeDriver.Common;

namespace USe.TradeDriver.Test
{
    public partial class USeTestOrderDriver
    {
        public class DataBuffer
        {
            /// <summary>
            /// 下单委托列表
            /// </summary>
            private List<USeOrderBook> m_order_list = null;
            public List<USeOrderBook> OrderBookList
            {
                get { return m_order_list; }
                set { m_order_list = value; }
            }

            /// <summary>
            /// 持仓列表
            /// </summary>
            private List<USePosition> m_positionList = new List<USePosition>();
            public List<USePosition> PositionDataList
            {
                get { return m_positionList; }
                set { m_positionList = value; }

            }

            #region 创建默认持仓

            public List<USePosition> CreateDefaultPositonList()
            {
                List<USePosition> list = new List<USePosition>();
                list.Add(GetPositionCu1706Buy());
                list.Add(GetPositionCu1707Buy());
                list.Add(GetPositionCu1708Buy());
                list.Add(GetPositionCu1706Sell());
                list.Add(GetPositionCu1707Sell());
                list.Add(GetPositionCu1708Sell());

                return list;
            }
            public USePosition GetPositionCu1706Buy()
            {
                USePosition position = new USePosition();
                USeInstrument instrument = new USeInstrument("cu1706", "沪铜1706", USeMarket.SHFE);

                position.Instrument = instrument;
                position.Direction = USeDirection.Long;
                position.NewPosition = 3;
                position.OldPosition = 5;

                position.YesterdayPosition = 8;
                position.OpenQty = 0;
                position.AvgPirce = 46120.900m;
                position.Amount = 49000.570m;
                position.CloseQty = 0;
                position.NewFrozonPosition = 0;
                position.OldFrozonPosition = 0;

                return position;
            }
            public USePosition GetPositionCu1707Buy()
            {
                USePosition position = new USePosition();
                USeInstrument instrument = new USeInstrument("cu1707", "沪铜1707", USeMarket.SHFE);

                position.Instrument = instrument;
                position.Direction = USeDirection.Long;
                position.NewPosition = 10;
                position.OldPosition = 2;

                position.YesterdayPosition = 12;
                position.OpenQty = 0;
                position.AvgPirce = 46300.230m;
                position.Amount = 56000.570m;
                position.CloseQty = 0;
                position.NewFrozonPosition = 0;
                position.OldFrozonPosition = 0;

                return position;
            }
            public USePosition GetPositionCu1708Buy()
            {
                USePosition position = new USePosition();
                USeInstrument instrument = new USeInstrument("cu1708", "沪铜1708", USeMarket.SHFE);

                position.Instrument = instrument;
                position.Direction = USeDirection.Long;
                position.NewPosition = 8;
                position.OldPosition = 2;

                position.YesterdayPosition = 10;
                position.OpenQty = 0;
                position.AvgPirce = 46400.560m;
                position.Amount = 62000.570m;
                position.CloseQty = 0;
                position.NewFrozonPosition = 0;
                position.OldFrozonPosition = 0;

                return position;
            }
            public USePosition GetPositionCu1706Sell()
            {
                USePosition position = new USePosition();
                USeInstrument instrument = new USeInstrument("cu1706", "沪铜1706", USeMarket.SHFE);

                position.Instrument = instrument;
                position.Direction = USeDirection.Short;
                position.NewPosition = 3;
                position.OldPosition = 5;

                position.YesterdayPosition = 8;
                position.OpenQty = 0;
                position.AvgPirce = 46400.560m;
                position.Amount = 62000.570m;
                position.CloseQty = 0;
                position.NewFrozonPosition = 0;
                position.OldFrozonPosition = 0;

                return position;
            }
            public USePosition GetPositionCu1707Sell()
            {
                USePosition position = new USePosition();
                USeInstrument instrument = new USeInstrument("cu1707", "沪铜1707", USeMarket.SHFE);

                position.Instrument = instrument;
                position.Direction = USeDirection.Short;
                position.NewPosition = 10;
                position.OldPosition = 2;

                position.YesterdayPosition = 12;
                position.OpenQty = 0;
                position.AvgPirce = 49400.560m;
                position.Amount = 64000.570m;
                position.CloseQty = 0;
                position.NewFrozonPosition = 0;
                position.OldFrozonPosition = 0;

                return position;
            }
            public USePosition GetPositionCu1708Sell()
            {
                USePosition position = new USePosition();
                USeInstrument instrument = new USeInstrument("cu1708", "沪铜1708", USeMarket.SHFE);

                position.Instrument = instrument;
                position.Direction = USeDirection.Short;
                position.NewPosition = 9;
                position.OldPosition = 2;

                position.YesterdayPosition = 11;
                position.OpenQty = 0;
                position.AvgPirce = 56000.560m;
                position.Amount = 75000.570m;
                position.CloseQty = 0;
                position.NewFrozonPosition = 0;
                position.OldFrozonPosition = 0;

                return position;
            }
#endregion

            public List<USeTradeBook> GetTradeBookList()
            {
                //查找orderlist里面状态为全部成交的Order返回
                List<USeTradeBook> trade_book_list = new List<USeTradeBook>();

                foreach (USeOrderBook order_book in m_order_list)
                {
                    USeOrderStatus order_status = order_book.OrderStatus;
                    if (order_status == USeOrderStatus.AllTraded)
                    {
                        USeTradeBook tradeBook = new USeTradeBook();
                        tradeBook.TradeNum = order_book.OrderNum.ToString();
                        tradeBook.Instrument = order_book.Instrument;
                        tradeBook.OrderNum = order_book.OrderNum;
                        tradeBook.OrderSide = order_book.OrderSide;
                        tradeBook.OffsetType = order_book.OffsetType;
                        tradeBook.Price = order_book.OrderPrice;
                        tradeBook.Qty = order_book.OrderQty;
                        tradeBook.Amount = 0;
                        tradeBook.Fee = 0;
                        tradeBook.TradeTime = DateTime.Now;
                        tradeBook.Account = order_book.Account;
                        trade_book_list.Add(tradeBook);
                    }
                }
                return trade_book_list;

            }

            public List<USePosition> GetPositionList()
            {
                return m_positionList;
            }

            public USeInvestorBaseInfo GetInvestorBaseInfo()
            {
                USeInvestorBaseInfo baseInfo = new USeInvestorBaseInfo();
                baseInfo.RealName = "张三";
                return baseInfo;
            }

            public List<USeInstrumentDetail> GetInstrumentDetail()
            {
                List<USeInstrumentDetail> list = new List<USeInstrumentDetail>();
                list.Add(GetInstrument_1706());
                list.Add(GetInstrument_1707());
                list.Add(GetInstrument_1708());
                list.Add(GetInstrument_1709());
                return list;
            }

            public List<USeInstrumentDetail> GetInstrumentDetailByVarieties(string varieties)
            {
                List<USeInstrumentDetail> list = new List<USeInstrumentDetail>();
                if (varieties == null) return null;
                list = (from p in GetInstrumentDetail()
                        where p.Varieties == varieties
                        select p).ToList();
                if (list == null) return null;
                return list;
            }

            public USeInstrumentDetail GetInstrumentDetail(USeInstrument instrument)
            {
                USeInstrumentDetail ins_detail = new USeInstrumentDetail();
                if (instrument == null) return null;
                ins_detail = (from p in GetInstrumentDetail()
                              where p.Instrument == instrument
                              select p).FirstOrDefault();
                if (ins_detail == null) return null;
                return ins_detail;
            }

            public USeInstrumentDetail GetInstrument_1706()
            {
                USeInstrumentDetail cuInstrument_1706 = new USeInstrumentDetail();
                USeInstrument instrument_cu1706 = new USeInstrument("cu1706", "沪铜1706", USeMarket.SHFE);
                cuInstrument_1706.Instrument = instrument_cu1706;
                cuInstrument_1706.OpenDate = DateTime.Now;
                cuInstrument_1706.ExpireDate = DateTime.Now;
                cuInstrument_1706.EndDelivDate = DateTime.Now;
                cuInstrument_1706.VolumeMultiple = 5;
                cuInstrument_1706.IsTrading = true;
                cuInstrument_1706.Varieties = "cu";
                cuInstrument_1706.PriceTick = 10;
                cuInstrument_1706.ExchangeLongMarginRatio = 0;
                cuInstrument_1706.ExchangeShortMarginRatio = 0;
                cuInstrument_1706.ProductClass = USeProductClass.Futures;
                cuInstrument_1706.UnderlyingInstrument = "cu";
                cuInstrument_1706.StrikePrice = 0;
                cuInstrument_1706.OptionsType = USeOptionsType.None;
                cuInstrument_1706.InstrumentSerial = "cu";
                return cuInstrument_1706;
            }

            public USeInstrumentDetail GetInstrument_1707()
            {
                USeInstrumentDetail cuInstrument_1707 = new USeInstrumentDetail();
                USeInstrument instrument_cu1707 = new USeInstrument("cu1707", "沪铜1707", USeMarket.SHFE);
                cuInstrument_1707.Instrument = instrument_cu1707;
                cuInstrument_1707.OpenDate = DateTime.Now;
                cuInstrument_1707.ExpireDate = DateTime.Now;
                cuInstrument_1707.EndDelivDate = DateTime.Now;
                cuInstrument_1707.VolumeMultiple = 5;
                cuInstrument_1707.IsTrading = true;
                cuInstrument_1707.Varieties = "cu";
                cuInstrument_1707.PriceTick = 10;
                cuInstrument_1707.ExchangeLongMarginRatio = 0;
                cuInstrument_1707.ExchangeShortMarginRatio = 0;
                cuInstrument_1707.ProductClass = USeProductClass.Futures;
                cuInstrument_1707.UnderlyingInstrument = "cu";
                cuInstrument_1707.StrikePrice = 0;
                cuInstrument_1707.OptionsType = USeOptionsType.None;
                cuInstrument_1707.InstrumentSerial = "cu";
                return cuInstrument_1707;
            }

            public USeInstrumentDetail GetInstrument_1709()
            {
                USeInstrumentDetail cuInstrument_1709 = new USeInstrumentDetail();
                USeInstrument instrument_cu1709 = new USeInstrument("cu1709", "沪铜1709", USeMarket.SHFE);
                cuInstrument_1709.Instrument = instrument_cu1709;
                cuInstrument_1709.OpenDate = DateTime.Now;
                cuInstrument_1709.ExpireDate = DateTime.Now;
                cuInstrument_1709.EndDelivDate = DateTime.Now;
                cuInstrument_1709.VolumeMultiple = 5;
                cuInstrument_1709.IsTrading = true;
                cuInstrument_1709.Varieties = "cu";
                cuInstrument_1709.PriceTick = 10;
                cuInstrument_1709.ExchangeLongMarginRatio = 0;
                cuInstrument_1709.ExchangeShortMarginRatio = 0;
                cuInstrument_1709.ProductClass = USeProductClass.Futures;
                cuInstrument_1709.UnderlyingInstrument = "cu";
                cuInstrument_1709.StrikePrice = 0;
                cuInstrument_1709.OptionsType = USeOptionsType.None;
                cuInstrument_1709.InstrumentSerial = "cu";
                return cuInstrument_1709;
            }

            public USeInstrumentDetail GetInstrument_1708()
            {
                USeInstrumentDetail cuInstrument_1708 = new USeInstrumentDetail();
                USeInstrument instrument_cu1708 = new USeInstrument("cu1708", "沪铜1708", USeMarket.SHFE);
                cuInstrument_1708.Instrument = instrument_cu1708;
                cuInstrument_1708.OpenDate = DateTime.Now;
                cuInstrument_1708.ExpireDate = DateTime.Now;
                cuInstrument_1708.EndDelivDate = DateTime.Now;
                cuInstrument_1708.VolumeMultiple = 5;
                cuInstrument_1708.IsTrading = true;
                cuInstrument_1708.Varieties = "cu";
                cuInstrument_1708.PriceTick = 10;
                cuInstrument_1708.ExchangeLongMarginRatio = 0;
                cuInstrument_1708.ExchangeShortMarginRatio = 0;
                cuInstrument_1708.ProductClass = USeProductClass.Futures;
                cuInstrument_1708.UnderlyingInstrument = "cu";
                cuInstrument_1708.StrikePrice = 0;
                cuInstrument_1708.OptionsType = USeOptionsType.None;
                cuInstrument_1708.InstrumentSerial = "cu";
                return cuInstrument_1708;
            }

            /// <summary>
            /// 构造函数初始化
            /// </summary>
            public DataBuffer()
            {
                m_order_list = new List<USeOrderBook>();
            }
        }
    }
}
