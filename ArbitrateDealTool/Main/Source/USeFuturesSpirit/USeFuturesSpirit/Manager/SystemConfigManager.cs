using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 系统配置变更委托。
    /// </summary>
    public delegate void SystemConfigChangedEventHandle();

    /// <summary>
    /// 用户自定义变更委托。
    /// </summary>
    public delegate void UserDefineSettingChangedEventHandle();

    public class SystemConfigManager
    {
        #region event
        /// <summary>
        /// 系统配置变更。
        /// </summary>
        public event SystemConfigChangedEventHandle OnSystemSettingChanged;

        /// <summary>
        /// 用户自定义配置变更。
        /// </summary>
        public event UserDefineSettingChangedEventHandle OnUserDefineSettingChanged;
        #endregion

        #region member
        private IUSeDataAccessor m_dataAccessor = null;
        private USeSystemSetting m_systemSetting = null;
        private UserDefineSetting m_userDefineSetting = null;
        private object m_syncObj = new object();
        #endregion

        public SystemConfigManager()
        {

        }

        /// <summary>
        /// 设定数据访问器。
        /// </summary>
        /// <param name="dataAccessor"></param>
        public void SetDataAccessor(IUSeDataAccessor dataAccessor)
        {
            m_dataAccessor = dataAccessor;
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        public void Initialize()
        {
            try
            {
                if (m_dataAccessor == null)
                {
                    throw new ApplicationException("未设定数据访问器");
                }

                USeSystemSetting systemSetting = m_dataAccessor.GetUseSystemSetting();
                if (systemSetting == null)
                {
                    systemSetting = USeSystemSetting.CreateDefault();
                }

                UserDefineSetting userDefineSetting = m_dataAccessor.GetUserDefineSetting();
                if(userDefineSetting == null)
                {
                    userDefineSetting = UserDefineSetting.CreateDefault();
                }

                lock (m_syncObj)
                {
                    m_systemSetting = systemSetting;
                    m_userDefineSetting = userDefineSetting;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("初始化系统配置管理类失败," + ex.Message);
            }
        }

        /// <summary>
        /// 获取系统设定。
        /// </summary>
        /// <returns></returns>
        public USeSystemSetting GetSystemSetting()
        {
            Debug.Assert(m_systemSetting != null);
            lock(m_syncObj)
            {
                USeSystemSetting setting = m_systemSetting.Clone();
                return setting;
            }
        }

        /// <summary>
        /// 获取任务下单设定。
        /// </summary>
        /// <returns></returns>
        public TaskOrderSetting GetTaskOrderSetting()
        {
            Debug.Assert(m_systemSetting != null);
            lock (m_syncObj)
            {
                TaskOrderSetting setting = m_systemSetting.TaskOrder.Clone();
                return setting;
            }
        }

        // <summary>
        /// 获取保证金设定。
        /// </summary>
        /// <returns></returns>
        public OrderMarginSetting GetOrderMarginSetting()
        {
            Debug.Assert(m_systemSetting != null);
            lock (m_syncObj)
            {
                OrderMarginSetting setting = m_systemSetting.OrderMargin.Clone();
                return setting;
            }
        }

        // <summary>
        /// 获取预警设定。
        /// </summary>
        /// <returns></returns>
        public AlarmSetting GetAlarmSetting()
        {
            Debug.Assert(m_systemSetting != null);
            lock (m_syncObj)
            {
                AlarmSetting setting = m_systemSetting.Alarm.Clone();
                return setting;
            }
        }

        /// <summary>
        /// 保存系统设定。
        /// </summary>
        /// <param name="systemSetting"></param>
        public void SaveSystemSetting(USeSystemSetting systemSetting)
        {
            try
            {
                m_dataAccessor.SaveUSeSystemConfig(systemSetting);

                lock (m_syncObj)
                {
                    m_systemSetting = systemSetting.Clone();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("保存系统设定失败," + ex.Message);
            }

            SafeFireSystemSettingChanged();
        }

        /// <summary>
        /// 获取用户自定义配置。
        /// </summary>
        /// <returns></returns>
        public UserDefineSetting GetUserDefineSetting()
        {
            Debug.Assert(m_userDefineSetting != null);
            lock (m_syncObj)
            {
                UserDefineSetting setting = m_userDefineSetting.Clone();
                return setting;
            }
        }


        /// <summary>
        /// 保存用户设定。
        /// </summary>
        /// <param name="userSetting"></param>
        public void SaveUserDefineSetting(UserDefineSetting userSetting)
        {
            try
            {
                m_dataAccessor.SaveUserDefineConfig(userSetting);

                lock (m_syncObj)
                {
                    m_userDefineSetting = userSetting.Clone();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("保存用户设定失败," + ex.Message);
            }

            SafeFireUserDefineSettingChanged();
        }
        /// <summary>
        /// 触发状态变更事件。
        /// </summary>
        private void SafeFireSystemSettingChanged()
        {
            SystemConfigChangedEventHandle handle = this.OnSystemSettingChanged;

            if (handle != null)
            {
                try
                {
                    handle();
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 触发状态变更事件。
        /// </summary>
        private void SafeFireUserDefineSettingChanged()
        {
            UserDefineSettingChangedEventHandle handle = this.OnUserDefineSettingChanged;

            if (handle != null)
            {
                try
                {
                    handle();
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        public override string ToString()
        {
            return "SystemConfigManager";
        }
    }
}
