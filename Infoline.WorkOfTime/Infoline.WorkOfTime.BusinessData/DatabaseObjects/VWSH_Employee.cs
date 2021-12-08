using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_Employee : InfolineTable
    {
        public bool? status { get; set;}
        public int? type { get; set;}
        public string code { get; set;}
        public string loginname { get; set;}
        public string firstname { get; set;}
        public string lastname { get; set;}
        public DateTime? birthday { get; set;}
        public string password { get; set;}
        public string title { get; set;}
        public string email { get; set;}
        public string phone { get; set;}
        public string cellphone { get; set;}
        public string address { get; set;}
        public Guid? locationId { get; set;}
        public string companyCellPhone { get; set;}
        public string companyCellPhoneCode { get; set;}
        public string companyOfficePhone { get; set;}
        public string companyOfficePhoneCode { get; set;}
        public string fullName { get; set;}
        public string nationality { get; set;}
        public string City_Title { get; set;}
        public string Town_Title { get; set;}
        public string BornLocation_Title { get; set;}
        public string FatherName { get; set;}
        public string MotherName { get; set;}
        public string IdentificationNumber { get; set;}
        public string EmergencyPerson { get; set;}
        public string EmergencyPhone { get; set;}
        public string Gender_Title { get; set;}
        public string MaritalStatus_Title { get; set;}
        public string Military_Title { get; set;}
        public DateTime? Military_DoneDate { get; set;}
        public DateTime? Probation_Date { get; set;}
        public string ProbationDetail { get; set;}
        public string IDBloodGroup_Title { get; set;}
        public string personLanguages { get; set;}
        public string personCertificates { get; set;}
        public string personGroups { get; set;}
        public string companyName { get; set;}
        public string deparmanTitle { get; set;}
        public DateTime? jobStartDate { get; set;}
        public string PersonWorking { get; set;}
        public DateTime? jobEndDate { get; set;}
        public string jobLeavingDescription { get; set;}
        public Guid? manager1 { get; set;}
        public Guid? manager2 { get; set;}
        public Guid? manager3 { get; set;}
        public Guid? manager4 { get; set;}
        public Guid? manager5 { get; set;}
        public Guid? manager6 { get; set;}
        public double? salary { get; set;}
        public string workExperiences { get; set;}
        public string gratutedSchooles { get; set;}
        public string lastGratutedSchool { get; set;}
        public DateTime? lastGratutedDate { get; set;}
        public string primarySchool { get; set;}
        public string highSchool { get; set;}
        public string university { get; set;}
    }
}
