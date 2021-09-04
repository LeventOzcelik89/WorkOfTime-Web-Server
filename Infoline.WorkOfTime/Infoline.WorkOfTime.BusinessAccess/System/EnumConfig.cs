using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class EnumInfo : Attribute
    {
        public Type TableObject { get; set; }
        public string TableField { get; set; }

        public EnumInfo(Type tableObject, string tableField)
        {
            this.TableObject = tableObject;
            this.TableField = tableField;
                //tableObject.GetProperty(tableField) != null
                //? tableObject.GetProperty(tableField).Name
                //: null;
        }


        public static void Execute()
        {

            var db = new WorkOfTimeDatabase();
            var oldList = db.GetSYS_Enums().ToList();
            var newList = new List<SYS_Enums>();
            var projectEnums = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.IsEnum == true).ToList();

            foreach (var item in projectEnums)
            {

                var _tableName = String.Empty;
                var _fieldName = String.Empty;

                if (item.GetCustomAttribute<EnumInfo>() != null)
                {
                    var Attribute = item.GetCustomAttribute<EnumInfo>();
                    _tableName = Attribute.TableObject.Name;
                    _fieldName = Attribute.TableField;
                }

                foreach (var prop in Enum.GetValues(item))
                {

                    var key = (int)prop;
                    var value = prop.ToString();
                    var description = item.GetField(value).GetCustomAttribute<DescriptionAttribute>() != null ? item.GetField(value).GetCustomAttribute<DescriptionAttribute>().Description : null;
                    newList.Add(new SYS_Enums
                    {
                        createdby = Guid.Empty,
                        created = DateTime.Now,
                        tableName = _tableName,
                        fieldName = _fieldName,
                        Name = item.Name,
                        enumKey = key,
                        enumValue = value,
                        enumDescription = description
                    });
                }

            }

            var rs = db.AllDeleteSYS_Enums();
            rs &= db.BulkInsertSYS_Enums(newList);

        }
    }

}
