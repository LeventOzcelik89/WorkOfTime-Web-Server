using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_PersonInformation : InfolineTable
    {
        public Guid? UserId { get; set;}
        public string Nationality { get; set;}
        public int? Gender { get; set;}
        public int? MaritalStatus { get; set;}
        public int? Military { get; set;}
        public DateTime? MilitaryDoneDate { get; set;}
        public int? MilitaryDoneDuration { get; set;}
        public string MilitaryExemptDetail { get; set;}
        public string MilitaryProbationDetail { get; set;}
        public DateTime? MilitaryProbationDate { get; set;}
        public string IDSerialNumber { get; set;}
        public string IDMotherName { get; set;}
        public string IDFatherName { get; set;}
        public Guid? IDBornLocation { get; set;}
        public int? IDBloodGroup { get; set;}
        public string IDPreviousSurname { get; set;}
        public Guid? IDCity { get; set;}
        public Guid? IDTown { get; set;}
        public string IDDistrict { get; set;}
        public string IDVillage { get; set;}
        public string IDDeliveryLocation { get; set;}
        public string IDDeliveryDetail { get; set;}
        public string IDRecordNumber { get; set;}
        public DateTime? IDDeliveryDate { get; set;}
        public string IDVolumeNumber { get; set;}
        public string IDFamilyNumber { get; set;}
        public string IDRowNumber { get; set;}
        public string PersonalMail { get; set;}
        public string PersonalHomePhone { get; set;}
        public string EmergencyName { get; set;}
        public string EmergencyPhone { get; set;}
        public string EmergencyMail { get; set;}
        public string EmergencyProximity { get; set;}
        public string Religious { get; set;}
        public string IDBornTownLocation { get; set;}
        public string InsuranceIdentityNumber { get; set;}
        public string IdentificationNumber { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string IDCity_Title { get; set;}
        public string IDTown_Title { get; set;}
        public string IDBornLocation_Title { get; set;}
        public string Gender_Title { get; set;}
        public string MaritalStatus_Title { get; set;}
        public string Military_Title { get; set;}
        public string IDBloodGroup_Title { get; set;}
        public DateTime? JobEndDate { get; set;}
        public string User_Title { get; set;}
    }
}
