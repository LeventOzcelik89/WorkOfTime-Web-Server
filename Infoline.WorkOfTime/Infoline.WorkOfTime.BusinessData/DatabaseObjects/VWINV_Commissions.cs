using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_Commissions : InfolineTable
    {
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public int? Information { get; set;}
        public string InformationDetail { get; set;}
        public int? TravelInformation { get; set;}
        public string TravelInformationDetail { get; set;}
        public string Descriptions { get; set;}
        public DateTime? Manager1ApprovalDate { get; set;}
        public Guid? Manager1Approval { get; set;}
        public int? ApproveStatus { get; set;}
        public string CommissionCode { get; set;}
        public string ToCorporation { get; set;}
        public string ToAdress { get; set;}
        public string CommissionSubject { get; set;}
        public double? TotalDays { get; set;}
        public double? TotalHours { get; set;}
        public int? RequestForAccommodation { get; set;}
        public Guid? IdCompanyCar { get; set;}
        public string VehiclePlate { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Information_Title { get; set;}
        public string TravelInformation_Title { get; set;}
        public string ApproveStatus_Title { get; set;}
        public string Manager1Approval_Title { get; set;}
        public string Car_Title { get; set;}
        public string TotalHoursText { get; set;}
    }
}
