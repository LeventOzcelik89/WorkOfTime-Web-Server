using System;
using GeoAPI.Geometries;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Database
{
    public static class QueryExtensions
    {
        public static bool In(this object item, Array array)
        {
            foreach (var a in array)
                if (a.Equals(item)) return false;
            return true;
        }
        

        #region Abs
        public static decimal Abs(this decimal value) { return Math.Abs(value); }
        public static double Abs(this double value) { return Math.Abs(value); }
        public static float Abs(this float value) { return Math.Abs(value); }
        public static int Abs(this int value) { return Math.Abs(value); }
        public static long Abs(this long value) { return Math.Abs(value); }
        public static sbyte Abs(this sbyte value) { return Math.Abs(value); }
        public static short Abs(this short value) { return Math.Abs(value); }
        public static decimal? Abs(this decimal? value) { if (value.HasValue) return Math.Abs(value.Value); return null; }
        public static double? Abs(this double? value) { if (value.HasValue) return Math.Abs(value.Value); return null; }
        public static float? Abs(this float? value) { if (value.HasValue) return Math.Abs(value.Value); return null; }
        public static int? Abs(this int? value) { if (value.HasValue) return Math.Abs(value.Value); return null; }
        public static long? Abs(this long? value) { if (value.HasValue) return Math.Abs(value.Value); return null; }
        public static sbyte? Abs(this sbyte? value) { if (value.HasValue) return Math.Abs(value.Value); return null; }
        public static short? Abs(this short? value) { if (value.HasValue) return Math.Abs(value.Value); return null; }
        #endregion
        #region Ceiling
        public static decimal Ceiling(this decimal value) { return Math.Ceiling(value); }
        public static double Ceiling(this double value) { return Math.Ceiling(value); }
        public static double Ceiling(this float value) { return Math.Ceiling(value); }
        public static decimal? Ceiling(this decimal? value) { if (value.HasValue) return Math.Ceiling(value.Value); return null; }
        public static double? Ceiling(this double? value) { if (value.HasValue) return Math.Ceiling(value.Value); return null; }
        public static double? Ceiling(this float? value) { if (value.HasValue) return Math.Ceiling(value.Value); return null; }
        #endregion
        #region Floor
        public static decimal Floor(this decimal value) { return Math.Floor(value); }
        public static double Floor(this double value) { return Math.Floor(value); }
        public static double Floor(this float value) { return Math.Floor(value); }
        public static decimal? Floor(this decimal? value) { if (value.HasValue) return Math.Floor(value.Value); return null; }
        public static double? Floor(this double? value) { if (value.HasValue) return Math.Floor(value.Value); return null; }
        public static double? Floor(this float? value) { if (value.HasValue) return Math.Floor(value.Value); return null; }
        #endregion
        #region Acos
        public static double Acos(this double value) { return Math.Acos(value); }
        public static double Acos(this float value) { return Math.Acos(value); }
        public static double Acos(this int value) { return Math.Acos(value); }
        public static double Acos(this long value) { return Math.Acos(value); }
        public static double Acos(this sbyte value) { return Math.Acos(value); }
        public static double Acos(this short value) { return Math.Acos(value); }
        public static double? Acos(this double? value) { if (value.HasValue) return Math.Acos(value.Value); return null; }
        public static double? Acos(this float? value) { if (value.HasValue) return Math.Acos(value.Value); return null; }
        public static double? Acos(this int? value) { if (value.HasValue) return Math.Acos(value.Value); return null; }
        public static double? Acos(this long? value) { if (value.HasValue) return Math.Acos(value.Value); return null; }
        public static double? Acos(this sbyte? value) { if (value.HasValue) return Math.Acos(value.Value); return null; }
        public static double? Acos(this short? value) { if (value.HasValue) return Math.Acos(value.Value); return null; }
        #endregion
        #region Asin
        public static double Asin(this double value) { return Math.Asin(value); }
        public static double Asin(this float value) { return Math.Asin(value); }
        public static double Asin(this int value) { return Math.Asin(value); }
        public static double Asin(this long value) { return Math.Asin(value); }
        public static double Asin(this sbyte value) { return Math.Asin(value); }
        public static double Asin(this short value) { return Math.Asin(value); }
        public static double? Asin(this double? value) { if (value.HasValue) return Math.Asin(value.Value); return null; }
        public static double? Asin(this float? value) { if (value.HasValue) return Math.Asin(value.Value); return null; }
        public static double? Asin(this int? value) { if (value.HasValue) return Math.Asin(value.Value); return null; }
        public static double? Asin(this long? value) { if (value.HasValue) return Math.Asin(value.Value); return null; }
        public static double? Asin(this sbyte? value) { if (value.HasValue) return Math.Asin(value.Value); return null; }
        public static double? Asin(this short? value) { if (value.HasValue) return Math.Asin(value.Value); return null; }
        #endregion
        #region Atan
        public static double Atan(this double value) { return Math.Atan(value); }
        public static double Atan(this float value) { return Math.Atan(value); }
        public static double Atan(this int value) { return Math.Atan(value); }
        public static double Atan(this long value) { return Math.Atan(value); }
        public static double Atan(this sbyte value) { return Math.Atan(value); }
        public static double Atan(this short value) { return Math.Atan(value); }
        public static double? Atan(this double? value) { if (value.HasValue) return Math.Atan(value.Value); return null; }
        public static double? Atan(this float? value) { if (value.HasValue) return Math.Atan(value.Value); return null; }
        public static double? Atan(this int? value) { if (value.HasValue) return Math.Atan(value.Value); return null; }
        public static double? Atan(this long? value) { if (value.HasValue) return Math.Atan(value.Value); return null; }
        public static double? Atan(this sbyte? value) { if (value.HasValue) return Math.Atan(value.Value); return null; }
        public static double? Atan(this short? value) { if (value.HasValue) return Math.Atan(value.Value); return null; }
        #endregion
        #region Atan2

        #endregion
        #region Cos
        public static double Cos(this double value) { return Math.Cos(value); }
        public static double Cos(this float value) { return Math.Cos(value); }
        public static double Cos(this int value) { return Math.Cos(value); }
        public static double Cos(this long value) { return Math.Cos(value); }
        public static double Cos(this sbyte value) { return Math.Cos(value); }
        public static double Cos(this short value) { return Math.Cos(value); }
        public static double? Cos(this double? value) { if (value.HasValue) return Math.Cos(value.Value); return null; }
        public static double? Cos(this float? value) { if (value.HasValue) return Math.Cos(value.Value); return null; }
        public static double? Cos(this int? value) { if (value.HasValue) return Math.Cos(value.Value); return null; }
        public static double? Cos(this long? value) { if (value.HasValue) return Math.Cos(value.Value); return null; }
        public static double? Cos(this sbyte? value) { if (value.HasValue) return Math.Cos(value.Value); return null; }
        public static double? Cos(this short? value) { if (value.HasValue) return Math.Cos(value.Value); return null; }
        #endregion
        #region Exp
        public static double Exp(this double value) { return Math.Exp(value); }
        public static double Exp(this float value) { return Math.Exp(value); }
        public static double Exp(this int value) { return Math.Exp(value); }
        public static double Exp(this long value) { return Math.Exp(value); }
        public static double Exp(this sbyte value) { return Math.Exp(value); }
        public static double Exp(this short value) { return Math.Exp(value); }
        public static double? Exp(this double? value) { if (value.HasValue) return Math.Exp(value.Value); return null; }
        public static double? Exp(this float? value) { if (value.HasValue) return Math.Exp(value.Value); return null; }
        public static double? Exp(this int? value) { if (value.HasValue) return Math.Exp(value.Value); return null; }
        public static double? Exp(this long? value) { if (value.HasValue) return Math.Exp(value.Value); return null; }
        public static double? Exp(this sbyte? value) { if (value.HasValue) return Math.Exp(value.Value); return null; }
        public static double? Exp(this short? value) { if (value.HasValue) return Math.Exp(value.Value); return null; }
        #endregion
        #region Sqrt
        public static double Sqrt(this double value) { return Math.Sqrt(value); }
        public static double Sqrt(this float value) { return Math.Sqrt(value); }
        public static double Sqrt(this int value) { return Math.Sqrt(value); }
        public static double Sqrt(this long value) { return Math.Sqrt(value); }
        public static double Sqrt(this sbyte value) { return Math.Sqrt(value); }
        public static double Sqrt(this short value) { return Math.Sqrt(value); }
        public static double? Sqrt(this double? value) { if (value.HasValue) return Math.Sqrt(value.Value); return null; }
        public static double? Sqrt(this float? value) { if (value.HasValue) return Math.Sqrt(value.Value); return null; }
        public static double? Sqrt(this int? value) { if (value.HasValue) return Math.Sqrt(value.Value); return null; }
        public static double? Sqrt(this long? value) { if (value.HasValue) return Math.Sqrt(value.Value); return null; }
        public static double? Sqrt(this sbyte? value) { if (value.HasValue) return Math.Sqrt(value.Value); return null; }
        public static double? Sqrt(this short? value) { if (value.HasValue) return Math.Sqrt(value.Value); return null; }
        #endregion
        #region Square
        public static double Square(this double value) { return (value * value); }
        public static double Square(this float value) { return (value * value); }
        public static double Square(this int value) { return (value * value); }
        public static double Square(this long value) { return (value * value); }
        public static double Square(this sbyte value) { return (value * value); }
        public static double Square(this short value) { return (value * value); }
        public static double? Square(this double? value) { if (value.HasValue) return (value.Value * value.Value); return null; }
        public static double? Square(this float? value) { if (value.HasValue) return (value.Value * value.Value); return null; }
        public static double? Square(this int? value) { if (value.HasValue) return (value.Value * value.Value); return null; }
        public static double? Square(this long? value) { if (value.HasValue) return (value.Value * value.Value); return null; }
        public static double? Square(this sbyte? value) { if (value.HasValue) return (value.Value * value.Value); return null; }
        public static double? Square(this short? value) { if (value.HasValue) return (value.Value * value.Value); return null; }
        #endregion
        #region Sin
        public static double Sin(this double value) { return Math.Sin(value); }
        public static double Sin(this float value) { return Math.Sin(value); }
        public static double Sin(this int value) { return Math.Sin(value); }
        public static double Sin(this long value) { return Math.Sin(value); }
        public static double Sin(this sbyte value) { return Math.Sin(value); }
        public static double Sin(this short value) { return Math.Sin(value); }
        public static double? Sin(this double? value) { if (value.HasValue) return Math.Sin(value.Value); return null; }
        public static double? Sin(this float? value) { if (value.HasValue) return Math.Sin(value.Value); return null; }
        public static double? Sin(this int? value) { if (value.HasValue) return Math.Sin(value.Value); return null; }
        public static double? Sin(this long? value) { if (value.HasValue) return Math.Sin(value.Value); return null; }
        public static double? Sin(this sbyte? value) { if (value.HasValue) return Math.Sin(value.Value); return null; }
        public static double? Sin(this short? value) { if (value.HasValue) return Math.Sin(value.Value); return null; }
        #endregion
        #region Sign
        public static int Sign(this decimal value) { return Math.Sign(value); }
        public static int Sign(this double value) { return Math.Sign(value); }
        public static int Sign(this float value) { return Math.Sign(value); }
        public static int Sign(this int value) { return Math.Sign(value); }
        public static int Sign(this long value) { return Math.Sign(value); }
        public static int Sign(this sbyte value) { return Math.Sign(value); }
        public static int Sign(this short value) { return Math.Sign(value); }
        public static int? Sign(this decimal? value) { if (value.HasValue) return Math.Sign(value.Value); return null; }
        public static int? Sign(this double? value) { if (value.HasValue) return Math.Sign(value.Value); return null; }
        public static int? Sign(this float? value) { if (value.HasValue) return Math.Sign(value.Value); return null; }
        public static int? Sign(this int? value) { if (value.HasValue) return Math.Sign(value.Value); return null; }
        public static int? Sign(this long? value) { if (value.HasValue) return Math.Sign(value.Value); return null; }
        public static int? Sign(this sbyte? value) { if (value.HasValue) return Math.Sign(value.Value); return null; }
        public static int? Sign(this short? value) { if (value.HasValue) return Math.Sign(value.Value); return null; }
        #endregion
        #region Tan
        public static double Tan(this double value) { return Math.Tan(value); }
        public static double Tan(this float value) { return Math.Tan(value); }
        public static double Tan(this int value) { return Math.Tan(value); }
        public static double Tan(this long value) { return Math.Tan(value); }
        public static double Tan(this sbyte value) { return Math.Tan(value); }
        public static double Tan(this short value) { return Math.Tan(value); }
        public static double? Tan(this double? value) { if (value.HasValue) return Math.Tan(value.Value); return null; }
        public static double? Tan(this float? value) { if (value.HasValue) return Math.Tan(value.Value); return null; }
        public static double? Tan(this int? value) { if (value.HasValue) return Math.Tan(value.Value); return null; }
        public static double? Tan(this long? value) { if (value.HasValue) return Math.Tan(value.Value); return null; }
        public static double? Tan(this sbyte? value) { if (value.HasValue) return Math.Tan(value.Value); return null; }
        public static double? Tan(this short? value) { if (value.HasValue) return Math.Tan(value.Value); return null; }
        #endregion
        #region Radians
        public static double Radians(this double value) { return (0.0174532924f * value); }
        public static double Radians(this float value) { return (0.0174532924f * value); }
        public static double Radians(this int value) { return (0.0174532924f * value); }
        public static double Radians(this long value) { return (0.0174532924f * value); }
        public static double Radians(this sbyte value) { return (0.0174532924f * value); }
        public static double Radians(this short value) { return (0.0174532924f * value); }
        public static double? Radians(this double? value) { if (value.HasValue) return (0.0174532924f * value.Value); return null; }
        public static double? Radians(this float? value) { if (value.HasValue) return (0.0174532924f * value.Value); return null; }
        public static double? Radians(this int? value) { if (value.HasValue) return (0.0174532924f * value.Value); return null; }
        public static double? Radians(this long? value) { if (value.HasValue) return (0.0174532924f * value.Value); return null; }
        public static double? Radians(this sbyte? value) { if (value.HasValue) return (0.0174532924f * value.Value); return null; }
        public static double? Radians(this short? value) { if (value.HasValue) return (0.0174532924f * value.Value); return null; }
        #endregion
        #region Degrees
        public static double Degrees(this double value) { return (57.29578f * value); }
        public static double Degrees(this float value) { return (57.29578f * value); }
        public static double Degrees(this int value) { return (57.29578f * value); }
        public static double Degrees(this long value) { return (57.29578f * value); }
        public static double Degrees(this sbyte value) { return (57.29578f * value); }
        public static double Degrees(this short value) { return (57.29578f * value); }
        public static double? Degrees(this double? value) { if (value.HasValue) return (57.29578f * value.Value); return null; }
        public static double? Degrees(this float? value) { if (value.HasValue) return (57.29578f * value.Value); return null; }
        public static double? Degrees(this int? value) { if (value.HasValue) return (57.29578f * value.Value); return null; }
        public static double? Degrees(this long? value) { if (value.HasValue) return (57.29578f * value.Value); return null; }
        public static double? Degrees(this sbyte? value) { if (value.HasValue) return (57.29578f * value.Value); return null; }
        public static double? Degrees(this short? value) { if (value.HasValue) return (57.29578f * value.Value); return null; }
        #endregion
        public static IGeometry STGeomFromText(this string text, int srid)
        {
            var wktReader = new NetTopologySuite.IO.WKTReader();
            var val = wktReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STPointFromText(this string text, int srid)
        {
            var wktReader = new NetTopologySuite.IO.WKTReader();
            var val = wktReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STLineFromText(this string text, int srid)
        {
            var wktReader = new NetTopologySuite.IO.WKTReader();
            var val = wktReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STPolyFromText(this string text, int srid)
        {
            var wktReader = new NetTopologySuite.IO.WKTReader();
            var val = wktReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STMPointFromText(this string text, int srid)
        {
            var wktReader = new NetTopologySuite.IO.WKTReader();
            var val = wktReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STMLineFromText(this string text, int srid)
        {
            var wktReader = new NetTopologySuite.IO.WKTReader();
            var val = wktReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STMPolyFromText(this string text, int srid)
        {
            var wktReader = new NetTopologySuite.IO.WKTReader();
            var val = wktReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STGeomCollFromText(this string text, int srid)
        {
            var wktReader = new NetTopologySuite.IO.WKTReader();
            var val = wktReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STGeomFromWKB(this byte[] text, int srid)
        {
            var wkbReader = new NetTopologySuite.IO.WKBReader();
            var val = wkbReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STPointFromWKB(this byte[] text, int srid)
        {
            var wkbReader = new NetTopologySuite.IO.WKBReader();
            var val = wkbReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STLineFromWKB(this byte[] text, int srid)
        {
            var wkbReader = new NetTopologySuite.IO.WKBReader();
            var val = wkbReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STPolyFromWKB(this byte[] text, int srid)
        {
            var wkbReader = new NetTopologySuite.IO.WKBReader();
            var val = wkbReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STMPointFromWKB(this byte[] text, int srid)
        {
            var wkbReader = new NetTopologySuite.IO.WKBReader();
            var val = wkbReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STMLineFromWKB(this byte[] text, int srid)
        {
            var wkbReader = new NetTopologySuite.IO.WKBReader();
            var val = wkbReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STMPolyFromWKB(this byte[] text, int srid)
        {
            var wkbReader = new NetTopologySuite.IO.WKBReader();
            var val = wkbReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry STGeomCollFromWKB(this byte[] text, int srid)
        {
            var wkbReader = new NetTopologySuite.IO.WKBReader();
            var val = wkbReader.Read(text);
            val.SRID = srid;
            return val;
        }
        public static IGeometry GeomFromGML(this string text, int srid)
        {
            var wktReader = new NetTopologySuite.IO.GML2.GMLReader();
            var val = wktReader.Read(text);
            val.SRID = srid;
            return val;
        }
    }
}
