using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.Common
{
    /// <summary>
    /// USeMath。
    /// </summary>
    public static class USeMath
    {
        /// <summary>
        /// 向下倍数取整。
        /// </summary>
        /// <param name="d">值。</param>
        /// <param name="multiple">倍数。</param>
        /// <returns></returns>
        public static decimal Floor(decimal d,decimal multiple)
        {
            decimal temp = Math.Floor(d / multiple);
            return temp * multiple;
        }

        /// <summary>
        /// 向下倍数取整。
        /// </summary>
        /// <param name="d">值。</param>
        /// <param name="multiple">倍数。</param>
        /// <returns></returns>
        public static double Floor(double d, double multiple)
        {
            double temp = Math.Floor(d / multiple);
            return temp * multiple;
        }

        /// <summary>
        /// 向上倍数取整。
        /// </summary>
        /// <param name="d">值。</param>
        /// <param name="multiple">倍数。</param>
        /// <returns></returns>
        public static decimal Ceiling(decimal d,decimal multiple)
        {
            decimal temp = Math.Ceiling(d / multiple);
            return temp * multiple;
        }

        /// <summary>
        /// 向上倍数取整。
        /// </summary>
        /// <param name="d">值。</param>
        /// <param name="multiple">倍数。</param>
        /// <returns></returns>
        public static double Ceiling(double d, double multiple)
        {
            double temp = Math.Ceiling(d / multiple);
            return temp * multiple;
        }

        /// <summary>
        /// 按倍数四舍五入。
        /// </summary>
        /// <param name="d">值。</param>
        /// <param name="multiple">倍数。</param>
        /// <returns></returns>
        public static decimal Round(decimal d, decimal multiple)
        {
            decimal temp = Math.Round(d / multiple);
            return temp * multiple;
        }

        /// <summary>
        /// 按倍数四舍五入。
        /// </summary>
        /// <param name="d">值。</param>
        /// <param name="multiple">倍数。</param>
        /// <returns></returns>
        public static double Round(double d, double multiple)
        {
            double temp = Math.Round(d / multiple);
            return temp * multiple;
        }
    }
}
