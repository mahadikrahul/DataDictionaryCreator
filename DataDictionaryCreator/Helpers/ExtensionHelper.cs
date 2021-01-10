using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace DataDictionaryCreator.Helpers
{
    public static class ExtensionHelper
    {
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            DataTable dataTable = null;
            if (list != null && list.Any())
            {
                dataTable = new DataTable();
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                foreach (PropertyDescriptor prop in properties)
                {
                    dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

                foreach (T item in list)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }
    }
}