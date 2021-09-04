using System;
using GeoAPI.Geometries;

namespace Infoline.ProjectManagement.BusinessData
{
    public partial class VWPRD_StockActionSummary : InfolineTable
    {
        public short? type { get; set;}
        public Guid? productId { get; set;}
        public Guid? stockId { get; set;}
        public string description { get; set;}
        public string table { get; set;}
        public Guid? dataId { get; set;}
        public int? quantity { get; set;}
        public double? unitPrice { get; set;}
        public int? quantityTotal { get; set;}
        public long? row { get; set;}
        public Guid? storageId { get; set;}
        public Guid? storageSectionIdId { get; set;}
        public Guid? invoiceId { get; set;}
    }
}
