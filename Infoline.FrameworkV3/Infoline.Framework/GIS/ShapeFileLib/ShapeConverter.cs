using EGIS.ShapeFileLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.GIS
{
    public struct ShapeConverter
    {
        public static GeometryType ConvertShapeTypeToGeometryType(ShapeType shapeType)
        {
            if (shapeType == ShapeType.Point)
                return GeometryType.Point;

            else if (shapeType == ShapeType.PolyLine)
                return GeometryType.PolyLine;

            else if (shapeType == ShapeType.Polygon)
                return GeometryType.Polygon;

            else if (shapeType == ShapeType.MultiPoint)
                return GeometryType.MultiPoint;

            else if (shapeType == ShapeType.MultiPointZ)
                return GeometryType.MultiPointZ;

            else if (shapeType == ShapeType.PointZ)
                return GeometryType.PointZ;

            else if (shapeType == ShapeType.PolyLineZ)
                return GeometryType.PolyLineZ;

            else if (shapeType == ShapeType.PointM)
                return GeometryType.PointM;

            else if (shapeType == ShapeType.PolygonZ)
                return GeometryType.PolygonZ;

            else if (shapeType == ShapeType.PolyLineM)
                return GeometryType.PolyLineM;

            else
                return GeometryType.Null;

        }

        public static ShapeType ConvertGeometryTypeToShapeType(GeometryType geometryType)
        {
            if (geometryType == GeometryType.MultiPoint)
                return ShapeType.MultiPoint;

            else if (geometryType == GeometryType.MultiPointZ)
                return ShapeType.MultiPointZ;

            else if (geometryType == GeometryType.Point)
                return ShapeType.Point;

            else if (geometryType == GeometryType.PointM)
                return ShapeType.PointM;

            else if (geometryType == GeometryType.PointZ)
                return ShapeType.PointZ;

            else if (geometryType == GeometryType.Polygon)
                return ShapeType.Polygon;

            else if (geometryType == GeometryType.PolygonZ)
                return ShapeType.PolygonZ;

            else if (geometryType == GeometryType.PolyLine)
                return ShapeType.PolyLine;

            else if (geometryType == GeometryType.PolyLineM)
                return ShapeType.PolyLine;

            else if (geometryType == GeometryType.PolyLineZ)
                return ShapeType.PolyLine;
            else
                return ShapeType.NullShape;

        }
        
        public static Type ConvertShapeTypeToCSharpType(DbfFieldType dbfFieldType)
        {
            if (dbfFieldType == DbfFieldType.Binary)
                return typeof(bool);
            else if (dbfFieldType == DbfFieldType.Character)
                return typeof(string);
            else if (dbfFieldType == DbfFieldType.Date)
                return typeof(DateTime);
            else if (dbfFieldType == DbfFieldType.FloatingPoint)
                return typeof(float);
            else if (dbfFieldType == DbfFieldType.General)
                return typeof(string);
            else if (dbfFieldType == DbfFieldType.Logical)
                return typeof(byte);
            else if (dbfFieldType == DbfFieldType.Number)
                return typeof(double);
            else if (dbfFieldType == DbfFieldType.None)
                return typeof(string);
            else
                return typeof(string);
        }

        public static string ConvertShapeFileDateToDateTime(string item)
        {
            try
            {
                return new DateTime(
                    Convert.ToInt32(item.Substring(0, 4)),
                    Convert.ToInt32(item.Substring(4, 2)),
                    Convert.ToInt32(item.Substring(6, 2))
                    ).ToString();
            }
            catch
            {
                return null;
            }
        }

        public static List<EGIS.ShapeFileLib.PointD[]> GetPointDList(GeometryObject geometryObject)
        {
            var pdk = new List<EGIS.ShapeFileLib.PointD[]>();

            foreach (var item in geometryObject.TableRows)
            {
                if (item.Polygon != null)
                {
                    var pointDlist = SqlGeographyConverter.SQLGeographyToWGS84(item.Polygon);
                    pdk.Add(ConvertWGS84ToPointD(pointDlist));
                }
            }
            return pdk;
        }

        public static EGIS.ShapeFileLib.PointD[] ConvertWGS84ToPointD(WGS84[] pointList)
        {
            var pointdList = new List<EGIS.ShapeFileLib.PointD>();
            foreach (var item in pointList)
            {
                pointdList.Add(new EGIS.ShapeFileLib.PointD { X = item.Longitude, Y = item.Latitude });
            }
            return pointdList.ToArray();
        }

        public static DbfFieldProperties ConvertSharpTypeToDbfFieldType(Type sharpType, int length)
        {
            if (sharpType == typeof(int) || sharpType == typeof(Int16) || sharpType == typeof(Int32) || sharpType == typeof(Int64))
                return new DbfFieldProperties
                {
                    FieldType = DbfFieldType.Number,
                    DecimalCount = 3,
                    FieldLength = 10
                };

            else if (sharpType == typeof(double))
                return new DbfFieldProperties
                {
                    FieldType = DbfFieldType.Number,
                    DecimalCount = 3,
                    FieldLength = 10
                };
            else if (sharpType == typeof(float))
                return new DbfFieldProperties
                {
                    FieldType = DbfFieldType.Number,
                    DecimalCount = 3,
                    FieldLength = 10
                };
            else if (sharpType == typeof(double))
                return new DbfFieldProperties
                {
                    FieldType = DbfFieldType.Number,
                    DecimalCount = 3,
                    FieldLength = 10
                };
            else if (sharpType == typeof(bool))
                return new DbfFieldProperties
                {
                    FieldType = DbfFieldType.Binary,
                    DecimalCount = 0,
                    FieldLength = 10
                };
            else if (sharpType == typeof(DateTime))
                return new DbfFieldProperties
                {
                    FieldType = DbfFieldType.Date,
                    DecimalCount = 0,
                    FieldLength = 10
                };
            else if (sharpType == typeof(string) || sharpType == typeof(String))
                return new DbfFieldProperties
                {
                    FieldType = DbfFieldType.Character,
                    DecimalCount = 0,
                    FieldLength = 30
                };
            else
                return new DbfFieldProperties
                {
                    FieldType = DbfFieldType.Character,
                    DecimalCount = 1,
                    FieldLength = 30
                };
        }
    }
}
