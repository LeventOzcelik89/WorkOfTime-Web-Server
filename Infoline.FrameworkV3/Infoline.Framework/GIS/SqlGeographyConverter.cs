using GeoAPI.Geometries;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infoline.GIS
{
    public struct SqlGeographyConverter
    {
        public static string WGS84ListToString(WGS84[] WGS84List)
        {
            var stringBuilder = new StringBuilder();
            foreach (var item in WGS84List)
            {
                var t = item.ToString();
                stringBuilder.Append(
                                    item.Longitude.ToString().Replace(",", ".") + "," +
                                    item.Latitude.ToString().Replace(",", ".") + "," +
                                    item.Altitude.ToString().Replace(",", ".") + " "
                                    );
            }
            return stringBuilder.ToString();
        }

        public static WGS84[] SQLGeographyToWGS84(IGeometry item)
        {
            // TODO: TEST ET [FM]
            var WGS84List = new List<WGS84>();
            if (item.NumPoints== 1)
            {
                WGS84List.Add(new WGS84
                {
                    Latitude = item.Coordinates[0].Y,
                    Longitude = item.Coordinates[0].X
                    //Altitude = item.Z.Value
                });
            }
            else
            {
                for (int i = 1; i < item.NumPoints; i++)
                {
                    var sqlPoint = item.Coordinates[i];
                    WGS84List.Add(new WGS84
                    {
                        Latitude = sqlPoint.Y,
                        Longitude = sqlPoint.X
                        //Altitude = sqlPoint.Z.Value
                    });
                }
            }
            return WGS84List.ToArray();
        }

        public static IGeometry StringToSqlGeography(string item, int? srid = 4326)
        {
            if (String.IsNullOrEmpty(item))
                return null;
            return CheckStringGeography(item, Convert.ToInt32(srid));
        }

        private static string CheckForPolygon(string item)
        {
            var itemStr = item.Replace("POLYGON ((", "").Replace("))", "").Trim();
            var itemArr = itemStr.Split(',').ToList();

            if (itemArr.FirstOrDefault().Trim() != itemArr.LastOrDefault().Trim())
            {
                return "POLYGON ((" + itemStr + ", " + itemArr.FirstOrDefault().Trim() + "))";
            }
            else
            {
                return "POLYGON ((" + itemStr + "))";
            }

        }

        public static IGeometry GetValidatedGeometryFromStr(string item, int srid = 4326)
        {
            // TODO: CHECK
            item = CheckForPolygon(item);
            var reader = new WKTReader();
            var sqlGeog = reader.Read(item);
            //var sqlGeog = new SqlGeography();
            //try
            //{
            //    sqlGeog = SqlGeography.STGeomFromText(new System.Data.SqlTypes.SqlChars(item), srid);
            //}
            //catch (Exception)
            //{
            //    return null;
            //}

            //if (!sqlGeog.STIsValid().IsTrue)
            //{
            //    return null;
            //}

            //if (sqlGeog.EnvelopeAngle() >= 90)
            //{
            //    sqlGeog = sqlGeog.ReorientObject();
            //}
            return sqlGeog;
        }



        public static IGeometry CheckSqlGeography(IGeometry item)
        {

            if (item == null || item.IsEmpty)
            {
                return null;
            }
            //   TODO: FIXME
            //else if (item.IsValid || item.EnvelopeAngle() >= 90)
            //{
            //    return item.MakeValid().ReorientObject().MakeValid();
            //}

            return item;

        }

        private static IGeometry CheckStringGeography(string item, int srid)
        {
            try
            {

                var reader = new WKTReader();
                var sqlGeog = reader.Read(item);
                
                if (sqlGeog.IsEmpty) { return null; }
                // TODO : FIXME 
                //if (sqlGeog.IsValid)
                //{
                //    try
                //    {
                //        sqlGeog.Normalize();
                //    }
                //    catch
                //    {
                //        return null;
                //    }
                //}

                //try
                //{
                //    //if (sqlGeog.EnvelopeAngle() >= 90)
                //    //    sqlGeog = sqlGeog.ReorientObject();
                //}
                //catch (Exception) { }

                return sqlGeog;
            }
            catch (Exception ex)
            {
                var r = ex;
                return null;
            }
        }

    }
}
