using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CommissionsProjects : InfolineTable
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
        public double? VehicleKilometer { get; set;}
        public string Information_Title { get; set;}
        public string TravelInformation_Title { get; set;}
        public Guid? IdCommissions { get; set;}
        public Guid? IdProject { get; set;}
        public double? Percentile { get; set;}
        public string Project_Title { get; set;}
        public Guid? IdUser { get; set;}
        public string Person_Title { get; set;}
    }
}
