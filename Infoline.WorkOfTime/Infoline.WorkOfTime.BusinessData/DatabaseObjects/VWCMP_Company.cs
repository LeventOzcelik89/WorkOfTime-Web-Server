using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_Company : InfolineTable
    {
        public int? type { get; set;}
        public Guid? pid { get; set;}
        public string code { get; set;}
        public string name { get; set;}
        public string mersisNo { get; set;}
        public string email { get; set;}
        public string phone { get; set;}
        public string fax { get; set;}
        public short? taxType { get; set;}
        public string taxOffice { get; set;}
        public string taxNumber { get; set;}
        public IGeometry  location { get; set;}
        public string invoiceAddress { get; set;}
        public Guid? invoiceAddressLocationId { get; set;}
        public string openAddress { get; set;}
        public Guid? openAddressLocationId { get; set;}
        public string commercialTitle { get; set;}
        public short? isActive { get; set;}
        public string description { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string type_Title { get; set;}
        public string pid_Title { get; set;}
        public string taxType_Title { get; set;}
        public string isActive_Title { get; set;}
        public string invoiceAddressLocationId_Title { get; set;}
        public string openAddressLocationId_Title { get; set;}
        public string fullName { get; set;}
        public string logo { get; set;}
        public string Sectors { get; set;}
        public string CMPTypes_Title { get; set;}
        public string customerIds { get; set;}
        public string searchField { get; set;}
    }
}
