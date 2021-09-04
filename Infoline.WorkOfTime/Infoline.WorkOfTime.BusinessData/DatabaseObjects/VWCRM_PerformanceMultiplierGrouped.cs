using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_PerformanceMultiplierGrouped : InfolineTable
    {
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
    }
}
