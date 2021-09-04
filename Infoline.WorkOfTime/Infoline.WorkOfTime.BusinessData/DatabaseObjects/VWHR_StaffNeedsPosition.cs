using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHR_StaffNeedsPosition : InfolineTable
    {
        public Guid? HrPositionId { get; set;}
        public Guid? HrStaffNeedsId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string HrPositionId_Title { get; set;}
    }
}
