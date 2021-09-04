using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_Permit : InfolineTable
    {
        public string PermitCode { get; set;}
        public int? ApproveStatus { get; set;}
        public string ApproveDetail { get; set;}
        public Guid? PermitTypeId { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public string AccessPhone { get; set;}
        public string ArriveAdress { get; set;}
        public string Detail { get; set;}
        public DateTime? Manager1ApprovalDate { get; set;}
        public DateTime? Manager2ApprovalDate { get; set;}
        public Guid? Manager1Approval { get; set;}
        public Guid? Manager2Approval { get; set;}
        public DateTime? IkApprovalDate { get; set;}
        public Guid? IkApproval { get; set;}
        public double? TotalDays { get; set;}
        public Guid? IdUser { get; set;}
        public double? TotalHours { get; set;}
        public string PersonToLook1 { get; set;}
        public string PersonToLook2 { get; set;}
        public string PersonToLook3 { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string ApproveStatus_Title { get; set;}
        public string PermitType_Title { get; set;}
        public string Person_Title { get; set;}
        public string Manager1Approval_Title { get; set;}
        public string Manager2Approval_Title { get; set;}
        public string IkApproval_Title { get; set;}
        public string TotalHoursText { get; set;}
        public string TcNo { get; set;}
        public string Company_Title { get; set;}
        public string Files { get; set;}
        public string searchField { get; set;}
    }
}
