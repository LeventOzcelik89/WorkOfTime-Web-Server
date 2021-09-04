using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_CompanyCars : InfolineTable
    {
        public Guid? companyId { get; set;}
        public Guid? companyStorageId { get; set;}
        public string name { get; set;}
        public string plate { get; set;}
        public string brand { get; set;}
        public string model { get; set;}
        public bool? isHire { get; set;}
        public DateTime? contractStartDate { get; set;}
        public DateTime? contractEndDate { get; set;}
        public string vehicleIdentificationNumber { get; set;}
        public string vehicleTransitionNumber { get; set;}
        public double? consumptionFuel { get; set;}
        public DateTime? insuranceStartDate { get; set;}
        public DateTime? insuranceEndDate { get; set;}
        public DateTime? trafficInsuranceStartDate { get; set;}
        public DateTime? trafficInsuranceEndDate { get; set;}
        public string tradeName { get; set;}
        public string color { get; set;}
        public string engineNumber { get; set;}
        public string chassisNumber { get; set;}
        public DateTime? trafficReleaseDate { get; set;}
        public DateTime? registrationDate { get; set;}
        public string licenseSeries { get; set;}
        public string userName { get; set;}
        public int? fuelType { get; set;}
        public int? gearType { get; set;}
        public string policyNumber { get; set;}
        public Guid? responsiblePersonId { get; set;}
        public string createdby_Title { get; set;}
        public string responsiblePersonId_Title { get; set;}
        public string changedby_Title { get; set;}
        public string companyId_Title { get; set;}
        public string companyStorageId_Title { get; set;}
        public double? lastKilometer { get; set;}
        public string isHire_Title { get; set;}
        public string fuelType_Title { get; set;}
    }
}
