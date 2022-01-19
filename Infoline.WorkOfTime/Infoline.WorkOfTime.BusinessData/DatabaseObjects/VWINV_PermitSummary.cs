using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_PermitSummary : InfolineTable
    {
        public double dayCountDeserved { get; set;}
        public int PermitYearlyUsed { get; set;}
        public double? dayCountUsable { get; set;}
        public string Name { get; set;}
        public bool? IsPaidPermit { get; set;}
        public int? PaidPermitDay { get; set;}
        public bool? CanHourly { get; set;}
        public string Descriptions { get; set;}
        public bool? BeDocumented { get; set;}
        public bool? RequestStaff { get; set;}
        public bool? ViewStaff { get; set;}
        public string IsPaidPermit_Title { get; set;}
        public string CanHourly_Title { get; set;}
        public string BeDocumented_Title { get; set;}
        public Guid? userId { get; set;}
        public string userId_Title { get; set;}
        public Guid? companyId { get; set;}
        public string companyId_Title { get; set;}
        public Guid? DepartmentId { get; set;}
        public string departmentId_Title { get; set;}
        public DateTime? employeeJobStartDate { get; set;}
        public DateTime? employeeDateOfBirth { get; set;}
        public bool? userStatus { get; set;}
        public string Type_Title { get; set;}
        public string Status_Title { get; set;}
        public double? dayCountUsed { get; set;}
        public double? dayCountUsedPending { get; set;}
    }
}
