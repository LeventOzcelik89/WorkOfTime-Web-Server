using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_EntegrationAction : InfolineTable
    {
        public Guid? EntegrationFileId { get; set;}
        public Guid? DistributorId { get; set;}
        public string DistributorName { get; set;}
        public string InvoiceNumber { get; set;}
        public string CustomerOperatorCode { get; set;}
        public string CustomerOperatorName { get; set;}
        public Guid? CustomerOperatorId { get; set;}
        public string CustomerOperatorStorageCode { get; set;}
        public Guid? CustomerOperatorStorageId { get; set;}
        public string CustomerOperatorStorageCity { get; set;}
        public string CustomerOperatorStorageTown { get; set;}
        public string BranchName { get; set;}
        public string BranchCode { get; set;}
        public string TaxNumber { get; set;}
        public string ConsolidationCode { get; set;}
        public string ConsolidationName { get; set;}
        public Guid? ProductId { get; set;}
        public Guid? InventoryId { get; set;}
        public string Imei { get; set;}
        public string SerialNo { get; set;}
        public int? Quantity { get; set;}
        public DateTime? Invoice_Address { get; set;}
        public string Invoice_No { get; set;}
        public string Company_Code { get; set;}
        public string CompanyName { get; set;}
        public string county { get; set;}
        public string town { get; set;}
        public string DeviceCode { get; set;}
        public string DeviceName { get; set;}
        public DateTime? ActivationDate { get; set;}
        public int? Count { get; set;}
    }
}
