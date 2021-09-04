using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHR_PlanPerson : InfolineTable
    {
        public Guid? HrPlanId { get; set;}
        public Guid? UserId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string UserId_Title { get; set;}
        public string UserId_Titles { get; set;}
        public string HRPerson_Title { get; set;}
        public Guid? HrPersonId { get; set;}
        public DateTime? PlanDate { get; set;}
        public int? Result { get; set;}
        public string Description { get; set;}
        public string Result_Title { get; set;}
    }
}
