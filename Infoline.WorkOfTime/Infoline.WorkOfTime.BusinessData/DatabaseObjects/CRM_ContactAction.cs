using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CRM_ContactAction : InfolineTable
    {
        public Guid? ContactId { get; set;}
        public string description { get; set;}
        public IGeometry  location { get; set;}
    }
}
