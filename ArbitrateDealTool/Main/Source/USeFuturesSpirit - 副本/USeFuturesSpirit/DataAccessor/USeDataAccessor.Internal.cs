using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 数据访问器--私有成员
    /// </summary>
    internal partial class USeDataAccessor
    {
        #region property
        /// <summary>
        /// 数据存储根目录。
        /// </summary>
        private string RootPath { get; set; }

        /// <summary>
        /// 服务配置文件路径。
        /// </summary>
        private string GlobalFontServerConfigFilePath
        {
            get { return Path.Combine(this.RootPath, "GlobalFrontServerConfig.xml"); }
        }

        /// <summary>
        /// 系统配置文件路径。
        /// </summary>
        private string SystemConfigFilePath
        {
            get { return Path.Combine(this.RootPath, "SystemConfig.xml"); }
        }

        /// <summary>
        /// 用户自定义配置文件路径。
        /// </summary>
        private string UserDefineSettingFilePath
        {
            get { return Path.Combine(this.RootPath, "UserDefineSetting.xml"); }
        }

        /// <summary>
        /// 用户自定义套利合约组合配置文件路径。
        /// </summary>
        private string GetArbitrageCombineInstrumentsFilePath(string brokerId, string account)
        {
            string subFolder = string.Format(@"ArbitrageCombineInstruments\{0}_{1}", brokerId, account);
            return Path.Combine(this.RootPath, subFolder);
        }

        /// <summary>
        /// 用户自定义套利合约组合配置文件路径。
        /// </summary>
        private string GetArbitrageCombineInstrumentsFileFullName(string brokerId,string account)
        {
            string directoryPath = GetArbitrageCombineInstrumentsFilePath(brokerId, account);
            string fileName = string.Format("ArbitrageCombineInstrument.xml");
            return Path.Combine(directoryPath, fileName);
        }

        /// <summary>
        /// 用户自定义套利合约组合下单配置文件路径。
        /// </summary>
        private string GetArbitrageOrderSettingsFilePath(string brokerId, string account)
        {
            string subFolder = string.Format(@"ArbitrageOrderSettings\{0}_{1}", brokerId, account);
            return Path.Combine(this.RootPath, subFolder);
        }

        /// <summary>
        /// 用户自定义套利合约组合下单配置文件路径。
        /// </summary>
        private string GetArbitrageOrderSettingsFileFullName(string brokerId, string account)
        {
            string directoryPath = GetArbitrageOrderSettingsFilePath(brokerId, account);
            string fileName = string.Format("ArbitrageOrderSettings.xml");
            return Path.Combine(directoryPath, fileName);
        }

        /// <summary>
        /// 未完成套利单存储路径。
        /// </summary>
        /// <param name="brokerId">经纪商ID。</param>
        /// <param name="account">资金帐号。</param>
        /// <returns></returns>
        private string GetUnFinishOrderPath(string brokerId, string account)
        {
            string subFolder = string.Format(@"Order\{0}_{1}\UnFinishOrder", brokerId, account);
            return Path.Combine(this.RootPath, subFolder);
        }

        /// <summary>
        /// 已完成套利单存储路径。
        /// </summary>
        /// <param name="brokerId">经纪商ID。</param>
        /// <param name="account">资金帐号。</param>
        /// <returns></returns>
        private string GetFinishOrderPath(string brokerId, string account)
        {
            string subFolder = string.Format(@"Order\{0}_{1}\FinishOrder", brokerId, account);
            return Path.Combine(this.RootPath, subFolder);
        }

        /// <summary>
        /// 未完成套利单文件存储路径。
        /// </summary>
        /// <param name="arbitrageOrder">套利单。</param>
        /// <returns></returns>
        private string GetUnFinishArbitrageOrderFileFullName(USeArbitrageOrder arbitrageOrder)
        {
            Debug.Assert(string.IsNullOrEmpty(arbitrageOrder.BrokerId) == false);
            Debug.Assert(string.IsNullOrEmpty(arbitrageOrder.Account) == false);

            string directoryPath = GetUnFinishOrderPath(arbitrageOrder.BrokerId, arbitrageOrder.Account);
            string fileName = string.Format("ArbitrageOrder_{0}.xml", arbitrageOrder.CreateTime.ToString(ORDER_TIME_FORMAT));
            return Path.Combine(directoryPath, fileName);
        }

        /// <summary>
        /// 已完成套利单文件存储路径。
        /// </summary>
        /// <param name="arbitrageOrder">套利单。</param>
        /// <returns></returns>
        private string GetFinishArbitrageOrderFilePath(USeArbitrageOrder arbitrageOrder)
        {
            Debug.Assert(string.IsNullOrEmpty(arbitrageOrder.BrokerId) == false);
            Debug.Assert(string.IsNullOrEmpty(arbitrageOrder.Account) == false);
            Debug.Assert(arbitrageOrder.FinishTime.HasValue);

            string directoryPath = GetFinishOrderPath(arbitrageOrder.BrokerId, arbitrageOrder.Account);
            string fileName = string.Format("ArbitrageOrder_{0}_{1}.xml",
                arbitrageOrder.CreateTime.ToString(ORDER_TIME_FORMAT),
                arbitrageOrder.FinishTime.Value.ToString(ORDER_TIME_FORMAT));
            return Path.Combine(directoryPath, fileName);
        }
        #endregion


        /// <summary>
        /// 创建文件夹。
        /// </summary>
        /// <param name="directoryParth"></param>
        private void CreateDirectory(string directoryParth)
        {
            if (Directory.Exists(directoryParth) == false)
            {
                Directory.CreateDirectory(directoryParth);
            }
        }
    }
}
