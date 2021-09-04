using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHR_StaffNeedsPerson : InfolineTable
    {
        public Guid? HrStaffNeedsId { get; set;}
        public Guid? HrPersonId { get; set;}
        public int? status { get; set;}
        public string Description { get; set;}
        public int? SalaryRangeMin { get; set;}
        public int? SalaryRangeMax { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string HrPersonId_Title { get; set;}
        public string NeedCode { get; set;}
        public string status_Title { get; set;}
    }
}
