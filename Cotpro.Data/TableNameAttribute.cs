using System;
using System.Collections.Generic;
using System.Text;

namespace Cotpro.Data
{
    /// <summary>
    /// Attribute to declare a class related with a table in database.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class)]
    public class TableNameAttribute:Attribute
    {        
        /// <summary>
        /// Name of class in database table.
        /// </summary>
        public string Name { get; set; }

        public TableNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
