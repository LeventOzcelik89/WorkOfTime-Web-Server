using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPDS_FormPermitTaskUser : InfolineTable
    {
        public int status { get; set;}
        public string formName { get; set;}
        public string formCode { get; set;}
        public int? formType { get; set;}
        public string formType_Title { get; set;}
        public string ProfilePhoto { get; set;}
        public int? evaluateCount { get; set;}
        public DateTime? lastEvaluateDate { get; set;}
        public Guid? lastId { get; set;}
        public Guid? formPermitTaskId { get; set;}
        public Guid? evaluatorId { get; set;}
        public Guid? evaluatedUserId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string evaluator_Title { get; set;}
        public string evaluatedUser_Title { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public Guid? evaluateFormId { get; set;}
    }
}
