using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MarketDataStore.Config
{
    /// <summary>
    /// Ctp交易服务器配置节定义。
    /// </summary>
    public class KLineStorageSection : ConfigurationSection
    {
        /// <summary>
        /// FileKLineStorage配置节集合, 只读。
        /// </summary>
        [ConfigurationProperty("FileKLineStorages", IsDefaultCollection = false)]
        public FileKLineStorageElementCollection FileStorages
        {
            get
            {
                return (FileKLineStorageElementCollection)base["FileKLineStorages"];
            }
        }

        /// <summary>
        /// MySqlKLineStorage配置节集合, 只读。
        /// </summary>
        [ConfigurationProperty("MySqlKLineStorages", IsDefaultCollection = false)]
        public MySqlKLineStorageElementCollection MySqlStorages
        {
            get
            {
                return (MySqlKLineStorageElementCollection)base["MySqlKLineStorages"];
            }
        }

        /// <summary>
        /// RabbitMQKLineStorage配置节集合, 只读。
        /// </summary>
        [ConfigurationProperty("RabbitMQKLineStorages", IsDefaultCollection = false)]
        public RabbitMQKLineStorageElementCollection RabbitMQStorages
        {
            get
            {
                return (RabbitMQKLineStorageElementCollection)base["RabbitMQKLineStorages"];
            }
        }

        /// <summary>
        /// RocketMQKLineStorage配置节集合, 只读。
        /// </summary>
        [ConfigurationProperty("RocketMQKLineStorages", IsDefaultCollection = false)]
        public RocketMQKLineStorageElementCollection RocketMQStorages
        {
            get
            {
                return (RocketMQKLineStorageElementCollection)base["RocketMQKLineStorages"];
            }
        }

        /// <summary>
        /// 获取系统默认配置文件里面的KLineStorageSection配置节集合。
        /// </summary>
        /// <returns>配置节。</returns>
        public static KLineStorageSection GetSection()
        {
            object section = ConfigurationManager.GetSection("KLineStorage");
            return section as KLineStorageSection;
        }
    }
}
