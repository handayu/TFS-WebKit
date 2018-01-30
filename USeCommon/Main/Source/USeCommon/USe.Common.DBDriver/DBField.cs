using System;
using System.Collections.Generic;

namespace USe.Common.DBDriver
{
    /// <summary>
    /// 数据库字段定义。
    /// </summary>
    public class DBField
    {
        #region construction
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldValue">字段值。</param>
        public DBField(string fieldName, int fieldValue)
            : this(fieldName, fieldValue, false)
        {
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldValue">字段值。</param>
        public DBField(string fieldName, int fieldValue, bool withQuote)
        {
            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.WithQuote = withQuote;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldValue">字段值。</param>
        public DBField(string fieldName, double fieldValue)
            : this(fieldName, fieldValue, false)
        {
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldValue">字段值。</param>
        public DBField(string fieldName, double fieldValue, bool withQuote)
        {
            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.WithQuote = withQuote;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldValue">字段值。</param>
        public DBField(string fieldName, float fieldValue)
            : this(fieldName, fieldValue, false)
        {
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldValue">字段值。</param>
        public DBField(string fieldName, float fieldValue, bool withQuote)
        {
            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.WithQuote = withQuote;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldValue">字段值。</param>
        public DBField(string fieldName, decimal fieldValue)
            : this(fieldName, fieldValue, false)
        {
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldValue">字段值。</param>
        public DBField(string fieldName, decimal fieldValue, bool withQuote)
        {
            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.WithQuote = withQuote;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldValue">字段值。</param>
        public DBField(string fieldName, string fieldValue)
            : this(fieldName, fieldValue, true)
        {
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldValue">字段值。</param>
        public DBField(string fieldName, string fieldValue, bool withQuote)
        {
            this.FieldName = fieldName;
            if (withQuote)
            {
                if (fieldValue != null)
                {
                    this.FieldValue = fieldValue.Replace("'", "''");
                }
                else
                {
                    this.FieldValue = fieldValue;
                }
            }
            else
            {
                this.FieldValue = fieldValue;
            }
            this.WithQuote = withQuote;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldValue">字段值。</param>
        public DBField(string fieldName, DateTime fieldValue,DatetimeType type)
            : this(fieldName, fieldValue,type, true)
        {
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldValue">字段值。</param>
        public DBField(string fieldName, DateTime fieldValue, DatetimeType type, bool withQuote)
        {
            this.FieldName = fieldName;
            if (type == DatetimeType.Date)
            {
                this.FieldValue = fieldValue.ToString("yyyy-MM-dd");
            }
            else if (type == DatetimeType.Time)
            {
                this.FieldValue = fieldValue.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (type == DatetimeType.LongTime)
            {
                this.FieldValue = fieldValue.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            this.WithQuote = withQuote;
        }

        ///// <summary>
        ///// 构造函数。
        ///// </summary>
        ///// <param name="fieldName">字段名称。</param>
        ///// <param name="fieldValue">字段值。</param>
        //public DBField(string fieldName, object fieldValue)
        //    : this(fieldName, fieldValue, true)
        //{
        //}

        ///// <summary>
        ///// 构造函数。
        ///// </summary>
        ///// <param name="fieldName">字段名称。</param>
        ///// <param name="fieldValue">字段值。</param>
        //public DBField(string fieldName, object fieldValue, bool withQuote)
        //{
        //    this.FieldName = fieldName;
        //    this.FieldValue = fieldValue;
        //    this.WithQuote = withQuote;
        //    if (fieldValue == null || fieldValue == DBNull.Value)
        //    {
        //    }
        //    else
        //    {
        //        m_valueType = fieldValue.GetType();
        //    }
        //}
        #endregion // construction

        #region property
        /// <summary>
        /// 字段名称。
        /// </summary>
        public string FieldName
        {
            get;
            private set;
        }

        /// <summary>
        /// 字段值。
        /// </summary>
        public object FieldValue
        {
            get;
            private set;
        }

        /// <summary>
        /// 插入数据库是否带'。
        /// </summary>
        public bool WithQuote
        {
            get;
            private set;
        }

        public string ValueString
        {
            get
            {
                return string.Format("{1}{0}{1}", this.FieldValue == null ? "" : this.FieldValue.ToString(), this.WithQuote ? "'" : "");
            }
        }

        #endregion // property
    }

    /// <summary>
    /// 日期字串类型。
    /// </summary>
    public enum DatetimeType
    {
        /// <summary>
        /// 日期。
        /// </summary>
        Date,
        /// <summary>
        /// 日期+时间。
        /// </summary>
        Time,

        /// <summary>
        /// 带毫秒时间。
        /// </summary>
        LongTime,
    }
}
