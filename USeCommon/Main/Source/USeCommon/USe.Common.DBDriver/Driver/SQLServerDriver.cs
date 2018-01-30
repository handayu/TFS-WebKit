using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections;

namespace USe.Common.DBDriver
{
    /// <summary>
    /// SqlServer数据库访问类。
    /// </summary>
    public sealed class SQLServerDriver
    {
        #region member
        private string m_connStr = string.Empty;
        private SqlConnection m_connection;
        private SqlCommand m_command;
        private SqlDataAdapter m_adapter;
        private SqlDataReader m_reader;
        private SqlTransaction m_trans;
        private DBTranType m_tranType;
        #endregion // member

        #region construction
        public SQLServerDriver(string connStr)
            : this(DBTranType.NoTransaction, connStr)
        {
        }

        public SQLServerDriver(DBTranType tranType, string connstr)
        {
            m_tranType = tranType;
            m_connection = new SqlConnection(connstr);
            
            m_command = new SqlCommand();
            Connect();
            if (m_tranType == DBTranType.Transaction)
            {
                m_trans = m_connection.BeginTransaction();
                m_command.Transaction = m_trans;
            }
            m_command.Connection = m_connection;
        }
        #endregion // construction

        #region methods
        /// <summary>
        /// 连接数据库。
        /// </summary>
        private void Connect()
        {
            if (m_connection.State != ConnectionState.Open)
            {
                m_connection.Open();
            }
        }

