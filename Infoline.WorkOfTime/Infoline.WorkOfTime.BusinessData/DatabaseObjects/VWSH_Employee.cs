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
        public string activityName { get; set;}
        public string activityLastStatus { get; set;}
        public string companyName { get; set;}
        public string companyCode { get; set;}
        public string carPlates { get; set;}
        public string lastCompanyApporevedRequest { get; set;}
        public string lastCompanyApporevedOrder { get; set;}
    }
}
