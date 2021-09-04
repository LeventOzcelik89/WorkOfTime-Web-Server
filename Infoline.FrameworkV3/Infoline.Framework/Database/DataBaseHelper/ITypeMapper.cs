using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Database
{
    public interface ITypeMapper
    {
        Type GetType(string sqlType);
        string GetSqlType(Type type, int? length = null);
        string FormatSqlByType(object val);
        object ConvertFromSql(object obj);
        object ConvertToSql(object obj);
    }
}
