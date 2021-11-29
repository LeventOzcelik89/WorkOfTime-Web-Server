using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_EntegrationImport : InfolineTable
    {
        public string customerName { get; set;}
        public string customerCode { get; set;}
        public string productModel { get; set;}
        public string distributorName { get; set;}
        public DateTime? distributorConfirmationDate { get; set;}
        public string imei { get; set;}
        public string customerType { get; set;}
        public DateTime? contractStartDate { get; set;}
        public DateTime? contractEndDate { get; set;}
        public string contractCode { get; set;}
        public string productGroup { get; set;}
        public string sellingChannelType { get; set;}
        public string distributorCode { get; set;}
        public int? sellingQuantity { get; set;}
        public int? month { get; set;}
        public int? year { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public Guid? companyId_Title { get; set;}
        public Guid? inventoryId_Title { get; set;}
        public Guid? distributor_Title { get; set;}
    }
}
