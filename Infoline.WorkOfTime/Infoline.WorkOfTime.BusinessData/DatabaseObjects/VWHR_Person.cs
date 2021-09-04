using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHR_Person : InfolineTable
    {
        public string FullName { get; set;}
        public string searchField { get; set;}
        public string Name { get; set;}
        public string SurName { get; set;}
        public string IdentifyNumber { get; set;}
        public string Phone { get; set;}
        public string Email { get; set;}
        public Guid? LocationId { get; set;}
        public DateTime? Birthday { get; set;}
        public int? ArrivalType { get; set;}
        public Guid? ReferenceId { get; set;}
        public int? Education { get; set;}
        public int? MilitaryStatus { get; set;}
        public int? Language { get; set;}
        public int? LanguageType { get; set;}
        public int? DriverLicense { get; set;}
        public int? ExprienceYear { get; set;}
        public string Description { get; set;}
        public DateTime? RetirementDate { get; set;}
        public string Adress { get; set;}
        public string SchoolName { get; set;}
        public string Branch { get; set;}
        public int? SalaryRangeMin { get; set;}
        public int? SalaryRangeMax { get; set;}
        public string MilitaryExemptDetail { get; set;}
        public int? Gender { get; set;}
        public DateTime? MilitaryDoneDate { get; set;}
        public int? MaritalStatus { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string ReferenceId_Title { get; set;}
        public string LocationId_Title { get; set;}
        public string ArrivalType_Title { get; set;}
        public string Education_Title { get; set;}
        public string DriverLicense_Title { get; set;}
        public string MilitaryStatus_Title { get; set;}
        public string Language_Title { get; set;}
        public string LanguageLevel_Title { get; set;}
        public string Gender_Title { get; set;}
        public string MaritalStatus_Title { get; set;}
        public int? Result { get; set;}
        public string Result_Title { get; set;}
        public string Result_Titles { get; set;}
        public string HrPosition_Title { get; set;}
        public string HrKeywords_Title { get; set;}
        public string ProfilePhoto { get; set;}
    }
}
