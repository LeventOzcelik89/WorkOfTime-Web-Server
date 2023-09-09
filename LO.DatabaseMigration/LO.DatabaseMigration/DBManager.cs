using Infoline.Framework.CodeGeneration.CodeGenerators;
using Infoline.Framework.Database;
using Infoline.Framework.Database.Mssql;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LO.DatabaseMigration
{
    public class DBManager
    {

        public DBManager() { }

        public void Run(string nameSpace)
        {

            //  RunAllTables(nameSpace);

            var clss = typeof(TEN_Tenant);

            CreateTable(clss, "Infoline.WorkOfTime.BusinessAccess");

        }

        private void RunAllTables(string nameSpace)
        {

            var typeConverter = new SqlTypeConverter();
            var typeMapper = new MssqlTypeMapper();

            var classes = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.StartsWith(nameSpace)).GetTypes().Where(a => a.IsClass).ToArray();

            foreach (var clss in classes)
            {
                CreateTable(clss, nameSpace);
            }
        }

        private string GetColumnString(string name, Type propertyType)
        {
            var typeMapper = new MssqlTypeMapper();

            var sqlType = typeMapper.GetSqlType(propertyType);
            var isNullable = name != "id"
                ? Nullable.GetUnderlyingType(propertyType) != null
                : false;

            isNullable = new string[] { "geography", "nvarchar(255)" }.Contains(sqlType);

            var res = string.Format("\t [{0}] {1} {2}", name, sqlType, (isNullable ? "NULL" : "NOT NULL"));

            switch (name)
            {
                case "id":
                    res = string.Format("{0} {1}", res, "PRIMARY KEY");
                    break;

                case "created":
                    res = string.Format("{0} {1}", res, "DEFAULT GETDATE()");
                    break;

                default:
                    break;
            }

            return res;

        }

        public void CreateTable(Type clss, string nameSpace)
        {

            var _name = clss.Name;
            if (clss.Namespace != nameSpace)
            {
                return;
            }
            if (_name.StartsWith("Page"))
            {
                return;
            }
            if (_name.StartsWith("VW"))
            {
                return;
            }

            var columns = clss.GetProperties()
                .OrderBy(a => typeof(InfolineTable).GetProperties().Select(b => b.Name).Contains(a.Name) ? 1 : 2)
                .Where(a => a.CanWrite)
                .Select(a => GetColumnString(a.Name, a.PropertyType))
                .ToArray();

            var query = string.Format("CREATE TABLE [{0}].[{1}](\n {2} \n )", "dbo", clss.Name, String.Join(System.Environment.NewLine + ",", columns));

            var db = new WorkOfTimeDatabase("Server=.;Database=WorkOfTime;User Id=sa;Password=Levent1125!;");
            var rs = db.ExecuteNonQuery(query);

        }
    }
}