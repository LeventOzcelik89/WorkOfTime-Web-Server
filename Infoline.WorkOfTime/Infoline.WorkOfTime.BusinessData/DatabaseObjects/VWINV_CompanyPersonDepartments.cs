using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CompanyPersonDepartments : InfolineTable
    {
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public string Title { get; set;}
        public Guid? DepartmentId { get; set;}
        public bool? IsBasePosition { get; set;}
        public Guid? IdUser { get; set;}
        public Guid? Manager1 { get; set;}
        public Guid? Manager2 { get; set;}
        public Guid? Manager3 { get; set;}
        public Guid? Manager4 { get; set;}
        public Guid? Manager5 { get; set;}
        public Guid? Manager6 { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Person_Title { get; set;}
        public string Manager1_Title { get; set;}
        public string Manager2_Title { get; set;}
        public string Manager3_Title { get; set;}
        public Guid? ProjectId { get; set;}
        public string Project_Title { get; set;}
        public string Department_Title { get; set;}
        public Guid? Department_PID { get; set;}
        public string PID_Title { get; set;}
        public int? OrganizationType { get; set;}
        public int? DayArea { get; set;}
    }
}
