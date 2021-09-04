using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_StockTaskPlan : InfolineTable
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
    }
}
