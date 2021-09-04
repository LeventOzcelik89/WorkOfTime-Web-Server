using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_Pages : InfolineTable
    {
        public string Description { get; set;}
        public string Action { get; set;}
        public string Area { get; set;}
        public string Controller { get; set;}
        public string Method { get; set;}
        public bool? AllowEveryone { get; set;}
    }
}
