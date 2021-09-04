using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPDS_FormResult : InfolineTable
    {
        public string comment { get; set;}
        public int? status { get; set;}
        public Guid? formPermitTaskUserId { get; set;}
        public Guid? evaluateFormId { get; set;}
        public Guid? evaluatorId { get; set;}
        public Guid? evaluatedUserId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string status_Title { get; set;}
        public string evaluator_Title { get; set;}
        public string evaluatedUser_Title { get; set;}
        public string formName { get; set;}
        public string formCode { get; set;}
        public int? formType { get; set;}
        public string formType_Title { get; set;}
        public double? score { get; set;}
    }
}
