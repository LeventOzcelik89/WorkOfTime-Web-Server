using Infoline.Helper;
using System.ComponentModel;


namespace Infoline.GIS
{
    public enum GeometryType
    {
        [Description("")]
        Null = 0,

        [Description("POINT")]
        Point = 1,

        [Description("LINESTRING")]
        PolyLine = 3,

        [Description("POLYGON")]
        Polygon = 5,
        
        [Description("MULTIPOINT")]
        MultiPoint = 8,

        [Description("POINT")]
        PointZ = 11,

        [Description("LINESTRING")]
        PolyLineZ = 13,

        [Description("POLYGON")]
        PolygonZ = 15,

        [Description("POINT")]
        MultiPointZ = 18,

        [Description("MULTIPOINT")]
        PointM = 21,

        [Description("MULTILINESTRING")]
        PolyLineM = 22,

        [Description("MULTIPOLYGON")]
        PolygonM = 23,

        [Description("MULTIPOINT")]
        MultiPointM = 24,

        [Description("CIRCULARSTRING")]
        CircularString = 25,

        [Description("COMPOUNDCURVE")]
        CompoundCurve = 26,

        [Description("CURVEPOLYGON")]
        CurvePolygon = 27,

        [Description("GEOMETRYCOLLECTION")]
        GeometryCollection = 28,

        
    }



    public class GeometryTypeConverter
    {

        public static string GeometryTypeStringConverter(string item)
        {

            if (item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.CircularString))
            {
                return "CircularString";
            }
            else if (item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.Point) || item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.PointZ) || item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.MultiPointZ))
            {
                return "Point";
            }
            else if (item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.PolyLine) || item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.PolyLineZ))
            {
                return "LineString";
            }
            else if (item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.Polygon) || item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.PolygonZ))
            {
                return "Polygon";
            }
            else if (item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.PolygonM))
            {
                return "MultiPolygon";
            }

            else if (item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.MultiPoint) || item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.PointM) || item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.MultiPointM))
            {
                return "MultiPoint";
            }
            else if (item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.PolyLineM) )
            {
                return "MultiLineString";
            }
           
            else if (item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.CompoundCurve))
            {
                return "CompoundCurve";
            }
            else if (item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.CurvePolygon))
            {
                return "CurvePolygon";
            }
            else if (item == EnumsProperties.GetDescriptionFromEnumValue(GeometryType.GeometryCollection))
            {
                return "GeometryCollection";
            }
            else {
                return string.Empty;
            }
            
        }

        public static GeometryType GetSTGeometryTypeToGeometryType(string sTGeometryType)
        {
            if (sTGeometryType.ToUpper() == "POINT" || sTGeometryType.ToLower() == "point")
                return GeometryType.Point;

            else if (sTGeometryType.ToUpper() == "LINESTRING" || sTGeometryType.ToLower() == "linestring")
                return GeometryType.PolyLine;

            else if (sTGeometryType.ToUpper() == "POLYGON")
                return GeometryType.Polygon;

            else if (sTGeometryType.ToUpper() == "MULTIPOINT" || sTGeometryType.ToLower() == "multipoint")
                return GeometryType.MultiPoint;

            else if (sTGeometryType.ToUpper() == "MULTILINESTRING" || sTGeometryType.ToLower() == "multilinestring")
                return GeometryType.PolyLineM;

            else if (sTGeometryType.ToUpper() == "MULTIPOLYGON" || sTGeometryType.ToLower() == "multipolygon")
                return GeometryType.PolygonM;

            else if (sTGeometryType.ToUpper() == "CIRCULARSTRING" || sTGeometryType.ToLower() == "circularstring")
                return GeometryType.CircularString;

            else if (sTGeometryType.ToUpper() == "COMPOUNDCURVE" || sTGeometryType.ToLower() == "compoundcurve")
                return GeometryType.CompoundCurve;

            else if (sTGeometryType.ToUpper() == "CURVEPOLYGON" || sTGeometryType.ToLower() == "curvepolygon")
                return GeometryType.CurvePolygon;

            else if (sTGeometryType.ToUpper() == "GEOMETRYCOLLECTION" || sTGeometryType.ToLower() == "geometrycollection")
                return GeometryType.GeometryCollection;
            else
                return GeometryType.Null;
        }
    }
}
