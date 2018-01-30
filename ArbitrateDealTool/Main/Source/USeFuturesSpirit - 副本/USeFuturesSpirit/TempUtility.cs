using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using System.Xml.Serialization;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.Configuration;
using USeFuturesSpirit.Configuration;
using USeFuturesSpirit.Arbitrage;

namespace USeFuturesSpirit
{
    class TempUtility
    {
        public static void Test()
        {

            //TestFormPicture formPicture = new TestFormPicture();
            //formPicture.ShowDialog();
            //ErrorOrderBookProcessForm errorForm = new ErrorOrderBookProcessForm(null);
            //errorForm.ShowDialog();
            //AlarmTest();
            //TestHanYu();
            //TestTradingRangeConfig();

            //ArbitrageQuoteChoiceForm form = new ArbitrageQuoteChoiceForm();
            //form.ShowDialog();
        }

        public static void AlarmTest()
        {
            try
            {
                AlarmManager alarmManager = new AlarmManager();
                alarmManager.FireAlarm(AlarmType.AutoTraderError, false);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        public static void TestTradingRangeConfig()
        {
            try
            {
                TradeRangeSection section = TradeRangeSection.GetSection();
                string s = "";
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
        }
        public static List<USeInstrument> GetInstrumentList()
        {



            List<USeInstrument> list = new List<USeInstrument>();
            list.Add(new USeInstrument("cu1705", "铜1705", USeMarket.SHFE));
            list.Add(new USeInstrument("cu1706", "铜1706", USeMarket.SHFE));
            list.Add(new USeInstrument("cu1707", "铜1707", USeMarket.SHFE));
            list.Add(new USeInstrument("cu1708", "铜1708", USeMarket.SHFE));
            list.Add(new USeInstrument("cu1709", "铜1709", USeMarket.SHFE));
            list.Add(new USeInstrument("cu1710", "铜1710", USeMarket.SHFE));
            list.Add(new USeInstrument("cu1711", "铜1711", USeMarket.SHFE));
            list.Add(new USeInstrument("cu1712", "铜1712", USeMarket.SHFE));
            list.Add(new USeInstrument("cu1801", "铜1801", USeMarket.SHFE));
            list.Add(new USeInstrument("cu1802", "铜1802", USeMarket.SHFE));
            list.Add(new USeInstrument("cu1803", "铜1803", USeMarket.SHFE));
            list.Add(new USeInstrument("cu1804", "铜1804", USeMarket.SHFE));
            return list;
        }

        public static List<USeMarketData> GetMarketDataList()
        {
            List<USeMarketData> list = new List<USeMarketData>();

            List<USeInstrument> instrumentList = GetInstrumentList();
            foreach (USeInstrument instrument in instrumentList)
            {
                USeMarketData market = new USeMarketData(instrument);
                market.AskPrice = 45400;
                market.AskSize = 1;
                market.BidPrice = 45420;
                market.BidSize = 3;
                market.OpenPrice = 45060;
                market.HighPrice = 45450;
                market.LowPrice = 44800;
                market.LastPrice = 45400;
                market.ClosePrice = 0;
                market.PreClosePrice = 45300;
                market.UpperLimitPrice = 47700;
                market.LowerLimitPrice = 42360;
                market.PreSettlementPrice = 45300;
                market.SettlementPrice = 0;
                market.OpenInterest = 1000;
                market.Volume = 12312;
                market.Turnover = 123423334;
                market.UpdateTime = DateTime.Now;

                list.Add(market);
            }
            return list;
        }


        ////public static List<OrderActionLog> GetSimulateOrderActionLog()
        //{
        //    List<OrderActionLog> list = new List<OrderActionLog>();
        //    DateTime time = new DateTime(2017, 05, 11, 13, 03, 04);

        //    list.Add(new OrderActionLog()
        //    {
        //        Time = time,
        //        Message = "cu1706与cu1707价差达到100点"
        //    });

        //    time = time.AddSeconds(1);
        //    list.Add(new OrderActionLog()
        //    {
        //        Time = time,
        //        Message = "cu1706买入委托下单5手"
        //    });

        //    time = time.AddMinutes(1);
        //    list.Add(new OrderActionLog()
        //    {
        //        Time = time,
        //        Message = "cu1706买入成交3手"
        //    });
        //    time = time.AddSeconds(5);
        //    list.Add(new OrderActionLog()
        //    {
        //        Time = time,
        //        Message = "cu1706买入成交2手"
        //    });
        //    time = time.AddSeconds(1);
        //    list.Add(new OrderActionLog()
        //    {
        //        Time = time,
        //        Message = "cu1706买入委托下单5手"
        //    });
        //    list.Add(new OrderActionLog()
        //    {
        //        Time = time,
        //        Message = "cu1707卖出委托下单5手"
        //    });

        //    return list;
        //}

            public static void TestHanYu()
        {
            USeOrderBook group = CreateUseOrderBook();
            try
            {
                XmlAttributeOverrides attrOverrides = new XmlAttributeOverrides();
                XmlAttributes attrs = new XmlAttributes();
                XmlElementAttribute attr = new XmlElementAttribute("ctpNum", typeof(USe.TradeDriver.Ctp.CtpOrderNum));
                attrs.XmlElements.Add(attr);
                attrOverrides.Add(typeof(USeOrderBook), "OrderNum", attrs);

                string fileFullName = @"D:\hanyu.xml";
                TempUtility.SaveToXml(fileFullName, group, attrOverrides);
            }
            catch (Exception ex)
            {
                throw new Exception("SaveUSeArbitrageOrder failed,Error:" + ex.Message);
            }

        }
        public static void TestOrder()
        {
            USeArbitrageOrder order = new USeArbitrageOrder();


            //ArbiOrder基础参数
            order.BrokerId = "9000";
            order.TraderIdentify = Guid.NewGuid();
            order.Account = "090952";
            order.State = ArbitrageOrderState.Closed;
            order.CreateTime = DateTime.Now;
            order.FinishTime = DateTime.Now;

            //order.OpenArgument = CreateOpenArg();
            //order.CloseArgument = CreateCloseArg();


            order.CloseTaskGroup = CreateTaskGroup();
            USeDataAccessor dataAccessor = new USeDataAccessor();
            dataAccessor.SaveUSeArbitrageOrder(order);
        }

        private static ArbitrageOpenArgument CreateOpenArg()
        {
            //ArbiOrder开仓仓参数
            ArbitrageOpenArgument arg = new ArbitrageOpenArgument();
            arg.BuyInstrument = new USeInstrument("CF1701", "CF1701", USeMarket.CFFEX);
            arg.SellInstrument = new USeInstrument("CF1701", "CF1701", USeMarket.CFFEX);
            arg.BuyInstrumentOrderPriceType = ArbitrageOrderPriceType.OpponentPrice;
            arg.SellInstrumentOrderPriceType = ArbitrageOrderPriceType.LastPrice;
            arg.PreferentialSide = USeOrderSide.Buy;
            arg.OpenCondition = new PriceSpreadCondition()
            {
                PriceSpreadSide = PriceSpreadSide.LessOrEqual,
                PriceSpreadThreshold = 200
            };
            arg.TotalOrderQty = 100;
            arg.OrderQtyUint = 10;
            arg.DifferentialUnit = 3;
            return arg;
        }

        private static ArbitrageCloseArgument CreateCloseArg()
        {
            //ArbiOrder平仓参数
            ArbitrageCloseArgument arg = new ArbitrageCloseArgument();
            arg.BuyInstrument = new USeInstrument("CF1701", "CF1701", USeMarket.CFFEX);
            arg.SellInstrument = new USeInstrument("CF1701", "CF1701", USeMarket.CFFEX);
            arg.BuyInstrumentOrderPriceType = ArbitrageOrderPriceType.OpponentPrice;
            arg.SellInstrumentOrderPriceType = ArbitrageOrderPriceType.LastPrice;
            arg.PreferentialSide = USeOrderSide.Buy;
            arg.CloseCondition = new PriceSpreadCondition() {
                PriceSpreadSide = PriceSpreadSide.LessOrEqual,
                PriceSpreadThreshold = 200 };
            arg.OrderQtyUint = 10;
            arg.DifferentialUnit = 3;
            return arg;
        }

        private static ArbitrageTaskGroup CreateTaskGroup()
        {
            //买入合约开仓任务组
            ArbitrageTaskGroup group = new ArbitrageTaskGroup();
            //group.Instrument = new USeInstrument("IF1710", "股指1710", USeMarket.CFFE);
            //group.OrderSide = USeOrderSide.Sell;


            List<ArbitrageTask> taskList = new List<ArbitrageTask>();
            taskList.Add(CreateTask());
            group.TaskList = taskList;

            return group;

        }

        private static ArbitrageTask CreateTask()
        {
            ArbitrageTask task = new ArbitrageTask();
            task.TaskId = 100;
            //task.TaskState = ArbitrageOrderTaskState.Finish;
            //task.Instrument = new USeInstrument("CF1701", "CF1701", USeMarket.CFFE);
            //task.OrderPriceType = ArbitrageOrderPriceType.LastPrice;
            //task.OrderSide = USeOrderSide.Buy;
            //task.OffsetType = USeOffsetType.Close;
            //task.PlanOrderQty = 3;

            //List<USeOrderBook> use_order_booklist = new List<USeOrderBook>();
            //use_order_booklist.Add(CreateUseOrderBook());
            //task.OrderBooks = use_order_booklist;

            task.UnFinishOrderBooks.Add(CreateUseOrderBook());



            List<USeOrderBook> orderBookList = new List<USeOrderBook>();
            orderBookList.Add(CreateUseOrderBook());

            return task;
        }

        private static USeOrderBook CreateUseOrderBook()
        {
            USeOrderBook orderBook = new USeOrderBook();
            orderBook.Account = "090952";
            
            orderBook.OrderNum = new USe.TradeDriver.Ctp.CtpOrderNum(1,1,"1111");
            orderBook.CancelQty = 0;
            orderBook.Instrument = new USeInstrument("CF1710", "棉花1710", USeMarket.CFFEX);
            orderBook.Memo = "开仓任务0";
            orderBook.OffsetType = USeOffsetType.Open;
            orderBook.Account = "090952";
            orderBook.Account = "090952";
            return orderBook;
        }

        public static void OrderNumToSerialization()
        {

            XmlAttributeOverrides attrOverrides = new XmlAttributeOverrides();
            XmlAttributes attrs = new XmlAttributes();
            XmlElementAttribute attr = new XmlElementAttribute("ctpNum", typeof(USe.TradeDriver.Ctp.CtpOrderNum));
            attrs.XmlElements.Add(attr);
            attrOverrides.Add(typeof(USeOrderBook), "OrderNum", attrs);
        }

        //public static void TestXML()
        //{
        //TestXML entity = new USeFuturesSpirit.TestXML();
        //entity.TestBase1 = new XMLTestSubClass1()
        //{
        //    Name = "test1",
        //    MyProperty = "propert1"
        //};
        ////entity.TestBase2 = new XMLTestSubClass2() {
        ////    Name = "test2",
        ////    MySex = "sex2"
        ////};

        ////entity.TestBase1 = new XMLTestSubClass2() {
        ////    Name = "test2aaa",
        ////    MySex = "sex2aaa"
        ////};
        //entity.TestBase2 = new XMLTestSubClass2()
        //{
        //    Name = "test2",
        //    MySex = "sex2"
        //};

        //HanYuTest hanyu = new HanYuTest()
        //{
        //    MyTest = entity
        //};
        //try
        //{
        //    //string Json = JsonHelper.JsonSerializer<TestXML>(entity); 
        //    string fileFullName = @"D:\test.xml";
        //    XmlAttributeOverrides attrOverrides = new XmlAttributeOverrides();
        //    // Create the XmlAttributes class.
        //    XmlAttributes attrs = new XmlAttributes();
        //    XmlElementAttribute attr = new XmlElementAttribute("sub1", typeof(XMLTestSubClass1));
        //    attrs.XmlElements.Add(attr);

        //    XmlElementAttribute attr2 = new XmlElementAttribute("sub2", typeof(XMLTestSubClass2));
        //    attrs.XmlElements.Add(attr2);

        //    /* Add the XmlAttributes to the XmlAttributeOverrides. 
        //    "Students" is the name being overridden. */
        //    attrOverrides.Add(typeof(TestXML), "TestBase1", attrs);

        //    XmlAttributes attrs22 = new XmlAttributes();
        //    XmlElementAttribute attr22 = new XmlElementAttribute("ym1", typeof(XMLTestSubClass1));
        //    attrs22.XmlElements.Add(attr22);

        //    XmlElementAttribute attr222 = new XmlElementAttribute("ym2", typeof(XMLTestSubClass2));
        //    attrs22.XmlElements.Add(attr222);

        //    attrOverrides.Add(typeof(TestXML), "TestBase2", attrs22);

        //    SaveToXml(fileFullName, hanyu, attrOverrides);


        //    HanYuTest result = LoadFromXml<HanYuTest>(fileFullName, attrOverrides);

        //    string s = "";



        //}
        //catch (Exception ex)
        //{
        //    throw new Exception("SaveGlobalFontServerConfig failed,Error:" + ex.Message);
        //}

        //}

        public static T LoadFromXml<T>(string filePath, XmlAttributeOverrides attrOverrides)
        {
            if (File.Exists(filePath) == false) return default(T);

            object result = null;
            using (StreamReader reader = new StreamReader(filePath))
            {
                if (reader.EndOfStream)
                {
                    return default(T);
                }
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T), attrOverrides);
                result = xmlSerializer.Deserialize(reader);
            }

            return (T)result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="sourceObj"></param>
        public static void SaveToXml(string filePath, object sourceObj, XmlAttributeOverrides attrOverrides)
        {
            if (sourceObj == null) return;

            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Directory.Exists == false)
            {
                fileInfo.Directory.Create();
            }

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(sourceObj.GetType(), attrOverrides);
                xmlSerializer.Serialize(writer, sourceObj);
            }
        }

    }


    //public class HanYuTest
    //{
    //    public TestXML MyTest { get; set; }
    //}

    //public class TestXML
    //{
    //    public XMLTestBase TestBase1 { get; set; }

    //    public XMLTestBase TestBase2 { get; set; }
    //}

    //public class XMLTestBase
    //{
    //    public string Name { get; set; }
    //}

    //public class XMLTestSubClass1 : XMLTestBase
    //{

    //    public string MyProperty { get; set; }
    //}

    //public class XMLTestSubClass2 : XMLTestBase
    //{
    //    public string MySex { get; set; }
    //}


}
