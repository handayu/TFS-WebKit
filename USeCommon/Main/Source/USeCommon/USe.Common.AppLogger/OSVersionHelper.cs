using System;

namespace USe.Common.AppLogger
{
    /// <summary>
    /// 操作系统类型。
    /// </summary>
    internal enum OSVertionType
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown,
         
        /// <summary>
        /// Windows 2000。
        /// </summary>
        Widows2000,

        /// <summary>
        /// Windows XP。
        /// </summary>
        WindesXP,

        /// <summary>
        /// Windows 2003。
        /// </summary>
        Windows2003,

        /// <summary>
        /// Windows Vista。
        /// </summary>
        WindowsVista,

        /// <summary>
        /// Windows7。
        /// </summary>
        Windows7,
    }

    /// <summary>
    /// 操作系统版本。
    /// </summary>
    internal class OSVersionHelper
    {
        /// <summary>
        /// 获取当前操作系统版本。
        /// </summary>
        /// <returns>当前操作系统版本。</returns>
        public static OSVertionType GetCurrenetOSVertion()
        {
            Version vertion = System.Environment.OSVersion.Version;
            if (vertion.Major == 5 && vertion.Minor == 0)
            {
                return OSVertionType.Widows2000;
            }
            else if (vertion.Major == 5 && vertion.Minor == 1)
            {
                return OSVertionType.WindesXP;
            }
            else if (vertion.Major == 5 && vertion.Minor == 2)
            {
                return OSVertionType.Windows2003;
            }
            else if (vertion.Major == 6 && vertion.Minor == 0)
            {
                return OSVertionType.WindowsVista;
            }
            else if (vertion.Major == 6 && vertion.Minor == 1)
            {
                return OSVertionType.Windows7;
            }
            else
            {
                return OSVertionType.Unknown;
            }
        }

        /// <summary>
        /// 是否包含UAC。
        /// </summary>
        /// <returns></returns>
        public static bool HasUAC()
        {
            Version vertion = System.Environment.OSVersion.Version;
            return (vertion.Major >= 6);
        }
    }
}
