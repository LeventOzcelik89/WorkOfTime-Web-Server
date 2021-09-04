using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class UT_Currency : InfolineTable
    {
        public string code { get; set;}
        public string name { get; set;}
        public string symbol { get; set;}
        public string subName { get; set;}
        public bool? isLogging { get; set;}
    }
}
