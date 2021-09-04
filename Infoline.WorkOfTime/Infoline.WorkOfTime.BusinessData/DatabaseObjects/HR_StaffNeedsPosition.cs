using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class HR_StaffNeedsPosition : InfolineTable
    {
        public Guid? HrPositionId { get; set;}
        public Guid? HrStaffNeedsId { get; set;}
    }
}
