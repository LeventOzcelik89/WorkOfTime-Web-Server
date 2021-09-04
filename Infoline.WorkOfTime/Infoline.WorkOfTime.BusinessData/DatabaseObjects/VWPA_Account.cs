using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPA_Account : InfolineTable
    {
        public bool myAccount { get; set;}
        public double balance { get; set;}
        public double balanceTL { get; set;}
        public string account_Title { get; set;}
        public string created_Title { get; set;}
        public string searchField { get; set;}
        public string companyId_Image { get; set;}
        public string Currency_Code { get; set;}
        public string Currency_Symbol { get; set;}
        public string name { get; set;}
        public string code { get; set;}
        public short? type { get; set;}
        public Guid? currencyId { get; set;}
        public Guid? bankId { get; set;}
        public string iban { get; set;}
        public Guid? dataId { get; set;}
        public string dataTable { get; set;}
        public bool? status { get; set;}
        public bool? isDefault { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string type_Title { get; set;}
        public string currencyId_Title { get; set;}
        public string bankId_Title { get; set;}
        public string status_Title { get; set;}
        public string dataId_Title { get; set;}
    }
}