        /// <summary>
        /// 执行无返回信息的行的SQL，如insert,update,delete返回受影响的行数。
        /// </summary>
        public int Command(string strSql)
        {
            try
            {
                if (m_reader != null)
                {
                    if (!m_reader.IsClosed)
                    {
                        m_reader.Close();
                    }
                }
                Connect();
                m_command.Connection = m_connection;
                m_command.CommandText = strSql;
                return m_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (m_tranType == DBTranType.Transaction)
                {
                    Rollback();
                }
                this.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// 执行无返回信息的行的SQL，如insert,update,delete返回受影响的行数。
        /// </summary>
        public int Command(string strSql,List<SqlParameter> parameterList)
        {
            try
            {
                if (m_reader != null)
                {
                    if (!m_reader.IsClosed)
                    {
                        m_reader.Close();
                    }
                }
                Connect();
                m_command.Connection = m_connection;
                m_command.CommandText = strSql;
                m_command.Parameters.Clear();
                if (parameterList != null && parameterList.Count > 0)
                {
                    foreach (SqlParameter parameter in parameterList)
                    {
                        m_command.Parameters.Add(parameter);
                    }
                }
                return m_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (m_tranType == DBTranType.Transaction)
                {
                    Rollback();
                }
                this.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// 执行带返回结果的SQL。
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public object ExeScalar(string strSql)
        {
            try
            {
                if (m_reader != null)
                {
                    if (!m_reader.IsClosed)
                    {
                        m_reader.Close();
                    }
                }
                Connect();

                m_command.Connection = m_connection;
                m_command.CommandText = strSql;
                return m_command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                if (m_tranType == DBTranType.Transaction)
                {
                    Rollback();
                }
                this.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// 执行SQL语句进行查询，返回的行在本类中的reader中
        /// </summary>
        public void Query(String strSql)
        {
            try
            {
                if (m_reader != null)
                {
                    if (!m_reader.IsClosed)
                    {
                        m_reader.Close();
                    }
                }
                Connect();
                m_command.CommandText = strSql;
                m_reader = m_command.ExecuteReader();
            }
            catch (Exception ex)
            {
                if (m_tranType == DBTranType.Transaction)
                {
                    Rollback();
                }
                Dispose();

                throw ex;
            }
        }

        public DataTable QueryTable(string strSql)
        {
            try
            {
                Connect();
                m_command.Connection = m_connection;
                m_command.CommandText = strSql;
                m_command.CommandTimeout = 30;

                m_adapter = new System.Data.SqlClient.SqlDataAdapter();
                m_adapter.SelectCommand = m_command;

                DataTable table = new DataTable();
                m_adapter.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                if (m_tranType == DBTranType.Transaction)
                {
                    Rollback();
                }
                Dispose();

                throw ex;
            }
        }

        /// <summary>
        /// 执行SQL将数据存入myDataSet
        /// </summary>
        public DataSet QueryDataSet(string strSql)
        {
            try
            {
                Connect();
                m_command.Connection = m_connection;
                m_command.CommandText = strSql;
                m_command.CommandTimeout = 30;

                m_adapter = new System.Data.SqlClient.SqlDataAdapter();
                m_adapter.SelectCommand = m_command;

                DataSet dataSet = new DataSet();
                m_adapter.Fill(dataSet);
                return dataSet;
            }
            catch (Exception ex)
            {
                if (m_tranType == DBTranType.Transaction)
                {
                    Rollback();
                }
                Dispose();

                throw ex;
            }

        }

        /// <summary>
        /// 采用事务处理方式时，确定提交数据
        /// </summary>
        public void Commit()
        {
            try
            {
                if (m_tranType == DBTranType.NoTransaction
                    || m_trans == null 
                    || m_trans.Connection == null)
                {
                    return;
                }
                m_trans.Commit();
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// 采用事务处理方式时，发生错误，进行数据回滚
        /// </summary>
        public void Rollback()
        {
            try
            {
                if (m_tranType == DBTranType.NoTransaction
                    || m_trans == null
                    || m_trans.Connection == null)
                {
                    return;
                }
                m_trans.Rollback();
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// 对SqlServerDriver类中的资源释放
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (m_reader != null && !m_reader.IsClosed)
                {
                    m_reader.Close();
                    m_reader = null;
                }
                if (m_trans != null)
                {
                    m_trans.Dispose();
                    m_trans = null;
                }
                if (m_adapter != null)
                {
                    m_adapter.Dispose();
                    m_adapter = null;
                }
                if (m_command != null)
                {
                    m_command.Dispose();
                    m_command = null;
                }
                if (m_connection != null)
                {
                    if (m_connection.State == ConnectionState.Open)
                    {
                        m_connection.Close();
                    }
                    m_connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }
        #endregion // methods

        #region static methods
        /// <summary>
        /// 查询数据。
        /// </summary>
        /// <param name="strConn"></param>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataTable GetTableFromDB(string strConn, string strSql)
        {
            SqlConnection sqlConn = new SqlConnection(strConn);
            DataTable table = new DataTable();
            try
            {
                sqlConn.Open();
                SqlDataAdapter oDa = new SqlDataAdapter(strSql, sqlConn);
                oDa.Fill(table);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                }
            }
            return table;
        }

        /// <summary>
        /// 查询数据。
        /// </summary>
        /// <param name="strConn"></param>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataSet GetDataFromDB(string strConn, string strSql)
        {
            SqlConnection sqlConn = new SqlConnection(strConn);
            DataSet dataSet = new DataSet();
            try
            {
                sqlConn.Open();
                SqlDataAdapter oDa = new SqlDataAdapter(strSql, sqlConn);
                oDa.Fill(dataSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                }
            }
            return dataSet;
        }

        /// <summary>
        /// 更新数据库。
        /// </summary>
        /// <param name="strConn"></param>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static int UpdateDatabase(string strConn, string strSql)
        {
            SqlConnection sqlConn = new SqlConnection(strConn);
            int iRows;
            try
            {
                sqlConn.Open();
                SqlCommand oComm = new SqlCommand(strSql, sqlConn);
                iRows = oComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlConn.State == ConnectionState.Open)
                    sqlConn.Close();
            }
            return iRows;
        }

        /// <summary>
        /// 更新数据库,并且有返回值。
        /// </summary>
        /// <param name="strConn"></param>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static object UpdateDatabaseWithResult(string strConn, string strSql)
        {
            SqlConnection sqlConn = new SqlConnection(strConn);
            object result = null;
            try
            {
                sqlConn.Open();
                SqlCommand oComm = new SqlCommand(strSql, sqlConn);
                result = oComm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlConn.State == ConnectionState.Open)
                    sqlConn.Close();
            }
            return result;
        }

        /// <summary>
        /// 事务执行命令集合。
        /// </summary>
        /// <param name="commond"></param>
        public static void ExcuteTransCommand(string strConn,List<string> commond)
        {
            if (commond == null || commond.Count <= 0) return;

            SqlConnection sqlConn = new SqlConnection(strConn);
            SqlCommand sqlCommand = new SqlCommand();
            SqlTransaction trans = null;

            string currentCommand = string.Empty;
            try
            {
                sqlConn.Open();
                trans = sqlConn.BeginTransaction();
                sqlCommand.Transaction = trans;
                sqlCommand.Connection = sqlConn;

                foreach (string item in commond)
                {
                    currentCommand = item;
                    sqlCommand.CommandText = item;
                    sqlCommand.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }

                throw ex;
            }
            finally
            {
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                }
            }
        }
        #endregion // static methods
    }
}