using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_ContactRelationTables 
    {
        public string dataTable { get; set;}
        public string description { get; set;}
        public string color { get; set;}
        public Guid id { get; set;}
        public string Name { get; set;}
        public string ManagingUserIds { get; set;}
    }
}
