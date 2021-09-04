using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class UT_LocationConfig : InfolineTable
    {
        public string configName { get; set;}
        public string shiftStart { get; set;}
        public string shiftEnd { get; set;}
        public int? dataSendingCount { get; set;}
        public string workDays { get; set;}
    }
}
