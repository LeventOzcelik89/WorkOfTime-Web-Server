using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infoline.GIS
{
    [Serializable]
    public class Coordinate
    {
        public double Z { get; set; }
    }
    [Serializable]
    public class GeoCoordinate : Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
    [Serializable]
    public class MapCoordinate : Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }
        //public double Z { get; set; }
    }
}
