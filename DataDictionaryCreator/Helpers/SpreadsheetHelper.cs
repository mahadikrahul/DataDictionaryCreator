using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataDictionaryCreator.Helpers
{
    public static class SpreadsheetHelper
    {
        /// <summary>
        /// Converts to spreadsheet.
        /// </summary>
        /// <param name="lists">The lists.</param>
        /// <returns></returns>
        public static byte[] ToSpreadsheet(this Dictionary<string, DataTable> lists)
        {
            using ExcelPackage package = new ExcelPackage();
            if (lists != null && lists.Any())
            {
                DataTable dataTable;
                ExcelWorksheet wsData;
                foreach (var item in lists)
                {
                    dataTable = item.Value;
                    wsData = package.Workbook.Worksheets.Add(item.Key);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        wsData.Cells["A1"].LoadFromDataTable(dataTable, true);
                        using (ExcelRange objRange = wsData.Cells["A1:XFD1"])
                        {
                            objRange.Style.Font.Bold = true;
                            objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }

                        wsData.Cells.AutoFitColumns();
                    }
                }

                return package.GetAsByteArray();
            }

            return null;
        }
    }
}