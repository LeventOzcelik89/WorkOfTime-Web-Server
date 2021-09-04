using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CompanyPerson : InfolineTable
    {
        public Guid? CompanyId { get; set;}
        public Guid? IdUser { get; set;}
        public DateTime? JobStartDate { get; set;}
        public DateTime? JobEndDate { get; set;}
        public int? JobLeaving { get; set;}
        public string JobLeavingDescription { get; set;}
        public string Title { get; set;}
        public int? Level { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Person_Title { get; set;}
        public string Company_Title { get; set;}
        public string JobLeaving_Title { get; set;}
        public int? PersonWorkingCount { get; set;}
    }
}
