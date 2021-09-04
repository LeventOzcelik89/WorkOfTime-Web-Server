using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWUT_Location : InfolineTable
    {
        public Guid? pid { get; set;}
        public int? type { get; set;}
        public string code { get; set;}
        public string name { get; set;}
        public IGeometry  polygon { get; set;}
        public string pid_Title { get; set;}
        public string type_Title { get; set;}
    }
}
