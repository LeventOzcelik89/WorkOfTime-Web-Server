using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_StockSummary 
    {
        public Guid? id { get; set;}
        public DateTime? created { get; set;}
        public double? quantity { get; set;}
        public double? alternativeQuantity { get; set;}
        public short? status { get; set;}
        public Guid? stockId { get; set;}
        public string stockTable { get; set;}
        public Guid? stockCompanyId { get; set;}
        public Guid? tritemUnitId { get; set;}
        public Guid? alternativeUnitId { get; set;}
        public string stockId_Title { get; set;}
        public string productName { get; set;}
        public short? productType { get; set;}
        public string productCode { get; set;}
        public string stockCompanyId_Title { get; set;}
        public Guid? productId { get; set;}
        public string productId_Title { get; set;}
        public string productId_Image { get; set;}
        public string unitId_Title { get; set;}
        public string alternativeUnitId_Title { get; set;}
        public short? stockType { get; set;}
        public string categoryId_Title { get; set;}
        public double? productPrice { get; set;}
        public double? servicePrice { get; set;}
        public string searchField { get; set;}
    }
}
