using System;
using System.Collections.Generic;
using System.Text;

namespace USe.Common.DBDriver
{
    /// <summary>
    /// SQLServer数据库SQL语句生成类。
    /// </summary>
    public class MySQLSqlCreator : DBSqlCreator
    {
        /// <summary>
        /// 创建者名称。
        /// </summary>
        public override string CreatorName
        {
            get { return "SQLServer"; }
        }

        /// <summary>
        /// 创建Insert语句。
        /// </summary>
        /// <param name="dbName">数据库名称。</param>
        /// <param name="tableName">数据库表名。</param>
        /// <param name="insertFields">插入字段。</param>
        /// <returns></returns>
        public override string CreateInsertSql(string dbName, string tableName, List<DBField> insertFields)
        {
            return CreateInsertSql(dbName, tableName, insertFields, true);
        }

        /// <summary>
        /// 创建Insert语句。
        /// </summary>
        /// <param name="dbName">数据库名称。</param>
        /// <param name="tableName">数据库表名。</param>
        /// <param name="insertFields">插入字段。</param>
        /// <returns></returns>
        public override string CreateInsertSql(string dbName, string tableName, List<DBField> insertFields, bool selectAutoID)
        {
            if (string.IsNullOrEmpty(tableName)) throw new ArgumentException("Invalid tableName.");
            if (insertFields == null || insertFields.Count == 0) throw new ArgumentException("Invalid insertFields.");

            StringBuilder fieldStr = new StringBuilder();
            StringBuilder valueStr = new StringBuilder();
            for (int i = 0; i < insertFields.Count; i++)
            {
                DBField field = insertFields[i];
                fieldStr.Append(field.FieldName + ",");
                valueStr.Append(string.Format("{0},", field.ValueString));
            }
            fieldStr = fieldStr.Remove(fieldStr.Length - 1, 1);
            valueStr = valueStr.Remove(valueStr.Length - 1, 1);

            return string.Format(" insert into {0}{1} ({2}) values({3}); {4}",
                string.IsNullOrEmpty(dbName) ? "" : dbName + ".",
                tableName,
                fieldStr.ToString(),
                valueStr.ToString(),
                selectAutoID ? "select @@identity;" : "");
        }

        /// <summary>
        /// 创建Update语句。
        /// </summary>
        /// <param name="dbName">数据库名称。</param>
        /// <param name="tableName">数据库表名。</param>
        /// <param name="updateFields">更新字段。</param>
        /// <param name="keyFields">关键字字段。</param>
        /// <returns></returns>
        public override string CreateUpdateSql(string dbName, string tableName, List<DBField> updateFields, List<DBField> keyFields)
        {
            if (string.IsNullOrEmpty(tableName)) throw new ArgumentException("Invalid tableName.");
            if (updateFields == null || updateFields.Count == 0) throw new ArgumentException("Invalid updateFields.");

            StringBuilder updateStr = new StringBuilder();
            for (int i = 0; i < updateFields.Count; i++)
            {
                DBField field = updateFields[i];
                string fieldValue = field.FieldValue == null ? "" : field.FieldValue.ToString();
                updateStr.Append(string.Format("{0} = {2}{1}{2},", field.FieldName, fieldValue, field.WithQuote ? "'" : ""));
            }
            updateStr = updateStr.Remove(updateStr.Length - 1, 1);

            StringBuilder keyStr = new StringBuilder();
            for (int i = 0; i < keyFields.Count; i++)
            {
                DBField field = keyFields[i];
                keyStr.Append(string.Format("{0} = {2}{1}{2} and ", field.FieldName, field.FieldValue, field.WithQuote ? "'" : ""));
            }
            keyStr = keyStr.Remove(keyStr.Length - 4, 4);

            return string.Format("update {0}{1} set {2} where {3};",
                 string.IsNullOrEmpty(dbName) ? "" : dbName + ".",
                 tableName,
                 updateStr.ToString(),
                 keyStr.ToString());
        }

        /// <summary>
        /// 创建Delete语句。
        /// </summary>
        /// <param name="dbName">数据库名称。</param>
        /// <param name="tableName">数据库表名。</param>
        /// <param name="keyFields">关键字字段。</param>
        /// <returns></returns>
        public override string CreateDeleteSql(string dbName, string tableName, List<DBField> keyFields)
        {
            if (string.IsNullOrEmpty(tableName)) throw new ArgumentException("Invalid tableName.");

            StringBuilder keyStr = new StringBuilder();
            for (int i = 0; i < keyFields.Count; i++)
            {
                DBField field = keyFields[i];
                keyStr.Append(string.Format("{0} = {2}{1}{2} and ", field.FieldName, field.FieldValue, field.WithQuote ? "'" : ""));
            }
            keyStr = keyStr.Remove(keyStr.Length - 4, 4);

            return string.Format("delete from {0}{1} where {2};",
                 string.IsNullOrEmpty(dbName) ? "" : dbName + ".",
                 tableName,
                 keyStr.ToString());
        }

        /// <summary>
        /// 获取Where查询条件。
        /// </summary>
        /// <param name="queryFields"></param>
        /// <returns></returns>
        public override string GetWhereConditionSql(List<string> queryFields)
        {
            if (queryFields == null || queryFields.Count <= 0) return string.Empty;

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" where ");
            for (int i = 0; i < queryFields.Count; i++)
            {
                string item = queryFields[i];
                sbWhere.Append(item + " and ");
            }
            return sbWhere.Remove(sbWhere.Length - 4, 4).ToString();
        }
    }
}
