using System;
using System.Collections.Generic;
using System.Text;

namespace USe.Common.DBDriver
{

    /// <summary>
    /// 数据库SQL语句生成类。
    /// </summary>
    public abstract class DBSqlCreator
    {
        /// <summary>
        /// Creator名称。
        /// </summary>
        public abstract string CreatorName
        {
            get;
        }

        /// <summary>
        /// 创建Insert语句。
        /// </summary>
        /// <param name="dbName">数据库名称。</param>
        /// <param name="tableName">数据库表名。</param>
        /// <param name="insertFields">插入字段。</param>
        /// <returns></returns>
        public abstract string CreateInsertSql(string dbName, string tableName, List<DBField> insertFields);

        /// <summary>
        /// 创建Insert语句。
        /// </summary>
        /// <param name="dbName">数据库名称。</param>
        /// <param name="tableName">数据库表名。</param>
        /// <param name="insertFields">插入字段。</param>
        /// <returns></returns>
        public abstract string CreateInsertSql(string dbName, string tableName, List<DBField> insertFields, bool selectAutoID);

        /// <summary>
        /// 创建Update语句。
        /// </summary>
        /// <param name="dbName">数据库名称。</param>
        /// <param name="tableName">数据库表名。</param>
        /// <param name="updateFields">更新字段。</param>
        /// <param name="keyFields">关键字字段。</param>
        /// <returns></returns>
        public abstract string CreateUpdateSql(string dbName, string tableName, List<DBField> updateFields, List<DBField> keyFields);

        /// <summary>
        /// 创建Delete语句。
        /// </summary>
        /// <param name="dbName">数据库名称。</param>
        /// <param name="tableName">数据库表名。</param>
        /// <param name="keyFields">关键字字段。</param>
        /// <returns></returns>
        public abstract string CreateDeleteSql(string dbName, string tableName,List<DBField> keyFields);

        /// <summary>
        /// 获取Where查询条件。
        /// </summary>
        /// <param name="queryFields"></param>
        /// <returns></returns>
        public abstract string GetWhereConditionSql(List<string> queryFields);


        /// <summary>
        /// 创建SQL语句创建者。
        /// </summary>
        /// <param name="creatorName">创建者名称。</param>
        /// <returns></returns>
        public DBSqlCreator CreateDBSqlCreator(string creatorName)
        {
            creatorName = creatorName.ToUpper();
            switch (creatorName)
            {
                case "SQLSERVER": return new SQLServerSqlCreator();
                case "ACCESS": return new AccessSqlCreator();
                default:
                    throw new Exception("Invalid creatorName");
            }
        }
    }
}
