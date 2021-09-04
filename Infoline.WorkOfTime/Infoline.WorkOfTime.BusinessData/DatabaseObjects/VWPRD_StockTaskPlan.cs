using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_StockTaskPlan : InfolineTable
    {
        public Guid? inventoryId { get; set;}
        public Guid? taskId { get; set;}
        public short? status { get; set;}
        public Guid? storageId { get; set;}
        public string code { get; set;}
        public string inventoryIndexValue { get; set;}
        public int? inventoryStampYear { get; set;}
        public string newInventoryBrand { get; set;}
        public string newInventoryIndexValue { get; set;}
        public Guid? newInventoryId { get; set;}
        public int? newInventoryStampYear { get; set;}
        public string inventorySerialNumber { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string status_Title { get; set;}
        public string storageId_Title { get; set;}
        public string storageCode { get; set;}
        public string storageAddress { get; set;}
        public Guid? companyId { get; set;}
        public string locationId_Title { get; set;}
        public string serialNumber { get; set;}
        public string product_Title { get; set;}
        public string lastOperationStatus_Title { get; set;}
    }
}
