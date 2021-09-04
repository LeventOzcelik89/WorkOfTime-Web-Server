using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_MonthlyTargetGrouped : InfolineTable
    {
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public DateTime? CPeriod { get; set;}
    }
}
