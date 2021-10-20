using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class UT_Airport : InfolineTable
    {
        public string code { get; set;}
        public string name { get; set;}
        public short? status { get; set;}
    }
}
