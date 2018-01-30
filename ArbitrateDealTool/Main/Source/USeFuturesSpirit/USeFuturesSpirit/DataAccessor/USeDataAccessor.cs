using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using USe.Common.AppLogger;
using USe.Common;
using System.Xml.Serialization;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    /// <summary>
    /// 数据访问器。
    /// </summary>
    internal partial class USeDataAccessor
    {
        #region member
        private const string ORDER_TIME_FORMAT = "yyyyMMddHHmmssfff";
        private string m_filePath = string.Empty;  // 
        private object m_syncObj = new object();
        private IAppLogger m_serverLogger = null;
        #endregion

        #region construction
        public USeDataAccessor()
        {
            string localApplicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.RootPath = Path.Combine(localApplicationDataPath, @"USe\USeFuturesSpirit\Data");

            m_serverLogger = new NullLogger("USeDataAccessor<Null>");
        }
        #endregion

        public IAppLogger ServerLogger
        {
            set
            {
                if (value != null)
                {
                    m_serverLogger = value;
                }
                else
                {
                    m_serverLogger = new NullLogger("USeDataAccessor<Null>");
                }
            }
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        public void Initialize()
        {
        }

        public override string ToString()
        {
            return "USeDataAccessor";
        }

        #region private class
        /// <summary>
        /// 已完成套利单关键字。
        /// </summary>
        private class ArbitrageOrderKey
        {
            /// <summary>
            /// 创建时间。
            /// </summary>
            public DateTime CreateTime { get; set; }

            /// <summary>
            /// 结束时间。
            /// </summary>
            public DateTime? FinishTime { get; set; }

            /// <summary>
            /// 文件路径。
            /// </summary>
            public string FilePath { get; set; }

            /// <summary>
            /// 创建套利单关键字。
            /// </summary>
            /// <param name="fileFullName"></param>
            /// 文件名
            /// <returns></returns>
            public static ArbitrageOrderKey Create(string fileFullName)
            {
                if (string.IsNullOrEmpty(fileFullName))
                {
                    throw new ArgumentNullException("fileName", "fileName is empty");
                }

                ArbitrageOrderKey orderKey = new ArbitrageOrderKey();
                try
                {
                    FileInfo fileInfo = new FileInfo(fileFullName);
                    string[] orderInfo = fileInfo.Name.Split(new char[] { '_', '.' });
                    if (orderInfo.Length == 4)
                    {
                        if (orderInfo[0] != "ArbitrageOrder" || orderInfo[3].ToLower() != "xml")
                        {
                            throw new Exception("Is not arbitrageOrder file");
                        }

                        orderKey.CreateTime = ToDateTime(orderInfo[1]);
                        orderKey.FinishTime = ToDateTime(orderInfo[2]);
                        orderKey.FilePath = fileFullName;
                    }
                    else if (orderInfo.Length == 3)
                    {
                        if (orderInfo[0] != "ArbitrageOrder" || orderInfo[2].ToLower() != "xml")
                        {
                            throw new Exception("Invalid arbitrageOrder file");
                        }
                        orderKey.CreateTime = ToDateTime(orderInfo[1]);
                        orderKey.FilePath = fileFullName;
                    }
                    else
                    {
                        throw new Exception("Invalid arbitrageOrder file");
                    }
                    return orderKey;
                }
                catch (Exception ex)
                {
                    throw new Exception("CreatArbitrageOrderKey failed,Error:" + ex.Message);
                }
            }
        }

        private static DateTime ToDateTime(string value)
        {
            DateTime time = DateTime.ParseExact(value, "yyyyMMddHHmmssfff", null);
            return time;
        }
        #endregion
    }
}
