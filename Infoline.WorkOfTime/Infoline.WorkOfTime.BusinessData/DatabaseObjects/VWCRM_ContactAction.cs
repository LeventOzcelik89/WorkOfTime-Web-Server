using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_ContactAction : InfolineTable
    {
        public Guid? ContactId { get; set;}
        public string description { get; set;}
        public IGeometry  location { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
