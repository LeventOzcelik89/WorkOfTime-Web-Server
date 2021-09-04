using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHR_StaffStatus : InfolineTable
    {
        public Guid? HrPersonId { get; set;}
        public Guid? HrStaffNeedsId { get; set;}
        public int? Status { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string HrPersonId_Title { get; set;}
        public string HrStaffNeedsId_Title { get; set;}
    }
}
