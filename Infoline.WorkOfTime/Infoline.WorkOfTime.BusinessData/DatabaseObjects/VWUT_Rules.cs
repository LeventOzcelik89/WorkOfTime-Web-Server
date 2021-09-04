using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWUT_Rules : InfolineTable
    {
        public string name { get; set;}
        public short? type { get; set;}
        public bool? isDefault { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string type_Title { get; set;}
    }
}
