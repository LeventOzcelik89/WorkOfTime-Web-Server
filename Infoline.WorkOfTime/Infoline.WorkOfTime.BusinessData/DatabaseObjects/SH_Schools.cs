using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_Schools : InfolineTable
    {
        public string SchoolName { get; set;}
        public int? Type { get; set;}
    }
}
