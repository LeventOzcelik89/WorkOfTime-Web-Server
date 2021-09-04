using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Database.Mssql
{
    public class MssqlTypeMapper : ITypeMapper
    {
        NetTopologySuite.IO.WKBReader _wkbReader;
        static Dictionary<string, Type> _sqlToCode = new Dictionary<string, Type>();
        static Dictionary<Type, string> _codeToSql = new Dictionary<Type, string>();

        static MssqlTypeMapper()
        {
            _sqlToCode.Add("bigint", typeof(Int64));
            _sqlToCode.Add("binary", typeof(Byte[]));
            _sqlToCode.Add("bit", typeof(Boolean));
            _sqlToCode.Add("char", typeof(String));
            _sqlToCode.Add("date", typeof(DateTime));
            _sqlToCode.Add("datetime", typeof(DateTime));
            _sqlToCode.Add("datetime2", typeof(DateTime));
            _sqlToCode.Add("datetimeoffset", typeof(DateTimeOffset));
            _sqlToCode.Add("decimal", typeof(Decimal));
            _sqlToCode.Add("float", typeof(Double));
            _sqlToCode.Add("image", typeof(Byte[]));
            _sqlToCode.Add("int", typeof(Int32));
            _sqlToCode.Add("money", typeof(Decimal));
            _sqlToCode.Add("nchar", typeof(String));
            _sqlToCode.Add("ntext", typeof(String));
            _sqlToCode.Add("numeric", typeof(Decimal));
            _sqlToCode.Add("nvarchar", typeof(String));
            _sqlToCode.Add("real", typeof(Single));
            _sqlToCode.Add("rowversion", typeof(Byte[]));
            _sqlToCode.Add("smalldatetime", typeof(DateTime));
            _sqlToCode.Add("smallint", typeof(Int16));
            _sqlToCode.Add("smallmoney", typeof(Decimal));
            _sqlToCode.Add("text", typeof(String));
            _sqlToCode.Add("time", typeof(TimeSpan));
            _sqlToCode.Add("timestamp", typeof(Byte[]));
            _sqlToCode.Add("tinyint", typeof(Byte));
            _sqlToCode.Add("uniqueidentifier", typeof(Guid));
            _sqlToCode.Add("varbinary", typeof(Byte[]));
            _sqlToCode.Add("varchar", typeof(String));
            _sqlToCode.Add("geography", typeof(IGeometry));
            _sqlToCode.Add("geometry", typeof(IGeometry));
            _codeToSql.Add(typeof(DateTimeOffset), "datetimeoffset");
            _codeToSql.Add(typeof(DateTime), "datetime");
            _codeToSql.Add(typeof(TimeSpan), "time");
            _codeToSql.Add(typeof(byte[]), "varbinary(max)");
            _codeToSql.Add(typeof(string), "nvarchar");
            _codeToSql.Add(typeof(decimal), "decimal");
            _codeToSql.Add(typeof(Single), "real");
            _codeToSql.Add(typeof(bool), "bit");
            _codeToSql.Add(typeof(Byte), "tinyint");
            _codeToSql.Add(typeof(short), "smallint");
            _codeToSql.Add(typeof(int), "int");
            _codeToSql.Add(typeof(long), "bigint");
            _codeToSql.Add(typeof(double), "float");
            _codeToSql.Add(typeof(Guid), "uniqueidentifier");
            _codeToSql.Add(typeof(IGeometry), "geography");

        }

        public MssqlTypeMapper()
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

            if (type == typeof(DBNull))
                return "varchar(50)";

            if (type.IsEnum)
                return "varchar(50)";

            var result = _codeToSql[type];
            if (typeof(string) == type)
                result = string.Format("{0}({1})", result, length.HasValue ? length : 255);
            return result;
        }

        public string FormatSqlByType(object val)
        {
            if (val == null)
                return "";

            if (val == DBNull.Value)
                return "null";

            var type = val.GetType();
            if (typeof(string) == type ||
                typeof(Guid) == type)
                return string.Format("'{0}'", val);

            if (typeof(bool) == type)
                return (bool)val ? "1" : "0";

            if (typeof(int) == type ||
                typeof(decimal) == type ||
                typeof(Single) == type ||
                typeof(byte) == type ||
                typeof(short) == type ||
                typeof(long) == type ||
                typeof(double) == type)
                return val.ToString();

            if (type.IsEnum)
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
            if (typeof(IGeometry).IsAssignableFrom(obj.GetType()))
            {
                obj = GeometryValidator.ReorientObject(obj as IGeometry);
                obj = GeometryValidator.MakeValid(obj as IGeometry);
                
                //if ((obj as IGeometry).IsEmpty)
                //    return (obj as IGeometry).AsText();
                //if (obj is IMultiPolygon)
                //{
                //    var geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory();
                //    var polygons = new List<IPolygon>();
                //    for (var i = 0; i < ((IMultiPolygon)obj).Count; i++)
                //    {
                //        var polygon = (IPolygon)((IMultiPolygon)obj)[i];
                //        if (!polygon.Shell.IsCCW)
                //            polygon = (IPolygon)polygon.Reverse();
                //        polygons.Add(polygon);
                //    }
                //    obj = geometryFactory.CreateMultiPolygon(polygons.ToArray());
                //}
                //else if (obj is IPolygon)
                //{
                //    if(!((IPolygon)obj).Shell.IsCCW)
                //    {
                //        obj = ((IGeometry)obj).Reverse();
                //    }
                //}
                return ((IGeometry)obj).AsText();
            }
            return obj;
        }
    }
}
