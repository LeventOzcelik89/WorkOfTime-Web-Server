using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class HR_StaffStatus : InfolineTable
    {
        public Guid? HrPersonId { get; set;}
        public Guid? HrStaffNeedsId { get; set;}
        public int? Status { get; set;}
    }
}
