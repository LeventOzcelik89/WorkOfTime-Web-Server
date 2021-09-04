using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Database.DataBaseHelper.PostgisGeoDatabaseHelper
{
    class PostgisGeoDatabaseTypeMapper
    {
    }

    public class PostgresqlWithGeodatabaseTypeMapper : ITypeMapper
    {

        public Type GetType(string sqlType)
        {
            throw new NotImplementedException();
        }

        public string GetSqlType(Type type, int? length = null)
        {
            throw new NotImplementedException();
        }

        public string FormatSqlByType(object val)
        {
            throw new NotImplementedException();
        }

        public object ConvertFromSql(object obj)
        {
            throw new NotImplementedException();
        }

        public object ConvertToSql(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
