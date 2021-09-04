
using System;
using System.Net;


namespace Infoline.GIS
{
    [Serializable]
    public class WGS84 : GeoCoordinate
    {
        public UTM ToUtM()
        {


















            return GeoUTMConverter.ToUTM(this);
        }
        public UTM ToUtM(double zone)
        {
            return GeoUTMConverter.ToUTM(this, zone);
        }
        public override string ToString()
        {
            return string.Format("La:{0} Lo:{1} Z:{2}", Latitude, Longitude, Altitude);
        }

        public double Distance(WGS84 geo)
        {
            return Haversine.Distance(geo, this);
        }
    }
    public class SphericalMercator : Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }
        public override string ToString()
        {
            return string.Format("X:{0} Y:{1} :Z:{2}", X, Y,Z);
        }
    }
    public static class SphericalMercatorConverter
    {
        private readonly static double radius = 6378137;
        private static double D2R = Math.PI / 180;
        private static double HALF_PI = Math.PI / 2;


        public static SphericalMercator ToSphericalMercator(this WGS84 value)
        {
            return new SphericalMercator { X = radius * (D2R * value.Longitude), Y = radius * Math.Log(Math.Tan(Math.PI * 0.25 + (D2R * value.Latitude) * 0.5)) };
        }

        public static WGS84 ToLonLat(this SphericalMercator value)
        {
            return new WGS84() { Longitude = (value.X / radius / D2R), Latitude = (HALF_PI - 2 * Math.Atan((Math.Exp(-value.Y / (radius))))) / D2R };
        }
    }



}