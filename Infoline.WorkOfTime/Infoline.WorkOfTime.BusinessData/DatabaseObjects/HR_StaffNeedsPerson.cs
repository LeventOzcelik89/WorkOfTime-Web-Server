using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class HR_StaffNeedsPerson : InfolineTable
    {
        public Guid? HrStaffNeedsId { get; set;}
        public Guid? HrPersonId { get; set;}
        public int? status { get; set;}
        public string Description { get; set;}
        public int? SalaryRangeMin { get; set;}
        public int? SalaryRangeMax { get; set;}
    }
}
