using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CompanyCars : InfolineTable
    {
        public string Name { get; set;}
        public string Plate { get; set;}
        public string Brand { get; set;}
        public string Model { get; set;}
        public bool? IsHire { get; set;}
        public DateTime? ContractStartDate { get; set;}
        public DateTime? ContractEndDate { get; set;}
        public string VehicleIdentificationNumber { get; set;}
        public string VehicleTransitionNumber { get; set;}
        public Guid? CompanyId { get; set;}
        public double? ConsumptionFuel { get; set;}
        public DateTime? InsuranceStartDate { get; set;}
        public DateTime? InsuranceEndDate { get; set;}
        public DateTime? TrafficInsuranceStartDate { get; set;}
        public DateTime? TrafficInsuranceEndDate { get; set;}
        public string TradeName { get; set;}
        public string Color { get; set;}
        public string EngineNumber { get; set;}
        public string ChassisNumber { get; set;}
        public DateTime? TrafficReleaseDate { get; set;}
        public DateTime? RegistrationDate { get; set;}
        public string LicenseSeries { get; set;}
        public string LocationId { get; set;}
        public string UserName { get; set;}
        public DateTime? ExaminationDate { get; set;}
        public int? FuelType { get; set;}
        public int? GearType { get; set;}
        public string PolicyNumber { get; set;}
        public string KaskoPolicyNumber { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Company_Title { get; set;}
        public string LocationId_Title { get; set;}
        public string UserIdTitle { get; set;}
    }
}
