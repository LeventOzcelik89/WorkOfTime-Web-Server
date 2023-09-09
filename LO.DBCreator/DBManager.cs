using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.CodeGeneration.CodeGenerators;
using Infoline.Framework.Database;
using Infoline.Framework.Database.Mssql;
using Infoline.WorkOfTime.BusinessAccess;

namespace TableGenerator
{
    public class DBManager
    {

        public DBManager() { }

        public void Run(string nameSpace)
        {

            var typeConverter = new SqlTypeConverter();
            var typeMapper = new MssqlTypeMapper();

            var classes = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.StartsWith(nameSpace)).GetTypes().Where(a => a.IsClass).ToArray();

            var classProperties = new Dictionary<String, Dictionary<String, Type>>();

            foreach (var clss in classes)
            {

                var columns = clss.GetProperties().Where(a => a.CanWrite)
                    .Select(a => string.Format("\t [{0}] {1} {2}", a.Name, typeMapper.GetSqlType(a.PropertyType), (Nullable.GetUnderlyingType(a.PropertyType) != null ? "NULL" : "NOT NULL")))
                    .ToArray();

                var query = string.Format("CREATE TABLE [{0}].[{1}](\n {2} \n )", "dbo", clss.Name, String.Join(System.Environment.NewLine + ",", columns));

                var db = new WorkOfTimeDatabase("Server=.;Database=WorkOfTime;User Id=sa;Password=Levent1125!;");
                var rs = db.ExecuteNonQuery(query);

            }

        }

    }
}