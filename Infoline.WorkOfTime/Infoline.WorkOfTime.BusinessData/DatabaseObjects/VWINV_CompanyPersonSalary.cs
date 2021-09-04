using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CompanyPersonSalary : InfolineTable
    {
        public int isActive { get; set;}
        public Guid? IdUser { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public double? Salary { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Person_Title { get; set;}
    }
}
