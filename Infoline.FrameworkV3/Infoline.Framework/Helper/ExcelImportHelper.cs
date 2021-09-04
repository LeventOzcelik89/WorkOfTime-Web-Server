using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Helper
{

    public class ColumnInfo
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Type { get; set; }//Javascript Type
        public object DefaultValue { get; set; }
        public bool Required { get; set; }
        public bool Unique { get; set; }
        public string Description { get; set; }
        public string Info { get; set; }
        public bool IsHidden { get; set; }
    }


    public class ColumnInfoAttribute : Attribute
    {
        public string Alias { get; set; }
        public object DefaultValue { get; set; }
        public bool Required { get; set; }
        public bool IsUnique { get; set; }
        public string Description { get; set; }
        public string Info { get; set; }
        public bool IsHidden { get; set; }
        public ColumnInfoAttribute(string alias, bool required = false, object defaultValue = null, bool isUnique = false, string description = "", string info = "", bool isHidden = false)
        {
            Alias = alias;
            DefaultValue = defaultValue;
            Required = required;
            IsUnique = isUnique;
            Description = description;
            Info = info;
            IsHidden = isHidden;
        }
    }
    public class ExcelResultStatus
    {
        public bool status { get; set; }
        public string message { get; set; }
        public int rowNumber { get; set; }
    }


    public static class ExcelImportHelper
    {
        public static string GetSchema(Type excelClass, string tableName = "")
        {
            return Infoline.Helper.Json.Serialize(GetColumnInfo(excelClass, tableName));
        }
        public static ColumnInfo[] GetColumnInfo(Type excelClass, string tableName = "")
        {

            var list = new List<ColumnInfo>();
            foreach (var item in excelClass.GetProperties().OrderBy(a => a.MetadataToken))
            {
                var column = new ColumnInfo();
                column.Name = item.Name;

                if (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    column.Type = item.PropertyType.GetGenericArguments()[0].Name;
                }
                else
                {
                    column.Type = item.PropertyType.Name;
                }

                column.Alias = item.GetCustomAttribute<ColumnInfoAttribute>().Alias;
                column.DefaultValue = item.GetCustomAttribute<ColumnInfoAttribute>().DefaultValue;
                column.Required = item.GetCustomAttribute<ColumnInfoAttribute>().Required;
                column.Unique = item.GetCustomAttribute<ColumnInfoAttribute>().IsUnique;
                column.Info = item.GetCustomAttribute<ColumnInfoAttribute>().Info;
                column.IsHidden = item.GetCustomAttribute<ColumnInfoAttribute>().IsHidden;

                if (column.Type == "DateTime")
                {
                    column.Description = " Örnek : (12.03.1996)";
                }

                if (column.Type == "Double")
                {
                    column.Description = " Örnek : (1234.45)";
                }

                if (column.Type == "Boolean")
                {
                    column.Description = " (1 veya 0)";
                }


                list.Add(column);
            }
            return list.ToArray();
        }
    }



}
