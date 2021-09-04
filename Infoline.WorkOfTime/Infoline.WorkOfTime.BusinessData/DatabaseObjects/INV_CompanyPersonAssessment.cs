using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_CompanyPersonAssessment : InfolineTable
    {
        public string AssessmentCode { get; set;}
        public int? ApproveStatus { get; set;}
        public DateTime? AssessmentDate { get; set;}
        public short? AssessmentType { get; set;}
        public DateTime? Manager1ApprovalDate { get; set;}
        public DateTime? Manager2ApprovalDate { get; set;}
        public string Manager1ApprovalComment { get; set;}
        public string Manager2ApprovalComment { get; set;}
        public Guid? Manager1Approval { get; set;}
        public Guid? Manager2Approval { get; set;}
        public DateTime? IkApprovalDate { get; set;}
        public Guid? IkApproval { get; set;}
        public string IKApprovalComment { get; set;}
        public Guid? IdUser { get; set;}
        public Guid? GeneralManagerApproval { get; set;}
        public string GeneralManagerApprovalComment { get; set;}
        public DateTime? GeneralManagerApprovalDate { get; set;}
        public int? ConfirmStatus { get; set;}
    }
}
