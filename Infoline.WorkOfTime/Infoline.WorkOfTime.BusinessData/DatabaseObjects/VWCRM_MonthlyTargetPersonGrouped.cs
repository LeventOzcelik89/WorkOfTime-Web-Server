using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_MonthlyTargetPersonGrouped : InfolineTable
    {
        public Guid? TargetUserId { get; set;}
        public DateTime? CPeriod { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string TargetUser_Title { get; set;}
    }
}
