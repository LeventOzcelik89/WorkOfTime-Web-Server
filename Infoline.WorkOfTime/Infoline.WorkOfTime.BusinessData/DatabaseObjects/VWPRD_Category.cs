using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_Category : InfolineTable
    {
        public string name { get; set;}
        public Guid? pid { get; set;}
        public string fullname { get; set;}
        public int? generation { get; set;}
    }
}
