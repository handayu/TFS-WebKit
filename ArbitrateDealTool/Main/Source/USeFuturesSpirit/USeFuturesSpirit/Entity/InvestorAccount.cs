using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 投资者账户。
    /// </summary>
    class InvestorAccount
    {
        public InvestorAccount()
        {
        }

        public InvestorAccount(string brokerId,string acccount,string password)
        {
            this.BrokerId = brokerId;
            this.Account = acccount;
            this.Password = password;
        }

        /// <summary>
        /// 经纪商ID。
        /// </summary>
        public string BrokerId { get; set; }

        /// <summary>
        /// 账号。
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        public string Password { get; set; }

        public override string ToString()
        {
            return string.Format("({0}){1}", this.BrokerId, this.Account);
        }
    }
}
