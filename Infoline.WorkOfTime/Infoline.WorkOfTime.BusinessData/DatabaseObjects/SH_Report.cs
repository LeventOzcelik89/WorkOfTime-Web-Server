using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_Report : InfolineTable
    {
        public string title { get; set;}
        public int? type { get; set;}
        public string schema { get; set;}
    }
}
