using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCSM_School : InfolineTable
    {
        public string name { get; set;}
        public Guid? locationId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string locationId_Title { get; set;}
    }
}
