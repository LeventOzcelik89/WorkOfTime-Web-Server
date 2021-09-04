using EGIS.ShapeFileLib;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.GIS
{
    public struct ShapeCheck
    {
        public static string CheckPolygonString(string LineString, ShapeType shapeType)
        {
            LineString = LineString.Trim();

            string result = null;

            switch (shapeType)
            {
                case ShapeType.Point:
                    result = string.Format("POINT ( {0} )", LineString);
                    break;
                case ShapeType.PolyLine:
                    result = string.Format("LINESTRING ( {0} )", LineString);
                    break;
                case ShapeType.Polygon:
                    result = string.Format("POLYGON (( {0} ))", LineString);
                    break;
                case ShapeType.MultiPoint:
                    result = string.Format("POLYGON (( {0} ))", LineString);
                    break;
                case ShapeType.PointZ:
                    result = string.Format("POINT ( {0} )", LineString);
                    break;
                case ShapeType.PolyLineZ:
                    result = string.Format("LINESTRING ( {0} )", LineString);
                    break;
                case ShapeType.PolygonZ:
                    result = string.Format("POLYGON (( {0} ))", LineString);
                    break;
                case ShapeType.MultiPointZ:
                    result = string.Format("POLYGON (( {0} ))", LineString);
                    break;
                case ShapeType.PointM:
                    result = string.Format("POINT ( {0} )", LineString);
                    break;
                case ShapeType.PolyLineM:
                    result = string.Format("LINESTRING ( {0} )", LineString);
                    break;
                default:
                    break;
            }

            return result;

        }

        public static SqlGeography ConvertStringToSqlGeography(string stringLine, ShapeType shapeType)
        {
            try
            {
                stringLine = CheckPolygonString(stringLine, shapeType);
                SqlGeography sqlGeog = new SqlGeography();
                sqlGeog = SqlGeography.STGeomFromText(new System.Data.SqlTypes.SqlChars(stringLine), 4326);
                if (sqlGeog.STIsValid().IsFalse)
                    return null;

                if ((shapeType == ShapeType.Polygon || shapeType == ShapeType.PolygonZ) && sqlGeog.EnvelopeAngle() >= 90)
                    sqlGeog = sqlGeog.ReorientObject();
                return sqlGeog;
            }
            catch (Exception ex)
            {
                var d = ex;
                return null;
            }
        }

        public static object CheckDbfFieldTypeValue(string value, DbfFieldType fieldType)
        {
            var _value = value.Trim();
            if (_value != null && _value != "")
            {
                if (ShapeConverter.ConvertShapeTypeToCSharpType(fieldType) == typeof(DateTime))
                {
                    return ShapeConverter.ConvertShapeFileDateToDateTime(_value);
                }
                else
                    return _value;
            }
            else
                return _value;
        }
    }
}
