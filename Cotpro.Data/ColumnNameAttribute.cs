using System;
using System.Collections.Generic;
using System.Text;

namespace Cotpro.Data
{
    /// <summary>
    /// An attribute to define column name of field in database equal to specified property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute:Attribute
    {
        /// <summary>
        /// Name of property in database table.
        /// </summary>
        public string Name { get; set; }

        public ColumnNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
