using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 系统配置。
    /// </summary>
    public class USeSystemSetting
    {
        /// <summary>
        /// 市价单计算方式。
        /// </summary>
        public USeMarketPriceMethod MarketPriceMethods { get; set; }

        /// <summary>
        /// 任务下单设定。
        /// </summary>
        public TaskOrderSetting TaskOrder { get; set; }

        /// <summary>
        /// 保证金设定。
        /// </summary>
        public OrderMarginSetting OrderMargin { get; set; }

        /// <summary>
        /// 预警设定。
        /// </summary>
        public AlarmSetting Alarm { get; set; }

        /// <summary>
        /// 克隆。
        /// </summary>
        /// <returns></returns>
        public USeSystemSetting Clone()
        {
            USeSystemSetting setting = new USeSystemSetting();
            setting.MarketPriceMethods = this.MarketPriceMethods;
            if (this.TaskOrder != null)
            {
                setting.TaskOrder = this.TaskOrder.Clone();
            }
            if(this.OrderMargin != null)
            {
                setting.OrderMargin = this.OrderMargin.Clone();
            }
            if(this.Alarm != null)
            {
                setting.Alarm = this.Alarm.Clone();
            }
            return setting;
        }

        /// <summary>
        /// 创建默认系统配置。
        /// </summary>
        /// <returns></returns>
        public static USeSystemSetting CreateDefault()
        {
            USeSystemSetting setting = new USeSystemSetting();
            setting.MarketPriceMethods = USeMarketPriceMethod.OpponentPrice;

            setting.TaskOrder = new TaskOrderSetting() {
                TaskMaxTryCount = 3,
                TryOrderMinInterval = new TimeSpan(0, 0, 5)
            };

            setting.OrderMargin = new OrderMarginSetting() {
                MaxUseRate = 0.9m
            };

            setting.Alarm = new AlarmSetting() {

            };

            return setting;
        }
    }

    /// <summary>
    /// 任务尝试下单条件设定。
    /// </summary>
    public class TaskOrderSetting
    {
        /// <summary>
        /// 任务下单连续尝试次数。
        /// </summary>
        public int TaskMaxTryCount
        {
            get;
            set;
        }

        /// <summary>
        /// 连续两次下单尝试最小间隔时间。
        /// </summary>
        public TimeSpan TryOrderMinInterval
        {
            get;
            set;
        }

        public TaskOrderSetting Clone()
        {
            TaskOrderSetting setting = new TaskOrderSetting();
            setting.TaskMaxTryCount = this.TaskMaxTryCount;
            setting.TryOrderMinInterval = this.TryOrderMinInterval;

            return setting;
        }
    }

    /// <summary>
    /// 下单保证金设定。
    /// </summary>
    public class OrderMarginSetting
    {
        /// <summary>
        /// 最大占用比例。
        /// </summary>
        public decimal MaxUseRate { get; set; }

        public OrderMarginSetting Clone()
        {
            OrderMarginSetting setting = new OrderMarginSetting();
            setting.MaxUseRate = this.MaxUseRate;

            return setting;
        }
    }

    /// <summary>
    /// 预警设定。
    /// </summary>
    public class AlarmSetting
    {
        public AlarmSetting Clone()
        {
            AlarmSetting setting = new AlarmSetting();
            return setting;
        }
    }
}
