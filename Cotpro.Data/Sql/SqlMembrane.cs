using System;
using System.Collections.Generic;
using System.Text;

namespace Cotpro.Data.Sql
{
    public class SqlMembrane<T> : Cotpro.Data.MembraneBase<T> where T:new()
    {
        private System.Data.SqlClient.SqlDataAdapter _dataAdapter = null;
        private string _ConnectionString = "";

        public string ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }

        public SqlMembrane(string ConnectionString):base()
        {
            this._ConnectionString = ConnectionString;
            _dataAdapter = new System.Data.SqlClient.SqlDataAdapter("", this._ConnectionString);
            this.FillSchema();            
        }
        protected void FillSchema()
        {
            _dataAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * from [" + this.tableName + "]", new System.Data.SqlClient.SqlConnection(this._ConnectionString));
            _dataAdapter.FillSchema(this.Dataset, System.Data.SchemaType.Mapped,this.tableName);
        }
        /// <summary>
        /// Generate a select query.
        /// </summary>
        /// <param name="WhereClause"></param>
        /// <param name="OrderByClause"></param>
        /// <param name="Grouping"></param>
        /// <returns></returns>
        private string SelectQuery(string WhereClause)
        {
            string columnNames = "";
            foreach (string s in this.ColumnNames)
                columnNames += s + ",";
            columnNames = columnNames.Remove(columnNames.Length - 1);
             string selectQuery = "SELECT " + columnNames + " FROM " + this.tableName;
             if (!string.IsNullOrEmpty(WhereClause))
                 selectQuery += " WHERE " + WhereClause;
             return selectQuery;
        }
        public T[] Select(string WhereClause)
        {            
            _dataAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand(SelectQuery(WhereClause),new System.Data.SqlClient.SqlConnection(this._ConnectionString));
            _dataAdapter.Fill(this.Dataset,tableName);
            return base.Select();
        }
        /// <summary>
        /// Insert data into database.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Insert(T[] t)
        {
            foreach (T te in t)
                base.Insert(te);
            System.Data.SqlClient.SqlCommandBuilder scb = new System.Data.SqlClient.SqlCommandBuilder(_dataAdapter);
            //_dataAdapter.InsertCommand= scb.GetInsertCommand();
            if (_dataAdapter.Update(this.Dataset, this.tableName) > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Update database rows with specified object. If property be null or empty , it wouldnt affect on any row.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="WhereClause">sql server </param>
        /// <returns></returns>
        public bool Update(T t, string WhereClause)
        {
            string sqlUpdateQuery = "UPDATE " + this.tableName + " SET ";
            Dictionary<string, object> obj = base.Update(t);

            System.Data.SqlClient.SqlCommand sc = new System.Data.SqlClient.SqlCommand();

            foreach (string key in obj.Keys)
            {
                sqlUpdateQuery += key + "=@" + key + ",";
                sc.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + key, obj[key]));
            }
            if (!string.IsNullOrEmpty(WhereClause))
                sc.CommandText = sqlUpdateQuery.Remove(sqlUpdateQuery.Length - 1) + " WHERE " + WhereClause;
            else
                sc.CommandText = sqlUpdateQuery.Remove(sqlUpdateQuery.Length - 1);
            
            sc.CommandType = System.Data.CommandType.Text;
            sc.Connection = _dataAdapter.SelectCommand.Connection;
            _dataAdapter.UpdateCommand = sc;
            sc.Connection.Open();
            if (sc.ExecuteNonQuery() > 0)
            {
                sc.Connection.Close();
                return true;
            }
            sc.Connection.Close();
            return false;
        }

        /// <summary>
        /// Delete item frm database.
        /// </summary>
        /// <param name="WhereClause"></param>
        /// <returns></returns>
        public bool Delete(string WhereClause)
        {
            if (string.IsNullOrEmpty(WhereClause))
                throw new ArgumentNullException("Where clause can not be null or empty.");
            string sqlDeleteQuery = "DELETE FROM " + this.tableName + " WHERE " + WhereClause;

            System.Data.SqlClient.SqlCommand sc = new System.Data.SqlClient.SqlCommand(sqlDeleteQuery,_dataAdapter.SelectCommand.Connection);
            sc.Connection.Open();
            if (sc.ExecuteNonQuery() > 0)
            {
                sc.Connection.Close();
                return true;
            }
            sc.Connection.Close();
            return false;

        }

        /// <summary>
        /// It Executes any query.
        /// it uses error catching , so any probable error raised, this method rollback all the excuted queries.
        /// [In multi queries string, you should seprate query sections with GO]
        /// </summary>
        /// <param name="query">Query string to be executed.</param>
        /// <returns>Returns true if operation completed. otherwise false.</returns>
        public bool ExecuteNonQuery(string query)
        {
            string[] parts = query.Split(new string[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
            string GQuery = @"DECLARE @intErrorCode INT 
                                BEGIN TRANSACTION  ";
            foreach (string s in parts)
                GQuery += s + @" GO
    SELECT @intErrorCode = @@ERROR
    IF (@intErrorCode <> 0) GOTO PROBLEM";

            GQuery += @"    COMMIT TRANSACTION  
            
PROBLEM:
IF (@intErrorCode <> 0) BEGIN
PRINT 'Unexpected error occurred!'
    ROLLBACK TRAN
END";


            System.Data.SqlClient.SqlCommand sc = new System.Data.SqlClient.SqlCommand(GQuery, _dataAdapter.SelectCommand.Connection);
            sc.Connection.Open();
            try
            {
                if (sc.ExecuteNonQuery() > 0)
                {
                }
                    sc.Connection.Close();
                    return true;
            }
            catch (System.Data.SqlClient.SqlException exception)
            {
                throw exception;
            }
            finally
            {
                sc.Connection.Close();
            }
            return false;
        }
        /// <summary>
        /// It opens a file and execute its content.
        /// it uses error catching , so any probable error raised, this method rollback all the excuted queries.
        /// [In multi queries string, you should seprate query sections with GO]
        /// </summary>
        /// <param name="path">Path of file.</param>
        /// <param name="IsSql">is a .sql file?</param>
        /// <returns>Returns true if operation completed. otherwise false.</returns>
        public bool ExecuteNonQuery(string path, bool IsSql)
        {
            if (!System.IO.File.Exists(path))
                throw new System.IO.FileNotFoundException("No file found in given address.[" + path + "]");

            bool res = false;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
            {
                res = ExecuteNonQuery(sr.ReadToEnd());
            }
            return res;
        }
    }
}
