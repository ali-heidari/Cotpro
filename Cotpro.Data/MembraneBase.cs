using System;
using System.Collections.Generic;
using System.Text;

namespace Cotpro.Data
{
    /// <summary>
    /// This class provide functions to communicate with database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MembraneBase<T> where T : new()
    {
        #region Fields

        private string[] _columnsNames = null;
        private System.Data.DataSet ds;

        #endregion

        #region Constructors
        public MembraneBase()
        {
            _columnsNames = this.FindColumns();
            ds = new System.Data.DataSet();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Dataset which provider work with it.
        /// </summary>
        protected System.Data.DataSet Dataset
        {
            get { return ds; }
            set { ds = value; }
        }
        /// <summary>
        /// Return an array of column names.
        /// </summary>
        protected string[] ColumnNames
        {
            get
            {
                return this._columnsNames;
            }
        }
        /// <summary>
        /// Return name of the table.
        /// </summary>
        protected string tableName
        {
            get
            {
                return this.GetTableName();
            }
        }

        #endregion


        #region Private Methods
        /// <summary>
        /// Iterate in properties of type T, and find those which declared with ColumnNameAttribute, then Name property of attribute that is the name of property in the table.
        /// </summary>
        /// <returns>String array of columns' names.</returns>
        private string[] FindColumns()
        {
            string columns = "";
            ColumnNameAttribute cn = null;
            System.Reflection.PropertyInfo[] propertyinfos = typeof(T).GetProperties();
            foreach (System.Reflection.PropertyInfo pi in propertyinfos)
            {
                foreach (object o in pi.GetCustomAttributes(typeof(ColumnNameAttribute), false))
                    if (o is ColumnNameAttribute)
                    {
                        cn = (ColumnNameAttribute)o;
                        columns += cn.Name + ",";
                    }
            }
            return columns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Return an array of properties from a type that declared with specified attribute.
        /// </summary>
        /// <param name="attribute">Type of attribute.</param>
        /// <returns>An array of attributes</returns>
        private System.Reflection.PropertyInfo[] GetProperties(Type attribute)
        {
            List<System.Reflection.PropertyInfo> prin = new List<System.Reflection.PropertyInfo>();
            System.Reflection.PropertyInfo[] propertyinfos = typeof(T).GetProperties();
            foreach (System.Reflection.PropertyInfo pi in propertyinfos)
            {
                foreach (object o in pi.GetCustomAttributes(attribute, false))
                    if (o is ColumnNameAttribute)
                    {
                        prin.Add(pi);
                        break;
                    }
            }
            return prin.ToArray();
        }

        /// <summary>
        /// Return table name of T from its TableNameAttribute.
        /// </summary>
        /// <returns>Table name.</returns>
        private string GetTableName()
        {
            string tableName = "";
            foreach (object o in typeof(T).GetCustomAttributes(false))
                if (o is TableNameAttribute)
                    tableName = ((TableNameAttribute)o).Name;
            return tableName;
        }

        /// <summary>
        /// Return column name of property.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private string GetColumnName(System.Reflection.PropertyInfo propertyInfo)
        {
            foreach (object o in propertyInfo.GetCustomAttributes(false))
                if (o is ColumnNameAttribute)
                    return ((ColumnNameAttribute)o).Name;
            return "";
        }

        #endregion

        #region Public and protected Methods
        /// <summary>
        /// In inherited class first fill dataset then invoke base.select to get array of objects.
        /// </summary>
        /// <returns></returns>
        protected virtual T[] Select()
        {
            List<T> tlist = new List<T>();
            T t;
            string tableName = this.GetTableName();
            string columnName = "";
            System.Data.DataRow[] drs = ds.Tables[tableName].Select();


            System.Reflection.PropertyInfo[] properties = GetProperties(typeof(ColumnNameAttribute));
            foreach (System.Data.DataRow dr in drs)
            {
                t = new T();
                foreach (System.Reflection.PropertyInfo pi in properties)
                {
                    columnName = GetColumnName(pi);
                    if (pi.PropertyType.Name == dr.Table.Columns[columnName].DataType.Name)
                        pi.SetValue(t, Convert.ChangeType(dr[columnName].ToString(), pi.PropertyType), null);
                }
                tlist.Add(t);
            }


            return tlist.ToArray();
        }

        /// <summary>
        /// In inherited class should invoke base.insert and then Update dataadapter.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        protected virtual bool Insert(T t)
        {
            string tableName = this.GetTableName();
            string columnName = "";

            System.Data.DataRow dr = ds.Tables[tableName].NewRow();

            foreach (System.Reflection.PropertyInfo pi in GetProperties(typeof(ColumnNameAttribute)))
            {
                columnName = GetColumnName(pi);
                dr[columnName] = Convert.ChangeType(pi.GetValue(t, null), dr.Table.Columns[columnName].DataType);

            }
            ds.Tables[tableName].Rows.Add(dr);
            return false;
        }
        /// <summary>
        /// Return a dictionary with column name as key and conent as value.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        protected virtual Dictionary<string, object> Update(T t)
        {
            string tableName = this.GetTableName();
            string columnName = "";
            object value = null;
            Dictionary<string, object> data = new Dictionary<string, object>();

            foreach (System.Reflection.PropertyInfo pi in GetProperties(typeof(ColumnNameAttribute)))
            {
                columnName = GetColumnName(pi);
                value = pi.GetValue(t, null);
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    data.Add( columnName ,value.ToString());
            }

            return data;
        }

        /// <summary>
        /// Delete rows from database.
        /// </summary>
        /// <returns></returns>
        protected virtual bool Delete(string whereclause)
        {
            return false;
        }
        #endregion
    }
}
