using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.Import
{
    public static class tool
    {
        public static DataTable ReadExcel(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                Excel.IExcelDataReader reader = null;
                try
                {
                    if (path.EndsWith(".xls"))
                    {
                        reader = Excel.ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    if (path.EndsWith(".xlsx"))
                    {
                        reader = Excel.ExcelReaderFactory.CreateOpenXmlReader(stream);

                    }

                    var _ds = reader.AsDataSet();
                    reader.Dispose();
                    var _dt = _ds.Tables[0];
                    if (_dt.Columns[0].Caption == "Column1")
                    {
                        for (int i = 0; i < _dt.Columns.Count; i++)
                        {
                            if (string.IsNullOrEmpty(_dt.Rows[0][i].ToString())) continue;
                            _dt.Columns[i].ColumnName = _dt.Rows[0][i].ToString();

                        }
                        _dt.Rows.RemoveAt(0);
                    }


                    return _dt;
                }
                catch 
                {

                }
            }


            return exceldata(path);
        }


        static DataTable exceldata(string filePath)
        {
            try
            {


                DataTable dtexcel = new DataTable();
                bool hasHeaders = false;
                string HDR = hasHeaders ? "Yes" : "No";
                string strConn;
                if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
                else
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                //Looping Total Sheet of Xl File
                /*foreach (DataRow schemaRow in schemaTable.Rows)
                {
                }*/
                //Looping a first Sheet of Xl File
                DataRow schemaRow = schemaTable.Rows[0];
                string sheet = schemaRow["TABLE_NAME"].ToString();
                if (!sheet.EndsWith("_"))
                {
                    string query = "SELECT  * FROM [" + sheet + "]";
                    OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                    dtexcel.Locale = CultureInfo.CurrentCulture;
                    daexcel.Fill(dtexcel);
                }

                conn.Close();
                return dtexcel;
            }
            catch 
            {

            }
            return null;

        }


        public static T ToObject<T>(this DataRow dataRow)
 where T : new()
        {
            T item = new T();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                var _colName = strCol(column.Caption);

                PropertyInfo property = GetProperty(typeof(T), _colName);

                

                if (property != null && dataRow[column] != DBNull.Value && dataRow[column].ToString() != "NULL")
                {
                    property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
                }
                else
                {

                }

            }

            return item;
        }

        public static string strCol(string str1)
        {
            return str1.ToLower().Replace(" ", "").Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u").Replace("ş","s").Replace("ç","c").Replace("ö","o");
        }
        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperties().Where(a=> strCol(a.Name)== (attributeName)).FirstOrDefault();

            if (property != null)
            {
                return property;
            }

            return type.GetProperties()
                 .Where(p => p.IsDefined(typeof(DisplayAttribute), false) && (p.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name) == (attributeName))
                 .FirstOrDefault();
        }

        public static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            return Convert.ChangeType(value, type);
        }
    }
}
