using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SupportUtilV4.Classes
{
    public static class DataTableExtensions
    {
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            var list = new List<T>();

            // Get the properties of the class T
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (DataRow row in table.Rows)
            {
                var obj = new T();
                foreach (var property in properties)
                {
                    if (table.Columns.Contains(property.Name))
                    {
                        var value = row[property.Name];
                        if (value != DBNull.Value)
                        {
                            property.SetValue(obj, Convert.ChangeType(value, property.PropertyType));
                        }
                    }
                }
                list.Add(obj);
            }

            return list;
        }
    }
}
