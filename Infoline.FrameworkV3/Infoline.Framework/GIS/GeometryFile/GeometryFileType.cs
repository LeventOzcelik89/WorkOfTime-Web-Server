using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.GIS
{
    public enum GeometryFileType
    {
        [Description("")]
        Null = 0,
        [Description("Shape")]
        SHAPE = 1,
        [Description("Kml")]
        KML = 2,
        [Description("GeoJson")]
        GEOJSON = 3,
        [Description("TopoJson")]
        TOPOJSON = 4,
        [Description("MsSQL")]
        MSSQL = 100
    }
}
