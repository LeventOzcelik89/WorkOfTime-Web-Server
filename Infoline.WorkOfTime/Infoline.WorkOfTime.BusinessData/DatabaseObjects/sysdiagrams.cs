using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class sysdiagrams : InfolineTable
    {
        public string name { get; set;}
        public int principal_id { get; set;}
        public int diagram_id { get; set;}
        public int? version { get; set;}
        public byte [] definition { get; set;}
    }
}
