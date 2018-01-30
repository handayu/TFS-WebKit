using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USeFuturesSpirit
{
    public class GlobalFontServerConfig
    {
        /// <summary>
        /// 默认选中BrokerId。
        /// </summary>
        public string DefaultBrokerId { get; set; }

        /// <summary>
        /// 服务列表。
        /// </summary>
        public List<FrontSeverConfig> ServerList { get; set; }
    }
}
