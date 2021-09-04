using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_PerformanceMultiplier : InfolineTable
    {
        public Guid? GroupId { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public int? TargetGroupType { get; set;}
        public Guid? ProductGroupId { get; set;}
        public int? MinPercentage { get; set;}
        public int? MaxPercentage { get; set;}
        public int? Point { get; set;}
        public bool? IsFocus { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string TargetGroupType_Title { get; set;}
        public string ProductGroup_Title { get; set;}
    }
}
