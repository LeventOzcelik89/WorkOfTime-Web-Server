using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_MonthlyTarget : InfolineTable
    {
        public DateTime? CPeriod { get; set;}
        public int? TargetGroupType { get; set;}
        public Guid? ProductGroupId { get; set;}
        public int? TargetPoint { get; set;}
        public int? TargetPercent { get; set;}
        public int? BonusPercentage { get; set;}
        public bool? IsFocus { get; set;}
        public bool? IsMultipleFocus { get; set;}
        public Guid? RowId { get; set;}
        public Guid? GroupId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string ProductGroup_Title { get; set;}
    }
}
