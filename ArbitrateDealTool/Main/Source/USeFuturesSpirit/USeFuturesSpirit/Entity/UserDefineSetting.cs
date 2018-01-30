using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 用户自定义配置。
    /// </summary>
    public class UserDefineSetting
    {
        /// <summary>
        /// 上次登录投资者ID。
        /// </summary>
        public string LastInvestorId { get; set; }

        /// <summary>
        /// 是否保存帐号。
        /// </summary>
        public bool SaveInvestorId { get; set; }

        /// <summary>
        /// 上次选择品种。
        /// </summary>
        public string LastSelectProduct { get; set; }

        /// <summary>
        /// 上次选择品种。
        /// </summary>
        public string LastSelectProductName { get; set; }

        public UserDefineSetting Clone()
        {
            UserDefineSetting setting = new UserDefineSetting() {
                LastInvestorId = this.LastInvestorId,
                SaveInvestorId = this.SaveInvestorId,
                LastSelectProduct = this.LastSelectProduct,
                LastSelectProductName = this.LastSelectProductName
            };

            return setting;
        }

        /// <summary>
        /// 创建用户默认定义。
        /// </summary>
        /// <returns></returns>
        public static UserDefineSetting CreateDefault()
        {
            UserDefineSetting config = new UserDefineSetting() {
                LastInvestorId = string.Empty,
                SaveInvestorId = true,
                LastSelectProduct = "cu",
                LastSelectProductName = "铜"
            };
            return config;
        }
    }
}
