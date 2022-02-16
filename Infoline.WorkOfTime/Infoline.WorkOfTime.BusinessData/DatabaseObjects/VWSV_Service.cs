using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSV_Service : InfolineTable
    {
        public string code { get; set;}
        public Guid? inventoryId { get; set;}
        public short? stage { get; set;}
        public short? deliveryType { get; set;}
        public string deliveryTypeDescription { get; set;}
        public Guid? cargoId { get; set;}
        public string cargoNo { get; set;}
        public Guid? companyId { get; set;}
        public short? customerType { get; set;}
        public string customerTypeDescription { get; set;}
        public Guid? storageId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string inventoryCode_Title { get; set;}
        public string cargoId_Title { get; set;}
        public string companyId_Title { get; set;}
        public string lastOperationStatus_Title { get; set;}
        public string delivery_Title { get; set;}
        public string customerType_Title { get; set;}
        public string stage_Title { get; set;}
        public string serialCode { get; set;}
        public string product_Title { get; set;}
        public Guid? productId { get; set;}
        public string storageId_Title { get; set;}
        public string customerFullName { get; set;}
        public string Imei { get; set;}
        public string customerPhoneNumber { get; set;}
        public short? lastOperationStatus { get; set;}
    }
}
