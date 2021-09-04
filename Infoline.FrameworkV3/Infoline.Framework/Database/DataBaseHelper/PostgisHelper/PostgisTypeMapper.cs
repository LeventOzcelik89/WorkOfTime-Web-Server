using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Database.Postgis
{
    public class PostgisTypeMapper : ITypeMapper
    {
        NetTopologySuite.IO.WKBReader _wkbReader;
        static Dictionary<string, Type> _sqlToCode = new Dictionary<string, Type>();
        static Dictionary<Type, string> _codeToSql = new Dictionary<Type, string>();

        static PostgisTypeMapper()
        {
            _sqlToCode.Add("boolean", typeof(bool));
            _sqlToCode.Add("smallint", typeof(short));
            _sqlToCode.Add("integer", typeof(int));
            _sqlToCode.Add("bigint", typeof(long));
            _sqlToCode.Add("character varying", typeof(string));
            _sqlToCode.Add("uuid", typeof(Guid));
            _sqlToCode.Add("numeric", typeof(double));
            _sqlToCode.Add("timestamp without time zone", typeof(DateTime));
            _sqlToCode.Add("geometry", typeof(IGeometry));

            _codeToSql.Add(typeof(DateTimeOffset), "datetimeoffset");
            _codeToSql.Add(typeof(TimeSpan), "time");
            _codeToSql.Add(typeof(byte[]), "varbinary(max)");

            _codeToSql.Add(typeof(DateTime), "timestamp without time zone");
            _codeToSql.Add(typeof(Guid), "uuid");
            _codeToSql.Add(typeof(string), "character varying");
            _codeToSql.Add(typeof(bool), "boolean");
            _codeToSql.Add(typeof(short), "smallint");
            _codeToSql.Add(typeof(int), "integer");
            _codeToSql.Add(typeof(long), "bigint");
            _codeToSql.Add(typeof(double), "numeric");
            _codeToSql.Add(typeof(IGeometry), "geometry");

            _codeToSql.Add(typeof(decimal), "decimal");
            _codeToSql.Add(typeof(Single), "real");
            _codeToSql.Add(typeof(byte), "tinyint");

        }
        public PostgisTypeMapper()
        {
            _wkbReader = new NetTopologySuite.IO.WKBReader();
        }

        public Type GetType(string sqlType)
        {
            return _sqlToCode[sqlType];
        }
        public string GetSqlType(Type type, int? length = null)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);

            if (typeof(IGeometry).IsAssignableFrom(type))
                type = typeof(IGeometry);

            return _codeToSql[type];
        }
        public string FormatSqlByType(object val)
        {
            if (val == null)
                return "";

            var type = val.GetType();
            if (typeof(string) == type ||
                typeof(Guid) == type)
                return string.Format("'{0}'", val);

            if (typeof(bool) == type)
                return (bool)val ? "'t'" : "'f'";

            if (typeof(int) == type ||
                typeof(decimal) == type ||
                typeof(Single) == type ||
                typeof(byte) == type ||
                typeof(short) == type ||
                typeof(long) == type ||
                typeof(double) == type)
                return val.ToString();

            if (typeof(DateTime) == type)
                return string.Format("'{0}'", ((DateTime)val).ToString("yyyy-MM-dd hh:mm:ssss"));


            return val.ToString();
        }
        public object ConvertFromSql(object obj)
        {
            if (obj == DBNull.Value) return null;
            if (obj.GetType().Name == "SqlGeography" || obj.GetType().Name == "SqlGeometry")
            {
                var val1 = (obj.GetType().GetMethod("STAsBinary").Invoke(obj, null) as SqlBytes).Value;
                IGeometry geo = _wkbReader.Read(val1);
                return geo;
            }
            return obj;
        }
        public object ConvertToSql(object obj)
        {
            if (obj == null) return DBNull.Value;
            if (obj == DBNull.Value) return null;
            if (typeof(IGeometry).IsAssignableFrom(obj.GetType()))
                return ((IGeometry)obj).AsText();
            return obj;
        }
    }
}
