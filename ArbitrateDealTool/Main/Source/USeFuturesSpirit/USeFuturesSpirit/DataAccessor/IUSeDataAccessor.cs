using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    public interface IUSeDataAccessor
    {
        #region 系统配置数据访问
        /// <summary>
        /// 获取系统配置信息。
        /// </summary>
        /// <returns></returns>
        USeSystemSetting GetUseSystemSetting();

        /// <summary>
        /// 保存系统配置。
        /// </summary>
        /// <param name="setting">系统配置。</param>
        void SaveUSeSystemConfig(USeSystemSetting setting);
        #endregion

        #region 用户配置数据访问
        /// <summary>
        /// 获取用户配置信息。
        /// </summary>
        /// <returns></returns>
        UserDefineSetting GetUserDefineSetting();

        /// <summary>
        /// 保存用户配置。
        /// </summary>
        /// <param name="userSetting">用户配置。</param>
        void SaveUserDefineConfig(UserDefineSetting userSetting);
        #endregion

        #region 前置服务数据访问
        /// <summary>
        /// 获取前置服务配置信息。
        /// </summary>
        /// <returns></returns>
        GlobalFontServerConfig GetGlobalFontServerConfig();

        /// <summary>
        /// 保存全局参数配置。
        /// </summary>
        /// <param name="config"></param>
        void SaveGlobalFontServerConfig(GlobalFontServerConfig config);
        #endregion

        #region 套利单数据访问
        /// <summary>
        /// 获取所有未完成套利单。
        /// </summary>
        /// <param name="brokerId">经纪商ID。</param>
        /// <param name="account">资金帐号。</param>
        /// <returns></returns>
        List<USeArbitrageOrder> GetUnfinishArbitrageOrders(string brokerId, string account);

        /// <summary>
        /// 获取历史完成套利单。
        /// </summary>
        /// <param name="brokerId">经纪商ID。</param>
        /// <param name="account">资金帐号。</param>
        /// <returns></returns>
        List<USeArbitrageOrder> GetHistoryArbitrageOrders(string brokerId, string account);

        /// <summary>
        /// 获取历史完成套利单。
        /// </summary>
        /// <returns></returns>
        /// <param name="brokerId">经纪商ID。</param>
        /// <param name="account">资金帐号。</param>
        /// <param name="beginTime">起始时间。</param>
        /// <param name="endTime">截至时间。</param>
        List<USeArbitrageOrder> GetHistoryArbitrageOrders(string brokerId, string account, DateTime? beginTime, DateTime? endTime);

        /// <summary>
        /// 保存套利单。
        /// </summary>
        /// <param name="order"></param>
        void SaveUSeArbitrageOrder(USeArbitrageOrder order);
        #endregion

        #region 套利组合定义数据访问
        /// <summary>
        /// 获取套利组合数据。
        /// </summary>
        /// <returns></returns>
        List<ArbitrageCombineInstrument> GetCombineInstruments(string brokerId, string account);
        /// <summary>
        /// 保存套利组合数据。
        /// </summary>
        /// <param name="userSetting">用户配置。</param>
        void SaveCombineInstruments(string brokerId, string account, List<ArbitrageCombineInstrument> combineInstruments);
        #endregion

        #region 套利组合下单参数配置。
        /// <summary>
        /// 获取套利组合默认参数配置。
        /// </summary>
        /// <returns></returns>
        List<ArbitrageCombineOrderSetting> GetCombineOrderSettings(string brokerId, string account);
        /// <summary>
        /// 保存套利组合默认参数配置。
        /// </summary>
        /// <param name="userSetting">用户配置。</param>
        void SaveCombineOrderSettings(string brokerId, string account, List<ArbitrageCombineOrderSetting> combineOrderSetting);
        #endregion
    }
}
