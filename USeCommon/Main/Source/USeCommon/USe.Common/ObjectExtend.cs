using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.Common
{
    /// <summary>
    /// Object类型转换数值扩展类。
    /// </summary>
    public static class ObjectExtend
    {
        /// <summary>
        /// 两数相除a/b。
        /// </summary>
        /// <param name="a">被除数。</param>
        /// <param name="b">除数。</param>
        /// <remarks>除数为0返回0。 </remarks>
        /// <returns></returns>
        public static decimal Divide(this decimal a, decimal b)
        {
            if (b == 0m)
            {
                return 0m;
            }
            else
            {
                return a / b;
            }
        }

        /// <summary>
        /// 转换为Int32类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回0。
        /// </remarks>
        public static int ToInt(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;

            int result = 0;
            int.TryParse(obj.ToString(), out result);
            return result;
        }

        /// <summary>
        /// 转换为int?类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回Null。
        /// </remarks>
        public static int? ToIntNull(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;

            int result = 0;
            if (int.TryParse(obj.ToString(), out result) == false)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 转换为Int16类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回0。
        /// </remarks>
        public static short ToInt16(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;

            short result = 0;
            short.TryParse(obj.ToString(), out result);
            return result;
        }

        /// <summary>
        /// 转换为Int16?类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回Null。
        /// </remarks>
        public static short? ToInt16Null(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;

            short result = 0;
            if (short.TryParse(obj.ToString(), out result) == false)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 转换为Double类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回0。
        /// </remarks>
        public static double ToDouble(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0d;

            double result = 0;
            double.TryParse(obj.ToString(), out result);
            return result;
        }

        /// <summary>
        /// 转换为Double?类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回Null。
        /// </remarks>
        public static double? ToDoubleNull(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;

            double result = 0;
            if (double.TryParse(obj.ToString(), out result) == false)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 转换为Float类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回0f。
        /// </remarks>
        public static float ToFloat(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0f;

            float result = 0f;
            float.TryParse(obj.ToString(), out result);
            return result;
        }

        /// <summary>
        /// 转换为Float?类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回Null。
        /// </remarks>
        public static float? ToFloatNull(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;

            float result = 0f;
            if (float.TryParse(obj.ToString(), out result) == false)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 转换为Decimal类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回0m。
        /// </remarks>
        public static decimal ToDecimal(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0m;

            decimal result = 0m;
            decimal.TryParse(obj.ToString(), out result);
            return result;
        }

        /// <summary>
        /// 转换为Decimal?类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回Null。
        /// </remarks>
        public static decimal? ToDecimalNull(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;

            decimal result = 0m;
            if (decimal.TryParse(obj.ToString(), out result) == false)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 转换为Decimal类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <param name="decimals">小数位数。</param>
        /// <returns>转换的值。</returns>
        public static decimal ToDecimal(this object obj, int decimals)
        {
            decimal result = obj.ToDecimal();
            return decimal.Round(result, decimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 转换为Decimal类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <param name="decimals">小数位数。</param>
        /// <returns>转换的值。</returns>
        public static decimal? ToDecimalNull(this object obj, int decimals)
        {
            decimal? result = obj.ToDecimalNull();
            if (result == null)
            {
                return null;
            }
            else
            {
                return decimal.Round(result.Value, decimals, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// 转换为DateTime类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回DateTime.MinValue。
        /// </remarks>
        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return DateTime.MinValue;

            DateTime result = DateTime.MinValue;
            DateTime.TryParse(obj.ToString(), out result);
            return result;
        }

        /// <summary>
        /// 转换为DateTime?类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回Null
        /// </remarks>
        public static DateTime? ToDateTimeNull(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;

            DateTime result = DateTime.MinValue;
            if (DateTime.TryParse(obj.ToString(), out result) == false)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 转换为Boolean类型。
        /// </summary>
        /// <param name="obj">包含要转换的object。</param>
        /// <returns></returns>
        /// <remarks>
        /// 若转换无效则返回false。
        /// </remarks>
        public static bool ToBoolean(this object obj)
        {
            if (obj == null || obj == DBNull.Value) return false;

            bool result = false;
            Boolean.TryParse(obj.ToString(), out result);
            return result;
        }
    }
}
