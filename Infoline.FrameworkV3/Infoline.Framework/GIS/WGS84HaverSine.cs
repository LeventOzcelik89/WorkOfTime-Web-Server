using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infoline.GIS
{
    /// <summary>
    /// The distance type to return the results in.
    /// </summary>
    public enum DistanceType { Miles, Kilometers };
    /// <summary>
    public static class Haversine
    {
        static double R = 6378.137 * 2;
        static double PI180 = Math.PI / 180;
        public static double Distance(WGS84 pos1, WGS84 pos2)
        {
            double dLat = (PI180) * (pos2.Latitude - pos1.Latitude);
            double dLon = (PI180) * (pos2.Longitude - pos1.Longitude);
            var sinlat = Math.Sin(dLat / 2);
            var sinlon = Math.Sin(dLon / 2);
            double a = sinlat * sinlat +
                Math.Cos((PI180) * (pos1.Latitude)) * Math.Cos((PI180) * (pos2.Latitude)) *
                sinlon * sinlon;
            return Math.Asin(Math.Min(1, Math.Sqrt(a))) * R;
        }

    }
}
