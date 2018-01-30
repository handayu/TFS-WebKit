using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using USe.Common;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    internal partial class USeDataAccessor : IUSeDataAccessor
    {
        #region 用户配置数据访问
        /// <summary>
        /// 获取用户配置信息。
        /// </summary>
        /// <returns></returns>
        public UserDefineSetting GetUserDefineSetting()
        {
            UserDefineSetting userConfig = null;
            try
            {
                if (File.Exists(this.UserDefineSettingFilePath) == false)
                {
                    userConfig = new UserDefineSetting();
                }
                else
                {
                    userConfig = USeXmlSerializer.LoadFromXml<UserDefineSetting>(this.UserDefineSettingFilePath);
                }
                return userConfig;
            }
            catch (Exception ex)
            {
                throw new Exception("GetUserDefineSetting failed,Error:" + ex.Message);
            }
        }

        /// <summary>
        /// 保存用户配置。
        /// </summary>
        /// <param name="userConfig">用户配置。</param>
        public void SaveUserDefineConfig(UserDefineSetting userConfig)
        {
            try
            {
                USeXmlSerializer.SaveToXml(this.UserDefineSettingFilePath, userConfig);
            }
            catch (Exception ex)
            {
                throw new Exception("SaveUSeSystemConfig failed,Error:" + ex.Message);
            }
        }
        #endregion

        #region 系统配置数据访问
        /// <summary>
        /// 获取系统配置信息。
        /// </summary>
        /// <returns></returns>
        public USeSystemSetting GetUseSystemSetting()
        {
            USeSystemSetting setting = null;
            try
            {
                if (File.Exists(this.SystemConfigFilePath) == false)
                {
                    setting = null;
                }
                else
                {
                    setting = USeXmlSerializer.LoadFromXml<USeSystemSetting>(this.SystemConfigFilePath);
                }
                return setting;
            }
            catch (Exception ex)
            {
                throw new Exception("GetUseSystemConfig failed,Error:" + ex.Message);
            }
        }

        /// <summary>
        /// 保存系统配置。
        /// </summary>
        /// <param name="config">系统配置。</param>
        public void SaveUSeSystemConfig(USeSystemSetting config)
        {
            try
            {
                USeXmlSerializer.SaveToXml(this.SystemConfigFilePath, config);
            }
            catch (Exception ex)
            {
                throw new Exception("SaveUSeSystemConfig failed,Error:" + ex.Message);
            }
        }
        #endregion

        #region 前置服务数据访问
        /// <summary>
        /// 获取前置服务配置信息。
        /// </summary>
        /// <returns></returns>
        public GlobalFontServerConfig GetGlobalFontServerConfig()
        {
            GlobalFontServerConfig config = null;
            try
            {
                if (File.Exists(this.GlobalFontServerConfigFilePath) == false)
                {
                    config = new GlobalFontServerConfig();
                    config.DefaultBrokerId = "9999";

                    //FrontSeverConfig guoTaiJunAnConfigItem = new FrontSeverConfig() {
                    //    BrokerID = "2071",
                    //    BrokerName = "国泰君安(模拟)",
                    //    QuoteFrontAddress = "180.169.77.111",
                    //    QuoteFrontPort = 42213,
                    //    TradeFrontAddress = "180.169.77.111",
                    //    TradeFrontPort = 42205
                    //};

                    FrontSeverConfig simNowConfigItem = new FrontSeverConfig()
                    {
                        BrokerID = "9999",
                        BrokerName = "SimNow",
                        QuoteFrontAddress = "180.168.146.187",
                        QuoteFrontPort = 10010,
                        TradeFrontAddress = "180.168.146.187",
                        TradeFrontPort = 10000
                    };

                    //config.ServerList = new List<FrontSeverConfig>() { simNowConfigItem, guoTaiJunAnConfigItem };
                    config.ServerList = new List<FrontSeverConfig>() { simNowConfigItem };
                }
                else
                {
                    config = USeXmlSerializer.LoadFromXml<GlobalFontServerConfig>(this.GlobalFontServerConfigFilePath);
                }
                return config;
            }
            catch (Exception ex)
            {
                throw new Exception("GetGlobalFontServerConfig failed,Error:" + ex.Message);
            }
        }

        /// <summary>
        /// 保存全局参数配置。
        /// </summary>
        /// <param name="config"></param>
        public void SaveGlobalFontServerConfig(GlobalFontServerConfig config)
        {
            try
            {
                USeXmlSerializer.SaveToXml(this.GlobalFontServerConfigFilePath, config);
            }
            catch (Exception ex)
            {
                throw new Exception("SaveGlobalFontServerConfig failed,Error:" + ex.Message);
            }
        }
        #endregion

        #region 套利单数据访问
        /// <summary>
        /// 获取所有未完成套利单。
        /// </summary>
        /// <param name="brokerId">经纪商ID。</param>
        /// <param name="account">资金帐号。</param>
        /// <returns></returns>
        public List<USeArbitrageOrder> GetUnfinishArbitrageOrders(string brokerId, string account)
        {
            List<USeArbitrageOrder> list = new List<USeArbitrageOrder>();

            string unFinishOrderPath = GetUnFinishOrderPath(brokerId, account);
            DirectoryInfo unFinishOrderDir = new DirectoryInfo(unFinishOrderPath);
            if (unFinishOrderDir.Exists == false)
            {
                return list;
            }

            FileInfo[] fileArray = unFinishOrderDir.GetFiles("ArbitrageOrder_*.xml");
            if (fileArray == null || fileArray.Length <= 0)
            {
                return list;
            }

            XmlAttributeOverrides xmlOverrides = CreateArbitrageOrderXMLOverrides();
            foreach (FileInfo fileInfo in fileArray)
            {
                try
                {
                    USeArbitrageOrder unFinishOrder = USeXmlSerializer.LoadFromXml<USeArbitrageOrder>(fileInfo.FullName, xmlOverrides);
                    if (unFinishOrder != null)
                    {
                        list.Add(unFinishOrder);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("GetUnfinishArbitrageOrders failed,FileName:{0},Error:{1}", fileInfo.FullName, ex.Message));
                }
            }

            return list;
        }

        /// <summary>
        /// 获取历史完成套利单。
        /// </summary>
        /// <param name="brokerId">经纪商ID。</param>
        /// <param name="account">资金帐号。</param>
        /// <returns></returns>
        public List<USeArbitrageOrder> GetHistoryArbitrageOrders(string brokerId, string account)
        {
            return GetHistoryArbitrageOrders(brokerId, account, null, null);
        }

        /// <summary>
        /// 获取历史完成套利单。
        /// </summary>
        /// <returns></returns>
        /// <param name="brokerId">经纪商ID。</param>
        /// <param name="account">资金帐号。</param>
        /// <param name="beginTime">起始时间。</param>
        /// <param name="endTime">截至时间。</param>
        public List<USeArbitrageOrder> GetHistoryArbitrageOrders(string brokerId, string account, DateTime? beginTime, DateTime? endTime)
        {
            List<USeArbitrageOrder> list = new List<USeArbitrageOrder>();

            string finishOrderPath = GetFinishOrderPath(brokerId, account);
            DirectoryInfo finishOrderDir = new DirectoryInfo(finishOrderPath);
            if (finishOrderDir.Exists == false)
            {
                return list;  // 目录不存在
            }

            FileInfo[] fileArray = finishOrderDir.GetFiles("ArbitrageOrder_*.xml");
            if (fileArray == null || fileArray.Length <= 0)
            {
                return list;  // 目录无文件
            }

            foreach (FileInfo fileInfo in finishOrderDir.GetFiles())
            {
                try
                {
                    ArbitrageOrderKey orderKey = ArbitrageOrderKey.Create(fileInfo.FullName);
                    if (beginTime.HasValue)
                    {
                        Debug.Assert(orderKey.FinishTime.HasValue);
                        //过滤结束时间小于开始时间的
                        if (orderKey.FinishTime.Value < beginTime) continue;
                    }
                    if (endTime.HasValue)
                    {
                        // 过滤创建时间大于截止时间的
                        if (orderKey.CreateTime > endTime) continue;
                    }

                    XmlAttributeOverrides xmlOverrides = CreateArbitrageOrderXMLOverrides();
                    USeArbitrageOrder arbitrageOrder = USeXmlSerializer.LoadFromXml<USeArbitrageOrder>(fileInfo.FullName, xmlOverrides);
                    list.Add(arbitrageOrder);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("GetHistoryArbitrageOrders failed,FilePath:{0},Error:{1}", fileInfo.FullName, ex.Message));
                }
            }

            return list;
        }

        /// <summary>
        /// 保存套利单。
        /// </summary>
        /// <param name="order"></param>
        public void SaveUSeArbitrageOrder(USeArbitrageOrder order)
        {
            //判断order.State决定保存到未完成还是已完成
            if (order.State == ArbitrageOrderState.Finish)
            {
                // 保存到已完成
                try
                {
                    XmlAttributeOverrides xmlAttributeOrderNum = CreateArbitrageOrderXMLOverrides();
                    string fileFullName = GetFinishArbitrageOrderFilePath(order);
                    USeXmlSerializer.SaveToXml(fileFullName, order, xmlAttributeOrderNum);
                }
                catch (Exception ex)
                {
                    throw new Exception("SaveFinishUSeArbitrageOrder failed,Error:" + ex.Message);
                }

                //删除相对应的未完成的目录的对应的文件
                try
                {
                    string fileFullName = GetUnFinishArbitrageOrderFileFullName(order);
                    if (File.Exists(fileFullName) == false)
                    {
                        Debug.Assert(false);
                    }
                    else
                    {
                        File.Delete(fileFullName);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Delete UnFinishUseArbitrageOrder failed, Error:" + ex.Message);
                }
            }
            else
            {
                //  保存到未完成
                try
                {
                    XmlAttributeOverrides xmlOverrides = CreateArbitrageOrderXMLOverrides();
                    string fileFullName = GetUnFinishArbitrageOrderFileFullName(order);
                    USeXmlSerializer.SaveToXml(fileFullName, order, xmlOverrides);
                }
                catch (Exception ex)
                {
                    throw new Exception("SaveUnFinishUSeArbitrageOrder failed,Error:" + ex.Message);
                }
            }
        }
        #endregion

        #region 套利组合定义数据访问
        /// <summary>
        /// 获取套利单组合合约列表
        /// </summary>
        /// <returns></returns>
        public List<ArbitrageCombineInstrument> GetCombineInstruments(string brokerId,string account)
        {
            List<ArbitrageCombineInstrument> list = new List<ArbitrageCombineInstrument>();

            string ArbitrageCombineInstrumentsFilePath = GetArbitrageCombineInstrumentsFilePath(brokerId, account);
            DirectoryInfo ArbitrageCombineInstrumentsFilePathDir = new DirectoryInfo(ArbitrageCombineInstrumentsFilePath);
            if (ArbitrageCombineInstrumentsFilePathDir.Exists == false)
            {
                return list;  // 目录不存在
            }
            FileInfo[] fileArray = ArbitrageCombineInstrumentsFilePathDir.GetFiles("ArbitrageCombineInstrument.xml");
            if (fileArray == null || fileArray.Length <= 0)
            {
                return list;  // 目录无文件
            }

            foreach (FileInfo fileInfo in ArbitrageCombineInstrumentsFilePathDir.GetFiles())
            {
                try
                {
                    List<ArbitrageCombineInstrument> combineIns = USeXmlSerializer.LoadFromXml<List<ArbitrageCombineInstrument>>(fileInfo.FullName);
                    list = combineIns;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("GetCombineInstruments failed,FilePath:{0},Error:{1}", fileInfo.FullName, ex.Message));
                }
            }

            return list;
        }


        /// <summary>
        /// 保存套利单合约列表
        /// </summary>
        /// <param name="combineInstruments">套利单列表。</param>
        public void SaveCombineInstruments(string brokerId,string account,List<ArbitrageCombineInstrument> combineInstruments)
        {
            //if (combineInstruments == null) return;
            //Debug.Assert(combineInstruments.Count > 0);

            //string ArbitrageCombineInstrumentsFilePath = GetArbitrageCombineInstrumentsFilePath(combineInstruments[0].BrokerID, combineInstruments[0].Account);
            //DirectoryInfo ArbitrageCombineInstrumentsFilePathDir = new DirectoryInfo(ArbitrageCombineInstrumentsFilePath);
            //if (ArbitrageCombineInstrumentsFilePathDir.Exists == false)
            //{
            //    throw new Exception("文件路径异常");
            //}
            //FileInfo[] fileArray = ArbitrageCombineInstrumentsFilePathDir.GetFiles("ArbitrageCombineInstrument_*.xml");

            //for(int i = 0;i<fileArray.Count();i++)
            //{
            //    fileArray[i].Delete();
            //}

            //try
            //{
            //    foreach (ArbitrageCombineInstrument combineIns in combineInstruments)
            //    {
            //        string fileFullName = GetArbitrageCombineInstrumentsFileFullName(combineIns);
            //        USeXmlSerializer.SaveToXml(fileFullName, combineIns);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("SaveCombineInstruments failed,Error:" + ex.Message);
            //}

            if (combineInstruments == null) return;

            string ArbitrageCombineInstrumentsFilePath = GetArbitrageCombineInstrumentsFilePath(brokerId, account);
            DirectoryInfo ArbitrageCombineInstrumentsFilePathDir = new DirectoryInfo(ArbitrageCombineInstrumentsFilePath);
            if (ArbitrageCombineInstrumentsFilePathDir.Exists == false)
            {
                ArbitrageCombineInstrumentsFilePathDir.Create();
            }

            try
            {
                FileInfo[] fileArray = ArbitrageCombineInstrumentsFilePathDir.GetFiles("ArbitrageCombineInstrument.xml");

                for (int i = 0; i < fileArray.Count(); i++)
                {
                    fileArray[i].Delete();
                }

                string fileFullName = GetArbitrageCombineInstrumentsFileFullName(brokerId,account);
                    USeXmlSerializer.SaveToXml(fileFullName, combineInstruments);
            }
            catch (Exception ex)
            {
                throw new Exception("SaveCombineInstruments failed,Error:" + ex.Message);
            }

        }
        #endregion

        #region 套利组合默认设定数据访问。
        /// <summary>
        /// 获取套利品种默认设定。
        /// </summary>
        /// <returns></returns>
        public List<ArbitrageCombineOrderSetting> GetCombineOrderSettings(string brokerId, string account)
        {
            List<ArbitrageCombineOrderSetting> list = new List<ArbitrageCombineOrderSetting>();

            string ArbitrageOrderSettingsFilePath = GetArbitrageOrderSettingsFilePath(brokerId, account);
            DirectoryInfo ArbitrageOrderSettingsFilePathDir = new DirectoryInfo(ArbitrageOrderSettingsFilePath);
            if (ArbitrageOrderSettingsFilePathDir.Exists == false)
            {
                return list;  // 目录不存在
            }
            FileInfo[] fileArray = ArbitrageOrderSettingsFilePathDir.GetFiles("ArbitrageOrderSettings.xml");
            if (fileArray == null || fileArray.Length <= 0)
            {
                return list;  // 目录无文件
            }

            foreach (FileInfo fileInfo in ArbitrageOrderSettingsFilePathDir.GetFiles())
            {
                try
                {
                    List<ArbitrageCombineOrderSetting> combineOrderSettingList = USeXmlSerializer.LoadFromXml<List<ArbitrageCombineOrderSetting>>(fileInfo.FullName);
                    list = combineOrderSettingList;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("GetCombineOrderSettings failed,FilePath:{0},Error:{1}", fileInfo.FullName, ex.Message));
                }
            }

            return list;
        }

        /// <summary>
        /// 获取套利品种默认设定。
        /// </summary>
        /// <returns></returns>
        public ArbitrageCombineOrderSetting GetCombineOrderSetting(string brokerId, string account,string productCode)
        {
            string fileName = GetArbitrageOrderSettingsFileFullName(brokerId, account);
            if (File.Exists(fileName) == false)
            {
                return null;
            }
            else
            {
                try
                {
                    List<ArbitrageCombineOrderSetting> list = USeXmlSerializer.LoadFromXml<List<ArbitrageCombineOrderSetting>>(fileName);
                    foreach (ArbitrageCombineOrderSetting item in list)
                    {
                        if (item.Product.ProductCode == productCode)
                        {
                            return item;
                        }
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("GetCombineOrderSettings failed,FilePath:{0},Error:{1}", fileName, ex.Message));
                }
            }
        }


        /// <summary>
        /// 保存套利品种默认参数设定。
        /// </summary>
        /// <param name="ArbitrageCombineOrderSetting">套利单列表。</param>
        public void SaveCombineOrderSettings(string brokerId, string account, List<ArbitrageCombineOrderSetting> combineOrderSettingsList)
        {
            //if (combineInstruments == null) return;
            //Debug.Assert(combineInstruments.Count > 0);

            //string ArbitrageCombineInstrumentsFilePath = GetArbitrageCombineInstrumentsFilePath(combineInstruments[0].BrokerID, combineInstruments[0].Account);
            //DirectoryInfo ArbitrageCombineInstrumentsFilePathDir = new DirectoryInfo(ArbitrageCombineInstrumentsFilePath);
            //if (ArbitrageCombineInstrumentsFilePathDir.Exists == false)
            //{
            //    throw new Exception("文件路径异常");
            //}
            //FileInfo[] fileArray = ArbitrageCombineInstrumentsFilePathDir.GetFiles("ArbitrageCombineInstrument_*.xml");

            //for(int i = 0;i<fileArray.Count();i++)
            //{
            //    fileArray[i].Delete();
            //}

            //try
            //{
            //    foreach (ArbitrageCombineInstrument combineIns in combineInstruments)
            //    {
            //        string fileFullName = GetArbitrageCombineInstrumentsFileFullName(combineIns);
            //        USeXmlSerializer.SaveToXml(fileFullName, combineIns);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("SaveCombineInstruments failed,Error:" + ex.Message);
            //}

            if (combineOrderSettingsList == null) return;

            string ArbitrageOrderSettingsFilePath = GetArbitrageOrderSettingsFilePath(brokerId, account);
            DirectoryInfo ArbitrageOrderSettingsFilePathDir = new DirectoryInfo(ArbitrageOrderSettingsFilePath);
            if (ArbitrageOrderSettingsFilePathDir.Exists == false)
            {
                ArbitrageOrderSettingsFilePathDir.Create();
            }

            try
            {
                FileInfo[] fileArray = ArbitrageOrderSettingsFilePathDir.GetFiles("ArbitrageOrderSettings.xml");

                for (int i = 0; i < fileArray.Count(); i++)
                {
                    fileArray[i].Delete();
                }

                string fileFullName = GetArbitrageOrderSettingsFileFullName(brokerId, account);
                USeXmlSerializer.SaveToXml(fileFullName, combineOrderSettingsList);
            }
            catch (Exception ex)
            {
                throw new Exception("SaveCombineOrderSettings failed,Error:" + ex.Message);
            }

        }
        #endregion



        /// <summary>
        /// 创建ArbitrageOrder XML序列化属性。
        /// </summary>
        /// <returns></returns>
        private XmlAttributeOverrides CreateArbitrageOrderXMLOverrides()
{
    XmlAttributeOverrides attrOverrides = new XmlAttributeOverrides();
    XmlAttributes attrs = new XmlAttributes();
    {
        XmlElementAttribute attr = new XmlElementAttribute("ctpOrderNum", typeof(USe.TradeDriver.Ctp.CtpOrderNum));
        attrs.XmlElements.Add(attr);
    }
    {
        XmlElementAttribute attr = new XmlElementAttribute("testOrderNum", typeof(USe.TradeDriver.Test.TestOrderNum));
        attrs.XmlElements.Add(attr);
    }
    attrOverrides.Add(typeof(USeOrderBook), "OrderNum", attrs);
    return attrOverrides;
}
    }
}
