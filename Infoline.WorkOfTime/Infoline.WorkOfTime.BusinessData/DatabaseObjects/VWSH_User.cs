using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_User : InfolineTable
    {
        public string FullName { get; set;}
        public int PermitYearlyDeserved { get; set;}
        public int PermitYearlyUsed { get; set;}
        public double PermitExcuseDeserved { get; set;}
        public double PermitExcuseUsed { get; set;}
        public int PermitYearlyUsable { get; set;}
        public double PermitExcuseUsable { get; set;}
        public string searchField { get; set;}
        public int? PersonWorkingCount { get; set;}
        public string PersonWorking { get; set;}
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
        public string IdentificationNumber { get; set;}
        public Guid? CompanyId { get; set;}
        public string CompanyLogo { get; set;}
        public string Company_Title { get; set;}
        public Guid? CompanyPersonId { get; set;}
        public string CompanyPerson_Title { get; set;}
        public int? CompanyPerson_Level { get; set;}
        public string JobLeavingDescription { get; set;}
        public int? JobLeaving { get; set;}
        public string JobLeaving_Title { get; set;}
        public DateTime? JobEndDate { get; set;}
        public DateTime? JobStartDate { get; set;}
        public bool? IsWorking { get; set;}
        public Guid? Manager1 { get; set;}
        public string Manager1_Title { get; set;}
        public Guid? Manager2 { get; set;}
        public string Manager2_Title { get; set;}
        public Guid? Manager3 { get; set;}
        public string Manager3_Title { get; set;}
        public Guid? Manager4 { get; set;}
        public string Manager4_Title { get; set;}
        public Guid? Manager5 { get; set;}
        public string Manager5_Title { get; set;}
        public Guid? Manager6 { get; set;}
        public string Manager6_Title { get; set;}
        public Guid? DepartmentId { get; set;}
        public Guid? CompanyPersonDepartmentId { get; set;}
        public string companyId_Code { get; set;}
        public string Department_Title { get; set;}
        public string Type_Title { get; set;}
        public string Status_Title { get; set;}
        public string locationId_Title { get; set;}
        public string RoleIds { get; set;}
        public string ProfilePhoto { get; set;}
        public string SchoolLevel_Title { get; set;}
        public string Gender_Title { get; set;}
        public string IBAN { get; set;}
    }
}
