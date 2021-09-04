using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWAPM_ActivityRelationTables 
    {
        public Guid id { get; set;}
        public string dataTable { get; set;}
        public string description { get; set;}
        public string color { get; set;}
        public string Name { get; set;}
    }
}
