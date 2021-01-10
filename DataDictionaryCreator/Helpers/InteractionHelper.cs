using DataDictionaryCreator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DataDictionaryCreator.Helpers
{
    public static class InteractionHelper
    {
        public static void CreateDictionary()
        {
            var dt = GetData();
        }

        private static DataTable GetData()
        {
            var dh = new DatabaseHelper();
            var dt = dh.ExecuteQuery(Query.GetAllTableData);
            var tableDetails = GetTableDetails(dt);
            var spreadsheet = tableDetails.ToSpreadsheet();
            File.WriteAllBytes(@$"{GlobalProperties.FilePath}\\DataDictionary.xlsx", spreadsheet);
            return dt;
        }

        private static Dictionary<string, DataTable> GetTableDetails(DataTable dt)
        {
            Dictionary<string, DataTable> tableDetails = new Dictionary<string, DataTable>();
            if (dt != null && dt.Rows.Count > 0)
            {
                var table = dt.AsEnumerable();
                var tableNames = table.Select(x => x.Field<string>("Table Name")).Distinct().ToList();
                foreach (var entity in tableNames)
                {
                    var tableDetail = table.Where(x => x.Field<string>("Table Name") == entity)
                        .Select(x => new
                        {
                            Postion = x.Field<int>("Position"),
                            ColumnName = x.Field<string>("Column Name"),
                            DefaultValue = x.Field<string>("Default Value"),
                            IsNullable = x.Field<string>("Is Nullable"),
                            DataType = x.Field<string>("Data Type"),
                            PrimaryKey = x.Field<string>("Primary Key"),
                            Description = x.Field<string>("Description")
                        }).ToList().ToDataTable();

                    for (int i = 0; i < tableDetail.Columns.Count; i++)
                    {
                        string columnName = tableDetail.Columns[i].ColumnName;
                        columnName = Regex.Replace(columnName, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
                        tableDetail.Columns[i].ColumnName = columnName;
                    }

                    tableDetails[entity] = tableDetail;
                }

            }

            return tableDetails;
        }

    }
}