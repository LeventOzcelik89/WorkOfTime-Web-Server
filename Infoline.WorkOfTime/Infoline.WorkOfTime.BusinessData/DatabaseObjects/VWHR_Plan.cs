using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHR_Plan : InfolineTable
    {
        public string InterviewUser_Title { get; set;}
        public Guid? HrPersonId { get; set;}
        public DateTime? PlanDate { get; set;}
        public int? Result { get; set;}
        public string Description { get; set;}
        public Guid? PdsEvulateFormId { get; set;}
        public Guid? PdsEvulateFormResultId { get; set;}
        public bool? mailSend { get; set;}
        public Guid? CompanyId { get; set;}
        public Guid? StaffNeedsId { get; set;}
        public string contactLink { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string HrPersonId_Title { get; set;}
        public string Result_Title { get; set;}
        public string FormName { get; set;}
        public string Evaluator_Title { get; set;}
        public double? score { get; set;}
        public string Company_Title { get; set;}
        public string UserId_Titles { get; set;}
    }
}
